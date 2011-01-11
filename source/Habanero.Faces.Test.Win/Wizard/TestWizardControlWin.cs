using System;
using Habanero.Faces.Test.Base.Wizard;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Wizard
{
    [TestFixture]
    public class TestWizardControlWin : TestWizardControl
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override IWizardControllerSpy CreateWizardControllerStub()
        {
            return new WizardControllerSpy<WizardStepStubWin>();
        }

        protected override IWizardStepStub CreateWizardStepStub()
        {
            return new WizardStepStubWin();
        }

        internal class WizardStepStubWin : ControlWin, IWizardStepStub
        {
            public bool UndoMoveOnWasCalled { get; set; }
            public bool MoveOnWasCalled { get; set; }
            public WizardStepStubWin()
                : this("")
            {
            }

            public WizardStepStubWin(string headerText)
            {
                AllowMoveBack = true;
                AllowMoveOn = true;
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