using System;
using System.Drawing;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Test;
using Habanero.Test.Structure;
using Habanero.Faces.Base;

using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    public abstract class TestContextMenuBuilder
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
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
        }

        protected abstract IControlFactory CreateControlFactory();
        protected abstract IFormControlStub CreateFormControlStub();
        protected abstract IMenuBuilder CreateMenuBuilder();
        //protected abstract void AssertControlDockedInForm(IControlHabanero control, IControlHabanero form);

        protected HabaneroMenu CreateHabaneroMenuFullySetup()
        {
            IControlFactory controlFactory = GetControlFactory();
            IFormHabanero form = controlFactory.CreateForm();
            return new HabaneroMenu("Main", form, controlFactory);
        }

        [Test]
        public void Test_Construct_ContextMenuBuilder()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(IMenuBuilder), menuBuilder);
        }

        [Test]
        public void Test_BuildMainMenu()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            //---------------Test Result -----------------------
            Assert.IsNotNull(menu);
            Assert.AreEqual(habaneroMenu.Name, menu.Name);
        }

        [Test]
        public void Test_AddMenuItem_Should_ReturnMenuItemAfterBuildMainMenu()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            string menuItemName = TestUtil.GetRandomString();
            habaneroMenu.AddMenuItem(menuItemName);


            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Assert Preconditions---------------
            Assert.AreEqual(1, habaneroMenu.MenuItems.Count);
            //---------------Execute Test ----------------------
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, menu.MenuItems.Count);
            Assert.AreEqual(menuItemName, menu.MenuItems[0].Text);
        }

        [Test]
        public void Test_AddTwoMenuItems_Should_ReturnTwoMenuItemAfterBuildMainMenu()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            string menuItemName = TestUtil.GetRandomString();
            habaneroMenu.AddMenuItem(menuItemName);
            string menuItemName2 = TestUtil.GetRandomString();
            habaneroMenu.AddMenuItem(menuItemName2);


            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Assert Preconditions---------------
            Assert.AreEqual(2, habaneroMenu.MenuItems.Count);
            //---------------Execute Test ----------------------
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, menu.MenuItems.Count);
            Assert.AreEqual(menuItemName, menu.MenuItems[0].Text);
            Assert.AreEqual(menuItemName2, menu.MenuItems[1].Text);
        }

        [Test]
        public void Test_AddMultipleMenuItems_Should_ReturnMultipleMenuItemAfterBuildMainMenu()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            string menuItemName = TestUtil.GetRandomString();
            habaneroMenu.AddMenuItem(menuItemName);
            string menuItemName2 = TestUtil.GetRandomString();
            habaneroMenu.AddMenuItem(menuItemName2);
            string menuItemName3 = TestUtil.GetRandomString();
            habaneroMenu.AddMenuItem(menuItemName3);


            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Assert Preconditions---------------
            Assert.AreEqual(3, habaneroMenu.MenuItems.Count);
            //---------------Execute Test ----------------------
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, menu.MenuItems.Count);
            Assert.AreEqual(menuItemName, menu.MenuItems[0].Text);
            Assert.AreEqual(menuItemName2, menu.MenuItems[1].Text);
            Assert.AreEqual(menuItemName3, menu.MenuItems[2].Text);
        }



        [Test]
        public void Test_PerformClick_NoCreatorsCalledWhenMenuFormNotSet()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            string menuItemName = TestUtil.GetRandomString();
            var menuItem = habaneroMenu.AddMenuItem(menuItemName);
            //HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
            //HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.GetRandomString());
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
            IMenuItem formsMenuItem = menu.MenuItems[0];
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.IsFalse(called);
        }

        [Ignore("I am not sure this test would make sense as a context menu is does not get associated with form")]
        [Test]
        public virtual void Test_Click_WhenFormControlCreatorSet_ShouldCallSetFormOnFormControl()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu =  CreateHabaneroMenuFullySetup();
            var menuItem = habaneroMenu.AddMenuItem(TestUtil.GetRandomString());

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
            //menu.DockInForm(habaneroMenu.Form);
            IMenuItem formsMenuItem = menu.MenuItems[0];
            //--------------- Test Preconditions ----------------
            Assert.IsFalse(creatorCalled);
            Assert.IsFalse(formControlStub.SetFormCalled);
            Assert.IsNull(formControlStub.SetFormArgument);
            //---------------Execute Test ----------------------
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.IsTrue(creatorCalled);
            Assert.IsTrue(formControlStub.SetFormCalled);
            //The MenuBuilderVWG sites the control on a UserControl instead of a form (VWG does not support MDI Children), so this next assert would fail.
            //Assert.IsNotNull(formControlStub.SetFormArgument);
        }

        [Test]
        public void Test_PerformClick_WhenMenuFormHasNoControlSet_ShouldNotCallCreators()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main", GetControlFactory().CreateForm(), GetControlFactory());
            var menuItem = habaneroMenu.AddMenuItem(TestUtil.GetRandomString());
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
            IMenuItem formsMenuItem = menu.MenuItems[0];
            formsMenuItem.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsFalse(called);
        }

        [Ignore("Andrew - No form to dock to...")]
        [Test]
        public void TestFormControlCreatorCalledOnClickIfSet()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            var menuItem = habaneroMenu.AddMenuItem(TestUtil.GetRandomString());
            bool called = false;
            FormControlCreator formControlCreatorDelegate = delegate
            {
                called = true;
                return CreateFormControlStub();
            };
            menuItem.FormControlCreator += formControlCreatorDelegate;
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            //menu.DockInForm(habaneroMenu.Form);
            IMenuItem formsMenuItem = menu.MenuItems[0];

            //---------------Execute Test ----------------------
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.IsTrue(called);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCustomMenuHandlerCalledIfSet()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            var menuItem = habaneroMenu.AddMenuItem(TestUtil.GetRandomString());
            bool called = false;
            System.EventHandler customerHandler = delegate { called = true; };
            menuItem.CustomHandler += customerHandler;
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            //---------------Execute Test ----------------------
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            IMenuItem formsMenuItem = menu.MenuItems[0];
            formsMenuItem.PerformClick();

            //---------------Test Result -----------------------
            Assert.IsTrue(called);
            //---------------Tear Down -------------------------                
        }

        [Ignore("Andrew - No form to dock to...")]
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
            var menuItem = habaneroMenu.AddMenuItem(TestUtil.GetRandomString());
            bool customHandlerCalled = false;
            EventHandler customerHandler = delegate { customHandlerCalled = true; };
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
            IMenuItem formsMenuItem = menu.MenuItems[0];
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
            var menuItem = habaneroMenu.AddMenuItem(TestUtil.GetRandomString());
            menuItem.CustomHandler += delegate { throw exception; };
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            IMenuItem formsMenuItem = menu.MenuItems[0];

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

        [Ignore("Andrew - No form to dock to...")]
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
            menuItem.FormControlCreator += delegate { throw exception; };
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

        [Ignore("Andrew - No form to dock to...")]
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
            menuItem.ControlManagerCreator += delegate { throw exception; };
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

        //[Test]
        //public void TestClickMenuItemDocksControlInForm()
        //{
        //    //---------------Set up test pack-------------------
        //    HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
        //    IControlHabanero frm = habaneroMenu.Form;
        //    HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
        //    HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.GetRandomString());
        //    IFormControl expectedFormControl = CreateFormControlStub();
        //    menuItem.FormControlCreator += (() => expectedFormControl);
        //    IMenuBuilder menuBuilder = CreateMenuBuilder();
        //    IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
        //    menu.DockInForm(habaneroMenu.Form);
        //    IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];

        //    //---------------Execute Test ----------------------
        //    formsMenuItem.PerformClick();

        //    //---------------Test Result -----------------------
        //    AssertControlDockedInForm((IControlHabanero)expectedFormControl, frm);
        //}

        //[Test]
        //public void TestClickMenuItemTwiceOnlyLeavesSameControlDocked()
        //{
        //    //---------------Set up test pack-------------------
        //    HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
        //    HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
        //    HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.GetRandomString());
        //    IFormControl expectedFormControl = CreateFormControlStub();
        //    menuItem.FormControlCreator += (() => expectedFormControl);
        //    IMenuBuilder menuBuilder = CreateMenuBuilder();
        //    IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
        //    menu.DockInForm(habaneroMenu.Form);
        //    IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
        //    formsMenuItem.PerformClick();

        //    //-------------Assert Preconditions -------------
        //    AssertControlDockedInForm((IControlHabanero)expectedFormControl, habaneroMenu.Form);

        //    //---------------Execute Test ----------------------
        //    formsMenuItem.PerformClick();

        //    //---------------Test Result -----------------------
        //    AssertControlDockedInForm((IControlHabanero)expectedFormControl, habaneroMenu.Form);
        //}

        //[Test]
        //public void TestClickSecondItemDocksNewControlInForm()
        //{
        //    //---------------Set up test pack-------------------

        //    HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
        //    HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
        //    HabaneroMenu.Item menuItem1 = submenu.AddMenuItem(TestUtil.GetRandomString());
        //    IFormControl expectedFormControl1 = CreateFormControlStub();
        //    menuItem1.FormControlCreator += (() => expectedFormControl1);
        //    HabaneroMenu.Item menuItem2 = submenu.AddMenuItem(TestUtil.GetRandomString());
        //    IFormControl expectedFormControl2 = CreateFormControlStub();
        //    menuItem2.FormControlCreator += (() => expectedFormControl2);

        //    IMenuBuilder menuBuilder = CreateMenuBuilder();
        //    IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
        //    menu.DockInForm(habaneroMenu.Form);
        //    IMenuItem formsMenuItem1 = menu.MenuItems[0].MenuItems[0];
        //    IMenuItem formsMenuItem2 = menu.MenuItems[0].MenuItems[1];
        //    formsMenuItem1.PerformClick();

        //    //-------------Assert Preconditions -------------
        //    AssertControlDockedInForm((IControlHabanero)expectedFormControl1, habaneroMenu.Form);

        //    //---------------Execute Test ----------------------
        //    formsMenuItem2.PerformClick();

        //    //---------------Test Result -----------------------
        //    AssertControlDockedInForm((IControlHabanero)expectedFormControl2, habaneroMenu.Form);
        //}


        //protected virtual bool IsMenuDocked(IMainMenuHabanero menu, IControlHabanero form)
        //{
        //    return form.Controls.Count == 1;
        //}

        public interface IFormControlStub : IFormControl
        {
            IFormHabanero SetFormArgument { get; }
            bool SetFormCalled { get; }
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

        protected virtual IControlFactory GetControlFactory()
        {
            IControlFactory factory = CreateControlFactory();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
    }

}