using Habanero.Faces.Test.Base.Mappers;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Mappers
{
    [TestFixture]
    public class TestNumericUpDownIntegerMapperWin : TestNumericUpDownIntegerMapper
    {
        public override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        [Test]
        public void Test_ValueChangedEvent_UpdatesBusinessObject()
        {
            //---------------Set up test pack-------------------
            INumericUpDown numUpDown = GetControlFactory().CreateNumericUpDownInteger();
            NumericUpDownIntegerMapper mapper = new NumericUpDownIntegerMapper(numUpDown, INT_PROP_NAME, false, GetControlFactory());
            Sample s = new Sample();
            s.SampleInt = 100;
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            int newValue = 555;
            numUpDown.Value = newValue;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(NumericUpDownMapperStrategyWin), mapper.MapperStrategy);
            Assert.AreEqual(newValue, s.SampleInt);
            //---------------Tear down -------------------------
        }


        [Test]
        public void Test_BusinessObjectChanged_UpdatesControl()
        {
            //---------------Set up test pack-------------------
            INumericUpDown numUpDown = GetControlFactory().CreateNumericUpDownInteger();
            NumericUpDownIntegerMapper mapper = new NumericUpDownIntegerMapper(numUpDown, INT_PROP_NAME, false, GetControlFactory());
            Sample s = new Sample();
            s.SampleInt = 100;
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            int newValue = 555;
            s.SampleInt = newValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(newValue, numUpDown.Value);
            //---------------Tear down -------------------------
        }
    }
}