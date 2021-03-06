using Habanero.Base;
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base.FilterController
{
    [TestFixture]
    public class TestStringStaticFilter
    {

        [Test]
        public void TestConstructor_WhenFilterValueNull_ShouldReturnNullFilterClause()
        {
            //---------------Set up test pack-------------------
            string propertyName = TestUtil.GetRandomString();
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.OpGreaterThan;
            //---------------Execute Test ----------------------
            StringStaticFilter filter = new StringStaticFilter(propertyName, filterClauseOperator, null);

            //---------------Test Result -----------------------
            Assert.IsNull(filter.Control);
            Assert.AreEqual(propertyName, filter.PropertyName);
            Assert.AreEqual(filterClauseOperator, filter.FilterClauseOperator);
            Assert.IsInstanceOf(typeof(DataViewNullFilterClause), filter.GetFilterClause(new DataViewFilterClauseFactory()));
        }        
        [Test]
        public void Test_Construct_WhenFilterValueSet_ShouldReturnStringFilterClause()
        {
            //---------------Set up test pack-------------------
            string propertyName = TestUtil.GetRandomString();
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.OpGreaterThan;
            const string filterValue = "constantValue";
            //---------------Execute Test ----------------------
            StringStaticFilter filter = new StringStaticFilter(propertyName, filterClauseOperator, filterValue);

            //---------------Test Result -----------------------
            Assert.IsNull(filter.Control);
            Assert.AreEqual(propertyName, filter.PropertyName);
            Assert.AreEqual(filterClauseOperator, filter.FilterClauseOperator);

            IFilterClause filterClause = filter.GetFilterClause(new DataViewFilterClauseFactory());
            Assert.IsInstanceOf<DataViewStringFilterClause>( filterClause);
        }

        [Test]
        public void Test_GetFilterClause()
        {
            //---------------Set up test pack-------------------
            string propertyName = TestUtil.GetRandomString();
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.OpGreaterThan;
            const string filterValue = "constantValue";
            StringStaticFilter filter = new StringStaticFilter(propertyName, filterClauseOperator, filterValue);
            //---------------Execute Test ----------------------
            IFilterClause filterClause = filter.GetFilterClause(new DataViewFilterClauseFactory());
            //---------------Test Result -----------------------
            Assert.AreEqual(string.Format("{0} > '{1}'", propertyName, filterValue), filterClause.GetFilterClauseString()); 
        }
        [Test]
        public void Test_GetFilterClause_WhenOpIs_ShouldNotSetValueDelimiters()
        {
            //---------------Set up test pack-------------------
            string propertyName = TestUtil.GetRandomString();
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.Is;
            const string filterValue = "NULL";
            StringStaticFilter filter = new StringStaticFilter(propertyName, filterClauseOperator, filterValue);
            //---------------Execute Test ----------------------
            IFilterClause filterClause = filter.GetFilterClause(new DataViewFilterClauseFactory());
            //---------------Test Result -----------------------
            Assert.AreEqual(string.Format("{0} Is {1}", propertyName, filterValue), filterClause.GetFilterClauseString()); 
        }

        [Test]
        public void Test_ClearFilterClause_ShouldDoNothing()
        {
            //---------------Set up test pack-------------------
            string propertyName = TestUtil.GetRandomString();
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.OpGreaterThan;
            const string filterValue = "constantValue";
            StringStaticFilter filter = new StringStaticFilter(propertyName, filterClauseOperator, filterValue);
            //---------------Assert Precondition----------------
            IFilterClause filterClause = filter.GetFilterClause(new DataViewFilterClauseFactory());
            Assert.AreEqual(string.Format("{0} > '{1}'", propertyName, filterValue), filterClause.GetFilterClauseString()); 
            //---------------Execute Test ----------------------
            filter.Clear();
            //---------------Test Result -----------------------
            filterClause = filter.GetFilterClause(new DataViewFilterClauseFactory());
            Assert.AreEqual(string.Format("{0} > '{1}'", propertyName, filterValue), filterClause.GetFilterClauseString()); 
        }

    }


}