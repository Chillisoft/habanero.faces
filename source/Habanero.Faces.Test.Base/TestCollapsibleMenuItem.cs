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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Test;
using Habanero.Test.Structure;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    public abstract class TestCollapsibleMenuItem
    {
        [SetUp]
        public void SetupTest()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef.ClassDefs.Add(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader(), new DefClassFactory()).LoadClassDefs());
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GlobalUIRegistry.ControlFactory = CreateNewControlFactory();
        }

        protected virtual IControlFactory GetControlFactory()
        {
            IControlFactory factory = CreateNewControlFactory();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected abstract IMenuItem CreateControl();
        protected abstract IMenuItem CreateControl(string name);
        protected abstract IMenuItem CreateControl(HabaneroMenu.Item habaneroMenuItem);
        protected abstract IMenuItem CreateControl(IControlFactory controlFactory, HabaneroMenu.Item habaneroMenuItem);
        protected abstract IControlFactory CreateNewControlFactory();

        [Test]
        public void Test_ConstructMenuItem_ShouldSetName()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMenuItem collapsibleMenuItem = CreateControl(name);
            //---------------Test Result -----------------------
            Assert.AreEqual(name, collapsibleMenuItem.Text);
            Assert.IsNotNull(collapsibleMenuItem.MenuItems);
            Assert.IsInstanceOf(typeof (IButton), collapsibleMenuItem);
        }

        [Test]
        public void Test_ConstructMenuItem_ControlFactoryNull_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item item = new HabaneroMenu.Item(null, name);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                CreateControl(null, item);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("controlFactory", ex.ParamName);
            }
        }

        [Test]
        public void Test_ConstructMenuItem_WithHabaneroMenuItem_ShouldSetName()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item item = new HabaneroMenu.Item(null, name);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMenuItem collapsibleMenuItem = CreateControl(item);
            //---------------Test Result -----------------------
            Assert.AreEqual(name, collapsibleMenuItem.Text);
            Assert.AreEqual(item.Name, collapsibleMenuItem.Text);
            Assert.IsNotNull(collapsibleMenuItem.MenuItems);
        }

        [Test]
        public void Test_ConstructMenuItem_WithHabaneroMenuItemNull_ShouldNotSetName()
        {
            //---------------Set up test pack-------------------
            const HabaneroMenu.Item item = null;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMenuItem collapsibleMenuItem = CreateControl(item);

            //---------------Test Result -----------------------
            TestUtil.AssertStringEmpty(collapsibleMenuItem.Text, "collapsibleMenuItem.Text");
            Assert.IsNotNull(collapsibleMenuItem.MenuItems);
        }

        [Test]
        public void Test_MenuItems_ShouldAlwaysReturnTheSameInstance()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item item = new HabaneroMenu.Item(null, name);
            IMenuItem collapsibleMenuItem = CreateControl(item);
            IMenuItemCollection expectedMenuItems = collapsibleMenuItem.MenuItems;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(collapsibleMenuItem);
            Assert.IsNotNull(expectedMenuItems);
            //---------------Execute Test ----------------------
            IMenuItemCollection secondCallToMenuItems = collapsibleMenuItem.MenuItems;
            //---------------Test Result -----------------------
            Assert.AreSame(expectedMenuItems, secondCallToMenuItems);
        }
    }
}