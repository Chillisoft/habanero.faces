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
using System.Collections.Generic;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// Represents the collection of selected items in the ListBox
    /// </summary>
    public interface ISelectedObjectCollection : IEnumerable
    {
        /// <summary>
        /// Adds an item to the list of selected items for a ListBox
        /// </summary>
        /// <param name="item">An object representing the item to add
        /// to the collection of selected items</param>
        void Add(object item);

        /// <summary>
        /// Determines whether the specified item is located within the collection.
        /// </summary>
        /// <param name="selectedObject">An object representing the item to locate in the collection.</param>
        /// <returns>true if the specified item is located in the collection; otherwise, false.</returns>
        bool Contains(object selectedObject);
    }

    public class NullSelectedObjectCollection : ISelectedObjectCollection
    {
        private IList list = new List<object>();
        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public void Add(object item)
        {
            //Do nothing
        }

        public bool Contains(object selectedObject)
        {
            return false;
        }
    }
}