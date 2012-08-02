using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Timers;
using Habanero.Base;
using Habanero.BO;
using Habanero.Faces.Base.Async;
using Habanero.Faces.Base.ControlInterfaces;
using Habanero.Faces.Base.ControlMappers;

namespace Habanero.Faces.Base
{
    ///<summary>
    /// The mapper for <see cref="IExtendedTextBox"/>.
    ///</summary>
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
    // ReSharper disable MemberCanBePrivate.Global
    public class ExtendedTextBoxMapper : ControlMapper
    {
        /// <summary>
        /// Raised when the popup form has been created so that client code can fiddle with
        /// the form or subscribe to events
        /// </summary>
        public EventHandler PopupFormCreated { get; set; }

        /// <summary>
        /// Boolean to enable / disable editing pane
        /// </summary>
        public bool EnableEditing { get; set; }

        /// <summary>
        /// The extended Text box being mapped to the property by this mapper.
        /// </summary>
        protected IExtendedTextBox ExtendedTextBox { get; set; }

        /// <summary>
        /// The <see cref="IButtonGroupControl"/> that has been has the Cancel and Select Buttons.
        /// </summary>
        protected IButtonGroupControl SelectButtonGroupControl { get; set; }

        protected bool _loadCollectionAfterFormLoad;

        ///<summary>
        /// Constructs the mapper for <see cref="IExtendedComboBox"/>.
        ///</summary>
        public ExtendedTextBoxMapper
            (IExtendedTextBox ctl, string propName, bool isReadOnly, IControlFactory controlFactory)
            : base(ctl, propName, isReadOnly, controlFactory)
        {
            this._loadCollectionAfterFormLoad = true;
            this.EnableEditing = true;
            ExtendedTextBox = ctl;
            ExtendedTextBox.ControlMapper = this;
            ExtendedTextBox.Button.Click += delegate
                     {
                         SetupPopupForm();
                         PopupForm.Closing += HandlePopUpFormClosedEvent;
                         PopupForm.ShowDialog();
                     };
        }

        /// <summary>
        /// Handles the Closing of the Popup form.
        /// By default this saves the Business Object that is currently selectedin the Popup  (if there is one)
        /// and Sets the Currently selected Business Object.ToString as the text of the TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void HandlePopUpFormClosedEvent(object sender, CancelEventArgs e)
        {
            if (this.EnableEditing)
            {
                IBusinessObject currentBusinessObject = GetSelectedBusinessObject();
                //TODO brett 27 May 2010: Check if dirty if dirty then 
                // if valid Ask  have option of Save, CancelEdits, CancelClose.
                // if not valid ask if want to CancelEdits or CancelClose
                if ((currentBusinessObject != null) && currentBusinessObject.IsValid())
                {
                    currentBusinessObject.Save();
                }
            }
        }

        ///<summary>
        /// The <see cref="BusinessObject"/> that is set on the <see cref="IBOGridAndEditorControl"/>.
        ///</summary>
        protected virtual IBusinessObject GetSelectedBusinessObject()
        {
            var grid = this.GetReadOnlyGrid();
            if (grid == null) return null;
            return grid.SelectedBusinessObject;
        }

        /// <summary>
        /// Returns the <see cref="IReadOnlyGrid"/> that is being used to select the
        /// RelatedBusinessObject.
        /// </summary>
        /// <returns></returns>

        protected virtual IReadOnlyGridControl GetReadOnlyGrid(IControlHabanero parent = null)
        {
            if (parent == null) parent = this.PopupForm;
            if (parent == null) return null;
            foreach (var control in parent.Controls)
            {
                var ctl = control as IControlHabanero;
                if (ctl == null) continue;
                var ret = ctl as IReadOnlyGridControl;
                if (ret != null) return ret;
                ret = this.GetReadOnlyGrid(ctl);
                if (ret != null) return ret;
            }
            return null;
        }
        ///<summary>
        /// Shows the popup form that is displayed when the button is clicked.
        /// This popup form is used to edit the <see cref="BusinessObject"/>s that fill the combobox.
        ///</summary>
        public virtual void SetupPopupForm()
        {
            Type classType;
            IClassDef lookupTypeClassDef = GetLookupTypeClassDef(out classType);
            CreatePopupForm();
            var originalSize = new Size(PopupForm.Size.Width, PopupForm.Size.Height);
            
            SetupSelectButtonGroupControl();
            ISupportAsyncLoadingCollection viewer;
            int minHeight;
            int minWidth;
            IGenericGridFilterControl filterControlPanel;
            var control = this.GenerateSelectionInterface(lookupTypeClassDef, out viewer, out minHeight, out minWidth, out filterControlPanel);

            this.LoadSelectionCollection(viewer, classType);

            BorderLayoutManager manager = ControlFactory.CreateBorderLayoutManager(PopupForm);
            manager.AddControl(control, BorderLayoutManager.Position.Centre);
            manager.AddControl(SelectButtonGroupControl, BorderLayoutManager.Position.South);
            if (filterControlPanel != null)
                manager.AddControl(filterControlPanel, BorderLayoutManager.Position.North);
            this.PopupForm.Text = "Loading... Please wait...";
            control.Dock = DockStyle.Fill;

            PopupForm.MinimumSize = new Size(minWidth + 250, minHeight + 100);
            PopupForm.Size = originalSize;
            if (this.PopupFormCreated != null)
                this.PopupFormCreated(this.PopupForm, new EventArgs());
        }

        private IControlHabanero GenerateSelectionInterface(IClassDef lookupTypeClassDef, 
            out ISupportAsyncLoadingCollection viewer, out int minHeight, out int minWidth, out IGenericGridFilterControl filterControlPanel)
        {
            filterControlPanel = null;
            IControlHabanero control;
            IGenericGridFilterControl addFilterEvents = null;
            if (this.EnableEditing)
            {
                var iboGridAndEditorControl = this.ControlFactory.CreateGridAndBOEditorControl(lookupTypeClassDef);
                iboGridAndEditorControl.SkipSaveOnSelectionChanged = true;
                iboGridAndEditorControl.GridControl.Grid.RowDoubleClicked += this.SelectClickHandler;
                control = iboGridAndEditorControl;
                viewer = iboGridAndEditorControl;
                minWidth = iboGridAndEditorControl.MinimumSize.Width;
                minHeight = iboGridAndEditorControl.MinimumSize.Height;
                addFilterEvents = iboGridAndEditorControl.FilterControl;
            }
            else
            {
                var grid = this.ControlFactory.CreateReadOnlyGridControl();
                filterControlPanel = this.ControlFactory.CreateGenericGridFilter(grid.Grid);
                grid.DoubleClickEditsBusinessObject = false;
                grid.AllowUsersToAddBO = false;
                grid.AllowUsersToDeleteBO = false;
                grid.AllowUsersToEditBO = false;
                grid.Grid.RowDoubleClicked += SelectClickHandler;
                grid.Buttons.Visible = false;
                control = grid;
                viewer = grid;
                minWidth = grid.MinimumSize.Width;
                minHeight = grid.MinimumSize.Height;
                addFilterEvents = filterControlPanel;
            }
            if (addFilterEvents != null)
            {
                addFilterEvents.FilterStarted += (s, e) => { this.PopupForm.Text = "Filtering..."; };
                addFilterEvents.FilterCompleted += (s, e) => { this.PopupForm.Text = "Please select..."; };
            }
            return control;
        }

        private void LoadSelectionCollection(ISupportAsyncLoadingCollection viewer, Type classType)
        {
            if (this._loadCollectionAfterFormLoad)
            {
                this.PopupForm.Load += (sender, e) =>
                    {
                        viewer.PopulateCollectionAsync(() => { return GetCollection(classType); },
                                                       () => { this.PopupForm.Text = "Please select..."; }); 
                    };
            }
            else // branch to make testing easier
            {
                viewer.BusinessObjectCollection = GetCollection(classType);
            }
        }

        private void CreatePopupForm()
        {
            PopupForm = ControlFactory.CreateForm();
            PopupForm.Height = 600;
            PopupForm.Width = 800;
            PopupForm.MinimumSize = new Size(400, 300);
            PopupForm.StartPosition = FormStartPosition.CenterScreen;
        } 

        private void SetupSelectButtonGroupControl()
        {
            SelectButtonGroupControl = ControlFactory.CreateButtonGroupControl();
            SelectButtonGroupControl.AddButton("Cancel", CancelClickHandler);
            SelectButtonGroupControl.AddButton("Select", SelectClickHandler);
        }
        /// <summary>
        /// Handler for the Select click
        /// </summary>
        protected virtual void SelectClickHandler(object sender, EventArgs e)
        {
            var selectedBusinessObject = GetSelectedBusinessObject();
            CloseForm();
            SetPropertyValue(selectedBusinessObject);
        }
        /// <summary>
        /// Handler for the Cancel click
        /// </summary>
        protected virtual void CancelClickHandler(object sender, EventArgs e)
        {
            CloseForm();
        }

        ///<summary>
        ///</summary>
        protected virtual void CloseForm()
        {
            PopupForm.Close();
        }

        protected virtual IClassDef GetLookupTypeClassDef(out Type classType)
        {
            BOMapper mapper = new BOMapper(BusinessObject);
            IClassDef lookupTypeClassDef = mapper.GetLookupListClassDef(PropertyName);
            classType = lookupTypeClassDef.ClassType;
            return lookupTypeClassDef;
        }

        private IBusinessObjectCollection GetCollection(Type classType)
        {
            string criteria = String.Empty;
            string order = String.Empty;
            try
            {
                var boll = this.BusinessObject.ClassDef.PropDefcol[this.PropertyName].LookupList as BusinessObjectLookupList;
                if (boll != null)
                {
                    criteria = boll.Criteria == null ? String.Empty : boll.Criteria.ToString();
                    order = boll.OrderCriteria == null ? String.Empty : boll.OrderCriteria.ToString();
                }
            }
            catch (Exception) { }
            Type collectionType = typeof (BusinessObjectCollection<>).MakeGenericType(classType);
            IBusinessObjectCollection col = (IBusinessObjectCollection) Activator.CreateInstance(collectionType);
            col.Load(criteria, order);
            //col.LoadAll();
            return col;
        }

        ///<summary>
        /// Returns the Popup Form.
        ///</summary>
        public IFormHabanero PopupForm { get; protected set; }

        /// <summary>
        /// Updates the properties on the represented business object
        /// </summary>
        public override void ApplyChangesToBusinessObject()
        {
            //We normally have the apply changes to Business Object so as to 
            // optimise VWG and other remote clients from updating the server 
            // for every change on the Control, 
            // but for this control the call back has already occured to handle the 
            // form popping up and being closed so there is no point in optimising this action.
            // The property value (SetPropertyValue) is therefore updated directly 
            // in the SelectClickHandler.
        }

        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="IControlMapper.BusinessObject"/>
        /// </summary>
        protected override void InternalUpdateControlValueFromBo()
        {
            ExtendedTextBox.Text = Convert.ToString(GetPropertyValue());
        }
    }
}