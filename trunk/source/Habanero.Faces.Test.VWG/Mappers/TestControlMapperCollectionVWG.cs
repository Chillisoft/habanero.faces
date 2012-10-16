using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Test.Base.Mappers;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Mappers
{
    [TestFixture]
    public class TestControlMapperCollectionVWG : TestControlMapperCollection
    {
        protected override IControlFactory GetControlFactory()
        {
            Habanero.Faces.VWG.ControlFactoryVWG factory = new Habanero.Faces.VWG.ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void TestChangeControlValues_DoesNotChangeBusinessObjectValues()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            myBO.TestProp = START_VALUE_1;
            myBO.SetPropertyValue(TEST_PROP_2, START_VALUE_2);

            Habanero.Faces.Base.PanelBuilder factory = new Habanero.Faces.Base.PanelBuilder(GetControlFactory());
            IPanelInfo panelInfo = factory.BuildPanelForForm(myBO.ClassDef.UIDefCol["default"].UIForm);
            panelInfo.BusinessObject = myBO;
            //---------------Execute Test ----------------------
            ChangeValuesInControls(panelInfo);
            //---------------Test Result -----------------------

            Assert.AreEqual(START_VALUE_1, myBO.GetPropertyValue(TEST_PROP_1));
            Assert.AreEqual(START_VALUE_2, myBO.GetPropertyValue(TEST_PROP_2));

        }

    }
}