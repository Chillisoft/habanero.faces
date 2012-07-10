//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Test;
using Habanero.Test.Structure;
using Habanero.Faces.Base;


using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{ // ReSharper disable InconsistentNaming
    public abstract class TestPanelBuilder
    {
        protected const int DEFAULT_CONTROLS_PER_FIELD = 3;
        protected abstract IControlFactory GetControlFactory();
        protected abstract Sample.SampleUserInterfaceMapper GetSampleUserInterfaceMapper();

        [SetUp]
        public void SetupTest() { ClassDef.ClassDefs.Clear(); }

        [Test]
        public virtual void TestConstructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Test Result -----------------------
            Assert.AreEqual(GetControlFactory().GetType(), panelBuilder.ControlFactory.GetType());
        }

        [Test]
        public void Test_BuildPanelForTab_tabNull()
        {
            //---------------Set up test pack-------------------
            PanelBuilder panelBuilder = new PanelBuilder(this.GetControlFactory());

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                panelBuilder.BuildPanelForTab(null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("formTab", ex.ParamName);
            }
        }

        [Test]
        public void Test_BuildPanelForTab_1Field_String()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneField();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD, panel.Controls.Count);
            Assert.IsInstanceOf(typeof (ILabel), panel.Controls[0]);
            Assert.IsInstanceOf(typeof (ITextBox), panel.Controls[1]);
            Assert.IsInstanceOf(typeof (IPanel), panel.Controls[2]);

            ILabel label = (ILabel) panel.Controls[0];
            Assert.AreEqual("Text:", label.Text);
        }

        [Ignore("This needs to be implemented and tested")] //TODO Brett 27 Oct 2009: Ignored Test - This needs to be implemented and tested
        [Test]
        public void Test_BuildPanelForTab_1Field_HasUOM()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneField();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD, panel.Controls.Count);
            Assert.IsInstanceOf(typeof (ILabel), panel.Controls[0]);
            Assert.IsInstanceOf(typeof (ITextBox), panel.Controls[1]);
            Assert.IsInstanceOf(typeof (IPanel), panel.Controls[2]);

            ILabel label = (ILabel) panel.Controls[0];
            Assert.AreEqual("Text (UOM) :", label.Text);
        } 

        [Test]
        public void Test_BuildPanelForTab_1Field_Integer()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleIntegerFieldTab = interfaceMapper.GetFormTabOneIntegerField();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleIntegerFieldTab).Panel;
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD, panel.Controls.Count);
            Assert.IsInstanceOf(typeof (ILabel), panel.Controls[0]);
            Assert.IsInstanceOf(typeof (INumericUpDown), panel.Controls[1]);
            Assert.IsInstanceOf(typeof (IPanel), panel.Controls[2]);
        }

        [Test]
        public void Test_ConfigureInputControl_WhenUIFormFieldKeepValuePrivateTrue_ShouldSetPasswordChar()
        {
            //---------------Set up test pack-------------------
            FakeUIFormField field = new FakeUIFormField();
            field.SetKeepValuePrivate(true);
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.IsTrue(field.KeepValuePrivate);
            //---------------Execute Test ----------------------
            IControlMapper mapper;
            var habaneroControl = panelBuilder.ConfigureInputControl(field, out mapper);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<ITextBox>(habaneroControl);
            ITextBox tBox = (ITextBox) habaneroControl;
            Assert.AreEqual("*", tBox.PasswordChar.ToString());
        }
        [Test]
        public void Test_ConfigureInputControl_WhenUIFormFieldKeepValuePrivateFalse_ShouldSetPasswordCharNull()
        {
            //---------------Set up test pack-------------------
            FakeUIFormField field = new FakeUIFormField();
            field.SetKeepValuePrivate(false);
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.IsFalse(field.KeepValuePrivate);
            //---------------Execute Test ----------------------
            IControlMapper mapper;
            var habaneroControl = panelBuilder.ConfigureInputControl(field, out mapper);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<ITextBox>(habaneroControl);
            ITextBox tBox = (ITextBox) habaneroControl;
            Assert.AreEqual("\0", tBox.PasswordChar.ToString());
        }

        [Test]
        public void Test_BuildPanelForTab_1Field_GroupBoxLayout_Integer()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleIntegerFieldTab = interfaceMapper.GetFormTabOneIntegerField();
            IUIFormField formField = singleIntegerFieldTab[0][0];
            formField.Layout = LayoutStyle.GroupBox;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleIntegerFieldTab).Panel;
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD, panel.Controls.Count);  //still has a null control in place
            Assert.IsInstanceOf(typeof (IGroupBox), panel.Controls[0]);
            IGroupBox groupBox = (IGroupBox) panel.Controls[0];

            Assert.IsInstanceOf(typeof (IPanel), panel.Controls[2]);
            Assert.AreEqual(1, groupBox.Controls.Count);
            Assert.IsInstanceOf(typeof(INumericUpDown), groupBox.Controls[0]);
        }

        [Test]
        public void Test_BuildPanelForTab_1Field_GroupBoxLayout_TextBox_Multiline()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleIntegerFieldTab = interfaceMapper.GetFormTabOneFieldWithMultiLineParameter();
            IUIFormField formField = singleIntegerFieldTab[0][0];
            formField.Layout = LayoutStyle.GroupBox;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleIntegerFieldTab).Panel;
            //---------------Test Result -----------------------
            IGroupBox groupBox = (IGroupBox)panel.Controls[0];
            Assert.IsInstanceOf(typeof(ITextBox), groupBox.Controls[0]);
            Assert.AreEqual(3 * GetControlFactory().CreateTextBox().Height + (2 * LayoutManager.DefaultGapSize), groupBox.Height);
            ITextBox textBox = (ITextBox)groupBox.Controls[0];
            Assert.IsTrue(textBox.Multiline);
        }

        [Test]
        public void Test_BuildPanelForTab_1Field_Layout()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleIntegerFieldTab = interfaceMapper.GetFormTabOneField();
            IUIFormColumn column = singleIntegerFieldTab[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleIntegerFieldTab).Panel;
            //---------------Test Result -----------------------
            ILabel label = (ILabel) panel.Controls[0];
            IControlHabanero textbox = panel.Controls[1];
            IPanel errorProviderPanel = (IPanel) panel.Controls[2];

            //--- check horizontal position of label (should be left aligned and sized according to its preferred size) -----
            Assert.AreEqual(LayoutManager.DefaultBorderSize, label.Left);
            Assert.AreEqual(label.PreferredWidth, label.Width);

            //--- check horizontal position of error provider (should be right aligned and a specified width) -----
            Assert.AreEqual(column.Width - LayoutManager.DefaultBorderSize, errorProviderPanel.Left + errorProviderPanel.Width);
            Assert.AreEqual(PanelBuilder.ERROR_PROVIDER_WIDTH, errorProviderPanel.Width);

            //--- check horizontal position of text box (should fill the rest of the row -----
            Assert.AreEqual(LayoutManager.DefaultBorderSize + label.Width + LayoutManager.DefaultGapSize, textbox.Left);
            Assert.AreEqual(errorProviderPanel.Left - LayoutManager.DefaultGapSize, textbox.Left + textbox.Width);
        }

        [Test]
        public void Test_BuildPanelForTab_1Field_GroupBoxLayout_Layout()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleIntegerFieldTab = (UIFormTab) interfaceMapper.GetSimpleUIFormDef()[0];
            IUIFormColumn column = singleIntegerFieldTab[0];
            IUIFormField formField = column[0];
            formField.Layout = LayoutStyle.GroupBox;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleIntegerFieldTab).Panel;
            //---------------Test Result -----------------------
            IGroupBox groupBox = (IGroupBox) panel.Controls[0];
            IPanel errorProviderPanel = (IPanel) panel.Controls[2];

            //--- check horizontal position of error provider (should be right aligned and a specified width) -----
            Assert.AreEqual(column.Width - LayoutManager.DefaultBorderSize, errorProviderPanel.Left + errorProviderPanel.Width);
            Assert.AreEqual(PanelBuilder.ERROR_PROVIDER_WIDTH, errorProviderPanel.Width);

            //--- check horizontal position of GroupBox (should be left aligned and fill the row) -----
            Assert.AreEqual(LayoutManager.DefaultBorderSize, groupBox.Left);
            Assert.AreEqual(errorProviderPanel.Left - LayoutManager.DefaultGapSize, groupBox.Left + groupBox.Width);
        }

        [Test]
        public void Test_BuildPanelForTab_2Fields_Layout()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab twoFieldTab = interfaceMapper.GetFormTabTwoFields();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoFieldTab).Panel;
            //---------------Test Result -----------------------
            ILabel label1 = (ILabel) panel.Controls[0];
            IControlHabanero textbox1 = panel.Controls[1];
            IPanel errorProviderPanel1 = (IPanel) panel.Controls[2];

            Assert.AreEqual(LayoutManager.DefaultBorderSize, label1.Top);
            Assert.AreEqual(LayoutManager.DefaultBorderSize, textbox1.Top);
            Assert.AreEqual(LayoutManager.DefaultBorderSize, errorProviderPanel1.Top);

            Assert.AreEqual(textbox1.Height, label1.Height);
            Assert.AreEqual(textbox1.Height, errorProviderPanel1.Height);

            ILabel label2 = (ILabel) panel.Controls[3];
            IControlHabanero textbox2 = panel.Controls[4];
            IPanel errorProviderPanel2 = (IPanel) panel.Controls[5];

            int expectedSecondRowTop = textbox1.Top + textbox1.Height + LayoutManager.DefaultGapSize;
            Assert.AreEqual(expectedSecondRowTop, label2.Top);
            Assert.AreEqual(expectedSecondRowTop, textbox2.Top);
            Assert.AreEqual(expectedSecondRowTop, errorProviderPanel2.Top);

            Assert.AreEqual(label1.Left, label2.Left);
            Assert.AreEqual(textbox1.Left, textbox2.Left);
            Assert.AreEqual(errorProviderPanel1.Left, errorProviderPanel2.Left);
            Assert.AreEqual(label1.Right, label2.Right);
            Assert.AreEqual(textbox1.Right, textbox2.Right);
            Assert.AreEqual(errorProviderPanel1.Right, errorProviderPanel2.Right);
        }

        [Test]
        public virtual void Test_BuildPanelForTab_2Fields()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab twoFieldTab = interfaceMapper.GetFormTabTwoFields();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            const int expectedFields = 2;
            //---------------Assert Precondition----------------
            Assert.AreEqual(expectedFields, twoFieldTab[0].Count);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoFieldTab).Panel;
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD*expectedFields, panel.Controls.Count);

            //-- Row 1
            Assert.IsInstanceOf(typeof (ILabel), panel.Controls[0]);
            ILabel row1Label = (ILabel) panel.Controls[0];
            Assert.AreEqual("Text:", row1Label.Text);
            Assert.IsInstanceOf(typeof (ITextBox), panel.Controls[1]);
            Assert.IsInstanceOf(typeof (IPanel), panel.Controls[2]);

            //-- Row 2
            Assert.IsInstanceOf(typeof (ILabel), panel.Controls[3]);
            ILabel row2Label = (ILabel) panel.Controls[3];
            Assert.AreEqual("Integer:", row2Label.Text);
            Assert.IsInstanceOf(typeof (INumericUpDown), panel.Controls[4]);
            Assert.IsInstanceOf(typeof (IPanel), panel.Controls[5]);
        }

        [Test]
        public void Test_BuildPanelForTab_2Columns_1_1()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabTwoColumns_1_1();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            const int expectedColumns = 2;
            const int expectedFieldsInEachColumn = 1;
            //---------------Assert Precondition----------------
            Assert.AreEqual(expectedColumns, twoColumnTab.Count);
            Assert.AreEqual(expectedFieldsInEachColumn, twoColumnTab[0].Count);
            Assert.AreEqual(expectedFieldsInEachColumn, twoColumnTab[1].Count);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoColumnTab).Panel;
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD*expectedColumns*expectedFieldsInEachColumn, panel.Controls.Count);
        }

        [Test]
        public void Test_BuildPanelForTab_2Columns_1_2()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabTwoColumns_1_2();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            const int expectedColumns = 2;
            const int maxFieldsInAColumn = 2;
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, twoColumnTab.Count);
            Assert.AreEqual(1, twoColumnTab[0].Count);
            Assert.AreEqual(2, twoColumnTab[1].Count);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoColumnTab).Panel;
            //---------------Test Result -----------------------
            Assert.AreEqual(DEFAULT_CONTROLS_PER_FIELD*expectedColumns*maxFieldsInAColumn, panel.Controls.Count);
        }

        [Test]
        public void Test_BuildPanelForTab_2Columns_1_2_CorrectLayout()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabTwoColumns_1_2();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            IUIFormColumn formColumn = twoColumnTab[0];
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, twoColumnTab.Count);
            Assert.AreEqual(1, formColumn.Count);
            Assert.AreEqual(2, twoColumnTab[1].Count);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoColumnTab).Panel;
            //---------------Test Result -----------------------
            IControlCollection panelControls = panel.Controls;
            //-----Row 1 Column 1
            Assert.IsInstanceOf(typeof (ILabel), panelControls[0]);
            Assert.AreEqual("Text:", panelControls[0].Text);
            Assert.AreEqual(formColumn[0].PropertyName, panelControls[0].Name);
            Assert.IsInstanceOf(typeof (ITextBox), panelControls[1]);

            Assert.IsInstanceOf(typeof (IPanel), panelControls[2]);
            Assert.AreEqual(PanelBuilder.ERROR_PROVIDER_WIDTH, panelControls[2].Width);
            //----Row 1 Column 2
            Assert.IsInstanceOf(typeof (ILabel), panelControls[3]);
            Assert.AreEqual("Integer:", panelControls[3].Text);
            Assert.IsInstanceOf(typeof (INumericUpDown), panelControls[4]);
            Assert.IsInstanceOf(typeof (IPanel), panelControls[5]);
            Assert.AreEqual(PanelBuilder.ERROR_PROVIDER_WIDTH, panelControls[5].Width);
            //---Row 2 Column 1
            Assert.IsInstanceOf(typeof (IControlHabanero), panelControls[6]);
            Assert.IsInstanceOf(typeof (IControlHabanero), panelControls[7]);
            Assert.IsInstanceOf(typeof (IControlHabanero), panelControls[8]);
            //---Row 2 Column 2
            Assert.AreEqual("Date:", panelControls[9].Text);
            Assert.IsInstanceOf(typeof (IDateTimePicker), panelControls[10]);
            Assert.IsInstanceOf(typeof (IPanel), panelControls[11]);
            Assert.AreEqual(PanelBuilder.ERROR_PROVIDER_WIDTH, panelControls[11].Width);
        }

        [Test]
        public void Test_BuildPanelForTab_ColumnWidths_SingleColumn()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabOneColumnOneRowWithWidth();
            IUIFormColumn column1 = twoColumnTab[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoColumnTab).Panel;

            //---------------Test Result -----------------------
            IControlHabanero column1Control1 = panel.Controls[0];
            IControlHabanero column1LastControl = panel.Controls[DEFAULT_CONTROLS_PER_FIELD - 1];

            // test the width of the entire panel
            Assert.AreEqual(column1.Width, panel.Width);

            // check that the left control is at the correct position (0 + Border size)
            Assert.AreEqual(LayoutManager.DefaultBorderSize, column1Control1.Left);

            // check that the last control of column 1 has its right edge at the correct position (column width)
            Assert.AreEqual(column1.Width - LayoutManager.DefaultBorderSize, column1LastControl.Left + column1LastControl.Width);
        }

        [Test]
        public void Test_BuildPanelForTab_ColumnWidths_FirstColumnOfMultiColumn()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabTwoColumnsOneRowWithWidths();
            IUIFormColumn column1 = twoColumnTab[0];
            IUIFormColumn column2 = twoColumnTab[1];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoColumnTab).Panel;

            //---------------Test Result -----------------------
            IControlHabanero column1Control1 = panel.Controls[0];
            IControlHabanero column1LastControl = panel.Controls[DEFAULT_CONTROLS_PER_FIELD - 1];

            // test the width of the entire panel
            Assert.AreEqual(column1.Width + column2.Width, panel.Width);

            // check that the left control is at the correct position (0 + Border size)
            Assert.AreEqual(LayoutManager.DefaultBorderSize, column1Control1.Left);

            // check that the last control of column 1 has its right edge at the correct position (column width)
            Assert.AreEqual(column1.Width, column1LastControl.Left + column1LastControl.Width);
        }

        [Test]
        public void Test_BuildPanelForTab_ColumnWidths_LastColumnOfMultiColumn()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab twoColumnTab = interfaceMapper.GetFormTabTwoColumnsOneRowWithWidths();

            IUIFormColumn column1 = twoColumnTab[0];
            IUIFormColumn column2 = twoColumnTab[1];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoColumnTab).Panel;

            //---------------Test Result -----------------------
            IControlHabanero column2LastControl = panel.Controls[DEFAULT_CONTROLS_PER_FIELD*2 - 1];
            IControlHabanero column2Control1 = panel.Controls[DEFAULT_CONTROLS_PER_FIELD];

            // check that the first control of the second column is at correct left position (column 1 width + gap)
            Assert.AreEqual(column1.Width + LayoutManager.DefaultGapSize, column2Control1.Left);

            // check that the last control of column 2 has its right edge at the correct position (panel width - border)
            Assert.AreEqual(column1.Width + column2.Width - LayoutManager.DefaultBorderSize,
                            column2LastControl.Left + column2LastControl.Width);
        }

        [Test]
        public void Test_BuildPanelForTab_ColumnWidths_MiddleColumnOfMultiColumn()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab threeColumnTab = interfaceMapper.GetFormTabThreeColumnsOneRowWithWidths();

            IUIFormColumn column1 = threeColumnTab[0];
            IUIFormColumn column2 = threeColumnTab[1];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(threeColumnTab).Panel;

            //---------------Test Result -----------------------
            IControlHabanero column2LastControl = panel.Controls[DEFAULT_CONTROLS_PER_FIELD*2 - 1];
            IControlHabanero column2Control1 = panel.Controls[DEFAULT_CONTROLS_PER_FIELD];

            // check that the first control of the second column is at correct left position (column 1 width + gap)
            Assert.AreEqual(column1.Width + LayoutManager.DefaultGapSize, column2Control1.Left);

            // check that the last control of column 2 has its right edge at the correct position (panel width - border)
            Assert.AreEqual(column1.Width + column2.Width, column2LastControl.Left + column2LastControl.Width);
        }

        [Test]
        public void Test_BuildPanelForTab_ColumnWidths_DataColumnResizes()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleIntegerFieldTab = interfaceMapper.GetFormTabOneFieldNoColumnWidth();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            IPanel panel = panelBuilder.BuildPanelForTab(singleIntegerFieldTab).Panel;

            const int columnWidthOrig = 300;
            const int columnWidthAfter = 500;
            panel.Width = columnWidthOrig;
            ILabel label = (ILabel) panel.Controls[0];
            IControlHabanero textbox = panel.Controls[1];
            IPanel errorProviderPanel = (IPanel) panel.Controls[2];
            int originalWidth = textbox.Width;

            //---------------Assert Precondition----------------
            Assert.AreEqual(label.Left + label.Width + LayoutManager.DefaultGapSize, textbox.Left);
            Assert.AreEqual(errorProviderPanel.Left - LayoutManager.DefaultGapSize, textbox.Left + textbox.Width);
            //---------------Execute Test ----------------------
            panel.Width = columnWidthAfter;
            //---------------Test Result -----------------------

            Assert.AreEqual(originalWidth + columnWidthAfter - columnWidthOrig, textbox.Width);
        }

        [Test]
        public virtual void Test_BuildPanelForTab_3Columns_1Column_RowSpan2()
        {
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleIntegerFieldTab = interfaceMapper.GetFormTabThreeColumnsOneColumnWithRowSpan();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleIntegerFieldTab).Panel;
            //---------------Test Result -----------------------
            IControlHabanero textBoxCol1 = panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO];
            IControlHabanero col1Text1RowSpan2Label = panel.Controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO];
            IControlHabanero textBoxCol2 = panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN];
            IControlHabanero nullControl =
                panel.Controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN*3];
            ILabel col2TextBox2Label =
                (ILabel) panel.Controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN*4];

            Assert.IsNotInstanceOf(typeof (ILabel), nullControl);
            Assert.AreEqual(textBoxCol2.Height*2 + LayoutManager.DefaultGapSize, textBoxCol1.Height);
            Assert.AreEqual(col1Text1RowSpan2Label.Left, nullControl.Left);

            Assert.IsInstanceOf(typeof (ILabel), col2TextBox2Label);
            Assert.AreEqual("Col2TextBox2", col2TextBox2Label.Text);
            Assert.AreEqual(
                textBoxCol1.Left + textBoxCol1.Width + PanelBuilder.ERROR_PROVIDER_WIDTH + GridLayoutManager.DefaultGapSize*2,
                col2TextBox2Label.Left);
        }

        [Test]
        public void Test_BuildPanelForTab_RowSpanAndColumnSpan_DoesNotCauseError()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = new Sample.SampleUserInterfaceMapperWin();
            UIFormTab oneFieldRowColSpan = interfaceMapper.GetFormTabOneFieldHasRowAndColSpan();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(oneFieldRowColSpan).Panel;
            //---------------Test Result -----------------------

            IControlCollection controlCollection = panel.Controls;
        }

        [Test]
        public void Test_BuildPanelForTab_RowSpan()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab formTab = interfaceMapper.GetFormTabOneColumnThreeRowsWithRowSpan();

            IUIFormColumn column1 = formTab[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(formTab).Panel;
            //---------------Test Result -----------------------

            IControlHabanero row1InputControl = panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO];
            ITextBox row2InputControl =
                (ITextBox) panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN];
            ITextBox row3InputControl =
                (ITextBox) panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN*3];

            Assert.IsTrue(row2InputControl.Multiline);
            Assert.AreEqual(row1InputControl.Height*2 + LayoutManager.DefaultGapSize, row2InputControl.Height);
//            Assert.AreEqual(row2InputControl.Bottom+LayoutManager.DefaultGapSize,row3InputControl.Top);
        }

        [Test]
        public void Test_BuildPanelForTab_Parameter_ColumnSpan()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab formTab = interfaceMapper.GetFormTabTwoColumns_2_1_ColSpan();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(formTab).Panel;
            //---------------Test Result -----------------------

            IControlHabanero columnSpanningControl = panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO];
            IControlHabanero columnSpanningErrorProviderControl = panel.Controls[PanelBuilder.ERROR_PROVIDER_COLUMN_NO];
            IControlHabanero row2Col1InputControl =
                panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN*2];
            IControlHabanero row2Col2InputControl =
                panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN*3];
            IControlHabanero row2Col2ErrorProviderControl =
                panel.Controls[PanelBuilder.ERROR_PROVIDER_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN*3];

            // -- check that the col spanning control is the correct width
            Assert.AreEqual(row2Col1InputControl.Left, columnSpanningControl.Left);
            Assert.AreEqual(row2Col2InputControl.Right, columnSpanningControl.Right);

            // check that the error provider control is in the correct position (on the right of the col spanning control)
            Assert.AreEqual(row2Col2ErrorProviderControl.Left, columnSpanningErrorProviderControl.Left);

            // check that the first control in the second column is moved down to accommodate the col spanning control
            Assert.AreEqual(row2Col1InputControl.Top, row2Col2InputControl.Top);
        }

        [Test]
        public void Test_BuildPanelForTab_Parameter_DefaultAlignment_Left()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabTwoFieldsWithNoAlignment();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            UIFormField formField = (UIFormField) singleFieldTab[0][0];
            Assert.IsTrue(String.IsNullOrEmpty(formField.Alignment));

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOf(typeof (ITextBox), panel.Controls[1]);
            ITextBox control = (ITextBox) panel.Controls[1];
            Assert.AreEqual(HorizontalAlignment.Left, control.TextAlign);

            Assert.IsInstanceOf(typeof (INumericUpDown), panel.Controls[PanelBuilder.CONTROLS_PER_COLUMN + 1]);
            INumericUpDown numericUpDown = (INumericUpDown) panel.Controls[PanelBuilder.CONTROLS_PER_COLUMN + 1];
            Assert.AreEqual(HorizontalAlignment.Left, numericUpDown.TextAlign);
        }

        [Test]
        public void Test_BuildPanelForTab_Parameter_Alignment_Right()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithRightAlignment();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            UIFormField formField = (UIFormField)singleFieldTab[0][0];
            Assert.AreEqual("right", formField.Alignment);

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOf(typeof (ITextBox), panel.Controls[1]);
            ITextBox control = (ITextBox) panel.Controls[1];
            Assert.AreEqual(HorizontalAlignment.Right, control.TextAlign);
        }

        [Test]
        public void Test_BuildPanelForTab_Parameter_Alignment_Center()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithCenterAlignment();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            UIFormField formField = (UIFormField) singleFieldTab[0][0];
            Assert.AreEqual("center", formField.Alignment);

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOf(typeof (ITextBox), panel.Controls[1]);
            ITextBox control = (ITextBox) panel.Controls[1];
            Assert.AreEqual(HorizontalAlignment.Center, control.TextAlign);
        }

        [Test]
        public void Test_BuildPanelForTab_Parameter_Alignment_InvalidAlignment()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithInvalidAlignment();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            bool errorThrown = false;
            string errMessage = "";
            //---------------Assert Precondition----------------
            UIFormField formField = (UIFormField)singleFieldTab[0][0];
            Assert.AreEqual("Top", formField.Alignment);
            //---------------Execute Test ----------------------

            try
            {
                IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            }
            catch (HabaneroDeveloperException ex)
            {
                errorThrown = true;
                errMessage = ex.Message;
            }

            //---------------Test Result -----------------------

            Assert.IsTrue(errorThrown, "The alignment value is invalid and a HabaneroDeveloperException should be thrown.");
            StringAssert.Contains("Invalid alignment property value ", errMessage);
        }

        [Test]
        public void Test_BuildPanelForTab_Parameter_MultiLine()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithMultiLineParameter();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOf(typeof (ITextBox), panel.Controls[1]);
            ITextBox control = (ITextBox) panel.Controls[1];
            Assert.IsTrue(control.Multiline);
            Assert.IsTrue(control.AcceptsReturn);
            Assert.AreEqual(60 + (LayoutManager.DefaultGapSize * 2), control.Height);
            Assert.AreEqual(ScrollBars.Vertical, control.ScrollBars);
        }

        [Test]
        public void Test_BuildPanelForTab_Parameter_InvalidMultiLineValue()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithInvalidMultiLineParameter();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            bool errorThrown = false;
            string errMessage = "";

            //---------------Execute Test ----------------------
            try
            {
                IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            }
            catch (InvalidXmlDefinitionException ex)
            {
                errorThrown = true;
                errMessage = ex.Message;
            }
            Assert.IsTrue(errorThrown,
                          "An error occurred while reading the 'numLines' parameter from the class definitions.  The 'value' attribute must be a valid integer.");
            StringAssert.Contains(
                "An error occurred while reading the 'numLines' parameter from the class definitions.  The 'value' attribute must be a valid integer.",
                errMessage);
        }

        [Test]
        public void Test_BuildPanelForTab_Parameter_DecimalPlaces_NumericUpDownMoneyMapper()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithDecimalPlacesParameter();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            UIFormField formField = (UIFormField) singleFieldTab[0][0];
            Assert.IsFalse(String.IsNullOrEmpty(formField.DecimalPlaces));
            Assert.AreEqual("3", formField.DecimalPlaces);
            Assert.AreEqual("NumericUpDownCurrencyMapper", singleFieldTab[0][0].MapperTypeName);

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOf(typeof (INumericUpDown), panel.Controls[1]);
            INumericUpDown control = (INumericUpDown) panel.Controls[1];
            Assert.AreEqual(3, control.DecimalPlaces);
        }

        [Test]
        public void Test_BuildPanelForTab_Parameter_Options()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithOptionsParameter();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            UIFormField formField = (UIFormField) singleFieldTab[0][0];
            Assert.IsFalse(String.IsNullOrEmpty(formField.Options));
            Assert.AreEqual("M|F", formField.Options);
            Assert.AreEqual("ListComboBoxMapper", singleFieldTab[0][0].MapperTypeName);

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOf(typeof (IComboBox), panel.Controls[1]);
            IComboBox control = (IComboBox) panel.Controls[1];
            Assert.AreEqual(3, control.Items.Count);
            Assert.AreEqual("", control.Items[0].ToString());
            Assert.AreEqual("M", control.Items[1].ToString());
            Assert.AreEqual("F", control.Items[2].ToString());
        }

        [Test, Ignore("Can not test the event on the TextBox")]
        public void Test_BuildPanelForTab_Parameter_isEmal()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithIsEmailParameter();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            UIFormField formField = (UIFormField)singleFieldTab[0][0];
            Assert.IsFalse(String.IsNullOrEmpty(formField.IsEmail));
            Assert.IsTrue(Convert.ToBoolean(formField.IsEmail));

            UIFormField formField1 = (UIFormField) singleFieldTab[0][1];
            Assert.IsFalse(String.IsNullOrEmpty(formField1.IsEmail));
            Assert.IsFalse(Convert.ToBoolean(formField1.IsEmail));

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOf(typeof (ITextBox), panel.Controls[1]);
            ITextBox control = (ITextBox) panel.Controls[1];
        }

        [Test]
        public void Test_BuildPanelForTab_Parameter_DateFormat()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldWithDateFormatParameter();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            UIFormField formField = (UIFormField) singleFieldTab[0][0];
            Assert.IsFalse(String.IsNullOrEmpty(formField.DateFormat));
            UIFormField formField1 = (UIFormField) singleFieldTab[0][1];
            Assert.IsFalse(String.IsNullOrEmpty(formField1.DateFormat));

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOf(typeof (IDateTimePicker), panel.Controls[1]);
            IDateTimePicker control1 = (IDateTimePicker) panel.Controls[1];
            Assert.AreEqual(DateTimePickerFormat.Short, control1.Format);

            Assert.IsInstanceOf(typeof (IDateTimePicker), panel.Controls[PanelBuilder.CONTROLS_PER_COLUMN + 1]);
            IDateTimePicker control2 = (IDateTimePicker) panel.Controls[PanelBuilder.CONTROLS_PER_COLUMN + 1];
            Assert.AreEqual(DateTimePickerFormat.Custom, control2.Format);
            Assert.AreEqual(formField1.DateFormat, control2.CustomFormat);
        }

        [Test]
        public void Test_BuildPanelForTab_GetAlignmentValueMethod_Left()
        {
            //---------------Set up test pack-------------------
            const string alignment = "left";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            HorizontalAlignment alignmentValueLowerCase = PanelBuilder.GetAlignmentValue(alignment.ToLower());
            HorizontalAlignment alignmentValueUpperCase = PanelBuilder.GetAlignmentValue(alignment.ToUpper());
            //---------------Test Result -----------------------

            Assert.AreEqual(HorizontalAlignment.Left, alignmentValueLowerCase);
            Assert.AreEqual(HorizontalAlignment.Left, alignmentValueUpperCase);
            Assert.AreEqual(alignmentValueUpperCase, alignmentValueLowerCase);
        }

        [Test]
        public void Test_BuildPanelForTab_GetAlignmentValueMethod_Right()
        {
            //---------------Set up test pack-------------------
            const string alignment = "right";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            HorizontalAlignment alignmentValueLowerCase = PanelBuilder.GetAlignmentValue(alignment.ToLower());
            HorizontalAlignment alignmentValueUpperCase = PanelBuilder.GetAlignmentValue(alignment.ToUpper());
            //---------------Test Result -----------------------
            Assert.AreEqual(HorizontalAlignment.Right, alignmentValueLowerCase);
            Assert.AreEqual(HorizontalAlignment.Right, alignmentValueUpperCase);
            Assert.AreEqual(alignmentValueUpperCase, alignmentValueLowerCase);
        }

        [Test]
        public void Test_BuildPanelForTab_GetAlignmentValueMethod_Center()
        {
            //---------------Set up test pack-------------------
            const string alignment = "center";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            HorizontalAlignment alignmentValueLowerCase = PanelBuilder.GetAlignmentValue(alignment.ToLower());
            HorizontalAlignment alignmentValueUpperCase = PanelBuilder.GetAlignmentValue(alignment.ToUpper());
            //---------------Test Result -----------------------
            Assert.AreEqual(HorizontalAlignment.Center, alignmentValueLowerCase);
            Assert.AreEqual(HorizontalAlignment.Center, alignmentValueUpperCase);
            Assert.AreEqual(alignmentValueUpperCase, alignmentValueLowerCase);
        }

        [Test]
        public void Test_BuildPanelForTab_GetAlignmentValueMethod_Centre()
        {
            //---------------Set up test pack-------------------
            const string alignment = "centre";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            HorizontalAlignment alignmentValueLowerCase = PanelBuilder.GetAlignmentValue(alignment.ToLower());
            HorizontalAlignment alignmentValueUpperCase = PanelBuilder.GetAlignmentValue(alignment.ToUpper());
            //---------------Test Result -----------------------
            Assert.AreEqual(HorizontalAlignment.Center, alignmentValueLowerCase);
            Assert.AreEqual(HorizontalAlignment.Center, alignmentValueUpperCase);
            Assert.AreEqual(alignmentValueUpperCase, alignmentValueLowerCase);
        }

        [Test]
        public void Test_BuildPanelForTab_GetAlignmentValueMethod_ThrowsADeveloperException()
        {
            //---------------Set up test pack-------------------
            const string alignment = "TestAlignment";
            bool errorThrown = false;
            string errMessage = "";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            try
            {
                PanelBuilder.GetAlignmentValue(alignment.ToLower());
            }
            catch (HabaneroDeveloperException ex)
            {
                errorThrown = true;
                errMessage = ex.Message;
            }

            //---------------Test Result -----------------------
            Assert.IsTrue(errorThrown, "The alignment value is invalid and a HabaneroDeveloperException should be thrown.");
            StringAssert.Contains("Invalid alignment property value ", errMessage);
        }

        [Test]
        public void Test_BuildPanelForTab_CompulsoryFieldsAreBoldAndStarred()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneCompulsory();
            UIFormTab twoFieldTabOneCompulsory = (UIFormTab) classDef.UIDefCol["default"].UIForm[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoFieldTabOneCompulsory).Panel;
            //---------------Test Result -----------------------
            IControlCollection controls = panel.Controls;

            ILabel compulsoryLabel = (ILabel)controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO];
            Assert.AreEqual("CompulsorySampleText: *", compulsoryLabel.Text);
            Assert.IsTrue(compulsoryLabel.Font.Bold);

            ILabel nonCompulsoryLabel =
                (ILabel)controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN];
            Assert.AreEqual("SampleTextNotCompulsory:", nonCompulsoryLabel.Text);
            Assert.IsFalse(nonCompulsoryLabel.Font.Bold);
        }

        [Test]
        public virtual void Test_BuildPanelForTab_CompulsoryRelationshipsAreBoldAndStarred()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithAssociationRelationship();
            classDef.PropDefcol["RelatedID"].Compulsory = true;
            UIFormTab twoFieldTabOneCompulsory = (UIFormTab)classDef.UIDefCol["default"].UIForm[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab(twoFieldTabOneCompulsory);
            //---------------Test Result -----------------------

            ILabel compulsoryLabel = (ILabel)panelInfo.FieldInfos["MyRelationship"].LabelControl;
            Assert.AreEqual("My Relationship: *", compulsoryLabel.Text);
            Assert.IsTrue(compulsoryLabel.Font.Bold);
        }
        
        [Test]
        public void Test_BuildPanelForTab_WithNonEditableField_ShouldDisableLabel()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneNotEditable();
            UIFormTab twoFieldTabOneCompulsory = (UIFormTab) classDef.UIDefCol["default"].UIForm[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            
            //-------------Assert Preconditions -------------
            
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(twoFieldTabOneCompulsory).Panel;
            //---------------Test Result -----------------------
            IControlCollection controls = panel.Controls;

            ILabel compulsoryLabel = (ILabel)controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO];
            Assert.AreEqual("EditableFieldSampleText:", compulsoryLabel.Text);
            Assert.IsTrue(compulsoryLabel.Enabled);

            ILabel nonCompulsoryLabel =
                (ILabel)controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO + PanelBuilder.CONTROLS_PER_COLUMN];
            Assert.AreEqual("SampleTextNotEditableField:", nonCompulsoryLabel.Text);
            Assert.IsFalse(nonCompulsoryLabel.Enabled);
        }

        [Test]
        public void Test_BuildPanelForTab_PopulatesFieldInfoCollection()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            UIFormTab twoFieldTabOneCompulsory = (UIFormTab) classDef.UIDefCol["default"].UIForm[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            Sample sample = new Sample();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab(twoFieldTabOneCompulsory);
            panelInfo.BusinessObject = sample;
            //---------------Test Result -----------------------

            IControlHabanero sampleTextLabel = panelInfo.Panel.Controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO];
            IControlHabanero sampleTextInputControl = panelInfo.Panel.Controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO];
            const string propertyName = "SampleText";
            IControlMapper sampleTextControlMapper = panelInfo.FieldInfos[propertyName].ControlMapper;

            Assert.AreEqual(2, panelInfo.FieldInfos.Count);

            PanelInfo.FieldInfo fieldInfo = panelInfo.FieldInfos[propertyName];
            Assert.AreSame(sampleTextLabel, fieldInfo.LabelControl);
            Assert.AreSame(sampleTextInputControl, fieldInfo.InputControl);

            Assert.AreSame(sampleTextInputControl, sampleTextControlMapper.Control);
            Assert.AreSame(sample, sampleTextControlMapper.BusinessObject);
            Assert.AreEqual(propertyName, sampleTextControlMapper.PropertyName);
        }

        [Test]
        public void Test_BuildPanelForTab_SetsClassDefForControlMappers()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            UIFormTab twoFieldTabOneCompulsory = (UIFormTab) classDef.UIDefCol["default"].UIForm[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab(twoFieldTabOneCompulsory);
            //---------------Test Result -----------------------
            PanelInfo.FieldInfo info = panelInfo.FieldInfos[0];
            IClassDef def = info.ControlMapper.ClassDef;
            Assert.AreSame(classDef, def);
        }

        [Test]
        public void Test_BuildPanelForTab_InputControlsHaveCorrectEnabledState()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            UIFormTab twoFieldTabOneCompulsory = (UIFormTab) classDef.UIDefCol["default"].UIForm[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab(twoFieldTabOneCompulsory);
            panelInfo.BusinessObject = new Sample();

            //---------------Test Result -----------------------
            Assert.IsTrue(panelInfo.FieldInfos[0].InputControl.Enabled);
            Assert.IsFalse(panelInfo.FieldInfos[1].InputControl.Enabled);
        }

        [Test]
        public void Test_BuildPanelForTab_LayoutManagerIsSet()
        {
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            UIFormTab twoFieldTabOneCompulsory = (UIFormTab) classDef.UIDefCol["default"].UIForm[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab(twoFieldTabOneCompulsory);

            //---------------Test Result -----------------------
            GridLayoutManager layoutManager = panelInfo.LayoutManager;
            Assert.IsNotNull(layoutManager);
            Assert.AreEqual(2, layoutManager.Rows.Count);
        }

        [Test]
        public void Test_BuildPanel_TabOrder_Simple()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            UIFormTab twoFieldTabOneCompulsory = (UIFormTab) classDef.UIDefCol["default"].UIForm[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab(twoFieldTabOneCompulsory);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, panelInfo.FieldInfos.Count);
            Assert.AreEqual(0, panelInfo.FieldInfos[0].InputControl.TabIndex);
            Assert.AreEqual(1, panelInfo.FieldInfos[1].InputControl.TabIndex);
        }

        [Test]
        public void Test_BuildPanel_TabOrder_TwoColumns()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            UIFormTab twoFieldTabOneCompulsory = (UIFormTab) classDef.UIDefCol["TwoColumns"].UIForm[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab(twoFieldTabOneCompulsory);
            //---------------Test Result -----------------------
            Assert.AreEqual(4, panelInfo.FieldInfos.Count);
            PanelInfo.FieldInfo fieldInfoC1R1 = panelInfo.FieldInfos[0];
            PanelInfo.FieldInfo fieldInfoC2R1 = panelInfo.FieldInfos[1];
            PanelInfo.FieldInfo fieldInfoC1R2 = panelInfo.FieldInfos[2];
            PanelInfo.FieldInfo fieldInfoC2R2 = panelInfo.FieldInfos[3];
            Assert.AreEqual("SampleText1:", fieldInfoC1R1.LabelControl.Text); //just making sure
            Assert.AreEqual("SampleText2:", fieldInfoC2R1.LabelControl.Text);
            Assert.AreEqual("SampleInt1:", fieldInfoC1R2.LabelControl.Text);
            Assert.AreEqual("SampleInt2:", fieldInfoC2R2.LabelControl.Text);
            Assert.AreEqual(0, fieldInfoC1R1.InputControl.TabIndex);
            Assert.AreEqual(2, fieldInfoC2R1.InputControl.TabIndex);
            Assert.AreEqual(1, fieldInfoC1R2.InputControl.TabIndex);
            Assert.AreEqual(3, fieldInfoC2R2.InputControl.TabIndex);
        }

        [Test]
        public void Test_BuildPanel_TabOrder_ThreeColumns()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            UIFormTab twoFieldTabOneCompulsory = (UIFormTab) classDef.UIDefCol["ThreeColumns"].UIForm[0];
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab(twoFieldTabOneCompulsory);
            //---------------Test Result -----------------------
            Assert.AreEqual(6, panelInfo.FieldInfos.Count);
            PanelInfo.FieldInfo fieldInfoC1R1 = panelInfo.FieldInfos[0];
            PanelInfo.FieldInfo fieldInfoC2R1 = panelInfo.FieldInfos[1];
            PanelInfo.FieldInfo fieldInfoC3R1 = panelInfo.FieldInfos[2];
            PanelInfo.FieldInfo fieldInfoC1R2 = panelInfo.FieldInfos[3];
            PanelInfo.FieldInfo fieldInfoC2R2 = panelInfo.FieldInfos[4];
            PanelInfo.FieldInfo fieldInfoC3R2 = panelInfo.FieldInfos[5];
            Assert.AreEqual("SampleText1:", fieldInfoC1R1.LabelControl.Text); //just making sure
            Assert.AreEqual("SampleText2:", fieldInfoC2R1.LabelControl.Text);
            Assert.AreEqual("SampleText3:", fieldInfoC3R1.LabelControl.Text);
            Assert.AreEqual("SampleInt1:", fieldInfoC1R2.LabelControl.Text);
            Assert.AreEqual("SampleInt2:", fieldInfoC2R2.LabelControl.Text);
            Assert.AreEqual("SampleInt3:", fieldInfoC3R2.LabelControl.Text);
            Assert.AreEqual(0, fieldInfoC1R1.InputControl.TabIndex);
            Assert.AreEqual(2, fieldInfoC2R1.InputControl.TabIndex);
            Assert.AreEqual(4, fieldInfoC3R1.InputControl.TabIndex);
            Assert.AreEqual(1, fieldInfoC1R2.InputControl.TabIndex);
            Assert.AreEqual(3, fieldInfoC2R2.InputControl.TabIndex);
            Assert.AreEqual(5, fieldInfoC3R2.InputControl.TabIndex);
        }

        [Test]
        public void Test_BuildPanelForForm_WithTwoTabPages_ShouldBuildTabControl()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = classDef.UIDefCol["TwoTabs"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.IsNotNull(GlobalUIRegistry.ControlFactory);
            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(form);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, panelInfo.Panel.Controls.Count); // only one control because it's the tab control
            Assert.IsInstanceOf(typeof (ITabControl), panelInfo.Panel.Controls[0]);
            ITabControl tabControl = (ITabControl) panelInfo.Panel.Controls[0];
            Assert.AreEqual(form.Count, tabControl.TabPages.Count);
        }

        [Test]
        public void Test_BuildPanelForForm_WithTwoTabPages_ShouldBuildTabControl_WithCorrectControlsOnTabPage()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = classDef.UIDefCol["TwoTabs"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(form);
            //---------------Test Result -----------------------
            ITabControl tabControl = (ITabControl) panelInfo.Panel.Controls[0];
            ITabPage tabPage1 = tabControl.TabPages[0];
            ITabPage tabPage2 = tabControl.TabPages[1];
            Assert.AreEqual(1, tabPage1.Controls.Count);
            Assert.IsInstanceOf(typeof (IPanel), tabPage1.Controls[0]);
            IPanel tabPage1Panel = (IPanel) tabPage1.Controls[0];
            Assert.AreEqual(PanelBuilder.CONTROLS_PER_COLUMN, tabPage1Panel.Controls.Count);
            Assert.AreEqual(1, tabPage2.Controls.Count);
            Assert.IsInstanceOf(typeof (IPanel), tabPage2.Controls[0]);
            IPanel tabPage2Panel = (IPanel) tabPage2.Controls[0];
            Assert.AreEqual(PanelBuilder.CONTROLS_PER_COLUMN, tabPage2Panel.Controls.Count);
        }

        [Test]
        public void Test_BuildPanelForForm_WithTwoTabPages_ShouldHaveCorrectNumberOfFieldInfos()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = classDef.UIDefCol["TwoTabs"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(form);
            //---------------Test Result -----------------------
            Assert.AreEqual(form.Count, panelInfo.PanelInfos.Count);
            Assert.AreEqual(panelInfo.FieldInfos.Count,
                            panelInfo.PanelInfos[0].FieldInfos.Count + panelInfo.PanelInfos[1].FieldInfos.Count);
        }

        [Test]
        public void Test_BuildPanelForForm_formNull()
        {
            //---------------Set up test pack-------------------
            PanelBuilder panelBuilder = new PanelBuilder(this.GetControlFactory());

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                panelBuilder.BuildPanelForForm(null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("uiForm", ex.ParamName);
            }
        }

        [Test]
        public void Test_BuildPanelForForm_ReturnsOnlyPanelForOneTab()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = classDef.UIDefCol["default"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(form);
            //---------------Test Result -----------------------
            Assert.AreEqual(6, panelInfo.Panel.Controls.Count);
            Assert.AreEqual(form, panelInfo.UIForm);
        }

        [Test]
        public void Test_BuildPanelForForm_WhenNoTabs_ShouldReturnPanelWithNoControls()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = new UIForm();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, form.Count);
            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(form);
            //---------------Test Result -----------------------
            IPanel panel = panelInfo.Panel;
            Assert.AreEqual(0, panel.Controls.Count);
        }

        [Test]
        public void Test_BuildPanelForForm_WhenNoTabs_ShouldReturnPanelSizedCorrectly()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = new UIForm();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, form.Count);
            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(form);
            //---------------Test Result -----------------------
            IPanel panel = panelInfo.Panel;
            Assert.AreEqual(form.Height, panel.Height, "Height should be correct");
            Assert.AreEqual(form.Width, panel.Width, "Width should be correct");
        }

        [Test]
        public void Test_BuildPanelForForm_WhenNoTabs_ShouldReturnPanelWithCorrectMinimumSize()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = new UIForm();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, form.Count);
            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(form);
            //---------------Test Result -----------------------
            IPanel panel = panelInfo.Panel;
            Assert.AreEqual(new Size(0, 0), panel.MinimumSize, "Minimum size should be correct");
        }

        [Test]
        public void Test_BuildPanelForForm_WhenOnlyOneTab_ShouldReturnPanelSizedCorrectly()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = classDef.UIDefCol["default"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, form.Count);
            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(form);
            //---------------Test Result -----------------------
            IPanel panel = panelInfo.Panel;
            Assert.AreEqual(form.Height, panel.Height, "Height should be correct");
            Assert.AreEqual(form.Width, panel.Width, "Width should be correct");
        }

        [Test]
        public void Test_BuildPanelForForm_WhenOnlyOneTab_WithMoreHeightThanControls_ShouldReturnPanelWithCorrectMinimumSize()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = classDef.UIDefCol["default"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, form.Count);
            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(form);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, panelInfo.FieldInfos.Count);
            PanelInfo.FieldInfo lastFieldInfo = panelInfo.FieldInfos[1];
            Assert.That(panelInfo.MinimumPanelHeight, Is.LessThan(form.Height), "Test case is not set up correctly");
            IPanel panel = panelInfo.Panel;
            Assert.AreEqual(panelInfo.MinimumPanelHeight, panel.MinimumSize.Height, "Minimum height should be correct");
            Assert.AreEqual(form.Width, panel.MinimumSize.Width, "Minimum width should be correct");
        }

        [Test]
        public void Test_BuildPanelForForm_WhenOnlyOneTab_WithMoreControlsThanHeight_ShouldReturnPanelWithCorrectMinimumSize()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = classDef.UIDefCol["default"].UIForm;
            form.Height = 20;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, form.Count);
            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(form);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, panelInfo.FieldInfos.Count);
            Assert.That(panelInfo.MinimumPanelHeight, Is.GreaterThan(form.Height), "Test case is not set up correctly");
            IPanel panel = panelInfo.Panel;
            Assert.AreEqual(form.Height, panel.MinimumSize.Height, "Minimum height should be correct");
            Assert.AreEqual(form.Width, panel.MinimumSize.Width, "Minimum width should be correct");
        }

        [Test]
        public void Test_BuildPanelForForm_WithTwoTabPages_ShouldReturnPanelSizedCorrectly()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = classDef.UIDefCol["TwoTabs"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, form.Count);
            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(form);
            //---------------Test Result -----------------------
            IPanel panel = panelInfo.Panel;
            Assert.AreEqual(form.Height, panel.Height, "Height should be correct");
            Assert.AreEqual(form.Width, panel.Width, "Width should be correct");
        }

        [Test]
        public void Test_CreateOnePanelPerUIFormTab_2Panels()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithTwoUITabs();
            IUIForm uiForm = classDef.UIDefCol["default"].UIForm;
            //--------------Assert PreConditions----------------            
            //---------------Execute Test ----------------------
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            IList<IPanelInfo> panelList = panelBuilder.CreateOnePanelPerUIFormTab(uiForm);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, panelList.Count);
            Assert.AreEqual("Tab1", panelList[0].UIFormTab.Name);
            Assert.AreEqual("Tab2", panelList[1].UIFormTab.Name);
            Assert.AreSame(uiForm, panelList[0].UIForm);
            Assert.AreSame(uiForm, panelList[1].UIForm);
        }

        [Test]
        public void Test_MinimumPanelHeight()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm uiForm = classDef.UIDefCol["default"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab((UIFormTab)uiForm[0]);
            IPanel pnl = panelInfo.Panel;
            //---------------Test Result -----------------------
            Assert.AreEqual(pnl.Height, panelInfo.MinimumPanelHeight);
            Assert.AreEqual(pnl.Width, panelInfo.Panel.MinimumSize.Width);
            Assert.AreEqual(pnl.Height, panelInfo.Panel.MinimumSize.Height);
        }

        [Test]
        public void Test_MinimumPanelHeight_ShouldBeTheBottomOfTheLastControl()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm uiForm = classDef.UIDefCol["default"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab((UIFormTab)uiForm[0]);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, panelInfo.FieldInfos.Count);
            PanelInfo.FieldInfo lastFieldInfo = panelInfo.FieldInfos[1];
            int bottomOfLastInputControl = lastFieldInfo.InputControl.Top + lastFieldInfo.InputControl.Height;
            bottomOfLastInputControl += panelInfo.LayoutManager.BorderSize;
            Assert.AreEqual(bottomOfLastInputControl, panelInfo.MinimumPanelHeight);
        }

        [Test]
        public void Test_BuldUsingAlaternateFormBuilder_CollapsiblePanel()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = classDef.UIDefCol["TwoTabs"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, form.Count);
            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(form, GetControlFactory().CreateCollapsiblePanelGroupControl);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(ICollapsiblePanelGroupControl), panelInfo.Panel.Controls[0]);
            ICollapsiblePanelGroupControl groupControl = (ICollapsiblePanelGroupControl)panelInfo.Panel.Controls[0];
            ICollapsiblePanel panel_1 = groupControl.PanelsList[0];
            ICollapsiblePanel panel_2 = groupControl.PanelsList[1];
            Assert.AreEqual(2, panel_1.Controls.Count, "Should have the Collapse Button and the Content Control");
            Assert.IsInstanceOf(typeof(IPanel), panel_1.Controls[1]);
            IPanel contentPanel_1 = (IPanel)panel_1.Controls[0];
            Assert.AreEqual(PanelBuilder.CONTROLS_PER_COLUMN, contentPanel_1.Controls.Count, "Should have the Collapse Button and the Content Control");
            Assert.AreEqual(2, panel_2.Controls.Count);
            Assert.IsInstanceOf(typeof(IPanel), panel_2.Controls[1]);
            IPanel contentPanel_2 = (IPanel)panel_2.Controls[0];
            Assert.AreEqual(PanelBuilder.CONTROLS_PER_COLUMN, contentPanel_2.Controls.Count);
        }

        [Test]
        public void Test_BuildPanelForForm_NullForm_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = classDef.UIDefCol["TwoTabs"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, form.Count);
            //---------------Execute Test ----------------------
            try
            {
                panelBuilder.BuildPanelForForm(null, GetControlFactory().CreateTabControl);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("uiForm", ex.ParamName);
            }


        }

        [Test]
        public void Test_BuildPanelForForm_NullCreateGroupControl_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = classDef.UIDefCol["TwoTabs"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, form.Count);
            //---------------Execute Test ----------------------
            try
            {
                panelBuilder.BuildPanelForForm(form, null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("groupControlCreator", ex.ParamName);
            }
        }

        [Test]
        public void Test_Constructor_NullControlFactory_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = classDef.UIDefCol["TwoTabs"].UIForm;
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, form.Count);
            //---------------Execute Test ----------------------
            try
            {
                new PanelBuilder(null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("controlFactory", ex.ParamName);
            }
        }

        [Test]
        public void Test_Set_GroupControlCreator_Null_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = classDef.UIDefCol["TwoTabs"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, form.Count);
            //---------------Execute Test ----------------------
            try
            {
                panelBuilder.GroupControlCreator = null;
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("value", ex.ParamName);
            }
        }

        [Test]
        public void Test_ConstructsWithDefault_ControlCreator()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = classDef.UIDefCol["TwoTabs"].UIForm;
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, form.Count);
            //---------------Execute Test ----------------------
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Test Result -----------------------
            Assert.IsNotNull(panelBuilder.GroupControlCreator);
        }

        [Test]
        public void Test_Set_GroupControlCreator_ToCollapsiblePanel()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = classDef.UIDefCol["TwoTabs"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            GroupControlCreator groupControlCreator = GetControlFactory().CreateCollapsiblePanelGroupControl;
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, form.Count);
            //---------------Execute Test ----------------------
            panelBuilder.GroupControlCreator = groupControlCreator;
            //---------------Test Result -----------------------
            Assert.IsNotNull(panelBuilder.GroupControlCreator);
            Assert.AreEqual(groupControlCreator, panelBuilder.GroupControlCreator);
        }

        [Test]
        public void Test_BuldUsingAlternateFormBuilder()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IUIForm form = classDef.UIDefCol["TwoTabs"].UIForm;
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, form.Count);
            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(form, GetControlFactory().CreateTabControl);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(ITabControl), panelInfo.Panel.Controls[0]);
            ITabControl tabControl = (ITabControl)panelInfo.Panel.Controls[0];
            ITabPage tabPage1 = tabControl.TabPages[0];
            ITabPage tabPage2 = tabControl.TabPages[1];
            Assert.AreEqual(1, tabPage1.Controls.Count);
            Assert.IsInstanceOf(typeof(IPanel), tabPage1.Controls[0]);
            IPanel tabPage1Panel = (IPanel)tabPage1.Controls[0];
            Assert.AreEqual(PanelBuilder.CONTROLS_PER_COLUMN, tabPage1Panel.Controls.Count);
            Assert.AreEqual(1, tabPage2.Controls.Count);
            Assert.IsInstanceOf(typeof(IPanel), tabPage2.Controls[0]);
            IPanel tabPage2Panel = (IPanel)tabPage2.Controls[0];
            Assert.AreEqual(PanelBuilder.CONTROLS_PER_COLUMN, tabPage2Panel.Controls.Count);
            Assert.AreEqual(30, panelInfo.MinimumPanelHeight);
            Assert.AreEqual(100, panelInfo.Panel.MinimumSize.Height);
            Assert.AreEqual(200, panelInfo.Panel.MinimumSize.Width);

        }
    }

    public class FakeUIFormField : UIFormField
    {
        private bool _keepValuePrivate;
        private IClassDef _classDef;

        public FakeUIFormField(string label, string propertyName)
            : base(label, propertyName)
        {
        }

        public FakeUIFormField(string label, string propertyName, Type controlType, string mapperTypeName, string mapperAssembly, bool editable, string toolTipText, Hashtable parameters, LayoutStyle layout)
            : base(label, propertyName, controlType, mapperTypeName, mapperAssembly, editable, toolTipText, parameters, layout)
        {
        }

        public FakeUIFormField(string label, string propertyName, string controlTypeName, string controlAssembly, string mapperTypeName, string mapperAssembly, bool editable, bool? showAsComulsory, string toolTipText, Hashtable parameters, LayoutStyle layout)
            : base(label, propertyName, controlTypeName, controlAssembly, mapperTypeName, mapperAssembly, editable, showAsComulsory, toolTipText, parameters, layout)
        {
        }

        public FakeUIFormField()
            : base(TestUtil.GetRandomString(), TestUtil.GetRandomString())
        {
        }

        public override bool KeepValuePrivate
        {
            get { return _keepValuePrivate; }
        }
        public void SetKeepValuePrivate(bool keepValuePrivate)
        {
            _keepValuePrivate = keepValuePrivate;
        }

        public override IClassDef GetClassDef()
        {
            return _classDef;
        }
        public void SetClassDef(IClassDef classDef)
        {
            _classDef = classDef;
        }
    }

}