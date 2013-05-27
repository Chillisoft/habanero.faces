using System;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.StandardControls
{
    /// <summary>
    /// This test class tests the TreeView class.
    /// </summary>
    [TestFixture]
    public class TestTreeViewWin : TestTreeView
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void Test_SuppressDoubleClickEvent_GetAndSet_WithTrue()
        {
            //---------------Set up test pack-------------------
            TreeViewWin treeView = GetControlledLifetimeFor((TreeViewWin)GetControlFactory().CreateTreeView());
            const bool newValue = true;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            treeView.SuppressDoubleClickEvent = newValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(newValue, treeView.SuppressDoubleClickEvent);
        }

        [Test]
        public void Test_SuppressDoubleClickEvent_GetAndSet_WithFalse()
        {
            //---------------Set up test pack-------------------
            TreeViewWin treeView = GetControlledLifetimeFor((TreeViewWin)GetControlFactory().CreateTreeView());
            const bool newValue = false;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            treeView.SuppressDoubleClickEvent = newValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(newValue, treeView.SuppressDoubleClickEvent);
        }

        /// <summary>
        /// This test was created because of some erroneous code in the WndProc override of the treeview.
        /// The erroneous code was causing an unhandled exception to occur when the form was shown. 
        /// The resolution of this issue was to just restore the base method call for the default execution paths through the overridden method.
        /// The unhandled exception helper was added here in order to prevent the case of this failing test freezing the build server (as it was doing).
        /// The presence of the <see cref="UnhandledExceptionHelperWin"/> causes the unhandled exception to be handled (by default constructor) and to throw the error in a normal fasion.
        /// </summary>
        [Test]
        public void Test_Show_ShouldNotGiveErrors_BUGFIX()
        {
            using (var unhandledExceptionHelper = new UnhandledExceptionHelperWin())
            {   
                //---------------Set up test pack-------------------
                TreeViewWin treeView = GetControlledLifetimeFor((TreeViewWin) GetControlFactory().CreateTreeView());
                var form = GetControlledLifetimeFor(GetControlFactory().CreateForm());
                form.Controls.Add(treeView);
                //---------------Assert Precondition----------------
                Assert.IsTrue(unhandledExceptionHelper.IsExceptionHandlingActive);
                //---------------Execute Test ----------------------
                form.Show();
                //---------------Test Result -----------------------
                Assert.IsTrue(form.Visible);
            }
        }
    }
}