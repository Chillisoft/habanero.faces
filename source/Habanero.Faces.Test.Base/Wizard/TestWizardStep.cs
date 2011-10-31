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
using Habanero.Faces.Base;


using NUnit.Framework;

namespace Habanero.Faces.Test.Base.Wizard
{
    public abstract class TestWizardStep
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
            //base.TearDownTest();
        }

        protected abstract IWizardStep CreateWizardStep();

        [Test]
        public void Test_CanFinish_ShouldRetFalseByDefault()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var step = CreateWizardStep();
            var canFinish = step.CanFinish();
            //---------------Test Result -----------------------
            Assert.IsFalse(canFinish);
        }
        [Test]
        public void Test_CanCancel_ShouldRetTrueByDefault()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var step = CreateWizardStep();
            var canCancel = step.CanCancel();
            //---------------Test Result -----------------------
            Assert.IsTrue(canCancel);
        }
        [Test]
        public void Test_InitialiseStep_DoesnotThrowError()
        {
            //---------------Set up test pack-------------------
            var step = CreateWizardStep();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            step.InitialiseStep();
            //---------------Test Result -----------------------
            Assert.IsTrue(true, "Should not do anything");
        }

        [Test]
        public void Test_MoveOn_DoesnotThrowError()
        {
            //---------------Set up test pack-------------------
            var step = CreateWizardStep();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            step.MoveOn();
            //---------------Test Result -----------------------
            Assert.IsTrue(true, "Should not do anything");
        }

        [Test]
        public void Test_CancelStep_DoesnotThrowError()
        {
            //---------------Set up test pack-------------------
            var step = CreateWizardStep();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            step.CancelStep();
            //---------------Test Result -----------------------
            Assert.IsTrue(true, "Should not do anything");
        }

        [Test]
        public void Test_CancelMoveOn_DoesnotThrowError()
        {
            //---------------Set up test pack-------------------
            var step = CreateWizardStep();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            step.UndoMoveOn();
            //---------------Test Result -----------------------
            Assert.IsTrue(true, "Should not do anything");
        }

        [Test]
        public void Test_HeaderText_DoesnotThrowError()
        {
            //---------------Set up test pack-------------------
            var step = CreateWizardStep();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var headerText = step.HeaderText;
            //---------------Test Result -----------------------
            Assert.AreEqual("", headerText);
        }

        [Test]
        public void Test_CanMoveOn_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var step = CreateWizardStep();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string message;
            var canMoveOn = step.CanMoveOn(out message);
            //---------------Test Result -----------------------
            Assert.IsTrue(canMoveOn);
            Assert.AreEqual("", message);
        }

        [Test]
        public void Test_CanMoveBack_ShouldRetTrue()
        {
            //---------------Set up test pack-------------------
            var step = CreateWizardStep();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var canMoveBack = step.CanMoveBack();
            //---------------Test Result -----------------------
            Assert.IsTrue(canMoveBack);
        }
    }


}