using Habanero.Faces.Test.Base.ButtonsControl;
using Habanero.Faces.Test.Base.Wizard;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win
{


    [TestFixture]
    public class TestButtonSizePolicyWin : TestButtonSizePolicy
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override IButtonSizePolicy CreateButtonSizePolicy()
        {
            return new ButtonSizePolicyWin(GetControlFactory());
        }

        [Test]
        public void TestButtonWidthOneSmallButton_WinOnly()
        {
            //---------------Set up test pack-------------------
            IButtonSizePolicy buttonSizePolicy = CreateButtonSizePolicy();
            IControlCollection buttonCollection = GetControlFactory().CreatePanel().Controls;
            IButton btnTest = GetControlFactory().CreateButton("A");
            buttonCollection.Add(btnTest);

            //---------------Execute Test ----------------------
            buttonSizePolicy.RecalcButtonSizes(buttonCollection);

            ////---------------Test Result -----------------------

            Assert.AreEqual(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 24, btnTest.Width,
                            "Button width is incorrect - when buttons are very small they should instead be 1 24th of screen width.");
        }
    }
}