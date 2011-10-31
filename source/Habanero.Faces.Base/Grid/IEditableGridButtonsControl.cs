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

namespace Habanero.Faces.Base
{
    /// <summary>
    /// Provides a set of buttons for use on an <see cref="IEditableGridControl"/>.
    /// By default, Save and Cancel buttons are available.
    /// </summary>
    public interface IEditableGridButtonsControl : IButtonGroupControl
    {
        /// <summary>
        /// Fires when the Save button is clicked
        /// </summary>
        event EventHandler SaveClicked;

        /// <summary>
        /// Fires when the Cancel button is clicked
        /// </summary>
        event EventHandler CancelClicked;
    }
}
