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
using Habanero.Base;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// Provides a common interface that is specialised for showing a collection of 
    /// Business Objects and allowing the user to select and edit one.
    /// The common controls used for selecting business objects are ComboBox, ListBox, ListView, Grid,
    ///  <see cref="ICollapsiblePanelGroupControl"/>, <see cref="IBOColTabControl"/>, a <see cref="IMultiSelector{T}"/>
    ///  or an <see cref="ITreeView"/>.
    /// E.g. For an <see cref="IEditableGridControl"/> the business objects can be edited directly in the grid. In the case of 
    ///    - The <see cref="IReadOnlyGridControl"/> the business objects are edited via pop up forms that open when appropriate.
    ///    - The <see cref="IBOGridAndEditorControl"/> has a grid selector with the functionality to edit the business obects to the right.
    /// This interface inherits from the <see cref="IBOColSelectorControl"/> and adds three additional methods.
    /// <li><see cref="AllowUsersToAddBO"/></li>"
    /// <li><see cref="AllowUsersToDeleteBO"/></li>"
    /// <li><see cref="AllowUsersToEditBO"/></li>"
    /// </summary>
    /// <remarks>
    /// Should possibly allow the user to set an adding, editing or deleting delegate
    /// </remarks>
    public interface IBOSelectorAndEditor : IBOColSelectorControl
    {
        ///<summary>
        /// Gets and sets whether the user can add Business objects via this control
        ///</summary>
        bool AllowUsersToAddBO { get; set; }

        /// <summary>
        /// Gets and sets whether the user can Delete (<see cref="IBusinessObject.MarkForDelete"/>) <see cref="IBusinessObject"/>s via this control
        /// </summary>
        bool AllowUsersToDeleteBO { get; set; }

        /// <summary>
        /// Gets and sets whether the user can edit <see cref="IBusinessObject"/>s via this control
        /// </summary>
        bool AllowUsersToEditBO { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that determines whether to confirm
        /// deletion with the user when they have chosen to delete a row/ businessObject
        /// </summary>
        bool ConfirmDeletion { get; set; }

        /// <summary>
        /// Gets or sets the delegate that checks whether the user wants to delete selected rows.
        /// If <see cref="ConfirmDeletion"/> is true and no specific <see cref="CheckUserConfirmsDeletionDelegate"/> is set then
        /// a default <see cref="CheckUserConfirmsDeletion"/> is used.
        /// </summary>
        CheckUserConfirmsDeletion CheckUserConfirmsDeletionDelegate { get; set; }
    }
}