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
