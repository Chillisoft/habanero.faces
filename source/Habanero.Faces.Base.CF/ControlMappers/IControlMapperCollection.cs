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
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// Manages a collection of mappers that are sub-types of ControlMapper
    /// </summary>
    public interface IControlMapperCollection: IList<IControlMapper>
    {

        /// <summary>
        /// Provides an indexing facility as before, but allows the objects
        /// to be referenced using their property names instead of their
        /// numerical position
        /// </summary>
        /// <param name="propertyName">The property name of the object</param>
        /// <returns>Returns the mapper if found, or null if not</returns>
        IControlMapper this[string propertyName] { get; }

        /// <summary>
        /// Adds a mapper object to the collection
        /// </summary>
        /// <param name="mapper">The object to add, which must be a type or
        /// sub-type of ControlMapper</param>
        new IControlMapper Add(IControlMapper mapper);

        /// <summary>
        /// Gets and sets the business object being represented by
        /// the mapper collection.  Updates the business object for 
        /// every control mapper in this collection.
        /// </summary>
        IBusinessObject BusinessObject { get; set; }

        /// <summary>
        /// Enables or disables all the controls managed in this control mapper collection.
        /// </summary>
        bool ControlsEnabled { set; }

    }
}