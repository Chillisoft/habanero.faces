using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.StandardControls
{
    [TestFixture]
    public class TestPictureBoxWin : TestPictureBox
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override string GetUnderlyingSizeModeToString(IPictureBox pictureBox)
        {
            System.Windows.Forms.PictureBox control = (System.Windows.Forms.PictureBox)pictureBox;
            return control.SizeMode.ToString();
        }
    }
}