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

using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    /// <summary>
    /// Summary description for TestReadOnlyGrid.
    /// </summary>
    public abstract class TestReadOnlyGrid
    {
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
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract IControlFactory GetControlFactory();
        protected abstract void AddControlToForm(IControlHabanero cntrl);



        [Test]
        public void TestCreateGridBase()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero grid = GetControlFactory().CreateReadOnlyGrid();

            ////---------------Test Result -----------------------
            Assert.IsNotNull(grid);
            Assert.IsTrue(grid is IReadOnlyGrid);
            IReadOnlyGrid readOnlyGrid = (IReadOnlyGrid) grid;
            readOnlyGrid.ReadOnly = true;
            readOnlyGrid.AllowUserToAddRows = false;
            readOnlyGrid.AllowUserToDeleteRows = false;
            //Need interfact to test selectionMode not sure if worth it.
            //see when implementing for windows. 
            //  readOnlyGrid.SelectionMode = Gizmox.WebGUI.Forms.DataGridViewSelectionMode.FullRowSelect;
        }

        [Test]
        public void TestSetCollectionOnGrid_NoOfRows()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IReadOnlyGrid readOnlyGrid = GetControlFactory().CreateReadOnlyGrid();
            AddControlToForm(readOnlyGrid);
            SetupGridColumnsForMyBo(readOnlyGrid);
            //---------------Execute Test ----------------------
#pragma warning disable 618,612
            readOnlyGrid.SetBusinessObjectCollection(col);
#pragma warning restore 618,612
            //---------------Test Result -----------------------
            Assert.AreEqual(4, readOnlyGrid.Rows.Count);
            //---------------Tear Down -------------------------    
        }

        [Test]
        public void Test_Set_BusinessObjectCollectionOnGrid_NoOfRows()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IReadOnlyGrid readOnlyGrid = GetControlFactory().CreateReadOnlyGrid();
            AddControlToForm(readOnlyGrid);
            SetupGridColumnsForMyBo(readOnlyGrid);
            //---------------Execute Test ----------------------
            readOnlyGrid.BusinessObjectCollection = col;
            //---------------Test Result -----------------------
            Assert.AreEqual(4, readOnlyGrid.Rows.Count);
            //---------------Tear Down -------------------------    
        }

        [Test]
        public void Test_GetBusinessObjectAtRow_WhenHasBO_ShouldRetBO()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IReadOnlyGrid readOnlyGrid = GetControlFactory().CreateReadOnlyGrid();
            AddControlToForm(readOnlyGrid);
            SetupGridColumnsForMyBo(readOnlyGrid);
            readOnlyGrid.BusinessObjectCollection = col;
            //---------------Assert Precondition----------------
            Assert.AreEqual(4, readOnlyGrid.Rows.Count);
            //---------------Execute Test ----------------------
            var actualBO = readOnlyGrid.GetBusinessObjectAtRow(1);
            //---------------Test Result -----------------------
            Assert.AreSame(col[1], actualBO);

        }

        [Test]
        public void Test_GetBusinessObjectAtRow_WhenSetViaCustomLoad_ShouldRetBO()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IReadOnlyGrid readOnlyGrid = GetControlFactory().CreateReadOnlyGrid();
            AddControlToForm(readOnlyGrid);
            SetupGridColumnsForMyBo(readOnlyGrid);
            readOnlyGrid.BusinessObjectCollection = col;
            //---------------Assert Precondition----------------
            Assert.AreEqual(4, readOnlyGrid.Rows.Count);
            //---------------Execute Test ----------------------
            var actualBO = readOnlyGrid.GetBusinessObjectAtRow(1);
            //---------------Test Result -----------------------
            Assert.AreSame(col[1], actualBO);
        }


        private static BusinessObjectCollection<MyBO> CreateCollectionWith_4_Objects()
        {
            MyBO cp = new MyBO {TestProp = "b"};
            MyBO cp2 = new MyBO {TestProp = "d"};
            MyBO cp3 = new MyBO {TestProp = "c"};
            MyBO cp4 = new MyBO {TestProp = "a"};
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO> {{cp, cp2, cp3, cp4}};
            return col;
        }

        private static void SetupGridColumnsForMyBo(IDataGridView readOnlyGrid)
        {
            readOnlyGrid.Columns.Add("TestProp", "TestProp");
        }
        
    }

}