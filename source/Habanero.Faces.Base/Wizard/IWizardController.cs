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
    /// Controls the behaviour of a wizard, which guides users through a process one
    /// step at a time. Each step in the wizard is represented by a user control that implements
    /// the IWizardStep Interface.
    /// </summary>
    public interface IWizardController
    {
        /// <summary>
        /// Returns the next step in the Wizard and sets the current step to that step.
        /// </summary>
        /// <exception cref="WizardStepException">Thrown if the current step is the last step.</exception>
        /// <returns>The next step.</returns>
        IWizardStep GetNextStep();

        /// <summary>
        /// Returns the Previous Step and sets the step pointer to that step.
        /// </summary>
        /// <exception cref="WizardStepException">Thrown if the current step is the first step.</exception>
        /// <returns>The previous step.</returns>
        IWizardStep GetPreviousStep();

        /// <summary>
        /// Returns the First Step of the Wizard and sets the current step to that step.
        /// </summary>
        /// <returns>The first step.</returns>
        IWizardStep GetFirstStep();

        /// <summary>
        /// Checks if the current step is the last step.
        /// </summary>
        /// <returns>True if the current step is the last step.</returns>
        bool IsLastStep();

        /// <summary>
        /// Checks if the current Step is the first step.
        /// </summary>
        /// <returns>True if the current step is the first step.</returns>
        bool IsFirstStep();

        /// <summary>
        /// Method that is to be run when the Wizard is finished. This method should be overridden to do all persistance that is required.
		/// 
        /// </summary>
        void Finish();

        /// <summary>
        /// Checks if the Wizard can proceed to the next step. Calls through to the CanMoveOn method of the current IWizardStep.
        /// </summary>
        /// <param name="message">Describes why the Wizard cannot move on. Only applicable if CanMoveOn returns false.</param>
        /// <returns>True if moving to the next step is allowed.</returns>
        bool CanMoveOn(out string message);

        /// <summary>
        /// Returns the number of Steps in the Wizard.
        /// </summary>
        int StepCount { get; }

        /// <summary>
        /// Gets or Sets the Current Step of the Wizard.
        /// </summary>
        int CurrentStep
        {
            get;
        }

        /// <summary>
        /// Returns the step that the Wizard is currently on.
        /// </summary>
        /// <returns></returns>
        IWizardStep GetCurrentStep();

        /// <summary>
        /// This provides a method which is called when the wizard is cancelled. The wizard controller can 
        /// undo any changes that have occured up until that point so as to ensure that the objects are returned
        /// to their original state.
        /// </summary>
        void CancelWizard();
        /// <summary>
        /// Does any actions involved in the current wizard step when you move on
        /// to the next wizard step. E.g. Updates any Objects from 
        /// User interface controls. it does this by calling the current
        /// <see cref="IWizardStep.MoveOn"/>
        /// </summary>
        void CompleteCurrentStep();

        /// <summary>
        /// Undoes any actions that have been done by the <see cref="CompleteCurrentStep"/> 
        /// when you move back to it from a previous step.
        /// It does this by calling the current <see cref="IWizardStep.UndoMoveOn"/>
        /// </summary>
        void UndoCompleteCurrentStep();

        /// <summary>
        /// Checks if the Wizard Can proceed to the next step. Calls through to the <see cref="IWizardStep.CanMoveBack"/>
        /// </summary>
        bool CanMoveBack();

        /// <summary>
        /// Can the user select finish from when the wizard controller is in the current state.
        /// I.e. Is sufficient information captured 
        /// so that all information gathered in future steps can be set to defaults.
        /// </summary>
        /// <returns></returns>
        bool CanFinish();

        /// <summary>
        /// Can the user cancel from this wizard controller when it is in this state. 
        /// I.e. Have the objects under the control of this
        /// controller have not been moved into a state that prohibits cancelling all changes.
        /// It is very rare that you would not be able to cancel at any point in a wizard.
        /// </summary>
        /// <returns></returns>
        bool CanCancel();
    }
}
