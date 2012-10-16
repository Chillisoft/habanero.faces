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

        protected abstract IWizardControllerSpy CreateWizardControllerStub();
        protected abstract IWizardStepStub CreateWizardStepStub();




        private IWizardControllerSpy _controller;
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
            IWizardControllerSpy wizardController = CreateWizardControllerStub();

            //---------------Execute Test ----------------------
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            SetWizardControlSize(wizardControl);

            //---------------Test Result -----------------------
            Assert.IsNotNull(wizardControl.PreviousButton);
            Assert.IsNotNull(wizardControl.NextButton);
            Assert.IsNotNull(wizardControl.CancelButton);
            Assert.IsNotNull(wizardControl.FinishButton);
            Assert.Less(wizardControl.FinishButton.Left, wizardControl.CancelButton.Left);
            Assert.Less(wizardControl.NextButton.Left, wizardControl.FinishButton.Left);
            Assert.Less(wizardControl.PreviousButton.Left, wizardControl.NextButton.Left);
            Assert.Less(wizardControl.NextButton.Left, wizardControl.CancelButton.Left);
            Assert.AreEqual(0, wizardControl.PreviousButton.TabIndex);
            Assert.AreEqual(1, wizardControl.NextButton.TabIndex);

            Assert.AreEqual(wizardControl.Height, wizardControl.NextButton.Height + wizardControl.WizardStepPanel.Height + 10);
        }

        [Test]
        public void TestCancelFiresEvent()
        {
            //---------------Set up test pack-------------------
            IWizardControllerSpy wizardController = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            //--------------Assert PreConditions----------------            
            Assert.IsFalse(wizardController.CancelWizardWasCalled);
            //---------------Execute Test ----------------------
            wizardControl.CancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(wizardController.CancelWizardWasCalled);

        }
        [Test]
        public void TestFinishButton_WhenClicked_WhenCanMoveOn_ShouldFireFinishEvent()
        {
            //---------------Set up test pack-------------------
            IWizardControllerSpy wizardController = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            wizardController.AllowCanMoveOn = true;
            wizardControl.Start();
            //--------------Assert PreConditions----------------            
            Assert.IsFalse(wizardController.FinishWizardWasCalled);
            string message;
            Assert.IsTrue(wizardController.CanMoveOn(out message));
            //---------------Execute Test ----------------------
            wizardControl.Finish();
            //---------------Test Result -----------------------
            Assert.IsTrue(wizardController.FinishWizardWasCalled, "Finish should be called");
        }

        [Test]
        public void TestFinish_WhenNotCanMoveOn_ShouldNotFireFinishEventAndShouldPostMessage()
        {
            //---------------Set up test pack-------------------
            IWizardControllerSpy wizardController = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            string message;
            
            wizardController.AllowCanMoveOn = false;
            wizardControl.Start();
            //--------------Assert PreConditions----------------            
            Assert.IsFalse(wizardController.FinishWizardWasCalled);
            Assert.IsFalse(wizardController.CanMoveOn(out message));
            message = "";
            //---------------Execute Test ----------------------
            wizardControl.MessagePosted += delegate(string messagePosted) { message = messagePosted; };
            wizardControl.Finish();
            //---------------Test Result -----------------------
            Assert.IsFalse(wizardController.FinishWizardWasCalled, "Finish should be called");
            Assert.AreEqual("AllowCanMoveOnFalse", message);
        }

        private static void SetWizardControlSize(IWizardControl wizardControl)
        {
            wizardControl.Width = 500;
            wizardControl.Height = 412;
        }

        [Test]
        public void TestStart()
        {
            //Setup -----------------------------------------------------
            IWizardControllerSpy wizardController = CreateWizardControllerStub();
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
            IWizardControllerSpy wizardController = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            SetWizardControlSize(wizardControl);
            wizardControl.Start();
            //--------------Assert PreConditions----------------            
            Assert.AreEqual("ControlForStep1", wizardControl.CurrentControl.Name);
            //Assert.IsFalse(wizardControl.HeadingLabel.Visible);
            //Assert.AreEqual(wizardControl.Height - wizardControl.NextButton.Height - 62, wizardControl.WizardStepPanel.Height);
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
            // this test doesn't do what it says?!
            //---------------Set up test pack-------------------
            IWizardControllerSpy wizardController = CreateWizardControllerStub();
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
            //Assert.AreEqual(wizardControl.Height - wizardControl.NextButton.Height - 62, wizardControl.WizardStepPanel.Height);
        }

        //TODO: Tab indexes are not being set up correctly in VWG with the flow layout manager
        // right alignment
        [Test]
        public void Test_SetWizardController_CallsStart()
        {
            //Setup ----------------------------------------------------
            IWizardControllerSpy wizardController = CreateWizardControllerStub();
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
        public void Test_Previous_WhenLastStep_WhenCanFinishOnSecondStep_ShouldEnableNextButtonAndFinishButton()
        {
            //---------------Set up test pack-------------------
            IWizardControllerSpy controller = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(controller);
            var step1 = controller.ControlForStep1;
            var step2 = controller.ControlForStep2;
            var step3 = controller.ControlForStep3;
            step1.AllowMoveOn = true;
            step3.AllowMoveBack = true;
            step2.AllowFinish = true;
            controller.GetFirstStep();
            MoveToLastStep(wizardControl);
            //---------------Assert Precondition----------------
            Assert.IsTrue(controller.CanMoveBack());
            Assert.AreSame(step3, controller.GetCurrentStep());

            Assert.IsTrue(step2.CanFinish());
            Assert.IsTrue(controller.IsLastStep(), "Should be last step");
            Assert.IsTrue(wizardControl.FinishButton.Enabled, "Last step so finish enabled");
            Assert.IsFalse(wizardControl.NextButton.Enabled, "last step so next disabled");
            //---------------Execute Test ----------------------
            wizardControl.Previous();
            //---------------Test Result -----------------------
            Assert.IsFalse(controller.IsLastStep(), "Should be last step");
            Assert.AreSame(step2, controller.GetCurrentStep());

            Assert.IsTrue(wizardControl.FinishButton.Enabled, "Not Last step But CanFinish True so finish Enabled");
            Assert.IsTrue(wizardControl.NextButton.Enabled, "Not last step so Next Enabled");
        }
        [Test]
        public void Test_Previous_WhenLastStep_WhenNotCanFinishOnSecondStep_ShouldEnableNextButtonAndDisableFinishButton()
        {
            //---------------Set up test pack-------------------
            IWizardControllerSpy controller = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(controller);
            var step1 = controller.ControlForStep1;
            var step2 = controller.ControlForStep2;
            var step3 = controller.ControlForStep3;
            step1.AllowMoveOn = true;
            step3.AllowMoveBack = true;
            step2.AllowFinish = false;
            controller.GetFirstStep();
            MoveToLastStep(wizardControl);
            //---------------Assert Precondition----------------
            Assert.IsTrue(controller.CanMoveBack());
            Assert.AreSame(step3, controller.GetCurrentStep());

            Assert.IsFalse(step2.CanFinish());
            Assert.IsTrue(controller.IsLastStep(), "Should be last step");
            Assert.IsTrue(wizardControl.FinishButton.Enabled, "Last step so finish enabled");
            Assert.IsFalse(wizardControl.NextButton.Enabled, "last step so next disabled");
            //---------------Execute Test ----------------------
            wizardControl.Previous();
            //---------------Test Result -----------------------
            Assert.IsFalse(controller.IsLastStep(), "Should be last step");
            Assert.AreSame(step2, controller.GetCurrentStep());
            Assert.IsFalse(wizardControl.FinishButton.Enabled, "Not Last step and CanFinish False so finish disabled");
            Assert.IsTrue(wizardControl.NextButton.Enabled, "Not last step so Next Enabled");
        }

        private void MoveToLastStep(IWizardControl wizardControl)
        {
            wizardControl.Next();
            wizardControl.Next();
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
                StringAssert.Contains("Invalid Wizard Step: ", ex.Message);
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

       /* [Test]
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
        }*/

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
            Assert.IsTrue(_controller.FinishWizardWasCalled);
            Assert.IsTrue(finished);
        }

        [Test]
        public void Test_FinishClick_WhenAtLastStep_ShouldCallFinish()
        {
            //---------------Set up test pack-------------------
            _wizardControl.Next();
            _wizardControl.Next();
            //-=----------Assert preconditions ----------------------------

            Assert.IsFalse(_controller.FinishWizardWasCalled);
            //Execute ---------------------------------------------------
            _wizardControl.FinishButton.PerformClick();
            //---------------Assert result -----------------------------------
            Assert.IsTrue(_controller.FinishWizardWasCalled, "Clicking the Finish Button should cause Finish Wizard to be called");
        }
        [Test]
        public void Test_NextClick_WhenAtLastStep_ShouldNotCallFinish()
        {
            //---------------Set up test pack-------------------
            _wizardControl.Next();
            _wizardControl.Next();
            //-=----------Assert preconditions ----------------------------
            Assert.IsTrue(_controller.IsLastStep());
            Assert.IsFalse(_controller.FinishWizardWasCalled);
            //Execute ---------------------------------------------------
            try
            {
                _wizardControl.Next();
                Assert.Fail("Expected to throw an WizardStepException");
            }
                //---------------Test Result -----------------------
            catch (WizardStepException ex)
            {
                StringAssert.Contains("Invalid Wizard Step", ex.Message);
            }
        }

        [Test]
        public void Test_NextClickAtLastStep_WhenCanMoveOnfalse_DoesNotFinish()
        {
            //---------------Set up test pack-------------------
            IWizardControllerSpy controller = CreateWizardControllerStub();
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
            Assert.IsFalse(controller.FinishWizardWasCalled);
        }

        [Test]
        public void TestFinishEventPosted()
        {
            //---------------Set up test pack-------------------
            _wizardControl.Next();
            _wizardControl.Next();
            bool finishEventPosted = false;
            _wizardControl.Finished += delegate { finishEventPosted = true; };
            //-=----------Assert preconditions ----------------------------
            Assert.IsFalse(finishEventPosted);
            Assert.IsNotNull(_wizardControl.WizardController);
            Assert.IsTrue(_wizardControl.WizardController.IsLastStep());
            //Execute ---------------------------------------------------
            _wizardControl.FinishButton.PerformClick();
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
        public void Test_FinishButton_WhenCanFinishFalse_ShouldBeDisabled()
        {
            //---------------Set up test pack-------------------
            IWizardControllerSpy wizardController = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            wizardController.ControlForStep2.AllowFinish = false;
            wizardControl.Start();

            //---------------Assert Preconditions ----------------------
            Assert.IsFalse(wizardController.ControlForStep1.AllowFinish);
            Assert.IsFalse(wizardController.ControlForStep1.CanFinish());
            //---------------Execute Test ----------------------
            wizardControl.Next();
            //---------------Assert result -----------------------
            Assert.AreSame(wizardControl.CurrentControl, wizardController.ControlForStep2);
            Assert.IsFalse(wizardController.IsLastStep());
            Assert.IsFalse(wizardControl.FinishButton.Enabled, "Finish button should be disabled");
        }

        [Test]
        public void Test_FinishButton_WhenCanFinishFalse_WhenFromPreviousStep_ShouldBeDisabled()
        {
            //---------------Set up test pack-------------------
            IWizardControllerSpy wizardController = CreateWizardControllerStub();
            wizardController.ForTestingAddWizardStep(CreateWizardStepStub());
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            wizardController.ControlForStep2.AllowFinish = false;
            wizardControl.Start();
            //---------------Assert Preconditions ----------------------
            Assert.IsFalse(wizardController.ControlForStep2.AllowFinish);
            Assert.IsFalse(wizardController.ControlForStep2.CanFinish());
            //---------------Execute Test ----------------------
            wizardControl.Next();
            wizardControl.Next();
            wizardControl.Previous();
            //---------------Assert result -----------------------
            Assert.AreSame(wizardControl.CurrentControl, wizardController.ControlForStep2);
            Assert.IsFalse(wizardControl.FinishButton.Enabled);
        }

        [Test]
        public void Test_FinishButton_WhenCanFinishTrue_ShouldBeEnabled()
        {
            //---------------Set up test pack-------------------
            IWizardControllerSpy wizardController = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            wizardController.ControlForStep2.AllowFinish = true;
            wizardControl.Start();
            //---------------Assert Preconditions ----------------------
            Assert.IsTrue(wizardController.ControlForStep2.AllowFinish);
            Assert.IsTrue(wizardController.ControlForStep2.CanFinish());
            //---------------Execute Test ----------------------
            wizardControl.Next();
            //---------------Assert result -----------------------
            Assert.AreSame(wizardControl.CurrentControl, wizardController.ControlForStep2);
            Assert.IsTrue(wizardControl.FinishButton.Enabled, "Finish button should be enabled");
        }

        [Test]
        public void Test_FinishButton_WhenAtLastStep_ShouldBeEnabled()
        {
            //---------------Set up test pack-------------------
            IWizardControllerSpy wizardController = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            wizardController.ControlForStep3.AllowFinish = true;
            wizardControl.Start();
            wizardControl.Next();
            wizardControl.Next();
            //---------------Assert Preconditions ----------------------
            Assert.IsTrue(wizardController.IsLastStep());
            //---------------Execute Test ----------------------
            wizardControl.Finish();
            //---------------Test Result -----------------------
            Assert.IsTrue(wizardControl.FinishButton.Enabled);
        }

        [Test]
        public void Test_NextButton_WhenAtLastStep_ShouldBeDisabled()
        {
            //---------------Set up test pack-------------------
            IWizardControllerSpy wizardController = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            wizardController.ControlForStep3.AllowFinish = true;
            wizardControl.Start();
            wizardControl.Next();
            //---------------Assert Preconditions ----------------------
            Assert.IsFalse(wizardController.IsLastStep(), "Should not be last step");
            Assert.IsTrue(wizardControl.NextButton.Enabled, "If this is not the last step should be enabled");
            //---------------Execute Test ----------------------
            wizardControl.Next();
            //---------------Test Result -----------------------
            Assert.IsTrue(wizardController.IsLastStep(), "Should be last step");
            Assert.IsFalse(wizardControl.NextButton.Enabled, "Should disable next button and enable Finish button at last step");
            Assert.IsTrue(wizardControl.FinishButton.Enabled, "Should enable Finish button at last step");
        }

        [Test]
        public void Test_PreviousButtonDisabledIfCanMoveBackFalse()
        {
            //---------------Set up test pack-------------------
            IWizardControllerSpy wizardController = CreateWizardControllerStub();
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
            IWizardControllerSpy wizardController = CreateWizardControllerStub();
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
            IWizardControllerSpy wizardController = CreateWizardControllerStub();
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
            IWizardControllerSpy wizardController = CreateWizardControllerStub();
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
            IWizardControllerSpy controller = CreateWizardControllerStub();
            var step1 = controller.ControlForStep1;
            step1.AllowMoveOn = true;
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(controller);
            controller.GetFirstStep();
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, controller.StepCount);
            Assert.IsFalse(step1.MoveOnWasCalled);
            Assert.AreSame(step1, controller.GetCurrentStep());
            //---------------Execute Test ----------------------
            wizardControl.Next();
            //---------------Test Result -----------------------
            Assert.IsTrue(step1.MoveOnWasCalled);
        }


        [Test]
        public void Test_CancelButton_WhenCanCancelFalse_ShouldBeDisabled()
        {
            //---------------Set up test pack-------------------
            IWizardControllerSpy wizardController = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            wizardController.AllowCancel = false;
            wizardControl.Start();

            //---------------Assert Preconditions ----------------------
            Assert.IsFalse(wizardController.CanCancel());
            //---------------Execute Test ----------------------
            var cancelEnabled = wizardControl.CancelButton.Enabled;
            //---------------Assert result -----------------------
            Assert.IsFalse(cancelEnabled, "Cancel should be disabled");
        }


        [Test]
        public void Test_CancelButton_WhenCanCancelTrue_ShouldBeEnabled()
        {
            //---------------Set up test pack-------------------
            IWizardControllerSpy wizardController = CreateWizardControllerStub();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(wizardController);
            wizardController.AllowCancel = true;
            wizardControl.Start();
            //---------------Assert Preconditions ----------------------
            Assert.IsTrue(wizardController.CanCancel());
            //---------------Execute Test ----------------------
            var cancelEnabled = wizardControl.CancelButton.Enabled;
            //---------------Assert result -----------------------
            Assert.IsTrue(cancelEnabled, "Cancel should b enabled");
        }
    }
}
