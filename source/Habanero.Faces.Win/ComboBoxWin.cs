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
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.Faces.Base;

namespace Habanero.Faces.Win
{
    /// <summary>
    /// Represents a ComboBox control
    /// </summary>
    public class ComboBoxWin : ComboBox, IComboBox
    {
        private readonly ComboBoxManager _manager;
        //private IComboBoxObjectCollection _objectCollection;

        ///<summary>
        /// Constructor for ComboBoxWin
        ///</summary>
        public ComboBoxWin()
        {
            _manager = new ComboBoxManager(this);
          //  _objectCollection = new ComboBoxObjectCollectionWin(base.Items);
        }

        /// <summary>
        /// Gets or sets the anchoring style.
        /// </summary>
        /// <value></value>
        Base.AnchorStyles IControlHabanero.Anchor
        {
            get { return (Base.AnchorStyles) base.Anchor; }
            set { base.Anchor = (System.Windows.Forms.AnchorStyles) value; }
        }

        /// <summary>
        /// Gets an object representing the collection of the items
        /// contained in this ComboBox
        /// </summary>
        public new IListControlObjectCollection Items
        {
            get
            {
                // The two collections get out of synch without a re-instantiation ever time that
                // Items is called. Since this is a simple wrapper there is very little
                // performance disadvantage.
                return new ComboBoxObjectCollectionWin(base.Items);
            }
        }

        /// <summary>
        /// Gets or sets currently selected item in the ComboBox
        /// </summary>
        object IListControl.SelectedItem
        {
            get { return _manager.GetSelectedItem(this.SelectedItem); }
            set { this.SelectedItem = _manager.GetItemToSelect(value); }
        }

        public ISelectedObjectCollection SelectedItems
        {
            get { return new NullSelectedObjectCollection(); }
        }

        /// <summary>
        /// Gets or sets the value of the member property specified by
        /// the ValueMember property
        /// </summary>
        object IListControl.SelectedValue
        {
            get { return _manager.GetSelectedValue(this.SelectedItem); }
            set { SelectedItem = _manager.GetValueToSelect(value); }
        }

        Base.AutoCompleteMode IComboBox.AutoCompleteMode
        {
            get { return ComboBoxAutoCompleteModeWin.GetAutoCompleteMode(base.AutoCompleteMode); }
            set { base.AutoCompleteMode = ComboBoxAutoCompleteModeWin.GetAutoCompleteMode(value); }
        }

        Base.AutoCompleteSource IComboBox.AutoCompleteSource
        {
            get { return ComboBoxAutoCompleteSourceWin.GetAutoCompleteSource(base.AutoCompleteSource); }
            set { base.AutoCompleteSource = ComboBoxAutoCompleteSourceWin.GetAutoCompleteSource(value); }
        }

        /// <summary>
        /// Represents the collection of items in a ComboBox
        /// </summary>
        public class ComboBoxObjectCollectionWin : IComboBoxObjectCollection
        {
            private readonly ObjectCollection _items;
            /// <summary>
            /// Construct this wrapper collection
            /// </summary>
            /// <param name="items"></param>
            public ComboBoxObjectCollectionWin(ObjectCollection items)
            {
                this._items = items;
            }

            /// <summary>
            /// Adds an item to the list of items for a ComboBox
            /// </summary>
            /// <param name="item">An object representing the item to add to the collection</param>
            public void Add(object item)
            {
                _items.Add(item);
            }

            /// <summary>
            /// Gets the number of items in the collection
            /// </summary>
            public int Count
            {
                get { return _items.Count; }
            }
//
//            /// <summary>
//            /// Gets or sets the label to display
//            /// </summary>
//            public string Label
//            {
//                get {return ""; }
//                set { throw new System.NotImplementedException(); }
//            }

            /// <summary>
            /// Removes the specified item from the ComboBox
            /// </summary>
            /// <param name="item">The System.Object to remove from the list</param>
            public void Remove(object item)
            {
                _items.Remove(item);
            }

            /// <summary>
            /// Removes all items from the ComboBox
            /// </summary>
            public void Clear()
            {
                _items.Clear();
            }

            ///// <summary>
            ///// Populates the collection using the given BusinessObjectCollection
            ///// </summary>
            ///// <param name="collection">A BusinessObjectCollection</param>
            //public void SetCollection(IBusinessObjectCollection collection)
            //{
            //    throw new System.NotImplementedException();
            //}

            /// <summary>
            /// Retrieves the item at the specified index within the collection
            /// </summary>
            /// <param name="index">The index of the item in the collection to retrieve</param>
            /// <returns>An object representing the item located at the
            /// specified index within the collection</returns>
            public object this[int index]
            {
                get { return _items[index]; }
                set { _items[index] = value; }
            }

            /// <summary>
            /// Determines if the specified item is located within the collection
            /// </summary>
            /// <param name="value">An object representing the item to locate in the collection</param>
            /// <returns>true if the item is located within the collection; otherwise, false</returns>
            public bool Contains(object value)
            {
                return _items.Contains(value);
            }

            /// <summary>
            /// Retrieves the index within the collection of the specified item
            /// </summary>
            /// <param name="value">An object representing the item to locate in the collection</param>
            /// <returns>The zero-based index where the item is
            /// located within the collection; otherwise, -1</returns>
            public int IndexOf(object value)
            {
                return _items.IndexOf(value);
            }

            /// <summary>
            /// Inserts an item into the collection at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index location where the item is inserted</param>
            /// <param name="item">An object representing the item to insert.</param>
            /// <exception cref="ArgumentNullException">The item was null</exception>
            /// <exception cref="ArgumentOutOfRangeException">The index was less than zero.-or- The index was greater than the count of items in the collection.</exception>
            public void Insert(int index, object item)
            {
                _items.Insert(index, item);
            }

            ///<summary>
            ///Returns an enumerator that iterates through a collection.
            ///</summary>
            ///<returns>
            ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
            ///</returns>
            ///<filterpriority>2</filterpriority>
            public IEnumerator GetEnumerator()
            {
                return _items.GetEnumerator();
            }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlHabanero.Dock
        {
            get { return DockStyleWin.GetDockStyle(base.Dock); }
            set { base.Dock = DockStyleWin.GetDockStyle(value); }
        }
    }

}