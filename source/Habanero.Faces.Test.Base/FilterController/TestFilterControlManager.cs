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
using Habanero.Base;
using Habanero.Base.Util;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base.FilterController
{
    public abstract class TestFilterControlManager
    {
        protected abstract IControlFactory GetControlFactory();

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
        }

        [Test]
        public void TestCreateFilterControlManager()
        {
            //---------------Set up test pack-------------------
            GridLayoutManager layoutManager = new GridLayoutManager(GetControlFactory().CreatePanel(),
                                                                    GetControlFactory());
            //---------------Execute Test ----------------------
            FilterControlManager filterControlManager = new FilterControlManager(GetControlFactory(), layoutManager);
            //---------------Test Result -----------------------
            Assert.AreEqual(layoutManager, filterControlManager.LayoutManager);
        }

        [Test]
        public void ClearFilters_FIXBUG()
        {
            //---------------Set up test pack-------------------
            GridLayoutManager layoutManager = new GridLayoutManager(GetControlFactory().CreatePanel(),
                                                                    GetControlFactory());
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            FilterControlManager filterControlManager = new FilterControlManager(GetControlFactory(), layoutManager);
            filterControlManager.AddStringFilterTextBox("Test:", "TestColumn");
            filterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpLike, "testvalue");

            filterControlManager.AddStringFilterTextBox("Test2:", "TestColumn2");
            filterControlManager.GetFilterClause().GetFilterClauseString();
            //---------------Execute Test ----------------------
            filterControlManager.ClearFilters();
            //---------------Test Result -----------------------
            Assert.IsTrue(true, "Should not cause an Erro");
        }

        [Test]
        public void Test_AddStaticFilterClause()
        {
            //---------------Set up test pack-------------------
            GridLayoutManager layoutManager = new GridLayoutManager(GetControlFactory().CreatePanel(), GetControlFactory());
            FilterControlManager filterControlManager = new FilterControlManager(GetControlFactory(), layoutManager);
            //---------------Execute Test ----------------------
            const string propertyName = "TestColumn2";
            const string filtervalue = "FilterValue";
            filterControlManager.AddStaticStringFilterClause(propertyName, FilterClauseOperator.OpGreaterThan, filtervalue);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, filterControlManager.FilterControls.Count);
            ICustomFilter control = filterControlManager.FilterControls[0];
            Assert.AreEqual(propertyName, control.PropertyName);
            Assert.AreEqual(FilterClauseOperator.OpGreaterThan, control.FilterClauseOperator);
            IFilterClause filterClause = control.GetFilterClause(new DataViewFilterClauseFactory());
            Assert.AreEqual(string.Format("{0} > '{1}'", propertyName, filtervalue), filterClause.GetFilterClauseString()); 
        }

        [Test]
        public void TestAdd_DateRangeFilterComboBox()
        {
            //---------------Set up test pack-------------------

            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            //---------------Execute Test ----------------------
            IDateRangeComboBox dr1 = filterControl.AddDateRangeFilterComboBox("test", "test", true, true);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, filterControl.FilterControls.Count);
            Assert.IsTrue(filterControl.FilterPanel.Controls.Contains(dr1));
        }

        [Test]
        public void TestAdd_DateRangeFilterComboBoxInclusive()
        {
            //---------------Set up test pack-------------------

            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            DateTime testDate = new DateTime(2007, 1, 2, 3, 4, 5, 6);

            //---------------Execute Test ----------------------
            IDateRangeComboBox dr1 = filterControl.AddDateRangeFilterComboBox("test", "test", true, true);
            dr1.UseFixedNowDate = true;
            dr1.FixedNowDate = testDate;
            dr1.SelectedItem = "Today";
            IFilterClause clause1 =
                filterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpGreaterThanOrEqualTo,
                                                           new DateTime(2007, 1, 2, 0, 0, 0));
            IFilterClause clause2 =
                filterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpLessThanOrEqualTo,
                                                           new DateTime(2007, 1, 2, 23, 59, 59, 999));
            IFilterClause compClause =
                filterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
            //---------------Test Result -----------------------

            Assert.AreEqual(compClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        }
        [Test]
        public void TestAdd_DateRangeFilterComboBox_WhenExclusiveNotDefined_ShouldSetAsInclusive()
        {
            //---------------Set up test pack-------------------

            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            DateTime testDate = new DateTime(2007, 1, 2, 3, 4, 5, 6);

            //---------------Execute Test ----------------------
            IDateRangeComboBox dr1 = filterControl.AddDateRangeFilterComboBox("test", "test");
            dr1.UseFixedNowDate = true;
            dr1.FixedNowDate = testDate;
            dr1.SelectedItem = "Today";
            IFilterClause clause1 =
                filterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpGreaterThanOrEqualTo,
                                                           new DateTime(2007, 1, 2, 0, 0, 0));
            IFilterClause clause2 =
                filterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpLessThanOrEqualTo,
                                                           new DateTime(2007, 1, 2, 23, 59, 59, 999));
            IFilterClause compClause =
                filterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
            //---------------Test Result -----------------------

            Assert.AreEqual(compClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        }

        [Test]
        public void TestAdd_DateRangeFilterComboBoxExclusive()
        {
            //---------------Set up test pack-------------------

            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            DateTime testDate = new DateTime(2007, 1, 2, 3, 4, 5, 6);

            //---------------Execute Test ----------------------
            IDateRangeComboBox dr1 = filterControl.AddDateRangeFilterComboBox("test", "test", false, false);
            dr1.UseFixedNowDate = true;
            dr1.FixedNowDate = testDate;
            dr1.SelectedItem = "Today";
            IFilterClause clause1 =
                filterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpGreaterThan,
                                                           new DateTime(2007, 1, 2, 0, 0, 0));
            IFilterClause clause2 =
                filterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpLessThan,
                                                           new DateTime(2007, 1, 2, 23, 59, 59, 999));
            IFilterClause compClause =
                filterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
            //---------------Test Result -----------------------

            Assert.AreEqual(compClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        }

        [Test]
        public void TestAdd_DateRangeFilterComboBoxOverload()
        {
            //---------------Set up test pack-------------------

            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            List<DateRangeOptions> options = new List<DateRangeOptions>();
            options.Add(DateRangeOptions.Today);
            options.Add(DateRangeOptions.Yesterday);

            //---------------Execute Test ----------------------
            IDateRangeComboBox dateRangeCombo =
                filterControl.AddDateRangeFilterComboBox("test", "test", options, true, false);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, dateRangeCombo.Items.Count);
        }

        [Test]
        public void TestAdd_CustomFilter()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            ICustomFilter customFilter = new CustomFilterStub(GetControlFactory());

            //---------------Execute Test ----------------------
            filterControl.AddCustomFilter("LabelText", customFilter);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, filterControl.FilterControls.Count);
            IControlHabanero controlHabanero = filterControl.GetChildControl("test");
            Assert.AreEqual(customFilter.Control, controlHabanero);
            Assert.IsNotNull(controlHabanero);
            Assert.IsInstanceOf(typeof (ITextBox), controlHabanero);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCustomFilterValue_changedFiresWhenTextChanged()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            ICustomFilter customFilter = new CustomFilterStub(GetControlFactory());
            filterControl.AddCustomFilter("LabelText", customFilter);

            //---------------Assert pre conditions--------------
            Assert.IsFalse(((CustomFilterStub) customFilter)._valueChangedFired);

            //---------------Execute Test ----------------------
            customFilter.Control.Text = "newText";

            //---------------Test Result -----------------------
            Assert.IsTrue(((CustomFilterStub) customFilter)._valueChangedFired);
        }

        [Test]
        public void TestAddTextBox()
        {
            //---------------Set up test pack-------------------
            IFilterControl ctl = GetControlFactory().CreateFilterControl();

            //---------------Execute Test ----------------------
            ITextBox myTextBox = ctl.AddStringFilterTextBox("", "");

            //---------------Test Result -----------------------
            Assert.IsNotNull(myTextBox);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddStringFilterTextBox()
        {
            //---------------Set up test pack-------------------
            IFilterClause nullClause = new DataViewNullFilterClause();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            //---------------Execute Test ----------------------
            ITextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");
            tb.Text = "";
            //---------------Test Result -----------------------
            Assert.AreEqual(nullClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
            Assert.AreEqual(1, filterControl.FilterControls.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAdd_TwoStringFilterTextBox()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            //---------------Execute Test ----------------------
            filterControl.AddStringFilterTextBox("Test:", "TestColumn");
            filterControl.AddStringFilterTextBox("Test2:", "TestColumn2");
            //---------------Test Result -----------------------
            Assert.AreEqual(2, filterControl.FilterControls.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAdd_TwoStringFilterTextBox_GetControl()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            ITextBox tbExpected = filterControl.AddStringFilterTextBox("Test:", "TestColumn");
            filterControl.AddStringFilterTextBox("Test2:", "TestColumn2");
            //---------------Execute Test ----------------------
            ITextBox tbReturned = (ITextBox) filterControl.GetChildControl("TestColumn");
            //---------------Test Result -----------------------
            Assert.AreSame(tbExpected, tbReturned);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAdd_TwoStringFilterTextBox_Combo_GetControl()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IComboBox tbExpected = filterControl.AddStringFilterComboBox("Test:", "TestColumn", new[] {""}, false);
            filterControl.AddStringFilterTextBox("Test2:", "TestColumn2");
            //---------------Execute Test ----------------------
            IComboBox tbReturned = (IComboBox) filterControl.GetChildControl("TestColumn");
            //---------------Test Result -----------------------
            Assert.AreSame(tbExpected, tbReturned);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAdd_TwoStringFilterTextBox_DateTime__GetControl()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IDateTimePicker tbExpected =
                filterControl.AddDateFilterDateTimePicker("Test:", "TestColumn", DateTime.Now,
                                                          FilterClauseOperator.OpEquals, false);
            filterControl.AddStringFilterTextBox("Test2:", "TestColumn2");
            //---------------Execute Test ----------------------
            IDateTimePicker tbReturned = (IDateTimePicker) filterControl.GetChildControl("TestColumn");
            //---------------Test Result -----------------------
            Assert.AreSame(tbExpected, tbReturned);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAdd_AddBooleanFilterCheckBox_CheckBox__GetControl()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            ICheckBox tbExpected = filterControl.AddBooleanFilterCheckBox("Test:", "TestColumn", false);
            filterControl.AddStringFilterTextBox("Test2:", "TestColumn2");
            //---------------Execute Test ----------------------
            ICheckBox tbReturned = (ICheckBox) filterControl.GetChildControl("TestColumn");
            //---------------Test Result -----------------------
            Assert.AreSame(tbExpected, tbReturned);
        }

        [Test]
        public void TestAdd_AddBooleanFilterCheckBox_CheckBox()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();

            //---------------Execute Test ----------------------
            ICheckBox cb = filterControl.AddBooleanFilterCheckBox("Test:", "TestColumn", false);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, filterControl.FilterPanel.Controls.Count);
            Assert.AreSame(cb, filterControl.FilterPanel.Controls[1]);
        }

        [Test]
        public void TestAdd_AddBooleanFilterComboBox_GetControl()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IComboBox cbExpected = filterControl.AddBooleanFilterComboBox("Test:", "TestColumn", false);
            filterControl.AddStringFilterTextBox("Test2:", "TestColumn2");
            //---------------Assert Precondition----------------
            Assert.IsNotNull(cbExpected);
            //---------------Execute Test ----------------------
            IComboBox cbReturned = (IComboBox)filterControl.GetChildControl("TestColumn");
            //---------------Test Result -----------------------
            Assert.AreSame(cbExpected, cbReturned);
        }

        [Test]
        public void TestAdd_AddBooleanFilterComboBox_WhenDefaultFalse_ShouldSetFalse()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();

            //---------------Execute Test ----------------------
            IComboBox cb = filterControl.AddBooleanFilterComboBox("Test:", "TestColumn", false);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, filterControl.FilterPanel.Controls.Count);
            Assert.AreSame(cb, filterControl.FilterPanel.Controls[1]);
            Assert.AreEqual("False", cb.SelectedItem);
        }

        [Test]
        public void TestAdd_AddBooleanFilterComboBox_WhenDefaultTrue_shouldSetTrue()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();

            //---------------Execute Test ----------------------
            IComboBox cb = filterControl.AddBooleanFilterComboBox("Test:", "TestColumn", true);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, filterControl.FilterPanel.Controls.Count);
            Assert.AreSame(cb, filterControl.FilterPanel.Controls[1]);
            Assert.AreEqual("True", cb.SelectedItem);
        }

        [Test]
        public void TestAdd_AddBooleanFilterComboBox_WhenDefaultNull_shouldSetNull()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();

            //---------------Execute Test ----------------------
            IComboBox cb = filterControl.AddBooleanFilterComboBox("Test:", "TestColumn", null);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, filterControl.FilterPanel.Controls.Count);
            Assert.AreSame(cb, filterControl.FilterPanel.Controls[1]);
            Assert.AreEqual("", cb.SelectedItem);
        }

        [Test]
        public void TestGetTextBoxFilterClause()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory itsFilterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            ITextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");

            //---------------Execute Test ----------------------
            tb.Text = "testvalue";
            string filterClauseString = filterControl.GetFilterClause().GetFilterClauseString();

            //---------------Test Result -----------------------
            IFilterClause clause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpLike, "testvalue");
            Assert.AreEqual(clause.GetFilterClauseString(), filterClauseString);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetTextBoxFilterClause_Equals()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory itsFilterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            ITextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn", FilterClauseOperator.OpEquals);

            //---------------Execute Test ----------------------
            tb.Text = "testvalue";
            string filterClauseString = filterControl.GetFilterClause().GetFilterClauseString();

            //---------------Test Result -----------------------
            IFilterClause clause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "testvalue");
            Assert.AreEqual(clause.GetFilterClauseString(), filterClauseString);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestTwoStringTextBoxFilter()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory itsFilterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            ITextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");
            tb.Text = "testvalue";
            ITextBox tb2 = filterControl.AddStringFilterTextBox("Test:", "TestColumn2");
            tb2.Text = "testvalue2";

            //---------------Execute Test ----------------------
            string filterClauseString = filterControl.GetFilterClause().GetFilterClauseString();

            //---------------Test Result -----------------------
            IFilterClause clause1 =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpLike, "testvalue");
            IFilterClause clause2 =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn2", FilterClauseOperator.OpLike, "testvalue2");
            IFilterClause fullClause =
                itsFilterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
            Assert.AreEqual(fullClause.GetFilterClauseString(), filterClauseString);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestLabelAndTextBoxAreOnPanel()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();

            //---------------Assert Preconditions --------------
            Assert.AreEqual(0, filterControl.FilterPanel.Controls.Count);

            //---------------Execute Test ----------------------
            ITextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");

            //---------------Test Result -----------------------

            Assert.AreEqual(2, filterControl.FilterPanel.Controls.Count);
            Assert.IsTrue(filterControl.FilterPanel.Controls.Contains(tb));
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddComboBox()
        {
            //---------------Set up test pack-------------------
            //IFilterClause nullClause = new DataViewNullFilterClause();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            //---------------Execute Test ----------------------
            IComboBox cb = filterControl.AddStringFilterComboBox("t", "TestColumn", new ArrayList(), true);

            //---------------Test Result -----------------------
            Assert.IsNotNull(cb);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddStringFilterComboBox()
        {
            //---------------Set up test pack-------------------
            IFilterClause nullClause = new DataViewNullFilterClause();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            //---------------Execute Test ----------------------
            filterControl.AddStringFilterComboBox("Test:", "TestColumn", new ArrayList(), true);
            //---------------Test Result -----------------------
            Assert.AreEqual(nullClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());

            //---------------Tear Down -------------------------          
        }

        public enum PurchaseOrderStatus
        {
            Open,
            Processed
        }

        [Test]
        public void TestAddEnumFilterComboBox()
        {
            //---------------Set up test pack-------------------
            IFilterClause nullClause = new DataViewNullFilterClause();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            //---------------Execute Test ----------------------
            filterControl.AddEnumFilterComboBox("Test:", "TestColumn", typeof(PurchaseOrderStatus));
            //---------------Test Result -----------------------
            Assert.AreEqual(nullClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetComboBoxAddSelectedItems()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IList options = new ArrayList();
            options.Add("1");
            options.Add("2");
            //---------------Execute Test ----------------------
            IComboBox comboBox = filterControl.AddStringFilterComboBox("Test:", "TestColumn", options, true);
            //---------------Test Result -----------------------
            int numOfItemsInCollection = 2;
            int numItemsExpectedInComboBox = numOfItemsInCollection + 1; //one extra for the null selected item
            Assert.AreEqual(numItemsExpectedInComboBox, comboBox.Items.Count);
        }

        [Test]
        public void TestSelectItem()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IList options = new ArrayList();
            options.Add("1");
            options.Add("2");
            IComboBox comboBox = filterControl.AddStringFilterComboBox("Test:", "TestColumn", options, true);
            //---------------Execute Test ----------------------
            comboBox.SelectedIndex = 1;
            //---------------Test Result -----------------------
            Assert.AreEqual("1", comboBox.SelectedItem.ToString());
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetComboBoxFilterClause()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IComboBox comboBox = TestFilterControlManager.GetFilterComboBox_2Items(filterControl);

            //---------------Execute Test ----------------------
            comboBox.SelectedIndex = 1;
            string filterClauseString = filterControl.GetFilterClause().GetFilterClauseString();

            //---------------Test Result -----------------------
            IFilterClause clause =
                filterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "1");
            Assert.AreEqual(clause.GetFilterClauseString(), filterClauseString);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetComboBoxFilterClauseNoSelection()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IComboBox comboBox = TestFilterControlManager.GetFilterComboBox_2Items(filterControl);
            //---------------Execute Test ----------------------
            comboBox.SelectedIndex = -1;
            string filterClauseString = filterControl.GetFilterClause().GetFilterClauseString();
            //---------------Test Result -----------------------
            IFilterClause clause = filterClauseFactory.CreateNullFilterClause();
            Assert.AreEqual(clause.GetFilterClauseString(), filterClauseString);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetComboBoxFilterClause_SelectDeselect()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IComboBox comboBox = TestFilterControlManager.GetFilterComboBox_2Items(filterControl);
            //---------------Execute Test ----------------------
            comboBox.SelectedIndex = 1;
            comboBox.SelectedIndex = -1;
            string filterClauseString = filterControl.GetFilterClause().GetFilterClauseString();
            //---------------Test Result -----------------------
            IFilterClause nullClause = filterClauseFactory.CreateNullFilterClause();
            Assert.AreEqual(nullClause.GetFilterClauseString(), filterClauseString);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_ClearStringComboBoxFilter_ShouldClearTextInCombo()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IComboBox comboBox = filterControl.AddStringFilterComboBox("Test", "Test", new string[] {"One", "Two", "Three"}, true);
            comboBox.SelectedItem = "Two";
            //---------------Assert preconditions---------------
            Assert.AreEqual("Two", comboBox.Text);
            //---------------Execute Test ----------------------
            filterControl.ClearFilters();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, comboBox.SelectedIndex);
            Assert.AreEqual("", comboBox.Text);
        }

        [Test]
        public void Test_ClearDateRangeComboBoxFilter_ShouldClearTextInCombo()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IComboBox comboBox = filterControl.AddDateRangeFilterComboBox("Test", "Test");
            comboBox.SelectedItem = "Today";
            //---------------Assert preconditions---------------
            Assert.AreEqual("Today", comboBox.Text);
            //---------------Execute Test ----------------------
            filterControl.ClearFilters();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, comboBox.SelectedIndex);
            Assert.AreEqual("(Date Ranges)", comboBox.Text);
        }

        [Test]
        public void Test_ClearEnumComboBoxFilter_ShouldClearTextInCombo()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IComboBox comboBox = filterControl.AddEnumFilterComboBox("Test", "Test", typeof(PurchaseOrderStatus));
            comboBox.SelectedItem = PurchaseOrderStatus.Processed;
            //---------------Assert preconditions---------------
            Assert.AreEqual("Processed", comboBox.Text);
            //---------------Execute Test ----------------------
            filterControl.ClearFilters();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, comboBox.SelectedIndex);
            Assert.AreEqual("", comboBox.Text);
        }

        private static IComboBox GetFilterComboBox_2Items(IFilterControl filterControl)
        {
            IList options = new ArrayList();
            options.Add("1");
            options.Add("2");
            return filterControl.AddStringFilterComboBox("Test:", "TestColumn", options, true);
        }

        [Test]
        public void Test_Construct_BoolComboBoxFilter_ShouldSetPropNameAndFilterOperator()
        {
            //---------------Set up test pack-------------------
            const string expectedPropName = "TestColumn";            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var boolComboBoxFilter = new BoolComboBoxFilter(GetControlFactory(), expectedPropName);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedPropName, boolComboBoxFilter.PropertyName);
            Assert.AreEqual(FilterClauseOperator.OpEquals, boolComboBoxFilter.FilterClauseOperator);
            Assert.IsInstanceOf<IComboBox>(boolComboBoxFilter.Control);
        }
        [Test]
        public void Test_Construct_BoolComboBoxFilter_ShouldSetOptionsTrueAndFalse()
        {
            //---------------Set up test pack-------------------
            const string expectedPropName = "TestColumn";            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var boolComboBoxFilter = new BoolComboBoxFilter(GetControlFactory(), expectedPropName);
            //---------------Test Result -----------------------
            var comboBox = (IComboBox)boolComboBoxFilter.Control;
            Assert.AreEqual(3, comboBox.Items.Count);
            Assert.IsTrue(comboBox.Items.Contains("True"), "Should Contain True");
            Assert.IsTrue(comboBox.Items.Contains("False"), "Should Contain False");
        }
    }


}