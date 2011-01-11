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
using System;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// This manager groups common logic for IReadOnlyGridButtonsControl objects.
    /// Do not use this object in working code - rather call CreateReadOnlyGridButtonsControl
    /// in the appropriate control factory.
    /// </summary>
    public class ReadOnlyGridButtonsControlManager
    {
        private readonly IReadOnlyGridButtonsControl _buttonsControl;

        ///<summary>
        /// Constructor for the <see cref="ReadOnlyGridButtonsControlManager"/>
        ///</summary>
        ///<param name="buttonsControl"></param>
        public ReadOnlyGridButtonsControlManager(IReadOnlyGridButtonsControl buttonsControl)
        {
            _buttonsControl = buttonsControl;
        }

        ///<summary>
        /// The delete button.
        ///</summary>
        public IButton DeleteButton { get; private set; }

        ///<summary>
        /// Creates the delete button and binds it to the <paramref name="eventHandler"/>
        ///</summary>
        ///<param name="eventHandler"></param>
        public void CreateDeleteButton(EventHandler eventHandler)
        {
            DeleteButton = _buttonsControl.AddButton("Delete", eventHandler);
            DeleteButton.Visible = false;
        }
        ///<summary>
        /// Creates the Edit button and binds it to the <paramref name="eventHandler"/>
        ///</summary>
        ///<param name="eventHandler"></param>
        public void CreateEditButton(EventHandler eventHandler)
        {
            IButton editButton = _buttonsControl.AddButton("Edit", eventHandler);
            editButton.Visible = true;
        }
        ///<summary>
        /// Creates the add button and binds it to the <paramref name="eventHandler"/>
        ///</summary>
        ///<param name="eventHandler"></param>
        public void CreateAddButton(EventHandler eventHandler)
        {
            IButton addButton = _buttonsControl.AddButton("Add", eventHandler);
            addButton.Visible = true;
        }

    }
}
