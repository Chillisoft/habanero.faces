using Habanero.BO.ClassDefinition;
using Habanero.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Test.Base
{
    [TestFixture]
    public class TestControlNamingStrategy
    {// ReSharper disable InconsistentNaming

        private static ControlNamingStrategy CreateControlNamingStrategy()
        {
            return new ControlNamingStrategy();
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() =>
            {
                new ControlNamingStrategy();
            });
        }

        [Test]
        public void GetInputControlName_ShouldReturnPropertyName()
        {
            //---------------Set up test pack-------------------
            var controlNamingStrategy = CreateControlNamingStrategy();
            var uiFormField = MockRepository.GenerateStub<IUIFormField>();
            uiFormField.PropertyName = "MyProp";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var name = controlNamingStrategy.GetInputControlName(uiFormField);
            //---------------Test Result -----------------------
            Assert.AreEqual("MyProp", name);
        }

        [Test]
        public void GetLabelControlName_ShouldReturnPropertyNameWithLabelAppend()
        {
            //---------------Set up test pack-------------------
            var controlNamingStrategy = CreateControlNamingStrategy();
            var uiFormField = MockRepository.GenerateStub<IUIFormField>();
            uiFormField.PropertyName = "MyProp";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var name = controlNamingStrategy.GetLabelControlName(uiFormField);
            //---------------Test Result -----------------------
            Assert.AreEqual("MyProp_Label", name);
        }

        [Test]
        public void GetUIFormControlName_ShouldReturnClassName()
        {
            //---------------Set up test pack-------------------
            var controlNamingStrategy = CreateControlNamingStrategy();
            var uiForm = CreateUIFormForClassWithName("MyClass");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var name = controlNamingStrategy.GetUIFormControlName(uiForm);
            //---------------Test Result -----------------------
            Assert.AreEqual("MyClass", name);
        }

        [Test]
        public void GetUIFormControlName_WhenNoClassDef_ShouldReturnUiDefClassName()
        {
            //---------------Set up test pack-------------------
            var controlNamingStrategy = CreateControlNamingStrategy();
            var uiForm = CreateUIFormForClassWithName("MyClass");
            uiForm.ClassDef = null;
            uiForm.UIDef.Stub(ui => ui.ClassName).Return("MyClassName");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var name = controlNamingStrategy.GetUIFormControlName(uiForm);
            //---------------Test Result -----------------------
            Assert.AreEqual("MyClassName", name);
        }

        [Test]
        public void GetUIFormControlName_WhenNoClassDefOrUiDef_ShouldReturnTitle()
        {
            //---------------Set up test pack-------------------
            var controlNamingStrategy = CreateControlNamingStrategy();
            var uiForm = CreateUIFormForClassWithName("MyClass");
            uiForm.ClassDef = null;
            uiForm.UIDef = null;
            uiForm.Title = "My Form Title";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var name = controlNamingStrategy.GetUIFormControlName(uiForm);
            //---------------Test Result -----------------------
            Assert.AreEqual("MyFormTitle", name);
        }

        [Test]
        public void GetUIFormControlName_WhenNoClassDefOrUiDefOrTitle_ShouldReturnDefault()
        {
            //---------------Set up test pack-------------------
            var controlNamingStrategy = CreateControlNamingStrategy();
            var uiForm = CreateUIFormForClassWithName("MyClass");
            uiForm.ClassDef = null;
            uiForm.UIDef = null;
            uiForm.Title = null;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var name = controlNamingStrategy.GetUIFormControlName(uiForm);
            //---------------Test Result -----------------------
            Assert.AreEqual("HabaneroPanel", name);
        }

        [Test]
        public void GetUniqueControlName_WhenNameNotUsed_ShouldReturnSame()
        {
            //---------------Set up test pack-------------------
            var controlNamingStrategy = CreateControlNamingStrategy();
            var panel = new PanelWin();
            panel.Controls.Add(CreatePanel("bob"));
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var name = controlNamingStrategy.GetUniqueControlNameWithin(panel, "NewName");
            //---------------Test Result -----------------------
            Assert.AreEqual("NewName", name);
        }

        [Test]
        public void GetUniqueControlName_WhenNameAlreadyUsed_ShouldReturnWithNumberSuffix()
        {
            //---------------Set up test pack-------------------
            var controlNamingStrategy = CreateControlNamingStrategy();
            var panel = CreatePanel();
            panel.Controls.Add(CreatePanel("NewName"));
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var name = controlNamingStrategy.GetUniqueControlNameWithin(panel, "NewName");
            //---------------Test Result -----------------------
            Assert.AreEqual("NewName_1", name);
        }

        [Test]
        public void GetUniqueControlName_WhenNameAlreadyUsedAsAlternate_ShouldReturnWithNumberSuffix()
        {
            //---------------Set up test pack-------------------
            var controlNamingStrategy = CreateControlNamingStrategy();
            var panel = CreatePanel();
            panel.Controls.Add(CreatePanel("NewName"));
            panel.Controls.Add(CreatePanel("NewName_1"));
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var name = controlNamingStrategy.GetUniqueControlNameWithin(panel, "NewName_1");
            //---------------Test Result -----------------------
            Assert.AreEqual("NewName_1_1", name);
        }

        [Test]
        public void GetUniqueControlName_WhenNameAlreadyUsedTwice_ShouldReturnWithNumberSuffix()
        {
            //---------------Set up test pack-------------------
            var controlNamingStrategy = CreateControlNamingStrategy();
            var panel = CreatePanel();
            panel.Controls.Add(CreatePanel("NewName"));
            panel.Controls.Add(CreatePanel("NewName_1"));
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var name = controlNamingStrategy.GetUniqueControlNameWithin(panel, "NewName");
            //---------------Test Result -----------------------
            Assert.AreEqual("NewName_2", name);
        }

        [Test]
        public void GetUniqueControlName_WhenNameAlreadyUsedManyTimes_ShouldReturnWithNumberSuffix()
        {
            //---------------Set up test pack-------------------
            var controlNamingStrategy = CreateControlNamingStrategy();
            var panel = CreatePanel();
            panel.Controls.Add(CreatePanel("NewName"));
            panel.Controls.Add(CreatePanel("NewName_1"));
            panel.Controls.Add(CreatePanel("NewName_2"));
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var name = controlNamingStrategy.GetUniqueControlNameWithin(panel, "NewName");
            //---------------Test Result -----------------------
            Assert.AreEqual("NewName_3", name);
        }

        private static PanelWin CreatePanel(string name = "DefaultName")
        {
            return new PanelWin {Name = name};
        }


        private static IUIForm CreateUIFormForClassWithName(string className)
        {
            var uiForm = MockRepository.GenerateStub<IUIForm>();
            var classDef = MockRepository.GenerateStub<IClassDef>();
            classDef.ClassName = className;
            uiForm.ClassDef = classDef;
            uiForm.UIDef = MockRepository.GenerateStub<IUIDef>();
            uiForm.UIDef.ClassDef = classDef;
            return uiForm;
        }
    }
}