﻿#region Licensing Header
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

namespace Habanero.Faces.Base
{
    /// <summary>
    /// Represents the collection of items in a ListControl
    /// </summary>
    public interface IListControlObjectCollection :IEnumerable
    {
        /// <summary>
        /// Adds an item to the list of items for a ComboBox
        /// </summary>
        /// <param name="item">An object representing the item to add to the collection</param>
        void Add(object item);

        /// <summary>
        /// Gets the number of items in the collection
        /// </summary>
        int Count { get; }

        //        /// <summary>
        //        /// Gets or sets the label to display
        //        /// </summary>
        //        string Label { get; set; }

        /// <summary>
        /// Removes the specified item from the ComboBo
        /// </summary>
        /// <param name="item">The System.Object to remove from the list</param>
        void Remove(object item);

        /// <summary>
        /// Removes all items from the ComboBox
        /// </summary>
        void Clear();

        ///// <summary>
        ///// Populates the collection using the given BusinessObjectCollection
        ///// </summary>
        ///// <param name="collection">A BusinessObjectCollection</param>
        //void SetCollection(IBusinessObjectCollection collection);

        /// <summary>
        /// Retrieves the item at the specified index within the collection
        /// </summary>
        /// <param name="index">The index of the item in the collection to retrieve</param>
        /// <returns>An object representing the item located at the
        /// specified index within the collection</returns>
        object this[int index] { get; set; }

        /// <summary>
        /// Determines if the specified item is located within the collection
        /// </summary>
        /// <param name="value">An object representing the item to locate in the collection</param>
        /// <returns>true if the item is located within the collection; otherwise, false</returns>
        bool Contains(object value);

        /// <summary>
        /// Retrieves the index within the collection of the specified item
        /// </summary>
        /// <param name="value">An object representing the item to locate in the collection</param>
        /// <returns>The zero-based index where the item is
        /// located within the collection; otherwise, -1</returns>
        int IndexOf(object value);
        /// <summary>
        /// Inserts an item into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index location where the item is inserted</param>
        /// <param name="item">An object representing the item to insert.</param>
        /// <exception cref="ArgumentNullException">The item was null</exception>
        /// <exception cref="ArgumentOutOfRangeException">The index was less than zero.-or- The index was greater than the count of items in the collection.</exception>
        void Insert(int index, object item);
    }
}