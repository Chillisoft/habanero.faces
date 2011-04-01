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
using Habanero.Base;
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
    public class ListBoxCollectionManager : ListControlCollectionManager<IListBox>
    {
        /// <summary>
        /// Constructor to create a new collection ListBox mapper object.
        /// </summary>
        /// <param name="listBox">The ListBox object to map</param>
        public ListBoxCollectionManager(IListBox listBox) : base(listBox)
        {
        }

        /// <summary>
        /// Sets the collection being represented to a specific collection
        /// of business objects
        /// </summary>
        /// <param name="collection">The collection to represent</param>
        public override void SetCollection(IBusinessObjectCollection collection)
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

        private void SetListBoxCollectionInternal(IListBox lstBox, IBusinessObjectCollection col)
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
    }
}