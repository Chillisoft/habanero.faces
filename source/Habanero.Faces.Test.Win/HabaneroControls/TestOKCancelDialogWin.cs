using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
    [TestFixture]
    public class TestOKCancelDialogWin : TestOKCancelDialog
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        [Test]
        public void Test_CreateOKCancelForm_ShouldSetupOKButtonAsAcceptButton()
        {
            //---------------Set up test pack-------------------
            IOKCancelDialogFactory okCancelDialogFactory = CreateOKCancelDialogFactory();
            //---------------Execute Test ----------------------
            FormWin dialogForm = (FormWin)okCancelDialogFactory.CreateOKCancelForm(GetControlFactory().CreatePanel(), "");
            //---------------Test Result -----------------------
            IButtonGroupControl buttons = (IButtonGroupControl)dialogForm.Controls[0].Controls[1];
            Assert.AreSame(buttons["OK"], dialogForm.AcceptButton);
        }

        [Test]
        public void Test_DialogResult_WhenOkClicked_ShouldBeOK()
        {
            //---------------Set up test pack-------------------
            OKCancelDialogFactoryWin okCancelDialogFactory = (OKCancelDialogFactoryWin)CreateOKCancelDialogFactory();
            FormWin dialogForm = (FormWin)okCancelDialogFactory.CreateOKCancelForm(GetControlFactory().CreatePanel(), "");

            //---------------Execute Test ----------------------
            okCancelDialogFactory.OkButton_ClickHandler(dialogForm);
            //---------------Test Result -----------------------
            Assert.AreEqual(dialogForm.DialogResult, Habanero.Faces.Base.DialogResult.OK);
        }
        [Test]
        public void Test_DialogResult_WhenCancelClicked_ShouldBeCancel()
        {
            //---------------Set up test pack-------------------
            OKCancelDialogFactoryWin okCancelDialogFactory = (OKCancelDialogFactoryWin)CreateOKCancelDialogFactory();
            FormWin dialogForm = (FormWin)okCancelDialogFactory.CreateOKCancelForm(GetControlFactory().CreatePanel(), "");

            //---------------Execute Test ----------------------
            okCancelDialogFactory.CancelButton_ClickHandler(dialogForm);
            //---------------Test Result -----------------------
            Assert.AreEqual(dialogForm.DialogResult, Habanero.Faces.Base.DialogResult.Cancel);
        }
    }
}