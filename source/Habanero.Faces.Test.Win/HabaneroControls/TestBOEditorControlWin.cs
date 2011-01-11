using Habanero.Base;
using Habanero.Test.BO;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
    [TestFixture]
    public class TestBOEditorControlWin : TestBOEditorControl
    {
        protected override IControlFactory CreateControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override IBOPanelEditorControl CreateEditorControl(IClassDef classDef)
        {
            return new BOEditorControlWin(classDef);
        }

        protected override IBOPanelEditorControl CreateEditorControl
            (IControlFactory controlFactory, IClassDef def, string uiDefName)
        {
            return new BOEditorControlWin(controlFactory, def, uiDefName);
        }
    }

    [TestFixture]
    public class TestBOEditorControlWin_Generic : TestBOEditorControlWin
    {
        protected override IBOPanelEditorControl CreateEditorControl(IClassDef classDef)
        {
            if (classDef != null && classDef.ClassName == "OrganisationTestBO")
            {
                return new BOEditorControlWin<OrganisationTestBO>();
            }
            return new BOEditorControlWin<ContactPersonTestBO>();
        }

        protected override IBOPanelEditorControl CreateEditorControl
            (IControlFactory controlFactory, IClassDef classDef, string uiDefName)
        {
            if (classDef != null && classDef.ClassName == "OrganisationTestBO")
            {
                return new BOEditorControlWin<OrganisationTestBO>(controlFactory, uiDefName);
            }
            return new BOEditorControlWin<ContactPersonTestBO>(controlFactory, uiDefName);
        }

        [Test]
        public override void TestConstructor_NullClassDef_ShouldRaiseError()
        {
            //Not relevant for a Generic since the ClassDef is implied from the Generic Type
        }

        [Test]
        public void Test_ConstructDefaultConstructor_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(GlobalUIRegistry.ControlFactory);
            //---------------Execute Test ----------------------
            IBOPanelEditorControl controlWin = new BOEditorControlWin<OrganisationTestBO>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(controlWin);
            Assert.AreEqual("default" ,controlWin.PanelInfo.UIForm.UIDef.Name);
        }
        [Test]
        public void Test_ConstructDefaultConstructor_WithUIDefName_ShouldConstructWithName()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(GlobalUIRegistry.ControlFactory);
            //---------------Execute Test ----------------------
            IBOPanelEditorControl controlWin = new BOEditorControlWin<OrganisationTestBO>(CUSTOM_UIDEF_NAME);
            //---------------Test Result -----------------------
            Assert.IsNotNull(controlWin);
            Assert.AreEqual(CUSTOM_UIDEF_NAME, controlWin.PanelInfo.UIForm.UIDef.Name);
        }
    }
}