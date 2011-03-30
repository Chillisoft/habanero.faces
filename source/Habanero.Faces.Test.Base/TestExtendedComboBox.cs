using System.Drawing;
using Habanero.Faces.Base.CF;
using Habanero.Faces.Base.ControlMappers;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    public abstract class TestExtendedComboBox
    {

        protected abstract IControlFactory GetControlFactory();
/*            {
                return new ControlFactoryWin();
            }*/

            [Test]
            public void Test_Layout()
            {
                //--------------- Set up test pack-------------------
                IControlFactory controlFactory = GetControlFactory();
                //--------------- Test Preconditions ----------------

                //--------------- Execute Test ----------------------
                IExtendedComboBox extendedComboBox = controlFactory.CreateExtendedComboBox();
                //--------------- Test Result -----------------------
                Assert.AreEqual(2, extendedComboBox.Controls.Count);
                IControlHabanero control1 = extendedComboBox.Controls[0];
                IControlHabanero control2 = extendedComboBox.Controls[1];
                Assert.IsInstanceOf(typeof(IComboBox), control1);
                Assert.IsInstanceOf(typeof(IButton), control2);
                Assert.AreEqual("...", control2.Text);
                Assert.AreEqual(0, control1.Left);
                Assert.LessOrEqual(control1.Width, control2.Left);
                Assert.GreaterOrEqual(extendedComboBox.Width, control2.Left + control2.Width);
            }

            [Test]
            public void Test_GetCombo()
            {
                //--------------- Set up test pack ------------------
                IControlFactory controlFactory = GetControlFactory();
                IExtendedComboBox extendedComboBox = controlFactory.CreateExtendedComboBox();
                //--------------- Test Preconditions ----------------

                //--------------- Execute Test ----------------------
                IComboBox comboBox = extendedComboBox.ComboBox;
                //--------------- Test Result -----------------------
                Assert.IsNotNull(comboBox);
            }

            [Test]
            public void Test_GetButton()
            {
                //--------------- Set up test pack ------------------
                IControlFactory controlFactory = GetControlFactory();
                IExtendedComboBox extendedComboBox = controlFactory.CreateExtendedComboBox();
                //--------------- Test Preconditions ----------------

                //--------------- Execute Test ----------------------
                IButton button = extendedComboBox.Button;
                //--------------- Test Result -----------------------
                Assert.IsNotNull(button);
            }

    }
}