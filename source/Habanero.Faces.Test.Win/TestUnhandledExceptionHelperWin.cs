using System;
using System.ComponentModel;
using System.Windows.Forms;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win
{
    [TestFixture]
    public class TestUnhandledExceptionHelperWin
    {
        static void CauseUnhandledException()
        {
            using (var badControl = new BadControl())
            {
                using (var form = new Form())
                {
                    form.Controls.Add(badControl);
                    form.Show();
                }
            }
            
        }

        private class BadControl : Control
        {
            protected override void WndProc(ref Message m)
            {
                // This does nothing and will result in the unhandled exception: "Win32Exception: Error creating window handle"
            }
        }

        [Test]
        public void Test_Construct_WithNoArgs_ShouldActivateExceptionHandling()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var unhandledExceptionHelper = new UnhandledExceptionHelperWin();
            //---------------Test Result -----------------------
            Assert.IsTrue(unhandledExceptionHelper.IsExceptionHandlingActive);
        }

        [Test]
        public void Test_Construct_WithAddExceptionHandlingTrue_ShouldActivateExceptionHandling()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var unhandledExceptionHelper = new UnhandledExceptionHelperWin(true);
            //---------------Test Result -----------------------
            Assert.IsTrue(unhandledExceptionHelper.IsExceptionHandlingActive);
        }

        [Test]
        public void Test_Construct_WithAddExceptionHandlingFalse_ShouldNotActivateExceptionHandling()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var unhandledExceptionHelper = new UnhandledExceptionHelperWin(false);
            //---------------Test Result -----------------------
            Assert.IsFalse(unhandledExceptionHelper.IsExceptionHandlingActive);
        }

        [Test]
        public void Test_RemoveExceptionHandling_ShouldRemoveExceptionHandling()
        {
            //---------------Set up test pack-------------------
            var unhandledExceptionHelper = new UnhandledExceptionHelperWin();
            //---------------Assert Precondition----------------
            Assert.IsTrue(unhandledExceptionHelper.IsExceptionHandlingActive);
            //---------------Execute Test ----------------------
            unhandledExceptionHelper.RemoveExceptionHandling();
            //---------------Test Result -----------------------
            Assert.IsFalse(unhandledExceptionHelper.IsExceptionHandlingActive);
        }

        [Test]
        public void Test_AddExceptionHandling_ShouldAddExceptionHandling()
        {
            //---------------Set up test pack-------------------
            var unhandledExceptionHelper = new UnhandledExceptionHelperWin(false);
            //---------------Assert Precondition----------------
            Assert.IsFalse(unhandledExceptionHelper.IsExceptionHandlingActive);
            //---------------Execute Test ----------------------
            unhandledExceptionHelper.AddExceptionHandling();
            //---------------Test Result -----------------------
            Assert.IsTrue(unhandledExceptionHelper.IsExceptionHandlingActive);
        }

        [Test]
        public void Test_WhenHandlerActive_ShouldHandleUnhandledUIFormException()
        {
            UnhandledExceptionEventArgs unhandledExceptionEventArgs = null;
            using (var unhandledExceptionHelper = new UnhandledExceptionHelperWin((sender, args) => unhandledExceptionEventArgs = args))
            {
                //---------------Set up test pack-------------------
                //---------------Assert Precondition----------------
                Assert.IsTrue(unhandledExceptionHelper.IsExceptionHandlingActive);
                //---------------Execute Test ----------------------
                var exception = Assert.Throws<Win32Exception>(CauseUnhandledException, "Expected to throw an Win32Exception");
                //---------------Test Result -----------------------
                Assert.IsNotNull(unhandledExceptionEventArgs, "Should have thrown an unhandled exception");
                Assert.IsInstanceOf<Win32Exception>(unhandledExceptionEventArgs.ExceptionObject);
                var unhandledExceptionObject = (Win32Exception)unhandledExceptionEventArgs.ExceptionObject;
                StringAssert.Contains("Error creating window handle", exception.Message);
                Assert.AreEqual(exception.NativeErrorCode, unhandledExceptionObject.NativeErrorCode, "NativeError code was expected to be the same as the exception");
                Assert.AreEqual(exception.Message, unhandledExceptionObject.Message, "Message was expected to be the same as the exception");
            }
        }

        [Test, Ignore("This test should only be run manually because it pops up an exception handler dialog")]
        public void Test_WhenHandlerNotActive_ShouldNotHandleUnhandledUIFormException()
        {
            UnhandledExceptionEventArgs unhandledExceptionEventArgs = null;
            using (var unhandledExceptionHelper = new UnhandledExceptionHelperWin((sender, args) => unhandledExceptionEventArgs = args, false))
            {
                //---------------Set up test pack-------------------
                //---------------Assert Precondition----------------
                Assert.IsFalse(unhandledExceptionHelper.IsExceptionHandlingActive);
                //---------------Execute Test ----------------------
                var exception = Assert.Throws<Win32Exception>(CauseUnhandledException, "Expected to throw an Win32Exception");
                //---------------Test Result -----------------------
                Assert.IsNull(unhandledExceptionEventArgs, "The error would have popped up an exception dialog box instead.");
                StringAssert.Contains("Error creating window handle", exception.Message);
            }
        }
    }
}