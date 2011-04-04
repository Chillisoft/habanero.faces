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
using System.Drawing;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    public abstract class TestOKCancelDialog
    {
        // ReSharper disable InconsistentNaming
        //TODO: refactor - WIN and VWG are copied and pasted.
        protected abstract IControlFactory GetControlFactory();

        protected IOKCancelDialogFactory CreateOKCancelDialogFactory()
        {
            return GetControlFactory().CreateOKCancelDialogFactory();
        }



            [Test]
        public void Test_CreateOKCancelForm_ShouldDockPanel()
        {
            //---------------Set up test pack-------------------
            IOKCancelDialogFactory okCancelDialogFactory = CreateOKCancelDialogFactory();
            //---------------Execute Test ----------------------
            IFormHabanero dialogForm = okCancelDialogFactory.CreateOKCancelForm(GetControlFactory().CreatePanel(), "");

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dialogForm.Controls.Count);
        }

        [Test]
        public void Test_CreateOKCancelPanel_ShouldNestControl()
        {
            //---------------Set up test pack-------------------
            IOKCancelDialogFactory okCancelDialogFactory = CreateOKCancelDialogFactory();
            IControlHabanero nestedControl = GetControlFactory().CreatePanel();
            //---------------Execute Test ----------------------
            IControlHabanero dialogControl = okCancelDialogFactory.CreateOKCancelPanel(nestedControl);
            //---------------Test Result -----------------------
            IPanel contentPanel = (IPanel)dialogControl.Controls[0];
            Assert.AreEqual(1, contentPanel.Controls.Count);
            Assert.AreSame(nestedControl, contentPanel.Controls[0]);
        }

        [Test]
        public void Test_CreateOKCancelPanel_ShouldNotChangeControlSize()
        {
            //---------------Set up test pack-------------------
            IOKCancelDialogFactory okCancelDialogFactory = CreateOKCancelDialogFactory();
            IControlHabanero nestedControl = GetControlFactory().CreatePanel();
            int width = TestUtil.GetRandomInt(100, 500);
            int height = TestUtil.GetRandomInt(100, 500);
            nestedControl.Size = new Size(width, height);
            //---------------Assert Precondition----------------
            Assert.AreEqual(width, nestedControl.Width);
            Assert.AreEqual(height, nestedControl.Height);
            //---------------Execute Test ----------------------
            IControlHabanero dialogControl = okCancelDialogFactory.CreateOKCancelPanel(nestedControl);
            //---------------Test Result -----------------------
            Assert.AreEqual(width, nestedControl.Width, "Width should not have changed");
            Assert.AreEqual(height, nestedControl.Height, "Height should not have changed");
        }

        [Test]
        public void Test_CreateOKCancelForm_ShouldNotChangeControlSize()
        {
            //---------------Set up test pack-------------------
            IOKCancelDialogFactory okCancelDialogFactory = CreateOKCancelDialogFactory();
            IControlHabanero nestedControl = GetControlFactory().CreatePanel();
            int width = TestUtil.GetRandomInt(100, 500);
            int height = TestUtil.GetRandomInt(100, 500);
            nestedControl.Size = new Size(width, height);
            //---------------Assert Precondition----------------
            Assert.AreEqual(width, nestedControl.Width);
            Assert.AreEqual(height, nestedControl.Height);
            //---------------Execute Test ----------------------
            IControlHabanero dialogControl = okCancelDialogFactory.CreateOKCancelForm(nestedControl, "MyTestForm");
            //---------------Test Result -----------------------
            Assert.AreEqual(width, nestedControl.Width, "Width should not have changed");
            Assert.AreEqual(height, nestedControl.Height, "Height should not have changed");
        }
    }
}