using System;
using System.Drawing;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Win;
using Habanero.Smooth;
using Habanero.Test.Structure;
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

        public class BaseBO : BusinessObject
        {
            public Guid ID
            {
                get { return (Guid)this.GetPropertyValue("ID"); }
                set { this.SetPropertyValue("ID", value); }
            }
            public virtual SubBO Sub
            {
                get { return Relationships.GetRelatedObject<SubBO>("Sub"); }
                set { Relationships.SetRelatedObject("Sub", value); }
            }
        }

        public class SubBO : BusinessObject
        {
            public override string ToString()
            {
                return this.Name;
            }
            public Guid ID
            {
                get { return (Guid)this.GetPropertyValue("ID"); }
                set { this.SetPropertyValue("ID", value); }
            }

            public string Name
            {
                get { return (string)this.GetPropertyValue("Name"); }
                set { this.SetPropertyValue("Name", value); }
            }

            public string Description
            {
                get { return (string)this.GetPropertyValue("Description"); }
                set { this.SetPropertyValue("Description", value); }
            }

            public DateTime Created
            {
                get { return (DateTime)this.GetPropertyValue("Created"); }
                set { this.SetPropertyValue("Created", value); }
            }
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
            Assert.AreEqual(textBox.BackColor, SystemColors.Window);
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