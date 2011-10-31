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
using System.Drawing.Design;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// Provides a common implementation of members for the ListBox and ComboBox classes
    /// </summary>
    public interface IListControl:IControlHabanero
    {
        /// <summary>
        /// Occurs when the SelectedValue property changes.
        /// </summary>
        /// <filterpriority>1</filterpriority>
        event EventHandler SelectedValueChanged;

        /// <summary>
        /// Gets a collection containing the currently selected items in the ListBox
        /// </summary>
        ISelectedObjectCollection SelectedItems { get; }
        /// <summary>
        /// Occurs when the SelectedIndex property has changed
        /// </summary>
        event EventHandler SelectedIndexChanged;
        /// <summary>
        /// Returns the text representation of the specified item.
        /// </summary>
        /// <returns>If the DisplayMember property is not specified, the value returned by
        /// GetItemText(System.Object) is the value of the item's ToString method.
        /// Otherwise, the method returns the string value of the member specified in the
        /// DisplayMember property for the object specified in the item parameter.</returns>
        /// <param name="item">The object from which to get the contents to display</param>
        /// <filterpriority>1</filterpriority>
        string GetItemText(object item);

        /// <summary>
        /// Gets or sets the data source for this ComboBox
        /// </summary>
        object DataSource { get; set; }

        /// <summary>
        /// Gets or sets the property to display for this ListControl
        /// </summary>
        /// <returns>
        /// A String specifying the name of an object property that is contained in the
        /// collection specified by the ListControl.DataSource property.
        /// The default is an empty string ("").</returns>
        /// <filterpriority>1</filterpriority>
        [DefaultValue("")]
        string DisplayMember { get; set; }

        /// <summary>
        /// When overridden in a derived class, gets or sets the zero-based
        /// index of the currently selected item.
        /// </summary>
        /// <returns>A zero-based index of the currently selected item.
        /// A value of negative one (-1) is returned if no item is selected.</returns>
        /// <filterpriority>1</filterpriority>
        [Bindable(true)]
        int SelectedIndex { get; set; }

        /// <summary>
        /// Gets or sets the value of the member property specified by the ValueMember property.
        /// </summary>
        /// <returns>An object containing the value of the member of the data source
        /// specified by the ValueMember property.</returns>
        /// <exception cref="T:System.InvalidOperationException">The assigned value is null or the empty string ("").</exception>
        /// <filterpriority>1</filterpriority>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        object SelectedValue { get; set; }


        /// <summary>
        /// Gets or sets the currently selected item in the ListBox
        /// </summary>
        ///	<returns>An object that represents the current selection in the control.</returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        [DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden), Browsable(false)
        , Bindable(true)]
        object SelectedItem { get; set; }
        /// <summary>
        /// Gets or sets the property to use as the actual value for the items in the <see cref="IListControl"/>
        /// </summary>
        /// <returns>A <see cref="T:System.String"></see> representing the name of an object property
        /// that is contained in the collection specified by the DataSource property.
        /// The default is an empty string ("").</returns>
        /// <exception cref="T:System.ArgumentException">The specified property cannot be found on the
        /// object specified by the DataSource property</exception>
        /// <filterpriority>1</filterpriority>
        [DefaultValue(""),
         Editor(
             "System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
             , typeof (UITypeEditor))]
        string ValueMember { get; set; }
        /// <summary>
        /// Gets an object representing the collection of the items
        /// contained in this ComboBox
        /// </summary>
        IListControlObjectCollection Items { get; }
    }
}
