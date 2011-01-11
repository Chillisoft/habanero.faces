using System.Windows.Forms;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Test.Win.StandardControls
{
    [TestFixture]
    public class TestDateTimePickerMapperStrategyWin 
    {
        protected IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
        [Test]
        public void Test_AddUpdateBoPropOnValueChangedHandler_WhenMapperUsingHabaneroControl_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var dtpWin = new DateTimePickerWin(GetControlFactory()) { Name = "TestComboBox", Enabled = true };
            var comboBoxMapper = new DateTimePickerMapperStub(dtpWin);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<IDateTimePicker>(comboBoxMapper.Control);
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new DateTimePickerMapperStrategyWin();
            comboBoxStrategyWin.AddUpdateBoPropOnValueChangedHandler(comboBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        [Test]
        public void Test_AddUpdateBoPropOnValueChangedHandler_WhenMapperUsingControlAdapter_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var dtpWin = GetWinFormsControlAdapter();
            var comboBoxMapper = new DateTimePickerMapperStub(dtpWin);
            //---------------Assert ComboBoxMapperStub----------------
            Assert.IsInstanceOf<DateTimePicker>(comboBoxMapper.Control.GetControl());
            Assert.IsInstanceOf<DateTimePicker>(comboBoxMapper.GetControl());
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new DateTimePickerMapperStrategyWin();
            comboBoxStrategyWin.AddUpdateBoPropOnValueChangedHandler(comboBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        private static IWinFormsDateTimePickerAdapter GetWinFormsControlAdapter()
        {
            var control = new DateTimePicker();
            var winFormsControlAdapter = MockRepository.GenerateStub<IWinFormsDateTimePickerAdapter>();
            winFormsControlAdapter.Stub(adapter => adapter.WrappedControl).Return(control);
            return winFormsControlAdapter;
        }
    }

    internal class DateTimePickerMapperStub : DateTimePickerMapper
    {
        public DateTimePickerMapperStub(IDateTimePicker dtp)
            : base(dtp, "Fdfad", false, new ControlFactoryWin())
        {
        }
    }
}