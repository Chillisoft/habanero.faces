using System.Collections;
using System.Collections.Generic;
using Habanero.Faces.Base;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Menu
{
    [TestFixture]
    public class TestMenuItemCollectionWin 
    {
        private IControlFactory _factory;

        protected IControlFactory GetControlFactory()
        {
            if ((_factory == null)) _factory = new ControlFactoryWin();

            GlobalUIRegistry.ControlFactory = _factory;
            return _factory;
        }

        [Test]
        public void Test_AddItem_ShouldAdd()
        {
            //---------------Set up test pack-------------------
            var menuItemCollectionWin = CreateMenuItemCollectionWin();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(menuItemCollectionWin);
            Assert.AreEqual(0, menuItemCollectionWin.Count);
            //---------------Execute Test ----------------------
            menuItemCollectionWin.Add(new MenuItemWin("fdasf"));
            //---------------Test Result -----------------------
            Assert.AreEqual(1, menuItemCollectionWin.Count);
        }

        private MenuItemCollectionWin CreateMenuItemCollectionWin()
        {
            System.Windows.Forms.Menu menu = (System.Windows.Forms.Menu) GetControlFactory().CreateMainMenu();
            System.Windows.Forms.Menu.MenuItemCollection menuItemCollection = new System.Windows.Forms.Menu.MenuItemCollection(menu);
            return new MenuItemCollectionWin(menuItemCollection);
        }

        [Test]
        public void Test_GetEnumerator_ViaIEnumerable_ShouldEnumerate()
        {
            //---------------Set up test pack-------------------
            var menuItemCollectionWin = CreateMenuItemCollectionWin();
            const string menuitemtext = "MenuItemText";
            menuItemCollectionWin.Add(new MenuItemWin(menuitemtext));
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, menuItemCollectionWin.Count);
            //---------------Execute Test ----------------------
            foreach (IMenuItem menuItem in (IEnumerable)menuItemCollectionWin)
            {
                Assert.AreEqual(menuitemtext, menuItem.Text);
            }
            //---------------Test Result -----------------------
        }

        [Test]
        public void Test_GetEnumerator_ViaIEnumerableGeneric_ShouldEnumerate()
        {
            //---------------Set up test pack-------------------
            var menuItemCollectionWin = CreateMenuItemCollectionWin();
            const string menuitemtext = "MenuItemText";
            menuItemCollectionWin.Add(new MenuItemWin(menuitemtext));
            var itemCollectionWinAsGeneric = (IEnumerable<IMenuItem>)menuItemCollectionWin;
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, menuItemCollectionWin.Count);
            Assert.IsInstanceOf<IEnumerable<IMenuItem>>(itemCollectionWinAsGeneric);
            //---------------Execute Test ----------------------
            foreach (IMenuItem menuItem in itemCollectionWinAsGeneric)
            {
                Assert.AreEqual(menuitemtext, menuItem.Text);
            }
        }
    }
}