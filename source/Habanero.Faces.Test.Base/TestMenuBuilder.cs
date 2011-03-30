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
using Habanero.Base;
using Habanero.Faces.Base.CF;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    public abstract class TestMenuBuilder
    {
        protected abstract IControlFactory GetControlFactory();
        protected abstract IMenuBuilder CreateMenuBuilder();
        protected abstract IFormControlStub CreateFormControlStub();
        protected abstract bool IsMenuDocked(IMainMenuHabanero menu, IFormHabanero form);
        protected abstract void AssertControlDockedInForm(IControlHabanero habanero, IFormHabanero frm);

        protected HabaneroMenu CreateHabaneroMenuFullySetup()
        {
            IControlFactory controlFactory = GetControlFactory();
            IFormHabanero form = controlFactory.CreateForm();
            return new HabaneroMenu("Main", form, controlFactory);
        }


        public interface IFormControlStub : IFormControl
        {
            IFormHabanero SetFormArgument { get; }
            bool SetFormCalled { get; }
        }

        [Test]
        public void TestSimpleMenuStructure()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            string subMenuName = TestUtil.GetRandomString();
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(subMenuName);
            string menuItemName = TestUtil.GetRandomString();
            submenu.AddMenuItem(menuItemName);
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Execute Test ----------------------
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, menu.MenuItems.Count);
            Assert.AreEqual(subMenuName, menu.MenuItems[0].Text);
            Assert.AreEqual(1, menu.MenuItems[0].MenuItems.Count);
            Assert.AreEqual(menuItemName, menu.MenuItems[0].MenuItems[0].Text);
                      
        }

        [Test]
        public void TestMultipleSubmenus()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            string subMenuName1 = TestUtil.GetRandomString();
            HabaneroMenu submenu1 = habaneroMenu.AddSubMenu(subMenuName1);
            string subMenuName2 = TestUtil.GetRandomString();
            HabaneroMenu submenu2 = habaneroMenu.AddSubMenu(subMenuName2);
            submenu1.AddMenuItem(TestUtil.GetRandomString());
            submenu2.AddMenuItem(TestUtil.GetRandomString());
            submenu2.AddMenuItem(TestUtil.GetRandomString());
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Execute Test ----------------------
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, menu.MenuItems.Count);
            Assert.AreEqual(subMenuName1, menu.MenuItems[0].Text);
            Assert.AreEqual(1, menu.MenuItems[0].MenuItems.Count);
            Assert.AreEqual(subMenuName2, menu.MenuItems[1].Text);
            Assert.AreEqual(2, menu.MenuItems[1].MenuItems.Count);
                      
        }

        [Test]
        public void TestMultipleItems()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            string subMenuName = TestUtil.GetRandomString();
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(subMenuName);
            string menuItemName1 = TestUtil.GetRandomString();
            submenu.AddMenuItem(menuItemName1);
            string menuItemName2 = TestUtil.GetRandomString();
            submenu.AddMenuItem(menuItemName2);
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Execute Test ----------------------
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, menu.MenuItems.Count);
            Assert.AreEqual(2, menu.MenuItems[0].MenuItems.Count);
            Assert.AreEqual(menuItemName1, menu.MenuItems[0].MenuItems[0].Text);
            Assert.AreEqual(menuItemName2, menu.MenuItems[0].MenuItems[1].Text);
                      
        }

        [Test]
        public void TestMultiLevels()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            string subMenuName = TestUtil.GetRandomString();
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(subMenuName);
            string menuItemName1 = TestUtil.GetRandomString();
            submenu.AddMenuItem(menuItemName1);

            string subsubMenuName = TestUtil.GetRandomString();
            HabaneroMenu subsubmenu = submenu.AddSubMenu(subsubMenuName);

            string menuItemName2 = TestUtil.GetRandomString();
            subsubmenu.AddMenuItem(menuItemName2);
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Execute Test ----------------------
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, menu.MenuItems.Count);
            IMenuItem createdSubMenu = menu.MenuItems[0];
            Assert.AreEqual(2, createdSubMenu.MenuItems.Count);
            IMenuItem createdSubsubMenu = createdSubMenu.MenuItems[0];
            Assert.AreEqual(1, createdSubsubMenu.MenuItems.Count);
            Assert.AreEqual(menuItemName1, createdSubMenu.MenuItems[1].Text);
            Assert.AreEqual(menuItemName2, createdSubsubMenu.MenuItems[0].Text);

                      
        }

        [Test]
        public void TestNoCreatorsCalledWhenMenuFormNotSet()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.GetRandomString());
            bool called = false;
            menuItem.FormControlCreator = delegate
            {
                called = true;
                return CreateFormControlStub();
            };
            menuItem.ControlManagerCreator = delegate
            {
                called = true;
                return new ControlManagerStub(GetControlFactory());
            };
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            //---------------Assert Precondition ---------------
            Assert.IsNull(menuItem.Form);
            //---------------Execute Test ----------------------
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.IsFalse(called);      
        }

        [Test]
        public void Test_PerformClick_WhenMenuFormHasNoControlSet_ShouldNotCallCreators()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main", GetControlFactory().CreateForm(), GetControlFactory());
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.GetRandomString());
            bool called = false;
            menuItem.FormControlCreator = delegate
            {
                called = true;
                return CreateFormControlStub();
            };
            menuItem.ControlManagerCreator = delegate
            {
                called = true;
                return new ControlManagerStub(GetControlFactory());
            };
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            //---------------Assert Precondition ---------------
            Assert.IsNotNull(menuItem.Form);
            Assert.AreEqual(0, menuItem.Form.Controls.Count);
            //---------------Execute Test ----------------------
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
            formsMenuItem.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsFalse(called);      
        }

        [Test]
        public void TestFormControlCreatorCalledOnClickIfSet()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.GetRandomString());
            bool called = false;
            FormControlCreator formControlCreatorDelegate = delegate
            {
                called = true;
                return CreateFormControlStub();
            };
            menuItem.FormControlCreator += formControlCreatorDelegate;
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            menu.DockInForm(habaneroMenu.Form);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];

            //---------------Execute Test ----------------------
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.IsTrue(called);
                      
        }

        [Test]
        public virtual void Test_Click_WhenFormControlCreatorSet_ShouldCallSetFormOnFormControl()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.GetRandomString());
            
            bool creatorCalled = false;
            IFormControlStub formControlStub = CreateFormControlStub();
            FormControlCreator formControlCreatorDelegate = delegate
            {
                creatorCalled = true;
                return formControlStub;
            };
            menuItem.FormControlCreator += formControlCreatorDelegate;
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            menu.DockInForm(habaneroMenu.Form);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
            //--------------- Test Preconditions ----------------
            Assert.IsFalse(creatorCalled);
            Assert.IsFalse(formControlStub.SetFormCalled);
            Assert.IsNull(formControlStub.SetFormArgument);
            //---------------Execute Test ----------------------
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.IsTrue(creatorCalled);
            Assert.IsTrue(formControlStub.SetFormCalled);
            Assert.IsNotNull(formControlStub.SetFormArgument);
        }

        

        [Test]
        public void TestCustomMenuHandlerCalledIfSet()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.GetRandomString());
            bool called = false;
            EventHandler customerHandler = delegate
            {
                called = true;
            };
            menuItem.CustomHandler += customerHandler;
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Execute Test ----------------------
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.IsTrue(called);
                            
        }

        [Test]
        public void TestControlManagerCreatorCalledIfSet()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.GetRandomString());
            bool called = false;
            IControlFactory controlFactoryPassedToCreator = null;
            ControlManagerCreator formControlCreatorDelegate = delegate(IControlFactory controlFactory)
            {
                called = true;
                controlFactoryPassedToCreator = controlFactory;
                return new ControlManagerStub(controlFactory);
            };
            menuItem.ControlManagerCreator += formControlCreatorDelegate;
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            menu.DockInForm(habaneroMenu.Form);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];

            //---------------Execute Test ----------------------
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.IsTrue(called);
            Assert.AreSame(habaneroMenu.ControlFactory, controlFactoryPassedToCreator);
        }


        [Test]
        public void TestCustomMenuHandlerTakesPrecedence()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.GetRandomString());
            bool customHandlerCalled = false;
            EventHandler customerHandler = delegate
            {
                customHandlerCalled = true;
            };
            bool formControlHandlerCalled = false;
            FormControlCreator formControlCreatorDelegate = delegate
            {
                formControlHandlerCalled = true;
                return CreateFormControlStub();
            };
            menuItem.CustomHandler += customerHandler;
            menuItem.FormControlCreator += formControlCreatorDelegate;
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Execute Test ----------------------
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.IsFalse(formControlHandlerCalled);
            Assert.IsTrue(customHandlerCalled);
        }

        [Test]
        public void TestHandlesError_FromCustomHandler()
        {
            //---------------Set up test pack-------------------
            MockExceptionNotifier exceptionNotifier = new MockExceptionNotifier();
            GlobalRegistry.UIExceptionNotifier = exceptionNotifier;
            Exception exception = new Exception();
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.GetRandomString());
            menuItem.CustomHandler += delegate {
                throw exception;
            };
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];

            //-------------Assert Preconditions -------------
            Assert.IsNull(exceptionNotifier.Exception);
            Assert.IsNull(exceptionNotifier.FurtherMessage);
            Assert.IsNull(exceptionNotifier.Title);

            //---------------Execute Test ----------------------
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.AreEqual(exception, exceptionNotifier.Exception);
            Assert.IsNull(null, exceptionNotifier.FurtherMessage);
            Assert.IsNull(null, exceptionNotifier.Title);
        }


        [Test]
        public void TestHandlesError_FromFormControlCreator()
        {
            //---------------Set up test pack-------------------
            MockExceptionNotifier exceptionNotifier = new MockExceptionNotifier();
            GlobalRegistry.UIExceptionNotifier = exceptionNotifier;
            Exception exception = new Exception();
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.GetRandomString());
            menuItem.FormControlCreator += delegate
            {
                throw exception;
            };
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
            menu.DockInForm(habaneroMenu.Form);

            //-------------Assert Preconditions -------------
            Assert.IsNull(exceptionNotifier.Exception);
            Assert.IsNull(exceptionNotifier.FurtherMessage);
            Assert.IsNull(exceptionNotifier.Title);

            //---------------Execute Test ----------------------
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.AreEqual(exception, exceptionNotifier.Exception);
            Assert.IsNull(null, exceptionNotifier.FurtherMessage);
            Assert.IsNull(null, exceptionNotifier.Title);
        }

        [Test]
        public void TestHandlesError_FromControlManagerCreator()
        {
            //---------------Set up test pack-------------------
            MockExceptionNotifier exceptionNotifier = new MockExceptionNotifier();
            GlobalRegistry.UIExceptionNotifier = exceptionNotifier;
            Exception exception = new Exception();
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.GetRandomString());
            menuItem.ControlManagerCreator += delegate
            {
                throw exception;
            };
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
            menu.DockInForm(habaneroMenu.Form);

            //-------------Assert Preconditions -------------
            Assert.IsNull(exceptionNotifier.Exception);
            Assert.IsNull(exceptionNotifier.FurtherMessage);
            Assert.IsNull(exceptionNotifier.Title);

            //---------------Execute Test ----------------------
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.AreEqual(exception, exceptionNotifier.Exception);
            Assert.IsNull(null, exceptionNotifier.FurtherMessage);
            Assert.IsNull(null, exceptionNotifier.Title);
        }

        [Test]
        public void TestDockMenuInForm()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
            submenu.AddMenuItem(TestUtil.GetRandomString());
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IFormHabanero form = (IFormHabanero) habaneroMenu.Form;
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            //-------------Assert Preconditions -------------
            Assert.IsFalse(IsMenuDocked(menu, form));
            //---------------Execute Test ----------------------
            menu.DockInForm(form);
            //---------------Test Result -----------------------
            Assert.IsTrue(IsMenuDocked(menu, form));
        }

        [Test]
        public void TestClickMenuItemDocksControlInForm()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            IFormHabanero frm = (IFormHabanero) habaneroMenu.Form;
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.GetRandomString());
            IFormControl expectedFormControl = CreateFormControlStub();
            menuItem.FormControlCreator += (() => expectedFormControl);
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            menu.DockInForm(habaneroMenu.Form);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];

            //---------------Execute Test ----------------------
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            AssertControlDockedInForm((IControlHabanero)expectedFormControl, frm);
                      
        }

        [Test]
        public void TestClickMenuItemTwiceOnlyLeavesSameControlDocked()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.GetRandomString());
            IFormControl expectedFormControl = CreateFormControlStub();
            menuItem.FormControlCreator += (() => expectedFormControl);
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            menu.DockInForm(habaneroMenu.Form);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
            formsMenuItem.PerformClick();

            //-------------Assert Preconditions -------------
            AssertControlDockedInForm((IControlHabanero)expectedFormControl, (IFormHabanero) habaneroMenu.Form);

            //---------------Execute Test ----------------------
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            AssertControlDockedInForm((IControlHabanero)expectedFormControl, (IFormHabanero) habaneroMenu.Form);
                      
        }

        [Test]
        public void TestClickSecondItemDocksNewControlInForm()
        {
            //---------------Set up test pack-------------------

            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
            HabaneroMenu.Item menuItem1 = submenu.AddMenuItem(TestUtil.GetRandomString());
            IFormControl expectedFormControl1 = CreateFormControlStub();
            menuItem1.FormControlCreator += (() => expectedFormControl1);
            HabaneroMenu.Item menuItem2 = submenu.AddMenuItem(TestUtil.GetRandomString());
            IFormControl expectedFormControl2 = CreateFormControlStub();
            menuItem2.FormControlCreator += (() => expectedFormControl2);

            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            menu.DockInForm(habaneroMenu.Form);
            IMenuItem formsMenuItem1 = menu.MenuItems[0].MenuItems[0];
            IMenuItem formsMenuItem2 = menu.MenuItems[0].MenuItems[1];
            formsMenuItem1.PerformClick();

            //-------------Assert Preconditions -------------
            AssertControlDockedInForm((IControlHabanero)expectedFormControl1, (IFormHabanero) habaneroMenu.Form);

            //---------------Execute Test ----------------------
            formsMenuItem2.PerformClick();

            //---------------Test Result -----------------------
            AssertControlDockedInForm((IControlHabanero)expectedFormControl2, (IFormHabanero) habaneroMenu.Form);
        }

        public class ControlManagerStub : IControlManager
        {
            private readonly IControlFactory _controlFactory;
            private readonly IControlHabanero _control;

            public ControlManagerStub(IControlFactory controlFactory)
            {
                _controlFactory = controlFactory;
                _control = _controlFactory.CreateControl();
            }

            public IControlHabanero Control
            {
                get { return _control; }
            }
        }
    }
}
