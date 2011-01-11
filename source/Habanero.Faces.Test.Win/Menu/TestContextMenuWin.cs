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