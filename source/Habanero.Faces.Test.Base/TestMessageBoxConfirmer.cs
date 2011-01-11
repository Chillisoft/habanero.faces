// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using Habanero.Base;
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using Is = Rhino.Mocks.Constraints.Is;

namespace Habanero.Faces.Test.Base
{
    [TestFixture]
    public class TestMessageBoxConfirmer
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
        public void Test_Constructor()
        {
            //---------------Set up test pack-------------------
            const string message = "Confirmer message";
            const DialogResult dialogResultToReturn = DialogResult.Yes;
            string title = TestUtil.GetRandomString();
            MessageBoxIcon messageBoxIcon = MessageBoxIcon.None;
            MockRepository mockRepository = new MockRepository();
            IControlFactory controlFactory = SetupControlFactoryMockWithExpectation(mockRepository, message, title, messageBoxIcon, dialogResultToReturn);

            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            MessageBoxConfirmer messageBoxConfirmer = new MessageBoxConfirmer(controlFactory, title, messageBoxIcon);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(IConfirmer), messageBoxConfirmer);
            Assert.AreSame(controlFactory, messageBoxConfirmer.ControlFactory);
            Assert.AreEqual(title, messageBoxConfirmer.Title);
            Assert.AreEqual(messageBoxIcon, messageBoxConfirmer.MessageBoxIcon);
        }

        [Test]
        public void Test_Confirm_True()
        {
            //---------------Set up test pack-------------------
            const string message = "Confirmer message";
            const string title = "MessageBoxTitle";
            const DialogResult dialogResultToReturn = DialogResult.Yes;
            MessageBoxIcon messageBoxIcon = TestUtil.GetRandomEnum<MessageBoxIcon>();

            MockRepository mockRepository = new MockRepository();
            IControlFactory controlFactory = SetupControlFactoryMockWithExpectation(mockRepository, message, title, messageBoxIcon, dialogResultToReturn);

            MessageBoxConfirmer messageBoxConfirmer = new MessageBoxConfirmer(controlFactory, title, messageBoxIcon);
            mockRepository.ReplayAll();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool confirmResult = messageBoxConfirmer.Confirm(message);
            //---------------Test Result -----------------------
            Assert.IsTrue(confirmResult);
            mockRepository.VerifyAll();
        }

        [Test]
        public void Test_Confirm_False()
        {
            //---------------Set up test pack-------------------
            const string message = "Confirmer message";
            const string title = "MessageBoxTitle";
            const DialogResult dialogResultToReturn = DialogResult.No;
            MessageBoxIcon messageBoxIcon = TestUtil.GetRandomEnum<MessageBoxIcon>();

            MockRepository mockRepository = new MockRepository();
            IControlFactory controlFactory = SetupControlFactoryMockWithExpectation(mockRepository, message, title, messageBoxIcon, dialogResultToReturn);

            MessageBoxConfirmer messageBoxConfirmer = new MessageBoxConfirmer(controlFactory, title, messageBoxIcon);
            mockRepository.ReplayAll();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool confirmResult = messageBoxConfirmer.Confirm(message);
            //---------------Test Result -----------------------
            Assert.IsFalse(confirmResult);
            mockRepository.VerifyAll();
        }

        [Test]
        public void Test_Confirm_True_WithDelegate()
        {
            //---------------Set up test pack-------------------
            const string message = "Confirmer message";
            const string title = "MessageBoxTitle";
            const DialogResult dialogResultToReturn = DialogResult.Yes;
            MessageBoxIcon messageBoxIcon = TestUtil.GetRandomEnum<MessageBoxIcon>();

            bool delegateWasCalled = false;
            bool confirmedParamInDelegate = false;
            ConfirmationDelegate confirmationDelegate = delegate(bool confirmed)
            {
                delegateWasCalled = true;
                confirmedParamInDelegate = confirmed;
            };

            MockRepository mockRepository = new MockRepository();
            IControlFactory controlFactory = SetupControlFactoryMockWithExpectationWithDelegate(
                mockRepository, message, title, messageBoxIcon, dialogResultToReturn, confirmationDelegate);

            MessageBoxConfirmer messageBoxConfirmer = new MessageBoxConfirmer(controlFactory, title, messageBoxIcon);

            mockRepository.ReplayAll();
            //---------------Assert Precondition----------------
            Assert.IsFalse(delegateWasCalled);
            Assert.IsFalse(confirmedParamInDelegate);
            //---------------Execute Test ----------------------
            messageBoxConfirmer.Confirm(message, confirmationDelegate);
            //---------------Test Result -----------------------
            Assert.IsTrue(delegateWasCalled);
            Assert.IsTrue(confirmedParamInDelegate);
            mockRepository.VerifyAll();
        }

        [Test]
        public void Test_Confirm_False_WithDelegate()
        {
            //---------------Set up test pack-------------------
            const string message = "Confirmer message";
            const string title = "MessageBoxTitle";
            const DialogResult dialogResultToReturn = DialogResult.No;
            MessageBoxIcon messageBoxIcon = TestUtil.GetRandomEnum<MessageBoxIcon>();

            bool delegateWasCalled = false;
            bool confirmedParamInDelegate = true;
            ConfirmationDelegate confirmationDelegate = delegate(bool confirmed)
            {
                delegateWasCalled = true;
                confirmedParamInDelegate = confirmed;
            };

            MockRepository mockRepository = new MockRepository();
            IControlFactory controlFactory = SetupControlFactoryMockWithExpectationWithDelegate(
                mockRepository, message, title, messageBoxIcon, dialogResultToReturn, confirmationDelegate);

            MessageBoxConfirmer messageBoxConfirmer = new MessageBoxConfirmer(controlFactory, title, messageBoxIcon);

            mockRepository.ReplayAll();
            //---------------Assert Precondition----------------
            Assert.IsFalse(delegateWasCalled);
            Assert.IsTrue(confirmedParamInDelegate);
            //---------------Execute Test ----------------------
            messageBoxConfirmer.Confirm(message, confirmationDelegate);
            //---------------Test Result -----------------------
            Assert.IsTrue(delegateWasCalled);
            Assert.IsFalse(confirmedParamInDelegate);
            mockRepository.VerifyAll();
        }


        private IControlFactory SetupControlFactoryMockWithExpectation(MockRepository mockRepository, string message, string title, MessageBoxIcon messageBoxIcon, DialogResult dialogResultToReturn)
        {
            IControlFactory controlFactory = mockRepository.StrictMock<IControlFactory>();
            controlFactory.Expect(
                factory => factory.ShowMessageBox(message, title, MessageBoxButtons.YesNo, messageBoxIcon))
                .Return(dialogResultToReturn);
            return controlFactory;
        }

        private IControlFactory SetupControlFactoryMockWithExpectationWithDelegate(
            MockRepository mockRepository, string message, string title,
            MessageBoxIcon messageBoxIcon, DialogResult dialogResultToReturn,
            ConfirmationDelegate confirmationDelegate)
        {
            IControlFactory controlFactory = mockRepository.StrictMock<IControlFactory>();
            controlFactory.Expect(
                factory => factory.ShowMessageBox(null, null, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Asterisk, null))
                .Return(dialogResultToReturn).Constraints(
                  Is.Equal(message), Is.Equal(title), Is.Equal(MessageBoxButtons.YesNo),
                  Is.Equal(messageBoxIcon), Is.Anything())
                .WhenCalled(invocation => confirmationDelegate(dialogResultToReturn == DialogResult.Yes));
            return controlFactory;
        }
    }
}