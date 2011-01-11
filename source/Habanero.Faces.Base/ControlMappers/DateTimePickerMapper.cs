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

namespace Habanero.Faces.Base
{
    /// <summary>
    /// Wraps a DateTimePicker in order to display and capture a DateTime property of the business object 
    /// </summary>
    public class DateTimePickerMapper : ControlMapper
    {
        private readonly IDateTimePicker _picker;
        private readonly IDateTimePickerMapperStrategy _dateTimePickerMapperStrategy;


        /// <summary>
        /// Constructor to initialise a new instance of the class
        /// </summary>
        /// <param name="picker">The DateTimePicker control to which the property is mapped</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnly">Whether this control is read only</param>
        /// <param name="factory">The control factory to be used when creating the controlMapperStrategy</param>
        public DateTimePickerMapper(IDateTimePicker picker, string propName, bool isReadOnly, IControlFactory factory)
            : base(picker, propName, isReadOnly, factory)
        {
            _picker = picker;
            PropertyName = propName;
            _dateTimePickerMapperStrategy = factory.CreateDateTimePickerMapperStrategy();
            _dateTimePickerMapperStrategy.AddUpdateBoPropOnValueChangedHandler(this);
        }

        /// <summary>
        /// Gets the DateTimePicker control to which the property is mapped
        /// </summary>
        public IDateTimePicker DateTimePicker
        {
            get { return _picker; }
        }

        /// <summary>
        /// Updates the properties on the represented business object
        /// </summary>
        public override void ApplyChangesToBusinessObject()
        {
            object newValue = GetValueOfDateTimePicker();
            SetPropertyValue(newValue);
        }
        /// <summary>
        /// Returns the value currently held by the picker
        /// </summary>
        /// <returns>Returns the value held</returns>
        private object GetValueOfDateTimePicker()
        {
            if (!_picker.ValueOrNull.HasValue) return null;
            return _picker.ValueOrNull.Value;
        }

        /// <summary>
        /// Initialises the control using the attributes already provided, using
        /// <see cref="ControlMapper.SetPropertyAttributes"/>.
        /// </summary>
        protected override void InitialiseWithAttributes()
        {
            if (_attributes.Contains("dateFormat"))
            {
                String dateFormat = Convert.ToString(_attributes["dateFormat"]);
                if (dateFormat.ToLower()=="d")
                {
                    _picker.Format = DateTimePickerFormat.Short;
                }
                else
                {
                    _picker.Format = DateTimePickerFormat.Custom;
                    _picker.CustomFormat = dateFormat;
                }
                //DateTimePickerUtil.SetCustomFormat(_dateTimePicker, dateFormat);
            }
            if (_attributes.Contains("showUpDown"))
            {
                bool showUpDown = Convert.ToBoolean(_attributes["showUpDown"]);
                _picker.ShowUpDown = showUpDown;
                //DateTimePickerUtil.SetShowUpDown(_dateTimePicker, showUpDown);
            }
            base.InitialiseWithAttributes();
        }


        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="IControlMapper.BusinessObject"/>
        /// </summary>
        protected  override void InternalUpdateControlValueFromBo()
        {
            if (_businessObject == null) return;
            object propertyValue = GetPropertyValue();
            if (propertyValue == null)
            {
                _picker.ValueOrNull = null;
            } else
            {
                _picker.Value = Convert.ToDateTime(propertyValue);
            }
        }
    }
}