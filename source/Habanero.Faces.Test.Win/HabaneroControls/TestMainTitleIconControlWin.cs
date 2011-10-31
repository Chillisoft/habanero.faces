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
using System.Drawing;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
    [TestFixture]
    public class TestMainTitleIconControlWin : TestMainTitleIconControl
    {
        protected virtual IMainTitleIconControl CreateControl()
        {
            IControlFactory factory = GetControlFactory();
            return new MainTitleIconControlWin(factory);
        }
        protected override IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = CreateNewControlFactory();
            return GlobalUIRegistry.ControlFactory;
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }
        [Test]
        public override void Test_Construction_WithControlFactory_ShouldSetControlFactory()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = CreateNewControlFactory();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMainTitleIconControl outlookStyleMenu = new MainTitleIconControlWin(factory);
            //---------------Test Result -----------------------
            Assert.AreSame(factory, outlookStyleMenu.ControlFactory);
        }
        [Test]
        public void Test_Construction_WithControlFactory_Null_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                new MainTitleIconControlWin(null);
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
        public void Test_CreateMainTitleIconControl()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            MainTitleIconControlWin titleIconControl = new MainTitleIconControlWin(GetControlFactory());
            //---------------Test Result -----------------------
            Assert.AreEqual(23, titleIconControl.Height);
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(DockStyle.Top, DockStyleWin.GetDockStyle(titleIconControl.Dock));

            Assert.IsInstanceOf(typeof(IPanel), titleIconControl.Panel);
            const string headerImage = "Images.headergradient.png";
            AssertBackGroundImageIsSet(titleIconControl, headerImage);
            AssertBackGroundimageIsTile(titleIconControl);
            Assert.AreEqual(Color.Transparent, titleIconControl.Panel.BackColor);
            Assert.AreEqual(DockStyle.Top, titleIconControl.Panel.Dock);
            Assert.AreEqual(23, titleIconControl.Panel.Height);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);

            Assert.IsInstanceOf(typeof(ILabel), titleIconControl.Icon);
            Assert.IsNotNull(GetBackGroundImage(titleIconControl));
            Assert.AreEqual(Color.Transparent, titleIconControl.Icon.BackColor);
            AssertBackGroundImagelayoutCentre(titleIconControl);
            Assert.AreEqual(DockStyle.Left, titleIconControl.Icon.Dock);


            Assert.IsInstanceOf(typeof(ILabel), titleIconControl.Title);
            Assert.IsEmpty("", titleIconControl.Title.Text);
            Assert.AreEqual(DockStyle.Fill, titleIconControl.Title.Dock);
            Assert.AreEqual(Color.Transparent, titleIconControl.Title.BackColor);
            Assert.AreEqual(ContentAlignment.MiddleLeft, titleIconControl.Title.TextAlign);
        }



        protected void AssertBackGroundImageIsSet(IMainTitleIconControl titleIconControl, string headerImage)
        {
            //TODO Brett 20 Apr 2009: Nubb to fix
            //Assert.AreEqual
            //    (headerImage, ((PanelWin)titleIconControl.Panel).BackgroundImage.ToString());
        }

        protected object GetBackGroundImage(IMainTitleIconControl titleIconControl)
        {
            return ((LabelWin)titleIconControl.Icon).BackgroundImage;
        }

        protected void AssertBackGroundImagelayoutCentre(IMainTitleIconControl titleIconControl)
        {
            Assert.AreEqual
                (System.Windows.Forms.ImageLayout.Center, ((LabelWin)titleIconControl.Icon).BackgroundImageLayout);
        }

        protected void AssertBackGroundimageIsTile(IMainTitleIconControl titleIconControl)
        {
            Assert.AreEqual
                (System.Windows.Forms.ImageLayout.Tile, ((PanelWin)titleIconControl.Panel).BackgroundImageLayout);
        }

        [Test]
        public void TestSetIcon()
        {
            //---------------Set up test pack-------------------
            MainTitleIconControlWin titleIconControl = new MainTitleIconControlWin(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);
            //---------------Execute Test ----------------------
            titleIconControl.SetIconImage("Images.Valid.gif");
            //---------------Test Result -----------------------
            //TODO Brett 20 Apr 2009: Nubb to fix
            //Assert.AreEqual("Images.Valid.gif", ((LabelWin)titleIconControl.Icon).BackgroundImage.ToString());
        }

        [Test]
        public void TestSetValidIcon()
        {
            //---------------Set up test pack-------------------
            MainTitleIconControlWin titleIconControl = new MainTitleIconControlWin(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);
            //---------------Execute Test ----------------------
            titleIconControl.SetValidImage();
            //---------------Test Result -----------------------
            //TODO Brett 20 Apr 2009: Nubb to fix  Assert.AreEqual("Images.Valid.gif", ((LabelWin)titleIconControl.Icon).BackgroundImage.ToString());
        }

        [Test]
        public void TestSetInvalidIcon()
        {
            //---------------Set up test pack-------------------
            MainTitleIconControlWin titleIconControl = new MainTitleIconControlWin(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);
            //---------------Execute Test ----------------------
            titleIconControl.SetInvalidImage();
            //---------------Test Result -----------------------
            //TODO Brett 20 Apr 2009: Nubb to fix  Assert.AreEqual("Images.Invalid.gif", ((LabelWin)titleIconControl.Icon).BackgroundImage.ToString());
        }

        [Test]
        public void TestRemoveIcon()
        {
            //---------------Set up test pack-------------------
            MainTitleIconControlWin titleIconControl = new MainTitleIconControlWin(GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);
            //---------------Execute Test ----------------------
            titleIconControl.RemoveIconImage();
            //---------------Test Result -----------------------
            Assert.IsNotNull(((LabelWin)titleIconControl.Icon).BackgroundImage);
        }

    }
}