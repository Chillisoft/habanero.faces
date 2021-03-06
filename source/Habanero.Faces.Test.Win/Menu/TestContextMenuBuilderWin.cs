﻿using System;
using System.Drawing;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Menu
{
    [TestFixture]
    public class TestContextMenuBuilderWin : TestContextMenuBuilder
    {

        protected override IControlFactory CreateControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override IFormControlStub CreateFormControlStub()
        {
            return new FormControlStubWin();
        }

        protected override IMenuBuilder CreateMenuBuilder()
        {
            return new ContextMenuBuilderWin(GetControlFactory());
        }

        private class FormControlStubWin : UserControlWin, IFormControlStub
        {
            public void SetForm(IFormHabanero form)
            {
                SetFormCalled = true;
                SetFormArgument = form;
            }

            public IFormHabanero SetFormArgument { get; private set; }

            public bool SetFormCalled { get; private set; }
        }

        [Test]
        public void Test_Constructor_ShouldSetControlFactory()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var contextMenuBuilderWin = new ContextMenuBuilderWin(GlobalUIRegistry.ControlFactory);
            //---------------Test Result -----------------------
            var controlFactory = contextMenuBuilderWin.ControlFactory;
            Assert.IsNotNull(controlFactory);
            Assert.IsInstanceOf<ControlFactoryWin>(controlFactory);
        }

        [Test]
        public void Test_Construct_WithNullControlFactory_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new ContextMenuBuilderWin(null);
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
        public void Test_BuildMainMenu_ShouldReturnTypeOf_ContextMenuWin()
        {
            //---------------Set up test pack-------------------
            //---------------Set up test pack-------------------
            IMenuBuilder contextMenuBuilderWin = new ContextMenuBuilderWin(GlobalUIRegistry.ControlFactory);
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var menu = contextMenuBuilderWin.BuildMainMenu(habaneroMenu);

            //---------------Test Result -----------------------
            Assert.IsInstanceOf<ContextMenuWin>(menu);
        }


        [Test]
        public void Test_BuildMainMenu_WhenAddingHabaneroMenuWithOneMenuItem_ShouldReturnMenuWithOneMenuItem()
        {
            //---------------Set up test pack-------------------
            IMenuBuilder contextMenuBuilderWin = new ContextMenuBuilderWin(GlobalUIRegistry.ControlFactory);
            HabaneroMenu habaneroMenu = new HabaneroMenu("Main");
            string menuNameItem = "test";
            habaneroMenu.AddMenuItem(menuNameItem);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, habaneroMenu.MenuItems.Count);

            //---------------Execute Test ----------------------
            var menu = contextMenuBuilderWin.BuildMainMenu(habaneroMenu);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, menu.MenuItems.Count);

        }


    }
}