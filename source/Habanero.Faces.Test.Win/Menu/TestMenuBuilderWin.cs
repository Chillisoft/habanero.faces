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
    public class TestMenuBuilderWin : TestMenuBuilder
    {
        private IControlFactory _factory;

        protected override IControlFactory GetControlFactory()
        {
            if ((_factory == null)) _factory = new ControlFactoryWin();

            GlobalUIRegistry.ControlFactory = _factory;
            return _factory;
        }

        protected override IMenuBuilder CreateMenuBuilder()
        {
            return new MenuBuilderWin(GetControlFactory());
        }

        protected override IFormControlStub CreateFormControlStub()
        {
            return new FormControlStubWin();
        }

        protected override bool IsMenuDocked(IMainMenuHabanero menu, IFormHabanero form)
        {
            System.Windows.Forms.Form formWin = (System.Windows.Forms.Form)form;
            return formWin.Menu == menu && form.IsMdiContainer;
        }

        protected override void AssertControlDockedInForm(IControlHabanero control, IFormHabanero form)
        {
            System.Windows.Forms.Form winForm = (System.Windows.Forms.Form)form;
            Assert.LessOrEqual(1, winForm.MdiChildren.Length);
            bool found = false;
            foreach (System.Windows.Forms.Form childForm in winForm.MdiChildren)
            {
                Assert.AreEqual(1, childForm.Controls.Count);
                System.Windows.Forms.Control childFormControl = childForm.Controls[0];
                if (childFormControl == control)
                {
                    found = true;
                    //Assert.AreSame(childForm, winForm.ActiveMdiChild,
                    //               "Control found in MDI children, but not the current docked form");
                    break;
                }
            }
            Assert.IsTrue(found, "Form was not found");
        }


        [Test]
        public void Test_Construct_SetsControlFactory()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IMenuBuilder menuBuilder = new MenuBuilderWin(GetControlFactory());
            //---------------Test Result -----------------------
            Assert.AreSame(GetControlFactory(), menuBuilder.ControlFactory);

        }
        [Test]
        public void Test_Construct_WithNullControlFactory_ShouldRaiseError()
        {
            //---------------Execute Test ----------------------
            try
            {
                new MenuBuilderWin(null);
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
        public void TestCloseFormAndClickCreatesNewForm()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu habaneroMenu = CreateHabaneroMenuFullySetup();
            IFormHabanero frm = (IFormHabanero)habaneroMenu.Form;
            frm.Show();
            HabaneroMenu submenu = habaneroMenu.AddSubMenu(TestUtil.GetRandomString());
            HabaneroMenu.Item menuItem = submenu.AddMenuItem(TestUtil.GetRandomString());


            menuItem.FormControlCreator += (() => new FormControlStubWin());
            IMenuBuilder menuBuilder = CreateMenuBuilder();
            IMainMenuHabanero menu = menuBuilder.BuildMainMenu(habaneroMenu);
            menu.DockInForm(habaneroMenu.Form);
            IMenuItem formsMenuItem = menu.MenuItems[0].MenuItems[0];
            formsMenuItem.PerformClick();
            System.Windows.Forms.Form winForm = (System.Windows.Forms.Form)frm;
            System.Windows.Forms.Form childForm = winForm.MdiChildren[0];
            System.Windows.Forms.Control expectedFormControl = childForm.Controls[0];
            //---------------Execute Test ----------------------

            childForm.Close();
            formsMenuItem.PerformClick();
            //---------------Test Result -----------------------

            Assert.AreEqual(1, winForm.MdiChildren.Length);
            childForm = winForm.MdiChildren[0];
            Assert.AreEqual(1, childForm.Controls.Count);
            Assert.IsInstanceOf(typeof(FormControlStubWin), winForm.MdiChildren[0].Controls[0]);
            Assert.AreNotSame(expectedFormControl, winForm.MdiChildren[0].Controls[0]);


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
    }
}