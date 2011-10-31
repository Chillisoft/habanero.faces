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
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
    [TestFixture]
    public class TestButtonGroupControlWin : TestButtonGroupControl
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override void AddControlToForm(IControlHabanero cntrl)
        {
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add((System.Windows.Forms.Control)cntrl);
        }

        //    //protected override IReadOnlyGridControl CreateReadOnlyGridControl()
        //    //{
        //    //    ReadOnlyGridControlWin readOnlyGridControlWin = new ReadOnlyGridControlWin();
        //    //    System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
        //    //    frm.Controls.Add(readOnlyGridControlWin);
        //    //    return readOnlyGridControlWin;
        //    //}
        [Test]
        public void Test_SetDefaultButton_WinOnly()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            IButton btn = buttons.AddButton("Test");
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add((System.Windows.Forms.Control)buttons);
            //---------------Execute Test ----------------------
            buttons.SetDefaultButton("Test");
            //---------------Test Result -----------------------
            Assert.AreSame(btn, frm.AcceptButton);
        }

        [Test]
        public void Test_UseMnemonic_WinOnly()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();

            //---------------Execute Test ----------------------
            System.Windows.Forms.Button btn = (System.Windows.Forms.Button)buttons.AddButton("Test", delegate { });
            //---------------Test Result -----------------------
            Assert.IsTrue(btn.UseMnemonic);
        }

        [Test]
        public void Test_ButtonIndexer_WithASpecialCharactersInTheName_Failing()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            //---------------Execute Test ----------------------
            const string buttonText = "T est@";
            IButton btn = buttons.AddButton(buttonText);
            //---------------Test Result -----------------------
            Assert.AreSame(btn, buttons["T est@"]);
        }
    }
}