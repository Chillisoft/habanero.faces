// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;

namespace Habanero.Faces.Base
{
    ///<summary>
    /// The mapper for <see cref="IExtendedComboBox"/>.
    ///</summary>
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
    // ReSharper disable MemberCanBePrivate.Global
    public class ExtendedComboBoxMapper : ControlMapper //: LookupComboBoxMapper
    {
        private IBOGridAndEditorControl _iboGridAndEditorControl;

        /// <summary>
        /// The <see cref="IExtendedComboBox"/> box being mapped to the property by this mapper.
        /// </summary>
        protected IExtendedComboBox ExtendedComboBox { get; set; }
        /// <summary>
        /// The <see cref="LookupComboBoxMapper"/> that is being used by this mapper as a helper class.
        /// </summary>
        protected LookupComboBoxMapper LookupComboBoxMapper { get; set; }

        ///<summary>
        /// Constructs the mapper for <see cref="IExtendedComboBox"/>.
        ///</summary>
        public ExtendedComboBoxMapper
            (IExtendedComboBox ctl, string propName, bool isReadOnly, IControlFactory controlFactory)
            : base(ctl, propName, isReadOnly, controlFactory)
        {
            ExtendedComboBox = ctl;
            LookupComboBoxMapper = new LookupComboBoxMapper
                (ExtendedComboBox.ComboBox, propName, isReadOnly, controlFactory);
            ExtendedComboBox.Button.Click += delegate
                {
                    ShowPopupForm();
                    PopupForm.Closed += HandlePopUpFormClosedEvent;
                 
                    PopupForm.ShowDialog();
                };
        }
        /// <summary>
        /// Handles the Closing of the Popup form.
        /// By default this saves the Business Object that is currently selectedin the Popup  (if there is one)
        /// Reloads the Combo Box using <see cref="ReloadLookupValues"/>.
        /// and Sets the Currently selected Business Object as the selected Item for the ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void HandlePopUpFormClosedEvent(object sender, EventArgs e)
        {
            IBOGridAndEditorControl iboGridAndEditorControl = GetIBOGridAndEditorControl();
            IBusinessObject currentBusinessObject = iboGridAndEditorControl.CurrentBusinessObject;
            if ((currentBusinessObject != null) && currentBusinessObject.Status.IsValid())
            {
                currentBusinessObject.Save();
            }
            ReloadLookupValues();
            if (currentBusinessObject != null)
            {
                ExtendedComboBox.ComboBox.SelectedValue = currentBusinessObject.ID.GetAsValue().ToString();
            }
        }
        /// <summary>
        /// The <see cref="IBOGridAndEditorControl"/> that is being popped up by this mapper.
        /// </summary>
        /// <returns></returns>
        protected virtual IBOGridAndEditorControl GetIBOGridAndEditorControl()
        {
            return _iboGridAndEditorControl;
        }
        /// <summary>
        /// Reloads the ComboBox.
        /// </summary>
        protected virtual void ReloadLookupValues()
        {
            Type classType;
            GetLookupTypeClassDef(out classType);
            IBusinessObjectCollection collection = GetCollection(classType);
            LookupComboBoxMapper.LookupList.Clear();
            Dictionary<string, string> lookupValues = new Dictionary<string, string>();
            foreach (IBusinessObject businessObject in collection)
            {
                string toString = businessObject.ToString();
                while (lookupValues.ContainsKey(toString))
                {
                    toString += " ";
                }
                lookupValues.Add(toString, businessObject.ID.GetAsValue().ToString());
            }
            LookupComboBoxMapper.SetupComboBoxItems();
        }

        ///<summary>
        /// Shows the popup form that is displayed when the button is clicked.
        /// This popup form is used to edit the <see cref="BusinessObject"/>s that fill the combobox.
        ///</summary>
        public virtual void ShowPopupForm()
        {
            Type classType;
            CheckBoNotNull();
            IClassDef lookupTypeClassDef = GetLookupTypeClassDef(out classType);
            PopupForm = ControlFactory.CreateForm();
            PopupForm.Height = 600;
            PopupForm.Width = 800;

            _iboGridAndEditorControl = this.ControlFactory.CreateGridAndBOEditorControl
                (lookupTypeClassDef);
            _iboGridAndEditorControl.Dock = DockStyle.Fill;
            PopupForm.Controls.Add(_iboGridAndEditorControl);
            IBusinessObjectCollection col = GetCollection(classType);
            _iboGridAndEditorControl.BusinessObjectCollection = col;
        }

        private void CheckBoNotNull()
        {
            if (_businessObject == null)
                throw new HabaneroApplicationException(
                    "You cannot Show PopupForm for ExtendedCombobox since the related business object is not set. ExtendedComboBox for : " + this.PropertyName);
        }

        private IClassDef GetLookupTypeClassDef(out Type classType)
        {
            BOMapper mapper = new BOMapper(BusinessObject);
            IClassDef lookupTypeClassDef = mapper.GetLookupListClassDef(PropertyName);
            classType = lookupTypeClassDef.ClassType;
            return lookupTypeClassDef;
        }

        private IBusinessObjectCollection GetCollection(Type classType)
        {
            Type collectionType = typeof (BusinessObjectCollection<>).MakeGenericType(classType);
            IBusinessObjectCollection col = (IBusinessObjectCollection) Activator.CreateInstance(collectionType);
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
            LookupComboBoxMapper.ApplyChangesToBusinessObject();
        }

        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="IControlMapper.BusinessObject"/>
        /// </summary>
        public override void UpdateControlValueFromBusinessObject()
        {
            LookupComboBoxMapper.UpdateControlValueFromBusinessObject();
        }

        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="IControlMapper.BusinessObject"/>
        /// </summary>
        protected override void InternalUpdateControlValueFromBo()
        {
            LookupComboBoxMapper.DoUpdateControlValueFromBO();
        }

        /// <summary>
        /// Gets and sets the business object that has a property
        /// being mapped by this mapper.  In other words, this property
        /// does not return the exact business object being shown in the
        /// control, but rather the business object shown in the
        /// form.  Where the business object has been amended or
        /// altered, the <see cref="ControlMapper.UpdateControlValueFromBusinessObject"/> method is automatically called here to 
        /// implement the changes in the control itself.
        /// </summary>
        public override IBusinessObject BusinessObject
        {
            get { return base.BusinessObject; }
            set
            {
                LookupComboBoxMapper.BusinessObject = value;
                base.BusinessObject = value;
            }
        }
        /// <summary>
        /// Gets and sets the lookup list used to populate the items in the
        /// ComboBox.  This method is typically called by SetupLookupList().
        /// </summary>
        public Dictionary<string, string> LookupList
        {
            get { return LookupComboBoxMapper.LookupList; }
            set { LookupComboBoxMapper.LookupList = value; }
        }
    }


    // ReSharper restore ClassWithVirtualMembersNeverInherited.Global
    // ReSharper restore MemberCanBePrivate.Global

}