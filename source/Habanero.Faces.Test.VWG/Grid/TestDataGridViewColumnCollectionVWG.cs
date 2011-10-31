﻿#region Licensing Header
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
using Gizmox.WebGUI.Forms;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using Habanero.Test;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Test.VWG.Grid
{
    [TestFixture]
    public class TestDataGridViewColumnCollectionVWG
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
        private static IDataGridViewColumnCollection GetColumnCollectionWithMock(out DataGridViewColumnCollection mock)
        {
            DataGridView dataGridView = new DataGridView();
            mock = MockRepository.GenerateMock<Gizmox.WebGUI.Forms.DataGridViewColumnCollection>(dataGridView);
            return  new DataGridViewVWG.DataGridViewColumnCollectionVWG(mock);
        }

        [Test]
        public void Test_Contains_Calls_UnderlyingDataGridViewContainsMethod()
        {
            //---------------Set up test pack-------------------
            DataGridViewColumnCollection mock;
            IDataGridViewColumnCollection columnCollection = GetColumnCollectionWithMock(out mock);
            string columnName = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            columnCollection.Contains(columnName);
            //---------------Test Result -----------------------
            mock.AssertWasCalled(collection => collection.Contains(columnName));
        }

        [Test]
        public void Test_Contains_ReturnsValueOf_UnderlyingDataGridViewContainsMethod_WhenFalse()
        {
            //---------------Set up test pack-------------------
            string columnName = TestUtil.GetRandomString();
            const bool expectedReturn = false;
            DataGridViewColumnCollection mock;
            IDataGridViewColumnCollection columnCollection = GetColumnCollectionWithMock(out mock);
            mock.Stub(t => mock.Contains(columnName)).Return(expectedReturn);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool returnValue = columnCollection.Contains(columnName);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedReturn, returnValue);
        }

        [Test]
        public void Test_Contains_ReturnsValueOf_UnderlyingDataGridViewContainsMethod_WhenTrue()
        {
            //---------------Set up test pack-------------------
            string columnName = TestUtil.GetRandomString();
            const bool expectedReturn = true;
            DataGridViewColumnCollection mock;
            IDataGridViewColumnCollection columnCollection = GetColumnCollectionWithMock(out mock);
            mock.Stub(t => mock.Contains(columnName)).Return(expectedReturn);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool returnValue = columnCollection.Contains(columnName);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedReturn, returnValue);
        }
    }
}