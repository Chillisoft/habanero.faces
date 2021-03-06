//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Framework;
using Rhino.Mocks;

// ReSharper disable InconsistentNaming
namespace Habanero.Faces.Test.Base.Mappers
{
    [TestFixture]
    public class TestControlMapperCollectionDirectoy
    {

        [SetUp]
        public void TestSetup()
        {
            //Executes once per test before the test is executed.
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TestTearDown()
        {
            //Code that is executed after each and every test is executed in this fixture/class.
        }
        [Test]
        public void Test_Add_ShouldAddControlMapper()
        {
            //---------------Set up test pack-------------------
            IControlMapperCollection controlMapperCollection = new ControlMapperCollection();
            var controlMapper = MockRepository.GenerateStub<IControlMapper>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, controlMapperCollection.Count);
            //---------------Execute Test ----------------------
            controlMapperCollection.Add(controlMapper);
            //---------------Test Result -----------------------
            Assert.AreSame(controlMapper, controlMapperCollection[0]);
        }
        [Test]
        public void Test_Add_ShouldReturnAddedControlMapper()
        {
            //---------------Set up test pack-------------------
            IControlMapperCollection controlMapperCollection = new ControlMapperCollection();
            var controlMapper = MockRepository.GenerateStub<IControlMapper>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, controlMapperCollection.Count);
            //---------------Execute Test ----------------------
            var addedMapper = controlMapperCollection.Add(controlMapper);
            //---------------Test Result -----------------------
            Assert.AreSame(controlMapper, addedMapper);
        }

        [Test]
        public void Test_SetBusinessObject_ShouldSet()
        {
            //---------------Set up test pack-------------------

            var expectedBO = GenerateStub<IBusinessObject>();
            IControlMapperCollection controlMapperCollection = new ControlMapperCollection();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            controlMapperCollection.BusinessObject = expectedBO;
            //---------------Test Result -----------------------
            Assert.AreSame(expectedBO, controlMapperCollection.BusinessObject);
        }

        [Test]
        public void Test_SetBusinessObject_ShouldSetBoOnControlMappers()
        {
            //---------------Set up test pack-------------------

            IControlMapperCollection controlMapperCollection = new ControlMapperCollection();
            var controlMapper = MockRepository.GenerateStub<IControlMapper>();
            controlMapperCollection.Add(controlMapper);
            var expectedBO = GenerateStub<IBusinessObject>();
            //---------------Assert Precondition----------------
            Assert.AreSame(controlMapper, controlMapperCollection[0]);
            //---------------Execute Test ----------------------
            controlMapperCollection.BusinessObject = expectedBO;
            //---------------Test Result -----------------------
            Assert.AreSame(expectedBO, controlMapper.BusinessObject);
        }
        [Test]
        public void Test_SetControlsEnabled_WithTrue_ShouldSetEnabledTrueOnControlMappers()
        {
            //---------------Set up test pack-------------------

            IControlMapperCollection controlMapperCollection = new ControlMapperCollection();
            var controlMapper = MockRepository.GenerateStub<IControlMapper>();
            controlMapperCollection.Add(controlMapper);
            //---------------Assert Precondition----------------
            Assert.AreSame(controlMapper, controlMapperCollection[0]);
            //---------------Execute Test ----------------------
            controlMapperCollection.ControlsEnabled = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(controlMapper.ControlEnabled);
        }


        private static T GenerateStub<T>() where T : class
        {
            return MockRepository.GenerateStub<T>();
        }
    }
    public abstract class TestControlMapperCollection 
    {
        protected abstract IControlFactory GetControlFactory();
        protected const string START_VALUE_1 = "StartValue1";
        protected const string START_VALUE_2 = "StartValue2";
        protected const string TEST_PROP_1 = "TestProp";
        protected const string TEST_PROP_2 = "TestProp2";
        protected const string CHANGED_VALUE_1 = "ChangedValue1";
        protected const string CHANGED_VALUE_2 = "ChangedValue2";
 
        


        [SetUp]
        public void TestSetup()
        {
            ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TestTearDown()
        {
            //Code that is executed after each and every test is executed in this fixture/class.
        }

        [Test]
        public void TestPanelSetup()
        {
            //---------------Set up test pack-------------------

            MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            myBO.TestProp = START_VALUE_1;
            myBO.SetPropertyValue(TEST_PROP_2, START_VALUE_2);
            PanelBuilder factory = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = factory.BuildPanelForForm(myBO.ClassDef.UIDefCol["default"].UIForm);
            panelInfo.BusinessObject = myBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(START_VALUE_1, panelInfo.FieldInfos[TEST_PROP_1].ControlMapper.Control.Text);
            Assert.AreEqual(START_VALUE_2, panelInfo.FieldInfos[TEST_PROP_2].ControlMapper.Control.Text);
        }


        protected static void ChangeValuesInControls(IPanelInfo panelInfo)
        {
            panelInfo.FieldInfos[TEST_PROP_1].ControlMapper.Control.Text = CHANGED_VALUE_1;
            panelInfo.FieldInfos[TEST_PROP_2].ControlMapper.Control.Text = CHANGED_VALUE_2;
        }

        [Test]
        public void TestApplyChangesToBusinessObject()
        {
            //---------------Set up test pack-------------------

            MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            myBO.TestProp = START_VALUE_1;
            myBO.SetPropertyValue(TEST_PROP_2, START_VALUE_2);

            PanelBuilder factory = new PanelBuilder(GetControlFactory());
            IPanelInfo panelInfo = factory.BuildPanelForForm(myBO.ClassDef.UIDefCol["default"].UIForm);
            panelInfo.BusinessObject = myBO;
            ChangeValuesInControls(panelInfo);

            //---------------Execute Test ----------------------

            panelInfo.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------

            Assert.AreEqual(CHANGED_VALUE_1, myBO.GetPropertyValue(TEST_PROP_1));
            Assert.AreEqual(CHANGED_VALUE_2, myBO.GetPropertyValue(TEST_PROP_2));
        }

        [Test]
        public void TestDisableControls()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            PanelBuilder factory = new PanelBuilder(GetControlFactory());
            IPanelInfo panelInfo = factory.BuildPanelForForm(myBO.ClassDef.UIDefCol["default"].UIForm);
            panelInfo.BusinessObject = myBO;
            //---------------Assert precondition----------------
            Assert.IsTrue(panelInfo.FieldInfos[TEST_PROP_1].ControlMapper.Control.Enabled);
        
            //---------------Execute Test ----------------------
            panelInfo.ControlsEnabled = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(panelInfo.FieldInfos[TEST_PROP_1].ControlMapper.Control.Enabled);
            Assert.IsFalse(panelInfo.FieldInfos[TEST_PROP_2].ControlMapper.Control.Enabled);
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestEnableControls()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            PanelBuilder factory = new PanelBuilder(GetControlFactory());
            IPanelInfo panelInfo = factory.BuildPanelForForm(myBO.ClassDef.UIDefCol["default"].UIForm);
            panelInfo.BusinessObject = myBO;
            panelInfo.ControlsEnabled = false;
             
            //---------------Execute Test ----------------------
            panelInfo.ControlsEnabled = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(panelInfo.FieldInfos[TEST_PROP_1].ControlMapper.Control.Enabled);
            Assert.IsTrue(panelInfo.FieldInfos[TEST_PROP_2].ControlMapper.Control.Enabled);
            //---------------Tear Down -------------------------
        }

    }
}
