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
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using Habanero.Faces.Base;

namespace Habanero.Faces.Win
{
    /// <summary>
    /// Represents a DateTimePicker control
    /// </summary>
    public class DateTimePickerWin : DateTimePicker, IDateTimePicker
    {
        private readonly DateTimePickerManager _manager;
/*
        ///<summary>
        /// Constructor for <see cref="DateTimePickerWin"/>
        ///</summary>
        public DateTimePickerWin()
        {
            if (!IsInDesignMode())
            {
                DateTimePickerManager.ValueGetter<DateTime> valueGetter = () => base.Value;
                DateTimePickerManager.ValueSetter<DateTime> valueSetter = delegate(DateTime value)
                {
                    base.Value = value;
                };
                _manager = new DateTimePickerManager(GlobalUIRegistry.ControlFactory, this, valueGetter, valueSetter);
            }
        }

        private static bool IsInDesignMode()
        {
#if DEBUG
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                return true;
            }
            if (Process.GetCurrentProcess().ProcessName.ToUpper().Equals("DEVENV"))
            {
                return true;
            }
#endif
            return false;
        }*/
        ///<summary>
        /// Constructor for <see cref="DateTimePickerWin"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        public DateTimePickerWin(IControlFactory controlFactory)
        {
            DateTimePickerManager.ValueGetter<DateTime> valueGetter = () => base.Value;
            DateTimePickerManager.ValueSetter<DateTime> valueSetter = delegate(DateTime value)
            {
                base.Value = value;
            };
            _manager = new DateTimePickerManager(controlFactory, this, valueGetter, valueSetter);
        }
                
        /// <summary>
        /// Gets or sets the anchoring style.
        /// </summary>
        /// <value></value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        Base.AnchorStyles IControlHabanero.Anchor
        {
            get { return (Base.AnchorStyles)base.Anchor; }
            set { base.Anchor = (System.Windows.Forms.AnchorStyles)value; }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        Base.DockStyle IControlHabanero.Dock
        {
            get { return DockStyleWin.GetDockStyle(base.Dock); }
            set { base.Dock = DockStyleWin.GetDockStyle(value); }
        }

        /// <summary>
        /// Gets or sets the date/time value assigned to the control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        DateTime IDateTimePicker.Value
        {
            get { return _manager.Value; }
            set { _manager.Value = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Value property has
        /// been set with a valid date/time value and the displayed value is able to be updated
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        bool IDateTimePicker.Checked
        {
            get { return base.Checked; }
            set
            {
                base.Checked = value;
                if (_manager != null) _manager.OnValueChanged(new EventArgs());
            }
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.DateTimePicker.ValueChanged" /> event.
        ///</summary>
        ///
        ///<param name="eventargs">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnValueChanged(EventArgs eventargs)
        {
            base.OnValueChanged(eventargs);
            if (_manager != null) _manager.OnValueChanged(eventargs);
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.
        ///</summary>
        ///
        ///<param name="eventargs">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            if (_manager != null) _manager.OnResize(eventargs);
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.Control.Click" /> event.
        ///</summary>
        ///
        ///<param name="eventargs">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnClick(EventArgs eventargs)
        {
            base.OnClick(eventargs);
            if (_manager != null) _manager.ChangeToValueMode();
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.Control.KeyDown" /> event.
        ///</summary>
        ///
        ///<param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if(_manager == null) return;
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                if (_manager.ChangeToNullMode())
                {
                    e.SuppressKeyPress = true;
                    return;
                }
            } else
            {
                if (_manager.ChangeToValueMode())
                {
                    e.SuppressKeyPress = true;
                    return;
                }
            }
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.Control.BackColorChanged" /> event.
        ///</summary>
        ///
        ///<param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            if (_manager != null) _manager.UpdateFocusState();
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.Control.ForeColorChanged" /> event.
        ///</summary>
        ///
        ///<param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            if (_manager != null) _manager.UpdateFocusState();
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged" /> event.
        ///</summary>
        ///
        ///<param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (_manager != null) _manager.UpdateFocusState();
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.
        ///</summary>
        ///
        ///<param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (_manager != null) _manager.UpdateFocusState();
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event.
        ///</summary>
        ///
        ///<param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (_manager != null) _manager.UpdateFocusState();
        }

        /// <summary>
        /// Occurs when the Value property changes
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        event EventHandler IDateTimePicker.ValueChanged
        {
            add { _manager.ValueChanged += value; }
            remove { _manager.ValueChanged -= value; }
        }

        /// <summary>
        /// Gets or sets the date/time value assigned to the control, but returns
        /// null if there is no date set in the picker
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime? ValueOrNull
        {
            get { return _manager.ValueOrNull; }
            set { _manager.ValueOrNull = value; }
        }

        ///<summary>
        /// The text that will be displayed when the Value is null
        ///</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string NullDisplayValue
        {
            get { return _manager.NullDisplayValue; }
            set { _manager.NullDisplayValue = value; }
        }

        /// <summary>
        /// Gets or sets the format of the date and time displayed in the control.
        /// </summary>
        ///	<returns>One of the <see cref="Base.DateTimePickerFormat"></see> values. The default is <see cref="Base.DateTimePickerFormat.Long"></see>.</returns>
        ///	<exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="Base.DateTimePickerFormat"></see> values. </exception>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        Base.DateTimePickerFormat IDateTimePicker.Format
        {
            get { return (Base.DateTimePickerFormat)base.Format; }
            set { base.Format = (System.Windows.Forms.DateTimePickerFormat)value; }
        }
    }
}