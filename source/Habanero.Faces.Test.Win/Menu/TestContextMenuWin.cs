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
using System.Windows.Forms;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Menu
{
    /// <summary>
    /// This test class tests the classes that implement IMainMenuHabanero.
    /// </summary>
    [TestFixture]
    public class TestContextMenuWin : TestContextMenu
    {

        protected override IMainMenuHabanero CreateControl()
        {
            return new ContextMenuWin();
        }

        protected override IMainMenuHabanero CreateControl(HabaneroMenu menu)
        {
            return new ContextMenuWin(menu);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }

        [Ignore("Visual Testing")]
        [Test]
        public void Test_Method()
        {

            ////---------------Set up test pack-------------------
            //FormWin frm = new FormWin();

            ////---------------Assert Precondition----------------
            //HabaneroMenu habaneroMenu = new HabaneroMenu("treeviewcontext");
            //habaneroMenu.AddMenuItem("wibble").CustomHandler += delegate { MessageBox.Show(@"hello world"); };
            //habaneroMenu.AddMenuItem("yebo").CustomHandler += delegate { MessageBox.Show(@"option 2"); };
            //habaneroMenu.AddMenuItem("gogo").CustomHandler += delegate { MessageBox.Show(@"option 3"); };
            //IMenuBuilder builder = new ContextMenuBuilderWin(GlobalUIRegistry.ControlFactory);
            //IMainMenuHabanero mainMenuHabanero = builder.BuildMainMenu(habaneroMenu);
            //frm.MouseUp += delegate(object sender, MouseEventArgs e)
            //{
            //    var contextMenuWin = (ContextMenuWin)mainMenuHabanero;
            //    contextMenuWin.Show(frm, new Point(e.X, e.Y));
            //};
            //frm.ShowDialog();
            ////---------------Execute Test ----------------------

            ////---------------Test Result -----------------------
            //Assert.Fail("Not Yet Implemented");
        }
    }
}