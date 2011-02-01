using System;
using Habanero.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;
using Rhino.Mocks;
// ReSharper disable InconsistentNaming
namespace Habanero.Faces.Test.Win.HabaneroControls
{
    [TestFixture]
    public class TestCloseBOEditorDialogWin
    {
        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
        }

        [SetUp]
        public void Setup()
        {
        }

        protected virtual ControlFactoryWin CreateControlFactoryWin()
        {
            return new ControlFactoryWin();
        }

        protected virtual CloseBOEditorDialogWin CreateDialogBox(IControlFactory factory)
        {
            return new CloseBOEditorDialogWinFake(factory);
        }
        protected virtual CloseBOEditorDialogWin CreateDialogBoxWithDisplayName(IControlFactory factory)
        {
            return new CloseBOEditorDialogWinFake(factory);
        }

        protected virtual ICloseBOEditorDialog CreateDialogBox()
        {
            IControlFactory factory = CreateControlFactoryWin();
            //var businessObject = CreateMockBO();
            return CreateDialogBox(factory);
        }

        private static IBusinessObject CreateMockBO()
        {
            return CreateMockBO(true, true, true);
        }

        private static IBusinessObject CreateMockBO(bool isNew, bool isDirty, bool isValid)
        {
            var businessObject = MockRepository.GenerateMock<IBusinessObject>();
            var boStatus = MockRepository.GenerateMock<IBOStatus>();
            boStatus.Stub(status => status.IsNew).Return(isNew);
            boStatus.Stub(status => status.IsDirty).Return(isDirty);
            boStatus.Stub(status => status.IsValid()).Return(isValid);
            businessObject.Stub(bo => bo.Status).Return(boStatus);
            var classDef = MockRepository.GenerateMock<IClassDef>();
            businessObject.Stub(bo => bo.ClassDef).Return(classDef);
            return businessObject;
        }

        [Test]
        public void Test_Construct_ShouldConstructButtons()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = CreateControlFactoryWin();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ICloseBOEditorDialog dialogWin = CreateDialogBox(factory);
            //---------------Test Result -----------------------
            Assert.IsNotNull(dialogWin);
            Assert.IsNotNull(dialogWin.SaveAndCloseBtn);
            Assert.IsNotNull(dialogWin.CloseWithoutSavingBtn);
            Assert.IsNotNull(dialogWin.CancelCloseBtn);
        }
        [Test]
        public void Test_ConstructWithDisplayname_ShouldConstructButtons()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = CreateControlFactoryWin();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ICloseBOEditorDialog dialogWin = CreateDialogBoxWithDisplayName(factory);
            //---------------Test Result -----------------------
            Assert.IsNotNull(dialogWin);
            Assert.IsNotNull(dialogWin.SaveAndCloseBtn);
            Assert.IsNotNull(dialogWin.CloseWithoutSavingBtn);
            Assert.IsNotNull(dialogWin.CancelCloseBtn);
        }

        [Test]
        public void Test_Construct_WhenNullControlFactory_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                CreateDialogBox(null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("controlFactory", ex.ParamName);
            }
        }

        [Test]
        public void Test_ClickSaveAndClose_ShouldSetDialogResult()
        {
            //---------------Set up test pack-------------------
            ICloseBOEditorDialog dialogWin = CreateDialogBox();

            //---------------Assert Precondition----------------
            Assert.IsNotNull(dialogWin.SaveAndCloseBtn);
            //---------------Execute Test ----------------------
            dialogWin.ShowDialog(CreateMockBO());
            dialogWin.SaveAndCloseBtn.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(CloseBOEditorDialogResult.SaveAndClose, dialogWin.BOEditorDialogResult);
        }


        [Test]
        public void Test_ClickSaveAndClose_ShouldCloseForm()
        {
            //---------------Set up test pack-------------------
            CloseBOEditorDialogWin dialogWin = (CloseBOEditorDialogWin)CreateDialogBox();
            EventHandler closedEventHandler = MockRepository.GenerateStub<EventHandler>();
            dialogWin.Closed += closedEventHandler;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(dialogWin.SaveAndCloseBtn);
            //---------------Execute Test ----------------------
            dialogWin.ShowDialog(CreateMockBO());
            dialogWin.SaveAndCloseBtn.PerformClick();
            //---------------Test Result -----------------------
            closedEventHandler.AssertWasCalled(handler => handler(Arg<object>.Is.Anything, Arg<EventArgs>.Is.Anything));
        }

        [Test]
        public void Test_CloseWithoutSaving_ShouldSetDialogResult()
        {
            //---------------Set up test pack-------------------
            ICloseBOEditorDialog dialogWin = CreateDialogBox();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(dialogWin.CloseWithoutSavingBtn);
            //---------------Execute Test ----------------------
            dialogWin.ShowDialog(CreateMockBO());
            dialogWin.CloseWithoutSavingBtn.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(CloseBOEditorDialogResult.CloseWithoutSaving, dialogWin.BOEditorDialogResult);
        }

        [Test]
        public void Test_CloseWithoutSaving_ShouldCloseForm()
        {
            //---------------Set up test pack-------------------
            CloseBOEditorDialogWin dialogWin = (CloseBOEditorDialogWin)CreateDialogBox();
            EventHandler closedEventHandler = MockRepository.GenerateStub<EventHandler>();
            dialogWin.Closed += closedEventHandler;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(dialogWin.CloseWithoutSavingBtn);
            //---------------Execute Test ----------------------
            dialogWin.ShowDialog(CreateMockBO());
            dialogWin.CloseWithoutSavingBtn.PerformClick();
            //---------------Test Result -----------------------
            closedEventHandler.AssertWasCalled(handler => handler(Arg<object>.Is.Anything, Arg<EventArgs>.Is.Anything));
        }

        [Test]
        public void Test_CancelCloseBtn_ShouldSetDialogResult()
        {
            //---------------Set up test pack-------------------
            ICloseBOEditorDialog dialogWin = CreateDialogBox();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(dialogWin.CancelCloseBtn);
            //---------------Execute Test ----------------------
            dialogWin.CancelCloseBtn.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(CloseBOEditorDialogResult.CancelClose, dialogWin.BOEditorDialogResult);
        }

        [Test]
        public void Test_CancelCloseBtn_ShouldCloseForm()
        {
            //---------------Set up test pack-------------------
            CloseBOEditorDialogWin dialogWin = (CloseBOEditorDialogWin)CreateDialogBox();
            EventHandler closedEventHandler = MockRepository.GenerateStub<EventHandler>();
            dialogWin.Closed += closedEventHandler;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(dialogWin.CancelCloseBtn);
            //---------------Execute Test ----------------------
            dialogWin.ShowDialog(CreateMockBO());
            dialogWin.CancelCloseBtn.PerformClick();
            //---------------Test Result -----------------------
            closedEventHandler.AssertWasCalled(handler => handler(Arg<object>.Is.Anything, Arg<EventArgs>.Is.Anything));
        }


        [Test]
        public void Test_Construct_WithDirtyValidBO_ShouldEnableAllButtons()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = CreateControlFactoryWin();
            var businessObject = CreateMockBO(false, true, true);
            //---------------Assert Precondition----------------
            Assert.IsTrue(businessObject.Status.IsDirty);
            Assert.IsTrue(businessObject.Status.IsValid());
            //---------------Execute Test ----------------------
            ICloseBOEditorDialog dialogWin = new CloseBOEditorDialogWinFake(factory);
            dialogWin.ShowDialog(businessObject);
            //---------------Test Result -----------------------
            Assert.IsTrue(dialogWin.SaveAndCloseBtn.Enabled);
            Assert.IsTrue(dialogWin.CloseWithoutSavingBtn.Enabled);
            Assert.IsTrue(dialogWin.CancelCloseBtn.Enabled);
        }

        [Test]
        public void Test_Construct_WithDirtyInvalidBO_ShouldDisableSaveAndCloseBtn()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = CreateControlFactoryWin();
            var businessObject = CreateMockBO(false, true, false);
            //---------------Assert Precondition----------------
            Assert.IsTrue(businessObject.Status.IsDirty);
            Assert.IsFalse(businessObject.Status.IsValid());
            //---------------Execute Test ----------------------
            ICloseBOEditorDialog dialogWin = new CloseBOEditorDialogWinFake(factory);
            dialogWin.ShowDialog(businessObject);
            //---------------Test Result -----------------------
            Assert.IsFalse(dialogWin.SaveAndCloseBtn.Enabled, "Should be disabled");
            Assert.IsTrue(dialogWin.CloseWithoutSavingBtn.Enabled);
            Assert.IsTrue(dialogWin.CancelCloseBtn.Enabled);
        }
        [Test]
        public void Test_Construct_WithNotDirtyNotValidBO_ShouldCloseFormAndReturnCloseWithoutSaving()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = CreateControlFactoryWin();
            var businessObject = CreateMockBO(false, false, true);
            //---------------Assert Precondition----------------
            Assert.IsFalse(businessObject.Status.IsDirty);
            Assert.IsTrue(businessObject.Status.IsValid());
            //---------------Execute Test ----------------------
            ICloseBOEditorDialog dialogWin = new CloseBOEditorDialogWinFake(factory);
            dialogWin.ShowDialog(businessObject);
            //---------------Test Result -----------------------
            Assert.AreEqual(CloseBOEditorDialogResult.CloseWithoutSaving, dialogWin.BOEditorDialogResult);
        }

        private class CloseBOEditorDialogWinFake : CloseBOEditorDialogWin
        {
            public CloseBOEditorDialogWinFake()
                : base(GlobalUIRegistry.ControlFactory)
            {
            }
            public CloseBOEditorDialogWinFake(IControlFactory controlFactory) : base(controlFactory)
            {
            }

            protected override void ShowForm()
            {
                this.Show();//This is necessarry so that can test the button click events just dont want to show
                //ShowDialog because that would hang the test runner.
                ShowDialogWasCalled = true;
            }

            public bool ShowDialogWasCalled { get; private set; }
        }
    }

}