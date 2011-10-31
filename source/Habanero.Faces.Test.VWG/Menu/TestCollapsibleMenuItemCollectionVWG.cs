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
using Habanero.Faces.VWG;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Menu
{
    [TestFixture]
    public class TestCollapsibleMenuItemCollectionVWG : TestCollapsibleMenuItemCollection
    {
        protected override IMenuItemCollection CreateControl()
        {
            CollapsibleMenuVWG menu = new CollapsibleMenuVWG();
            return new CollapsibleMenuItemCollectionVWG(menu);
        }

        protected override IMenuItemCollection CreateControl(IMenuItem menuItem)
        {
            return new CollapsibleMenuItemCollectionVWG(menuItem);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override IMenuItem CreateMenuItem()
        {
            return new CollapsibleMenuItemVWG(TestUtil.GetRandomString());
        }

        protected override IMenuItem CreateMenuItem(HabaneroMenu.Item habaneroMenuItem)
        {
            return new CollapsibleMenuItemVWG(habaneroMenuItem);
        }

        [Test]
        public virtual void Test_AddSubMenuItem_ShouldAddCollapsiblePanel()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            CollapsibleMenuVWG menu = new CollapsibleMenuVWG();
            CollapsibleMenuItemCollectionVWG collapsibleMenuItemCollection = new CollapsibleMenuItemCollectionVWG(menu);
            HabaneroMenu.Item item = new HabaneroMenu.Item(null, name);
            IMenuItem menuLeafItem = new CollapsibleSubMenuItemVWG(GetControlFactory(), item);
            //---------------Assert Precondition----------------
            Assert.AreSame(menu, collapsibleMenuItemCollection.OwnerMenuItem);
            Assert.IsInstanceOf(typeof(CollapsibleSubMenuItemVWG), menuLeafItem);
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
        public virtual void Test_AddLeafMenuItem_ShouldAddButtonToCollapsiblePanel()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item ownerItem = new HabaneroMenu.Item(null, name);
            CollapsibleSubMenuItemVWG subMenuItem = new CollapsibleSubMenuItemVWG(GetControlFactory(), ownerItem);
            CollapsibleMenuItemCollectionVWG collapsibleMenuItemCollection = new CollapsibleMenuItemCollectionVWG
                (subMenuItem);
            HabaneroMenu.Item item = new HabaneroMenu.Item(null, name);
            CollapsibleMenuItemVWG menuLeafItem = new CollapsibleMenuItemVWG(GetControlFactory(), item);
            //---------------Assert Precondition----------------
            Assert.AreSame(subMenuItem, collapsibleMenuItemCollection.OwnerMenuItem);
            Assert.IsInstanceOf(typeof(CollapsibleMenuItemVWG), menuLeafItem);
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
            Assert.AreEqual(menuLeafItem.Dock, DockStyleVWG.GetDockStyle(DockStyle.Top));
        }
        [Test]
        public virtual void Test_AddLeafMenuItem_ShouldIncreaseMinSizeOfCollapsiblePanel()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item ownerItem = new HabaneroMenu.Item(null, name);
            CollapsibleSubMenuItemVWG subMenuItem = new CollapsibleSubMenuItemVWG(GetControlFactory(), ownerItem);
            CollapsibleMenuItemCollectionVWG collapsibleMenuItemCollection = new CollapsibleMenuItemCollectionVWG
                (subMenuItem);
            HabaneroMenu.Item item = new HabaneroMenu.Item(null, name);
            CollapsibleMenuItemVWG menuLeafItem = new CollapsibleMenuItemVWG(GetControlFactory(), item);
            //---------------Assert Precondition----------------
            Assert.AreSame(subMenuItem, collapsibleMenuItemCollection.OwnerMenuItem);
            Assert.IsInstanceOf(typeof(CollapsibleMenuItemVWG), menuLeafItem);
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