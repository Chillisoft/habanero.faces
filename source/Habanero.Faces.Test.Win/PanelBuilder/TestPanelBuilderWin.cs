#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
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
            Habanero.Faces.Base.PanelBuilder panelBuilder = new Habanero.Faces.Base.PanelBuilder(GetControlFactory());
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


        //        [Test]
        //        public void Test_Set_IndividualControlCreator_Null_ShouldRaiseError()
        //        {
        //            //---------------Set up test pack-------------------
        //            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
        //            UIForm form = classDef.UIDefCol["TwoTabs"].UIForm;
        //            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
        //            //---------------Assert Precondition----------------
        //            Assert.AreEqual(2, form.Count);
        //            //---------------Execute Test ----------------------
        //            try
        //            {
        //                panelBuilder.SetControlCreators(GetControlFactory().CreateTabControl, null);
        //                Assert.Fail("expected ArgumentNullException");
        //            }
        //                //---------------Test Result -----------------------
        //            catch (ArgumentNullException ex)
        //            {
        //                StringAssert.Contains("Value cannot be null", ex.Message);
        //                StringAssert.Contains("individualControlCreator", ex.ParamName);
        //            }
        //        }

        //[Test, Ignore("This doesn't work in code for some reason")]
        //public void Test_BuildPanelForTab_SetToolTip()
        //{
        //    //---------------Set up test pack-------------------
        //    ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneWithToolTipText();
        //    UIFormTab twoFieldTabOneHasToolTip = classDef.UIDefCol["default"].UIForm[0];
        //    PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
        //    //-------------Assert Preconditions -------------

        //    //---------------Execute Test ----------------------
        //    IPanel panel = panelBuilder.BuildPanel(twoFieldTabOneHasToolTip).Panel;
        //    //---------------Test Result -----------------------
        //    IControlCollection controls = panel.Controls;
        //    ILabel labelWithToolTip =
        //        (ILabel)controls[PanelBuilder.LABEL_CONTROL_COLUMN_NO];
        //    IControlHabanero controlHabanero =
        //        controls[PanelBuilder.INPUT_CONTROL_COLUMN_NO];
        //    IToolTip toolTip = GetControlFactory().CreateToolTip();

        //    Assert.AreEqual("Test tooltip text", toolTip.GetToolTip(controlHabanero));
        //}
    }
}