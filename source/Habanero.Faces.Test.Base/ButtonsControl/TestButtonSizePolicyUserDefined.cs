using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Habanero.Faces.Base;

using NUnit.Framework;

namespace Habanero.Faces.Test.Base.ButtonsControl
{
    public abstract class TestButtonSizePolicyUserDefined:TestBaseWithDisposing
    {
        protected abstract IControlFactory GetControlFactory();

        [Test]
        public void TestButtonWidthPolicy_UserDefined()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttonGroupControl = GetControlFactory().CreateButtonGroupControl();
            DisposeOnTearDown(buttonGroupControl);
            Size buttonSize = new Size(20, 50);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            buttonGroupControl.ButtonSizePolicy = new ButtonSizePolicyUserDefined();
            IButton btnTest1 = buttonGroupControl.AddButton("");
            DisposeOnTearDown(btnTest1);
            btnTest1.Size = buttonSize;

            buttonGroupControl.AddButton("Bigger button");
            //---------------Test Result -----------------------

            Assert.AreEqual(buttonSize, btnTest1.Size);

        }

    }


}
