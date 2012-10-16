using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Test.Base.Mappers;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Mappers
{
    [TestFixture]
    public class TestControlMapperCollectionWin : TestControlMapperCollection
    {
        protected override IControlFactory GetControlFactory()
        {
            Habanero.Faces.Win.ControlFactoryWin factory = new Habanero.Faces.Win.ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void TestChangeControlValues_ChangesBusinessObjectValues()
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
            panelInfo.FieldInfos[TEST_PROP_1].ControlMapper.ApplyChangesToBusinessObject();
            panelInfo.FieldInfos[TEST_PROP_2].ControlMapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------

            Assert.AreEqual(CHANGED_VALUE_1, myBO.GetPropertyValue(TEST_PROP_1));
            Assert.AreEqual(CHANGED_VALUE_2, myBO.GetPropertyValue(TEST_PROP_2));

        }
    }
}