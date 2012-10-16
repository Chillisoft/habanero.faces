using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Base.UIHints;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win
{
    [TestFixture]
    class TestCollapsibleExceptionNotifyForm
    {
        [SetUp]
        public void _init_()
        {
            GlobalUIRegistry.ControlFactory = new ControlFactoryWin();
            Application.EnableVisualStyles();
        }

        [TearDown]
        public void _deinit_()
        {
            GlobalUIRegistry.ControlFactory = null;
            GlobalUIRegistry.UIStyleHints = null;
        }
        [Test]
        [Ignore("Visual test")]
        public void VisualTest()
        {
            //---------------Set up test pack-------------------
            var hints = new UIStyleHints();
            hints.ButtonHints.MinimumHeight = 35;
            GlobalUIRegistry.UIStyleHints = hints;
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            (new FormExceptionNotifier()).Notify(new Exception("This is a test"), "More information here", "Testing");
            //---------------Test Result -----------------------
            
        }
    }
}
