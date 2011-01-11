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
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base.FilterController
{
    public abstract class TestFilterControl
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
        public void TestSetLayoutManager()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IPanel panel = GetControlFactory().CreatePanel();
            GridLayoutManager layoutManager = new GridLayoutManager(panel, GetControlFactory());
            //---------------Execute Test ----------------------
            filterControl.LayoutManager = layoutManager;
            //---------------Test Result -----------------------
            Assert.AreEqual(layoutManager, filterControl.LayoutManager);
            Assert.IsNotNull(filterControl.FilterPanel);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void MultipleFilters()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            ITextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");
            tb.Text = "testvalue";
            IFilterClause clause =
                filterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpLike, "testvalue");

            ITextBox tb2 = filterControl.AddStringFilterTextBox("Test2:", "TestColumn2");
            tb2.Text = "testvalue2";
            //---------------Execute Test ----------------------

            string filterClause = filterControl.GetFilterClause().GetFilterClauseString();
            //---------------Test Result -----------------------
            IFilterClause clause2 =
                filterClauseFactory.CreateStringFilterClause("TestColumn2", FilterClauseOperator.OpLike, "testvalue2");

            IFilterClause compositeClause =
                filterClauseFactory.CreateCompositeFilterClause(clause, FilterClauseCompositeOperator.OpAnd, clause2);

            Assert.AreEqual(compositeClause.GetFilterClauseString(),
                            filterClause);
            //---------------Tear Down ------------------------- 
        }

        [Test]
        public void Test_ClearFilters()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            ITextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");
            tb.Text = "testvalue";
            filterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpLike, "testvalue");

            ITextBox tb2 = filterControl.AddStringFilterTextBox("Test2:", "TestColumn2");
            tb2.Text = "testvalue2";
            string initialFilterClause = filterControl.GetFilterClause().GetFilterClauseString();
            //---------------Assert Precondition----------------
            Assert.IsFalse(string.IsNullOrEmpty(initialFilterClause), "Should not be empty : " + initialFilterClause);
            //---------------Execute Test ----------------------
            filterControl.ClearFilters();
            //---------------Test Result -----------------------
            string finalFilterClause = filterControl.GetFilterClause().GetFilterClauseString();
            Assert.IsTrue(string.IsNullOrEmpty(finalFilterClause), "Should be empty : " + finalFilterClause);

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
                                                           new DateTime(2007, 1, 2, 3, 4, 5));
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
                                                           new DateTime(2007, 1, 2, 3, 4, 5));
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
            Assert.IsInstanceOf(typeof(ITextBox),controlHabanero);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCustomFilterValue_changedFiresWhenTextChanged()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            ICustomFilter customFilter = new CustomFilterStub(GetControlFactory());
#pragma warning disable 618,612
            filterControl.AddCustomFilter("LabelText", "test", customFilter);
#pragma warning restore 618,612

            //---------------Assert pre conditions--------------
            Assert.IsFalse(((CustomFilterStub)customFilter)._valueChangedFired);

            //---------------Execute Test ----------------------
            customFilter.Control.Text = "newText";

            //---------------Test Result -----------------------
            Assert.IsTrue(((CustomFilterStub)customFilter)._valueChangedFired);
        }


        [Test]
        public void Test_AddStaticFilterClause()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            //---------------Execute Test ----------------------
            const string propertyName = "TestColumn2";
            const string filtervalue = "FilterValue";
            filterControl.AddStaticStringFilterClause(propertyName, FilterClauseOperator.OpGreaterThan, filtervalue);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, filterControl.FilterControls.Count);
            ICustomFilter control = filterControl.FilterControls[0];
            Assert.AreEqual(propertyName, control.PropertyName);
            Assert.AreEqual(FilterClauseOperator.OpGreaterThan, control.FilterClauseOperator);
            IFilterClause filterClause = control.GetFilterClause(new DataViewFilterClauseFactory());
            Assert.AreEqual(string.Format("{0} > '{1}'", propertyName, filtervalue), filterClause.GetFilterClauseString());
        }

        private static IComboBox GetFilterComboBox_2Items(IFilterControl filterControl)
        {
            IList options = new ArrayList();
            options.Add("1");
            options.Add("2");
            return filterControl.AddStringFilterComboBox("Test:", "TestColumn", options, true);
        }

        #region TextBoxFilter

        #region TestAddTextBox

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

        #endregion


        #region TestAddStringFilterTextBox

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
        public void TestAddMulitplePropStringFilterTextBox()
        {
            //---------------Set up test pack-------------------
            IFilterClause nullClause = new DataViewNullFilterClause();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            //---------------Execute Test ----------------------
            List<string> props = new List<string>{"TestColumn"};
            ITextBox tb = filterControl.AddMultiplePropStringTextBox("Test:", props);
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
            IComboBox tbExpected = filterControl.AddStringFilterComboBox("Test:", "TestColumn", new string[] {""}, false);
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
        public void TestAdd_TwoStringFilterTextBox_CheckBox__GetControl()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            ICheckBox tbExpected = filterControl.AddBooleanFilterCheckBox("Test:", "TestColumn", false);
            filterControl.AddStringFilterTextBox("Test2:", "TestColumn2");
            //---------------Execute Test ----------------------
            ICheckBox tbReturned = (ICheckBox) filterControl.GetChildControl("TestColumn");
            //---------------Test Result -----------------------
            Assert.AreSame(tbExpected, tbReturned);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAdd_TwoStringFilterTextBox_CheckBox()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();

            //---------------Execute Test ----------------------
            ICheckBox cb = filterControl.AddBooleanFilterCheckBox("Test:", "TestColumn", false);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, filterControl.FilterPanel.Controls.Count);
            Assert.AreSame(cb, filterControl.FilterPanel.Controls[1]);
            //---------------Tear Down -------------------------          
        }

        #endregion

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
        public void TestGetMulitplePropTextBoxFilterClause()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory itsFilterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            ITextBox tb = filterControl.AddMultiplePropStringTextBox("Test:", new List<string>{"TestColumn"});

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
        public void TestGetMultiplePropTextBoxFilterClause_Equals()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory itsFilterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            ITextBox tb = filterControl.AddMultiplePropStringTextBox("Test:", new List<string> { "TestColumn" }, FilterClauseOperator.OpEquals);

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
        #endregion

        #region ComboBoxFilter

        //------------------------COMBO BOX----------------------------------------------------------

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
            const int numOfItemsInCollection = 2;
            const int numItemsExpectedInComboBox = numOfItemsInCollection + 1;
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
            IComboBox comboBox = GetFilterComboBox_2Items(filterControl);

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
            IComboBox comboBox = GetFilterComboBox_2Items(filterControl);
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
            IComboBox comboBox = GetFilterComboBox_2Items(filterControl);
            //---------------Execute Test ----------------------
            comboBox.SelectedIndex = 1;
            comboBox.SelectedIndex = -1;
            string filterClauseString = filterControl.GetFilterClause().GetFilterClauseString();
            //---------------Test Result -----------------------
            IFilterClause nullClause = filterClauseFactory.CreateNullFilterClause();
            Assert.AreEqual(nullClause.GetFilterClauseString(), filterClauseString);
            //---------------Tear Down -------------------------          
        }


        #endregion

        #region Nested type: TestFilterControlVWG


        #endregion

        #region Nested type: TestFilterControlWin


        #endregion


    }

    internal class CustomFilterStub : ICustomFilter
    {
        private readonly IControlFactory _factory;
        private static ITextBox _box;
        public bool _valueChangedFired;

        public CustomFilterStub(IControlFactory factory)
        {
            _factory = factory;
            _box = _factory.CreateTextBox();
            ValueChanged += CustomFilterStub_ValueChanged;
            _box.TextChanged += ValueChanged;
            _valueChangedFired = false;
        }

        void CustomFilterStub_ValueChanged(object sender, EventArgs e)
        {
            _valueChangedFired = true;
        }

        public IControlHabanero Control
        {
            get
            {
                return _box;
            }
        }

        public IFilterClause GetFilterClause(IFilterClauseFactory filterClauseFactory)
        {
            return filterClauseFactory.CreateStringFilterClause("test", FilterClauseOperator.OpEquals, _box.Text);
        }

        public void Clear()
        {
            _box.Text = "";
        }

        public event EventHandler ValueChanged;
        public string PropertyName { get { return "test"; } }
        public FilterClauseOperator FilterClauseOperator { get { return FilterClauseOperator.OpLike; } }
    }
}