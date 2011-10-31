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
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Menu
{
    [TestFixture]
    public class TestCollapsibleMenuItemCollectionWin : TestCollapsibleMenuItemCollection
    {

        protected override IMenuItemCollection CreateControl()
        {
            CollapsibleMenuWin menu = new CollapsibleMenuWin();
            return new CollapsibleMenuItemCollectionWin(menu);
        }

        protected override IMenuItemCollection CreateControl(IMenuItem menuItem)
        {
            return new CollapsibleMenuItemCollectionWin(menuItem);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override IMenuItem CreateMenuItem()
        {
            return new CollapsibleMenuWin();
        }

        protected override IMenuItem CreateMenuItem(HabaneroMenu.Item habaneroMenuItem)
        {
            return new CollapsibleMenuItemWin(habaneroMenuItem);
        }

        [Test]
        public void Test_AddSubMenuItem_ShouldAddCollapsiblePanel()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            CollapsibleMenuWin menu = new CollapsibleMenuWin();
            CollapsibleMenuItemCollectionWin collapsibleMenuItemCollection = new CollapsibleMenuItemCollectionWin(menu);
            HabaneroMenu.Item item = new HabaneroMenu.Item(null, name);
            IMenuItem menuLeafItem = new CollapsibleSubMenuItemWin(GetControlFactory(), item);
            //---------------Assert Precondition----------------
            Assert.AreSame(menu, collapsibleMenuItemCollection.OwnerMenuItem);
            Assert.IsInstanceOf(typeof(CollapsibleSubMenuItemWin), menuLeafItem);
            Assert.AreEqual(0, collapsibleMenuItemCollection.Count);
            //---------------Execute Test ----------------------
            collapsibleMenuItemCollection.Add(menuLeafItem);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, collapsibleMenuItemCollection.Count);
            Assert.AreEqual(1, menu.PanelsList.Count);
            ICollapsiblePanel collapsiblePanel = menu.PanelsList[0];
            Assert.AreEqual(name, collapsiblePanel.Text);
        }

        [Test]
        public void Test_AddLeafMenuItem_ShouldAddButtonToCollapsiblePanel()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item ownerItem = new HabaneroMenu.Item(null, name);
            CollapsibleSubMenuItemWin subMenuItem = new CollapsibleSubMenuItemWin(GetControlFactory(), ownerItem);
            CollapsibleMenuItemCollectionWin collapsibleMenuItemCollection = new CollapsibleMenuItemCollectionWin
                (subMenuItem);
            HabaneroMenu.Item item = new HabaneroMenu.Item(null, name);
            CollapsibleMenuItemWin menuLeafItem = new CollapsibleMenuItemWin(GetControlFactory(), item);
            //---------------Assert Precondition----------------
            Assert.AreSame(subMenuItem, collapsibleMenuItemCollection.OwnerMenuItem);
            Assert.IsInstanceOf(typeof(CollapsibleMenuItemWin), menuLeafItem);
            Assert.AreEqual(1, subMenuItem.Controls.Count, "CollapsiblePanel always has header button");
            Assert.AreEqual(0, collapsibleMenuItemCollection.Count);
            //---------------Execute Test ----------------------
            collapsibleMenuItemCollection.Add(menuLeafItem);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, collapsibleMenuItemCollection.Count);
            Assert.AreEqual(2, subMenuItem.Controls.Count, "Should have additional button control");
            IControlHabanero contentControl = subMenuItem.ContentControl;
            Assert.IsInstanceOf(typeof(IPanel), contentControl);
            Assert.AreEqual(1, contentControl.Controls.Count);
            Assert.IsInstanceOf(typeof(IButton), contentControl.Controls[0]);
        }

        [Test]
        public void Test_AddLeafMenuItem_ShouldIncreaseMinSizeOfCollapsiblePanel()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item ownerItem = new HabaneroMenu.Item(null, name);
            CollapsibleSubMenuItemWin subMenuItem = new CollapsibleSubMenuItemWin(GetControlFactory(), ownerItem);
            CollapsibleMenuItemCollectionWin collapsibleMenuItemCollection = new CollapsibleMenuItemCollectionWin
                (subMenuItem);
            HabaneroMenu.Item item = new HabaneroMenu.Item(null, name);
            CollapsibleMenuItemWin menuLeafItem = new CollapsibleMenuItemWin(GetControlFactory(), item);
            //---------------Assert Precondition----------------
            Assert.AreSame(subMenuItem, collapsibleMenuItemCollection.OwnerMenuItem);
            Assert.IsInstanceOf(typeof(CollapsibleMenuItemWin), menuLeafItem);
            Assert.AreEqual(subMenuItem.CollapseButton.Height, subMenuItem.MinimumSize.Height);
            //---------------Execute Test ----------------------
            collapsibleMenuItemCollection.Add(menuLeafItem);
            //---------------Test Result -----------------------
            int expectedHeight = subMenuItem.CollapseButton.Height + menuLeafItem.Height;
            Assert.AreEqual(expectedHeight, subMenuItem.Height);
            Assert.AreEqual
                (expectedHeight, subMenuItem.ExpandedHeight, "this will be zero untill the first time this is collapsed");
        }
    }
}