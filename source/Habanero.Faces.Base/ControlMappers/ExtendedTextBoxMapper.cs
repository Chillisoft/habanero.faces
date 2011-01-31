using System;
using System.ComponentModel;
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
        private IBusinessObject _selectedBusinessObject;

        /// <summary>
        /// The extended Text box being mapped to the property by this mapper.
        /// </summary>
        protected IExtendedTextBox ExtendedTextBox { get; set; }
        /// <summary>
        /// The <see cref="IButtonGroupControl"/> that has been has the Cancel and Select Buttons.
        /// </summary>
        protected IButtonGroupControl SelectButtonGroupControl { get; set; }

        ///<summary>
        /// Constructs the mapper for <see cref="IExtendedComboBox"/>.
        ///</summary>
        public ExtendedTextBoxMapper
            (IExtendedTextBox ctl, string propName, bool isReadOnly, IControlFactory controlFactory)
            : base(ctl, propName, isReadOnly, controlFactory)
        {
            ExtendedTextBox = ctl;
            ExtendedTextBox.Button.Click += delegate
                     {
                         SetupPopupForm();
                         //PopupForm.Closing += HandlePopUpFormClosedEvent;
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
            try
            {
                IBusinessObject currentBusinessObject = GetSelectedBusinessObject();
                //TODO brett 27 May 2010: Check if dirty if dirty then 
                // if valid Ask  have option of Save, CancelEdits, CancelClose.
                // if not valid ask if want to CancelEdits or CancelClose
                /*if ((currentBusinessObject != null) && currentBusinessObject.IsValid())
                {
                    currentBusinessObject.Save();
                }*/
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
                return;
            }
        }

        ///<summary>
        /// The <see cref="BusinessObject"/> that is set on the <see cref="IBOGridAndEditorControl"/>.
        ///</summary>
        protected virtual IBusinessObject GetSelectedBusinessObject()
        {
            IBOGridAndEditorControl iboGridAndEditorControl = GetIBOGridAndEditorControl();
            if (iboGridAndEditorControl == null) return null;
            IBusinessObject businessObject = iboGridAndEditorControl.CurrentBusinessObject;
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
            return (IBOGridAndEditorControl) PopupForm.Controls[0];
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
            
            SetupSelectButtonGroupControl();
            
            IBOGridAndEditorControl iboGridAndEditorControl = ControlFactory.CreateGridAndBOEditorControl(lookupTypeClassDef);
            IBusinessObjectCollection col = GetCollection(classType);
            iboGridAndEditorControl.BusinessObjectCollection = col;

            //PopupForm.Controls.Add(iboGridAndEditorControl);
            BorderLayoutManager manager = ControlFactory.CreateBorderLayoutManager(PopupForm);
            manager.AddControl(iboGridAndEditorControl, BorderLayoutManager.Position.Centre);
            manager.AddControl(SelectButtonGroupControl, BorderLayoutManager.Position.South);
            iboGridAndEditorControl.Dock = DockStyle.Fill;
        }

        private void CreatePopupForm()
        {
            PopupForm = ControlFactory.CreateForm();
            PopupForm.Height = 600;
            PopupForm.Width = 800;
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
            _selectedBusinessObject = GetSelectedBusinessObject();
            SetPropertyValue(_selectedBusinessObject);
            CloseForm(_selectedBusinessObject);
        }

        protected override void SetPropertyValue(object value)
        {
            var businessObject = value as BusinessObject;
            if (businessObject == null || !businessObject.Status.IsValid()) return;
            base.SetPropertyValue(businessObject);
        }

        /// <summary>
        /// Handler for the Cancel click
        /// </summary>
        protected virtual void CancelClickHandler(object sender, EventArgs e)
        {
            CloseForm(_selectedBusinessObject);
        }

        ///<summary>
        ///</summary>
        ///<param name="selectedBusinessObject"></param>
        protected virtual void CloseForm(IBusinessObject selectedBusinessObject)
        {
            if (selectedBusinessObject == null)
            {
                PopupForm.Close();
                return;
            }
            var confirmer = new MessageBoxConfirmer(ControlFactory, "Confirmation", MessageBoxIcon.Question);
            confirmer.Confirm("Do you want to save '" + selectedBusinessObject.ToString() + "'?", delegate(bool confirmed) 
            {
                if (confirmed)
                {
                    selectedBusinessObject.Save();
                    PopupForm.Close();  
                }
                else
                {
                    selectedBusinessObject.CancelEdits();
                    PopupForm.Close();   
                }
            });
        }

        private IClassDef GetLookupTypeClassDef(out Type classType)
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
            if(this.BusinessObject.Status.IsValid())
                ExtendedTextBox.Text = Convert.ToString(GetPropertyValue());    
        }
    }
}