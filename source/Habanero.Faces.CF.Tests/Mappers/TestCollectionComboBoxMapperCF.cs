using System.Windows.Forms;
using Habanero.Base;
using Habanero.Faces.Adapters;
using Habanero.Faces.Base;
using Habanero.Faces.Base.Tests;
using Habanero.Faces.Base.Tests.Mappers;
using Habanero.Faces.Controls;
using NUnit.Framework;

namespace Habanero.Faces.Tests.Mappers
{
    [TestFixture]
    public class TestCollectionComboBoxMapperCF : TestCollectionComboBoxMapper
    {
// ReSharper disable InconsistentNaming
        protected override IControlFactory GetControlFactory()
        {
            var factory = new ControlFactoryCF();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void TestChangePropValueUpdatesBusObj()
        {
            //---------------Set up test pack-------------------
            var cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            var mapper = new CollectionComboBoxMapper(cmbox, propName, true, GetControlFactory());
            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            var s = new Sample { SampleLookupID = car1.CarID };
            mapper.BusinessObject = s;
            //---------------Test Preconditions-------------------
            Assert.AreEqual(3, Sample.LookupCollection.Count);
            Assert.IsNotNull(mapper.BusinessObjectCollection);
            Assert.IsNotNull(cmbox.SelectedItem, "There should be a selected item to start with");
            //---------------Execute Test ----------------------
            s.SampleLookupID = car2.CarID;
            mapper.UpdateControlValueFromBusinessObject();

            //---------------Test Result -----------------------
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(s.SampleLookupID.ToString(), cmbox.SelectedItem.ToString(),
                            "Value is not set after changing bo prop Value");
        }

        [Test]
        public void TestChangePropValueUpdatesBusObj_WithoutCallingUpdateControlValue()
        {
            //---------------Set up test pack-------------------
            var cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            var mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            var s = new Sample { SampleLookupID = car1.CarID };
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------

            s.SampleLookupID = car2.CarID;

            //---------------Test Result -----------------------
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(s.SampleLookupID.ToString(), cmbox.SelectedItem.ToString(),
                            "Value is not set after changing bo prop Value");
        }

        [Test]
        public override void Test_ChangeComboBoxDoesntUpdateBusinessObject()
        {
            //For Windows the value should is changed (see TestChangeComboBox_UpdatesBusinessObject).
        }

        [Test]
        public void TestChangeComboBox_UpdatesBusinessObject()
        {
            //For Windows the value should be changed.
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            var mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            Sample s = new Sample { SampleLookupID = car1.CarID };
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = car2;
            //---------------Test Result -----------------------
            Assert.AreEqual(car2.CarID.ToString(), s.SampleLookupID.ToString(),
                            "For Windows the value should be changed");
        }

        [Test]
        public void TestKeyPressEventUpdatesBusinessObject_WithoutCallingApplyChanges()
        {
            //---------------Set up test pack-------------------
            var cmbox = CreateComboBoxWinStub();
            const string propName = "SampleLookupID";
            var mapper = new CollectionComboBoxMapper( new WinFormsComboBoxAdapter(cmbox), propName, false, GetControlFactory());
            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            Sample s = new Sample { SampleLookupID = car1.CarID };
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.Text = car2.ToString();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(ComboBoxDefaultMapperStrategyCF), mapper.MapperStrategy);
            Assert.AreEqual(car2.CarID, s.SampleLookupID);
        }

        [Test]
        public void Test_KeyPressStrategy_UpdatesBusinessObject_WhenEnterKeyPressed()
        {
            //---------------Set up test pack-------------------
            var cmbox = CreateComboBoxWinStub();
            const string propName = "SampleLookupID";
            var mapper = new CollectionComboBoxMapper(new WinFormsComboBoxAdapter(cmbox), propName, false, GetControlFactory())
                                                  {
                                                      MapperStrategy =
                                                          GetControlFactory().CreateLookupKeyPressMapperStrategy()
                                                  };
            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            var s = new Sample { SampleLookupID = car1.CarID };
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.Text = car2.ToString();
            cmbox.CallSendKeyBob();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(ComboBoxKeyPressMapperStrategyCF), mapper.MapperStrategy);
            Assert.AreEqual(car2.CarID, s.SampleLookupID);
        }

        private ComboBoxWinStub CreateComboBoxWinStub()
        {
            return new ComboBoxWinStub();
        }


        [Test]
        public void Test_KeyPressStrategy_DoesNotUpdateBusinessObject_When_SelectedIndexChanged()
        {
            //---------------Set up test pack-------------------
            var cmbox = CreateComboBoxWinStub();
            const string propName = "SampleLookupID";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(new WinFormsComboBoxAdapter(cmbox), propName, false, GetControlFactory());
            mapper.MapperStrategy = GetControlFactory().CreateLookupKeyPressMapperStrategy();
            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            Sample s = new Sample { SampleLookupID = car1.CarID };
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = car2;

            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(ComboBoxKeyPressMapperStrategyCF), mapper.MapperStrategy);
            Assert.AreEqual(car1.CarID, s.SampleLookupID);
        }

        [Test]
        public void Test_WhenUseToStringSet_ShouldUseBoToString()
        {
            //---------------Set up test pack-------------------
            var cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleText";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Car car1; Car car2;
            IBusinessObjectCollection collection =
                mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            car1.CarRegNo = "MyCarRegNo";
            Sample sample = new Sample { SampleLookupID = car1.CarID };
            mapper.BusinessObjectCollection = collection;
            mapper.BusinessObject = sample;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, cmbox.Items.Count);
            //---------------Execute Test ----------------------
            mapper.OwningBoPropertyName = "CarRegNo";
            cmbox.SelectedItem = car1;
            //---------------Test Result -----------------------
            Assert.AreEqual(car1.CarRegNo, sample.SampleText);
        }

        private class ComboBoxWinStub : ComboBox
        {
            public void CallSendKeyBob()
            {
                this.OnKeyPress(new System.Windows.Forms.KeyPressEventArgs((char)13));
            }
        }
    }
}