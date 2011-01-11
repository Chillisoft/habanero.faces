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
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.Faces.Win
{
    /// <summary>
    /// Maps a ListView object in a user interface
    /// </summary>
    public class ListViewCollectionManager
    {
        /// <summary>
        /// Construct the Manager with a ListView.
        /// </summary>
        /// <param name="listView"></param>
        public ListViewCollectionManager(ListView listView)
        {
            ListView = listView;
            _listItemsHash = new Hashtable();
            listView.MultiSelect = true;
        }
        /// <summary>
        /// The List view being manged by this Manager.
        /// </summary>
        public ListView ListView { get; private set; }
        private readonly Hashtable _listItemsHash;

        /// <summary>
        /// Returns the currently selected business object in the ListView
        /// or null if none is selected
        /// </summary>
        public IBusinessObject SelectedBusinessObject
        {
            get
            {
                if (ListView.SelectedItems.Count >= 1)
                {
                    return (BusinessObject)ListView.SelectedItems[0].Tag;
                }
                return null;
            }
        }

        /// <summary>
        /// Returns the collection used to populate the items shown in the ListBox
        /// </summary>
        public IBusinessObjectCollection Collection { get; protected set; }
        /// <summary>
        /// Specify the collection of objects to display and add these to the
        /// ListView object
        /// </summary>
        /// <param name="collection">The collection of business objects</param>
        public void SetCollection(IBusinessObjectCollection collection)
        {

            if (Collection != null)
            {
                Collection.BusinessObjectAdded -= BusinessObjectAddedHandler;
                Collection.BusinessObjectRemoved -= BusinessObjectRemovedHandler;
                Collection.BusinessObjectUpdated -= BusinessObjectUpdatedHandler;
            }
            Collection = collection;
            SetListViewCollection(ListView, Collection);
            Collection.BusinessObjectAdded += BusinessObjectAddedHandler;
            Collection.BusinessObjectRemoved += BusinessObjectRemovedHandler;
            Collection.BusinessObjectUpdated += BusinessObjectUpdatedHandler;
        }

        private void BusinessObjectUpdatedHandler(object sender, BOEventArgs e)
        {
            var businessObject = e.BusinessObject;
            /*            var boToBeUpdated = this.ListView.Items.Cast<ListViewItem>().FirstOrDefault(item => item.Tag == businessObject);
                        */
            var listItemToBeUpdated = _listItemsHash[businessObject] as ListViewItem;
            if (listItemToBeUpdated == null) return;
            listItemToBeUpdated.Text = businessObject.ToString();
        }

        /// <summary>
        /// A handler that updates the display when a business object has been
        /// removed from the collection
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectRemovedHandler(object sender, BOEventArgs e)
        {
            var businessObject = e.BusinessObject;
            var itemToBeRemoved = _listItemsHash[businessObject] as ListViewItem;
            if (itemToBeRemoved == null) return;
            ListView.Items.Remove(itemToBeRemoved);
        }

        /// <summary>
        /// A handler that updates the display when a business object has been
        /// added to the collection
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectAddedHandler(object sender, BOEventArgs e)
        {
            ListView.Items.Add(CreateListViewItem(e.BusinessObject));
        }

        /// <summary>
        /// Creates a ListViewItem from the business object provided.  This
        /// method is used by SetListViewCollection() to populate the ListView.
        /// </summary>
        /// <param name="bo">The business object to represent</param>
        /// <returns>Returns a new ListViewItem</returns>
        private ListViewItem CreateListViewItem(IBusinessObject bo)
        {
            var listViewItem = new ListViewItem(bo.ToString()) {Tag = bo};
            _listItemsHash.Add(bo, listViewItem);
            return listViewItem;
        }

        /// <summary>
        /// Adds the business objects in the collection to the ListView. This
        /// method is used by SetBusinessObjectCollection.
        /// </summary>
        /// <param name="listView">The ListView object to add to</param>
        /// <param name="collection">The business object collection</param>
        private void SetListViewCollection(ListView listView, IBusinessObjectCollection collection)
        {
            Clear(listView);

            foreach (IBusinessObject bo in collection)
            {
                listView.Items.Add(CreateListViewItem(bo));

            }
        }

        private void Clear(ListView listView)
        {
            listView.Clear();
            _listItemsHash.Clear();
        }
    }
}