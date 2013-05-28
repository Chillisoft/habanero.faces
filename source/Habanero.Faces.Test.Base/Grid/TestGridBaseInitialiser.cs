using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Test.Base
{
    [TestFixture]
    public abstract class TestGridBaseInitialiser:TestBaseWithDisposing
    {
        private const string _gridIdColumnName = "HABANERO_OBJECTID";

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            // base.SetupDBConnection();
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract IControlFactory GetControlFactory();
        protected abstract void AddControlToForm(IControlHabanero cntrl);
        protected abstract Type GetDateTimeGridColumnType();
        protected abstract Type GetComboBoxGridColumnType();
        protected abstract void AssertGridColumnTypeAfterCast(IDataGridViewColumn createdColumn, Type expectedColumnType);


        [Test]
        public void Test_Construct_WithNullGridBase_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new GridBaseInitialiser(null, MockRepository.GenerateStub<IControlFactory>());
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("gridBase", ex.ParamName);
            }
        }

        [Test]
        public void Test_InitialiseGrid_NoClassDef_NoColumnsDefined()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
//            IGridInitialiser initialiser = new GridBaseInitialiser(grid.Grid, GetControlFactory());
            //--------------Assert PreConditions----------------            
            Assert.IsFalse(grid.IsInitialised);

            //---------------Execute Test ----------------------
            try
            {
                grid.Initialise();
                Assert.Fail("Should raise error");
            }
            catch (GridBaseInitialiseException ex)
            {
                StringAssert.Contains
                    ("You cannot call initialise with no classdef since the ID column has not been added to the grid",
                     ex.Message);
            }
            //---------------Test Result -----------------------

            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestInitialiseGrid()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = LoadMyBoDefaultClassDef();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridBaseInitialiser(grid.Grid, GetControlFactory());
            IUIDef uiDef = classDef.UIDefCol["default"];
            IUIGrid uiGridDef = uiDef.UIGrid;
            //---------------Assert Preconditions---------------
            Assert.AreEqual(2, uiGridDef.Count, "2 defined columns in the defaultDef");
//            Assert.AreEqual("", grid.UiDefName);
            Assert.IsNull(grid.ClassDef);
            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef);

            //---------------Test Result -----------------------
            Assert.AreEqual("default", grid.UiDefName);
            Assert.AreEqual(classDef, grid.Grid.ClassDef);
            Assert.AreEqual
                (uiGridDef.Count + 1, grid.Grid.Columns.Count,
                 "There should be 1 ID column and 2 defined columns in the defaultDef");
            Assert.IsTrue(initialiser.IsInitialised);
//            Assert.IsTrue(grid.IsInitialised);
        }

        [Test]
        public void TestInitGrid_DefaultUIDef_VerifyColumnsSetupCorrectly()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = LoadMyBoDefaultClassDef();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridBaseInitialiser(grid.Grid, GetControlFactory());
            IUIDef uiDef = classDef.UIDefCol["default"];
            IUIGrid uiGridDef = uiDef.UIGrid;
            GlobalUIRegistry.DateDisplaySettings = new DateDisplaySettings();
            GlobalUIRegistry.DateDisplaySettings.GridDateFormat = "dd MMM yyyy";
            //---------------Assert Preconditions---------------
            Assert.AreEqual(2, uiGridDef.Count, "2 defined columns in the defaultDef");
            IUIGridColumn columnDef1 = uiGridDef[0];
            Assert.AreEqual("TestProp", columnDef1.PropertyName);
            IUIGridColumn columnDef2 = uiGridDef[1];
            Assert.AreEqual("TestProp2", columnDef2.PropertyName);
            Assert.IsNotNull(GlobalUIRegistry.DateDisplaySettings);
            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef);

            //---------------Test Result -----------------------
            IDataGridViewColumn idColumn = grid.Grid.Columns[0];
            AssertVerifyIDFieldSetUpCorrectly(idColumn);

            IDataGridViewColumn dataColumn1 = grid.Grid.Columns[1];
            AssertThatDataColumnSetupCorrectly(classDef, columnDef1, dataColumn1);
            Assert.AreEqual("", dataColumn1.DefaultCellStyle.Format);
            IDataGridViewColumn dataColumn2 = grid.Grid.Columns[2];
            AssertThatDataColumnSetupCorrectly(classDef, columnDef2, dataColumn2);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestInitGrid_UIDef_ZeroWidthColumn_HidesColumn()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWith_Grid_2Columns_1stHasZeroWidth();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridBaseInitialiser(grid.Grid, GetControlFactory());
            IUIDef uiDef = classDef.UIDefCol["default"];
            IUIGrid uiGridDef = uiDef.UIGrid;
            //---------------Assert Preconditions---------------
            Assert.AreEqual(2, uiGridDef.Count, "2 defined columns in the defaultDef");
            IUIGridColumn columnDef1 = uiGridDef[0];
            Assert.AreEqual("TestProp", columnDef1.PropertyName);
            Assert.AreEqual(0, columnDef1.Width);
            IUIGridColumn columnDef2 = uiGridDef[1];
            Assert.AreEqual("TestProp2", columnDef2.PropertyName);
            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef);

            //---------------Test Result -----------------------
            IDataGridViewColumn dataColumn1 = grid.Grid.Columns[1];
            AssertThatDataColumnSetupCorrectly(classDef, columnDef1, dataColumn1);

            IDataGridViewColumn dataColumn2 = grid.Grid.Columns[2];
            AssertThatDataColumnSetupCorrectly(classDef, columnDef2, dataColumn2);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestInitGrid_UIDef_DateFormat_FormatsDateColumn()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithDateTimeParameterFormat();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridBaseInitialiser(grid.Grid, GetControlFactory());
            IUIDef uiDef = classDef.UIDefCol["default"];
            IUIGrid uiGridDef = uiDef.UIGrid;
            AddControlToForm(grid);
            GlobalUIRegistry.DateDisplaySettings = new DateDisplaySettings();
            GlobalUIRegistry.DateDisplaySettings.GridDateFormat = "dd MMM yyyy";
            //--------------Assert PreConditions----------------            
            const string formattedPropertyName = "TestDateTimeFormat";
            Assert.IsNotNull(uiGridDef[formattedPropertyName]);
            Assert.IsNotNull(uiGridDef["TestDateTimeNoFormat"]);
            Assert.IsNotNull(uiGridDef["TestDateTime"]);

            Assert.IsNull(uiGridDef["TestDateTimeNoFormat"].GetParameterValue("dateFormat"));
            Assert.IsNull(uiGridDef["TestDateTime"].GetParameterValue("dateFormat"));
            object dateFormatObject = uiGridDef[formattedPropertyName].GetParameterValue("dateFormat");
            string dateFormatParameter = dateFormatObject.ToString();
            const string expectedFormat = "dd.MMM.yyyy";
            Assert.AreEqual(expectedFormat, dateFormatParameter);

            MyBO myBo = new MyBO();
            DateTime currentDateTime = DateTime.Now;
            myBo.SetPropertyValue(formattedPropertyName, currentDateTime);
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            col.Add(myBo);

            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef);
            grid.BusinessObjectCollection = col;

            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.AreEqual(1, grid.Grid.Rows.Count);
            IDataGridViewCell dataGridViewCell = grid.Grid.Rows[0].Cells[formattedPropertyName];
            //((DataGridViewCellVWG) dataGridViewCell).DataGridViewCell.HasStyle = false;
            Assert.AreSame(typeof (DateTime), dataGridViewCell.ValueType);
            Assert.AreEqual(currentDateTime.ToString(expectedFormat), dataGridViewCell.FormattedValue);

            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestInitGrid_UIDef_CurrencyFormat_ShouldFormatColumn()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithCurrencyParameterFormat_VirtualProp();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridBaseInitialiser(grid.Grid, GetControlFactory());
            IUIDef uiDef = classDef.UIDefCol["default"];
            IUIGrid uiGridDef = uiDef.UIGrid;
            AddControlToForm(grid);

            //--------------Assert PreConditions----------------            
            const string formattedPropertyName = "-MyVirtualDoubleProp-";
            Assert.IsNotNull(uiGridDef[formattedPropertyName]);

            object currencyFormat = uiGridDef[formattedPropertyName].GetParameterValue("currencyFormat");
            Assert.IsNotNull(currencyFormat);

            string currencyFormatParameter = currencyFormat.ToString();
            const string expectedFormat = "### ###.##";
            Assert.AreEqual(expectedFormat, currencyFormatParameter);

            MyBO myBo = new MyBO();
            double currencyValue = myBo.MyVirtualDoubleProp;
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            col.Add(myBo);

            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef);
            grid.BusinessObjectCollection = col;

            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.AreEqual(1, grid.Grid.Rows.Count);
            IDataGridViewCell dataGridViewCell = grid.Grid.Rows[0].Cells[formattedPropertyName];
            //((DataGridViewCellVWG) dataGridViewCell).DataGridViewCell.HasStyle = false;
            Assert.AreSame(typeof(Double), dataGridViewCell.ValueType);
            Assert.AreEqual(currencyValue.ToString(expectedFormat), dataGridViewCell.FormattedValue);      
        }

        [Test]
        public void TestInitGrid_UIDef_CurrencyFormat_DataTypeDecimal_ShouldFormatColumn()
        {
            //---------------Set up test pack-------------------
            GridBaseInitialiserSpy gridInitialiser = new GridBaseInitialiserSpy(MockRepository.GenerateStub<IGridBase>(), MockRepository.GenerateStub<IControlFactory>());

            IUIGridColumn gridColumn = MockRepository.GenerateStub<IUIGridColumn>();
            string expectedFormat = "fdafasdfsda";
            gridColumn.Stub(column => column.GetParameterValue("currencyFormat")).Return(expectedFormat);
            IDataGridViewColumn gridViewColumn = MockRepository.GenerateStub<IDataGridViewColumn>();

            gridViewColumn.DefaultCellStyle = MockRepository.GenerateStub<IDataGridViewCellStyle>();
            //--------------Assert PreConditions----------------            
            Assert.AreNotEqual(expectedFormat, gridViewColumn.DefaultCellStyle.Format); 

            //---------------Execute Test ----------------------
            gridInitialiser.CallSetupCurrencyWithParameters(typeof (Decimal), gridColumn, gridViewColumn);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedFormat, gridViewColumn.DefaultCellStyle.Format);    
        }

        [Test]
        public void TestInitGrid_UIDef_CurrencyFormat_WhenVirtualProp_ShouldFormatColumn()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithCurrencyParameterFormat();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridBaseInitialiser(grid.Grid, GetControlFactory());
            IUIDef uiDef = classDef.UIDefCol["default"];
            IUIGrid uiGridDef = uiDef.UIGrid;
            AddControlToForm(grid);

            //--------------Assert PreConditions----------------            
            const string formattedPropertyName = "TestCurrencyFormat";
            Assert.IsNotNull(uiGridDef[formattedPropertyName]);
            Assert.IsNotNull(uiGridDef[formattedPropertyName].GetParameterValue("currencyFormat"));

            const string unformattedPropName = "TestCurrencyNoFormat";
            Assert.IsNotNull(uiGridDef[unformattedPropName]);
            Assert.IsNull(uiGridDef[unformattedPropName].GetParameterValue("currencyFormat"));

            object currencyFormat = uiGridDef[formattedPropertyName].GetParameterValue("currencyFormat");
            string currencyFormatParameter = currencyFormat.ToString();
            const string expectedFormat = "### ###.##";
            Assert.AreEqual(expectedFormat, currencyFormatParameter);

            MyBO myBo = new MyBO();
            const double currencyValue = 222222.55555d;
            myBo.SetPropertyValue(formattedPropertyName, currencyValue);
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            col.Add(myBo);

            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef);
            grid.BusinessObjectCollection = col;

            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.AreEqual(1, grid.Grid.Rows.Count);
            IDataGridViewCell dataGridViewCell = grid.Grid.Rows[0].Cells[formattedPropertyName];
            //((DataGridViewCellVWG) dataGridViewCell).DataGridViewCell.HasStyle = false;
            Assert.AreSame(typeof(Double), dataGridViewCell.ValueType);
            Assert.AreEqual(currencyValue.ToString(expectedFormat), dataGridViewCell.FormattedValue);
        }
        [Test]
        public void TestInitGrid_GlobalDateFormat_FormatsDateColumn()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithDateTimeParameterFormat();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridBaseInitialiser(grid.Grid, GetControlFactory());
            IUIDef uiDef = classDef.UIDefCol["default"];
            IUIGrid uiGridDef = uiDef.UIGrid;
            AddControlToForm(grid);
            const string dateTimeNoFormatPropertyName = "TestDateTimeNoFormat";

            const string expectedFormat = "dd.MMM.yyyy";
            GlobalUIRegistry.DateDisplaySettings = new DateDisplaySettings();
            GlobalUIRegistry.DateDisplaySettings.GridDateFormat = expectedFormat;
            //--------------Assert PreConditions----------------            
            IUIGridColumn dateTimeNoFormatGridColumn = uiGridDef[dateTimeNoFormatPropertyName];
            Assert.IsNotNull(dateTimeNoFormatGridColumn);
            Assert.IsNull(dateTimeNoFormatGridColumn.GetParameterValue("dateFormat"));
            Assert.IsNotNull(GlobalUIRegistry.DateDisplaySettings);
            Assert.AreEqual(expectedFormat, GlobalUIRegistry.DateDisplaySettings.GridDateFormat);

            MyBO myBo = new MyBO();
            DateTime currentDateTime = DateTime.Now;
            myBo.SetPropertyValue(dateTimeNoFormatPropertyName, currentDateTime);
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            col.Add(myBo);

            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef);
            grid.BusinessObjectCollection = col;

            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.AreEqual(1, grid.Grid.Rows.Count);
            IDataGridViewCell dataGridViewCell = grid.Grid.Rows[0].Cells[dateTimeNoFormatPropertyName];
            //((DataGridViewCellVWG) dataGridViewCell).DataGridViewCell.HasStyle = false;
            Assert.AreSame(typeof(DateTime), dataGridViewCell.ValueType);
            Assert.AreEqual(currentDateTime.ToString(expectedFormat), dataGridViewCell.FormattedValue);

            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestInitGrid_WithNonDefaultUIDef()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = LoadMyBoDefaultClassDef();
            const string alternateUIDefName = "Alternate";
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridBaseInitialiser(grid.Grid, GetControlFactory());
            IUIDef uiDef = classDef.UIDefCol[alternateUIDefName];
            IUIGrid uiGridDef = uiDef.UIGrid;
            //---------------Assert Preconditions---------------
            Assert.AreEqual(1, uiGridDef.Count, "1 defined column in the alternateUIDef");
            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef, alternateUIDefName);

            //---------------Test Result -----------------------
            Assert.AreEqual(alternateUIDefName, grid.UiDefName);
            Assert.AreEqual(classDef, grid.Grid.ClassDef);
            Assert.AreEqual
                (uiGridDef.Count + 1, grid.Grid.Columns.Count,
                 "There should be 1 ID column and 1 defined column in the alternateUIDef");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestInitGrid_Twice_Fail()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridBaseInitialiser(grid.Grid, GetControlFactory());
            IClassDef classDef = LoadMyBoDefaultClassDef();
            //---------------Assert Preconditions---------------
            IUIDef uiDef = classDef.UIDefCol["default"];
            IUIGrid uiGridDef = uiDef.UIGrid;
            //---------------Assert Preconditions---------------
            Assert.AreEqual(2, uiGridDef.Count, "2 defined columns in the defaultDef");
            IUIGridColumn columnDef1 = uiGridDef[0];
            Assert.AreEqual("TestProp", columnDef1.PropertyName);
            IUIGridColumn columnDef2 = uiGridDef[1];
            Assert.AreEqual("TestProp2", columnDef2.PropertyName);
            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef);

            initialiser.InitialiseGrid(classDef);
            //---------------Test Result -----------------------
            IDataGridViewColumn idColumn = grid.Grid.Columns[0];
            AssertVerifyIDFieldSetUpCorrectly(idColumn);

            IDataGridViewColumn dataColumn1 = grid.Grid.Columns[1];
            AssertThatDataColumnSetupCorrectly(classDef, columnDef1, dataColumn1);

            IDataGridViewColumn dataColumn2 = grid.Grid.Columns[2];
            AssertThatDataColumnSetupCorrectly(classDef, columnDef2, dataColumn2);
        }

        [Test]
        public void TestInitGrid_WithInvalidUIDef()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = LoadMyBoDefaultClassDef();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridBaseInitialiser(grid.Grid, GetControlFactory());

            //---------------Execute Test ----------------------
            try
            {
                initialiser.InitialiseGrid(classDef, "NonExistantUIDef");
                Assert.Fail("Should raise an error if the class def does not have the UIDef");
                //---------------Test Result -----------------------
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(" does not contain a definition for UIDef ", ex.Message);
            }
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestInitGrid_With_NoGridDef()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = LoadMyBoDefaultClassDef();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridBaseInitialiser(grid.Grid, GetControlFactory());
            //---------------Execute Test ----------------------
            try
            {
                initialiser.InitialiseGrid(classDef, "AlternateNoGrid");
                Assert.Fail("Should raise an error if the class def does not the GridDef");
                //---------------Test Result -----------------------
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains
                    (" does not contain a grid definition for UIDef AlternateNoGrid for the class def ", ex.Message);
            }
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestInitGrid_With_GridDef()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = LoadMyBoDefaultClassDef();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            IGridInitialiser initialiser = new GridBaseInitialiser(grid.Grid, GetControlFactory());
            IUIGrid uiGridDef = classDef.UIDefCol["default"].UIGrid;
            //---------------Assert Preconditions---------------
            Assert.AreEqual(2, uiGridDef.Count, "2 defined columns in the defaultDef");
            IUIGridColumn columnDef1 = uiGridDef[0];
            Assert.AreEqual("TestProp", columnDef1.PropertyName);
            IUIGridColumn columnDef2 = uiGridDef[1];
            Assert.AreEqual("TestProp2", columnDef2.PropertyName);
            //---------------Execute Test ----------------------

            initialiser.InitialiseGrid(classDef, uiGridDef, "test");
            //---------------Test Result -----------------------
            IDataGridViewColumn idColumn = grid.Grid.Columns[0];
            AssertVerifyIDFieldSetUpCorrectly(idColumn);

            IDataGridViewColumn dataColumn1 = grid.Grid.Columns[1];
            AssertThatDataColumnSetupCorrectly(classDef, columnDef1, dataColumn1);

            IDataGridViewColumn dataColumn2 = grid.Grid.Columns[2];
            AssertThatDataColumnSetupCorrectly(classDef, columnDef2, dataColumn2);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public virtual void TestInitGrid_LoadsDataGridViewDateTimeColumn()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithDateTimeParameterFormat();
            IEditableGridControl grid = GetControlFactory().CreateEditableGridControl();
            DisposeOnTearDown(grid);
            IGridInitialiser initialiser = new GridBaseInitialiser(grid.Grid, GetControlFactory());
            IUIDef uiDef = classDef.UIDefCol["default"];
            IUIGrid uiGridDef = uiDef.UIGrid;
            IUIGridColumn uiDTColDef = uiGridDef[2];
            uiDTColDef.GridControlTypeName = "DataGridViewDateTimeColumn";
            AddControlToForm(grid);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef);
            //---------------Test Result -----------------------
            Assert.AreEqual(6, grid.Grid.Columns.Count);
            IDataGridViewColumn column3 = grid.Grid.Columns[3];
            Assert.AreEqual("TestDateTime", column3.Name);
            Assert.AreEqual(uiDTColDef.Heading, column3.HeaderText);
            Assert.IsInstanceOf(typeof(IDataGridViewColumn), column3);
            AssertGridColumnTypeAfterCast(column3, GetDateTimeGridColumnType());
        }

        [Test]
        public virtual void TestInitGrid_LoadsDataGridViewComboBoxColumn()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWith_Grid_1ComboBoxColumn();
            IEditableGridControl grid = GetControlFactory().CreateEditableGridControl();
            DisposeOnTearDown(grid);
            IGridInitialiser initialiser = new GridBaseInitialiser(grid.Grid, GetControlFactory());
            IUIDef uiDef = classDef.UIDefCol["default"];
            IUIGrid uiGridDef = uiDef.UIGrid;
            IUIGridColumn uiComboColDef = uiGridDef[0];
            uiComboColDef.GridControlTypeName = "DataGridViewComboBoxColumn";
            AddControlToForm(grid);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, grid.Grid.Columns.Count);
            IDataGridViewColumn column1 = grid.Grid.Columns[1];
            Assert.AreEqual("RelatedID", column1.Name);
            Assert.AreEqual(uiComboColDef.Heading, column1.HeaderText);
            Assert.IsInstanceOf(typeof(IDataGridViewComboBoxColumn), column1);
            AssertGridColumnTypeAfterCast(column1, GetComboBoxGridColumnType());
        }

        [Test]
        public virtual void TestInitGrid_LoadsDataGridViewComboBoxColumn_WhenPropDefMissing()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWith_Grid_1ComboBoxColumn();
            IEditableGridControl grid = GetControlFactory().CreateEditableGridControl();
            DisposeOnTearDown(grid);
            IGridInitialiser initialiser = new GridBaseInitialiser(grid.Grid, GetControlFactory());
            IUIDef uiDef = classDef.UIDefCol["default"];
            IUIGrid uiGridDef = uiDef.UIGrid;
            IUIGridColumn uiComboColDef = uiGridDef[0];
            uiComboColDef.GridControlTypeName = "DataGridViewComboBoxColumn";
            uiComboColDef.PropertyName = "OtherProp";
            AddControlToForm(grid);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, grid.Grid.Columns.Count);
            IDataGridViewColumn column1 = grid.Grid.Columns[1];
            Assert.AreEqual("OtherProp", column1.Name);
            Assert.AreEqual(uiComboColDef.Heading, column1.HeaderText);
            Assert.IsInstanceOf(typeof(IDataGridViewComboBoxColumn), column1);
            AssertGridColumnTypeAfterCast(column1, GetComboBoxGridColumnType());
        }

        private IReadOnlyGridControl CreateReadOnlyGridControl()
        {
            return GetControlledLifetimeFor(GetControlFactory().CreateReadOnlyGridControl());
        }

        private static IClassDef LoadMyBoDefaultClassDef()
        {
            return MyBO.LoadDefaultClassDef();
        }

        private static void AssertVerifyIDFieldSetUpCorrectly(IDataGridViewColumn column)
        {
            const string idPropertyName = _gridIdColumnName;
            Assert.AreEqual(idPropertyName, column.Name);
            Assert.AreEqual(idPropertyName, column.HeaderText);
            Assert.AreEqual(idPropertyName, column.DataPropertyName);
            Assert.IsTrue(column.ReadOnly);
            Assert.IsFalse(column.Visible);
            Assert.AreEqual(typeof (string), column.ValueType);
        }

        private static void AssertThatDataColumnSetupCorrectly
            (IClassDef classDef, IUIGridColumn columnDef1, IDataGridViewColumn dataColumn1)
        {
            Assert.AreEqual(columnDef1.PropertyName, dataColumn1.DataPropertyName); //Test Prop
            Assert.AreEqual(columnDef1.PropertyName, dataColumn1.Name);
            Assert.AreEqual(columnDef1.GetHeading(), dataColumn1.HeaderText);
            Assert.AreEqual(columnDef1.Width != 0, dataColumn1.Visible);
//            Assert.IsTrue(dataColumn1.ReadOnly); TODO: put this test into the readonlygridinitialiser
            int expectedWidth = columnDef1.Width;
            if (expectedWidth == 0) expectedWidth = 5;
            Assert.AreEqual(expectedWidth, dataColumn1.Width);
            PropDef propDef = (PropDef) GetPropDef(classDef, columnDef1);
            Assert.AreEqual(propDef.PropertyType, dataColumn1.ValueType);
        }

        private static IPropDef GetPropDef(IClassDef classDef, IUIGridColumn gridColumn)
        {
            /*            IPropDef propDef = null;
                        if (classDef.PropDefColIncludingInheritance.Contains(gridColumn.PropertyName))
                        {
                            propDef = classDef.PropDefColIncludingInheritance[gridColumn.PropertyName];
                        }*/

            return classDef.GetPropDef(gridColumn.PropertyName, false);
        }
    }

    internal class GridBaseInitialiserSpy: GridBaseInitialiser
    {
        
        ///<summary>
        /// Initialise the grid with the appropriate control factory.
        ///</summary>
        ///<param name="gridBase"></param>
        ///<param name="controlFactory"></param>
        internal GridBaseInitialiserSpy(IGridBase gridBase, IControlFactory controlFactory): base(gridBase, controlFactory)
        {
        }
            //        IUIGridColumn gridColumn = MockRepository.GenerateStub<IUIGridColumn>();
            //string expectedFormat = "fdafasdfsda";
            //gridColumn.Stub(column => column.GetParameterValue("currencyFormat")).Return(expectedFormat);
            //IDataGridViewColumn gridViewColumn = MockRepository.GenerateStub<IDataGridViewColumn>();

            //gridViewColumn.DefaultCellStyle = MockRepository.GenerateStub<IDataGridViewCellStyle>();
            ////--------------Assert PreConditions----------------            
            //Assert.AreNotEqual(expectedFormat, gridViewColumn.DefaultCellStyle.Format); 

            ////---------------Execute Test ----------------------
            //gridInitialiser.CallSetupCurrencyWithParameters(typeof (Decimal), gridColumn, gridViewColumn);
        internal void CallSetupCurrencyWithParameters(Type propertyType, IUIGridColumn gridColumn, IDataGridViewColumn gridViewColumn ) 
        {
           SetupCurrencyWithParameters(propertyType, gridColumn, gridViewColumn);
        }
    }
}