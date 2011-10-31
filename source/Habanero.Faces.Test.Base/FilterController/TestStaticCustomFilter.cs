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
using Habanero.Base;
using Habanero.Faces.Base.Grid;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base.FilterController
{
    [TestFixture]
    public class TestStaticCustomFilter
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //TestUIBase.SetupTest();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            //TestUIBase.TestFixtureSetup();
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //TestUIBase.TearDownTest();
        }

        [Test]
        public void Test_Construct_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var customFilter = GetStaticCustomFilter();

            //---------------Test Result -----------------------
            Assert.IsNotNull(customFilter);
        }

        protected virtual StaticCustomFilter GetStaticCustomFilter()
        {
            return new StaticCustomFilterStub();
        }

        [Test]
        public void Test_Control_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var customFilter = GetStaticCustomFilter();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var control = customFilter.Control;
            //---------------Test Result -----------------------
            Assert.IsNull(control);
        }
        [Test]
        public void Test_Clear_ShouldDoNothing()
        {
            //---------------Set up test pack-------------------
            var customFilter = GetStaticCustomFilter();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            customFilter.Clear();
            //---------------Test Result -----------------------
            Assert.IsTrue(true, "Clear should not raise an error and should do nothing since this is being added as a static filter");
        }
        [Test]
        public void Test_PropertyName_ShouldReturnEmptyString()
        {
            //---------------Set up test pack-------------------
            var customFilter = GetStaticCustomFilter();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var propertyName = customFilter.PropertyName;
            //---------------Test Result -----------------------
            Assert.IsEmpty(propertyName, "The PropertyName should be null");
        }
        [Test]
        public void Test_FilterClauseOperator_ShouldReturnsOpLike()
        {
            //---------------Set up test pack-------------------
            var customFilter = GetStaticCustomFilter();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var filterClauseOp = customFilter.FilterClauseOperator;
            //---------------Test Result -----------------------
            Assert.AreEqual(FilterClauseOperator.OpLike, filterClauseOp, "The Filter Clause Operator should be null but is non nullable so return the OpLike as a default");
        }


    }

    class StaticCustomFilterStub : StaticCustomFilter
    {
        public override IFilterClause GetFilterClause(IFilterClauseFactory filterClauseFactory)
        {
            return null;
        }
    }
}