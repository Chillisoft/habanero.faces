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

        protected override Sample.SampleUserInterfaceMapper GetSampleUserInterfaceMapper() { return new Sample.SampleUserInterfaceMapperVWG(); }

        [Test]
        public void Test_BuildPanelForTab_Parameter_SetAlignment_NumericUpDown()
        {
            //---------------Set up test pack-------------------
            Sample.SampleUserInterfaceMapper interfaceMapper = GetSampleUserInterfaceMapper();
            UIFormTab singleFieldTab = interfaceMapper.GetFormTabOneFieldsWithNumericUpDown();
            Habanero.Faces.Base.PanelBuilder panelBuilder = new Habanero.Faces.Base.PanelBuilder(GetControlFactory());
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