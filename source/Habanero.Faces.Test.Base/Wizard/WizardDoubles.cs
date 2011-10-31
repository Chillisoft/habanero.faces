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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Faces.Base;

namespace Habanero.Faces.Test.Base.Wizard
{

    #region Testing Wizard Class Interfaces

    public interface IWizardStepStub : IWizardStep
    {
        bool AllowMoveOn { get; set; }
        bool AllowFinish { get; set; }
        bool IsInitialised { get; }
        bool AllowMoveBack { get; set; }
        new string HeaderText { get; set; }
        bool UndoMoveOnWasCalled { get; set; }
        bool MoveOnWasCalled { get; set; }
    }

    public interface IWizardControllerSpy : IWizardController
    {
        IWizardStepStub ControlForStep1 { get; }
        bool CancelWizardWasCalled { get; }
        IWizardStepStub ControlForStep2 { get; }
        IWizardStepStub ControlForStep3 { get; }
        bool FinishWizardWasCalled { get; }
        bool AllowCancel { get; set; }
        bool? AllowCanMoveOn { get; set; }
        void ForTestingAddWizardStep(IWizardStep wizardStepStub);
    }

    public class WizardControllerSpy<T> : IWizardControllerSpy where T : IWizardStepStub, new()
    {
        private readonly List<IWizardStep> _wizardSteps;

        public IWizardStepStub ControlForStep1 { get; set; }

        public IWizardStepStub ControlForStep2 { get; set; }

        public IWizardStepStub ControlForStep3 { get; set; }

        public bool FinishWizardWasCalled { get; set; }

        public bool AllowCancel { get; set; }

        public bool? AllowCanMoveOn { get; set; }

        public WizardControllerSpy()
        {
            CancelWizardWasCalled = false;
            CurrentStep = -1;
            ControlForStep3 = new T();
            ControlForStep2 = new T();
            ControlForStep1 = new T();
            FinishWizardWasCalled = false;
            _wizardSteps = new List<IWizardStep>();
            ControlForStep1.Name = "ControlForStep1";
            ControlForStep2.Name = "ControlForStep2";
            ControlForStep3.Name = "ControlForStep3";
            ControlForStep2.HeaderText = "This is wizard step 2";
            ControlForStep3.HeaderText = "This is wizard step 3";
            _wizardSteps.Add(ControlForStep1);
            _wizardSteps.Add(ControlForStep2);
            _wizardSteps.Add(ControlForStep3);

        }

        public IWizardStep GetNextStep()
        {
            if (CurrentStep < _wizardSteps.Count - 1)
                return _wizardSteps[++CurrentStep];
            throw new WizardStepException("Invalid Wizard Step: " + (CurrentStep + 1));
        }

        public IWizardStep GetPreviousStep()
        {
            if (CurrentStep > 0)
                return _wizardSteps[--CurrentStep];
            throw new WizardStepException("Invalid Wizard Step: " + (CurrentStep - 1));
        }

        public IWizardStep GetFirstStep()
        {
            FinishWizardWasCalled = false;
            return _wizardSteps[CurrentStep = 0];
        }

        public bool IsLastStep()
        {
            return (CurrentStep == _wizardSteps.Count - 1);
        }

        public bool IsFirstStep()
        {
            return (CurrentStep == 0);
        }

        public void Finish()
        {
            FinishWizardWasCalled = true;
            //                else throw new WizardStepException("Invalid call to Finish(), not at last step");
        }

        public bool CanMoveOn(out string message)
        {
            message = "AllowCanMoveOnFalse";
            if (AllowCanMoveOn != null) return AllowCanMoveOn.GetValueOrDefault();
            return _wizardSteps[CurrentStep].CanMoveOn(out message);
        }

        public int StepCount
        {
            get { return _wizardSteps.Count; }
        }

        /// <summary>
        /// Gets or Sets the Current Step of the Wizard.
        /// </summary>
        public int CurrentStep { get; private set; }

        public bool CancelWizardWasCalled { get; private set; }

        public IWizardStep GetCurrentStep()
        {
            return _wizardSteps[CurrentStep];
        }

        /// <summary>
        /// This provides a method which is called when the wizard is cancelled. The wizard controller can 
        /// undo any changes that have occured up until that point so as to ensure that the objects are returned
        /// to their original state.
        /// </summary>
        public void CancelWizard()
        {
            this.CancelWizardWasCalled = true;
        }

        public void CompleteCurrentStep()
        {
            GetCurrentStep().MoveOn();
        }

        public void UndoCompleteCurrentStep()
        {
            GetCurrentStep().UndoMoveOn();
        }

        public bool CanMoveBack()
        {
            return this.GetCurrentStep().CanMoveBack();
        }

        public bool CanFinish()
        {
            return this.GetCurrentStep().CanFinish();
        }

        public bool CanCancel()
        {
            return this.AllowCancel;
        }

        public void ForTestingAddWizardStep(IWizardStep step)
        {
            _wizardSteps.Add(step);
        }
    }


    #endregion // Testing Wizard Class Interfaces
}
