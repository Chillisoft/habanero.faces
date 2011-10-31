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
using System.Drawing;
using Habanero.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Base.Wizard;

namespace Habanero.Faces.VWG
{
    /// <summary>
    /// Provides the controls for a wizard, which guides users through a process one
    /// step at a time.
    /// </summary>
    public class WizardControlVWG : UserControlVWG, IWizardControl
    {
        private IWizardController _wizardController;
        private readonly IControlFactory _controlFactory;
        private readonly IPanel _wizardStepPanel;
        private readonly WizardControlManager _wizardControlManager;

        /// <summary>
        /// Raised when the wizard is complete to notify the containing control or controlling object.
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// Raised when a message is communicated so the controlling object can display or log the message.
        ///  uses an <see cref="Action{T}"/> which is merely a predifined delegate that takes one parameter of Type T and
        /// returns a void.
        /// </summary>
        public event Action<string> MessagePosted;

        /// <summary>
        /// Raised when the wizard step changes. The new step is passed through as an event argument.
        /// </summary>
        public event Action<IWizardStep> StepChanged;


        /// <summary>
        /// The panel that the controls are physically being placed on.
        /// </summary>
        public IPanel WizardStepPanel
        {
            get { return _wizardStepPanel; }
        }

        /// <summary>
        /// Gets the Cancel Button so that it can be programmatically interacted with.
        /// </summary>
        public IButton CancelButton { get; private set; }

        /// <summary>
        /// Gets the Finish Button so that it can be programmatically interacted with if required.
        /// </summary>
        public IButton FinishButton { get; private set; }

        /// <summary>
        /// Initialises the WizardControl with the IWizardController.  No logic is performed other than storing the wizard controller.
        /// </summary>
        /// <param name="wizardController"></param>
        /// <param name="controlFactory">The control factory that this control will use to create a button</param>
        public WizardControlVWG(IWizardController wizardController, IControlFactory controlFactory)
        {
            if (wizardController == null) throw new ArgumentNullException("wizardController");
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            _wizardController = wizardController;
            _controlFactory = controlFactory;

            _wizardControlManager = new WizardControlManager(this);
            IPanel buttonPanel = CreateButtonPanel();

            _wizardStepPanel = _controlFactory.CreatePanel();

            BorderLayoutManagerVWG borderLayoutManager = new BorderLayoutManagerVWG(this, _controlFactory);
            borderLayoutManager.AddControl(_wizardStepPanel, BorderLayoutManager.Position.Centre);
            borderLayoutManager.AddControl(buttonPanel, BorderLayoutManager.Position.South);
        }

        private IPanel CreateButtonPanel()
        {
            IPanel buttonPanel = _controlFactory.CreatePanel();
            const int buttonWidth = 75;
            const int buttonHeight = 38;
            var buttonSize = new Size(buttonWidth, buttonHeight);
            FlowLayoutManager layoutManager = new FlowLayoutManager(buttonPanel, _controlFactory);
            layoutManager.Alignment = FlowLayoutManager.Alignments.Right;

            CancelButton = _controlFactory.CreateButton("Cancel");
            CancelButton.Click += this.CancelButton_Click;
            CancelButton.Size = new Size(75, 38);
            CancelButton.TabIndex = 3;
            layoutManager.AddControl(CancelButton);

            FinishButton = _controlFactory.CreateButton("Finish");
            FinishButton.Click += this.FinishButton_Click;
            FinishButton.Size = buttonSize;
            FinishButton.TabIndex = 2;
            layoutManager.AddControl(FinishButton);

            NextButton = _controlFactory.CreateButton("Next");
            NextButton.Click += this.NextButton_Click;
            NextButton.Size = new Size(75, 38);
            NextButton.TabIndex = 1;
            layoutManager.AddControl(NextButton);

            PreviousButton = _controlFactory.CreateButton("Previous");
            PreviousButton.Click += this.PreviousButton_Click;
            PreviousButton.Size = new Size(75, 38);
            PreviousButton.TabIndex = 0;
            layoutManager.AddControl(PreviousButton);


            return buttonPanel;
        }

        /// <summary>
        /// Gets the control that is currently displayed in the WizardControl (the current wizard step's control)
        /// </summary>
        public IControlHabanero CurrentControl { get; private set; }

        /// <summary>
        /// Gets the Next Button so that it can be programmatically interacted with.
        /// </summary>
        public IButton NextButton { get; private set; }

        /// <summary>
        /// Gets the Previous Button so that it can be programmatically interacted with.
        /// </summary>
        public IButton PreviousButton { get; private set; }

        /// <summary>
        /// Gets or sets the WizardController.  Upon setting the controller, the Start() method is called to begin the wizard.
        /// </summary>
        public IWizardController WizardController
        {
            get { return _wizardController; }
            set
            {
                _wizardController = value;
                if (_wizardController != null) Start();
            }
        }

        /// <summary>
        /// Attempts to go to the next step in the wizard.  If this is disallowed by the wizard controller a MessagePosted event will be fired.
        /// </summary>
        public void Next()
        {
            DoIfCanMoveOn(delegate
                              {
                                  _wizardController.CompleteCurrentStep();
                                  SetStep(_wizardController.GetNextStep());
                                  SetButtonState();
                              });
        }

        /// <summary>
        /// Attempts to go to the previous step in the wizard.
        ///  </summary>
        /// <exception cref="WizardStepException">If the wizard is on the first step this exception will be thrown.</exception>
        public void Previous()
        {
            var previousStep = _wizardController.GetPreviousStep();
            _wizardController.UndoCompleteCurrentStep();
            SetStep(previousStep);
            NextButton.Text = "Next";
            SetButtonState();
        }

        /// <summary>
        /// Starts the wizard by moving to the first step.
        /// </summary>
        public void Start()
        {
            SetStep(_wizardController.GetFirstStep());
            SetButtonState();
        }

        private void SetStep(IWizardStep step)
        {
            if (step == null) throw new ArgumentNullException("step");

            CurrentControl = step;
            FireStepChanged(step);
            //TODO: The border layout manager clearing panel etc not unit tested
            _wizardStepPanel.Controls.Clear();
            step.Top = WizardControl.PADDING;
            step.Left = WizardControl.PADDING;
            step.Width = _wizardStepPanel.Width - WizardControl.PADDING * 2;
            step.Height = _wizardStepPanel.Height - WizardControl.PADDING * 2;
            _wizardStepPanel.Controls.Add(step);
            //BorderLayoutManagerVWG borderLayoutManager = new BorderLayoutManagerVWG(_wizardStepPanel, _controlFactory);
            //borderLayoutManager.AddControl(stepControl, BorderLayoutManager.Position.Centre);

            step.InitialiseStep();
        }

        private delegate void Operation();

        private void DoIfCanMoveOn(Operation operation)
        {
            string message;
            if (_wizardController.CanMoveOn(out message))
            {
                operation();
            }
            else
            {
                FireMessagePosted(message);
            }
        }


        private void CancelButton_Click(object sender, EventArgs e)
        {
            _wizardController.CancelWizard();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            try
            {
                Next();
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "Cannot complete this wizard step due to an error:",
                                                          "Wizard Step Error");
            }
        }

        private void FinishButton_Click(object sender, EventArgs e)
        {
            try
            {
                Finish();
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "Cannot complete wizard step due to an error:",
                                                          "Wizard Step Error");
            }
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            Previous();
        }

        private void SetButtonState()
        {
            _wizardControlManager.SetButtonState();
        }

        /// <summary>
        /// Calls the finish method on the controller to being the completion process.  If this is successful the Finished event is fired.
        /// </summary>
        public void Finish()
        {
            DoIfCanMoveOn(delegate
                      {
                          _wizardController.Finish();
                          FireFinished();
                      });
        }

        private void FireFinished()
        {
            if (Finished != null)
            {
                Finished(this, new EventArgs());
            }
        }

        private void FireMessagePosted(string message)
        {
            if (MessagePosted != null)
            {
                MessagePosted(message);
            }
        }

        private void FireStepChanged(IWizardStep wizardStep)
        {
            if (StepChanged != null)
            {
                StepChanged(wizardStep);
            }
        }
    }
}