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
using System.Linq;
using System.Text;
using Habanero.Faces.Base;
using Habanero.Faces.Base.ControlMappers;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    public abstract class TestExtendedTextBox
    {
        protected abstract IControlFactory GetControlFactory();
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [Test]
        public void Test_Constructor()
        {
            //---------------Set up test pack-------------------
            IControlFactory controlFactory = GetControlFactory();
            //---------------Execute Test ----------------------
            IExtendedTextBox extendedTextBox = controlFactory.CreateExtendedTextBox();
            //---------------Test Result -----------------------
            ITextBox textBox = extendedTextBox.TextBox;
            Assert.IsNotNull(textBox);
            IButton button = extendedTextBox.Button;
            Assert.IsNotNull(button);
            Assert.AreEqual("...", button.Text);
            Assert.IsFalse(textBox.Enabled);
            Assert.AreEqual(Color.White, textBox.BackColor);
            Assert.AreEqual(extendedTextBox.Height, textBox.Height);
            Assert.Greater(button.Left, textBox.Left);
        }

        #region TestSelectItem Control

        [Test]
        public void Test_GetTextBox()
        {
            //--------------- Set up test pack ------------------
            IControlFactory controlFactory = GetControlFactory();
            IExtendedTextBox extendedComboBox = controlFactory.CreateExtendedTextBox();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            ITextBox comboBox = extendedComboBox.TextBox;
            //--------------- Test Result -----------------------
            Assert.IsNotNull(comboBox);
        }

        [Test]
        public void Test_GetButton()
        {
            //--------------- Set up test pack ------------------
            IControlFactory controlFactory = GetControlFactory();
            IExtendedTextBox extendedTextBoxWin = controlFactory.CreateExtendedTextBox();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            IButton button = extendedTextBoxWin.Button;
            //--------------- Test Result -----------------------
            Assert.IsNotNull(button);
        }

        #endregion //testSelectItem Control
    }

}
