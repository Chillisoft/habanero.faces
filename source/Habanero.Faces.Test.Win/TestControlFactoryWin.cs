#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using Habanero.Base.Exceptions;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win
{
    /// <summary>
    /// Created for testing purposes
    /// </summary>
    public class CustomDataGridViewColumnWin : System.Windows.Forms.DataGridViewTextBoxColumn
    {
    }

    [TestFixture]
    public class TestControlFactoryWin : TestControlFactory
    {
        [Test]
        public void TestCreateCheckBoxWin()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------

            ICheckBox cbx = GetControlFactory().CreateCheckBox();
            //---------------Test Result -----------------------
            Assert.IsFalse(cbx.Checked);
        }

        [Test]
        public void TestCreateCheckBoxWin_WithDefault()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------

            ICheckBox cbx = GetControlFactory().CreateCheckBox(true);
            //---------------Test Result -----------------------
            Assert.IsTrue(cbx.Checked);

        }

        [Test]
        public void TestCreateControl_ViaType_CreateCombo()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero controlHabanero = _factory.CreateControl(typeof(System.Windows.Forms.ComboBox));
            //---------------Verify Result -----------------------
            Assert.IsNotNull(controlHabanero);
            Assert.AreEqual(typeof(Habanero.Faces.Win.ComboBoxWin), controlHabanero.GetType());
            Assert.AreEqual(GetStandardTextBoxHeight(), controlHabanero.Height);
        }

        [Test]
        public void TestCreateControl_ViaType_CreateEditableGridControl()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero controlHabanero = _factory.CreateControl(typeof(Habanero.Faces.Win.EditableGridControlWin));
            //---------------Verify Result -----------------------
            Assert.IsNotNull(controlHabanero);
            Assert.AreEqual(typeof(Habanero.Faces.Win.EditableGridControlWin), controlHabanero.GetType());

        }

        [Test]
        public void TestCreateControl_ViaType_CreateCheckBox()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero controlHabanero = _factory.CreateControl(typeof(System.Windows.Forms.CheckBox));
            //---------------Verify Result -----------------------
            Assert.IsNotNull(controlHabanero);
            Assert.AreEqual(typeof(Habanero.Faces.Win.CheckBoxWin), controlHabanero.GetType());

        }
        [Test]
        public void TestCreateControl_ViaType_CreateTextBox()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero controlHabanero = _factory.CreateControl(typeof(System.Windows.Forms.TextBox));
            //---------------Verify Result -----------------------
            Assert.IsNotNull(controlHabanero);
            Assert.AreEqual(typeof(Habanero.Faces.Win.TextBoxWin), controlHabanero.GetType());

        }
        [Test]
        public void TestCreateControl_ViaType_CreateListBox()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero controlHabanero = _factory.CreateControl(typeof(System.Windows.Forms.ListBox));
            //---------------Verify Result -----------------------
            Assert.IsNotNull(controlHabanero);
            Assert.AreEqual(typeof(Habanero.Faces.Win.ListBoxWin), controlHabanero.GetType());

        }
        [Test]
        public void TestCreateControl_ViaType_CreateDateTimePicker()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero controlHabanero = _factory.CreateControl(typeof(System.Windows.Forms.DateTimePicker));
            //---------------Verify Result -----------------------
            Assert.IsNotNull(controlHabanero);
            Assert.AreEqual(typeof(Habanero.Faces.Win.DateTimePickerWin), controlHabanero.GetType());

        }

        [Test]
        public void TestCreateControl_ViaType_NumericUpDown()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            object controlHabanero = _factory.CreateControl(typeof(System.Windows.Forms.NumericUpDown));
            //---------------Verify Result -----------------------
            Assert.IsNotNull(controlHabanero);
            Assert.AreEqual(typeof(Habanero.Faces.Win.NumericUpDownWin), controlHabanero.GetType());

        }

        [Test]
        public void TestLoadWithIncorrectControlLibrary_RaisesAppropriateError()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            try
            {
                _factory.CreateControl(typeof(System.String));
                //---------------Verify Result -----------------------
            }
            catch (UnknownTypeNameException ex)
            {
                StringAssert.Contains("The control type name System.String does not inherit from System.Windows.Forms.Control", ex.Message);
            }
        }



        [Test]
        public void TestCreateSpecifiedControlType()
        {
            //---------------Set up test pack-------------------
            const string typeName = "TextBox";
            const string assemblyName = "System.Windows.Forms";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero control = _factory.CreateControl(typeName, assemblyName);
            //---------------Verify Result -----------------------
            Assert.IsTrue(control is System.Windows.Forms.TextBox);

        }

        [Test, Ignore("Not implemented for Win")]
        public override void TestCreateDataGridViewColumn_WithTypeName_Image()
        {
            base.TestCreateDataGridViewColumn_WithTypeName_Image();
        }

        protected override IControlFactory GetControlFactory()
        {
            IControlFactory factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override int GetBoldTextExtraWidth()
        {
            return 10;
        }

        protected Type GetCustomGridColumnType()
        {
            return typeof(CustomDataGridViewColumnWin);
        }

        protected override Type GetMasterGridColumnType()
        {
            return typeof(System.Windows.Forms.DataGridViewColumn);
        }

        protected override Type GetMasterTextBoxGridColumnType()
        {
            return typeof(System.Windows.Forms.DataGridViewTextBoxColumn);
        }

        protected override Type GetHabaneroMasterGridColumnType()
        {
            return typeof(Habanero.Faces.Win.DataGridViewColumnWin);
        }

        protected override string GetUINameSpace()
        {
            return "System.Windows.Forms";
        }

        protected override void AssertGridColumnTypeAfterCast(IDataGridViewColumn createdColumn, Type expectedColumnType)
        {
            Habanero.Faces.Win.DataGridViewColumnWin columnWin = (Habanero.Faces.Win.DataGridViewColumnWin)createdColumn;
            System.Windows.Forms.DataGridViewColumn column = columnWin.DataGridViewColumn;
            Assert.AreEqual(expectedColumnType, column.GetType());
        }

        [Test]
        public void TestCreateDataGridViewColumn_SpecifyNameAndAssembly()
        {
            //---------------Set up test pack-------------------
            Type columnType = GetCustomGridColumnType();
            string typeName = columnType.Name;  //"CustomDataGridViewColumn";
            const string assemblyName = "Habanero.Faces.Test.Win";
            //---------------Assert Precondition----------------
            Assert.IsTrue(columnType.IsSubclassOf(GetMasterGridColumnType()));
            //---------------Execute Test ----------------------
            IDataGridViewColumn column = GetControlFactory().CreateDataGridViewColumn(typeName, assemblyName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(column);
            Assert.IsInstanceOf(GetHabaneroMasterGridColumnType(), column);
            AssertGridColumnTypeAfterCast(column, columnType);
        }
        [Test]
        public void TestCreateDataGridViewColumn_SpecifyType()
        {
            //---------------Set up test pack-------------------
            Type columnType = GetCustomGridColumnType();
            //---------------Assert Precondition----------------
            Assert.IsTrue(columnType.IsSubclassOf(GetMasterGridColumnType()));
            //---------------Execute Test ----------------------
            IDataGridViewColumn column = GetControlFactory().CreateDataGridViewColumn(columnType);
            //---------------Test Result -----------------------
            Assert.IsNotNull(column);
            Assert.IsInstanceOf(GetHabaneroMasterGridColumnType(), column);
            AssertGridColumnTypeAfterCast(column, columnType);
        }

        [Test]
        public void TestCreateDataGridViewColumn_DefaultAssembly()
        {
            //---------------Set up test pack-------------------
            const string typeName = "DataGridViewCheckBoxColumn";
            //---------------Assert Precondition----------------
            Assert.IsTrue(typeName.Contains("DataGridViewCheckBoxColumn"));
            //---------------Execute Test ----------------------
            object column = GetControlFactory().CreateDataGridViewColumn(typeName, null);
            //---------------Test Result -----------------------
            Assert.IsNotNull(column);
            Assert.IsInstanceOf(typeof(IDataGridViewCheckBoxColumn), column);

            string correctAssembly = GetControlFactory().CreateDataGridViewCheckBoxColumn().GetType().AssemblyQualifiedName;
            Assert.AreEqual(correctAssembly, column.GetType().AssemblyQualifiedName);
        }
    }
}