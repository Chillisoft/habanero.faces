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
using System.Collections.Generic;
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
            ExtendedTextBox.Text = Convert.ToString(GetPropertyValue());
        }
    }
}