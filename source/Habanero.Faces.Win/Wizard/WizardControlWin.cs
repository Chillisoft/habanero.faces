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
using System.Drawing;
using Habanero.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Base.Wizard;

namespace Habanero.Faces.Win
{
    /// <summary>
    /// Provides the controls for a wizard, which guides users through a process one
    /// step at a time.
    /// </summary>
    public class WizardControlWin : UserControlWin, IWizardControl
    {
        private IWizardController _wizardController;
        protected IControlFactory ControlFactory { get; private set; }
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
        /// Initialises the WizardControl with the IWizardController.  No logic is performed other than storing the wizard controller.
        /// </summary>
        /// <param name="wizardController"></param>
        /// <param name="controlFactory">The control factory that this control will use to create a button</param>
        public WizardControlWin(IWizardController wizardController, IControlFactory controlFactory)
        {
            if (wizardController == null) throw new ArgumentNullException("wizardController");
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            _wizardController = wizardController;
            ControlFactory = controlFactory;

            _wizardControlManager = new WizardControlManager(this);

            IPanel buttonPanel = CreateButtonPanel();

            _wizardStepPanel = ControlFactory.CreatePanel();

            BorderLayoutManagerWin borderLayoutManager = new BorderLayoutManagerWin(this, ControlFactory);
            borderLayoutManager.AddControl(_wizardStepPanel, BorderLayoutManager.Position.Centre);
            borderLayoutManager.AddControl(buttonPanel, BorderLayoutManager.Position.South);
        }

        /// <summary>
        /// Gets the control that is currently displayed in the WizardControl (the current wizard step's control)
        /// </summary>
        public IControlHabanero CurrentControl { get; private set; }

        /// <summary>
        /// Gets the Cancel Button so that it can be programmatically interacted with.
        /// </summary>
        public IButton CancelButton { get; private set; }

        /// <summary>
        /// Gets the Finish Button so that it can be programmatically interacted with if required.
        /// </summary>
        public IButton FinishButton { get; private set; }

        /// <summary>
        /// Gets the Next Button so that it can be programmatically interacted with.
        /// </summary>
        public IButton NextButton { get; private set; }

        /// <summary>
        /// Gets the Previous Button so that it can be programmatically interacted with.
        /// </summary>
        public IButton PreviousButton { get; private set; }

        /// <summary>
        /// Creates all the Previous Next etc Buttons and puts them on a Panel.
        /// </summary>
        /// <returns></returns>
        protected virtual IPanel CreateButtonPanel()
        {
            IPanel buttonPanel = ControlFactory.CreatePanel();
            Size buttonSize = GetButtonSize();
            FlowLayoutManager layoutManager = GetLayoutManager(buttonPanel);

            layoutManager.AddControl(CreateCancelButton(buttonSize));

            layoutManager.AddControl(CreateFinishButton(buttonSize));

            layoutManager.AddControl(CreateNextButton(buttonSize));

            layoutManager.AddControl(CreatePreviousButton(buttonSize));

            return buttonPanel;
        }

        protected FlowLayoutManager GetLayoutManager(IPanel buttonPanel)
        {
            return new FlowLayoutManager(buttonPanel, ControlFactory)
                       {Alignment = FlowLayoutManager.Alignments.Right};
        }

        protected static Size GetButtonSize()
        {
            const int buttonWidth = 75;
            const int buttonHeight = 38;
            return new Size(buttonWidth, buttonHeight);
        }

        protected IButton CreatePreviousButton(Size buttonSize)
        {
            PreviousButton = ControlFactory.CreateButton("Previous");
            PreviousButton.Click += this.PreviousButton_Click;
            PreviousButton.Size = buttonSize;
            PreviousButton.TabIndex = 0;
            return PreviousButton;
        }

        protected IButton CreateNextButton(Size buttonSize)
        {
            NextButton = ControlFactory.CreateButton("Next");
            NextButton.Click += this.NextButton_Click;
            NextButton.Size = buttonSize;
            NextButton.TabIndex = 1;
            return NextButton;
        }

        protected IButton CreateFinishButton(Size buttonSize)
        {
            FinishButton = ControlFactory.CreateButton("Finish");
            FinishButton.Click += this.FinishButton_Click;
            FinishButton.Size = buttonSize;
            FinishButton.TabIndex = 2;
            return FinishButton;
        }

        protected IButton CreateCancelButton(Size buttonSize)
        {
            CancelButton = ControlFactory.CreateButton("Cancel");
            CancelButton.Click += this.CancelButton_Click;
            CancelButton.Size = buttonSize;
            CancelButton.TabIndex = 3;
            return CancelButton;
        }

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
            try
            {
                Previous();
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            try
            {
                _wizardController.CancelWizard();
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
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
            IControlHabanero stepControl = step;

            CurrentControl = stepControl;
            FireStepChanged(step);
            _wizardStepPanel.Controls.Clear();
            stepControl.Top = WizardControl.PADDING;
            stepControl.Left = WizardControl.PADDING;
            stepControl.Width = _wizardStepPanel.Width - WizardControl.PADDING*2;
            stepControl.Height = _wizardStepPanel.Height - WizardControl.PADDING*2;
            _wizardStepPanel.Controls.Add(stepControl);

            step.InitialiseStep();
        }

        protected delegate void Operation();

        protected void DoIfCanMoveOn(Operation operation)
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

        protected virtual void SetButtonState()
        {
            _wizardControlManager.SetButtonState();
        }

        /// <summary>
        /// Calls the finish method on the controller to being the completion process.  
        /// If this is successful the Finished event is fired.
        /// </summary>
        public virtual void Finish()
        {
            DoIfCanMoveOn(delegate
                  {
                      _wizardController.Finish();
                      FireFinished();
                  });
        }

        // ReSharper disable MemberCanBePrivate.Global
        protected void FireFinished()
        {
            if (Finished != null)
            {
                Finished(this,new EventArgs());
            }
        }
        protected void FireMessagePosted(string message)
        {
            if (MessagePosted != null)
            {
                MessagePosted(message);
            }
        }
        protected void FireStepChanged(IWizardStep wizardStep)
        {
            if (StepChanged != null)
            {
                StepChanged(wizardStep);
            }
        }
        // ReSharper restore MemberCanBePrivate.Global

    }
}