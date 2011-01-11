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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test;
using Habanero.Test.BO;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
#pragma warning disable 618,612
    /// <summary>
    /// - check editable grid in actual win and giz applications (check all extra EditableGrid behaviour also,
    ///     including ComboBoxClickOnce, ConfirmDeletion and DeleteKeyBehaviour, speak to Eric for clarification)
    /// - ComboBox population
    /// - When filtering on win version, should selection move to top? (in similar way that on Giz it moves back to page 1)
    /// - Custom methods like one that changes behaviour of combobox clicking and pressing delete button
    /// </summary>
    public abstract class TestEditableGridControl
    {
        private const string _HABANERO_OBJECTID = "HABANERO_OBJECTID";

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
           // base.SetupDBConnection();
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract IControlFactory GetControlFactory();
        protected abstract IFormHabanero AddControlToForm(IControlHabanero cntrl);
        protected abstract void AssertIsTextBoxColumnType(IDataGridViewColumn dataGridViewColumn);
        protected abstract void AssertIsCheckBoxColumnType(IDataGridViewColumn dataGridViewColumn);
        protected abstract void AssertIsComboBoxColumnType(IDataGridViewColumn dataGridViewColumn);
        protected abstract void AssertComboBoxItemCount(IDataGridViewColumn dataGridViewColumn, int expectedCount);
        protected abstract void AssertMainControlsOnForm(IFormHabanero form);
        protected abstract IEditableGridControl CreateEditableGridControl();

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            gridControl.Width = 200;
            gridControl.Height = 133;
            //---------------Test Result -----------------------
            Assert.IsNotNull(gridControl);
            Assert.IsNotNull(gridControl.Grid);
            Assert.AreSame(gridControl.Grid, gridControl.Grid);
            Assert.AreEqual(3, gridControl.Controls.Count);
            Assert.AreEqual(gridControl.Width, gridControl.Grid.Width);
            int addedControlHeights = gridControl.FilterControl.Height +
                                      gridControl.Grid.Height +
                                      gridControl.Buttons.Height;
            Assert.AreEqual(gridControl.Height, addedControlHeights);


            Assert.IsFalse(gridControl.Grid.ReadOnly);
            Assert.IsTrue(gridControl.Grid.AllowUserToAddRows);
            Assert.IsTrue(gridControl.Grid.AllowUserToDeleteRows);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSetAllowUsersToAddRowsToFalse()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            IEditableGrid editableGrid = gridControl.Grid;
            //---------------Assert Precondition----------------
            Assert.IsTrue(editableGrid.AllowUserToAddRows);
            Assert.IsTrue(editableGrid.AllowUserToDeleteRows);
            //---------------Execute Test ----------------------
            editableGrid.AllowUserToAddRows = false;
            editableGrid.AllowUserToDeleteRows = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(editableGrid.AllowUserToAddRows);
            Assert.IsFalse(editableGrid.AllowUserToDeleteRows);
        }

        [Test]
        public void Test_CreateButtonsControl()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            //---------------Test Result -----------------------
            Assert.IsNotNull(gridControl.Buttons);
            Assert.AreEqual(2, gridControl.Buttons.Controls.Count);
        }


        [Test]
        public void Test_CreateFilterControl()
        {
            //---------------Set up test pack-------------------

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            //---------------Test Result -----------------------
            Assert.AreEqual(FilterModes.Filter, gridControl.FilterMode);
            Assert.AreEqual(FilterModes.Filter, gridControl.FilterControl.FilterMode);
        }

        [Test]
        public void TestLayoutManagerContainsAllMainControls()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            IFormHabanero form = AddControlToForm(gridControl);
            //---------------Test Result -----------------------
            AssertMainControlsOnForm(form);
        }

        [Test]
        public void TestInitialise()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            MyBO.LoadDefaultClassDef();
            IClassDef def = ClassDef.ClassDefs[typeof (MyBO)];
            //---------------Execute Test ----------------------
            gridControl.Initialise(def);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, gridControl.Grid.Columns.Count);
            Assert.IsFalse(gridControl.Grid.ReadOnly);
            Assert.IsTrue(gridControl.Grid.AllowUserToAddRows);
            Assert.IsTrue(gridControl.Grid.AllowUserToDeleteRows);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestInitialise_CopiesValuesToGrid()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            MyBO.LoadDefaultClassDef();
            IClassDef def = ClassDef.ClassDefs[typeof(MyBO)];
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            const string uiDefName = "Alternate";
            gridControl.Initialise(def, uiDefName);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, gridControl.Grid.Columns.Count);
            Assert.IsFalse(gridControl.Grid.ReadOnly);
            Assert.IsTrue(gridControl.Grid.AllowUserToAddRows);
            Assert.IsTrue(gridControl.Grid.AllowUserToDeleteRows);

            Assert.AreEqual(uiDefName, gridControl.UiDefName);
            Assert.AreEqual(uiDefName, gridControl.Grid.UiDefName);
            Assert.AreEqual(def, gridControl.ClassDef);
            Assert.AreEqual(def, gridControl.Grid.ClassDef);
        }

        [Test]
        public void Test_SetCollection()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            MyBO.LoadDefaultClassDef();
            IClassDef def = ClassDef.ClassDefs[typeof (MyBO)];
            gridControl.Initialise(def);
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            gridControl.Grid.BusinessObjectCollection  = col;
            //---------------Test Result -----------------------
            Assert.IsFalse(gridControl.Grid.ReadOnly);
            Assert.IsTrue(gridControl.Grid.AllowUserToAddRows);
            Assert.IsTrue(gridControl.Grid.AllowUserToDeleteRows);

            Assert.AreEqual(1, gridControl.Grid.Rows.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_Using_EditableDataSetProvider()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            MyBO.LoadDefaultClassDef();
            IClassDef def = ClassDef.ClassDefs[typeof (MyBO)];
            gridControl.Initialise(def);
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            gridControl.Grid.BusinessObjectCollection  = col;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof (EditableDataSetProvider), gridControl.Grid.DataSetProvider);
            //---------------Tear Down -------------------------          
        }

        [Test, Ignore("Cannot get this to work need to look at firing the events - June 2008")]
        public void Test_EditInTextbox_FirstRow()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl grid = GetControlFactory().CreateEditableGridControl();
            MyBO.LoadDefaultClassDef();
            IClassDef def = ClassDef.ClassDefs[typeof (MyBO)];
            grid.Initialise(def);
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(1, grid.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            grid.Grid.BusinessObjectCollection  = col;
            const string testvalue = "testvalue";
            grid.Grid.Rows[0].Cells[1].Value = testvalue;
//            grid.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.CreatedBusinessObjects.Count);
            MyBO newBo = col.CreatedBusinessObjects[0];
            Assert.AreEqual(testvalue, newBo.TestProp);
        }

        [Test, Ignore("Cannot get this to work need to look at firing the events - June 2008")]
        public void Test_EditInTextbox_ExistingObject()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl grid = GetControlFactory().CreateEditableGridControl();
            AddControlToForm(grid);
            MyBO.LoadDefaultClassDef();
            IClassDef def = ClassDef.ClassDefs[typeof (MyBO)];
            grid.Initialise(def);
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO bo = new MyBO();
            bo.TestProp = "testPropValue";
            col.Add(bo);
            grid.Grid.BusinessObjectCollection  = col;
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, grid.Grid.Rows.Count, "Editable auto adds adding row");

            //---------------Execute Test ----------------------


            const string testvalue = "new test value";
            grid.Grid.Rows[0].Cells[1].Value = testvalue;
            grid.Grid.Rows[1].Selected = true;
//            grid.ApplyChangesToBusinessObject();

            //---------------Test Result -----------------------
            Assert.AreEqual(testvalue, bo.TestProp);
        }


        [Test]
        public void TestSetupComboBoxFromClassDef()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWith_Grid_1ComboBoxColumn();
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            GridInitialiser gridInitialiser = new GridInitialiser(gridControl, GetControlFactory());

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(0, gridControl.Grid.Columns.Count);
            Assert.AreEqual(1, classDef.UIDefCol.Count);
            const string uiDefName = "default";
            IUIGrid uiGridDef = classDef.UIDefCol[uiDefName].UIGrid;
            Assert.IsNotNull(uiGridDef);
            Assert.AreEqual(1, uiGridDef.Count);

            //---------------Execute Test ----------------------
            gridInitialiser.InitialiseGrid(classDef, uiDefName);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, gridControl.Grid.Columns.Count, "Should have ID column and should have comboBoxColumn");
            IDataGridViewColumn dataGridViewColumn = gridControl.Grid.Columns[1];
            AssertIsComboBoxColumnType(dataGridViewColumn);
        }

        [Test]
        public void TestSetupComboBoxFromClassDef_SetsItemsInComboBox()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPerson.DeleteAllContactPeople();
            OrganisationTestBO.ClearAllFromDB();
            OrganisationTestBO.CreateSavedOrganisation();
            OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            TestUtil.WaitForGC();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.LoadAll();

            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            GridInitialiser gridInitialiser = new GridInitialiser(gridControl, GetControlFactory());

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(0, gridControl.Grid.Columns.Count);
            Assert.AreEqual(1, classDef.UIDefCol.Count);
            const string uiDefName = "default";
            IUIGrid uiGridDef = classDef.UIDefCol[uiDefName].UIGrid;
            Assert.IsNotNull(uiGridDef);
            Assert.AreEqual(1, uiGridDef.Count);
            Assert.AreEqual(1, col.Count);
            Assert.AreEqual(2,
                            BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<OrganisationTestBO>
                                ("").Count);

            //---------------Execute Test ----------------------
            gridInitialiser.InitialiseGrid(classDef, uiDefName);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, gridControl.Grid.Columns.Count, "Should have ID column and should have comboBoxColumn");
            IDataGridViewColumn dataGridViewColumn = gridControl.Grid.Columns[1];
            AssertIsComboBoxColumnType(dataGridViewColumn);
            AssertComboBoxItemCount(dataGridViewColumn, 3);
        }


        [Test]
        public void Test_SetBusinessObjectCollection_Null_ClearsTheGrid()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGridControl editableGridControl = CreateEditableGridControl();
            AddControlToForm(editableGridControl);
            editableGridControl.Grid.Columns.Add("TestProp", "TestProp");

            editableGridControl.SetBusinessObjectCollection(col);

            //----------------Assert Preconditions --------------

            Assert.IsTrue(editableGridControl.Grid.Rows.Count > 0, "There should be items in teh grid b4 clearing");
            //---------------Execute Test ----------------------
            editableGridControl.SetBusinessObjectCollection(null);
            //---------------Verify Result ---------------------
            Assert.AreEqual(0, editableGridControl.Grid.Rows.Count,
                            "There should be no items in the grid  after setting to null");

            Assert.IsFalse(editableGridControl.Buttons.Enabled);
            Assert.IsFalse(editableGridControl.FilterControl.Enabled);
            Assert.IsFalse(editableGridControl.Grid.AllowUserToAddRows);
        }


        [Test]
        public void Test_SetBusinessObjectCollection_Empty_HasOnlyOneRow()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            IEditableGridControl editableGridControl = CreateEditableGridControl();
            AddControlToForm(editableGridControl);

            //----------------Assert Preconditions --------------


            //---------------Execute Test ----------------------
            editableGridControl.SetBusinessObjectCollection(col);
            //---------------Verify Result ---------------------
            Assert.AreEqual(1, editableGridControl.Grid.Rows.Count,
                            "There should be one item in the grid  after setting to empty collection");
            Assert.IsTrue(editableGridControl.Grid.AllowUserToAddRows);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_NullCol_ThenNonNullEnablesButtons()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGridControl editableGridControl = CreateEditableGridControl();
            AddControlToForm(editableGridControl);
            editableGridControl.AllowUsersToAddBO = true;
            editableGridControl.SetBusinessObjectCollection(col);
            Assert.IsTrue(editableGridControl.Grid.AllowUserToAddRows);
            editableGridControl.SetBusinessObjectCollection(null);
            //----------------Assert Preconditions --------------
            Assert.IsFalse(editableGridControl.Buttons.Enabled);
            Assert.IsFalse(editableGridControl.FilterControl.Enabled);
            Assert.AreEqual(0, editableGridControl.Grid.Rows.Count);
            Assert.IsFalse(editableGridControl.Grid.AllowUserToAddRows);
            //---------------Execute Test ----------------------
            editableGridControl.SetBusinessObjectCollection(col);
            //---------------Verify Result ---------------------
            Assert.IsTrue(editableGridControl.Buttons.Enabled);
            Assert.IsTrue(editableGridControl.FilterControl.Enabled);
            Assert.AreEqual(5, editableGridControl.Grid.Rows.Count);
            Assert.IsTrue(editableGridControl.Grid.AllowUserToAddRows);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_InitialisesGridIfNotPreviouslyInitialised()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGridControl editableGridControl = CreateEditableGridControl();

            //---------------Execute Test ----------------------
            editableGridControl.SetBusinessObjectCollection(col);
            ////---------------Test Result -----------------------
            Assert.AreEqual("default", editableGridControl.UiDefName);
            Assert.AreEqual(col.ClassDef, editableGridControl.ClassDef);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_NotInitialiseGrid_IfPreviouslyInitialised()
        {
            //Verify that setting the collection for a grid that is already initialised
            //does not cause it to be reinitialised.
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            const string alternateUIDefName = "Alternate";
            IEditableGridControl editableGridControl = CreateEditableGridControl();

            editableGridControl.Initialise(classDef, alternateUIDefName);
            //---------------Execute Test ----------------------
            editableGridControl.SetBusinessObjectCollection(col);
            ////---------------Test Result -----------------------
            Assert.AreEqual(alternateUIDefName, editableGridControl.UiDefName);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_IncorrectClassDef()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGridControl editableGridControl = CreateEditableGridControl();
            //Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            //frm.Controls.Add((Gizmox.WebGUI.Forms.Control)readOnlyGridControl);
            AddControlToForm(editableGridControl);
            //---------------Execute Test ----------------------
            editableGridControl.Initialise(Sample.CreateClassDefVWG());
            try
            {
                editableGridControl.SetBusinessObjectCollection(col);
                Assert.Fail(
                    "You cannot call set collection for a collection that has a different class def than is initialised");
                ////---------------Test Result -----------------------
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(
                    "You cannot call set collection for a collection that has a different class def than is initialised",
                    ex.Message);
            }
        }


        [Test]
        public void Test_SetBusinessObjectCollection_NumberOfGridRows_Correct()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGridControl editableGridControl = CreateEditableGridControl();

            AddControlToForm(editableGridControl);
            IEditableGrid editableGrid = editableGridControl.Grid;
            editableGrid.Columns.Add("TestProp", "TestProp");

            //--------------Assert PreConditions----------------   
            Assert.AreEqual(1, editableGrid.Rows.Count);

            //---------------Execute Test ----------------------
            editableGridControl.SetBusinessObjectCollection(col);
            ////---------------Test Result -----------------------
            Assert.AreEqual(col.Count + 1, editableGrid.Rows.Count, "The number of items in the grid plus the null item");
        }
        [Test]
        public void Test_SetBusinessObjectCollection_WhenAllowAddFalse_ShouldNotChangeAllowAdd()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGridControl editableGridControl = CreateEditableGridControl();

            AddControlToForm(editableGridControl);
            IEditableGrid editableGrid = editableGridControl.Grid;
            editableGrid.Columns.Add("TestProp", "TestProp");
            editableGridControl.AllowUsersToAddBO = false;
            //--------------Assert PreConditions----------------   
            Assert.AreEqual(0, editableGrid.Rows.Count);
            Assert.IsFalse(editableGrid.AllowUserToAddRows);
            //---------------Execute Test ----------------------
            editableGridControl.SetBusinessObjectCollection(col);
            ////---------------Test Result -----------------------
            Assert.IsFalse(editableGrid.AllowUserToAddRows);
            Assert.IsFalse(editableGridControl.AllowUsersToAddBO);
            Assert.AreEqual(col.Count, editableGrid.Rows.Count, "The number of items in the col");
        }

        [Test]
        public void Test_AutoSelectsFirstItem()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGridControl editableGridControl = CreateEditableGridControl();
            AddControlToForm(editableGridControl);
            SetupGridColumnsForMyBo(editableGridControl.Grid);
            editableGridControl.AllowUsersToAddBO = true;
            //-----------------Assert Preconditions-----------------------
            Assert.AreEqual(1, editableGridControl.Grid.Rows.Count, "The number of items in the grid plus the null item");
            //---------------Execute Test ----------------------
            editableGridControl.BusinessObjectCollection = col;
            //---------------Test Result -----------------------
            Assert.AreEqual(col.Count + 1, editableGridControl.Grid.Rows.Count, "should be 4 item 1 adding item");
            Assert.IsNotNull(editableGridControl.SelectedBusinessObject);
            Assert.AreSame(col[0], editableGridControl.SelectedBusinessObject);

        }

        [Test]
        public void Test_SelectItem_SetsSelectedBO()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGridControl editableGridControl = CreateEditableGridControl();
            AddControlToForm(editableGridControl);
            SetupGridColumnsForMyBo(editableGridControl.Grid);
            editableGridControl.BusinessObjectCollection = col;
            MyBO myBO = col[2];
            //---------------Execute Test ----------------------
            editableGridControl.SelectedBusinessObject = myBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(col.Count + 1, editableGridControl.Grid.Rows.Count, "should be 4 item 1 adding item");
            Assert.AreSame(myBO, editableGridControl.SelectedBusinessObject);

        }

        [Test]
        public void Test_SelectIndex_SetsSelectedBO()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGridControl editableGridControl = CreateEditableGridControl();
            AddControlToForm(editableGridControl);
            SetupGridColumnsForMyBo(editableGridControl.Grid);
            editableGridControl.BusinessObjectCollection = col;
            MyBO myBO = col[2];
            //---------------Execute Test ----------------------
            editableGridControl.Grid.Rows[2].Selected = true;
            //---------------Test Result -----------------------
           Assert.AreEqual(col.Count + 1, editableGridControl.Grid.Rows.Count, "should be 4 item 1 adding item");
            Assert.AreSame(myBO, editableGridControl.SelectedBusinessObject);
        }

        [Ignore(" Cannot get the selected BO event to fire - 05 Mar 2009")]
        [Test]
        public void Test_BusinessObjectSelectEvent()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGridControl editableGridControl = CreateEditableGridControl();
            AddControlToForm(editableGridControl);
            SetupGridColumnsForMyBo(editableGridControl.Grid);
            editableGridControl.BusinessObjectCollection = col;
            IBusinessObject boFromEvent = null;
            bool eventFired = false;
            editableGridControl.BusinessObjectSelected += delegate(object sender, BOEventArgs e) 
                {
                    eventFired = true;
                    boFromEvent = e.BusinessObject; 
                };
            MyBO myBO = col[2];
            //---------------Execute Test ----------------------
            editableGridControl.Grid.Rows[2].Selected = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(eventFired, "Selected event should have been fired");
            Assert.AreEqual(col.Count + 1, editableGridControl.Grid.Rows.Count, "should be 4 item 1 adding item");
            Assert.AreSame(myBO, editableGridControl.SelectedBusinessObject);
            Assert.AreEqual(myBO, boFromEvent);
        }      
        [Test]
        public void Test_SetBusinessObject_BusinessObjectSelectEvent_FiresAndReturnsAValidBO()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGridControl editableGridControl = CreateEditableGridControl();
            AddControlToForm(editableGridControl);
            SetupGridColumnsForMyBo(editableGridControl.Grid);
            editableGridControl.BusinessObjectCollection = col;
            IBusinessObject boFromEvent = null;
            editableGridControl.BusinessObjectSelected += delegate(object sender, BOEventArgs e) 
                {
                    boFromEvent = e.BusinessObject; 
                };
            MyBO myBO = col[2];
            //---------------Execute Test ----------------------
            editableGridControl.SelectedBusinessObject = myBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(col.Count + 1, editableGridControl.Grid.Rows.Count, "should be 4 item 1 adding item");
            Assert.AreSame(myBO, editableGridControl.SelectedBusinessObject);
            Assert.AreEqual(myBO, boFromEvent);
        }


        [Test]
        public void Test_GetBusinessObjectCollection()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGridControl editableGridControl = CreateEditableGridControl();

            AddControlToForm(editableGridControl);

            editableGridControl.Grid.Columns.Add("TestProp", "TestProp");
            editableGridControl.SetBusinessObjectCollection(col);
            //---------------Assert Preconditions --------------
            //---------------Execute Test ----------------------
            IBusinessObjectCollection returnedBusinessObjectCollection =
                editableGridControl.GetBusinessObjectCollection();
            //---------------Test Result -----------------------
            Assert.AreSame(col, returnedBusinessObjectCollection);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_AfterChanged()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            BusinessObjectCollection<MyBO> col2 = new BusinessObjectCollection<MyBO>();
            IEditableGridControl editableGridControl = CreateEditableGridControl();

            AddControlToForm(editableGridControl);

            editableGridControl.Grid.Columns.Add("TestProp", "TestProp");
            editableGridControl.SetBusinessObjectCollection(col);
            //---------------Assert Preconditions --------------
            Assert.AreSame(col, editableGridControl.GetBusinessObjectCollection());
            //---------------Execute Test ----------------------
            editableGridControl.SetBusinessObjectCollection(col2);
            IBusinessObjectCollection returnedBusinessObjectCollection =
                editableGridControl.GetBusinessObjectCollection();
            //---------------Test Result -----------------------
            Assert.AreSame(col2, returnedBusinessObjectCollection);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_WhenNull()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl editableGridControl = CreateEditableGridControl();
            //---------------Assert Preconditions --------------
            //---------------Execute Test ----------------------
            IBusinessObjectCollection returnedBusinessObjectCollection =
                editableGridControl.GetBusinessObjectCollection();
            //---------------Test Result -----------------------
            Assert.IsNull(returnedBusinessObjectCollection);
        }

        [Test]
        public void TestSetupColumnAsTextBoxType_FromClassDef()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWith_Grid_1TextboxColumn();
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            GridInitialiser gridInitialiser = new GridInitialiser(gridControl, GetControlFactory());

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(0, gridControl.Grid.Columns.Count);
            Assert.AreEqual(1, classDef.UIDefCol.Count);
            const string uiDefName = "default";
            IUIGrid uiGridDef = classDef.UIDefCol[uiDefName].UIGrid;
            Assert.IsNotNull(uiGridDef);
            Assert.AreEqual(1, uiGridDef.Count);

            //---------------Execute Test ----------------------
            gridInitialiser.InitialiseGrid(classDef, uiDefName);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, gridControl.Grid.Columns.Count, "Should have ID column and should have textBoxColumn");
            IDataGridViewColumn dataGridViewColumn = gridControl.Grid.Columns[1];
            AssertIsTextBoxColumnType(dataGridViewColumn);
            //---------------Tear Down -------------------------        
        }

        [Test]
        public void TestSetupColumnAsCheckBoxType_FromClassDef()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWith_Grid_1CheckBoxColumn();
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            GridInitialiser gridInitialiser = new GridInitialiser(gridControl, GetControlFactory());

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(0, gridControl.Grid.Columns.Count);
            Assert.AreEqual(1, classDef.UIDefCol.Count);
            const string uiDefName = "default";
            IUIGrid uiGridDef = classDef.UIDefCol[uiDefName].UIGrid;
            Assert.IsNotNull(uiGridDef);
            Assert.AreEqual(1, uiGridDef.Count);

            //---------------Execute Test ----------------------
            gridInitialiser.InitialiseGrid(classDef, uiDefName);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, gridControl.Grid.Columns.Count, "Should have ID column and should have checkBoxColumn");
            IDataGridViewColumn dataGridViewColumn = gridControl.Grid.Columns[1];
            AssertIsCheckBoxColumnType(dataGridViewColumn);
            //---------------Tear Down -------------------------        
        }
//
//        [Test,
//         Ignore(
//             "This test is failing since we added the filtercontrol and buttons to the layout manager in the EditableGridControlWin constructor"
//             )]


        [Test]
        public void TestButtonsControl_ClickSaveAcceptsChanges()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            //---------------Clean from previous tests----------
            const string originalText = "testsavechanges";
            const string newText = "testsavechanges_edited";
            Criteria criteria = new Criteria("TestProp", Criteria.ComparisonOp.Equals, originalText);
//            MyBO oldBO1 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<MyBO>(criteria);
//            if (oldBO1 != null)
//            {
//                oldBO1.MarkForDelete();
//                oldBO1.Save();
//            }
//            MyBO oldBO2 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<MyBO>(criteria);
//            if (oldBO2 != null)
//            {
//                oldBO2.MarkForDelete();
//                oldBO2.Save();
//            }

            MyBO bo = new MyBO {TestProp = originalText};
            bo.Save();

            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO> { bo, new MyBO { TestProp = "SomeText" } };

            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            AddControlToForm(gridControl.Grid);
            SetupGridColumnsForMyBo(gridControl.Grid);
            gridControl.Grid.BusinessObjectCollection  = col;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, gridControl.Grid.Rows.Count);
            Assert.AreEqual(originalText, gridControl.Grid.Rows[0].Cells[1].Value);
            criteria = new Criteria("TestProp", Criteria.ComparisonOp.Equals, newText);
            MyBO nullBO = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<MyBO>(criteria);
//            MyBO nullBO = BOLoader.Instance.GetBusinessObject<MyBO>("TestProp='" + newText + "'");
            Assert.IsNull(nullBO);
            //---------------Execute Test ----------------------
            gridControl.Grid.Rows[0].Cells[1].Value = newText;
            //---------------Assert Precondition----------------
            Assert.AreEqual(newText, gridControl.Grid.Rows[0].Cells[1].Value);
            gridControl.Grid.SelectedBusinessObject = col[1];
            //---------------Execute Test ----------------------
            gridControl.Buttons["Save"].PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(newText, gridControl.Grid.Rows[0].Cells[1].Value);
            criteria = new Criteria("TestProp", Criteria.ComparisonOp.Equals, newText);
            MyBO savedBO = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<MyBO>(criteria);
//            MyBO savedBO = BOLoader.Instance.GetBusinessObject<MyBO>("TestProp='" + newText + "'");
            Assert.IsNotNull(savedBO);
            //---------------Tear Down--------------------------
            savedBO.MarkForDelete();
            savedBO.Save();
        }

        [Test]
        public void TestAcceptance_FilterGrid()
        {
            //---------------Set up test pack-------------------
            //Get Grid with 4 items
            BusinessObjectCollection<MyBO> col;
            IEditableGridControl gridControl = GetGridWith_5_Rows(out col);
            //AddControlToForm(readOnlyGridControl);
            ITextBox tb = gridControl.FilterControl.AddStringFilterTextBox("Test Prop", "TestProp");
            //--------------Assert PreConditions
            Assert.AreEqual(5, gridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            //enter data in filter for 1 item
            tb.Text = "b";
            gridControl.FilterControl.ApplyFilter();
            //---------------Assert Result -----------------------
            // verify that grid has only 2 items in it (includes new row)
            Assert.AreEqual(2, gridControl.Grid.Rows.Count);
        }

        [Test]
        public void TestFixBug_SearchGridSearchesTheGrid_DoesNotCallFilterOnGridbase()
        {
            //FirstName is not in the grid def therefore if the grid calls the filter gridbase filter
            // the dataview will try to filter with a column that does not exist this will raise an error
            //---------------Set up test pack-------------------
            //Clear all contact people from the DB
            ContactPerson.DeleteAllContactPeople();
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
            CreateContactPersonInDB();

            //Create grid setup for search
            IEditableGridControl gridControl = CreateEditableGridControl();

            ITextBox txtboxFirstName = gridControl.FilterControl.AddStringFilterTextBox("FirstName", "FirstName");
            gridControl.Initialise(classDef);
            gridControl.FilterMode = FilterModes.Search;
            //---------------Execute Test ----------------------
            txtboxFirstName.Text = "FFF";
            gridControl.FilterControl.ApplyFilter();
            //---------------Test Result -----------------------
            Assert.IsTrue(true); //No error was thrown by the grid.
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAcceptance_SearchGridSearchesTheGrid()
        {
            //---------------Set up test pack-------------------
            //Clear all contact people from the DB
            ContactPerson.DeleteAllContactPeople();
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
            //Create data in the database with the 5 contact people two with Search in surname
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            //Create grid setup for search
            IEditableGridControl gridControl = CreateEditableGridControl();
            ITextBox txtbox = gridControl.FilterControl.AddStringFilterTextBox("Surname", "Surname");
            gridControl.Initialise(classDef);
            gridControl.FilterMode = FilterModes.Search;

            //--------------Assert PreConditions----------------            
            //No items in the grid
            Assert.AreEqual(1, gridControl.Grid.Rows.Count);

            //---------------Execute Test ----------------------
            //set data in grid to a value that should return 2 people
            const string filterByValue = "SSSSS";
            txtbox.Text = filterByValue;
            //grid.filtercontrols.searchbutton.click
            gridControl.OrderBy = "Surname";
            gridControl.FilterControl.ApplyFilter();

            //---------------Test Result -----------------------
            StringAssert.Contains(filterByValue,
                                  gridControl.FilterControl.GetFilterClause().GetFilterClauseString());
            //verify that there are 2 people in the grid.
            Assert.AreEqual(3, gridControl.Grid.Rows.Count);

            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load("Surname like %" + filterByValue + "%", "Surname");
            Assert.AreEqual(col.Count + 1, gridControl.Grid.Rows.Count);
            int rowNum = 0;
            foreach (ContactPersonTestBO person in col)
            {
                object rowID = gridControl.Grid.Rows[rowNum++].Cells["HABANERO_OBJECTID"].Value;
                Assert.AreEqual(person.ID.ToString(), rowID.ToString());
            }
            //---------------Tear Down -------------------------   
            ContactPerson.DeleteAllContactPeople();
        }

        [Test]
        public void TestFilterControlIsBuiltFromDef()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadDefaultClassDefWithFilterDef();
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            gridControl.Initialise(classDef);
            //---------------Test Result -----------------------
            Assert.IsTrue(gridControl.FilterControl.Visible);
            Assert.AreEqual(FilterModes.Filter, gridControl.FilterControl.FilterMode);
            Assert.IsNotNull(gridControl.FilterControl.GetChildControl("TestProp"));
            Assert.IsNotNull(gridControl.FilterControl.GetChildControl("TestProp2"));
            //---------------Tear Down -------------------------          
        }

        protected abstract IClassDef LoadMyBoDefaultClassDef();

        private static BusinessObjectCollection<MyBO> CreateCollectionWith_4_Objects()
        {
            MyBO cp = new MyBO {TestProp = "b"};
            MyBO cp2 = new MyBO {TestProp = "d"};
            MyBO cp3 = new MyBO {TestProp = "c"};
            MyBO cp4 = new MyBO {TestProp = "a"};
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO> {{cp, cp2, cp3, cp4}};
            return col;
        }

        protected IEditableGridControl GetGridWith_5_Rows(out BusinessObjectCollection<MyBO> col)
        {
            LoadMyBoDefaultClassDef();
            col = CreateCollectionWith_4_Objects();
            IEditableGridControl gridControl = CreateEditableGridControl();
            SetupGridColumnsForMyBo(gridControl.Grid);
            gridControl.SetBusinessObjectCollection(col);
            return gridControl;
        }

        private static void SetupGridColumnsForMyBo(IDataGridView gridBase)
        {
            gridBase.Columns.Add(_HABANERO_OBJECTID, _HABANERO_OBJECTID);
            gridBase.Columns.Add("TestProp", "TestProp");
        }

        private static void CreateContactPersonInDB()
        {
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Surname = Guid.NewGuid().ToString("N");
            contactPersonTestBO.Save();
            return;
        }

        private static void CreateContactPersonInDB_With_SSSSS_InSurname()
        {
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Surname = Guid.NewGuid().ToString("N") + "SSSSS";
            contactPersonTestBO.Save();
            return;
        }


    }
#pragma warning restore 618,612
}