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
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.StandardControls
{
    [TestFixture]
    public class TestDateTimePickerVWG : TestDateTimePicker
    {
    	protected override void SubscribeToBaseValueChangedEvent(IDateTimePicker dateTimePicker, EventHandler onValueChanged)
    	{
			Gizmox.WebGUI.Forms.DateTimePicker picker = (Gizmox.WebGUI.Forms.DateTimePicker)dateTimePicker;
			picker.ValueChanged += onValueChanged;
    	}

    	protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override EventArgs GetKeyDownEventArgsForDeleteKey()
        {
            return new Gizmox.WebGUI.Forms.KeyEventArgs(Gizmox.WebGUI.Forms.Keys.Delete);
        }

        protected override EventArgs GetKeyDownEventArgsForBackspaceKey()
        {
            return new Gizmox.WebGUI.Forms.KeyEventArgs(Gizmox.WebGUI.Forms.Keys.Back);
        }

        protected override EventArgs GetKeyDownEventArgsForOtherKey()
        {
            return new Gizmox.WebGUI.Forms.KeyEventArgs(Gizmox.WebGUI.Forms.Keys.A);
        }

        protected override void SetBaseDateTimePickerValue(IDateTimePicker dateTimePicker, DateTime value)
        {
            Gizmox.WebGUI.Forms.DateTimePicker picker = (Gizmox.WebGUI.Forms.DateTimePicker)dateTimePicker;
            picker.Value = value;
        }

        protected override void SetBaseDateTimePickerCheckedValue(IDateTimePicker dateTimePicker, bool value)
        {
            Gizmox.WebGUI.Forms.DateTimePicker picker = (Gizmox.WebGUI.Forms.DateTimePicker)dateTimePicker;
            picker.Checked = value;
        }

    }
}