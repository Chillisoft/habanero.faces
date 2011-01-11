using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habanero.Faces.Base.Wizard
{
    /// <summary>
    /// This manager groups common logic for IWizardControl objects.
    /// Do not use this object in project code - you should rather be 
    /// creating an <see cref="IWizardControl"/> of the appropriate type (Win or VWG).
    /// E.g. Via the <see cref="IControlFactory.CreateWizardControl"/> or directly via
    /// </summary>
    public class WizardControlManager
    {
        private IWizardControl WizardControl { get; set; }

        ///<summary>
        /// Create a wizard control manager with the wizard control it is managing
        ///</summary>
        ///<param name="wizardControl"></param>
        ///<exception cref="ArgumentNullException"></exception>
        public WizardControlManager(IWizardControl wizardControl)
        {
            if (wizardControl == null) throw new ArgumentNullException("wizardControl");
            WizardControl = wizardControl;
        }
        /// <summary>
        /// Sets the enabled state of the Finish, Previous, Cancel and next Buttons
        /// based on the state of the wizard controller.
        /// </summary>
        public void SetButtonState()
        {
            var wizardController = WizardControl.WizardController;
            WizardControl.PreviousButton.Enabled = !wizardController.IsFirstStep();
            if (WizardControl.PreviousButton.Enabled)
            {
                WizardControl.PreviousButton.Enabled = wizardController.CanMoveBack();
            }
            WizardControl.FinishButton.Enabled = wizardController.CanFinish();
            WizardControl.CancelButton.Enabled = wizardController.CanCancel();
            if (wizardController.IsLastStep())
            {
                WizardControl.FinishButton.Enabled = true;
                WizardControl.NextButton.Enabled = false;
                return;
            }
            WizardControl.NextButton.Enabled = true;
            
        }

    }
}
