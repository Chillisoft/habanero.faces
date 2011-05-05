using System;
using System.Windows.Forms;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using DateTimePickerFormat = Habanero.Faces.Base.DateTimePickerFormat;

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
            var mapper = new DateTimePickerMapperStub(dtpWin);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<IDateTimePicker>(mapper.Control);
			Assert.IsNull(dtpWin.ValueOrNull);
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new DateTimePickerMapperStrategyWin();
            comboBoxStrategyWin.AddUpdateBoPropOnValueChangedHandler(mapper);
            //---------------Assert Result----------------------
        	mapper.ApplyChangesWasCalled = false;
			dtpWin.ValueOrNull = dtpWin.Value;
        	Assert.IsTrue(mapper.ApplyChangesWasCalled, "Apply changes should have been called");
        }

        [Test]
        public void Test_AddUpdateBoPropOnValueChangedHandler_WhenMapperUsingControlAdapter_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var dtpWin = GetWinFormsControlAdapter();
            var mapper = new DateTimePickerMapperStub(dtpWin); 
            //---------------Assert ComboBoxMapperStub----------------
            Assert.IsInstanceOf<DateTimePicker>(mapper.Control.GetControl());
            Assert.IsInstanceOf<DateTimePicker>(mapper.GetControl());
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new DateTimePickerMapperStrategyWin();
            comboBoxStrategyWin.AddUpdateBoPropOnValueChangedHandler(mapper);
            //---------------Assert Result----------------------
        	mapper.ApplyChangesWasCalled = false;
			((DateTimePicker)dtpWin.WrappedControl).Value = DateTime.Now;
        	Assert.IsTrue(mapper.ApplyChangesWasCalled, "Apply changes should have been called");
        }

        [Test]
        public void Test_AddUpdateBoPropOnValueChangedHandler_WhenMapperUsingCustomHabaneroControl_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
			var dtpWin = MockRepository.GenerateStub<CustomDateTimePickerStub>();
			IEventRaiser valueChangedEventRaiser = dtpWin.GetEventRaiser(picker => picker.ValueChanged += null);
            var mapper = new DateTimePickerMapperStub(dtpWin);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<IDateTimePicker>(mapper.Control);
            Assert.IsNotNull(mapper.Control.GetControl(), "The custom control should support the Control base class");
			Assert.IsNotNull(mapper.GetControl(), "The custom control should support the Control base class");
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new DateTimePickerMapperStrategyWin();
            comboBoxStrategyWin.AddUpdateBoPropOnValueChangedHandler(mapper);
            //---------------Assert Result----------------------
        	mapper.ApplyChangesWasCalled = false;
        	valueChangedEventRaiser.Raise(dtpWin, EventArgs.Empty);
        	Assert.IsTrue(mapper.ApplyChangesWasCalled, "Apply changes should have been called");
        }

		public abstract class CustomDateTimePickerStub: ControlWin, IDateTimePicker
		{
			public abstract DateTime Value { get; set; }
			public abstract DateTime? ValueOrNull { get; set; }
			public abstract string CustomFormat { get; set; }
			public abstract DateTimePickerFormat Format { get; set; }
			public abstract bool ShowUpDown { get; set; }
			public abstract bool ShowCheckBox { get; set; }
			public abstract bool Checked { get; set; }
			public abstract string NullDisplayValue { get; set; }
			public abstract event EventHandler ValueChanged;
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
		public bool ApplyChangesWasCalled { get; set; }

        public DateTimePickerMapperStub(IDateTimePicker dtp)
            : base(dtp, "Fdfad", false, new ControlFactoryWin())
        {
        }

    	public override void ApplyChangesToBusinessObject()
    	{
    		ApplyChangesWasCalled = true;
    		base.ApplyChangesToBusinessObject();
    	}
    }
}