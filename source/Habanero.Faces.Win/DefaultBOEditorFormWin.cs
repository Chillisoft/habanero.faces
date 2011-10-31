#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using System.Drawing;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using log4net;
using FormStartPosition=System.Windows.Forms.FormStartPosition;

namespace Habanero.Faces.Win
{
    /// <summary>
    /// Provides a form used to edit business objects.  This form will usually
    /// be constructed using a UI Form definition provided in the class definitions.
    /// The appropriate UI definition is typically set in the constructor.
    /// </summary>
    public class DefaultBOEditorFormWin : FormWin, IDefaultBOEditorForm
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.Faces.Win.DefaultBOEditorFormWin");
        private readonly BusinessObject _bo;
        private readonly IControlFactory _controlFactory;
        private readonly string _uiDefName;
        private readonly IPanelInfo _panelInfo;
        private readonly IPanel _boPanel;
        private readonly IButtonGroupControl _buttons;
        private readonly PostObjectEditDelegate _postObjectEditAction;
        
        /// <summary>
        /// Constructs the <see cref="DefaultBOEditorFormWin"/> class  with 
        /// the specified <see cref="BusinessObject"/>, uiDefName, <see cref="IControlFactory"/> and post edit action. 
        /// </summary>
        /// <param name="bo">The business object to represent</param>
        /// <param name="uiDefName">The name of the ui def to use.</param>
        /// <param name="controlFactory">The <see cref="IControlFactory"/> to use for creating the Editor form controls</param>
        /// <param name="action">Action to be performed when the editing is completed or cancelled. Typically used if you want to update
        /// a grid or a list in an asynchronous environment (E.g. to select the recently edited item in the grid).</param>
        public DefaultBOEditorFormWin(BusinessObject bo, string uiDefName, IControlFactory controlFactory, PostObjectEditDelegate action)
            : this(bo, uiDefName, controlFactory)
        {
            _postObjectEditAction = action;
        }

        /// <summary>
        /// Constructs the <see cref="DefaultBOEditorFormWin"/> class  with 
        /// the specified businessObject, uiDefName and post edit action. 
        /// </summary>
        /// <param name="bo">The business object to represent</param>
        /// <param name="uiDefName">The name of the ui def to use.</param>
        /// <param name="controlFactory">The <see cref="IControlFactory"/> to use for creating the Editor form controls</param>
        /// <param name="creator">The Creator used to Create the Group Control.</param>
        public DefaultBOEditorFormWin(BusinessObject bo, string uiDefName, IControlFactory controlFactory, GroupControlCreator creator)
        {
            _bo = bo;
            _controlFactory = controlFactory;
            GroupControlCreator = creator;
            _uiDefName = uiDefName;

            BOMapper mapper = new BOMapper(bo);

            IUIForm def;
            if (_uiDefName.Length > 0)
            {
                IUIDef uiMapper = mapper.GetUIDef(_uiDefName);
                if (uiMapper == null)
                {
                    throw new NullReferenceException("An error occurred while " +
                                                     "attempting to load an object editing form.  A possible " +
                                                     "cause is that the class definitions do not have a " +
                                                     "'form' section for the class, under the 'ui' " +
                                                     "with the name '" + _uiDefName + "'.");
                }
                def = uiMapper.UIForm;
            }
            else
            {
                IUIDef uiMapper = mapper.GetUIDef();
                if (uiMapper == null)
                {
                    throw new NullReferenceException("An error occurred while " +
                                                     "attempting to load an object editing form.  A possible " +
                                                     "cause is that the class definitions do not have a " +
                                                     "'form' section for the class.");
                }
                def = uiMapper.UIForm;
            }
            if (def == null)
            {
                throw new NullReferenceException("An error occurred while " +
                                                 "attempting to load an object editing form.  A possible " +
                                                 "cause is that the class definitions do not have a " +
                                                 "'form' section for the class.");
            }

            PanelBuilder panelBuilder = new PanelBuilder(_controlFactory);
            //_panelInfo = panelBuilder.BuildPanelForForm(_bo.ClassDef.UIDefCol["default"].UIForm);
            //_panelInfo = panelBuilder.BuildPanelForForm(_bo.ClassDef.UIDefCol[uiDefName].UIForm, this.GroupControlCreator);
            _panelInfo = panelBuilder.BuildPanelForForm(def, this.GroupControlCreator);

            _panelInfo.BusinessObject = _bo;
            _boPanel = _panelInfo.Panel;

            _buttons = _controlFactory.CreateButtonGroupControl();
            _buttons.AddButton("Cancel", CancelButtonHandler);
            IButton okbutton = _buttons.AddButton("OK", OkButtonHandler);

            okbutton.NotifyDefault(true);
            this.AcceptButton = (ButtonWin) okbutton;
            this.Load += delegate { FocusOnFirstControl(); };
            this.FormClosing += OnFormClosing;

            this.Text = def.Title;
            SetupFormSize(def);
            MinimizeBox = false;
            MaximizeBox = false;
            //this.ControlBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            CreateLayout();
            OnResize(new EventArgs());
        }

        /// <summary>
        /// Constructs the <see cref="DefaultBOEditorFormWin"/> class  with 
        /// the specified <see cref="BusinessObject"/>, uiDefName and <see cref="IControlFactory"/>. 
        /// </summary>
        /// <param name="bo">The business object to represent</param>
        /// <param name="uiDefName">The name of the ui def to use.</param>
        /// <param name="controlFactory">The <see cref="IControlFactory"/> to use for creating the Editor form controls</param>
        public DefaultBOEditorFormWin(BusinessObject bo, string uiDefName, IControlFactory controlFactory)
            : this(bo, uiDefName, controlFactory, controlFactory.CreateTabControl)
        {
        }

        /// <summary>
        /// Returns the BOPanel being used to edit the form.
        /// </summary>
        protected IPanel BoPanel
        {
            get { return _boPanel; }
        }

        /// <summary>
        /// Sets all the controls up in a layout manager. By default uses the border layout manager
        /// with the editor control centre and the buttons south.
        /// </summary>
        protected virtual void CreateLayout()
        {
            BorderLayoutManager borderLayoutManager = new BorderLayoutManagerWin(this, _controlFactory);
            borderLayoutManager.AddControl(BoPanel, BorderLayoutManager.Position.Centre);
            borderLayoutManager.AddControl(Buttons, BorderLayoutManager.Position.South);
        }

        /// <summary>
        /// Sets up the forms size based on the BOPanel and the Buttons.
        /// </summary>
        /// <param name="def"></param>
        protected virtual void SetupFormSize(IUIForm def)
        {
            _boPanel.Size = new Size(def.Width, def.Height);
            int width = _boPanel.Width;
            int height = _boPanel.Height + _buttons.Height;
            
            ClientSize = new Size(width, height);
            MinimumSize = Size;
        }

        private void FocusOnFirstControl()
        {
            //IControlHabanero controlToFocus = _panelInfo.FirstControlToFocus;
            //MethodInfo focusMethod = controlToFocus.GetType().
            //    GetMethod("Focus", BindingFlags.Instance | BindingFlags.Public);
            //if (focusMethod != null)
            //{
            //    focusMethod.Invoke(controlToFocus, new object[] { });
            //}
        }

        /// <summary>
        /// Creates a transaction Committer with the Business Object added.
        /// </summary>
        /// <returns>Returns the transaction object</returns>
        protected virtual ITransactionCommitter CreateSaveTransaction()
        {
            ITransactionCommitter committer = (ITransactionCommitter)BORegistry.DataAccessor.CreateTransactionCommitter();
            committer.AddBusinessObject(_bo);
            return committer;
        }

        private void SafeCloseForm()
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error Closing Form");
            }
        }

        /// <summary>
        /// A handler to respond when the "OK" button has been pressed.
        /// All changes are committed to the database and the dialog is closed.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void OkButtonHandler(object sender, EventArgs e)
        {
            try
            {
                _panelInfo.ApplyChangesToBusinessObject();
                ITransactionCommitter committer = CreateSaveTransaction();
                committer.CommitTransaction();
                DialogResult = Base.DialogResult.OK;
                if (_postObjectEditAction != null)
                {
                    _postObjectEditAction(this._bo, false);
                }
                _panelInfo.BusinessObject = null;
                SafeCloseForm();
            }
            catch (Exception ex)
            {
                log.Error(ExceptionUtilities.GetExceptionString(ex, 0, true));
                GlobalRegistry.UIExceptionNotifier.Notify(ex,
                    "There was a problem saving for the following reason(s):",
                    "Saving Problem");
            }
        }

        /// <summary>
        /// A handler to respond when the "Cancel" button has been pressed.
        /// Any unsaved edits are cancelled and the dialog is closed.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CancelButtonHandler(object sender, EventArgs e)
        {
            if (CancelEditsToBusinessObject())
            {
                SafeCloseForm();
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (_panelInfo.BusinessObject != null)
            {
                if (!CancelEditsToBusinessObject())
                {
                    e.Cancel = true;
                }
            }
        }

        private bool CancelEditsToBusinessObject()
        {
            
            bool success = false;
            try
            {
                _panelInfo.BusinessObject = null;
                new DefaultBOEditorFormManager().CancelEditsToBusinessObject(_bo);
                DialogResult = Base.DialogResult.Cancel;
                if (_postObjectEditAction != null)
                {
                    _postObjectEditAction(this._bo, true);
                }
                success = true;
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex,
                    "There was a problem cancelling the edit for the following reason(s):",
                    "Problem Cancelling");
            }
            return success;
        }

        /// <summary>
        /// Gets the button control for the buttons in the form
        /// </summary>
        public IButtonGroupControl Buttons
        {
            get { return _buttons; }
        }

        /// <summary>
        /// Gets the object containing all information related to the form, including
        /// its controls, mappers and business object
        /// </summary>
        public IPanelInfo PanelInfo
        {
            get { return _panelInfo; }
        }

        ///<summary>
        /// The Creator (<see cref="IDefaultBOEditorForm.GroupControlCreator"/> used to create the <see cref="IGroupControl"/>
        ///</summary>
        public GroupControlCreator GroupControlCreator { get; protected set; }

        /// <summary>
        /// Pops the form up in a modal dialog.  If the BO is successfully edited and saved, returns true,
        /// else returns false.
        /// </summary>
        /// <returns>True if the edit was a success, false if not</returns>
        bool IDefaultBOEditorForm.ShowDialog()
        {
            return this.ShowDialog() == (System.Windows.Forms.DialogResult)Base.DialogResult.OK;
        }
    }
}