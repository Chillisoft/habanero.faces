using Habanero.Base;
using Habanero.Faces.Base.CF;
using Habanero.Faces.Base.Grid;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base.FilterController
{
    [TestFixture]
    public class TestStringLiteralCustomFilter: TestStaticCustomFilter
    {
   

/*


*/

        protected override StaticCustomFilter GetStaticCustomFilter()
        {
            return new StringLiteralCustomFilter();
        }
        [Test]
        public void Test_Construct_WithStringLiteral_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var filterClause = new StringLiteralCustomFilter(RandomValueGen.GetRandomString());
            //---------------Test Result -----------------------
            Assert.IsNotNull(filterClause);
        }[Test]
        public void Test_Construct_WithEmptyConstructo_SetStringLiteralToEmpty()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var filterClause = new StringLiteralCustomFilter();
            //---------------Test Result -----------------------
            Assert.AreEqual(string.Empty, filterClause.StringLiteral);
        }

        [Test]
        public void Test_GetStringLiteral_ShouldWork()
        {
            //---------------Set up test pack-------------------
            var expectedStringLiteral = GetRandomString();
            var filterClause = new StringLiteralCustomFilter(expectedStringLiteral);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualStringLiteral = filterClause.StringLiteral;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStringLiteral, actualStringLiteral);
        }
        [Test]
        public void Test_SetStringLiteral_ShouldWork()
        {
            //---------------Set up test pack-------------------
            var filterClause = new StringLiteralCustomFilter(GetRandomString());
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var expectedStringLiteral = GetRandomString();
            filterClause.StringLiteral = expectedStringLiteral;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStringLiteral, filterClause.StringLiteral);
        }

        private static string GetRandomString()
        {
            return RandomValueGen.GetRandomString();
        }

        [Test]
        public void Test_GetFilterClause_ShouldRetStringLiteralFilterClause()
        {
            //---------------Set up test pack-------------------
            var filterClause = new StringLiteralCustomFilter(GetRandomString());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var clause = filterClause.GetFilterClause(new DataViewFilterClauseFactory());
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<StringLiteralFilterClause>(clause );
        }
        [Test]
        public void Test_GetFilterClauseString_ShouldReturnStringLiteral()
        {
            //---------------Set up test pack-------------------
            var expectedStringLiteral = GetRandomString();
            var filterClause = new StringLiteralCustomFilter(expectedStringLiteral);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var filterClauseString = filterClause.GetFilterClause(new DataViewFilterClauseFactory()).GetFilterClauseString();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStringLiteral, filterClauseString);
        }

/*        

        [Test]
        public void Test_GetFilterClauseString_WithParams_ShouldReturnStdString()
        {
            //---------------Set up test pack-------------------
            var expectedStringLiteral = GetRandomString();
            var filterClause = new StringLiteralCustomFilter(expectedStringLiteral);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var filterClauseString = filterClause.GetFilterClauseString("f", "f");
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStringLiteral, filterClauseString);
        }*/
    }
}