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
using Habanero.Faces.Test.Base.Grid;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Grid
{
    [TestFixture]
    public class TestDataGridViewWin : TestDataGridView
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override string GetUnderlyingDataGridViewSelectionModeToString(IDataGridView dataGridView)
        {
            System.Windows.Forms.DataGridView control = (System.Windows.Forms.DataGridView)dataGridView;
            return control.SelectionMode.ToString();
        }

        protected override void AddToForm(IDataGridView dgv)
        {
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            form.Controls.Add((System.Windows.Forms.Control)dgv);
        }

        [Test]
        public void Test_GetItemsPerPage_ShouldReturnBase()
        {
            //---------------Set up test pack-------------------
            IDataGridView gridViewWin = new DataGridViewWin();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            int itemsPerPage = gridViewWin.ItemsPerPage;
            //---------------Test Result -----------------------
            Assert.AreEqual(0, itemsPerPage);
        }
        [Test]
        public void Test_SetItemsPerPage_ShouldReturnBase()
        {
            //---------------Set up test pack-------------------
            IDataGridView gridViewWin = new DataGridViewWin();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            gridViewWin.ItemsPerPage = 22;
            //---------------Test Result -----------------------
            Assert.AreEqual(22, gridViewWin.ItemsPerPage);
        }
        [Test]
        public void Test_GetTotalItems_ShouldReturnBase()
        {
            //---------------Set up test pack-------------------
            IDataGridView gridViewWin = new DataGridViewWin();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            int totalItems = gridViewWin.TotalItems;
            //---------------Test Result -----------------------
            Assert.AreEqual(0, totalItems);
        }
        [Test]
        public void Test_SetTotalItems_ShouldReturnBase()
        {
            //---------------Set up test pack-------------------
            IDataGridView gridViewWin = new DataGridViewWin();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            gridViewWin.TotalItems = 22;
            //---------------Test Result -----------------------
            Assert.AreEqual(22, gridViewWin.TotalItems);
        }
        [Test]
        public void Test_GetTotalPages_ShouldReturnBase()
        {
            //---------------Set up test pack-------------------
            IDataGridView gridViewWin = new DataGridViewWin();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            int totalPages = gridViewWin.TotalPages;
            //---------------Test Result -----------------------
            Assert.AreEqual(0, totalPages);
        }
        [Test]
        public void Test_SetTotalPages_ShouldReturnBase()
        {
            //---------------Set up test pack-------------------
            IDataGridView gridViewWin = new DataGridViewWin();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            gridViewWin.TotalPages = 22;
            //---------------Test Result -----------------------
            Assert.AreEqual(22, gridViewWin.TotalPages);
        }
    }
}