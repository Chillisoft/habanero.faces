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
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.LayoutManager
{
    [TestFixture]
    public class TestBorderLayoutManagerWin : TestBorderLayoutManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        [Test]
        public void TestSplitter()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = GetControlFactory().CreateControl();
            managedControl.Width = _STD_MANAGEDCONTROL_WIDTH;
            managedControl.Height = _STD_MANAGEDCONTROL_HEIGHT;
            IControlHabanero ctlEast = CreateControl(20, 20);
            IControlHabanero ctlCentre = CreateControl(1, 1);
            BorderLayoutManager manager = GetControlFactory().CreateBorderLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            manager.AddControl(ctlEast, BorderLayoutManager.Position.East, true);
            manager.AddControl(ctlCentre, BorderLayoutManager.Position.Centre);
            //---------------Test Result -----------------------
            Assert.AreEqual(managedControl.Controls.Count, 3, "There should be 3 controls because of the splitter.");
            Assert.AreEqual(80, ctlEast.Left, "East positioned control doesn't change width when splitter is added.");
            Assert.AreEqual(80 - 3, ctlCentre.Width,
                            "Splitter is 3 wide, so centre control should be 3 less than it would be");
        }
    }
}