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
    public class TestMenuBuilderVWG : TestMenuBuilder
    {
        private ControlFactoryVWG _factory;

        protected override IControlFactory GetControlFactory()
        {
            if ((_factory == null)) _factory = new ControlFactoryVWG();

            GlobalUIRegistry.ControlFactory = _factory;
            return _factory;
        }

        protected override IMenuBuilder CreateMenuBuilder()
        {
            return new MenuBuilderVWG(GetControlFactory());
        }

        protected override IFormControlStub CreateFormControlStub()
        {
            return new FormControlStubVWG();
        }
        protected override bool IsMenuDocked(IMainMenuHabanero menu, IFormHabanero form)
        {
            Gizmox.WebGUI.Forms.Form formWin = (Gizmox.WebGUI.Forms.Form)form;
            return formWin.Menu == menu && form.Controls.Count == 1;
        }

        protected override void AssertControlDockedInForm(IControlHabanero control, IFormHabanero form)
        {
            Assert.AreEqual(1, form.Controls.Count, "No container control found in form");
            IControlHabanero contentControl = form.Controls[0];
            Assert.AreEqual(1, contentControl.Controls.Count);
            Assert.AreSame(control, contentControl.Controls[0]);
            Assert.AreEqual(DockStyle.Fill, control.Dock);
        }

        private class FormControlStubVWG : UserControlVWG, IFormControlStub
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
        public void Test_Construct_SetsControlFactory()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IMenuBuilder menuBuilder = new MenuBuilderVWG(GetControlFactory());
            //---------------Test Result -----------------------
            Assert.AreSame(GetControlFactory(), menuBuilder.ControlFactory);

        }

        [Test]
        public void Test_Construct_WithNullControlFactory_ShouldRaiseError()
        {
            //---------------Execute Test ----------------------
            try
            {
                new MenuBuilderVWG(null);
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
        public override void Test_Click_WhenFormControlCreatorSet_ShouldCallSetFormOnFormControl()
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
            //The MenuBuilderVWG sites the control on a UserControl instead of a form (VWG does not support MDI Children), so this next assert would fail.
            //Assert.IsNotNull(formControlStub.SetFormArgument);
        }

    }
}