using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Faces.Base.Grid;
using Habanero.Testability;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base.FilterController
{
    [TestFixture]
    public class TestStringLiteralFilterClause
    {
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
        public void Test_Construct_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var filterClause = new StringLiteralFilterClause(RandomValueGen.GetRandomString());
            //---------------Test Result -----------------------
            Assert.IsNotNull(filterClause);
        }

        [Test]
        public void Test_GetStringLiteral_ShouldWork()
        {
            //---------------Set up test pack-------------------
            var expectedStringLiteral = RandomValueGen.GetRandomString();
            var filterClause = new StringLiteralFilterClause(expectedStringLiteral);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualStringLiteral = filterClause.StringLiteral;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStringLiteral, actualStringLiteral);
        }
        [Test]
        public void Test_GetFilterClauseString_ShouldReturnStdString()
        {
            //---------------Set up test pack-------------------
            var expectedStringLiteral = RandomValueGen.GetRandomString();
            var filterClause = new StringLiteralFilterClause(expectedStringLiteral);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var filterClauseString = filterClause.GetFilterClauseString();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStringLiteral, filterClauseString);
        }

        [Test]
        public void Test_GetFilterClauseString_WithParams_ShouldReturnStdString()
        {
            //---------------Set up test pack-------------------
            var expectedStringLiteral = RandomValueGen.GetRandomString();
            var filterClause = new StringLiteralFilterClause(expectedStringLiteral);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var filterClauseString = filterClause.GetFilterClauseString("f", "f");
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStringLiteral, filterClauseString);
        }
    }
}
