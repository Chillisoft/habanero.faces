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
    /// Provides a form used to edit business objects.  This form will usually
    /// be constructed using a UI Form definition provided in the class definitions.
    /// The appropriate UI definition is typically set in the constructor.
    /// </summary>
    public interface IDefaultBOEditorForm: IFormHabanero
    {
        /// <summary>
        /// Gets the button control for the buttons in the form
        /// </summary>
        IButtonGroupControl Buttons { get; }

        /// <summary>
        /// Pops the form up in a modal dialog.  If the BO is successfully edited and saved, returns true,
        /// else returns false.
        /// </summary>
        /// <returns>True if the edit was a success, false if not</returns>
        new bool ShowDialog();

        /// <summary>
        /// Gets the object containing all information related to the form, including
        /// its controls, mappers and business object
        /// </summary>
        IPanelInfo PanelInfo{ get;}

        ///<summary>
        /// The Creator (<see cref="GroupControlCreator"/> used to create the <see cref="IGroupControl"/>
        ///</summary>
        GroupControlCreator GroupControlCreator { get; }
    }
}