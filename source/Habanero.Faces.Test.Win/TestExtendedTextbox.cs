using System.Drawing;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win
{
    [TestFixture]
    public class TestExtendedTextBoxWin : TestExtendedTextBox
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }


   /*     [Test]
        public void Test_Constructor()
        {
            //---------------Set up test pack-------------------
            IControlFactory controlFactory = GetControlFactory();
            //---------------Execute Test ----------------------
            ExtendedTextBoxWin extendedTextBox = new ExtendedTextBoxWin(controlFactory);
            //---------------Test Result -----------------------
            ITextBox textBox = extendedTextBox.TextBox;
            Assert.IsNotNull(textBox);
            IButton button = extendedTextBox.Button;
            Assert.IsNotNull(button);
            Assert.AreEqual("...", button.Text);
            Assert.IsFalse(textBox.Enabled);
            Assert.AreEqual(textBox.BackColor, Color.White);
            Assert.AreEqual(extendedTextBox.Height, textBox.Height);
            Assert.Greater(button.Left, textBox.Left);
        }

        #region TestSelectItem Control

        [Test]
        public void Test_GetTextBox()
        {
            //--------------- Set up test pack ------------------
            IControlFactory controlFactory = GetControlFactory();
            ExtendedTextBoxWin extendedComboBox = new ExtendedTextBoxWin(controlFactory);
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            ITextBox comboBox = extendedComboBox.TextBox;
            //--------------- Test Result -----------------------
            Assert.IsNotNull(comboBox);
        }

        [Test]
        public void Test_GetButton()
        {
            //--------------- Set up test pack ------------------
            IControlFactory controlFactory = GetControlFactory();
            ExtendedTextBoxWin extendedTextBoxWin = new ExtendedTextBoxWin(controlFactory);
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            IButton button = extendedTextBoxWin.Button;
            //--------------- Test Result -----------------------
            Assert.IsNotNull(button);
        }*/

//        #endregion //testSelectItem Control
    }
}