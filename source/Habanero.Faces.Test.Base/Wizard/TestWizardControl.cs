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
using System.Collections.Generic;
using Habanero.Faces.Base;


using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Test.Base.Wizard
{
    public abstract class TestWizardControl
    {
        protected abstract IControlFactory GetControlFactory();

        protected abstract IWizardControllerStub CreateWizardControllerStub();
        protected abstract IWizardStepStub CreateWizardStepStub();

        #region Testing Wizard Class Interfaces

        public interface IWizardStepStub : IWizardStep
        {
            bool AllowMoveOn { get; set; }
            bool IsInitialised { get; }
            bool AllowMoveBack { get; set; }
            new string HeaderText { get; set; }
            bool UndoMoveOnWasCalled { get; set; }
            bool MoveOnWasCalled { get; set; }
        }

        public interface IWizardControllerStub : IWizardController
        {
            IWizardStepStub ControlForStep1 { get; }
            bool CancelButtonEventFired { get; }
            IWizardStepStub ControlForStep2 { get; }
            bool FinishCalled { get; }
            void ForTestingAddWizardStep(IWizardStep wizardStepStub);
        }

        protected internal class WizardControllerStub<T> : IWizardControllerStub where T : IWizardStepStub, new()
        {
            private readonly List<IWizardStep> _wizardSteps;

            public IWizardStepStub ControlForStep1 { get; set; }

            public IWizardStepStub ControlForStep2 { get; set; }

            public bool FinishCalled { get; set; }

            public WizardControllerStub()
            {
                CancelButtonEventFired = false;
                CurrentStep = -1;
                ControlForStep2 = new T();
                ControlForStep1 = new T();
                FinishCalled = false;
                _wizardSteps = new List<IWizardStep>();
                ControlForStep1.Name = "ControlForStep1";
                ControlForStep2.Name = "ControlForStep2";
                ControlForStep2.HeaderText = "This is wizard step 2";
                _wizardSteps.Add(ControlForStep1);
                _wizardSteps.Add(ControlForStep2);

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
                FinishCalled = false;
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
                if (IsLastStep())
                    FinishCalled = true;
                else throw new WizardStepException("Invalid call to Finish(), not at last step");
            }

            public bool CanMoveOn(out string message)
            {
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

            public bool CancelButtonEventFired { get; private set; }

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
                this.CancelButtonEventFired = true;
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

            public void ForTestingAddWizardStep(IWizardStep step)
            {
                _wizardSteps.Add(step);
            }
        }


        #endregion // Testing Wizard Class Interfaces



        private IWizardControllerStub _controller;
        private IWizardControl _wizardControl;

        private string _message;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            _controller = CreateWizardControllerStub();
            _wizardControl = GetControlFactory().CreateWizardControl(_controller);// new WizardControl(_controller);
        }

        [SetUp]
        public void SetupTest()
        {
            _controller.ControlForStep1.AllowMoveOn = true;
            _message = "";
            _wizardControl.Start();

        }

        //TODO: Tests for layout management?
        [TearDown]
        public void TearDownTest()
        {
        }

        [Test]
        public void TestConstructWizardControl()
        {
            //---------------Set up test pack-------------------
            IWizardControllerStub wizardController = CreateWizardControllerStub();

            //---------------Execute Test ----------------------
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            SetWizardControlSize(wizardControl);

            //---------------Test Result -----------------------
            Assert.IsNotNull(wizardControl.PreviousButton);
            Assert.IsNotNull(wizardControl.NextButton);
            Assert.IsNotNull(wizardControl.CancelButton);
            Assert.Less(wizardControl.PreviousButton.Left, wizardControl.NextButton.Left);
            Assert.Less(wizardControl.NextButton.Left, wizardControl.CancelButton.Left);
            Assert.AreEqual(0, wizardControl.PreviousButton.TabIndex);
            Assert.AreEqual(1, wizardControl.NextButton.TabIndex);
            Assert.AreEqual(wizardControl.Height - wizardControl.NextButton.Height - 62, wizardControl.WizardStepPanel.Height);
        }

        [Test]
        public void TestCancelFiresEvent()
        {
            //---------------Set up test pack-------------------
            IWizardControllerStub wizardController = CreateWizardControllerStub();

            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);

            //--------------Assert PreConditions----------------            
            Assert.IsFalse(wizardController.CancelButtonEventFired);
            //---------------Execute Test ----------------------

            wizardControl.CancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(wizardController.CancelButtonEventFired);

        }

        private static void SetWizardControlSize(IWizardControl wizardControl)
        {
            wizardControl.Width = 310;
            wizardControl.Height = 412;
        }

        [Test]
        public void TestStart()
        {
            //Setup -----------------------------------------------------
            IWizardControllerStub wizardController = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            //Execute ---------------------------------------------------
            wizardControl.Start();
            //Assert Results --------------------------------------------
            Assert.AreEqual("ControlForStep1", wizardControl.CurrentControl.Name);
            Assert.AreEqual(wizardController.ControlForStep1.Name, wizardControl.CurrentControl.Name);

        }

        [Test]
        public void TestHeaderLabelEnabledWhen_WizardStepTextSet()
        {
            //---------------Set up test pack-------------------
            IWizardControllerStub wizardController = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            SetWizardControlSize(wizardControl);
            wizardControl.Start();
            //--------------Assert PreConditions----------------            
            Assert.AreEqual("ControlForStep1", wizardControl.CurrentControl.Name);
            //Assert.IsFalse(wizardControl.HeadingLabel.Visible);
            Assert.AreEqual(wizardControl.Height - wizardControl.NextButton.Height - 62, wizardControl.WizardStepPanel.Height);
            //---------------Execute Test ----------------------
            wizardControl.Next();
            //---------------Test Result -----------------------
            IWizardStep currentStep = wizardController.GetCurrentStep();
            Assert.AreEqual("ControlForStep2", wizardControl.CurrentControl.Name);
            Assert.AreSame(currentStep, wizardControl.CurrentControl);
            Assert.IsTrue(((IWizardStepStub)currentStep).IsInitialised);
            //Assert.IsTrue(wizardControl.HeadingLabel.Visible);
            //Assert.IsTrue(wizardControl.HeadingLabel.Text.Length > 0);
            //Assert.AreEqual(step.HeaderText, wizardControl.HeadingLabel.Text);
            //Assert.AreEqual(wizardControl.Height - wizardControl.NextButton.Height - 62 - wizardControl.HeadingLabel.Height, wizardControl.WizardStepPanel.Height);
        }
        [Test]
        public void TestHeaderLabelDisabledWhen_WizardStepTextSetBackToNull()
        {
            //---------------Set up test pack-------------------
            IWizardControllerStub wizardController = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            SetWizardControlSize(wizardControl);
            wizardControl.Start();
            wizardControl.Next();
            //--------------Assert PreConditions----------------            
            wizardController.GetCurrentStep();
            Assert.AreEqual("ControlForStep2", wizardControl.CurrentControl.Name);
            //Assert.IsTrue(wizardControl.HeadingLabel.Visible); //removed the label and am now putting the header on the form
            // due to problems with giz hiding the some wizard controls that where double clicked
            //Assert.IsTrue(wizardControl.HeadingLabel.Text.Length > 0);
            //Assert.AreEqual(step.HeaderText, wizardControl.HeadingLabel.Text);
            //Assert.AreEqual(wizardControl.Height - wizardControl.NextButton.Height - 62 - wizardControl.HeadingLabel.Height, wizardControl.WizardStepPanel.Height);

            //---------------Execute Test ----------------------

            wizardControl.Previous();
            //---------------Test Result -----------------------
            wizardController.GetCurrentStep();
            Assert.AreEqual("ControlForStep1", wizardControl.CurrentControl.Name);
            //Assert.IsFalse(wizardControl.HeadingLabel.Visible);
            //Assert.IsFalse(wizardControl.HeadingLabel.Text.Length > 0);
            //Assert.AreEqual(step.HeaderText, wizardControl.HeadingLabel.Text);
            Assert.AreEqual(wizardControl.Height - wizardControl.NextButton.Height - 62, wizardControl.WizardStepPanel.Height);
        }

        //TODO: Tab indexes are not being set up correctly in Giz with the flow layout manager
        // right alignment
        [Test]
        public void Test_SetWizardController_CallsStart()
        {
            //Setup ----------------------------------------------------
            IWizardControllerStub wizardController = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(_controller);
            wizardControl.Width = 300;
            //Execute ---------------------------------------------------
            wizardControl.WizardController = wizardController;
            //Assert Results --------------------------------------------
            Assert.AreEqual("ControlForStep1", wizardControl.CurrentControl.Name);
            Assert.AreEqual(wizardController.ControlForStep1.Name, wizardControl.CurrentControl.Name);
            Assert.AreEqual(0, wizardControl.PreviousButton.TabIndex);
            Assert.AreEqual(1, wizardControl.NextButton.TabIndex);
        }
        [Test]
        public void Test_Next_ShouldSetStep2()
        {
            //Execute ---------------------------------------------------
            _wizardControl.Next();
            //Assert Results --------------------------------------------
            Assert.AreSame(_controller.ControlForStep2, _wizardControl.CurrentControl);
        }
//        [Test]
//        public void Test_UndoCurrentStep_ShouldCallStepMoveBack()
//        {
            //---------------Set up test pack-------------------
//            WizardController wizardController = new WizardController();
//            var step1 = MockRepository.GenerateMock<IWizardStep>();
//            wizardController.AddStep(step1);
//            wizardController.GetFirstStep();
            //---------------Assert Precondition----------------
//            Assert.AreEqual(1, wizardController.StepCount);
//            step1.AssertWasNotCalled(step => step.UndoMoveOn());
//            Assert.AreSame(step1, wizardController.GetCurrentStep());
            //---------------Execute Test ----------------------
//            wizardController.UndoCompleteCurrentStep();
            //---------------Test Result -----------------------
//            step1.AssertWasCalled(wizardStep => wizardStep.UndoMoveOn());
//        }
        [Test]
        public void Test_Previous_ShouldCallWizardControllerUndo()
        {
            //---------------Set up test pack-------------------
            IWizardController controller = MockRepository.GenerateMock<IWizardController>();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(controller);
            string message;
            controller.Stub(wizardController => wizardController.CanMoveOn(out message)).Return(true);
            controller.Stub(controller1 => controller1.GetPreviousStep()).Return(CreateWizardStepStub());
            //---------------Assert Precondition----------------
            Assert.IsTrue(controller.CanMoveOn(out message));
            controller.AssertWasNotCalled(cntrler => cntrler.UndoCompleteCurrentStep());
            //---------------Execute Test ----------------------
            wizardControl.Previous();
            //---------------Test Result -----------------------
            controller.AssertWasCalled(cntrler => cntrler.UndoCompleteCurrentStep());
        }

        [Test]
        public void Test_Previous_ShouldCallUndoMoveOnForPreviousStep()
        {
            //---------------Set up test pack-------------------
            WizardController controller = new WizardController();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(controller);
            var step1 = CreateWizardStepStub();
            controller.AddStep(step1);
            var step2 = CreateWizardStepStub();
            controller.AddStep(step2);
            step1.AllowMoveOn = true;
            step2.AllowMoveBack = true;
            controller.GetFirstStep();
            controller.GetNextStep();
            //---------------Assert Precondition----------------
            Assert.IsTrue(controller.CanMoveBack());
            Assert.AreSame(step2, controller.GetCurrentStep());
            Assert.IsFalse(step1.UndoMoveOnWasCalled);
            Assert.IsFalse(step2.UndoMoveOnWasCalled);
            //---------------Execute Test ----------------------
            wizardControl.Previous();
            //---------------Test Result -----------------------
            Assert.AreSame(step1, controller.GetCurrentStep());
            Assert.IsTrue(step1.UndoMoveOnWasCalled);
            Assert.IsFalse(step2.UndoMoveOnWasCalled);
        }
        [Test]
        public void Test_Next_ShouldCallWizardControllerNext()
        {
            //---------------Set up test pack-------------------
            IWizardController controller = MockRepository.GenerateMock<IWizardController>();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(controller);
            string message;
            controller.Stub(wizardController => wizardController.CanMoveOn(out message)).Return(true);
            controller.Stub(controller1 => controller1.GetNextStep()).Return(CreateWizardStepStub());
            //---------------Assert Precondition----------------
            Assert.IsTrue(controller.CanMoveOn(out message));
            controller.AssertWasNotCalled(cntrler => cntrler.CompleteCurrentStep());
            //---------------Execute Test ----------------------
            wizardControl.Next();
            //---------------Test Result -----------------------
            controller.AssertWasCalled(cntrler => cntrler.CompleteCurrentStep());
        }
        [Test]
        public void Test_Previous_WhenStep2_ShouldReturnStep1()
        {
            //Setup ----------------------------------------------------
            _wizardControl.Next();
            //Execute ---------------------------------------------------
            _wizardControl.Previous();
            //Assert Results --------------------------------------------
            Assert.AreSame(_controller.ControlForStep1, _wizardControl.CurrentControl);
        }

        [Test]
        public void Test_NextWithNoNextStep()
        {
            //Setup ----------------------------------------------------
            _wizardControl.Next();
            //Execute ---------------------------------------------------
            try
            {
                _wizardControl.Next();
                Assert.Fail("Expected to throw an WizardStepException");
            }
                //---------------Test Result -----------------------
            catch (WizardStepException ex)
            {
                StringAssert.Contains("Invalid Wizard Step: 2", ex.Message);
            }
        }

        [Test]
        public void Test_PreviousWithNoNextStep_ShouldRaiseError()
        {
            //Execute ---------------------------------------------------
            try
            {
                _wizardControl.Previous();
                Assert.Fail("Expected to throw an WizardStepException");
            }
                //---------------Test Result -----------------------
            catch (WizardStepException ex)
            {
                StringAssert.Contains("Invalid Wizard Step: -1", ex.Message);
            }
        }

        [Test]
        public void Test_Click_NextButton_ShouldSetStep2()
        {
            //Execute ---------------------------------------------------
            _wizardControl.NextButton.PerformClick();
            //Assert Results --------------------------------------------
            Assert.AreSame(_controller.ControlForStep2, _wizardControl.CurrentControl);
        }

        [Test]
        public void Test_ClickPreviousButton_WhenStep2_ShouldReturnStep1()
        {
            _wizardControl.Next();
            //Execute ---------------------------------------------------
            _wizardControl.PreviousButton.PerformClick();
            //Assert Results --------------------------------------------
            Assert.AreSame(_controller.ControlForStep1, _wizardControl.CurrentControl);
        }

        [Test]
        public void Test_NextButtonText_ShouldChangeWhenLastStep()
        {
            //Execute ---------------------------------------------------
            _wizardControl.Next();
            //Assert Results --------------------------------------------
            Assert.AreEqual("Finish", _wizardControl.NextButton.Text);
            //Execute ---------------------------------------------------
            _wizardControl.Previous();
            //Assert Results --------------------------------------------
            Assert.AreEqual("Next", _wizardControl.NextButton.Text);
        }

        [Test]
        public void Test_PreviousButtonDisabledAtStart()
        {
            Assert.IsFalse(_wizardControl.PreviousButton.Enabled);
        }

        [Test]
        public void Test_PreviousButtonEnabledAfterStart()
        {
            //--------------setup-----------------
            this._controller.ControlForStep2.AllowMoveBack = true;
            //---------------Execute-------------
            _wizardControl.Next();
            //Assert Results --------------------------------------------
            Assert.IsTrue(_wizardControl.PreviousButton.Enabled);
        }

        [Test]
        public void Test_PreviousButtonDisabled_ReturnToFirstStep()
        {
            _wizardControl.Next();
            //Execute ---------------------------------------------------
            _wizardControl.Previous();
            //Assert Results --------------------------------------------
            Assert.IsFalse(_wizardControl.PreviousButton.Enabled);
        }

        [Test]
        public void Test_CallFinish_WhenAtLastStep_ShouldRaiseEvent()
        {
            //---------------Set up test pack-------------------
            bool finished = false;
            _wizardControl.Finished += delegate { finished = true; };
            //---------------Execute Test ----------------------
            _wizardControl.Next();
            _wizardControl.Finish();
            //---------------Test Result -----------------------
            Assert.IsTrue(_controller.FinishCalled);
            Assert.IsTrue(finished);
        }

        [Test]
        public void TestFinishAtNonFinishStep()
        {
            //---------------Execute Test ----------------------
            try
            {
                _wizardControl.Finish();
                Assert.Fail("Expected to throw an WizardStepException");
            }
                //---------------Test Result -----------------------
            catch (WizardStepException ex)
            {
                StringAssert.Contains("Invalid call to Finish(), not at last step", ex.Message);
            }
        }

        [Test]
        public void Test_NextClick_WhenAtLastStep_ShouldCallFinish()
        {
            //---------------Set up test pack-------------------
            _wizardControl.Next();
            //-=----------Assert preconditions ----------------------------
            Assert.IsFalse(_controller.FinishCalled);

            //Execute ---------------------------------------------------
            _wizardControl.NextButton.PerformClick();
            //---------------Assert result -----------------------------------

            Assert.IsTrue(_controller.FinishCalled);
        }

        [Test]
        public void Test_NextClickAtLastStep_WhenCanMoveOnfalse_DoesNotFinish()
        {
            //---------------Set up test pack-------------------
            IWizardControllerStub controller = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(controller);// new WizardControl(_controller);
            controller.ControlForStep1.AllowMoveOn = true;
            controller.ControlForStep2.AllowMoveOn = false;

            wizardControl.Start();
            wizardControl.NextButton.PerformClick();
            //---------------Execute Test ----------------------
            wizardControl.NextButton.PerformClick();
            //---------------Test Result -----------------------
            // Finish should not have been called because CanMoveOn on step two is returning false and should prevent
            // the wizard from finishing.
            Assert.IsFalse(controller.FinishCalled);
        }

        [Test]
        public void TestFinishEventPosted()
        {
            //---------------Set up test pack-------------------
            _wizardControl.Next();
            bool finishEventPosted = false;
            _wizardControl.Finished += delegate { finishEventPosted = true; };
            //-=----------Assert preconditions ----------------------------
            Assert.IsFalse(finishEventPosted);
            //Execute ---------------------------------------------------
            _wizardControl.NextButton.PerformClick();
            //---------------Assert result -----------------------------------
            Assert.IsTrue(finishEventPosted);

        }
        [Test]
        public void Test_NextWhen_CanMoveOn_False_TestMessagPostedEventCalled()
        {
            //---------------Setup wizard Control -------------------------------
            _wizardControl.MessagePosted += delegate(string message) { _message = message; };
            _controller.ControlForStep1.AllowMoveOn = false;
            //---------------Execute Test ------------------------------------
            _wizardControl.Next();
            //---------------Assert result -----------------------------------
            Assert.AreSame(_controller.ControlForStep1, _wizardControl.CurrentControl);
            Assert.AreEqual("Sorry, can't move on", _message);

        }

        [Test]
        public void Test_PreviousButtonDisabledIfCanMoveBackFalse()
        {
            //---------------Set up test pack-------------------
            IWizardControllerStub wizardController = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            wizardController.ControlForStep2.AllowMoveBack = false;
            wizardControl.Start();

            //---------------Assert Preconditions ----------------------
            Assert.IsFalse(wizardController.ControlForStep2.CanMoveBack());
            //---------------Execute Test ----------------------
            wizardControl.Next();
            //---------------Assert result -----------------------
            Assert.AreSame(wizardControl.CurrentControl, wizardController.ControlForStep2);
            Assert.IsFalse(((IWizardStepStub)wizardControl.CurrentControl).AllowMoveBack);
            Assert.IsFalse(wizardControl.PreviousButton.Enabled);
        }


        [Test]
        public void Test_PreviousButtonDisabledIfCanMoveBackFalse_FromPreviousTep()
        {
            //TODO: setup with 3 steps set step 2 allow move back false
            //and go next next next previous and then ensure that canMoveBack false
            //---------------Set up test pack-------------------
            IWizardControllerStub wizardController = CreateWizardControllerStub();
            wizardController.ForTestingAddWizardStep(CreateWizardStepStub());

            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            wizardController.ControlForStep2.AllowMoveBack = false;
            wizardControl.Start();

            //---------------Assert Preconditions ----------------------
            Assert.IsFalse(wizardController.ControlForStep2.CanMoveBack());
            //---------------Execute Test ----------------------
            wizardControl.Next();
            wizardControl.Next();
            wizardControl.Previous();
            //---------------Assert result -----------------------
            Assert.AreSame(wizardControl.CurrentControl, wizardController.ControlForStep2);
            Assert.IsFalse(((IWizardStepStub)wizardControl.CurrentControl).AllowMoveBack);
            Assert.IsFalse(wizardControl.PreviousButton.Enabled);
        }

        [Test]
        public void Test_SetStepResizesControl()
        {
            //---------------Set up test pack-------------------
            IWizardControllerStub wizardController = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            wizardControl.Start();
            wizardController.ControlForStep2.Width = 10;

            //--------------Assert PreConditions----------------            
            string msg;
            Assert.IsTrue(wizardController.CanMoveOn(out msg));

            //---------------Execute Test ----------------------
            wizardControl.Next();

            //---------------Test Result -----------------------
            Assert.AreEqual(wizardControl.Width - WizardControl.PADDING * 2, wizardController.ControlForStep2.Width);
        }

        [Test, Ignore("The test is visually working but the tests are not picking up a change in width : June 2008")]
        public void Test_NextPreviousIn_theCorrectOrder()
        {
            //---------------Set up test pack-------------------
            IWizardControllerStub wizardController = CreateWizardControllerStub();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);

            //---------------Test Result -----------------------
            Assert.Less(wizardControl.NextButton.Left, wizardControl.PreviousButton.Left);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_Next_ShouldCallMoveNext()
        {
            //---------------Set up test pack-------------------
            IWizardControllerStub controller = CreateWizardControllerStub();
            var step1 = controller.ControlForStep1;
            step1.AllowMoveOn = true;
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(controller);
            controller.GetFirstStep();
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, controller.StepCount);
            Assert.IsFalse(step1.MoveOnWasCalled);
            Assert.AreSame(step1, controller.GetCurrentStep());
            //---------------Execute Test ----------------------
            wizardControl.Next();
            //---------------Test Result -----------------------
            Assert.IsTrue(step1.MoveOnWasCalled);
        }
    }
}
