using Habanero.BO.ClassDefinition;
using Habanero.Test;
using Habanero.Test.BO;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.PanelBuilder
{
    [TestFixture]
    public class TestPanelInfoVWG : TestPanelInfo
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG(); ;
        }

        [Test]
        public void TestApplyChangesToBusinessObject()
        {
            //---------------Set up test pack-------------------
            Sample.CreateClassDefWithTwoPropsOneInteger();
            Sample sampleBO = new Sample();
            const string startText = "startText";
            const string endText = "endText";
            sampleBO.SampleText = startText;
            sampleBO.SampleInt = 1;

            IPanelInfo panelInfo = new PanelInfo();
            PanelInfo.FieldInfo sampleTextFieldInfo = CreateFieldInfo("SampleText");
            PanelInfo.FieldInfo sampleIntFieldInfo = CreateFieldInfo("SampleInt");
            panelInfo.FieldInfos.Add(sampleTextFieldInfo);
            panelInfo.FieldInfos.Add(sampleIntFieldInfo);
            panelInfo.BusinessObject = sampleBO;

            sampleTextFieldInfo.InputControl.Text = endText;
            //---------------Assert Precondition----------------
            Assert.AreEqual(startText, sampleBO.SampleText);
            //---------------Execute Test ----------------------
            panelInfo.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreEqual(endText, sampleBO.SampleText);
            Assert.AreEqual(1, sampleBO.SampleInt);
        }

        [Test]
        public void Test_UpdateErrorProviderError_WhenBOInvalid_ShouldSetErrorMessage()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
            ContactPersonTestBO person = ContactPersonTestBO.CreateUnsavedContactPerson("", "");
            Habanero.Faces.Base.PanelBuilder panelBuilder = new Habanero.Faces.Base.PanelBuilder(GetControlFactory());
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab((UIFormTab) person.ClassDef.UIDefCol["default"].UIForm[0]);
            person.Surname = TestUtil.GetRandomString();
            panelInfo.BusinessObject = person;
            IControlMapper SurnameControlMapper = panelInfo.FieldInfos["Surname"].ControlMapper;
            person.Surname = "";
            //---------------Assert Precondition----------------
            Assert.IsFalse(person.Status.IsValid());
            Assert.AreEqual("", SurnameControlMapper.GetErrorMessage());
            //---------------Execute Test ----------------------
            panelInfo.UpdateErrorProvidersErrorMessages();
            //---------------Test Result -----------------------
            Assert.AreNotEqual("", SurnameControlMapper.GetErrorMessage());
        }
    }
}