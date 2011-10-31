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
using Habanero.Base;
using Habanero.BO;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// Provides a common interface that is specialised for showing a collection of 
    /// Business Objects and allowing the user to select one (Or in some cases more than one)
    /// The common controls used for selecting business objects are ComboBox, ListBox, ListView, Grid,
    ///  <see cref="ICollapsiblePanelGroupControl"/>, <see cref="IBOColTabControl"/>, a <see cref="IMultiSelector{T}"/>
    ///  or an <see cref="ITreeView"/>
    /// </summary>
    public interface IBOColSelector
    {
        /// <summary>
        /// Gets and Sets the business object collection displayed in the grid.  This
        /// collection must be pre-loaded using the collection's Load() command or from the
        /// <see cref="IBusinessObjectLoader"/>.
        /// The default UI definition will be used, that is a 'ui' element 
        /// without a 'name' attribute.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        IBusinessObjectCollection BusinessObjectCollection { get; set; }

        /// <summary>
        /// Gets and sets the currently selected business object in the grid
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        IBusinessObject SelectedBusinessObject { get; set; }
//
//        /// <summary>
//        /// Gets a List of currently selected business objects (In Controls that do not allow the selection 
//        /// of multiple items this will be a
//        /// </summary>
//        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        ReadOnlyCollection<BusinessObject> SelectedBusinessObjects { get; }

        /// <summary>
        /// Event Occurs when a business object is selected
        /// </summary>
        event EventHandler<BOEventArgs> BusinessObjectSelected;

//        /// <summary>
//        /// Occurs when the current selection in the selector is changed
//        /// </summary>
//        event EventHandler SelectionChanged;

//        /// <summary>
//        /// Occurs when the collection in the selector is changed
//        /// </summary>
//        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        event EventHandler CollectionChanged;

        /// <summary>
        /// Clears the business object collection and the rows in the data table
        /// </summary>
        void Clear();

        /// <summary>Gets the number of items displayed in the <see cref="IBOColSelector"></see>.</summary>
        /// <returns>The number of items in the <see cref="IBOColSelector"></see>.</returns>
        [DefaultValue(0), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
         EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false)]
        int NoOfItems { get; }
        
        /// <summary>
        /// Returns the business object at the specified row number
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        IBusinessObject GetBusinessObjectAtRow(int row);

        /// <summary>
        /// Gets and sets whether this selector autoselects the first item or not when a new collection is set.
        /// </summary>
        bool AutoSelectFirstItem { get; set; }

        /// <summary>
        /// Gets and sets whether the Control is enabled or not
        /// </summary>
        bool ControlEnabled { get; set; }
    }
}