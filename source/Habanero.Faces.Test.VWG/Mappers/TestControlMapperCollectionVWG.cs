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