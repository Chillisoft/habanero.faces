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
using Habanero.Base;
using Habanero.Test.BO;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.HabaneroControls
{
    [TestFixture]
    public class TestBOEditorControlVWG : TestBOEditorControl
    {
        protected override IControlFactory CreateControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override IBOPanelEditorControl CreateEditorControl(IClassDef classDef)
        {
            return new BOEditorControlVWG(classDef);
        }

        protected override IBOPanelEditorControl CreateEditorControl
            (IControlFactory controlFactory, IClassDef def, string uiDefName)
        {
            return new BOEditorControlVWG(controlFactory, def, uiDefName);
        }

        [Test]
        public override void Test_IfValidState_WhenSetControlValueToInvalidValue_ShouldUpdatesErrorProviders()
        {
            //The error provider is not refreshed immediately in VWG
            //Modify test to do an Update
        }

        [Test]
        public override void Test_HasErrors_WhenBOValid_ButCompulsorytFieldSetToNull_ShouldBeTrue()
        {
            //The error provider is not refreshed immediately in VWG
            //Modify test to do an Update
        }
    }

    [TestFixture]
    public class TestBOEditorControlVWG_Generic : TestBOEditorControlVWG
    {
        protected override IBOPanelEditorControl CreateEditorControl(IClassDef classDef)
        {
            if (classDef.ClassName == "OrganisationTestBO")
            {
                return new BOEditorControlVWG<OrganisationTestBO>();
            }
            return new BOEditorControlVWG<ContactPersonTestBO>();
        }

        protected override IBOPanelEditorControl CreateEditorControl
            (IControlFactory controlFactory, IClassDef classDef, string uiDefName)
        {
            if (classDef.ClassName == "OrganisationTestBO")
            {
                return new BOEditorControlVWG<OrganisationTestBO>(controlFactory, uiDefName);
            }
            return new BOEditorControlVWG<ContactPersonTestBO>(controlFactory, uiDefName);
        }
        [Test]
        public override void TestConstructor_NullClassDef_ShouldRaiseError()
        {
            //Not relevant for a Generic since the ClassDef is implied from the Generic Type
        }
    }
}