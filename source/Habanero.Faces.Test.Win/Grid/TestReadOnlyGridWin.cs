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

namespace Habanero.Faces.Test.Win.Grid
{
    [TestFixture]
    public class TestReadOnlyGridWin : TestReadOnlyGrid
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
        [Test]
        public void TestCreateGridBaseWin()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero grid = GetControlFactory().CreateReadOnlyGrid();
            ReadOnlyGridWin readOnlyGrid = (ReadOnlyGridWin)grid;
            ////---------------Test Result -----------------------
            Assert.IsTrue(readOnlyGrid.ReadOnly);
            Assert.IsFalse(readOnlyGrid.AllowUserToAddRows);
            Assert.IsFalse(readOnlyGrid.AllowUserToDeleteRows);
            Assert.IsTrue(readOnlyGrid.SelectionMode == System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect);
        }
    }
}