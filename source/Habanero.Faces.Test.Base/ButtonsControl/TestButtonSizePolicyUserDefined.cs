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
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Habanero.Faces.Base;

using NUnit.Framework;

namespace Habanero.Faces.Test.Base.ButtonsControl
{
    public abstract class TestButtonSizePolicyUserDefined
    {
        protected abstract IControlFactory GetControlFactory();

        [Test]
        public void TestButtonWidthPolicy_UserDefined()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttonGroupControl = GetControlFactory().CreateButtonGroupControl();
            Size buttonSize = new Size(20, 50);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            buttonGroupControl.ButtonSizePolicy = new ButtonSizePolicyUserDefined();
            IButton btnTest1 = buttonGroupControl.AddButton("");
            btnTest1.Size = buttonSize;

            buttonGroupControl.AddButton("Bigger button");
            //---------------Test Result -----------------------

            Assert.AreEqual(buttonSize, btnTest1.Size);

        }

    }


}
