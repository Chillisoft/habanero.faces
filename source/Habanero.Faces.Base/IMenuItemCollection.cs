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
using System.Collections;
using System.Collections.Generic;

namespace Habanero.Faces.Base
{
    ///<summary>
    /// This represents 
    /// A Collection of <see cref="IMenuItem"/>s. A <see cref="IMenuItem"/> is an item that 
    /// is shown in a Menu Control.
    /// <remarks>
    /// This Inteface is an extract of common functionality required for the IMenuItemCollection and is used to 
    /// isolate the implementation of the actual menu from the menu code using the menu.
    /// This allows the developer to swap menu's that support this interface without having to redevelop 
    /// any menu code.
    /// Habanero uses this to isolate the UIframework so that a different framework can be implemented
    /// using these interfaces.
    /// This allows the Architecture to swap between Visual Web Gui and Windows or in fact between any UI framework and
    /// any other UI Framework.
    /// </remarks>
    ///</summary>
    public interface IMenuItemCollection: IEnumerable<IMenuItem>, IEnumerable
    {
        /// <summary>
        /// Adds a Menu item to the <see cref="T:Habanero.Faces.Base.IMenuItemCollection" />.
        /// </summary>
        /// <param name="menuItem"></param>
        void Add(IMenuItem menuItem);

        /// <summary>
        ///The number of <see cref="IMenuItem"/>s in this <see cref="IMenuItemCollection"/>
        /// </summary>
        int Count { get; }

        /// <summary>
        /// The Menu Item that owns this colleciton of Menu Items.
        /// </summary>
        IMenuItem OwnerMenuItem { get; }

        /// <summary>
        /// Returns the Actual Menu item identified by the index.
        /// </summary>
        /// <param name="index"></param>
        IMenuItem this[int index] { get; }
    }
}