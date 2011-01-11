using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.StandardControls
{
    [TestFixture]
    public class TestPictureBoxVWG : TestPictureBox
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override string GetUnderlyingSizeModeToString(IPictureBox pictureBox)
        {
            Gizmox.WebGUI.Forms.PictureBox control = (Gizmox.WebGUI.Forms.PictureBox)pictureBox;
            return control.SizeMode.ToString();
        }
    }
}