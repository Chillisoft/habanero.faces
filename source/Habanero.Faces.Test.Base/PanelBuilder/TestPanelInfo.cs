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
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Test;
using Habanero.Test.BO;
using Habanero.Faces.Base;


using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    public abstract class TestPanelInfo
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        protected abstract IControlFactory GetControlFactory();

        [Test]
        public void TestPanel()
        {
            //---------------Set up test pack-------------------
            IPanelInfo panelInfo = new PanelInfo();
            IPanel panel = GetControlFactory().CreatePanel();
            //---------------Assert Precondition----------------
            Assert.IsNull(panelInfo.Panel);
            //---------------Execute Test ----------------------
            panelInfo.Panel = panel;
            //---------------Test Result -----------------------
            Assert.AreSame(panel, panelInfo.Panel);
        }
#pragma warning disable 168
        [Test]
        public void TestFieldInfos_WrongPropertyNameGivesUsefulError()
        {
            //---------------Set up test pack-------------------
            IPanelInfo panelInfo = new PanelInfo();

            //---------------Execute Test ----------------------
            try
            {
                PanelInfo.FieldInfo fieldInfo = panelInfo.FieldInfos["invalidPropName"];
                Assert.Fail("Expected to throw an InvalidPropertyNameException");
            }
                //---------------Test Result -----------------------
            catch (InvalidPropertyNameException ex)
            {
                StringAssert.Contains("A label for the property invalidPropName was not found", ex.Message);
            }
        }
#pragma warning restore 168

        [Test]
        public void TestFieldInfos()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = new PanelInfo();
            //---------------Test Result -----------------------
            Assert.IsNotNull(panelInfo.FieldInfos);
            Assert.AreEqual(0, panelInfo.FieldInfos.Count);
        }

        [Test]
        public void TestFieldInfo_Constructor()
        {
            //---------------Set up test pack-------------------
            ILabel label = GetControlFactory().CreateLabel();
            string propertyName = TestUtil.GetRandomString();
            ITextBox tb = GetControlFactory().CreateTextBox();
            IControlMapper controlMapper = new TextBoxMapper(tb, propertyName, false, GetControlFactory());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            PanelInfo.FieldInfo fieldInfo = new PanelInfo.FieldInfo(propertyName, label, controlMapper);

            //---------------Test Result -----------------------

            Assert.AreEqual(propertyName, fieldInfo.PropertyName);
            Assert.AreSame(label, fieldInfo.LabelControl);
            Assert.AreSame(controlMapper, fieldInfo.ControlMapper);
            Assert.AreSame(tb, fieldInfo.InputControl);
        }

        [Test]
        public void TestSetBusinessObjectUpdatesControlMappers()
        {
            //---------------Set up test pack-------------------
            Sample.CreateClassDefWithTwoPropsOneInteger();
            IPanelInfo panelInfo = new PanelInfo();
            panelInfo.FieldInfos.Add(CreateFieldInfo("SampleText"));
            panelInfo.FieldInfos.Add(CreateFieldInfo("SampleInt"));
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Sample sampleBO = new Sample();
            panelInfo.BusinessObject = sampleBO;
            //---------------Test Result -----------------------
            Assert.AreSame(sampleBO, panelInfo.BusinessObject);
            Assert.AreSame(sampleBO, panelInfo.FieldInfos[0].ControlMapper.BusinessObject);
            Assert.AreSame(sampleBO, panelInfo.FieldInfos[1].ControlMapper.BusinessObject);
        }

        [Test]
        public void TestControlsEnabled()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab((UIFormTab) classDef.UIDefCol["default"].UIForm[0]);
            panelInfo.BusinessObject = new Sample();
            //---------------Assert Precondition----------------
            Assert.IsTrue(panelInfo.FieldInfos[0].InputControl.Enabled);
            Assert.IsFalse(panelInfo.FieldInfos[1].InputControl.Enabled);
            //---------------Execute Test ----------------------
            panelInfo.ControlsEnabled = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(panelInfo.FieldInfos[0].InputControl.Enabled);
            Assert.IsFalse(panelInfo.FieldInfos[1].InputControl.Enabled);
        }

        [Test]
        public void TestControlsVisible()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab((UIFormTab) classDef.UIDefCol["default"].UIForm[0]);
            panelInfo.BusinessObject = new Sample();
            //---------------Assert Precondition----------------
            Assert.IsTrue(panelInfo.FieldInfos[0].InputControl.Visible);
            Assert.IsTrue(panelInfo.FieldInfos[1].InputControl.Visible);
            //---------------Execute Test ----------------------
            panelInfo.ControlsVisible = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(panelInfo.FieldInfos[0].InputControl.Visible);
            Assert.IsFalse(panelInfo.FieldInfos[1].InputControl.Visible);
        }

        [Test]
        public void TestPanelInfos()
        {
            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = new PanelInfo();
            //---------------Test Result -----------------------
            Assert.IsNotNull(panelInfo.PanelInfos);
            Assert.AreEqual(0, panelInfo.PanelInfos.Count);
        }

        [Test]
        public void TestClearErrorProviders()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneCompulsory();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab((UIFormTab) classDef.UIDefCol["default"].UIForm[0]);
            Sample businessObject = new Sample();

            panelInfo.BusinessObject = businessObject;

            //businessObject.SetPropertyValue("SampleText2", "sdlkfj");
            PanelInfo.FieldInfo fieldInfo = panelInfo.FieldInfos["SampleText2"];
            panelInfo.ApplyChangesToBusinessObject();
            IErrorProvider errorProvider = fieldInfo.ControlMapper.ErrorProvider;

            //---------------Assert Precondition----------------
            Assert.IsTrue(errorProvider.GetError(fieldInfo.InputControl).Length > 0);
            //---------------Execute Test ----------------------
            panelInfo.ClearErrorProviders();
            //---------------Test Result -----------------------
            Assert.IsFalse(errorProvider.GetError(fieldInfo.InputControl).Length > 0);
        }


        [Test]
        public void Test_UIFormTab()
        {
            //--------------- Set up test pack ------------------
            IClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab((UIFormTab) classDef.UIDefCol["default"].UIForm[0]);

            //--------------- Test Result -----------------------
            Assert.IsNotNull(panelInfo.UIFormTab);
            Assert.AreEqual(panelInfo.UIFormTab.Name, panelInfo.PanelTabText);
        }

        protected PanelInfo.FieldInfo CreateFieldInfo(string propertyName)
        {
            ILabel label = GetControlFactory().CreateLabel();
            ITextBox tb = GetControlFactory().CreateTextBox();
            IControlMapper controlMapper = new TextBoxMapper(tb, propertyName, false, GetControlFactory());
            GetControlFactory().CreateErrorProvider();
            return new PanelInfo.FieldInfo(propertyName, label, controlMapper);
        }

        [Test]
        public void Test_UpdateErrorProviderError_WhenBOValid_ShouldClearErrorMessage()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
            ContactPersonTestBO person = ContactPersonTestBO.CreateUnsavedContactPerson("", "");
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab((UIFormTab) person.ClassDef.UIDefCol["default"].UIForm[0]);
            panelInfo.BusinessObject = person;
            IControlMapper SurnameControlMapper = panelInfo.FieldInfos["Surname"].ControlMapper;
            panelInfo.UpdateErrorProvidersErrorMessages();
            //---------------Assert Precondition----------------
            Assert.AreNotEqual("", SurnameControlMapper.GetErrorMessage());
            //---------------Execute Test ----------------------
            person.Surname = "SomeValue";
            panelInfo.UpdateErrorProvidersErrorMessages();
            //---------------Test Result -----------------------
            Assert.AreEqual("", SurnameControlMapper.GetErrorMessage());
        }

    }


}