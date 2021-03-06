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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Base.Util;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base.UIHints;
using Habanero.Faces.Win;
using Habanero.Test;
using Habanero.Test.BO;
using Habanero.Faces.Base;
using Habanero.Test.Structure;
using NUnit.Framework;
using Rhino.Mocks;

// ReSharper disable InconsistentNaming
namespace Habanero.Faces.Test.Base
{
    [TestFixture]
    public abstract class TestControlFactory : TestBaseWithDisposing
    {
        protected IControlFactory _factory;

        [SetUp]
        public void TestSetup()
        {
            GlobalUIRegistry.UIStyleHints = null;
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
        public void Test_CreateMainMenu()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMainMenuHabanero mmenu = _factory.CreateMainMenu();
            DisposeOnTearDown(mmenu);
            //---------------Test Result -----------------------
            Assert.IsNotNull(mmenu);
            TestUtil.AssertStringEmpty(mmenu.Name, "mmenu.Name");
        }

        [Test]
        public void Test_CreateMenuItem()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();            
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMenuItem mmenu = _factory.CreateMenuItem(name);
            DisposeOnTearDown(mmenu);
            //---------------Test Result -----------------------
            Assert.IsNotNull(mmenu);
            Assert.AreEqual(name, mmenu.Text);
        }

        [Test]
        public void Test_CreateMenuItem_FromHabaneroMenuItem()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu.Item item = new HabaneroMenu.Item(null, TestUtil.GetRandomString());
            DisposeOnTearDown(item);     
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMenuItem mmenu = _factory.CreateMenuItem(item);
            DisposeOnTearDown(mmenu);
            //---------------Test Result -----------------------
            Assert.IsNotNull(mmenu);
            Assert.AreEqual(item.Name, mmenu.Text);
        }

        [Test]
        public void TestCreateLabel_NoText()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            ILabel lbl = _factory.CreateLabel();
            DisposeOnTearDown(lbl);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(lbl);
            Assert.IsFalse(lbl.TabStop);
        }

        [Test]
        public void TestCreateLabel_Text()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            const string labelText = "test label";
            //---------------Execute Test ----------------------

            ILabel lbl = _factory.CreateLabel(labelText);
            DisposeOnTearDown(lbl);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(lbl);
            Assert.IsFalse(lbl.TabStop);
            Assert.AreEqual(labelText, lbl.Text);
            Assert.AreNotEqual(labelText, lbl.Name);
            if (!(GetControlFactory() is ControlFactoryWin))
                Assert.AreEqual(lbl.PreferredWidth, lbl.Width);
            //TODO_Port_DoTest lbl.FlatStyle = FlatStyle.Standard;
        }

        [Test]
        public void TestCreateLabel_BoldText()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            const string labelText = "test label";
            //---------------Execute Test ----------------------

            ILabel lbl = _factory.CreateLabel(labelText, true);
            DisposeOnTearDown(lbl);
            //---------------Verify Result -----------------------
            //Assert.AreEqual(lbl.PreferredWidth + 10, lbl.Width);
            if (!(GetControlFactory() is ControlFactoryWin))    // windows labels are auto-sized
                Assert.AreEqual(lbl.PreferredWidth + GetBoldTextExtraWidth(), lbl.Width);
            Font expectedFont = new Font(lbl.Font, FontStyle.Bold);
            Assert.AreEqual(expectedFont, lbl.Font);
              
        }

        [Test]
        public void TestCreateButton()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            const string buttonText = "test label";
            //---------------Execute Test ----------------------

            IButton button = _factory.CreateButton(buttonText);
            DisposeOnTearDown(button);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(button);
            Assert.IsTrue(button.TabStop);
            Assert.AreEqual(buttonText, button.Text);
            Assert.AreEqual(buttonText, button.Name);
            int expectedButtonWidth = _factory.CreateLabel(buttonText).PreferredWidth + 20;
            Assert.AreEqual(buttonText, button.Name);
            Assert.AreEqual(expectedButtonWidth, button.Width);
            //To_Test: btn.FlatStyle = FlatStyle.System;
        }

        [Test]
        public void TestCreateButton_ObservesGlobalUIMinHints()
        {
            //---------------Set up test pack-------------------
            GlobalUIRegistry.UIStyleHints = new UIStyleHints();
            var defaultButton = GetControlFactory().CreateButton();
            var hints = GlobalUIRegistry.UIStyleHints.ButtonHints;
            hints.MinimumHeight = defaultButton.Height + RandomValueGen.GetRandomInt(10, 20);
            hints.MinimumWidth = defaultButton.Width + RandomValueGen.GetRandomInt(10, 20);
            hints.MinimumFontSize = defaultButton.Font.Size + 2;
            var newButton = GetControlFactory().CreateButton();
            DisposeOnTearDown(newButton);
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.That(newButton.Width, Is.GreaterThanOrEqualTo(hints.MinimumWidth), "UI width hint not observed");
            Assert.That(newButton.Height, Is.GreaterThanOrEqualTo(hints.MinimumHeight), "UI width hint not observed");
            Assert.That(newButton.Font.Size, Is.GreaterThanOrEqualTo(hints.MinimumFontSize), "UI font size hint not observed");
        }

        [Test]
        public void TestCreateButton_ObservesGlobalUIMaxHints()
        {
            //---------------Set up test pack-------------------
            GlobalUIRegistry.UIStyleHints = new UIStyleHints();
            var hints = GlobalUIRegistry.UIStyleHints.ButtonHints;
            hints.MaximumHeight = RandomValueGen.GetRandomInt(30, 40);
            hints.MaximumWidth = RandomValueGen.GetRandomInt(60, 100);
            var btn = GetControlFactory().CreateButton();
            DisposeOnTearDown(btn);
            btn.Width = hints.MaximumWidth + RandomValueGen.GetRandomInt(10, 20);
            btn.Height = hints.MaximumHeight + RandomValueGen.GetRandomInt(10, 20);
            
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.That(btn.Width, Is.LessThanOrEqualTo(hints.MaximumWidth), "UI width hint not observed");
            Assert.That(btn.Height, Is.LessThanOrEqualTo(hints.MaximumHeight), "UI height hint not observed");
        }

        [Test]
        public void Test_ConstructLayoutManager_ObservesUIHints()
        {
            //---------------Set up test pack-------------------
            var defaultHorizontalGap = RandomValueGen.GetRandomInt(10, 20);
            var defaultVerticalGap = RandomValueGen.GetRandomInt(10, 20);
            var defaultBorderSize = RandomValueGen.GetRandomInt(10, 20);
            GlobalUIRegistry.UIStyleHints = new UIStyleHints() 
            { 
                LayoutHints = new LayoutHints() 
                { 
                    DefaultHorizontalGap = defaultHorizontalGap,
                    DefaultVerticalGap = defaultVerticalGap,
                    DefaultBorderSize = defaultBorderSize
                }
            };
            var ctl = GetControlFactory().CreateControl();
            DisposeOnTearDown(ctl);
            var manager = new GridLayoutManager(ctl, GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.AreEqual(manager.HorizontalGapSize, defaultHorizontalGap, "default horizontal gap ui hint not observed");
            Assert.AreEqual(manager.VerticalGapSize, defaultVerticalGap, "default vertical gap ui hint not observed");
            Assert.AreEqual(manager.BorderSize, defaultBorderSize, "default border size ui hint not observed");
        }

        [Test]
        public void Test_DatePicker_ObservesMinUIHints()
        {
            //---------------Set up test pack-------------------
            var defaultcontrol = GetControlFactory().CreateDateTimePicker();
            GlobalUIRegistry.UIStyleHints = new UIStyleHints();
            var hints = GlobalUIRegistry.UIStyleHints.DateTimePickerHints;
            hints.MinimumHeight = defaultcontrol.Height + RandomValueGen.GetRandomInt(10, 20);
            hints.MinimumWidth = defaultcontrol.Width + RandomValueGen.GetRandomInt(10, 20);
            var newcontrol = GetControlFactory().CreateDateTimePicker();
            DisposeOnTearDown(newcontrol);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.That(newcontrol.Width, Is.GreaterThanOrEqualTo(hints.MinimumWidth), "UI width hint not observed");
            Assert.That(newcontrol.Height, Is.GreaterThanOrEqualTo(hints.MinimumHeight), "UI width hint not observed");
            Assert.That(newcontrol.Font.Size, Is.GreaterThanOrEqualTo(hints.MinimumFontSize), "UI font size hint not observed");
        }

        [Test]
        public void Test_DatePicker_ObservesMaxUIHints()
        {
            //---------------Set up test pack-------------------
            var defaultcontrol = GetControlFactory().CreateDateTimePicker();
            DisposeOnTearDown(defaultcontrol);
            GlobalUIRegistry.UIStyleHints = new UIStyleHints();
            var hints = GlobalUIRegistry.UIStyleHints.DateTimePickerHints;
            hints.MaximumHeight = defaultcontrol.Height + RandomValueGen.GetRandomInt(10, 20);
            hints.MaximumWidth = defaultcontrol.Width + RandomValueGen.GetRandomInt(10, 20);
            var newcontrol = GetControlFactory().CreateDateTimePicker();
            DisposeOnTearDown(newcontrol);
            newcontrol.Width = hints.MaximumWidth + 10;
            newcontrol.Height = hints.MaximumHeight + 10;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.That(newcontrol.Width, Is.LessThanOrEqualTo(hints.MaximumWidth), "UI width hint not observed");
            Assert.That(newcontrol.Height, Is.LessThanOrEqualTo(hints.MaximumHeight), "UI width hint not observed");
        }

        [Test]
        public void Test_Label_ObservesMinUIHints()
        {
            //---------------Set up test pack-------------------
            var defaultcontrol = GetControlFactory().CreateLabel();
            DisposeOnTearDown(defaultcontrol);
            GlobalUIRegistry.UIStyleHints = new UIStyleHints();
            var hints = GlobalUIRegistry.UIStyleHints.LabelHints;
            hints.MinimumHeight = defaultcontrol.Height + RandomValueGen.GetRandomInt(10, 20);
            hints.MinimumWidth = defaultcontrol.Width + RandomValueGen.GetRandomInt(10, 20);
            var newcontrol = GetControlFactory().CreateLabel();
            DisposeOnTearDown(newcontrol);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.That(newcontrol.Width, Is.GreaterThanOrEqualTo(hints.MinimumWidth), "UI width hint not observed");
            Assert.That(newcontrol.Height, Is.GreaterThanOrEqualTo(hints.MinimumHeight), "UI width hint not observed");
            Assert.That(newcontrol.Font.Size, Is.GreaterThanOrEqualTo(hints.MinimumFontSize), "UI font size hint not observed");
        }

        [Test]
        public void Test_Label_ObservesMaxUIHints()
        {
            //---------------Set up test pack-------------------
            var defaultcontrol = GetControlFactory().CreateLabel();
            DisposeOnTearDown(defaultcontrol);
            GlobalUIRegistry.UIStyleHints = new UIStyleHints();
            var hints = GlobalUIRegistry.UIStyleHints.LabelHints;
            hints.MaximumHeight = defaultcontrol.Height + RandomValueGen.GetRandomInt(10, 20);
            hints.MaximumWidth = defaultcontrol.Width + RandomValueGen.GetRandomInt(10, 20);
            var newcontrol = GetControlFactory().CreateLabel();
            DisposeOnTearDown(newcontrol);
            newcontrol.Width = hints.MaximumWidth + 10;
            newcontrol.Height = hints.MaximumHeight + 10;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.That(newcontrol.Width, Is.LessThanOrEqualTo(hints.MaximumWidth), "UI width hint not observed");
            Assert.That(newcontrol.Height, Is.LessThanOrEqualTo(hints.MaximumHeight), "UI width hint not observed");
        }

        [Test]
        public void Test_CheckBox_ObservesMinUIHints()
        {
            //---------------Set up test pack-------------------
            var defaultcontrol = GetControlFactory().CreateCheckBox();
            DisposeOnTearDown(defaultcontrol);
            GlobalUIRegistry.UIStyleHints = new UIStyleHints();
            var hints = GlobalUIRegistry.UIStyleHints.CheckBoxHints;
            hints.MinimumHeight = defaultcontrol.Height + RandomValueGen.GetRandomInt(10, 20);
            hints.MinimumWidth = defaultcontrol.Width + RandomValueGen.GetRandomInt(10, 20);
            var newcontrol = GetControlFactory().CreateCheckBox();
            DisposeOnTearDown(newcontrol);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.That(newcontrol.Width, Is.GreaterThanOrEqualTo(hints.MinimumWidth), "UI width hint not observed");
            Assert.That(newcontrol.Height, Is.GreaterThanOrEqualTo(hints.MinimumHeight), "UI width hint not observed");
            Assert.That(newcontrol.Font.Size, Is.GreaterThanOrEqualTo(hints.MinimumFontSize), "UI font size hint not observed");
        }

        [Test]
        public void Test_CheckBox_ObservesMaxUIHints()
        {
            //---------------Set up test pack-------------------
            var defaultcontrol = GetControlFactory().CreateCheckBox();
            DisposeOnTearDown(defaultcontrol);
            GlobalUIRegistry.UIStyleHints = new UIStyleHints();
            var hints = GlobalUIRegistry.UIStyleHints.CheckBoxHints;
            hints.MaximumHeight = defaultcontrol.Height + RandomValueGen.GetRandomInt(10, 20);
            hints.MaximumWidth = defaultcontrol.Width + RandomValueGen.GetRandomInt(10, 20);
            var newcontrol = GetControlFactory().CreateCheckBox();
            DisposeOnTearDown(newcontrol);
            newcontrol.Width = hints.MaximumWidth + 10;
            newcontrol.Height = hints.MaximumHeight + 10;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.That(newcontrol.Width, Is.LessThanOrEqualTo(hints.MaximumWidth), "UI width hint not observed");
            Assert.That(newcontrol.Height, Is.LessThanOrEqualTo(hints.MaximumHeight), "UI width hint not observed");
        }

        [Test]
        public void Test_TextBox_ObservesMinUIHints()
        {
            //---------------Set up test pack-------------------
            var defaultcontrol = GetControlFactory().CreateTextBox();
            DisposeOnTearDown(defaultcontrol);
            GlobalUIRegistry.UIStyleHints = new UIStyleHints();
            var hints = GlobalUIRegistry.UIStyleHints.TextBoxHints;
            hints.MinimumHeight = defaultcontrol.Height + RandomValueGen.GetRandomInt(10, 20);
            hints.MinimumWidth = defaultcontrol.Width + RandomValueGen.GetRandomInt(10, 20);
            var newcontrol = GetControlFactory().CreateTextBox();
            DisposeOnTearDown(newcontrol);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.That(newcontrol.Width, Is.GreaterThanOrEqualTo(hints.MinimumWidth), "UI width hint not observed");
            Assert.That(newcontrol.Height, Is.GreaterThanOrEqualTo(hints.MinimumHeight), "UI width hint not observed");
            Assert.That(newcontrol.Font.Size, Is.GreaterThanOrEqualTo(hints.MinimumFontSize), "UI font size hint not observed");
        }

        [Test]
        public void Test_TextBox_ObservesMaxUIHints()
        {
            //---------------Set up test pack-------------------
            var defaultcontrol = GetControlFactory().CreateTextBox();
            DisposeOnTearDown(defaultcontrol);
            GlobalUIRegistry.UIStyleHints = new UIStyleHints();
            var hints = GlobalUIRegistry.UIStyleHints.TextBoxHints;
            hints.MaximumHeight = defaultcontrol.Height + RandomValueGen.GetRandomInt(10, 20);
            hints.MaximumWidth = defaultcontrol.Width + RandomValueGen.GetRandomInt(10, 20);
            var newcontrol = GetControlFactory().CreateTextBox();
            DisposeOnTearDown(newcontrol);
            newcontrol.Width = hints.MaximumWidth + 10;
            newcontrol.Height = hints.MaximumHeight + 10;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.That(newcontrol.Width, Is.LessThanOrEqualTo(hints.MaximumWidth), "UI width hint not observed");
            Assert.That(newcontrol.Height, Is.LessThanOrEqualTo(hints.MaximumHeight), "UI width hint not observed");
        }

        [Test]
        public void Test_ComboBox_ObservesMinUIHints()
        {
            //---------------Set up test pack-------------------
            var defaultcontrol = GetControlFactory().CreateComboBox();
            DisposeOnTearDown(defaultcontrol);
            GlobalUIRegistry.UIStyleHints = new UIStyleHints();
            var hints = GlobalUIRegistry.UIStyleHints.ComboBoxHints;
            hints.MinimumHeight = defaultcontrol.Height + RandomValueGen.GetRandomInt(10, 20);
            hints.MinimumWidth = defaultcontrol.Width + RandomValueGen.GetRandomInt(10, 20);
            hints.AllowTextEditing = false;
            var newcontrol = GetControlFactory().CreateComboBox();
            DisposeOnTearDown(newcontrol);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.That(newcontrol.Width, Is.GreaterThanOrEqualTo(hints.MinimumWidth), "UI width hint not observed");
            // combobox height constraints are set by the containing text, apparently
            Assert.That(newcontrol.Font.Size, Is.GreaterThanOrEqualTo(hints.MinimumFontSize), "UI font size hint not observed");
            var cmbwin = newcontrol as Habanero.Faces.Win.ComboBoxWin;
            if (cmbwin != null)
            {
                Assert.AreEqual(cmbwin.DropDownStyle, System.Windows.Forms.ComboBoxStyle.DropDownList, "allow text edit hint ignored");
            }
            var cmbvwg = newcontrol as Habanero.Faces.VWG.ComboBoxVWG;
            if (cmbvwg != null)
            {
                Assert.AreEqual(cmbvwg.DropDownStyle, Gizmox.WebGUI.Forms.ComboBoxStyle.DropDownList, "allow text edit hint ignored");
            }
        }

        [Test]
        public void Test_ComboBox_ObservesMaxUIHints()
        {
            //---------------Set up test pack-------------------
            var defaultcontrol = GetControlFactory().CreateLabel();
            DisposeOnTearDown(defaultcontrol);
            GlobalUIRegistry.UIStyleHints = new UIStyleHints();
            var hints = GlobalUIRegistry.UIStyleHints.LabelHints;
            hints.MaximumHeight = defaultcontrol.Height + RandomValueGen.GetRandomInt(10, 20);
            hints.MaximumWidth = defaultcontrol.Width + RandomValueGen.GetRandomInt(10, 20);
            var newcontrol = GetControlFactory().CreateLabel();
            DisposeOnTearDown(newcontrol);
            newcontrol.Width = hints.MaximumWidth + 10;
            newcontrol.Height = hints.MaximumHeight + 10;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.That(newcontrol.Width, Is.LessThanOrEqualTo(hints.MaximumWidth), "UI width hint not observed");
            Assert.That(newcontrol.Height, Is.LessThanOrEqualTo(hints.MaximumHeight), "UI width hint not observed");
        }

        [Test]
        public void Test_SetAllControlHints_SetsAllControlHints()
        {
            //---------------Set up test pack-------------------
            var template = new ControlHints()
            {
                MaximumHeight = RandomValueGen.GetRandomInt(100, 150),
                MinimumHeight = RandomValueGen.GetRandomInt(50, 100),
                MinimumWidth = RandomValueGen.GetRandomInt(50, 100),
                MaximumWidth = RandomValueGen.GetRandomInt(100, 150),
                DefaultFontFamily = new FontFamily("Wingdings"),
                MinimumFontSize = (float)RandomValueGen.GetRandomDecimal(10, 20)
            };
            GlobalUIRegistry.UIStyleHints = new UIStyleHints();
            GlobalUIRegistry.UIStyleHints.SetAllControlHints(template);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            var test = new ControlHints[] {
                GlobalUIRegistry.UIStyleHints.ButtonHints,
                GlobalUIRegistry.UIStyleHints.LabelHints,
                GlobalUIRegistry.UIStyleHints.TextBoxHints,
                GlobalUIRegistry.UIStyleHints.DateTimePickerHints,
                GlobalUIRegistry.UIStyleHints.ComboBoxHints,
                GlobalUIRegistry.UIStyleHints.CheckBoxHints
            };
            foreach (var t in test)
            {
                Assert.That(t.Equals(template));
            }
        }

        [Test]
        public void TestCreateGroupBox()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IGroupBox groupBox = _factory.CreateGroupBox();
            DisposeOnTearDown(groupBox);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(groupBox);
            Assert.AreEqual("", groupBox.Text);
            Assert.AreEqual("", groupBox.Name);
            const int expectedWidth = 200;
            Assert.AreEqual(expectedWidth, groupBox.Width);
        }

        [Test]
        public void TestCreateGroupBox_WithText()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            const string groupBoxText = "test label";
            //---------------Execute Test ----------------------

            IGroupBox groupBox = _factory.CreateGroupBox(groupBoxText);
            DisposeOnTearDown(groupBox);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(groupBox);
            Assert.AreEqual(groupBoxText, groupBox.Text);
            Assert.AreEqual(groupBoxText, groupBox.Name);
            const int expectedWidth = 200;
            Assert.AreEqual(groupBoxText, groupBox.Name);
            Assert.AreEqual(expectedWidth, groupBox.Width);
              
        }

        [Test]
        public void TestCreateDataGridView()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            
            //---------------Execute Test ----------------------

            IDataGridView dataGridView = _factory.CreateDataGridView();
            DisposeOnTearDown(dataGridView);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(dataGridView);
              
        }

        [Test]
        public void TestCreateBOEditorForm_BoParam()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO businessObject = new MyBO();
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IDefaultBOEditorForm boEditorForm = _factory.CreateBOEditorForm(businessObject);
            DisposeOnTearDown(boEditorForm);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(boEditorForm);
            Assert.AreSame(businessObject, boEditorForm.PanelInfo.BusinessObject);
        }

        [Test]
        public void TestCreateBOEditorForm_BoParam_UiDefNameParam()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO businessObject = new MyBO();
            const string uiDefName = "Alternate";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IDefaultBOEditorForm boEditorForm = _factory.CreateBOEditorForm(businessObject, uiDefName);
            DisposeOnTearDown(boEditorForm);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(boEditorForm);
            Assert.AreSame(businessObject, boEditorForm.PanelInfo.BusinessObject);
            Assert.AreEqual(uiDefName, boEditorForm.PanelInfo.UIForm.UIDef.Name);
        }

        [Test]
        public void TestCreateBOEditorForm_BoParam_UiDefNameParam_PostObjectPersistingParam()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO businessObject = new MyBO();
            const string uiDefName = "Alternate";
            PostObjectEditDelegate action = delegate {  };
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IDefaultBOEditorForm boEditorForm = _factory.CreateBOEditorForm(businessObject, uiDefName, action);
            DisposeOnTearDown(boEditorForm);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(boEditorForm);
            Assert.AreSame(businessObject, boEditorForm.PanelInfo.BusinessObject);
            Assert.AreEqual(uiDefName, boEditorForm.PanelInfo.UIForm.UIDef.Name);
        }

        [Test]
        public void TestCreateBOEditorForm_BoParam_UiDefNameParam_GroupControlCreator()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO businessObject = new MyBO();
            const string uiDefName = "Alternate";
            GroupControlCreator controlCreator = GetControlFactory().CreateTabControl;
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IDefaultBOEditorForm boEditorForm = _factory.CreateBOEditorForm(businessObject, uiDefName, controlCreator);
            DisposeOnTearDown(boEditorForm);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(boEditorForm);
            Assert.AreSame(businessObject, boEditorForm.PanelInfo.BusinessObject);
            Assert.AreEqual(uiDefName, boEditorForm.PanelInfo.UIForm.UIDef.Name);
            Assert.AreSame(controlCreator, boEditorForm.GroupControlCreator);
        }

        [Test]
        public void TestCreateControlWithNullAssemblyName()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero control = _factory.CreateControl("NumericUpDown", null);
            DisposeOnTearDown(control);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(INumericUpDown), control);
        }

        [Test]
        public void TestCreateButton_WithEventHandler()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            const string buttonText = "test label";
            bool buttonClicked = false;
            EventHandler handler = delegate { buttonClicked = true; };

            //---------------Execute Test ----------------------

            IButton button = _factory.CreateButton(buttonText, handler);
            DisposeOnTearDown(button);

            button.PerformClick();
            //---------------Verify Result -----------------------
            Assert.IsNotNull(button);
            Assert.IsTrue(button.TabStop);
            Assert.AreEqual(buttonText, button.Text);
            Assert.AreEqual(buttonText, button.Name);
            int expectedButtonWidth = _factory.CreateLabel(buttonText).PreferredWidth + 20;
            Assert.AreEqual(buttonText, button.Name);
            Assert.AreEqual(expectedButtonWidth, button.Width);
            Assert.IsTrue(buttonClicked);
            //To_Test: btn.FlatStyle = FlatStyle.System;
              
        }

        [Test]
        public void TestCreateTextBox()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            ITextBox textBox = _factory.CreateTextBox();
            DisposeOnTearDown(textBox);
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
            ITextBox textBox = _factory.CreatePasswordTextBox();
            DisposeOnTearDown(textBox);
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
            IProgressBar progressBar = _factory.CreateProgressBar();
            DisposeOnTearDown(progressBar);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(progressBar);
            Assert.AreEqual(0, progressBar.Minimum);
            Assert.AreEqual(100, progressBar.Maximum);
            Assert.AreEqual(10, progressBar.Step);
            Assert.AreEqual(0, progressBar.Value);
              
        }

        [Test]
        public void TestCreateTreeView()
        {
            //---------------Set up test pack-------------------
            const string treeViewname = "TVNAme";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            ITreeView treeView = _factory.CreateTreeView(treeViewname);
            DisposeOnTearDown(treeView);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(treeView);
            Assert.IsTrue(treeView.TabStop);
            Assert.AreEqual(treeViewname, treeView.Name);
              
        }

        [Test]
        public void TestCreateTreeNode()
        {
            //---------------Set up test pack-------------------
            const string treeNodeName = "TVNodeName";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            ITreeNode treeNode = _factory.CreateTreeNode(treeNodeName);
            DisposeOnTearDown(treeNode);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(treeNode);
            Assert.AreEqual(treeNodeName, treeNode.Text);
              
        }

        [Test]
        public void TestCreatePanel()
        {
            //---------------Set up test pack-------------------
            const string pnlName = "PanelName";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IPanel panel = _factory.CreatePanel(pnlName, GetControlFactory());
            DisposeOnTearDown(panel);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(panel);
            //Assert.IsTrue(treeView.TabStop);

            Assert.AreEqual(pnlName, panel.Name);
              
        }

        [Test]
        public void TestCreateUserControl()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IUserControlHabanero userControlHabanero = _factory.CreateUserControl();
            DisposeOnTearDown(userControlHabanero);
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
            IUserControlHabanero userControlHabanero = _factory.CreateUserControl(name);
            DisposeOnTearDown(userControlHabanero);
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
            IDateTimePicker dateTimePicker = _factory.CreateDateTimePicker();
            DisposeOnTearDown(dateTimePicker);
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
                    IDateTimePicker dateTimePicker = _factory.CreateDateTimePicker(testDate);
                    DisposeOnTearDown(dateTimePicker);
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
            IDateTimePicker dateTimePicker = _factory.CreateMonthPicker();
            DisposeOnTearDown(dateTimePicker);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(dateTimePicker);
            Assert.AreEqual("MMM yyyy", dateTimePicker.CustomFormat);
              
        }

        [Test]
        public void TestCreateRadioButton()
        {
            //---------------Set up test pack-------------------
            string text = TestUtil.GetRandomString();
            int expectedWidth = GetControlledLifetimeFor(_factory.CreateLabel(text, false)).PreferredWidth + 25;
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IRadioButton radioButton = _factory.CreateRadioButton(text);
            DisposeOnTearDown(radioButton);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(radioButton);
            Assert.AreEqual(text, radioButton.Text);
            Assert.AreEqual(expectedWidth, radioButton.Width);
            Assert.IsFalse(radioButton.Checked);
              
        }

        [Test]
        public void TestCreateNumericUpDownMoney()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            INumericUpDown upDown = _factory.CreateNumericUpDownCurrency();
            DisposeOnTearDown(upDown);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(upDown);
            Assert.AreEqual(2, upDown.DecimalPlaces);
            Assert.AreEqual(decimal.MinValue, upDown.Minimum);
            Assert.AreEqual(decimal.MaxValue, upDown.Maximum);
              
        }

        [Test]
        public void TestCreateNumericUpDownInteger()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            INumericUpDown upDown = _factory.CreateNumericUpDownInteger();
            DisposeOnTearDown(upDown);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(upDown);
            Assert.AreEqual(0, upDown.DecimalPlaces);
            Assert.AreEqual(Int32.MinValue, upDown.Minimum);
            Assert.AreEqual(Int32.MaxValue, upDown.Maximum);
              
        }

        [Test]
        public void TestCreateNumericUpDown()
        {
            //---------------Set up test pack-------------------
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            INumericUpDown upDown = _factory.CreateNumericUpDown();
            DisposeOnTearDown(upDown);
            //---------------Verify Result -----------------------
            Assert.IsNotNull(upDown);
            Assert.AreEqual(0, upDown.DecimalPlaces);
              
        }

        [Test]
        public void TestCreateDefaultControl()
        {
            //---------------Set up test pack-------------------
            const string typeName = "";
            const string assemblyName = "";
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero control = _factory.CreateControl(typeName, assemblyName);
            DisposeOnTearDown(control);
            //---------------Verify Result -----------------------
            Assert.IsTrue(control is ITextBox);
              
        }

        [Test]
        public void Test_CreateControl()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IControlHabanero control = _factory.CreateControl();
            DisposeOnTearDown(control);
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
        public void TestCreateDateRangeComboBox()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            IDateRangeComboBox comboBox = GetControlFactory().CreateDateRangeComboBox();
            DisposeOnTearDown(comboBox);
            //---------------Test Result -----------------------
            Assert.IsNotNull(comboBox);
            Assert.AreEqual(15, comboBox.Items.Count);       // Includes blank option in the list
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCreateDateRangeComboBoxOverload()
        {
            //---------------Set up test pack-------------------
            List<DateRangeOptions> options = new List<DateRangeOptions>();
            options.Add(DateRangeOptions.Today);
            options.Add(DateRangeOptions.Yesterday);

            //---------------Execute Test ----------------------
            IDateRangeComboBox comboBox = GetControlFactory().CreateDateRangeComboBox(options);
            DisposeOnTearDown(comboBox);
            //---------------Test Result -----------------------
            Assert.IsNotNull(comboBox);
            Assert.IsFalse(comboBox.IgnoreTime);
            Assert.IsFalse(comboBox.UseFixedNowDate);
            Assert.AreEqual(0, comboBox.WeekStartOffset);
            Assert.AreEqual(0, comboBox.MonthStartOffset);
            Assert.AreEqual(0, comboBox.YearStartOffset);
            Assert.AreEqual(3, comboBox.Items.Count);          // Includes blank option in the list
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestCreateDataGridViewColumn_NullTypeNameAssumesDefault()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IDataGridViewColumn column = GetControlFactory().CreateDataGridViewColumn(null, null);
            DisposeOnTearDown(column);
            //---------------Test Result -----------------------
            Type expectedHabaneroType = GetHabaneroMasterGridColumnType();
            Type expectedMasterType = GetMasterTextBoxGridColumnType();
            Assert.AreEqual(expectedHabaneroType, column.GetType());
            AssertGridColumnTypeAfterCast(column, expectedMasterType);
        }

        [Test]
        public void TestCreateDataGridViewColumn_InvalidTypeNameThrowsException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool errorThrown = false;
            try
            {
                GetControlFactory().CreateDataGridViewColumn("InvalidColumnType", null);
            }
            catch (UnknownTypeNameException)
            {
                errorThrown = true;
            }
            //---------------Test Result -----------------------
            Assert.IsTrue(errorThrown, "Not specifying a type name should throw an exception (defaults must be set further up the chain)");
        }

        [Test]
        public void TestCreateDataGridViewColumn_ExceptionIfDoesNotImplementIDataGridViewColumn()
        {
            //---------------Set up test pack-------------------
            Type wrongType = typeof (MyBO);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool errorThrown = false;
            try
            {
                GetControlFactory().CreateDataGridViewColumn(wrongType);
            }
            catch (UnknownTypeNameException)
            {
                errorThrown = true;
            }
            //---------------Test Result -----------------------
            Assert.IsTrue(errorThrown, "Type must inherit from IDataGridViewColumn");
        }

        [Test]
        public virtual void TestCreateDataGridViewColumn_WithTypeName_NumericUpDown()
        {
            //---------------Set up test pack-------------------
            IDataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn = GetControlFactory().CreateDataGridViewNumericUpDownColumn();
            DisposeOnTearDown(dataGridViewNumericUpDownColumn);
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            IDataGridViewColumn dataGridViewColumn = GetControlFactory().
                CreateDataGridViewColumn("DataGridViewNumericUpDownColumn", null);
            DisposeOnTearDown(dataGridViewColumn);
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataGridViewColumn);
            Assert.IsInstanceOf(typeof(IDataGridViewNumericUpDownColumn), dataGridViewColumn);
            Assert.AreSame(dataGridViewNumericUpDownColumn.GetType(), dataGridViewColumn.GetType());
        }

        [Test]
        public virtual void TestCreateDataGridViewColumn_WithTypeName_DateTimePicker()
        {
            //---------------Set up test pack-------------------
            IDataGridViewDateTimeColumn dataGridViewNumericUpDownColumn = GetControlFactory().CreateDataGridViewDateTimeColumn();
            DisposeOnTearDown(dataGridViewNumericUpDownColumn);
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            IDataGridViewColumn dataGridViewColumn = GetControlFactory().
                CreateDataGridViewColumn("DataGridViewDateTimeColumn", null);
            DisposeOnTearDown(dataGridViewColumn);
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataGridViewColumn);
            Assert.IsInstanceOf(typeof(IDataGridViewDateTimeColumn), dataGridViewColumn);
            Assert.AreSame(dataGridViewNumericUpDownColumn.GetType(), dataGridViewColumn.GetType());
        }

        [Test]
        public virtual void TestCreateDataGridViewColumn_WithTypeName_CheckBox()
        {
            //---------------Set up test pack-------------------
            IDataGridViewCheckBoxColumn dataGridViewNumericUpDownColumn = GetControlFactory().CreateDataGridViewCheckBoxColumn();
            DisposeOnTearDown(dataGridViewNumericUpDownColumn);
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            IDataGridViewColumn dataGridViewColumn = GetControlFactory().
                CreateDataGridViewColumn("DataGridViewCheckBoxColumn", null);
            DisposeOnTearDown(dataGridViewColumn);
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataGridViewColumn);
            Assert.IsInstanceOf(typeof(IDataGridViewCheckBoxColumn), dataGridViewColumn);
            Assert.AreSame(dataGridViewNumericUpDownColumn.GetType(), dataGridViewColumn.GetType());
        }

        [Test]
        public virtual void TestCreateDataGridViewColumn_WithTypeName_ComboBox()
        {
            //---------------Set up test pack-------------------
            IDataGridViewComboBoxColumn dataGridViewNumericUpDownColumn = GetControlFactory().CreateDataGridViewComboBoxColumn();
            DisposeOnTearDown(dataGridViewNumericUpDownColumn);
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            IDataGridViewColumn dataGridViewColumn = GetControlFactory().
                CreateDataGridViewColumn("DataGridViewComboBoxColumn", null);
            DisposeOnTearDown(dataGridViewColumn);
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataGridViewColumn);
            Assert.IsInstanceOf(typeof(IDataGridViewComboBoxColumn), dataGridViewColumn);
            Assert.AreSame(dataGridViewNumericUpDownColumn.GetType(), dataGridViewColumn.GetType());
        }

        [Test]
        public virtual void TestCreateDataGridViewColumn_WithTypeName_Image()
        {
            //Not implemented in win
        }

        [Test]
        public void Test_Create_GroupBoxGroupControl()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var groupBoxGroupControl = GetControlFactory().CreateGroupBoxGroupControl();
            DisposeOnTearDown(groupBoxGroupControl);
            //---------------Test Result -----------------------
            Assert.IsNotNull(groupBoxGroupControl);
        }

        [Test]
        public virtual void Test_Create_ComboBox()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();  
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IComboBox control = factory.CreateComboBox();
            DisposeOnTearDown(control);
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
            Assert.AreEqual(GetStandardTextBoxHeight(), control.Height);
        }
        [Test]
        public virtual void Test_Create_ComboBoxSelector()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOComboBoxSelector control = factory.CreateComboBoxSelector();
            DisposeOnTearDown(control);
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
            Assert.AreEqual(GetStandardTextBoxHeight(), control.Height);
        }
        [Test]
        public void Test_Create_CollapsiblePanelSelector()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOCollapsiblePanelSelector control = factory.CreateCollapsiblePanelSelector();
            DisposeOnTearDown(control);
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
        }

        [Test]
        public void Test_Create_BOEditorControl_Generic_WithUIDef()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOPanelEditorControl control = factory.CreateBOEditorControl<ContactPersonTestBO>("default");
            DisposeOnTearDown(control);
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
        }

        [Test]
        public void Test_Create_BOEditorControl_Generic()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOPanelEditorControl control = factory.CreateBOEditorControl<ContactPersonTestBO>();
            DisposeOnTearDown(control);
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
        }
        [Test]
        public void Test_Create_BOEditorControl_NonGeneric_WithUIDef()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOPanelEditorControl control = factory.CreateBOEditorControl(classDef, "default");
            DisposeOnTearDown(control);
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
        }
        [Test]
        public void Test_Create_BOEditorControl_NonGeneric()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOPanelEditorControl control = factory.CreateBOEditorControl(classDef);
            DisposeOnTearDown(control);
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
        }
        
        [Test]
        public void Test_Create_MainTitleIconControl()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IMainTitleIconControl control = factory.CreateMainTitleIconControl();
            DisposeOnTearDown(control);
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
        }

        [Test]
        public void Test_CreateExtendedTextBox_ShouldCreateItem()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var extendedTextBox = factory.CreateExtendedTextBox();
            DisposeOnTearDown(extendedTextBox);
            //---------------Test Result -----------------------
            Assert.IsNotNull(extendedTextBox);
        }
        [Test]
        public void Test_CreateGridAndBOEditorControl_ShouldCreateItem()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            var classDef = MyBO.LoadDefaultClassDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var extendedTextBox = factory.CreateGridAndBOEditorControl(classDef);
            DisposeOnTearDown(extendedTextBox);
            //---------------Test Result -----------------------
            Assert.IsNotNull(extendedTextBox);
        }
        [Test]
        public void Test_CreateGridAndBOEditorControlGeneric_ShouldCreateItem()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            MyBO.LoadDefaultClassDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var extendedTextBox = factory.CreateGridAndBOEditorControl<MyBO>();
            DisposeOnTearDown(extendedTextBox);
            //---------------Test Result -----------------------
            Assert.IsNotNull(extendedTextBox);
        }
        protected abstract void AssertGridColumnTypeAfterCast(IDataGridViewColumn createdColumn, Type expectedColumnType);
    }
}
