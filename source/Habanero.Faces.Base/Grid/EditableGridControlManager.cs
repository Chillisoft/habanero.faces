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
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// This manager groups common logic for IEditableGridControl objects.
    /// Do not use this object in working code - rather call CreateEditableGridControl
    /// in the appropriate control factory.
    /// </summary>
    public class EditableGridControlManager
    {
//        private readonly IEditableGridControl _gridControl;
        private readonly IGridInitialiser _gridInitialiser;

        ///<summary>
        /// Constructor for <see cref="EditableGridControlManager"/>
        ///</summary>
        ///<param name="gridControl"></param>
        ///<param name="controlFactory"></param>
        public EditableGridControlManager(IEditableGridControl gridControl, IControlFactory controlFactory)
        {
//            _gridControl = gridControl;
            _gridInitialiser = new GridInitialiser(gridControl, controlFactory);
        }

        /// <summary>
        /// See <see cref="IGridControl.Initialise(IClassDef)"/>
        /// </summary>
        public void Initialise(IClassDef classDef)
        {
            _gridInitialiser.InitialiseGrid(classDef);
        }

        /// <summary>
        /// See <see cref="IGridControl.Initialise(IClassDef,string)"/>
        /// </summary>
        public void Initialise(IClassDef classDef, string uiDefName)
        {
            _gridInitialiser.InitialiseGrid(classDef, uiDefName);
        }


        /// <summary>
        /// See <see cref="IGridControl.Initialise(IClassDef,IUIGrid,string)"/>
        /// </summary>
        public void Initialise(IClassDef classDef, IUIGrid uiGridDef, string uiDefName)
        {
            _gridInitialiser.InitialiseGrid(classDef, uiGridDef, uiDefName);
        }
    }
}