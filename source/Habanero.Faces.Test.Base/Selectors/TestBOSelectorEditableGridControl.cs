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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{

    public abstract class TestBOSelectorEditableGridControl : TestBOColSelector
    {
        protected override void SetSelectedIndex(IBOColSelectorControl colSelector, int index)
        {
            int count = 0;

            IEditableGrid grid = ((IEditableGridControl) colSelector).Grid;
            foreach (IDataGridViewRow row in grid.Rows)
            {
                if (row == null) continue; //This is done to stop the Pragma warning.
                if (count == index)
                {
                    IBusinessObject businessObjectAtRow = grid.GetBusinessObjectAtRow(count);
                    colSelector.SelectedBusinessObject = businessObjectAtRow;
                }
                count++;
            }
        }

        protected override int SelectedIndex(IBOColSelectorControl colSelector)
        {
            IEditableGrid gridSelector = ((IEditableGridControl) colSelector).Grid;
            IDataGridViewRow currentRow = null;
            if (gridSelector.SelectedRows.Count > 0)
            {
                currentRow = gridSelector.SelectedRows[0];
            }

            if (currentRow == null) return -1;

            return gridSelector.Rows.IndexOf(currentRow);
        }

        protected override int NumberOfLeadingBlankRows()
        {
            return 0;
        }

        protected override int NumberOfTrailingBlankRows()
        {
            return 1;
        }

        [Test]
        public virtual void Test_Constructor_EditableGridControlSet()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOColSelectorControl colSelector = CreateSelector();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof (IEditableGridControl), colSelector);
        }

        [Test]
        public override void Test_Set_SelectedBusinessObject_ItemNotInList_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject myBO;
            IBusinessObjectCollection collection = this.GetCollectionWithTowBOs(out myBO);
            IBusinessObject myBO2 = collection[1];
            colSelector.BusinessObjectCollection = collection;
            SetSelectedIndex(colSelector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(ActualNumberOfRows(collection.Count), colSelector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(colSelector));
            Assert.AreEqual(myBO2, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.SelectedBusinessObject = CreateNewBO();
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualNumberOfRows(2), colSelector.NoOfItems, "The blank item");
            Assert.IsNull(colSelector.SelectedBusinessObject);
            Assert.AreEqual(-1, SelectedIndex(colSelector));
        }

        [Test]
        public override void Test_SetBOCollection_WhenAutoSelectsFirstItem_ShouldSelectFirstItem()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject myBO;
            IBusinessObjectCollection collection = this.GetCollectionWithTowBOs(out myBO);
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, colSelector.NoOfItems);
            Assert.AreEqual(-1, SelectedIndex(colSelector));
            Assert.AreEqual(null, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualNumberOfRows(collection.Count), colSelector.NoOfItems, "The blank item");
            Assert.AreSame(myBO, colSelector.SelectedBusinessObject);
            Assert.AreEqual(ActualIndex(0), SelectedIndex(colSelector));
        }

        [Ignore(" Not Yet implemented")] //TODO  01 Mar 2009:
        [Test]
        public void TestEditItemFromCollectionUpdatesItemInSelector()
        {
        }

        [Test]
        public override void Test_AutoSelectsFirstItem_NoItems()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObjectCollection collection = GetCollectionWithNoItems();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, colSelector.NoOfItems);
            Assert.AreEqual(-1, SelectedIndex(colSelector));
            Assert.AreEqual(null, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualNumberOfRows(0), colSelector.NoOfItems, "The blank item");
            Assert.AreSame(null, colSelector.SelectedBusinessObject);
            Assert.AreEqual(0, SelectedIndex(colSelector));
        }
    }

}