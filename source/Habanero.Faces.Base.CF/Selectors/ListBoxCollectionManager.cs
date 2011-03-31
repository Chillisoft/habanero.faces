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
using System;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// This class provides mapping from a business object collection to a
    /// user interface ListBox.  This mapper is used at code level when
    /// you are explicitly providing a business object collection.
    /// This Class is typically used by the <see cref="IBOComboBoxSelector"/> control and
    /// <see cref="ComboBoxMapper"/>.
    /// </summary>
    public class ListBoxCollectionManager
    {
        /// <summary>
        /// Constructor to create a new collection ListBox mapper object.
        /// </summary>
        /// <param name="listBox">The ListBox object to map</param>
        public ListBoxCollectionManager(ListBox listBox)
        {
            if (ReferenceEquals(listBox, null)) throw new ArgumentNullException("listControl");
            Control = listBox;
            _selectedIndexChanged = Control_OnSelectedIndexChanged;
            this.AutoSelectFirstItem = true;
            RegisterForControlEvents();
            /// <summary>
        }

        /// Sets the collection being represented to a specific collection
        /// of business objects
        /// </summary>
        /// <param name="collection">The collection to represent</param>
        public void SetCollection(IBusinessObjectCollection collection)
        {
            //Difference in Functionality where the ListBox and ComboBox are registering for updated events
            //The List Box is registering for the BusinessObjectUpdated Event
            //The ComboBox was registering for the BusinessObjectPropertyUpdated Event
            //When visually testing it turned out that this difference was necessary due to the fact that the ComboBox and ListBox behave different with regards to the
            //way in which they Handle Getting focus.
            // i.e. If the ListBox updates every time the BusinessObjectProperty is Updated then
            //  it gets focus and the user cannot contunue typing.
            if (Collection != null)
            {
                Collection.BusinessObjectAdded -= BusinessObjectAddedHandler;
                Collection.BusinessObjectRemoved -= BusinessObjectRemovedHandler;
                Collection.BusinessObjectUpdated -= BusinessObjectUpdatedHandler;
            }
            Collection = collection;
            SetListBoxCollectionInternal(Control, Collection);
            if (Collection == null) return;
            Collection.BusinessObjectAdded += BusinessObjectAddedHandler;
            Collection.BusinessObjectRemoved += BusinessObjectRemovedHandler;
            Collection.BusinessObjectUpdated += BusinessObjectUpdatedHandler;
        }


        private void BusinessObjectUpdatedHandler(object sender, BOEventArgs boEventArgs)
        {
            var businessObject = boEventArgs.BusinessObject;
            UpdateBusinessObject(businessObject);
        }

        private void SetListBoxCollectionInternal(ListBox lstBox, IBusinessObjectCollection col)
        {
            lstBox.Items.Clear();
            if (col == null) return;

            foreach (IBusinessObject businessObject in col)
            {
                lstBox.Items.Add(businessObject);
            }
            if (col.Count > 0 && this.AutoSelectFirstItem)
            {
                lstBox.SelectedIndex = 0;
                FireBusinessObjectSelected();
            }
        }

        /// <summary>
        /// A handler for the SelectedIndexChanged Event
        /// </summary>
        private EventHandler _selectedIndexChanged;



        /// <summary>
        /// Returns the collection used to populate the items shown in the ListBox
        /// </summary>
        public IBusinessObjectCollection Collection { get; protected set; }


        /// <summary>
        /// Returns the business object, in object form, that is currently 
        /// selected in the ListBox list, or null if none is selected
        /// </summary>
        public IBusinessObject SelectedBusinessObject
        {
            get
            {
                if (NoItemSelected() || NullItemSelected()) return null;

                return Control.SelectedItem as IBusinessObject;
            }
            set
            {
                Control.SelectedItem = ContainsValue(value) ? value : null;
                if (Control.SelectedItem == null) Control.Text = null;
            }
        }

        private bool ContainsValue(IBusinessObject value)
        {
            return (value != null && Control.Items.Contains(value));
        }

        private bool NullItemSelected()
        {
            return (Control.SelectedItem == null || Control.SelectedItem.ToString() == "");
        }

        private bool NoItemSelected()
        {
            return Control.SelectedIndex == -1;
        }

        ///<summary>
        /// Clears all items in the List Box, sets the selected item and <see cref="BusinessObjectCollection"/>
        /// to null
        ///</summary>
        public void Clear()
        {
            SetCollection(null);
        }

        /// <summary>Gets the number of items displayed in the <see cref="IBOColSelector"></see>.</summary>
        /// <returns>The number of items in the <see cref="IBOColSelector"></see>.</returns>
        public int NoOfItems
        {
            get { return Control.Items.Count; }
        }

        /// <summary>
        /// Gets and sets whether this selector autoselects the first item or not when a new collection is set.
        /// </summary>
        public bool AutoSelectFirstItem { get; set; }


        /// <summary>
        /// Gets and sets whether the Control is enabled or not
        /// </summary>
        public bool ControlEnabled
        {
            get { return this.Control.Enabled; }
            set { this.Control.Enabled = value; }
        }

        /// <summary>
        /// Returns the business object at the specified row number
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        public IBusinessObject GetBusinessObjectAtRow(int row)
        {
            if (IndexOutOfRange(row)) return null;
            return (IBusinessObject)Control.Items[row];
        }

        private bool IndexOutOfRange(int row)
        {
            return row < 0 || row >= NoOfItems;
        }

        /// <summary>
        /// Returns the ListBox control
        /// </summary>
        public ListBox Control { get; private set; }

#pragma warning disable 612,618
        public IBusinessObjectCollection BusinessObjectCollection
        {
            get { return this.Collection; }
            set { this.SetCollection(value); }
        }
#pragma warning restore 612,618


        /// <summary>
        /// Event Occurs when a business object is selected
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectSelected;


        /// <summary>
        /// Registers this controller for the <see cref="ComboBox.SelectedIndexChanged"/> event.
        /// </summary>
        public void RegisterForControlEvents()
        {
            Control.SelectedIndexChanged += _selectedIndexChanged;
        }

        /// <summary>
        /// Deregisters this controller for the <see cref="ComboBox.SelectedIndexChanged"/> event.
        /// </summary>
        public void DeregisterForControlEvents()
        {
            Control.SelectedIndexChanged -= _selectedIndexChanged;
        }

        private void Control_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            FireBusinessObjectSelected();
        }


        protected void FireBusinessObjectSelected()
        {
            if (this.BusinessObjectSelected != null)
            {
                this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
            }
        }

        /// <summary>
        /// This handler is called when a business object has been removed from
        /// the collection - it subsequently removes the item from the ListBox
        /// list as well.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        protected void BusinessObjectRemovedHandler(object sender, BOEventArgs e)
        {
            var businessObject = e.BusinessObject;
            var isSelectedItem = this.Control.SelectedItem == businessObject;
            this.Control.Items.Remove(businessObject);
            if (isSelectedItem)
            {
                //Fires the event with business object selected null since the selected bo has been removed
                FireBusinessObjectSelected();
            }
        }


        protected void BusinessObjectPropertyUpdatedHandler(object sender, BOPropUpdatedEventArgs e)
        {
            var businessObject = e.BusinessObject;
            UpdateBusinessObject(businessObject);
        }

        protected void UpdateBusinessObject(IBusinessObject businessObject)
        {
            var selectedBO = this.Control.SelectedItem;
            var indexOf = this.Control.Items.IndexOf(businessObject);
            if (indexOf == -1) return;
            this.Control.Items.Remove(businessObject);
            this.Control.Items.Insert(indexOf, businessObject);
            this.Control.SelectedItem = selectedBO;
        }

        /// <summary>
        /// This handler is called when a business object has been added to
        /// the collection - it subsequently adds the item to the ListBox
        /// list as well.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        protected void BusinessObjectAddedHandler(object sender, BOEventArgs e)
        {
            var businessObject = e.BusinessObject;
            if (businessObject == null) return;
            var items = Control.Items;
            if (items.Contains(businessObject)) return;
            if (string.IsNullOrEmpty(businessObject.ToString()))
            {
                string message =
                    string.Format(
                        "Cannot add a business object of type '{0}' to the '{1}' if its ToString is null or zero length",
                        businessObject.ClassDef.ClassName, this.GetType().Name);
                throw new HabaneroDeveloperException(message, message);
            }
            this.Control.Items.Add(e.BusinessObject);
        }
    }
}