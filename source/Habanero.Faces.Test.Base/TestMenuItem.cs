using Habanero.Faces.Base.CF;
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