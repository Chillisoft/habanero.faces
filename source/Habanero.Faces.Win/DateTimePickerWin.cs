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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Habanero.Faces.Base;
using DateTimePickerFormat = System.Windows.Forms.DateTimePickerFormat;

namespace Habanero.Faces.Win
{
    /// <summary>
    /// Represents a DateTimePicker control
    /// </summary>
    public class DateTimePickerWin : DateTimePicker, IDateTimePicker
    {

        private readonly DateTimePickerManager _manager;

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
        	_manager.ValueChanged += (sender, args) => base.OnValueChanged(args);
			_manager.NullDisplayBoxCustomizationDelegate += NullDisplayBoxCustomization;
            this.SetGlobalDefaultFormat();
        }

        private void SetGlobalDefaultFormat()
        {
            if ((GlobalUIRegistry.DateDisplaySettings != null) &&
                (GlobalUIRegistry.DateDisplaySettings.DateTimePickerDefaultFormat != null))
            {
                this.Format = DateTimePickerFormat.Custom;
                ;
                this.CustomFormat = GlobalUIRegistry.DateDisplaySettings.DateTimePickerDefaultFormat;
            }
        }

        private void NullDisplayBoxCustomization(ILabel label)
    	{
			if (Application.RenderWithVisualStyles)
			{
				label.Top = 3;
				Size maxTextSize = GetMaxTextSize();
				label.Height = maxTextSize.Height;
				if (this.Width >= maxTextSize.Width + 30)
				{
					// There is a drop down with the calendar shown
					label.Width = Width - label.Left - 35;
				}
				else
				{
					// There is a normal drop down shown
					label.Width = Width - 20;
				}
			}
    	}

    	private Size GetMaxTextSize()
    	{
    		var format = DateTimePickerUtil.GetDateFormatString(this);
    		var longestDate = new DateTime(2000,9,27,23,59,59); // This is a wednesday the longest day, and September the longest month
    		var longestDateString = longestDate.ToString(format);
    		return TextRenderer.MeasureText(longestDateString, this.Font);
    	}

    	/// <summary>
        /// Gets or sets the anchoring style.
        /// </summary>
        /// <value></value>
        Base.AnchorStyles IControlHabanero.Anchor
        {
            get { return (Base.AnchorStyles)base.Anchor; }
            set { base.Anchor = (System.Windows.Forms.AnchorStyles)value; }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlHabanero.Dock
        {
            get { return DockStyleWin.GetDockStyle(base.Dock); }
            set { base.Dock = DockStyleWin.GetDockStyle(value); }
        }

        /// <summary>
        /// Gets or sets the date/time value assigned to the control.
        /// </summary>
        DateTime IDateTimePicker.Value
        {
            get { return _manager.Value; }
            set { _manager.Value = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Value property has
        /// been set with a valid date/time value and the displayed value is able to be updated
        /// </summary>
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
			if (_manager != null)
			{
				//The base OnValueChanged will be called by the _manager.
				_manager.OnValueChanged(eventargs);
			}
            else base.OnValueChanged(eventargs);
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.
        ///</summary>
        ///
        ///<param name="eventargs">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            _manager.OnResize(eventargs);
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.Control.Click" /> event.
        ///</summary>
        ///
        ///<param name="eventargs">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnClick(EventArgs eventargs)
        {
            base.OnClick(eventargs);
            _manager.ChangeToValueMode();
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.Control.KeyDown" /> event.
        ///</summary>
        ///
        ///<param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e == null) return;
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
            _manager.UpdateFocusState();
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.Control.ForeColorChanged" /> event.
        ///</summary>
        ///
        ///<param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            _manager.UpdateFocusState();
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged" /> event.
        ///</summary>
        ///
        ///<param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            _manager.UpdateFocusState();
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.
        ///</summary>
        ///
        ///<param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _manager.UpdateFocusState();
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event.
        ///</summary>
        ///
        ///<param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            _manager.UpdateFocusState();
        }

        /// <summary>
        /// Occurs when the Value property changes
        /// </summary>
        event EventHandler IDateTimePicker.ValueChanged
        {
            add { _manager.ValueChanged += value; }
            remove { _manager.ValueChanged -= value; }
        }

        /// <summary>
        /// Gets or sets the date/time value assigned to the control, but returns
        /// null if there is no date set in the picker
        /// </summary>
        public DateTime? ValueOrNull
        {
            get { return _manager.ValueOrNull; }
            set { _manager.ValueOrNull = value; }
        }

        ///<summary>
        /// The text that will be displayed when the Value is null
        ///</summary>
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
        Base.DateTimePickerFormat IDateTimePicker.Format
        {
            get { return (Base.DateTimePickerFormat)base.Format; }
            set { base.Format = (System.Windows.Forms.DateTimePickerFormat)value; }
        }
    }
}