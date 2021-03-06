using Habanero.BO.ClassDefinition;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.PanelBuilder
{
    [TestFixture]
    public class TestPanelBuilderVWG : TestPanelBuilder
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override Sample.SampleUserInterfaceMapper GetSampleUserInterfaceMapper()
        {
            return new SampleUserInterfaceMapperVWG_Fixed();
        }

        public class SampleUserInterfaceMapperVWG_Fixed : Sample.SampleUserInterfaceMapperVWG
        {
            protected override void SetupTypeNameVariables()
            {
                this._textBoxTypeName = "TextBoxVWG";
                this._textBoxAssemblyName = "Habanero.Faces.VWG";
                this._dateTimePickerTypeName = "DateTimePickerVWG";
                this._dateTimePickerAssemblyName = "Habanero.Faces.VWG";
                this._dateTimePickerMapperName = "DateTimePickerMapper";
            }
        }

        [Test]
        public void Test_BuildPanelForTab_Parameter_SetAlignment_NumericUpDown()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldsWithNumericUpDown();
            Habanero.Faces.Base.PanelBuilder panelBuilder = CreatePanelBuilder();
            //---------------Assert Precondition----------------
            Assert.AreEqual("right", ((UIFormField)singleFieldTab[0][0]).Alignment);

            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOf(typeof(INumericUpDown), panel.Controls[1]);
            INumericUpDown control = (INumericUpDown)panel.Controls[1];
            Assert.AreEqual(HorizontalAlignment.Right, control.TextAlign);
        }
    }
}