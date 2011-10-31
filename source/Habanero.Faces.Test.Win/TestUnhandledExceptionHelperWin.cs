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
            var badControl = new BadControl();
            Form form = new Form();
            form.Controls.Add(badControl);
            form.Show();
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
                Assert.IsNotNull(unhandledExceptionEventArgs);
                Assert.IsInstanceOf<Win32Exception>(unhandledExceptionEventArgs.ExceptionObject);
                var unhandledExceptionObject = (Win32Exception)unhandledExceptionEventArgs.ExceptionObject;
                StringAssert.Contains("Error creating window handle", exception.Message);
                Assert.AreEqual(exception.NativeErrorCode, unhandledExceptionObject.NativeErrorCode);
                Assert.AreEqual(exception.Message, unhandledExceptionObject.Message);
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