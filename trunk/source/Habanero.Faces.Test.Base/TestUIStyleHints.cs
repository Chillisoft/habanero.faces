using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Faces.Base.UIHints;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    [TestFixture]
    class TestUIStyleHints
    {
        [Test]
        public void Constructor_SetsUpPropertyObjects()
        {
            //---------------Set up test pack-------------------
            var hints = new UIStyleHints();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsNotNull(hints.ButtonHints, "ButtonHints not set");
            Assert.IsNotNull(hints.CheckBoxHints, "CheckBoxHints not set");
            Assert.IsNotNull(hints.ComboBoxHints, "ComboBoxHints not set");
            Assert.IsNotNull(hints.DateTimePickerHints, "DateTimePickerHints not set");
            Assert.IsNotNull(hints.GridHints, "GridHints not set");
            Assert.IsNotNull(hints.LabelHints, "LabelHints not set");
            Assert.IsNotNull(hints.LayoutHints, "LayoutHints not set");
            Assert.IsNotNull(hints.TextBoxHints, "TextBoxHints not set");
            Assert.IsNotNull(hints.FormHints, "FormHints is not set");
            Assert.IsNotNull(hints.StaticDataEditorManagerHints, "StaticDataEditorManagerHints is not set");
        }
    }
}
