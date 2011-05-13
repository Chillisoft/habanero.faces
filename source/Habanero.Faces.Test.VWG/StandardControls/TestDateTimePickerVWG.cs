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