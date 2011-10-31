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
using Habanero.Base;
using Habanero.BO;
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    public abstract class TestBOSelectorGrid : TestBOColSelector
    {
        private const string _gridIdColumnName = "HABANERO_OBJECTID";

        protected override void SetSelectedIndex(IBOColSelectorControl colSelector, int index)
        {
            int count = 0;
            foreach (IDataGridViewRow row in ((IGridBase) colSelector).Rows)
            {
                if (count == index)
                {
                    IBusinessObject businessObjectAtRow = ((IGridBase) colSelector).GetBusinessObjectAtRow(count);
                    colSelector.SelectedBusinessObject = businessObjectAtRow;
                }
                count++;
            }
        }

        protected override int SelectedIndex(IBOColSelectorControl colSelector)
        {
            IGridBase gridSelector = (IGridBase) colSelector;
            IDataGridViewRow currentRow = null;
            if (gridSelector.SelectedRows.Count > 0)
            {
                currentRow = gridSelector.SelectedRows[0];
            }
            if (currentRow == null) return -1;

            return gridSelector.Rows.IndexOf(currentRow);
        }

        protected static void SetupGridColumnsForMyBo(IDataGridView gridBase)
        {
            gridBase.Columns.Add(_gridIdColumnName, _gridIdColumnName);
            gridBase.Columns.Add("TestProp", "TestProp");
        }

        protected override int NumberOfLeadingBlankRows()
        {
            return 0;
        }

        protected override int NumberOfTrailingBlankRows()
        {
            return 0;
        }

        [Test]
        public void Test_Constructor_GridSet()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOColSelectorControl colSelector = CreateSelector();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(IGridBase), colSelector);
        }

        [Ignore(" Not sure how to impolement this in grids.")] //TODO  01 Mar 2009:
        [Test]
        public override void Test_Set_SelectedBusinessObject_ItemNotInList_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, myBO2 };
            colSelector.BusinessObjectCollection = collection;
            SetSelectedIndex(colSelector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(colSelector));
            Assert.AreEqual(myBO2, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.SelectedBusinessObject = new MyBO();
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualIndex(2), colSelector.NoOfItems, "The blank item");
            Assert.IsNull(colSelector.SelectedBusinessObject);
            Assert.AreEqual(-1, SelectedIndex(colSelector));
        }

        [Ignore(" Not sure how to implement this in grids")] //TODO  01 Mar 2009:
        [Test]
        public override void Test_SetBOCollection_WhenAutoSelectsFirstItem_ShouldSelectFirstItem()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, myBO2 };
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, colSelector.NoOfItems);
            Assert.AreEqual(-1, SelectedIndex(colSelector));
            Assert.AreEqual(null, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item");
            Assert.AreSame(myBO, colSelector.SelectedBusinessObject);
            Assert.AreEqual(ActualIndex(0), SelectedIndex(colSelector));
        }

        [Ignore(" Not Yet implemented")] //TODO  01 Mar 2009:
        [Test]
        public void TestEditItemFromCollectionUpdatesItemInSelector()
        {
        }
    }

    
}