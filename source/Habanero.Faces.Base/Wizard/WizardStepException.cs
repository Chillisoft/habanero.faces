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
    /// An exception used in Wizards denoting that some error has occurred in navigating between wizard steps.
    /// </summary>
    public class WizardStepException : Exception
    {
        /// <summary>
        /// Initialises the WizardStepException
        /// </summary>
        /// <param name="message">the message of the exception</param>
        public WizardStepException(string message) :base(message)
        {
            
        }
    }

    /// <summary>
    /// Thrown when a wizard step is used but has not been initialised
    /// </summary>
    public class WizardStepNotInitialised : WizardStepException
    {
        ///<summary>
        /// Constructor for <see cref="WizardStepNotInitialised"/>
        ///</summary>
        ///<param name="message"></param>
        public WizardStepNotInitialised(string message) : base(message)
        {
        }
    }
}
