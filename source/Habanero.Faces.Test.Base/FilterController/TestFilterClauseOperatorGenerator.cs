using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    /// <summary>
    /// Summary description for TestDataviewFilterClauseBuilder.
    /// </summary>
    [TestFixture]
    public class TestFilterClauseOperatorGenerator
    {

        [TestFixtureSetUp]
        public void SetupFixture()
        {

        }

        [Test]
        public void Test_ConvertToString_OpEquals()
        {
            //---------------Set up test pack-------------------
            var clauseGenerator = new FilterClauseOperatorGenerator();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var clauseString = clauseGenerator.ConvertToString(FilterClauseOperator.OpEquals);
            //---------------Test Result -----------------------
            Assert.AreEqual(" = ", clauseString);
        }
        [Test]
        public void Test_ConvertToString_OpLike()
        {
            //---------------Set up test pack-------------------
            var clauseGenerator = new FilterClauseOperatorGenerator();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var clauseString = clauseGenerator.ConvertToString(FilterClauseOperator.OpLike);
            //---------------Test Result -----------------------
            Assert.AreEqual(" like ", clauseString);
        }
        [Test]
        public void Test_ConvertToString_OpGreaterThanOrEqualTo()
        {
            //---------------Set up test pack-------------------
            var clauseGenerator = new FilterClauseOperatorGenerator();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var clauseString = clauseGenerator.ConvertToString(FilterClauseOperator.OpGreaterThanOrEqualTo);
            //---------------Test Result -----------------------
            Assert.AreEqual(" >= ", clauseString);
        }
        [Test]
        public void Test_ConvertToString_OpLessThanOrEqualTo()
        {
            //---------------Set up test pack-------------------
            var clauseGenerator = new FilterClauseOperatorGenerator();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var clauseString = clauseGenerator.ConvertToString(FilterClauseOperator.OpLessThanOrEqualTo);
            //---------------Test Result -----------------------
            Assert.AreEqual(" <= ", clauseString);
        }
        [Test]
        public void Test_ConvertToString_OpGreaterThan()
        {
            //---------------Set up test pack-------------------
            var clauseGenerator = new FilterClauseOperatorGenerator();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var clauseString = clauseGenerator.ConvertToString(FilterClauseOperator.OpGreaterThan);
            //---------------Test Result -----------------------
            Assert.AreEqual(" > ", clauseString);
        }
        [Test]
        public void Test_ConvertToString_OpLessThan()
        {
            //---------------Set up test pack-------------------
            var clauseGenerator = new FilterClauseOperatorGenerator();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var clauseString = clauseGenerator.ConvertToString(FilterClauseOperator.OpLessThan);
            //---------------Test Result -----------------------
            Assert.AreEqual(" < ", clauseString);
        }
        [Test]
        public void Test_ConvertToString_OpIs()
        {
            //---------------Set up test pack-------------------
            var clauseGenerator = new FilterClauseOperatorGenerator();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var clauseString = clauseGenerator.ConvertToString(FilterClauseOperator.Is);
            //---------------Test Result -----------------------
            Assert.AreEqual(" Is ", clauseString);
        }
        [Test]
        public void Test_ConvertToString_OpNotEquals()
        {
            //---------------Set up test pack-------------------
            var clauseGenerator = new FilterClauseOperatorGenerator();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var clauseString = clauseGenerator.ConvertToString(FilterClauseOperator.OpNotEqual);
            //---------------Test Result -----------------------
            Assert.AreEqual(" <> ", clauseString);
        }
    }
}