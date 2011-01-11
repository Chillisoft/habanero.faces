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
namespace Habanero.Faces.Base
{
    /// <summary>
    /// Provides an interface that is specialised for showing a collection of 
    /// Business Objects in a <see cref="IComboBox"/> and allowing the user to select one.
    /// </summary>
    public interface IBOComboBoxSelector : IBOColSelectorControl, IComboBox
    {
//        ///<summary>
//        /// Returns the control factory used by this selector
//        ///</summary>
//        IControlFactory ControlFactory { get; }
        ///<summary>
        /// Returns the Underlying ComboBoxControl that is used by this selector
        /// Unfortunately due to limitations in WinForms Designer I cannot make these generic
        /// so have to have this as a different method on each interface.
        ///</summary>
        IComboBox ComboBox { get; }

        ///<summary>
        /// Gets or sets whether the current <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see> should be preserved in the selector when the 
        /// <see cref="IBOColSelectorControl.BusinessObjectCollection">BusinessObjectCollection</see> 
        /// is changed to a new collection which contains the current <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see>.
        /// If the <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see> doesn't exist in the new collection then the
        /// <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see> is set to null.
        /// If the current <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see> is null then this will also be preserved.
        /// This overrides the <see cref="IBOColSelectorControl.AutoSelectFirstItem">AutoSelectFirstItem</see> property.
        ///</summary>
        bool PreserveSelectedItem { get; set; }
    }

}