using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Faces.Win.Async;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win
{
    [TestFixture]
    class TestBackgroundWorkerWin
    {
        private class MyMethodDispatcher : MethodDispatcher
        {
            public MyMethodDispatcher() : base(null)
            {
            }

            public override void Dispatch(System.Windows.Forms.MethodInvoker method)
            {
                method();
            }
        }
        [Test]
        public void Run_CallsSuccessDelegateOnBackgroundSuccess()
        {
            //---------------Set up test pack-------------------
            var successCalled = false;
            var cancelCalled = false;
            var exceptionCalled = false;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var worker = BackgroundWorkerWin.Run(
                new MyMethodDispatcher(),
                (data) => { return true; },
                (data) => { successCalled = true; },
                (data) => { cancelCalled = true; },
                (ex) => { exceptionCalled = true; },
                null);
            worker.WaitForBackgroundWorkerToComplete();
            //---------------Test Result -----------------------
            Assert.IsTrue(successCalled, "Success delegate not called (and should have been)");
            Assert.IsFalse(cancelCalled, "Cancel delegate was called (and shoudn't have been)");
            Assert.IsFalse(exceptionCalled, "Exception delegate called (and shouldn't have been)");
        }

        [Test]
        public void Run_CallsCancelDelegateOnBackgroundCancel()
        {
            //---------------Set up test pack-------------------
            var successCalled = false;
            var cancelCalled = false;
            var exceptionCalled = false;
            //---------------Assert Precondition----------------


            //---------------Execute Test ----------------------
            var worker = BackgroundWorkerWin.Run(
                new MyMethodDispatcher(),
                (data) => { return false; },
                (data) => { successCalled = true; },
                (data) => { cancelCalled = true; },
                (ex) => { exceptionCalled = true; },
                null);
            worker.WaitForBackgroundWorkerToComplete();
            //---------------Test Result -----------------------
            Assert.IsFalse(successCalled, "Success delegate called (and shouldn't have been)");
            Assert.IsTrue(cancelCalled, "Cancel delegate wasn't called (and shoud have been)");
            Assert.IsFalse(exceptionCalled, "Exception delegate called (and shouldn't have been)");
        }

        [Test]
        public void Run_CallsExceptionDelegateOnBackgroundThreadException()
        {
            //---------------Set up test pack-------------------
            var successCalled = false;
            var cancelCalled = false;
            var exceptionCalled = false;
            //---------------Assert Precondition----------------


            //---------------Execute Test ----------------------
            var worker = BackgroundWorkerWin.Run(
                new MyMethodDispatcher(),
                (data) => { throw new Exception("Die!");  return false; },
                (data) => { successCalled = true; },
                (data) => { cancelCalled = true; },
                (ex) => { exceptionCalled = true; },
                null);
            worker.WaitForBackgroundWorkerToComplete();
            //---------------Test Result -----------------------
            Assert.IsFalse(successCalled, "Success delegate called (and shouldn't have been)");
            Assert.IsTrue(cancelCalled, "Cancel delegate wasn't called (and should have been)");
            Assert.IsTrue(exceptionCalled, "Exception not delegate called (and should have been)");
        }
    }
}
