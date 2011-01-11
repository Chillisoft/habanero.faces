using System;
using System.Windows.Forms;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Test.Win.StandardControls
{
    [TestFixture]
    public class TestNumericUpDownMapperStrategyWin 
    {
        protected IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
        [Test]
        public void Test_AddValueChangedHandler_WhenMapperUsingHabaneroControl_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var updWin = new NumericUpDownWin() { Name = "TestNumericUpDown", Enabled = true };
            var comboBoxMapper = new NumericUpDownMapperStub(updWin);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<NumericUpDown>(comboBoxMapper.Control);
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new NumericUpDownMapperStrategyWin();
            comboBoxStrategyWin.ValueChanged(comboBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        [Test]
        public void Test_AddValueChangedHandler_WhenMapperUsingControlAdapter_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var updWin = GetWinFormsControlAdapter();
            var comboBoxMapper = new NumericUpDownMapperStub(updWin);
            //---------------Assert ComboBoxMapperStub----------------
            Assert.IsInstanceOf<NumericUpDown>(comboBoxMapper.Control.GetControl());
            Assert.IsInstanceOf<NumericUpDown>(comboBoxMapper.GetControl());
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new NumericUpDownMapperStrategyWin();
            comboBoxStrategyWin.ValueChanged(comboBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        private static IWinFormsNumericUpDownAdapter GetWinFormsControlAdapter()
        {
            var control = new NumericUpDown();
            var winFormsControlAdapter = MockRepository.GenerateStub<IWinFormsNumericUpDownAdapter>();
            winFormsControlAdapter.Stub(adapter => adapter.WrappedControl).Return(control);
            return winFormsControlAdapter;
        }
    }

    internal class NumericUpDownMapperStub : NumericUpDownMapper
    {
        public NumericUpDownMapperStub(INumericUpDown ctl) 
            : base(ctl, "Fdaf", false, new ControlFactoryWin())
        {
        }

        public override void ApplyChangesToBusinessObject()
        {

        }
    }
}