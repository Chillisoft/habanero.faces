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
    /// Provides a single step in a wizard control which is used by the <see cref="IWizardController"/>
    /// </summary>
    public interface IWizardStep : IControlHabanero
    {
        /// <summary>
        /// Initialises the step. Run when the step is reached.
        /// </summary>
        void InitialiseStep();

        /// <summary>
        /// Verifies whether this step can be passed.
        /// </summary>
        /// <param name="message">Error message should moving on be disallowed. This message will be displayed to the user by the WizardControl.</param>
        bool CanMoveOn(out String message);

        /// <summary>
        /// Verifies whether the user can move back from this step.
        /// </summary>
        bool CanMoveBack();

        /// <summary>
        /// Does any actions involved in this wizard step when you move on
        /// to the next wizard step. E.g. Updates any Objects from 
        /// User interface controls.
        /// </summary>
        void MoveOn();

        /// <summary>
        /// Undoes any actions that have been done by this wizard step.
        /// Usually you would want this to do nothing since if the 
        /// user does a previous and then next they would not expect to 
        /// lose their. But in some cases you may have created objects based on
        /// the selection in this step and when you move back to this step you want to
        /// these so that if the user changes his/her selection then new objects or different
        /// objects are created.
        /// </summary>
        void UndoMoveOn();

        /// <summary>
        /// The text that you want displayed at the top of the wizard control when this step is active.
        /// </summary>
        string HeaderText { get; }

        /// <summary>
        /// Provides an interface for the developer to implement functionality to cancel any edits made as part of this
        /// wizard step. The default wizard controller functionality is to call all wizard steps cancelStep methods when
        /// its Cancel method is called.
        /// </summary>
        void CancelStep();

        /// <summary>
        /// Can the user select finish from this wizard step. I.e. is sufficient information captured 
        /// so that all information gathered in future steps can be set to defaults.
        /// </summary>
        /// <returns></returns>
        bool CanFinish();

        /// <summary>
        /// Can the user cancel from this wizard step. I.e. the objects under the control of this
        /// step have not been moved into a state that prohibits cancelling all changes.
        /// It is very rare that you would not be able to cancel at any point in a wizard.
        /// </summary>
        /// <returns></returns>
        bool CanCancel();
    }
}