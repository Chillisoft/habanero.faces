// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System.Drawing;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Console;
using Habanero.Faces.Win;
using Habanero.Test.Structure;
using Habanero.Faces.Base;
using NUnit.Framework;
using DialogResult = Habanero.Faces.Base.DialogResult;
using FormStartPosition = Habanero.Faces.Base.FormStartPosition;

namespace Habanero.Faces.Test.Base
{
    /// <summary>
    /// Summary description for TestDefaultBOEditorForm.
    /// </summary>
    public abstract class TestDefaultBOEditorForm// : TestUsingDatabase
    {
        protected IClassDef _classDefMyBo;
        private IBusinessObject _bo;
        private IDefaultBOEditorForm _defaultBOEditorForm;

        protected abstract IControlFactory GetControlFactory();

        protected virtual IDefaultBOEditorForm CreateDefaultBOEditorForm(IBusinessObject businessObject)
        {
            return GetControlFactory().CreateBOEditorForm((BusinessObject)businessObject);
        }

        protected virtual void ShowFormIfNecessary(IFormHabanero form)
        {
            form.Show();
        }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            GlobalRegistry.UIExceptionNotifier = new ConsoleExceptionNotifier();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            LoadMyBOClassDef();
            }

        protected abstract void LoadMyBOClassDef();

        [SetUp]
        public void SetupTest()
        {
            BusinessObjectManager.Instance.ClearLoadedObjects();
            _bo = _classDefMyBo.CreateNewBusinessObject();
            _defaultBOEditorForm = CreateDefaultBOEditorForm(_bo);
        }

        [Test]
        public void Test_Layout()
        {
            //---------------Test Result -----------------------
            Assert.AreEqual(2, _defaultBOEditorForm.Controls.Count);
            IControlHabanero boCtl = _defaultBOEditorForm.PanelInfo.Panel;
            Assert.AreEqual(6, boCtl.Controls.Count);
            IControlHabanero buttonControl = _defaultBOEditorForm.Controls[1];
            Assert.IsInstanceOf(typeof(IButtonGroupControl), buttonControl);
            Assert.AreEqual(2, buttonControl.Controls.Count);
            Assert.AreEqual(FormStartPosition.CenterScreen, _defaultBOEditorForm.StartPosition);
        }

        [Test]
        public void Test_Layout_ShouldSetupPanelSizeCorrectly()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Size size = _defaultBOEditorForm.PanelInfo.Panel.Size;
            IUIForm uiForm = _defaultBOEditorForm.PanelInfo.UIForm;
            Assert.AreEqual(uiForm.Width, size.Width, "Should set width correctly");
            Assert.AreEqual(uiForm.Height, size.Height, "Should set height correctly");
        }

        [Test]
        public void Test_Layout_ShouldSetupFormSizeCorrectly()
        {
            //---------------Set up test pack-------------------
            IUIForm uiForm = _defaultBOEditorForm.PanelInfo.UIForm;
            IPanel panel = GetControlFactory().CreatePanel();
            panel.Size = new Size(uiForm.Width, uiForm.Height);
            IFormHabanero okCancelForm = GetControlFactory().CreateOKCancelDialogFactory().CreateOKCancelForm(panel, "Test");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.AreEqual(okCancelForm.Width, _defaultBOEditorForm.Width, "Should set width correctly");
            Assert.AreEqual(okCancelForm.Height, _defaultBOEditorForm.Height, "Should set height correctly");
        }

        [Test]
        public void Test_Construct_ShouldConstructWithDefaultConstructor()
        {
            //---------------Set up test pack-------------------
            IBusinessObject bo = _classDefMyBo.CreateNewBusinessObject();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IDefaultBOEditorForm defaultBOEditorForm = GetControlFactory().CreateBOEditorForm((BusinessObject)bo);
            //---------------Test Result -----------------------
            Assert.IsNotNull(defaultBOEditorForm.PanelInfo);
            Assert.IsNotNull(defaultBOEditorForm.GroupControlCreator);
        }
        [Test]
        public void Test_AlternateConstruct_ShouldConstructWithDefaultConstructor()
        {
            //---------------Set up test pack-------------------
            IBusinessObject bo = _classDefMyBo.CreateNewBusinessObject();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IDefaultBOEditorForm defaultBOEditorForm = GetControlFactory().CreateBOEditorForm((BusinessObject)bo,"default");
            //---------------Test Result -----------------------
            Assert.IsNotNull(defaultBOEditorForm.PanelInfo);
            Assert.IsNotNull(defaultBOEditorForm.GroupControlCreator);
        }
        [Test]
        public void Test_AlternateConstruct_2_ShouldConstructWithDefaultConstructor()
        {
            //---------------Set up test pack-------------------
            IBusinessObject bo = _classDefMyBo.CreateNewBusinessObject();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IDefaultBOEditorForm defaultBOEditorForm = GetControlFactory().CreateBOEditorForm((BusinessObject)bo,"default", delegate {  });
            //---------------Test Result -----------------------
            Assert.IsNotNull(defaultBOEditorForm.PanelInfo);
            Assert.IsNotNull(defaultBOEditorForm.GroupControlCreator);
        }
        [Test]
        public void Test_Constructor_WithGroupCreator()
        {
            //---------------Set up test pack-------------------
            IBusinessObject bo = _classDefMyBo.CreateNewBusinessObject();
            GroupControlCreator groupControl = GetControlFactory().CreateCollapsiblePanelGroupControl;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IDefaultBOEditorForm defaultBOEditorForm = GetControlFactory().CreateBOEditorForm((BusinessObject)bo,"default",  groupControl);
            //---------------Test Result -----------------------
            Assert.IsNotNull(defaultBOEditorForm.PanelInfo);
            Assert.IsNotNull(defaultBOEditorForm.GroupControlCreator);
            Assert.AreSame(groupControl, defaultBOEditorForm.GroupControlCreator);
        }



        [Test]
        public void Test_Constructor_WhenUIDefIsInherited_ShouldUseInheritedUIDef()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Entity.LoadDefaultClassDef();
            IClassDef subClassDef = LegalEntity.LoadClassDef_WithSingleTableInheritance();
            IClassDef subSubClassDef = Vehicle.LoadClassDef_WithClassTableInheritance();
            UIForm uiForm = new UIForm(new UIFormTab(new UIFormColumn(new UIFormField("My Form Field", "EntityType"))));
            string uiDefName = "EntityUiDef";
            classDef.UIDefCol.Add(new UIDef(uiDefName, uiForm, null));
            IBusinessObject businessObject = subSubClassDef.CreateNewBusinessObject();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IDefaultBOEditorForm defaultBOEditorForm = GetControlFactory().CreateBOEditorForm((BusinessObject)businessObject, uiDefName);
            //---------------Test Result -----------------------
            IPanelInfo panelInfo = defaultBOEditorForm.PanelInfo;
            Assert.IsNotNull(panelInfo);
            Assert.AreSame(uiForm, panelInfo.UIForm);
        }


        [Test]
        public void Test_ClickOK_ShouldCommitEdits()
        {
            //---------------Set up test pack-------------------
            ShowFormIfNecessary(_defaultBOEditorForm);
            EditControlValueOnForm(_defaultBOEditorForm, "TestProp", "TestValue");
            EditControlValueOnForm(_defaultBOEditorForm, "TestProp2", "TestValue2");
            IButton okButton = _defaultBOEditorForm.Buttons["OK"];
            //--------------Assert PreConditions----------------
            Assert.IsNotNull(okButton);
            //---------------Execute Test ----------------------
            okButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsFalse(_defaultBOEditorForm.Visible);
            Assert.AreEqual(DialogResult.OK, _defaultBOEditorForm.DialogResult);
            Assert.AreEqual("TestValue", _bo.GetPropertyValue("TestProp"));
            Assert.AreEqual("TestValue2", _bo.GetPropertyValue("TestProp2"));
            Assert.IsFalse(_bo.Status.IsDirty);
            Assert.IsNull(_defaultBOEditorForm.PanelInfo.BusinessObject);
            //TearDown--------------------------
            //_defaultBOEditorForm.Dispose();
        }

        [Test]
        public void Test_ClickCancel_WhenIsNew_ShouldCancelEditsAndMarkForDelete()
        {
            //---------------Set up test pack-------------------
            ShowFormIfNecessary(_defaultBOEditorForm);
            EditControlValueOnForm(_defaultBOEditorForm, "TestProp", "TestValue");
            EditControlValueOnForm(_defaultBOEditorForm, "TestProp2", "TestValue2");
            IButton cancelButton = _defaultBOEditorForm.Buttons["Cancel"];
            //--------------Assert PreConditions----------------
            Assert.IsNotNull(cancelButton);
            Assert.IsTrue(_bo.Status.IsNew, "BO should  be new prior to editing.");
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsFalse(_defaultBOEditorForm.Visible);
            Assert.AreEqual(DialogResult.Cancel, _defaultBOEditorForm.DialogResult);
            Assert.AreEqual(null, _bo.GetPropertyValue("TestProp"));
            Assert.AreEqual(null, _bo.GetPropertyValue("TestProp2"));
            Assert.IsTrue(_bo.Status.IsDeleted);
            Assert.IsNull(_defaultBOEditorForm.PanelInfo.BusinessObject);
        }

        [Test]
        public void Test_ClickCancel_WhenNotIsNew_ShouldCancelEditsAndNotMarkForDelete()
        {
            //---------------Set up test pack-------------------
            IBusinessObject bo = _classDefMyBo.CreateNewBusinessObject();
            bo.Save();
            IDefaultBOEditorForm boEditorForm = GetControlFactory()
                .CreateBOEditorForm((BusinessObject) bo, "default", delegate { });

            ShowFormIfNecessary(boEditorForm);
            EditControlValueOnForm(boEditorForm, "TestProp", "TestValue");
            EditControlValueOnForm(boEditorForm, "TestProp2", "TestValue2");
            bo.SetPropertyValue("TestProp", "TestValue");
            IButton cancelButton = boEditorForm.Buttons["Cancel"];
            //--------------Assert PreConditions----------------
            Assert.IsNotNull(cancelButton);
            Assert.IsTrue(bo.Status.IsDirty, "BO should be dirty prior to cancelling");
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(DialogResult.Cancel, boEditorForm.DialogResult);
            Assert.AreEqual(null, bo.GetPropertyValue("TestProp"));
            Assert.AreEqual(null, bo.GetPropertyValue("TestProp2"));
            Assert.IsFalse(bo.Status.IsDirty, "BO should not be dirty after cancelling");
            Assert.IsFalse(bo.Status.IsDeleted, "Saved BO should not be deleted on cancelling edits");
            Assert.IsNull(boEditorForm.PanelInfo.BusinessObject);
        }
        [Test]
        public void Test_ClickOK_ShouldCallDelegateWithCorrectInformation()
        {
            //---------------Set up test pack-------------------
            IBusinessObject bo = _classDefMyBo.CreateNewBusinessObject();

            bool delegateCalled = false;
            bool cancelledValue = true;
            IBusinessObject boInDelegate = null;
            IDefaultBOEditorForm boEditorForm = GetControlFactory()
                .CreateBOEditorForm((BusinessObject)bo, "default",
                delegate(IBusinessObject bo1, bool cancelled)
                {
                    delegateCalled = true;
                    cancelledValue = cancelled;
                    boInDelegate = bo1;
                });
            ShowFormIfNecessary(boEditorForm);
            IButton okButton = boEditorForm.Buttons["OK"];
            //--------------Assert PreConditions----------------
            Assert.IsNotNull(okButton);
            Assert.IsFalse(delegateCalled);
            //---------------Execute Test ----------------------
            okButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(delegateCalled);
            Assert.IsFalse(cancelledValue);
            Assert.AreSame(bo, boInDelegate);
        }

        [Test]
        public void Test_ClickCancel_ShouldCallPostEditDelegateWithCancelled()
        {
            //---------------Set up test pack-------------------
            IBusinessObject bo = _classDefMyBo.CreateNewBusinessObject();

            bool delegateCalled = false;
            bool cancelledValue = false;
            IBusinessObject boInDelegate = null;
            IDefaultBOEditorForm boEditorForm =
                GetControlFactory().CreateBOEditorForm((BusinessObject)bo, "default",
                delegate(IBusinessObject bo1, bool cancelled)
                {
                    delegateCalled = true;
                    cancelledValue = cancelled;
                    boInDelegate = bo1;
                });
            ShowFormIfNecessary(boEditorForm);
            EditControlValueOnForm(_defaultBOEditorForm, "TestProp", "TestValue");
            EditControlValueOnForm(_defaultBOEditorForm, "TestProp2", "TestValue2");
            IButton cancelButton = boEditorForm.Buttons["Cancel"];
            //--------------Assert PreConditions----------------
            Assert.IsNotNull(cancelButton);
            Assert.IsFalse(delegateCalled);
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(delegateCalled);
            Assert.IsTrue(cancelledValue);
            Assert.AreSame(bo, boInDelegate);
        }

        [Test]
        public virtual void Test_CloseForm_ShouldCallPostEditDelegateWithCancelled()
        {
            //---------------Set up test pack-------------------
            IBusinessObject bo = _classDefMyBo.CreateNewBusinessObject();

            bool delegateCalled = false;
            bool cancelledValue = false;
            IBusinessObject boInDelegate = null;
            IDefaultBOEditorForm boEditorForm =
                GetControlFactory().CreateBOEditorForm((BusinessObject)bo, "default",
                delegate(IBusinessObject bo1, bool cancelled)
                {
                    delegateCalled = true;
                    cancelledValue = cancelled;
                    boInDelegate = bo1;
                });
            ShowFormIfNecessary(boEditorForm);
            EditControlValueOnForm(_defaultBOEditorForm, "TestProp", "TestValue");
            EditControlValueOnForm(_defaultBOEditorForm, "TestProp2", "TestValue2");
            //--------------Assert PreConditions----------------
            Assert.IsFalse(delegateCalled);
            //---------------Execute Test ----------------------
            boEditorForm.Close();
            //---------------Test Result -----------------------
            Assert.IsTrue(delegateCalled);
            Assert.IsTrue(cancelledValue);
            Assert.AreSame(bo, boInDelegate);
        }

        private static void EditControlValueOnForm(IDefaultBOEditorForm defaultBOEditorForm, string propertyName, string value)
        {
            defaultBOEditorForm.PanelInfo.FieldInfos[propertyName].ControlMapper.Control.Text = value;
        }
    }
}

//namespace VWGFormsTestExample
//{
//    using Gizmox.WebGUI.Forms;
//    using Gizmox.WebGUI.Common;
//    using NUnit.Framework;
//    using System.Threading;

//    [TestFixture]
//    public class TestFormVWG
//    {
//        private readonly Thread _thread;

//        public TestFormVWG()
//        {
//            _thread = new Thread(() => Gizmox.WebGUI.Client.Application.Run(typeof(TempForm)));
//            _thread.Start();
//            while (Global.Context == null) Thread.Sleep(100);
//        }

//        ~TestFormVWG()
//        {
//            _thread.Abort();
//        }

//        public class TempForm : Form
//        {
//            public TempForm()
//            {
//                Global.Context = this.Context;
//            }
//        }

//        [Test]
//        public void Test_ShowForm()
//        {
//            //---------------Set up test pack-------------------
//            Form form = new Form();
//            bool isLoaded = false;
//            form.Load += delegate { isLoaded = true; };
//            //---------------Assert Precondition----------------
//            Assert.IsFalse(isLoaded);
//            Assert.IsNotNull(form.Context);
//            Assert.IsNotNull(form.Context.Config); // Fails here
//            //---------------Execute Test ----------------------
//            form.Show();
//            //---------------Test Result -----------------------
//            Assert.IsTrue(isLoaded);
//        }
//    }
//}