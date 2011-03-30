// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using Habanero.BO;
using Habanero.BO.Loaders;
using Habanero.Faces.Base.CF;
using Habanero.Test;
using Habanero.Test.Structure;
using NUnit.Framework;
using Habanero.BO.ClassDefinition;

namespace Habanero.Faces.Test.Base
{
    public abstract class TestContextMenu
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

        protected abstract IMainMenuHabanero CreateControl();
        protected abstract IMainMenuHabanero CreateControl(HabaneroMenu menu);
        protected abstract IControlFactory CreateNewControlFactory();

        protected virtual IControlFactory GetControlFactory()
        {
            IControlFactory factory = CreateNewControlFactory();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void Test_Construction()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMainMenuHabanero contextMenu = CreateControl();

            var c = GlobalUIRegistry.ControlFactory;

            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(IMainMenuHabanero), contextMenu);
            Assert.IsNotNull(contextMenu.MenuItems);
            TestUtil.AssertStringEmpty(contextMenu.Name, "contextMenu.Name should be an empty string");
        }

        [Test]
        public void Test_ConstructContextMenu_WithHabaneroMenu()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu menu = new HabaneroMenu(TestUtil.GetRandomString());
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMainMenuHabanero contextMenu = CreateControl(menu);
            //---------------Test Result -----------------------
            Assert.IsNotNull(contextMenu);
            Assert.IsNotNull(contextMenu.MenuItems);
            Assert.AreEqual(menu.Name, contextMenu.Name);
        }

        [Test]
        public void Test_MenuItems_ShouldAlwaysReturnTheSameInstance()
        {
            //---------------Set up test pack-------------------
            IMainMenuHabanero mainMenu = CreateControl();
            IMenuItemCollection expectedMenuItems = mainMenu.MenuItems;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(mainMenu);
            Assert.IsNotNull(expectedMenuItems);
            //---------------Execute Test ----------------------
            IMenuItemCollection secondCallToMenuItems = mainMenu.MenuItems;
            //---------------Test Result -----------------------
            Assert.AreSame(expectedMenuItems, secondCallToMenuItems);
        }

        [Test]
        public void Test_ConstructMainMenu_WithHabaneroMenuNull_ShouldNotSetName()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMainMenuHabanero mainMenu = CreateControl(null);
            //---------------Test Result -----------------------
            Assert.IsNotNull(mainMenu);
            TestUtil.AssertStringEmpty(mainMenu.Name, "mainMenu.Name");
            Assert.IsNotNull(mainMenu.MenuItems);
        }

        [Test]
        public void Test_DockInForm()
        {
            //---------------Set up test pack-------------------
            IFormHabanero formHabanero = GetControlFactory().CreateForm();
            IMainMenuHabanero mainMenu = CreateControl();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, formHabanero.Controls.Count);
            //---------------Execute Test ----------------------
            try
            {
                mainMenu.DockInForm(formHabanero);
            }
            catch (Exception ex)
            {

                //---------------Test Result -----------------------
                Assert.IsInstanceOf<NotImplementedException>(ex);
                StringAssert.Contains("Context menu's cannot be docked", ex.Message);
            }
        }

    }

}