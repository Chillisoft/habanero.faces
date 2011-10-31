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
using System.Collections.Generic;
using System.Text;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;

using NUnit.Framework;

namespace Habanero.Faces.Test.Base.ButtonsControl
{
    public abstract class TestButtonSizePolicy
    {
        [SetUp]
        public void SetupTest()
        {
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract IControlFactory GetControlFactory();

        protected abstract IButtonSizePolicy CreateButtonSizePolicy();

        [Test]
        public void TestButtonWidth_ResizingAccordingToButtonText()
        {
            //---------------Set up test pack-------------------

            IButtonSizePolicy buttonSizePolicy = CreateButtonSizePolicy();
            const string buttonText = "TestMustBeLongEnoughToBeGreaterThanTwelthOfScreen";
            IControlCollection buttonCollection = GetControlFactory().CreatePanel().Controls;
            IButton btnTest = GetControlFactory().CreateButton(buttonText);
            buttonCollection.Add(btnTest);

            //---------------Execute Test ----------------------
            buttonSizePolicy.RecalcButtonSizes(buttonCollection);

            ////---------------Test Result -----------------------
            ILabel lbl = GetControlFactory().CreateLabel(buttonText);
            Assert.AreEqual(lbl.PreferredWidth + 15, btnTest.Width, "Button width is incorrect.");
        }


        [Test]
        public void TestButtonWidthTwoButtons()
        {
            //---------------Set up test pack-------------------
            IButtonSizePolicy buttonSizePolicy = CreateButtonSizePolicy();
            const string buttonText = "TestMustBeLongEnoughToBeGreaterThanTwelthOfScreen";
            IControlCollection buttonCollection = GetControlFactory().CreatePanel().Controls;
            IButton btnTest1 = GetControlFactory().CreateButton("Test");
            buttonCollection.Add(btnTest1);
            IButton btnTest2 = GetControlFactory().CreateButton(buttonText);
            buttonCollection.Add(btnTest2);

            //---------------Execute Test ----------------------
            buttonSizePolicy.RecalcButtonSizes(buttonCollection);
            ////---------------Test Result -----------------------

            ILabel lbl = GetControlFactory().CreateLabel(buttonText);
            Assert.AreEqual(lbl.PreferredWidth + 15, btnTest1.Width, "Button width is incorrect.");

            Assert.AreEqual(btnTest2.Width, btnTest1.Width, "Button width is incorrect.");
        }
    }



}
