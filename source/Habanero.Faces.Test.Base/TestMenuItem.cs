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
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    public abstract class TestMenuItem
    {
        protected virtual IMenuItem CreateControl()
        {
            return CreateControl(GetRandomString());
        }

        protected virtual IMenuItem CreateControl(string itemName)
        {
            return GetControlFactory().CreateMenuItem(itemName);
        }

        private static string GetRandomString()
        {
            return TestUtil.GetRandomString();
        }

        protected abstract IControlFactory GetControlFactory();

        [Test]
        public void Test_Construct_MenuItem()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMenuItem menuItem = CreateControl();
            //---------------Test Result -----------------------
            Assert.IsNotNull(menuItem);
            Assert.IsNotNull(menuItem.MenuItems);

        }


        [Test]
        public void Test_Text_Getter()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string menuItemText = TestUtil.GetRandomString();
            IMenuItem menuItem = CreateControl(menuItemText);
            //---------------Test Result -----------------------
            Assert.AreEqual(menuItemText,menuItem.Text);
        }

        [Test]
        public void Test_Default_Getters()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IMenuItem menuItem = CreateControl();
            //---------------Test Result -----------------------
            Assert.IsTrue(menuItem.Enabled);
            Assert.IsTrue(menuItem.Visible);
        }


        [Test]
        public void Test_Enabled_Property()
        {
            //---------------Set up test pack-------------------
            IMenuItem menuItem = CreateControl();
            const bool EXPECTED = false;
            
            //---------------Assert Precondition----------------
            Assert.IsTrue(menuItem.Enabled);
            //---------------Execute Test ----------------------
            menuItem.Enabled = EXPECTED;
            //---------------Test Result -----------------------
            Assert.AreEqual(EXPECTED, menuItem.Enabled);
        }


        [Test]
        public void Test_Visible_Property()
        {
            //---------------Set up test pack-------------------
            IMenuItem menuItem = CreateControl();
            const bool EXPECTED = false;

            //---------------Assert Precondition----------------
            Assert.IsTrue(menuItem.Visible);
            //---------------Execute Test ----------------------
            menuItem.Visible = EXPECTED;
            //---------------Test Result -----------------------
            Assert.AreEqual(EXPECTED, menuItem.Visible);
        }


        [Test]
        public void Test_DefaultMenuItem_ShouldHaveNoMenuItems()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IMenuItem menuItem = CreateControl();
            //---------------Test Result -----------------------
            Assert.AreEqual(0,menuItem.MenuItems.Count);
        }


        [Test]
        public void Test_MenuItem_WithOneChildMenuItemAdded_ShouldHaveOneMenuItem()
        {
            //---------------Set up test pack-------------------
            IMenuItem menuItem = CreateControl();
            
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, menuItem.MenuItems.Count);
            //---------------Execute Test ----------------------
            IMenuItem childMenuItem = GetControlFactory().CreateMenuItem("FirstChildItem");
            menuItem.MenuItems.Add(childMenuItem);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, menuItem.MenuItems.Count);
            Assert.AreSame(childMenuItem,menuItem.MenuItems[0]);
        }

        [Test]
        public void Test_MenuItem_WithTwoChildMenuItemAdded_ShouldHaveTwoMenuItem()
        {
            //---------------Set up test pack-------------------
            IMenuItem menuItem = CreateControl();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, menuItem.MenuItems.Count);
            //---------------Execute Test ----------------------
            IMenuItem childMenuItem = GetControlFactory().CreateMenuItem("FirstChildItem");
            menuItem.MenuItems.Add(childMenuItem);
            IMenuItem childMenuItem2 = GetControlFactory().CreateMenuItem("SecondChildItem");
            menuItem.MenuItems.Add(childMenuItem2);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, menuItem.MenuItems.Count);
            Assert.AreSame(childMenuItem, menuItem.MenuItems[0]);
            Assert.AreSame(childMenuItem2, menuItem.MenuItems[1]);
        }


        [Test]
        public void Test_MenuItem_WithMultipleChildMenuItemAdded_ShouldHaveMultipleMenuItem()
        {
            //---------------Set up test pack-------------------
            IMenuItem menuItem = CreateControl();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, menuItem.MenuItems.Count);
            //---------------Execute Test ----------------------
            IMenuItem childMenuItem = GetControlFactory().CreateMenuItem("FirstChildItem");
            menuItem.MenuItems.Add(childMenuItem);
            IMenuItem childMenuItem2 = GetControlFactory().CreateMenuItem("SecondChildItem");
            menuItem.MenuItems.Add(childMenuItem2);
            IMenuItem childMenuItem3 = GetControlFactory().CreateMenuItem("ThirdChildItem");
            menuItem.MenuItems.Add(childMenuItem3);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, menuItem.MenuItems.Count);
            Assert.AreSame(childMenuItem, menuItem.MenuItems[0]);
            Assert.AreSame(childMenuItem2, menuItem.MenuItems[1]);
            Assert.AreSame(childMenuItem3, menuItem.MenuItems[2]);
        }


    }
}