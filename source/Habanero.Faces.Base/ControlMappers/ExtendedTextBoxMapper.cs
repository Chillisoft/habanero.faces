using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Habanero.Base;
using Habanero.BO;
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
            ExtendedTextBox = ctl;
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
            IBusinessObject currentBusinessObject = GetSelectedBusinessObject();
            //TODO brett 27 May 2010: Check if dirty if dirty then 
            // if valid Ask  have option of Save, CancelEdits, CancelClose.
            // if not valid ask if want to CancelEdits or CancelClose
            if ((currentBusinessObject != null) && currentBusinessObject.IsValid())
            {
                currentBusinessObject.Save();
            }
        }

        ///<summary>
        /// The <see cref="BusinessObject"/> that is set on the <see cref="IBOGridAndEditorControl"/>.
        ///</summary>
        protected virtual IBusinessObject GetSelectedBusinessObject()
        {
            IBOGridAndEditorControl iboGridAndEditorControl = GetIBOGridAndEditorControl();
            if (iboGridAndEditorControl == null) return null;
            IBusinessObject businessObject = iboGridAndEditorControl.GridControl.SelectedBusinessObject; //.CurrentBusinessObject;
            return businessObject;
        }

        /// <summary>
        /// Returns the <see cref="IBOGridAndEditorControl"/> that is being used to select and edit the
        /// RelatedBusinessObject.
        /// </summary>
        /// <returns></returns>
        protected virtual IBOGridAndEditorControl GetIBOGridAndEditorControl()
        {
            if (PopupForm == null) return null;
            foreach (var ctl in PopupForm.Controls)
            {
                var ret = ctl as IBOGridAndEditorControl;
                if (ret != null)
                    return ret;
            }
            return null;
        }

        ///<summary>
        /// Shows the popup form that is displayed when the button is clicked.
        /// This popup form is used to edit the <see cref="BusinessObject"/>s that fill the combobox.
        ///</summary>
        protected virtual void SetupPopupForm()
        {
            Type classType;
            IClassDef lookupTypeClassDef = GetLookupTypeClassDef(out classType);
            CreatePopupForm();
            var originalSize = new Size(PopupForm.Size.Width, PopupForm.Size.Height);
            
            SetupSelectButtonGroupControl();
            
            IBOGridAndEditorControl iboGridAndEditorControl = ControlFactory.CreateGridAndBOEditorControl(lookupTypeClassDef);
            iboGridAndEditorControl.SkipSaveOnSelectionChanged = true;
            if (this._loadCollectionAfterFormLoad)
            {
                this.PopupForm.Load += (sender, e) =>
                {
                    //iboGridAndEditorControl.BusinessObjectCollection = GetCollection(classType);
                    iboGridAndEditorControl.PopulateCollectionAsync(() => { return GetCollection(classType); });
                };
            }
            else // this branch is really only here for tests which expect synchronous workings
            {
                iboGridAndEditorControl.BusinessObjectCollection = GetCollection(classType);
            }

            //PopupForm.Controls.Add(iboGridAndEditorControl);
            BorderLayoutManager manager = ControlFactory.CreateBorderLayoutManager(PopupForm);
            manager.AddControl(iboGridAndEditorControl, BorderLayoutManager.Position.Centre);
            manager.AddControl(SelectButtonGroupControl, BorderLayoutManager.Position.South);
            iboGridAndEditorControl.Dock = DockStyle.Fill;

            PopupForm.MinimumSize = new Size(iboGridAndEditorControl.MinimumSize.Width + 250, iboGridAndEditorControl.MinimumSize.Height + 100);

            PopupForm.Size = originalSize;
        }

        private void CreatePopupForm()
        {
            PopupForm = ControlFactory.CreateForm();
            PopupForm.Height = 600;
            PopupForm.Width = 800;
            PopupForm.MinimumSize = new Size(400, 300);
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

        private static IBusinessObjectCollection GetCollection(Type classType)
        {
            Type collectionType = typeof (BusinessObjectCollection<>).MakeGenericType(classType);
            IBusinessObjectCollection col = (IBusinessObjectCollection) Activator.CreateInstance(collectionType);
            col.LoadAll();
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