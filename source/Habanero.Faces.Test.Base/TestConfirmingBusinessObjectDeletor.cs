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
using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using Is=Rhino.Mocks.Constraints.Is;

namespace Habanero.Faces.Test.Base
{
    public class ConfirmerFake: IConfirmer
    {
        public ConfirmerFake(bool willBeConfirmed)
        {
            WillBeConfirmed = willBeConfirmed;
        }

        public bool WillBeConfirmed { get; set; }

        public bool Confirm(string message)
        {
            return WillBeConfirmed;
        }

        public void Confirm(string message, ConfirmationDelegate confirmationDelegate)
        {
            confirmationDelegate(WillBeConfirmed);
        }
    }
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class TestConfirmingBusinessObjectDeletor
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
            BORegistry.DataAccessor = new DataAccessorInMemoryWithMocks();
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
        }

        [Test]
        public void Test_Construct()
        {
            //---------------Set up test pack-------------------
            MockRepository mockRepository = new MockRepository();
            IConfirmer confirmer = mockRepository.StrictMock<IConfirmer>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ConfirmingBusinessObjectDeletor confirmingBusinessObjectDeletor = new ConfirmingBusinessObjectDeletor(confirmer);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(IBusinessObjectDeletor), confirmingBusinessObjectDeletor);
            Assert.AreSame(confirmer, confirmingBusinessObjectDeletor.Confirmer);
        }

        [Test]
        public void Test_Construct_WithCustomConfirmationMessageDelegate()
        {
            //---------------Set up test pack-------------------
            MockRepository mockRepository = new MockRepository();
            IConfirmer confirmer = mockRepository.StrictMock<IConfirmer>();
            Habanero.Util.Function<IBusinessObject, string> customConfirmationMessageDelegate = t => "aaa";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ConfirmingBusinessObjectDeletor confirmingBusinessObjectDeletor = new ConfirmingBusinessObjectDeletor(confirmer, customConfirmationMessageDelegate);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(IBusinessObjectDeletor), confirmingBusinessObjectDeletor);
            Assert.AreSame(confirmer, confirmingBusinessObjectDeletor.Confirmer);
            Assert.AreSame(customConfirmationMessageDelegate, confirmingBusinessObjectDeletor.CustomConfirmationMessageDelegate);
        }

        [Test]
        public void Test_DeleteBusinessObject_ConfirmationMessage()
        {
            //---------------Set up test pack-------------------
            MockRepository mockRepository = new MockRepository();
            string boToString = TestUtil.GetRandomString();
            string expectedMessage = string.Format("Are you certain you want to delete the object '{0}'", boToString);
            IConfirmer confirmer = CreateMockConfirmerWithExpectation(mockRepository, 
                Is.Equal(expectedMessage), false);
            IBusinessObject boToDelete = new MockBOWithToString(boToString);
            ConfirmingBusinessObjectDeletor confirmingBusinessObjectDeletor = new ConfirmingBusinessObjectDeletor(confirmer);
            mockRepository.ReplayAll();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            confirmingBusinessObjectDeletor.DeleteBusinessObject(boToDelete);
            //---------------Test Result -----------------------
            mockRepository.VerifyAll();
        }

        [Test]
        public void Test_DeleteBusinessObject_CustomConfirmationMessage()
        {
            //---------------Set up test pack-------------------
            MockRepository mockRepository = new MockRepository();
            string expectedMessage = TestUtil.GetRandomString();
            IConfirmer confirmer = CreateMockConfirmerWithExpectation(mockRepository, 
                Is.Equal(expectedMessage), false);
            IBusinessObject boToDelete = mockRepository.StrictMock<IBusinessObject>();
            ConfirmingBusinessObjectDeletor confirmingBusinessObjectDeletor = 
                new ConfirmingBusinessObjectDeletor(confirmer, t => expectedMessage);
            mockRepository.ReplayAll();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            confirmingBusinessObjectDeletor.DeleteBusinessObject(boToDelete);
            //---------------Test Result -----------------------
            mockRepository.VerifyAll();
        }


        [Test]
        public void Test_DeleteBusinessObject_ConfirmationDelegateUsed_ConfirmedIsTrue_ShouldDeleteBO()
        {
            //---------------Set up test pack-------------------
            var confirmer = new ConfirmerFake(true);
            var boToDelete = MockRepository.GenerateStub<IBusinessObject>();
            var confirmingBusinessObjectDeletor = new ConfirmingBusinessObjectDeletor(confirmer);
            //---------------Assert Precondition----------------
            Assert.IsTrue(confirmer.WillBeConfirmed);
            //---------------Execute Test ----------------------
            confirmingBusinessObjectDeletor.DeleteBusinessObject(boToDelete);
            //---------------Test Result -----------------------
            boToDelete.AssertWasCalled(o => o.MarkForDelete());
        }

        [Test]
        public void Test_DeleteBusinessObject_ConfirmedIsFalse_ShouldNotDeleteBO()
        {
            //---------------Set up test pack-------------------

            var confirmer = new ConfirmerFake(false);
            var boToDelete = MockRepository.GenerateStub<IBusinessObject>();
            var confirmingBusinessObjectDeletor = new ConfirmingBusinessObjectDeletor(confirmer);
            //---------------Assert Precondition----------------
            Assert.IsFalse(confirmer.WillBeConfirmed);
            //---------------Execute Test ----------------------
            confirmingBusinessObjectDeletor.DeleteBusinessObject(boToDelete);
            //---------------Test Result -----------------------
            boToDelete.AssertWasNotCalled(o => o.MarkForDelete());
        }

        private static IConfirmer CreateMockConfirmerWithExpectation(MockRepository mockRepository, AbstractConstraint messageConstraint, bool confirmReturnValue)
        {
            IConfirmer confirmer = mockRepository.StrictMock<IConfirmer>();
            Expect.Call(()=>confirmer.Confirm(null, null))
                .Constraints(messageConstraint,Is.NotNull())
                .Do((Delegates.Action<string, ConfirmationDelegate>)(
                (message, confirmationDelegate) => confirmationDelegate(confirmReturnValue)));
            return confirmer;
        }

        public class MockBOWithToString : BusinessObject
        {
            private readonly string _boToString;

            public MockBOWithToString(string boToString)
                : base(Circle.CreateClassDef())
            {
                _boToString = boToString;
            }

            public override string ToString()
            {
                return _boToString;
            }
        }
    }
// ReSharper restore InconsistentNaming
}