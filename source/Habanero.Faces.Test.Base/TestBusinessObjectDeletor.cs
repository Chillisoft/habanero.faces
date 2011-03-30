using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.Faces.Base.CF;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.MethodRecorders;

namespace Habanero.Faces.Test.Base
{
    public class DataAccessorInMemoryWithMocks: DataAccessorInMemory
    {
        public DataAccessorInMemoryWithMocks()
        {
            TransactionCommitter = MockRepository.GenerateStub<ITransactionCommitter>();
        }

        public ITransactionCommitter TransactionCommitter { get; private set; }


        public override ITransactionCommitter CreateTransactionCommitter()
        {
            
            return TransactionCommitter;
        }
    }

    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class TestBusinessObjectDeletor
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
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            DefaultBODeletor businessObjectDeletor = new DefaultBODeletor();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(IBusinessObjectDeletor), businessObjectDeletor);
        }

        [Test]
        public void Test_DeleteBusinessObject_Success()
        {
            //---------------Set up test pack-------------------
            ITransactionCommitter transactionCommitter = GetTransactionCommitter();
            IBusinessObject boToDelete = MockRepository.GenerateMock<IBusinessObject>();
            DefaultBODeletor businessObjectDeletor = new DefaultBODeletor();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(transactionCommitter);
            transactionCommitter.AssertWasNotCalled(committer => committer.CommitTransaction());
            //---------------Execute Test ----------------------
            businessObjectDeletor.DeleteBusinessObject(boToDelete);
            //---------------Test Result -----------------------
            boToDelete.AssertWasCalled(o => o.MarkForDelete());
            transactionCommitter.AssertWasCalled(committer => committer.CommitTransaction());
            boToDelete.AssertWasNotCalled(o => o.CancelEdits());
        }

        private static ITransactionCommitter GetTransactionCommitter()
        {
            var dataAccessorInMemoryWithMocks = new DataAccessorInMemoryWithMocks();
            var transactionCommitter = dataAccessorInMemoryWithMocks.TransactionCommitter;
            BORegistry.DataAccessor = dataAccessorInMemoryWithMocks;
            return transactionCommitter;
        }

        [Test]
        public void Test_DeleteBusinessObject_Failure()
        {
            //---------------Set up test pack-------------------
            ITransactionCommitter transactionCommitter = GetTransactionCommitter();
            DefaultBODeletor businessObjectDeletor = new DefaultBODeletor();
            IBusinessObject boToDelete = new FakeAddress();
            Exception expectedException = new Exception();
            transactionCommitter.Stub(t => t.CommitTransaction()).Throw(expectedException);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------

            try
            {
                businessObjectDeletor.DeleteBusinessObject(boToDelete);
                Assert.Fail("Expected to throw an Exception");
            }
                //---------------Test Result -----------------------
            catch (Exception exception)
            {
                Assert.AreSame(expectedException, exception);
            }
        }
    }
}
// ReSharper restore InconsistentNaming
