using System;
using System.Collections;
using System.Windows.Forms;
using Habanero.Faces.Base;

namespace Habanero.Faces.CF.Adapters
{
    /// <summary>
    /// This is a ControlWraper for Any Control that Inherits from System.Windows.Forms.Control
    /// It wraps this Control behind a standard interface that allows any Control in a Windows Environment 
    /// to take advantage of the Habanero ControlMappers <see cref="IControlMapper"/>
    /// </summary>
    public class WinFormsComboBoxAdapter : WinFormsControlAdapter, IWinFormsComboBoxAdapter
    {
        private readonly ComboBox _cmb;

        public WinFormsComboBoxAdapter(ComboBox control)
            : base(control)
        {
            _cmb = control;
            _cmb.SelectedIndexChanged += RaiseSelectedIndexChanged;
            _cmb.SelectedValueChanged += RaiseSelectedValueChanged;
        }

        private void RaiseSelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndexChanged != null) SelectedIndexChanged(sender, e);
        }

        private void RaiseSelectedValueChanged(object sender, EventArgs e)
        {
            if (SelectedValueChanged != null) SelectedValueChanged(sender, e);
        }

        public event EventHandler SelectedValueChanged;

        public ISelectedObjectCollection SelectedItems
        {
            get { return new NullSelectedObjectCollection(); }
        }

        public event EventHandler SelectedIndexChanged;

        public string GetItemText(object item)
        {
            return _cmb.GetItemText(item);
        }

        public string DisplayMember
        {
            get { return _cmb.DisplayMember; }
            set { _cmb.DisplayMember = value; }
        }

        public int SelectedIndex
        {
            get { return _cmb.SelectedIndex; }
            set { _cmb.SelectedIndex = value; }
        }


        public object SelectedValue
        {
            get { return _cmb.SelectedValue; }
            set { _cmb.SelectedValue = value; }
        }

        public string ValueMember
        {
            get { return _cmb.ValueMember; }
            set { _cmb.ValueMember = value; }
        }

        public IListControlObjectCollection Items
        {
            get
            {
                // The two collections get out of synch without a re-instantiation
                return new ComboBoxObjectCollectionCF(_cmb.Items);
            }
        }

        public object DataSource
        {
            get { return _cmb.DataSource; }
            set { _cmb.DataSource = value; }
        }

        public object SelectedItem
        {
            get { return _cmb.SelectedItem; }
            set { _cmb.SelectedItem = value; }
        }

        public int DropDownWidth { get; set; }

        public AutoCompleteMode AutoCompleteMode { get; set; }

        public AutoCompleteSource AutoCompleteSource { get; set; }

/*        public int DropDownWidth
        {
            get { return _cmb.DropDownWidth; }
            set { _cmb.DropDownWidth = value; }
        }*/

/*
        public AutoCompleteMode AutoCompleteMode
        {
            get { return ComboBoxAutoCompleteModeWin.GetAutoCompleteMode(_cmb.AutoCompleteMode); }
            set { _cmb.AutoCompleteMode = ComboBoxAutoCompleteModeWin.GetAutoCompleteMode(value); }
        }

        public AutoCompleteSource AutoCompleteSource
        {
            get { return ComboBoxAutoCompleteSourceWin.GetAutoCompleteSource(_cmb.AutoCompleteSource); }
            set { _cmb.AutoCompleteSource = ComboBoxAutoCompleteSourceWin.GetAutoCompleteSource(value); }
        }*/
    }


    /// <summary>
    /// Represents the collection of items in a ComboBox
    /// </summary>
    public class ComboBoxObjectCollectionCF : IComboBoxObjectCollection
    {
        private readonly ComboBox.ObjectCollection _items;

        /// <summary>
        /// Construct this wrapper collection
        /// </summary>
        /// <param name="items"></param>
        public ComboBoxObjectCollectionCF(ComboBox.ObjectCollection items)
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
}