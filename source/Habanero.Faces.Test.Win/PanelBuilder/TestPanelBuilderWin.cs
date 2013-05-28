using Habanero.BO.ClassDefinition;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.PanelBuilder
{
    [TestFixture]
    public class TestPanelBuilderWin : TestPanelBuilder
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override Sample.SampleUserInterfaceMapper GetSampleUserInterfaceMapper() { return new Sample.SampleUserInterfaceMapperWin(); }


        [Test]
        public void Test_BuildPanelForTab_Parameter_SetNumericUpDownAlignment()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldsWithAlignment_NumericUpDown();
            Habanero.Faces.Base.PanelBuilder panelBuilder = CreatePanelBuilder();
            //---------------Assert Precondition----------------
            Assert.AreEqual("left", ((UIFormField)singleFieldTab[0][0]).Alignment);
            Assert.AreEqual("right", ((UIFormField)singleFieldTab[0][1]).Alignment);
            Assert.AreEqual("center", ((UIFormField)singleFieldTab[0][2]).Alignment);
            Assert.AreEqual("centre", ((UIFormField)singleFieldTab[0][3]).Alignment);
            //---------------Execute Test ----------------------
            IPanel panel = panelBuilder.BuildPanelForTab(singleFieldTab).Panel;
            //---------------Test Result -----------------------

            Assert.IsInstanceOf(typeof(INumericUpDown), panel.Controls[1]);
            INumericUpDown control1 = (INumericUpDown)panel.Controls[1];
            Assert.AreEqual(HorizontalAlignment.Left, control1.TextAlign);

            Assert.IsInstanceOf(typeof(INumericUpDown), panel.Controls[Habanero.Faces.Base.PanelBuilder.CONTROLS_PER_COLUMN + 1]);
            INumericUpDown control2 = (INumericUpDown)panel.Controls[Habanero.Faces.Base.PanelBuilder.CONTROLS_PER_COLUMN + 1];
            Assert.AreEqual(HorizontalAlignment.Right, control2.TextAlign);

            Assert.IsInstanceOf(typeof(INumericUpDown), panel.Controls[Habanero.Faces.Base.PanelBuilder.CONTROLS_PER_COLUMN * 2 + 1]);
            INumericUpDown control3 = (INumericUpDown)panel.Controls[Habanero.Faces.Base.PanelBuilder.CONTROLS_PER_COLUMN * 2 + 1];
            Assert.AreEqual(HorizontalAlignment.Center, control3.TextAlign);

            Assert.IsInstanceOf(typeof(INumericUpDown), panel.Controls[Habanero.Faces.Base.PanelBuilder.CONTROLS_PER_COLUMN * 3 + 1]);
            INumericUpDown control4 = (INumericUpDown)panel.Controls[Habanero.Faces.Base.PanelBuilder.CONTROLS_PER_COLUMN * 3 + 1];
            Assert.AreEqual(HorizontalAlignment.Center, control4.TextAlign);
        }


        //TODO: add tests that label and error provider get tabstop set to false
    }
}