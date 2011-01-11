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
using System.Collections;
using System.ComponentModel;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// A collection of DataGridViewRow objects
    /// </summary>
    public interface IDataGridViewRowCollection : IEnumerable
    {
        /// <summary>
        /// Gets the number of rows in the collection
        /// </summary>
        int Count { get; }

        /// <summary>Gets the <see cref="IDataGridViewRow"></see> at the specified index.</summary>
        /// <returns>The <see cref="IDataGridViewRow"></see> at the specified index. Accessing
        ///  a <see cref="IDataGridViewRow"></see> with this indexer causes the row to become unshared. 
        /// To keep the row shared, use the SharedRow method. 
        /// For more information, see Best Practices for Scaling the Windows Forms DataGridView Control.</returns>
        /// <param name="index">The zero-based index of the <see cref="IDataGridViewRow"></see> to get.</param>
        /// <filterpriority>1</filterpriority>
        IDataGridViewRow this[int index] { get; }

        /// <summary>Adds a new row to the collection, and populates the cells with the specified objects.</summary>
        /// <returns>The index of the new row.</returns>
        /// <param name="values">A variable number of objects that populate the cells of the
        ///  new <see cref="IDataGridViewRow"></see>.</param>
        /// <exception cref="T:System.InvalidOperationException">The associated <see cref="IDataGridView"></see> 
        /// control is performing one of the following actions that temporarily prevents new rows from 
        /// being added:Selecting all cells in the control.Clearing the selection.-or-This method is 
        /// being called from a handler for one of the following <see cref="IDataGridView"></see>
        ///  events: CellEnter, CellLeave, CellValidating, CellValidated, RowEnter, RowLeave, RowValidated,
        /// RowValidating, -or-The VirtualMode property of the <see cref="IDataGridView"></see> is set to
        ///  true.- or -The <see cref="IDataGridView.DataSource"></see> property of the
        ///  <see cref="IDataGridView"></see> is not null.-or-The <see cref="IDataGridView"></see> has no
        ///  columns. -or-The row returned by the RowTemplate property has more cells than there are columns 
        /// in the control.-or-This operation would add a frozen row after unfrozen rows.</exception>
        /// <exception cref="T:System.ArgumentNullException">values is null.</exception>
        /// <filterpriority>1</filterpriority>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        int Add(params object[] values);


        ///// <summary>Adds a new row to the collection.</summary>
        ///// <returns>The index of the new row.</returns>
        ///// <exception cref="T:System.ArgumentException">The row returned by the <see cref="IDataGridView.RowTemplate"></see> property has more cells than there are columns in the control.</exception>
        ///// <exception cref="T:System.InvalidOperationException">The associated <see cref="IDataGridView"></see> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="IDataGridView"></see> events:<see cref="IDataGridView"></see>.CellEnter<see cref="IDataGridView"></see>.CellLeave<see cref="IDataGridView"></see>.CellValidating<see cref="IDataGridView"></see>.RowValidating<see cref="IDataGridView"></see>.RowEnter<see cref="IDataGridView"></see>.RowLeave<see cref="IDataGridView"></see>.RowValidated<see cref="IDataGridView"></see>.RowValidating-or-The <see cref="IDataGridView.DataSource"></see> property of the <see cref="IDataGridView"></see> is not null.-or-The <see cref="IDataGridView"></see> has no columns.-or-This operation would add a frozen row after unfrozen rows.</exception>
        ///// <filterpriority>1</filterpriority>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //int Add();

        ///// <summary>Adds the specified number of new rows to the collection.</summary>
        ///// <returns>The index of the last row that was added.</returns>
        ///// <param name="count">The number of rows to add to the <see cref="IDataGridViewRowCollection"></see>.</param>
        ///// <exception cref="T:System.ArgumentOutOfRangeException">count is less than 1.</exception>
        ///// <exception cref="T:System.InvalidOperationException">The associated <see cref="IDataGridView"></see> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="IDataGridView"></see> events:<see cref="IDataGridView"></see>.CellEnter<see cref="IDataGridView"></see>.CellLeave<see cref="IDataGridView"></see>.CellValidating<see cref="IDataGridView"></see>.RowValidating<see cref="IDataGridView"></see>.RowEnter<see cref="IDataGridView"></see>.RowLeave<see cref="IDataGridView"></see>.RowValidated<see cref="IDataGridView"></see>.RowValidating-or-The <see cref="IDataGridView.DataSource"></see> property of the <see cref="IDataGridView"></see> is not null.-or-The <see cref="IDataGridView"></see> has no columns.-or-The row returned by the <see cref="IDataGridView.RowTemplate"></see> property has more cells than there are columns in the control. -or-This operation would add frozen rows after unfrozen rows.</exception>
        ///// <filterpriority>1</filterpriority>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //int Add(int count);


        ///// <summary>Adds the specified <see cref="IDataGridViewRow"></see> to the collection.</summary>
        ///// <returns>The index of the new <see cref="IDataGridViewRow"></see>.</returns>
        ///// <param name="dataGridViewRow">The <see cref="IDataGridViewRow"></see> to add to the <see cref="IDataGridViewRowCollection"></see>.</param>
        ///// <exception cref="T:System.ArgumentException">dataGridViewRow has more cells than there are columns in the control.</exception>
        ///// <exception cref="T:System.InvalidOperationException">The associated <see cref="IDataGridView"></see> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="IDataGridView"></see> events:<see cref="IDataGridView"></see>.CellEnter<see cref="IDataGridView"></see>.CellLeave<see cref="IDataGridView"></see>.CellValidating<see cref="IDataGridView"></see>.RowValidating<see cref="IDataGridView"></see>.RowEnter<see cref="IDataGridView"></see>.RowLeave<see cref="IDataGridView"></see>.RowValidated<see cref="IDataGridView"></see>.RowValidating-or-The <see cref="IDataGridView.DataSource"></see> property of the <see cref="IDataGridView"></see> is not null.-or-The <see cref="IDataGridView"></see> has no columns.-or-The <see cref="IDataGridViewElement.DataGridView"></see> property of the dataGridViewRow is not null.-or-dataGridViewRow has a <see cref="IDataGridViewRow.Selected"></see> property value of true. -or-This operation would add a frozen row after unfrozen rows.</exception>
        ///// <exception cref="T:System.ArgumentNullException">dataGridViewRow is null.</exception>
        ///// <filterpriority>1</filterpriority>
        //int Add(IDataGridViewRow dataGridViewRow);

        /// <summary>Clears the collection. </summary>
        /// <exception cref="T:System.InvalidOperationException">The collection is data bound and the underlying data source does not support clearing the row data.-or-The associated <see cref="IDataGridView"></see> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="IDataGridView"></see> events:<see cref="IDataGridView"></see>.CellEnter<see cref="IDataGridView"></see>.CellLeave<see cref="IDataGridView"></see>.CellValidating<see cref="IDataGridView"></see>.RowValidating<see cref="IDataGridView"></see>.RowEnter<see cref="IDataGridView"></see>.RowLeave<see cref="IDataGridView"></see>.RowValidated<see cref="IDataGridView"></see>.RowValidating</exception>
        /// <filterpriority>1</filterpriority>
        void Clear();

        ///// <summary>Determines whether the specified <see cref="IDataGridViewRow"></see> is in the collection.</summary>
        ///// <returns>true if the <see cref="IDataGridViewRow"></see> is in the <see cref="IDataGridViewRowCollection"></see>; otherwise, false.</returns>
        ///// <param name="dataGridViewRow">The <see cref="IDataGridViewRow"></see> to locate in the <see cref="IDataGridViewRowCollection"></see>.</param>
        ///// <filterpriority>1</filterpriority>
        //bool Contains(IDataGridViewRow dataGridViewRow);

        ///// <summary>Returns the index of a specified item in the collection.</summary>
        ///// <returns>The index of value if it is a <see cref="IDataGridViewRow"></see> found in the <see cref="IDataGridViewRowCollection"></see>; otherwise, -1.</returns>
        ///// <param name="dataGridViewRow">The <see cref="IDataGridViewRow"></see> to locate in the <see cref="IDataGridViewRowCollection"></see>.</param>
        ///// <filterpriority>1</filterpriority>
        //int IndexOf(IDataGridViewRow dataGridViewRow);

        ///// <summary>Inserts the specified number of rows into the collection at the specified location.</summary>
        ///// <param name="count">The number of rows to insert into the <see cref="IDataGridViewRowCollection"></see>.</param>
        ///// <param name="rowIndex">The position at which to insert the rows.</param>
        ///// <exception cref="T:System.InvalidOperationException">The associated <see cref="IDataGridView"></see> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="IDataGridView"></see> events:<see cref="IDataGridView"></see>.CellEnter<see cref="IDataGridView"></see>.CellLeave<see cref="IDataGridView"></see>.CellValidating<see cref="IDataGridView"></see>.RowValidating<see cref="IDataGridView"></see>.RowEnter<see cref="IDataGridView"></see>.RowLeave<see cref="IDataGridView"></see>.RowValidated<see cref="IDataGridView"></see>.RowValidating-or-The <see cref="IDataGridView.DataSource"></see> property of the <see cref="IDataGridView"></see> is not null.-or-The <see cref="IDataGridView"></see> has no columns.-or-rowIndex is equal to the number of rows in the collection and the <see cref="IDataGridView.AllowUserToAddRows"></see> property of the <see cref="IDataGridView"></see> is set to true.-or-The row returned by the <see cref="IDataGridView.RowTemplate"></see> property has more cells than there are columns in the control. -or-This operation would insert a frozen row after unfrozen rows or an unfrozen row before frozen rows.</exception>
        ///// <exception cref="T:System.ArgumentOutOfRangeException">rowIndex is less than zero or greater than the number of rows in the collection. -or-count is less than 1.</exception>
        ///// <filterpriority>1</filterpriority>
        //void Insert(int rowIndex, int count);

        ///// <summary>Inserts the specified <see cref="IDataGridViewRow"></see> into the collection.</summary>
        ///// <param name="dataGridViewRow">The <see cref="IDataGridViewRow"></see> to insert into the <see cref="IDataGridViewRowCollection"></see>.</param>
        ///// <param name="rowIndex">The position at which to insert the row.</param>
        ///// <exception cref="T:System.ArgumentOutOfRangeException">rowIndex is less than zero or greater than the number of rows in the collection. </exception>
        ///// <exception cref="T:System.ArgumentException">dataGridViewRow has more cells than there are columns in the control.</exception>
        ///// <exception cref="T:System.InvalidOperationException">The associated <see cref="IDataGridView"></see> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="IDataGridView"></see> events:<see cref="IDataGridView"></see>.CellEnter<see cref="IDataGridView"></see>.CellLeave<see cref="IDataGridView"></see>.CellValidating<see cref="IDataGridView"></see>.RowValidating<see cref="IDataGridView"></see>.RowEnter<see cref="IDataGridView"></see>.RowLeave<see cref="IDataGridView"></see>.RowValidated<see cref="IDataGridView"></see>.RowValidating-or-The <see cref="IDataGridView.DataSource"></see> property of the <see cref="IDataGridView"></see> is not null.-or-rowIndex is equal to the number of rows in the collection and the <see cref="IDataGridView.AllowUserToAddRows"></see> property of the <see cref="IDataGridView"></see> is set to true.-or-The <see cref="IDataGridView"></see> has no columns.-or-The <see cref="IDataGridViewElement.DataGridView"></see> property of dataGridViewRow is not null.-or-dataGridViewRow has a <see cref="IDataGridViewRow.Selected"></see> property value of true. -or-This operation would insert a frozen row after unfrozen rows or an unfrozen row before frozen rows.</exception>
        ///// <exception cref="T:System.ArgumentNullException">dataGridViewRow is null.</exception>
        ///// <filterpriority>1</filterpriority>
        //void Insert(int rowIndex, IDataGridViewRow dataGridViewRow);

        ///// <summary>Inserts a row into the collection at the specified position, and populates the cells with the specified objects.</summary>
        ///// <param name="rowIndex">The position at which to insert the row.</param>
        ///// <param name="values">A variable number of objects that populate the cells of the new row.</param>
        ///// <exception cref="T:System.ArgumentOutOfRangeException">rowIndex is less than zero or greater than the number of rows in the collection. </exception>
        ///// <exception cref="T:System.ArgumentException">The row returned by the control's <see cref="IDataGridView.RowTemplate"></see> property has more cells than there are columns in the control.</exception>
        ///// <exception cref="T:System.InvalidOperationException">The associated <see cref="IDataGridView"></see> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="IDataGridView"></see> events:<see cref="IDataGridView"></see>.CellEnter<see cref="IDataGridView"></see>.CellLeave<see cref="IDataGridView"></see>.CellValidating<see cref="IDataGridView"></see>.RowValidating<see cref="IDataGridView"></see>.RowEnter<see cref="IDataGridView"></see>.RowLeave<see cref="IDataGridView"></see>.RowValidated<see cref="IDataGridView"></see>.RowValidating-or-The <see cref="IDataGridView.VirtualMode"></see> property of the <see cref="IDataGridView"></see> is set to true.-or-The <see cref="IDataGridView.DataSource"></see> property of the <see cref="IDataGridView"></see> is not null.-or-The <see cref="IDataGridView"></see> has no columns.-or-rowIndex is equal to the number of rows in the collection and the <see cref="IDataGridView.AllowUserToAddRows"></see> property of the <see cref="IDataGridView"></see> is set to true.-or-The <see cref="IDataGridViewElement.DataGridView"></see> property of the row returned by the control's <see cref="IDataGridView.RowTemplate"></see> property is not null. -or-This operation would insert a frozen row after unfrozen rows or an unfrozen row before frozen rows.</exception>
        ///// <exception cref="T:System.ArgumentNullException">values is null.</exception>
        ///// <filterpriority>1</filterpriority>
        //void Insert(int rowIndex, params object[] values);

        /// <summary>Removes the row from the collection.</summary>
        /// <param name="dataGridViewRow">The row to remove from the <see cref="IDataGridViewRowCollection"></see>.</param>
        /// <exception cref="T:System.InvalidOperationException">The associated <see cref="IDataGridView"></see> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="IDataGridView"></see> events:<see cref="IDataGridView"></see>.CellEnter<see cref="IDataGridView"></see>.CellLeave<see cref="IDataGridView"></see>.CellValidating<see cref="IDataGridView"></see>.RowValidating<see cref="IDataGridView"></see>.RowEnter<see cref="IDataGridView"></see>.RowLeave<see cref="IDataGridView"></see>.RowValidated<see cref="IDataGridView"></see>.RowValidating-or-dataGridViewRow is the row for new records.-or-The associated <see cref="IDataGridView"></see> control is bound to an <see cref="T:System.ComponentModel.IBindingList"></see> implementation with <see cref="P:System.ComponentModel.IBindingList.AllowRemove"></see> and <see cref="P:System.ComponentModel.IBindingList.SupportsChangeNotification"></see> property values that are not both true. </exception>
        /// <exception cref="T:System.ArgumentException">dataGridViewRow is not contained in this collection.-or-dataGridViewRow is a shared row.</exception>
        /// <exception cref="T:System.ArgumentNullException">dataGridViewRow is null.</exception>
        /// <filterpriority>1</filterpriority>
        void Remove(IDataGridViewRow dataGridViewRow);

        /// <summary>Removes the row at the specified position from the collection.</summary>
        /// <param name="index">The position of the row to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero and greater than the number of rows in the collection minus one. </exception>
        /// <exception cref="T:System.InvalidOperationException">The associated <see cref="IDataGridView"></see> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="IDataGridView"></see> events:<see cref="IDataGridView"></see>.CellEnter<see cref="IDataGridView"></see>.CellLeave<see cref="IDataGridView"></see>.CellValidating<see cref="IDataGridView"></see>.RowValidating<see cref="IDataGridView"></see>.RowEnter<see cref="IDataGridView"></see>.RowLeave<see cref="IDataGridView"></see>.RowValidated<see cref="IDataGridView"></see>.RowValidating-or-index is equal to the number of rows in the collection and the <see cref="IDataGridView.AllowUserToAddRows"></see> property of the <see cref="IDataGridView"></see> is set to true.-or-The associated <see cref="IDataGridView"></see> control is bound to an <see cref="T:System.ComponentModel.IBindingList"></see> implementation with <see cref="P:System.ComponentModel.IBindingList.AllowRemove"></see> and <see cref="P:System.ComponentModel.IBindingList.SupportsChangeNotification"></see> property values that are not both true.</exception>
        /// <filterpriority>1</filterpriority>
        void RemoveAt(int index);

        /// <summary>
        /// Returns the index of a specified item in the collection
        /// </summary>
        /// <param name="dataGridViewRow">The DataGridViewRow to locate in the DataGridViewRowCollection</param>
        /// <returns>The index of value if it is a DataGridViewRow found in the DataGridViewRowCollection; otherwise, -1.</returns>
        int IndexOf(IDataGridViewRow dataGridViewRow);
    }
}
