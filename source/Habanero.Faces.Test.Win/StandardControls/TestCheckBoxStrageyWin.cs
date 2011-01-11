using System;
using System.Windows.Forms;
using Habanero.Faces.Base;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Extensions.Forms;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Test.Win.StandardControls
{
    [TestFixture]
    public class TestCheckBoxStrageyWin 
    {
        protected IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        [Test]
        public void Test_AddClickEventHandler_WhenMapperUsingHabaneroControl_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var checkBoxWin = new CheckBoxWin {Name = "TestCheckBox", Checked = false, Enabled = true};
            var checkBoxMapper = new CheckBoxMapperStub(checkBoxWin);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<ICheckBox>(checkBoxMapper.Control);
            //---------------Execute Test ----------------------
            var checkBoxStrategyWin = new CheckBoxStrategyWin();
            checkBoxStrategyWin.AddClickEventHandler(checkBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        [Test]
        public void Test_AddClickEventHandler_WhenMapperUsingControlAdapter_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var checkBoxWin = GetWinFormsControlAdapter();
            var checkBoxMapper = new CheckBoxMapperStub(checkBoxWin);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<CheckBox>(checkBoxMapper.GetControl());
            //---------------Execute Test ----------------------
            var checkBoxStrategyWin = new CheckBoxStrategyWin();
            checkBoxStrategyWin.AddClickEventHandler(checkBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        private static IWinFormsCheckBoxAdapter GetWinFormsControlAdapter()
        {
            var control = new CheckBox();
            var winFormsControlAdapter = MockRepository.GenerateStub<IWinFormsCheckBoxAdapter>();
            winFormsControlAdapter.Stub(adapter => adapter.WrappedControl).Return(control);
            return winFormsControlAdapter;
        }
    }

    public class CheckBoxMapperStub : CheckBoxMapper
    {
        public CheckBoxMapperStub(ICheckBox cb) : base(cb, "Fdfad", false, new ControlFactoryWin())
        {
        }
/*
        public override void ApplyChangesToBusinessObject()
        {
            ApplyChangesToBusinessObjectCalled = true;
        }

        public bool ApplyChangesToBusinessObjectCalled { get; private set; }

        public override void UpdateControlValueFromBusinessObject()
        {
            UpdateControlValueFromBusinessObjectCalled = true;
        }

        public bool UpdateControlValueFromBusinessObjectCalled { get; private set; }*/
    }
}