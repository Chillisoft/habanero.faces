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
using Habanero.Faces.Test.Base.Wizard;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Wizard
{
    [TestFixture]
    public class TestWizardControlVWG : TestWizardControl
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override IWizardControllerSpy CreateWizardControllerStub()
        {
            return new WizardControllerSpy<WizardStepStubVWG>();
        }

        protected override IWizardStepStub CreateWizardStepStub()
        {
            return new WizardStepStubVWG();
        }

        public class WizardStepStubVWG : ControlVWG, IWizardStepStub
        {
            public bool UndoMoveOnWasCalled { get; set; }
            public bool MoveOnWasCalled { get; set; }
            public WizardStepStubVWG()
                : this("")
            {
            }

            public WizardStepStubVWG(string headerText)
            {
                AllowMoveOn = true;
                AllowMoveBack = true;
                AllowFinish = false;
                HeaderText = headerText;
                UndoMoveOnWasCalled = false;
                MoveOnWasCalled = false;
            }
            public void UndoMoveOn()
            {
                UndoMoveOnWasCalled = true;
            }

            public string HeaderText { get; set; }

            /// <summary>
            /// Provides an interface for the developer to implement functionality to cancel any edits made as part of this
            /// wizard step. The default wizard controller functionality is to call all wizard steps cancelStep methods when
            /// its Cancel method is called.
            /// </summary>
            public void CancelStep()
            {

            }

            /// <summary>
            /// Can the user select finish from this wizard step. I.e. is sufficient information captured 
            /// so that all information gathered in future steps can be set to defaults.
            /// </summary>
            /// <returns></returns>
            public bool CanFinish()
            {
                return AllowFinish;
            }

            /// <summary>
            /// Can the user cancel from this wizard step. I.e. the objects under the control of this
            /// step have not been moved into a state that prohibits cancelling all changes.
            /// It is very rare that you would not be able to cancel at any point in a wizard.
            /// </summary>
            /// <returns></returns>
            public bool CanCancel()
            {
                return true;
            }


            public bool AllowMoveBack { get; set; }
            public bool AllowFinish { get; set; }

            #region IWizardStep Members

            public void InitialiseStep()
            {
                IsInitialised = true;
            }

            public bool CanMoveOn(out string message)
            {
                message = "";
                if (!AllowMoveOn) message = "Sorry, can't move on";
                return AllowMoveOn;
            }

            /// <summary>
            /// Verifies whether the user can move back from this step.
            /// </summary>
            /// <returns></returns>
            public bool CanMoveBack()
            {
                return AllowMoveBack;
            }

            public void MoveOn()
            {
                MoveOnWasCalled = true;
            }

            #endregion

            public bool AllowMoveOn { get; set; }

            IControlCollection IControlHabanero.Controls
            {
                get
                {
                    return null;

                }
            }

            public bool IsInitialised { get; private set; }

            ///<summary>
            ///Returns a <see cref="T:System.String"></see> containing the name of the <see cref="T:System.ComponentModel.Component"></see>, if any. This method should not be overridden.
            ///</summary>
            ///
            ///<returns>
            ///A <see cref="T:System.String"></see> containing the name of the <see cref="T:System.ComponentModel.Component"></see>, if any, or null if the <see cref="T:System.ComponentModel.Component"></see> is unnamed.
            ///</returns>
            ///
            public override string ToString()
            {
                return Name;
            }
        }

    }
}