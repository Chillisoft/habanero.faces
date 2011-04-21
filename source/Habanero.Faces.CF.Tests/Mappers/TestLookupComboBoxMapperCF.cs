using System;
using System.Collections.Generic;
using Habanero.BO;
using Habanero.Faces.Base.CF.Tests;
using Habanero.Faces.CF;
using Habanero.Faces.CF.Controls;
using Habanero.Faces.Test.Base.Mappers;
using Habanero.Faces.Base;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Mappers
{
    [Ignore("Not using lookups at the moment")]
    [TestFixture]
    public class TestLookupComboBoxMapperCF : TestLookupComboBoxMapper
    {
// ReSharper disable InconsistentNaming
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryCF factory = new ControlFactoryCF();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void TestChangePropValueUpdatesBusObj()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Sample s = new Sample();
            mapper.LookupList = Sample.LookupCollection;
            Guid guidResult;
            StringUtilities.GuidTryParse(Sample.LookupCollection[LOOKUP_ITEM_1], out guidResult);
            s.SampleLookupID = guidResult;
            mapper.BusinessObject = s;
            //---------------Test Preconditions-------------------
            Assert.AreEqual(3, Sample.LookupCollection.Count);
            Assert.IsNotNull(mapper.LookupList);
            Assert.IsNotNull(cmbox.SelectedItem, "There should be a selected item to start with");
            //---------------Execute Test ----------------------

            s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2);
            mapper.UpdateControlValueFromBusinessObject();

            //---------------Test Result -----------------------
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(LOOKUP_ITEM_2, cmbox.SelectedItem.ToString(), "Value is not set after changing bo prop Value");
        }

        [Test]
        public void TestChangePropValueUpdatesBusObj_WithoutCallingUpdateControlValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Sample s = new Sample();
            mapper.LookupList = Sample.LookupCollection;
            s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------

            s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2);

            //---------------Test Result -----------------------
            Assert.AreEqual(LOOKUP_ITEM_2, cmbox.SelectedItem.ToString(), "Value is not set after changing bo prop");

            //---------------Tear Down -------------------------
        }

        [Test]
        public override void TestChangeComboBoxDoesntUpdateBusinessObject()
        {
            //For Windows the value should be changed.
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Sample s = new Sample();
            Dictionary<string, string> collection = mapper.LookupList = GetLookupList();
            Guid guidResult;
            StringUtilities.GuidTryParse(collection[LOOKUP_ITEM_1], out guidResult);
            s.SampleLookupID = guidResult;
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = LOOKUP_ITEM_2;

            //---------------Test Result -----------------------
            Assert.AreEqual(collection[LOOKUP_ITEM_2], s.SampleLookupID.ToString(), "For Windows the value should be changed");
        }

        private static Dictionary<string, string> GetLookupList()
        {
            Sample sample1 = new Sample();
            sample1.Save();
            Sample sample2 = new Sample();
            sample2.Save();
            Sample sample3 = new Sample();
            sample3.Save();
            return new Dictionary<string, string>
                       {
                           {"Test3", sample3.ID.GetAsValue().ToString()},
                           {"Test2", sample2.ID.GetAsValue().ToString()},
                           {"Test1", sample1.ID.GetAsValue().ToString()}
                       };
        }

        [Test]
        public void TestChangeComboBoxUpdatesBusinessObject()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Sample s = new Sample();
            mapper.LookupList = Sample.LookupCollection;
            s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = LOOKUP_ITEM_2;
            mapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreEqual((Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2), s.SampleLookupID);
        }

        [Test]
        public void TestChangeComboBoxUpdatesBusinessObject_WithoutCallingApplyChanges()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Sample s = new Sample();
            mapper.LookupList = Sample.LookupCollection;
            s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = LOOKUP_ITEM_2;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(ComboBoxDefaultMapperStrategyCF), mapper.MapperStrategy);
            Assert.AreEqual((Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2), s.SampleLookupID);
        }
        /* //TODO brett 21 Apr 2011: Port to CF
                [Test]
                public void TestKeyPressEventUpdatesBusinessObject_WithoutCallingApplyChanges()
                {
                    //---------------Set up test pack-------------------
                    ComboBoxWinStub cmbox = new ComboBoxWinStub();
                    const string propName = "SampleLookupID";
                    LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                    Sample s = new Sample();
                    mapper.LookupList = Sample.LookupCollection;
                    s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
                    mapper.BusinessObject = s;
                    //---------------Execute Test ----------------------
                    cmbox.Text = "Test2";
                    //---------------Test Result -----------------------
                    Assert.IsInstanceOf(typeof(ComboBoxDefaultMapperStrategyWin), mapper.MapperStrategy);
                    Assert.AreEqual((Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2), s.SampleLookupID);
                }

                [Test]
                public void Test_KeyPressStrategy_UpdatesBusinessObject_WhenEnterKeyPressed()
                {
                    //---------------Set up test pack-------------------
                    ComboBoxWinStub cmbox = new ComboBoxWinStub();
                    const string propName = "SampleLookupID";
                    LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                    mapper.MapperStrategy = GetControlFactory().CreateLookupKeyPressMapperStrategy();
                    Sample s = new Sample();
                    mapper.LookupList = Sample.LookupCollection;
                    s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
                    mapper.BusinessObject = s;
                    //---------------Execute Test ----------------------
                    cmbox.Text = "Test2";
                    cmbox.CallSendKeyBob();

                    //---------------Test Result -----------------------
                    Assert.IsInstanceOf(typeof(ComboBoxKeyPressMapperStrategyWin), mapper.MapperStrategy);
                    Assert.AreEqual((Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2), s.SampleLookupID);
                }


                [Test]
                public void Test_KeyPressStrategy_DoesNotUpdateBusinessObject_SelectedIndexChanged()
                {
                    //---------------Set up test pack-------------------
                    ComboBoxWinStub cmbox = new ComboBoxWinStub();
                    const string propName = "SampleLookupID";
                    LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                    mapper.MapperStrategy = GetControlFactory().CreateLookupKeyPressMapperStrategy();
                    Sample s = new Sample();
                    mapper.LookupList = Sample.LookupCollection;
                    s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
                    mapper.BusinessObject = s;
                    //---------------Execute Test ----------------------
                    cmbox.SelectedItem = LOOKUP_ITEM_2;

                    //---------------Test Result -----------------------
                    Assert.IsInstanceOf(typeof(ComboBoxKeyPressMapperStrategyWin), mapper.MapperStrategy);
                    Assert.AreEqual((Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1), s.SampleLookupID);
                    //---------------Tear Down -------------------------
                }*/
        /*       //TODO brett 21 Apr 2011: Port to CF
         * private class ComboBoxWinStub : ComboBoxWin
                {
                    public void CallSendKeyBob()
                    {
                        this.OnKeyPress(new System.Windows.Forms.KeyPressEventArgs((char)13));
                    }
                }*/
    }
}