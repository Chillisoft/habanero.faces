using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.StandardControls
{
    [TestFixture]
    public class TestComboBoxVWG : TestComboBox
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override string GetUnderlyingAutoCompleteSourceToString(IComboBox controlHabanero)
        {
            Gizmox.WebGUI.Forms.ComboBox control = (Gizmox.WebGUI.Forms.ComboBox)controlHabanero;
            return control.AutoCompleteSource.ToString();
        }
    }
}