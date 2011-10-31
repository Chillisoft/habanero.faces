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
using System.ComponentModel;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// Represents a menu item displayed in the Menu control. This class cannot be inherited.
    ///  A Menu control is made up of a hierarchy of menu items represented by MenuItem objects. 
    /// Menu items at the top level (level 0) that do not have a
    ///  parent menu item are called root menu items. A menu item that has a parent menu item is 
    /// called a submenu item. All root menu items are stored in the Items collection. Submenu 
    /// items are stored in a parent menu item's <see cref="MenuItems"/> collection.
    /// <remarks>
    /// This Inteface is an extract of common functionality required for menu item and is used to 
    /// isolate the implementation of the actual menu from the menu code using the menu.
    /// This allows the developer to swap menu's that support this interface without having to redevelop 
    /// any menu code.
    /// Habanero uses this to isolate the UIframework so that a different framework can be implemented
    /// using these interfaces.
    /// This allows the Architecture to swap between Visual Web Gui and Windows or in fact between any UI framework and
    /// any other UI Framework.
    /// </remarks>
    /// </summary>
    public interface IMenuItem
    {
        ///<summary>
        /// The text displayed for this <see cref="IMenuItem"/>.
        ///</summary>
        string Text { get; }
        /// <summary>
        /// Gets or sets a value indicating whether the menu item is enabled.
        /// </summary>
        bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the menu item is visible.
        /// </summary>
        bool Visible { get; set; }
        /// <summary>
        /// The Child Menu items for this <see cref="IMenuItem"/>.
        /// </summary>
        [Browsable(false)]
        IMenuItemCollection MenuItems { get; }
        /// <summary>
        /// Performs the Click event for this <see cref="IMenuItem"/>.
        /// </summary>
        [Browsable(false)]
        void PerformClick();

        /// <summary>
        /// Occurs when the MenuItem is Clicked
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        event EventHandler Click;

        /// <summary>
        /// This actually executes the Code when PerformClick is selected <see cref="IMenuItem"/>.
        /// </summary>
        [Browsable(false)]
        void DoClick();
    }
}