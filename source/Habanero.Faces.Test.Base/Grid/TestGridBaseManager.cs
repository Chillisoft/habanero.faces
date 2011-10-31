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
using System.ComponentModel;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Test.Structure;
using NUnit.Framework;
using Rhino.Mocks;

// ReSharper disable InconsistentNaming
namespace Habanero.Faces.Test.Base.Grid
{
    [TestFixture]
    public class TestGridBaseManager 
    {
//        protected const string _gridIdColumnName = "HABANERO_OBJECTID";

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
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        [Test]
        public void Test_ApplyFilter_ShouldSetFilterOnBindingList()
        {
            //---------------Set up test pack-------------------

            var grid = MockRepository.GenerateStub<IGridBase>();
            var gridBaseManager = new GridBaseManager(grid);
            var bindingListView = MockRepository.GenerateStub<IBindingListView>();
            grid.DataSource = bindingListView;
            var filterClause = GetFilterClause();
            //---------------Assert Precondition----------------
            Assert.AreSame(bindingListView, grid.DataSource);
            bindingListView.AssertWasNotCalled(view => view.Filter);
            var filterClauseString = filterClause.GetFilterClauseString("%", "#");
            Assert.IsNotNullOrEmpty(filterClauseString);
            //---------------Execute Test ----------------------
            gridBaseManager.ApplyFilter(filterClause);
            //---------------Test Result -----------------------          
            Assert.AreEqual(filterClauseString, bindingListView.Filter);
        }

        [Test]
        public void Test_ApplyFilter_WheResetDataSource_ShouldSetFilterOnBindingList_FixBug615()
        {
            //---------------Set up test pack-------------------
            var grid = MockRepository.GenerateStub<IGridBase>();

            var gridBaseManager = new GridBaseManager(grid);
            grid.DataSource = GenerateStub<IBindingListView>();
            gridBaseManager.ApplyFilter(GetFilterClause());


            var bindingListView = GenerateStub<IBindingListView>();
            grid.DataSource = bindingListView;
            var filterClause = GetFilterClause();
            //---------------Assert Precondition----------------
            Assert.AreSame(bindingListView, grid.DataSource);
            bindingListView.AssertWasNotCalled(view => view.Filter);
            var filterClauseString = filterClause.GetFilterClauseString("%", "#");
            Assert.IsNotNullOrEmpty(filterClauseString);
            //---------------Execute Test ----------------------
            gridBaseManager.ApplyFilter(filterClause);
            //---------------Test Result -----------------------          
            Assert.AreEqual(filterClauseString, bindingListView.Filter);
        }

        private static IFilterClause GetFilterClause()
        {
            var filterClause = GenerateStub<IFilterClause>();
            filterClause.Stub(clause => clause.GetFilterClauseString("%", "#")).Return(GetRandomString());
            return filterClause;
        }

        private static string GetRandomString()
        {
            return RandomValueGen.GetRandomString();
        }

        private static T GenerateStub<T>() where T : class
        {
            return MockRepository.GenerateStub<T>();
        }

/*        private class GridBaseManagerSpy : GridBaseManager
        {
            public GridBaseManagerSpy()
                : base(MockRepository.GenerateStub<IGridBase>())
            {
            }

            public GridBaseManagerSpy(IGridBase gridBase) : base(gridBase)
            {
            }
        }*/
    }
}