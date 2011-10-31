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
using System.Drawing;
using Habanero.Faces.Base;
using Habanero.Faces.Base.ControlMappers;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    public abstract class TestExtendedComboBox
    {

        protected abstract IControlFactory GetControlFactory();
/*            {
                return new ControlFactoryWin();
            }*/

            [Test]
            public void Test_Layout()
            {
                //--------------- Set up test pack-------------------
                IControlFactory controlFactory = GetControlFactory();
                //--------------- Test Preconditions ----------------

                //--------------- Execute Test ----------------------
                IExtendedComboBox extendedComboBox = controlFactory.CreateExtendedComboBox();
                //--------------- Test Result -----------------------
                Assert.AreEqual(2, extendedComboBox.Controls.Count);
                IControlHabanero control1 = extendedComboBox.Controls[0];
                IControlHabanero control2 = extendedComboBox.Controls[1];
                Assert.IsInstanceOf(typeof(IComboBox), control1);
                Assert.IsInstanceOf(typeof(IButton), control2);
                Assert.AreEqual("...", control2.Text);
                Assert.AreEqual(0, control1.Left);
                Assert.LessOrEqual(control1.Width, control2.Left);
                Assert.GreaterOrEqual(extendedComboBox.Width, control2.Left + control2.Width);
            }

            [Test]
            public void Test_GetCombo()
            {
                //--------------- Set up test pack ------------------
                IControlFactory controlFactory = GetControlFactory();
                IExtendedComboBox extendedComboBox = controlFactory.CreateExtendedComboBox();
                //--------------- Test Preconditions ----------------

                //--------------- Execute Test ----------------------
                IComboBox comboBox = extendedComboBox.ComboBox;
                //--------------- Test Result -----------------------
                Assert.IsNotNull(comboBox);
            }

            [Test]
            public void Test_GetButton()
            {
                //--------------- Set up test pack ------------------
                IControlFactory controlFactory = GetControlFactory();
                IExtendedComboBox extendedComboBox = controlFactory.CreateExtendedComboBox();
                //--------------- Test Preconditions ----------------

                //--------------- Execute Test ----------------------
                IButton button = extendedComboBox.Button;
                //--------------- Test Result -----------------------
                Assert.IsNotNull(button);
            }

    }
}