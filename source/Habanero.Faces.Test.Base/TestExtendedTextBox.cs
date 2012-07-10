using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Habanero.Faces.Base;
using Habanero.Faces.Base.ControlMappers;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    public abstract class TestExtendedTextBox
    {
        protected abstract IControlFactory GetControlFactory();
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [Test]
        public void Test_Constructor()
        {
            //---------------Set up test pack-------------------
            IControlFactory controlFactory = GetControlFactory();
            //---------------Execute Test ----------------------
            IExtendedTextBox extendedTextBox = controlFactory.CreateExtendedTextBox();
            //---------------Test Result -----------------------
            ITextBox textBox = extendedTextBox.TextBox;
            Assert.IsNotNull(textBox);
            IButton button = extendedTextBox.Button;
            Assert.IsNotNull(button);
            Assert.AreEqual("...", button.Text);
            Assert.IsFalse(textBox.Enabled);
            if (controlFactory is ControlFactoryWin)
                Assert.AreEqual(SystemColors.Window, textBox.BackColor);
            else
                Assert.AreEqual(Color.White, textBox.BackColor);
            Assert.AreEqual(extendedTextBox.Height, textBox.Height);
            Assert.Greater(button.Left, textBox.Left);
        }

        #region TestSelectItem Control

        [Test]
        public void Test_GetTextBox()
        {
            //--------------- Set up test pack ------------------
            IControlFactory controlFactory = GetControlFactory();
            IExtendedTextBox extendedComboBox = controlFactory.CreateExtendedTextBox();
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
            IExtendedTextBox extendedTextBoxWin = controlFactory.CreateExtendedTextBox();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            IButton button = extendedTextBoxWin.Button;
            //--------------- Test Result -----------------------
            Assert.IsNotNull(button);
        }

        #endregion //testSelectItem Control
    }

}
