// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Base.Util;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Test.Base
{

    public abstract class TestControlFactory
    {
        protected IControlFactory _factory;

        [SetUp]
        public void TestSetup()
        {
            _factory = GetControlFactory();
            ClassDef.ClassDefs.Clear();
            
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TestTearDown()
        {
            //Code that is executed after each and every test is executed in this fixture/class.
        }
        protected virtual int GetStandardTextBoxHeight()
        {
            return 21;
        }
        protected abstract IControlFactory GetControlFactory();

        protected abstract int GetBoldTextExtraWidth();
        protected abstract Type GetMasterGridColumnType();

        protected abstract Type GetMasterTextBoxGridColumnType();

        protected abstract Type GetHabaneroMasterGridColumnType();

        protected abstract string GetUINameSpace();


        [Test]
        public void TestCreateLabel_NoText()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            Label lbl = _factory.CreateLabel();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(lbl);
        }

        [Test]
        public void TestCreateLabel_Text()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            const string labelText = "test label";
            //---------------Execute Test ----------------------

            Label lbl = _factory.CreateLabel(labelText);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(lbl);
            Assert.AreEqual(labelText, lbl.Text);
            Assert.AreNotEqual(labelText, lbl.Name);
            //TODO_Port_DoTest lbl.FlatStyle = FlatStyle.Standard;
        }

/*        [Test]
        public void TestCreateLabel_BoldText()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            const string labelText = "test label";
            //---------------Execute Test ----------------------

            Label lbl = _factory.CreateLabel(labelText, true);
            //---------------Verify Result -----------------------
            //Assert.AreEqual(lbl.PreferredWidth + 10, lbl.Width);
            Assert.AreEqual(lbl.PreferredWidth + GetBoldTextExtraWidth(), lbl.Width);
            Font expectedFont = new Font(lbl.Font, 23f, FontStyle.Bold);
            Assert.AreEqual(expectedFont, lbl.Font);
              
        }*/

        [Test]
        public void TestCreateButton()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            const string buttonText = "test label";
            //---------------Execute Test ----------------------

            Button button = _factory.CreateButton(buttonText);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(button);
            Assert.IsTrue(button.TabStop);
            Assert.AreEqual(buttonText, button.Text);
            Assert.AreEqual(buttonText, button.Name);
            Assert.AreEqual(buttonText, button.Name);
            //To_Test: btn.FlatStyle = FlatStyle.System;
        }

        [Test]
        public void TestCreateControlWithNullAssemblyName()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            Control control = _factory.CreateControl("NumericUpDown", null);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(NumericUpDown), control);
        }

        [Test]
        public void TestCreateTextBox()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            TextBox textBox = _factory.CreateTextBox();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(textBox);
            Assert.IsTrue(textBox.TabStop);
              
        }

        [Test]
        public void TestCreatePasswordTextBox()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            TextBox textBox = _factory.CreatePasswordTextBox();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(textBox);
            Assert.IsTrue(textBox.TabStop);
            Assert.AreEqual('*', textBox.PasswordChar);
              
        }

        [Test]
        public void TestCreateProgressBar()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            ProgressBar progressBar = _factory.CreateProgressBar();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(progressBar);
            Assert.AreEqual(0, progressBar.Minimum);
            Assert.AreEqual(100, progressBar.Maximum);
            Assert.AreEqual(0, progressBar.Value);
              
        }

        [Test]
        public void TestCreatePanel()
        {
            //---------------Set up test pack-------------------
            const string pnlName = "PanelName";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            Panel panelName = _factory.CreatePanel(pnlName, GetControlFactory());
            //---------------Verify Result -----------------------
            Assert.IsNotNull(panelName);
            //Assert.IsTrue(treeView.TabStop);

            Assert.AreEqual(pnlName, panelName.Name);
              
        }

        [Test]
        public void TestCreateUserControl()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            UserControl userControlHabanero = _factory.CreateUserControl();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(userControlHabanero);
              
        }

        [Test]
        public void TestCreateUserControl_WithName()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            UserControl userControlHabanero = _factory.CreateUserControl(name);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(userControlHabanero);
            Assert.AreEqual(name, userControlHabanero.Name);
              
        }

        [Test]
        public void TestCreateDateTimePicker()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            DateTimePicker dateTimePicker = _factory.CreateDateTimePicker();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(dateTimePicker);
            //Assert.IsTrue(treeView.TabStop);
              
        }
                [Test]
        public void TestCreateDateTimePicker_DefaultValue()
        {
            //---------------Set up test pack-------------------
                    DateTime testDate = DateTime.Today.AddDays(-3);
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
                    DateTimePicker dateTimePicker = _factory.CreateDateTimePicker(testDate);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(dateTimePicker);
            Assert.AreEqual(testDate,dateTimePicker.Value);
              
        }

        [Test]
        public void TestCreateMonthPicker()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            DateTimePicker dateTimePicker = _factory.CreateMonthPicker();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(dateTimePicker);
            Assert.AreEqual("MMM yyyy", dateTimePicker.CustomFormat);
              
        }

        [Test]
        public void TestCreateRadioButton()
        {
            //---------------Set up test pack-------------------
            string text = TestUtil.GetRandomString();
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            RadioButton radioButton = _factory.CreateRadioButton(text);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(radioButton);
            Assert.AreEqual(text, radioButton.Text);
            Assert.IsFalse(radioButton.Checked);
              
        }

        [Test]
        public void TestCreateNumericUpDownMoney()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            NumericUpDown upDown = _factory.CreateNumericUpDownCurrency();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(upDown);
            Assert.AreEqual(decimal.MinValue, upDown.Minimum);
            Assert.AreEqual(decimal.MaxValue, upDown.Maximum);
              
        }

        [Test]
        public void TestCreateNumericUpDownInteger()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            NumericUpDown upDown = _factory.CreateNumericUpDownInteger();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(upDown);
            Assert.AreEqual(Int32.MinValue, upDown.Minimum);
            Assert.AreEqual(Int32.MaxValue, upDown.Maximum);
              
        }

        [Test]
        public void TestCreateNumericUpDown()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            NumericUpDown upDown = _factory.CreateNumericUpDown();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(upDown);
              
        }

        [Test]
        public void TestCreateDefaultControl()
        {
            //---------------Set up test pack-------------------
            const string typeName = "";
            const string assemblyName = "";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            Control control = _factory.CreateControl(typeName, assemblyName);
            //---------------Verify Result -----------------------
            Assert.IsTrue(control is TextBox);
              
        }

        [Test]
        public void Test_CreateControl()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Control control = _factory.CreateControl();
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
            Assert.AreEqual(100, control.Width);
            Assert.AreEqual(10, control.Height);
        }


        [Test]
        public void TestCreateInvalidControlType()
        {
            //---------------Set up test pack-------------------
            const string typeName = "GeewizBox";
            const string assemblyName = "SuperDuper.Components";
            //---------------Execute Test ----------------------
            try
            {
                _factory.CreateControl(typeName, assemblyName);
                Assert.Fail("Expected to throw an UnknownTypeNameException");
            }
                //---------------Test Result -----------------------
            catch (UnknownTypeNameException ex)
            {
                StringAssert.Contains("Unable to load the field type while attempting to load a field definition", ex.Message);
            }
              
        }

        [Test]
        public void TestCreateControlMapperStrategy()
        {
            //---------------Set up test pack-------------------
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            _factory.CreateControlMapperStrategy();

            //---------------Test Result -----------------------        
        }
        [Test]
        public virtual void TestCreateDataGridViewColumn_WithTypeName_Image()
        {
            //Not implemented in win
        }


        [Test]
        public virtual void Test_Create_ComboBox()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();  
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ComboBox control = factory.CreateComboBox();
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
            Assert.AreEqual(GetStandardTextBoxHeight(), control.Height);
        }


        
    }

    public static class TestUtil
    {
        public static string GetRandomString()
        {
            return "";
        }

        public static int GetRandomInt(int min, int max)
        {
            return 1;
        }
    }
}
