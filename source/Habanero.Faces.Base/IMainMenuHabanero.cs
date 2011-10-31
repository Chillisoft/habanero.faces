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
namespace Habanero.Faces.Base
{
    ///<summary>
    /// The main menu interface used to represent the main menu structure that has been 
    /// set up for it's specific implementation
    ///</summary>
    public interface IMainMenuHabanero//:IControlHabanero
    {
        ///<summary>
        /// The collection of menu items for this menu
        ///</summary>
        IMenuItemCollection MenuItems { get; }

        /// <summary>
        /// This method sets up the form so that the menu is displayed and the form is able to 
        /// display the controls loaded when the menu item is clicked.
        /// </summary>
        /// <param name="form">The form to set up with the menu</param>
        void DockInForm(IControlHabanero form);

        /// <summary>
        ///This method sets up the form so that the menu is displayed and the form is able to 
        ///display the controls loaded when the menu item is clicked.
        /// </summary>
        /// <param name="form">The form to set up with the menu</param>
        /// <param name="menuWidth">The width of the menu - configurable to so that each application can set its menu width</param>
        void DockInForm(IControlHabanero form, int menuWidth);

        /// <summary>
        /// Gets and Sets the name of the MainMenu
        /// </summary>
        /// <returns>A string representing the name.</returns>
        string Name { get; set; }
    }

}