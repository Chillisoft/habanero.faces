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

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Faces.Base;

using Habanero.Util;
using NUnit.Framework;
using Rhino.Mocks;

// ReSharper disable InconsistentNaming
namespace Habanero.Faces.Test.Base.Mappers
{
	[TestFixture]
	public class TestControlMapperDirect
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
			GlobalRegistry.LoggerFactory = new HabaneroLoggerFactoryStub();
		}

		[TearDown]
		public void TestTearDown()
		{
			//Code that is executed after each and every test is executed in this fixture/class.
		}

		[Test]
		public void Test_SetEnabled_WithTrue_ShouldSetMappedControlEnabled_ToTrue()

		{
			//---------------Set up test pack-------------------
			var control = GenerateStub<IControlHabanero>();
			IControlMapper controlMapper = new ControlMapperStub2(control);
			//---------------Assert Precondition----------------
			Assert.IsFalse(control.Enabled);
			//---------------Execute Test ----------------------
			controlMapper.ControlEnabled = true;
			//---------------Test Result -----------------------
			Assert.IsTrue(control.Enabled);
		}

		[Test]
		public void Test_GetEnabled_WhenMappedControlEnabledIsFalse_ShouldRetFalse()

		{
			//---------------Set up test pack-------------------
			var control = GenerateStub<IControlHabanero>();
			IControlMapper controlMapper = new ControlMapperStub2(control);
			//---------------Assert Precondition----------------
			Assert.IsFalse(control.Enabled);
			//---------------Execute Test ----------------------
			var controlEnabled = controlMapper.ControlEnabled;
			//---------------Test Result -----------------------
			Assert.IsFalse(controlEnabled);
		}
		[Test]
		public void Test_GetEnabled_WhenMappedControlEnabledIsTrue_ShouldRetTrue()

		{
			//---------------Set up test pack-------------------
			var control = GenerateStub<IControlHabanero>();
			IControlMapper controlMapper = new ControlMapperStub2(control);
			control.Enabled = true;
			//---------------Assert Precondition----------------
			Assert.IsTrue(control.Enabled);
			//---------------Execute Test ----------------------
			var controlEnabled = controlMapper.ControlEnabled;
			//---------------Test Result -----------------------
			Assert.IsTrue(controlEnabled);
		}

		[Test]
		public void Test_SetEnabled_Withfalse_ShouldSetMappedControlEnabled_ToFalse()

		{
			//---------------Set up test pack-------------------
			var control = GenerateStub<IControlHabanero>();
			IControlMapper controlMapper = new ControlMapperStub2(control);
			control.Enabled = true;
			//---------------Assert Precondition----------------
			Assert.IsTrue(control.Enabled);
			//---------------Execute Test ----------------------
			controlMapper.ControlEnabled = false;
			//---------------Test Result -----------------------
			Assert.IsFalse(control.Enabled);
		}

		[Test]
		public void Test_IsReadOnly_Set_ShouldSet()
		{
			//---------------Set up test pack-------------------
			var control = GenerateStub<IControlHabanero>();
			IControlMapper mapper = new ControlMapperStub2(control);
			//---------------Assert Precondition----------------
			Assert.IsFalse(mapper.IsReadOnly);
			//---------------Execute Test ----------------------
			mapper.IsReadOnly = true;
			//---------------Test Result -----------------------
			Assert.IsTrue(mapper.IsReadOnly);
		}
		private static T GenerateStub<T>() where T : class
		{
			return MockRepository.GenerateStub<T>();
		}
	}

	class ControlMapperStub2 : ControlMapper
	{
		public ControlMapperStub2(IControlHabanero ctl)
			: base(ctl, TestUtil.GetRandomString(), false, MockRepository.GenerateStub <IControlFactory>())
		{
		}

		public override void ApplyChangesToBusinessObject()
		{
			throw new NotImplementedException();
		}

		protected override void InternalUpdateControlValueFromBo()
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// Summary description for TestControlMapper.
	/// </summary>
	public abstract class TestControlMapper 
	{
		protected abstract IControlFactory GetControlFactory();


		//Test ControlMapper

		protected ITextBox _txtNormal;
		protected ITextBox _txtReadonly;
		private ITextBox _txtReflectedProperty;
	    //TODO brett 30 Mar 2011: CF protected Shape _shape;
		protected TextBoxMapper _normalMapper;
		protected TextBoxMapper _readOnlyMapper;
		private TextBoxMapper _reflectedPropertyMapper;

		#region Setup for Tests

		[TestFixtureSetUp]
		public void SetupTestFixture()
		{
			BORegistry.DataAccessor = new DataAccessorInMemory();
		}

		[SetUp]
		public void Setup()
		{
			_txtReadonly = GetControlFactory().CreateTextBox();
			_readOnlyMapper = new TextBoxMapper(_txtReadonly, "ShapeName", true, GetControlFactory());
			_txtReflectedProperty = GetControlFactory().CreateTextBox();
			_reflectedPropertyMapper = new TextBoxMapper
				(_txtReflectedProperty, "-ShapeNameGetOnly-", false, GetControlFactory());
			_txtNormal = GetControlFactory().CreateTextBox();
			_normalMapper = new TextBoxMapper(_txtNormal, "ShapeName", false, GetControlFactory());
		    //TODO brett 30 Mar 2011: CF _shape = new Shape {ShapeName = "TestShapeName"};
		}

		#endregion //Setup for Tests

		#region Test Mapper Creation


		[Test]
		public void Test_Constructor()
		{
			//---------------Set up test pack-------------------
			IControlFactory controlFactory = GetControlFactory();
			ITextBox ctl = controlFactory.CreateTextBox();
			//---------------Execute Test ----------------------
			string propName = TestUtil.GetRandomString();
			ControlMapperStub mapper = new ControlMapperStub(ctl, propName, false, controlFactory);
			//---------------Test Result -----------------------
			Assert.IsInstanceOf(typeof (ControlMapper), mapper);
			Assert.AreSame(ctl, mapper.Control);
			Assert.AreEqual(propName, mapper.PropertyName);
			Assert.AreEqual(false, mapper.IsReadOnly);
			Assert.AreEqual(controlFactory, mapper.ControlFactory);
		}

		[Test]
		public void Test_Constructor_Readonly()
		{
			//---------------Set up test pack-------------------
			IControlFactory controlFactory = GetControlFactory();
			ITextBox ctl = controlFactory.CreateTextBox();
			//---------------Execute Test ----------------------
			string propName = TestUtil.GetRandomString();
			ControlMapperStub mapper = new ControlMapperStub(ctl, propName, true, controlFactory);
			//---------------Test Result -----------------------
			Assert.IsInstanceOf(typeof (ControlMapper), mapper);
			ControlMapper controlMapper = mapper;
			Assert.AreSame(ctl, controlMapper.Control);
			Assert.AreEqual(propName, controlMapper.PropertyName);
			Assert.AreEqual(true, controlMapper.IsReadOnly);
			Assert.AreEqual(controlFactory, controlMapper.ControlFactory);
		}
/*//TODO brett 30 Mar 2011: CF
		[Test]
		public void Test_SetClassDef()
		{
			//---------------Set up test pack-------------------
			IControlFactory controlFactory = GetControlFactory();
			ITextBox ctl = controlFactory.CreateTextBox();
			string propName = TestUtil.GetRandomString();
			ControlMapperStub mapper = new ControlMapperStub(ctl, propName, true, controlFactory);
			//---------------Assert Precondition----------------

			//---------------Execute Test ----------------------
			mapper.ClassDef = _shape.ClassDef;
			//---------------Test Result -----------------------
			Assert.AreSame(_shape.ClassDef, mapper.ClassDef);

		}*/
	    #endregion //Test Mapper Creation

	    #region Tests for normal mapper

	    [Test]
	    public void TestNormalEnablesControl()
	    {
	        Assert.IsFalse(_txtNormal.Enabled, "A normal control should be disabled before it gets and object");
	        //TODO brett 30 Mar 2011: CF _normalMapper.BusinessObject = _shape;
	        Assert.IsTrue(_txtNormal.Enabled, "A normal control should be editable once it has an object");
	    }

	    #endregion

	    #region Tests for read-only mapper

/*//TODO brett 30 Mar 2011: CF
	    [Test]
	    public void TestReadOnlyDisablesControl()
	    {
	        Assert.IsFalse(_txtReadonly.Enabled, "A read-only control should be disabled before it gets and object");
	        _readOnlyMapper.BusinessObject = _shape;
	        Assert.IsFalse(_txtReadonly.Enabled, "A read-only control should be disabled once it has an object");
	    }
*/

	    #endregion

	    #region Test Reflected Property Mapper
/*//TODO brett 30 Mar 2011: CF
	    [Test]
	    public void Test_ReflectedProperty_WithNoSet_DisablesControl()
	    {
	        //---------------Set up test pack-------------------
	        //-------------Assert Preconditions ----------------
	        Assert.IsFalse
	            (_txtReflectedProperty.Enabled,
	             "A reflected property control should be disabled before it gets an object");
	        //---------------Execute Test ----------------------
	        _reflectedPropertyMapper.BusinessObject = _shape;
	        //---------------Test Result -----------------------
	        Assert.IsFalse
	            (_txtReflectedProperty.Enabled, "A reflected property control should be disabled once it has an object");
	    }

	    [Test]
	    public void Test_ReflectedProperty_WithSet_EnablesControl()
	    {
	        //---------------Set up test pack-------------------
	        ITextBox txtReflectedPropertyWithSet = GetControlFactory().CreateTextBox();
	        TextBoxMapper reflectedPropertyWithSetMapper = new TextBoxMapper
	            (txtReflectedPropertyWithSet, "-ShapeName-", false, GetControlFactory());
	        //-------------Assert Preconditions ----------------
	        Assert.IsFalse
	            (txtReflectedPropertyWithSet.Enabled,
	             "A reflected property control should be disabled before it gets an object");
	        //---------------Execute Test ----------------------
	        reflectedPropertyWithSetMapper.BusinessObject = _shape;
	        //---------------Test Result -----------------------
	        Assert.IsTrue
	            (txtReflectedPropertyWithSet.Enabled,
	             "A reflected property control should be enabled once it has an object if the reflected property has a set");
	    }

	    [Test]
	    public void Test_ReflectedProperty_WithSet_WritesToReflectedProperty()
	    {
	        //---------------Set up test pack-------------------
	        ITextBox txtReflectedPropertyWithSet = GetControlFactory().CreateTextBox();
	        ControlMapperStub reflectedPropertyWithSetMapper = new ControlMapperStub
	            (txtReflectedPropertyWithSet, "-ShapeName-", false, GetControlFactory());
	        Shape shape = new Shape();
	        shape.ShapeName = TestUtil.GetRandomString();
	        string newValue = TestUtil.GetRandomString();
	        reflectedPropertyWithSetMapper.BusinessObject = shape;
	        //-------------Assert Preconditions ----------------
	        Assert.AreEqual(shape.ShapeName, txtReflectedPropertyWithSet.Text);
	        //---------------Execute Test ----------------------
	        reflectedPropertyWithSetMapper.TestSetPropertyValue(newValue);
	        //reflectedPropertyWithSetMapper.TestSetPropertyValue(newValue);
	        //---------------Test Result -----------------------
	        Assert.AreEqual(newValue, shape.ShapeName, "The reflected property should have been set");
	    }

	    // This test documents the limitation that the mapper will not respond to a change in the reflected value
	    [Test]
	    public void Test_ReflectedProperty_ChangePropValue_ControlNotUpdated()
	    {
	        //---------------Set up test pack-------------------
	        const string oldShapeName = "TestShapeName";
	        const string newShapeName = "TestShapeName2";
	        Shape shape = new Shape();
	        shape.ShapeName = oldShapeName;
	        _reflectedPropertyMapper.BusinessObject = shape;
	        //-------------Assert Preconditions ----------------
	        Assert.AreEqual(oldShapeName, _txtReflectedProperty.Text);
	        //---------------Execute Test ----------------------
	        _shape.ShapeName = newShapeName;
	        //---------------Test Result -----------------------
	        Assert.AreEqual
	            (oldShapeName, _txtReflectedProperty.Text,
	             "A Reflected property will not be able to pick up changes to the property.");
	    }

	    [Test]
	    public void Test_ReflectedProperty_ChangeBO()
	    {
	        //---------------Set up test pack-------------------
	        _reflectedPropertyMapper.BusinessObject = _shape;
	        Shape newShape = new Shape();
	        const string expectedShapeName = "Different";
	        newShape.ShapeName = expectedShapeName;
	        //-------------Assert Preconditions -------------
	        Assert.AreEqual(_shape.ShapeNameGetOnly, _txtReflectedProperty.Text);
	        //---------------Execute Test ----------------------
	        _reflectedPropertyMapper.BusinessObject = newShape;
	        //---------------Test Result -----------------------
	        Assert.AreEqual
	            (expectedShapeName, _txtReflectedProperty.Text,
	             "A Reflected property should refresh the value when a new BO is loaded");
	    }*/

	    #endregion

	    #region Test Null BO

/*	//TODO brett 30 Mar 2011: CF    [Test]
	    public void TestNullBoDisabled()
	    {
	        _normalMapper.BusinessObject = null;
	        Assert.IsFalse
	            (_txtNormal.Enabled,
	             "A control representing a null BO cannot be edited, so it should disable the control");
	        _normalMapper.BusinessObject = _shape;
	        Assert.IsTrue
	            (_txtNormal.Enabled,
	             "A non read-only control representing a BO can be edited, so it should enable the control");
	        _normalMapper.BusinessObject = null;
	        Assert.IsFalse
	            (_txtNormal.Enabled, "A control changed to a null BO cannot be edited, so it should disable the control");
	    }*/

	    #endregion //Test Null BO

	    #region TestIntRules

	    [Test]
	    public void Test_ErrorProvider_HasCorrectMessage_ForIntegerDataType_NoRule()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithIntegerRule();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp2", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("a");
	        //---------------Test Result -----------------------
	        StringAssert.Contains("It is not a type of System.Int32", mapperStub.ErrorProvider.GetError(_txtNormal));
	    }

	    [Test]
	    public void Test_ErrorProvider_HasCorrectMessage_ForIntegerDataType()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithIntegerRule();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory())
	                                           {BusinessObject = testBo};
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("a");
	        //---------------Test Result -----------------------
	        StringAssert.Contains("It is not a type of System.Int32", mapperStub.ErrorProvider.GetError(_txtNormal));
	    }

	    [Test]
	    public void Test_ErrorProvider_ValidIntegerString_HasNoErrorMessage()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithIntegerRule();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition----------------
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("3");
	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
	    }

	    [Test]
	    public void Test_ErrorProvider_NoError_Null_ForInt_WithRule()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithIntegerRule();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition----------------
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue(null);
	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
	    }

	    [Test]
	    public void Test_ErrorProvider_NoError_NullString_ForInt_WithRule()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithIntegerRule();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition----------------
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("");
	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
	    }

	    [Test]
	    public void Test_ErrorProvider_HasCorrectMessage_ForInt_LT_Min()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithIntegerRule();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition----------------
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("1");
	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        StringAssert.Contains("The value cannot be less than 2", errorMessage);
	    }

	    [Test]
	    public void Test_ErrorProvider_HasCorrectMessage_ForInt_GT_Max()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithIntegerRule();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory())
	                                           {BusinessObject = testBo};
	        //---------------Assert Precondition----------------
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("7");
	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        StringAssert.Contains("The value cannot be more than 5", errorMessage);
	    }

	    [Test]
	    public void Test_ErrorProvider_ClearsErrorMessage_AfterValidValueIsSet()
	    {
	        //---------------Set up test pack-------------------

	        ClassDef.ClassDefs.Clear();
	        var classDef = MyTestBO.LoadClassDefWithIntegerRule();
	        var testBo = new MyTestBO(classDef);
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestIntProp", false, GetControlFactory())
	                                           {BusinessObject = testBo};
	        mapperStub.TestSetPropertyValue("7");
	        //---------------Assert Precondition----------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        StringAssert.Contains("The value cannot be more than 5", errorMessage);
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("3");
	        //---------------Test Result -----------------------
	        errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        Assert.IsTrue(string.IsNullOrEmpty(errorMessage), errorMessage);
	    }

	    #endregion //TestIntRules

	    #region TestDecimalRules

	    [Test]
	    public void Test_ErrorProvider_HasCorrectMessage_ForDecimalDataType_NoRule()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithDecimalRule();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp2", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("a");
	        //---------------Test Result -----------------------
	        StringAssert.Contains("to its specified type of 'System.Decimal'", mapperStub.ErrorProvider.GetError(_txtNormal));
	    }

	    [Test]
	    public void Test_ErrorProvider_HasCorrectMessage_ForDecimalDataType()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithDecimalRule();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("a");
	        //---------------Test Result -----------------------
	        StringAssert.Contains("to its specified type of 'System.Decimal'", mapperStub.ErrorProvider.GetError(_txtNormal));
	    }

	    [Test]
	    public void Test_ErrorProvider_ValidDecimalString_HasNoErrorMessage()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithDecimalRule();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition----------------
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("3.03");
	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
	    }

	    [Test]
	    public void Test_ErrorProvider_Decimal_HasCorrectMessage_LT_Min()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithDecimalRule();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition----------------
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("1.05");
	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        StringAssert.Contains("The value cannot be less than 2", errorMessage);
	    }

	    [Test]
	    public void Test_ErrorProvider_Decimal_HasCorrectMessage_GT_Max()
	    {
	        //---------------Set up test pack-------------------

	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithDecimalRule();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition----------------
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("7.02");
	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        StringAssert.Contains("The value cannot be more than 5", errorMessage);
	    }

	    #endregion //TestDecimalRules

	    #region TestDateTimeRules

	    [Test]
	    public void Test_ErrorProvider_HasCorrectMessage_ForDateTimeDataType_SetToString_NoRule()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithDateTime();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestDateTime", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("Error");
	        //---------------Test Result -----------------------
	        StringAssert.Contains("It is not a type of System.DateTime", mapperStub.ErrorProvider.GetError(_txtNormal));
	    }

	    [Test]
	    public void Test_ErrorProvider_HasCorrectMessage_ForDateTimeDataType_SetToInt_NoRule()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithDateTime();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestDateTime", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue(5);
	        //---------------Test Result -----------------------
	        StringAssert.Contains("It is not a type of System.DateTime", mapperStub.ErrorProvider.GetError(_txtNormal));
	    }

	    [Test]
	    public void TestCanSetDateTimeProp_ToNullString_NonCompulsory()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithDateTime();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestDateTime", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition---- ------------
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("");
	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
	    }

	    [Test]
	    public void TestCanSetDateTimeProp_ToNull_NonCompulsory()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithDateTime();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestDateTime", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition----------------
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue(null);
	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Error returned : " + errorMessage);
	    }

	    [Test]
	    public void Test_DateTime_LT_CorrectErrorMessage()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithDateTime();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub
	            (_txtNormal, "TestDateTime2", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition----------------
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("2005/05/05");
	        //---------------Test Result -----------------------
	        StringAssert.Contains("The date cannot be before", mapperStub.ErrorProvider.GetError(_txtNormal));
	    }

	    [Test]
	    public void Test_DateTime_NoErrorMessage_passesRules()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithDateTime();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub
	            (_txtNormal, "TestDateTime2", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition----------------
	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("2005/06/12");
	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Error returned : " + errorMessage);
	    }

	    #endregion //TestDateTime

	    #region TestString

	    [Test]
	    public void TestCanSetStringProp_ToGuid()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithDateTime();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition---- ------------

	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue(Guid.NewGuid());

	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
	    }

	    [Test]
	    public void TestCanSetStringProp_ToDecimal()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithDateTime();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition---- ------------

	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue(10.22544m);

	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
	    }

	    [Test]
	    public void Test_ErrorProvider_HasCorrectMessage_ForStringDataType_HasRule()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithStringRule();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;

	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue(5);

	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
	    }

	    [Test]
	    public void TestCanSetStringProp_ToInt_NoRule()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithDateTime();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition---- ------------

	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue(10);

	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
	    }

	    [Test]
	    public void TestCanSetStringProp_ToDateTime()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithDateTime();
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition---- ------------

	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue(DateTime.Now);

	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
	    }

	    #endregion //TestString

	    #region LookupList

	    [Test]
	    public void TestCanSetIntProp_ItemInList()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithSimpleIntegerLookup(); //valid values 1, 2, 3
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = CreateControlMapperStub();
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition---- ------------

	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("1");

	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
	    }

	    private ControlMapperStub CreateControlMapperStub()
	    {
	        return new ControlMapperStub(_txtNormal, "TestProp2", false, GetControlFactory());
	    }

	    [Test]
	    public void TestCanSetGuidProp_ItemInList()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithLookup(); //valid values s1, s2
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp2", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition---- ------------

	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("s1");

	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
	    }

	    [Test]
	    public void TestCanSetIntProp_NullString_Compulsory()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithSimpleIntegerLookup(); //valid values 1, 2, 3
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp2", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition---- ------------

	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("");

	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        StringAssert.Contains
	            ("is a compulsory field and has no value", errorMessage,
	             "Should have no error. Error was : " + errorMessage);
	    }

	    [Test]
	    public void TestCanSetIntProp_Null_Compulsory()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithSimpleIntegerLookup(); //valid values 1, 2, 3
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp2", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition---- ------------

	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue(null);

	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        StringAssert.Contains
	            ("is a compulsory field and has no value", errorMessage,
	             "Should have no error. Error was : " + errorMessage);
	    }

	    [Test]
	    public void TestCanSetIntProp_NullString_NotCompulsory()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithSimpleIntegerLookup(); //valid values 1, 2, 3
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub
	            (_txtNormal, "SimpleLookupNotCompulsory", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition---- ------------

	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("");

	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
	    }

	    [Test]
	    public void TestCanSetIntProp_Null_NotCompulsory()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithSimpleIntegerLookup(); //valid values 1, 2, 3
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub
	            (_txtNormal, "SimpleLookupNotCompulsory", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;
	        //---------------Assert Precondition---- ------------

	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue(null);

	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
	    }

//        [Test, Ignore("Need to move tests to include BO lookups and then refactor.")]
	    public void TestCanSetIntProp_ItemNotInList()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithSimpleIntegerLookup(); //valid values 1, 2, 3
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp2", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;

	        //---------------Assert Precondition---- ------------

	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("5");

	        //---------------Test Result -----------------------
	        StringAssert.Contains("is not in list", mapperStub.ErrorProvider.GetError(_txtNormal));
	    }

	    [Test]
	    public void Test_CanSetGuidToStringLookupValue()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithLookup(); //Guid Lookup valid s1 and s2
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp2", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;

	        //---------------Assert Precondition----------------

	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("s1");

	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
	    }

//        [Test, Ignore("Need to move tests to include BO lookups and then refactor.")]
	    public void Test_NotCanSetGuidToStringLookupValue_InvalidValue()
	    {
	        //---------------Set up test pack-------------------
	        ClassDef.ClassDefs.Clear();
	        MyBO.LoadClassDefWithLookup(); //Guid Lookup valid s1 and s2
	        MyBO testBo = new MyBO();
	        ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp2", false, GetControlFactory());
	        mapperStub.BusinessObject = testBo;

	        //---------------Assert Precondition----------------

	        //---------------Execute Test ----------------------
	        mapperStub.TestSetPropertyValue("invalid");

	        //---------------Test Result -----------------------
	        string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
	        StringAssert.Contains("is not in list", errorMessage);
	    }

	    #endregion //LookupList

/*//TODO brett 30 Mar 2011: CF
		[Test]
		public void TestReadOnlyChangeBO()
		{
			_readOnlyMapper.BusinessObject = _shape;
			Assert.AreEqual("TestShapeName", _txtReadonly.Text);
			Shape sh2 = new Shape();
			sh2.ShapeName = "Different";
			_readOnlyMapper.BusinessObject = sh2;
			Assert.AreEqual("Different", _txtReadonly.Text);
			sh2.ShapeName = "Different2";
			_readOnlyMapper.UpdateControlValueFromBusinessObject();
			Assert.AreEqual("Different2", _txtReadonly.Text);
		}


		protected ITextBox GetTextBoxForShapeNameWhereShapeNameCompulsory
			(out Shape shape, out ControlMapperStub controlMapper)
		{
			ITextBox textBox = GetControlFactory().CreateTextBox();
			controlMapper = new ControlMapperStub(textBox, "ShapeName", false, GetControlFactory());
			shape = new Shape();
			ClassDef def = shape.ClassDef;
			IPropDef shapeNameDef = def.PropDefcol["ShapeName"];
			shapeNameDef.Compulsory = true;
			return textBox;
		}

		[Test]
		public void Test_SetBusinessObject_IfControlHasErrors_WhenBONull_ShouldClearErrorMessage()
		{
			//---------------Set up test pack-------------------
			Shape shape;
			ControlMapperStub mapperStub;
			ITextBox textBox = GetTextBoxForShapeNameWhereShapeNameCompulsory(out shape, out mapperStub);
			mapperStub.BusinessObject = shape;
			mapperStub.UpdateErrorProviderErrorMessage();
			//---------------Assert Precondition----------------
			Assert.IsFalse(mapperStub.BusinessObject.IsValid());
			Assert.AreNotEqual("", mapperStub.ErrorProvider.GetError(textBox));
			//---------------Execute Test ----------------------
			mapperStub.BusinessObject = null;
			//---------------Test Result -----------------------
			Assert.IsNull(mapperStub.CurrentBOProp());
			Assert.IsNull(mapperStub.BusinessObject);
			Assert.AreEqual("", mapperStub.ErrorProvider.GetError(textBox));
		}

		[Test]
		public void Test_SetBusinessObject_IfControlHasErrors_WhenBOValid_ShouldClearErrorMessage()
		{
			//---------------Set up test pack-------------------
			Shape shape;
			ControlMapperStub mapperStub;
			ITextBox textBox = GetTextBoxForShapeNameWhereShapeNameCompulsory(out shape, out mapperStub);
			mapperStub.BusinessObject = shape;
			mapperStub.UpdateErrorProviderErrorMessage();
			//---------------Assert Precondition----------------
			Assert.IsFalse(mapperStub.BusinessObject.IsValid());
			Assert.AreNotEqual("", mapperStub.ErrorProvider.GetError(textBox));
			//---------------Execute Test ----------------------
			mapperStub.BusinessObject = new Shape { ShapeName = "someName" };
			//---------------Test Result -----------------------
			Assert.IsTrue(mapperStub.BusinessObject.IsValid());
			Assert.AreEqual("", mapperStub.ErrorProvider.GetError(textBox));
		}

		[Test]
		public void Test_GetErrorProviderErrorMessage_WhenInvalid_ShouldReturnErrorMessageFromErrorProvider()
		{
			//---------------Set up test pack-------------------
			Shape shape;
			ControlMapperStub textBoxMapper;
			ITextBox textBox = GetTextBoxForShapeNameWhereShapeNameCompulsory(out shape, out textBoxMapper);
			textBoxMapper.BusinessObject = shape;
			shape.Status.IsValid();
			textBoxMapper.UpdateErrorProviderErrorMessage();
			string expectedErrorProviderErroMessage = textBoxMapper.ErrorProvider.GetError(textBox);
			//---------------Assert Precondition----------------
			Assert.IsFalse(shape.Status.IsValid());
			Assert.AreNotEqual("", expectedErrorProviderErroMessage);
			//---------------Execute Test ----------------------
			string message = textBoxMapper.GetErrorMessage();
			//---------------Test Result -----------------------
			Assert.AreEqual(expectedErrorProviderErroMessage, message);
		}

		[Test]
		public void Test_ControlDisabled_BoPropAuthorisation_CanUpdate_False()
		{
			//---------------Set up test pack-------------------
			Shape shape;
			ControlMapperStub textBoxMapper;
			ITextBox textBox = GetTextBoxForShapeNameWhereShapeNameCompulsory(out shape, out textBoxMapper);
			IBOPropAuthorisation propAuthorisationStub = GetPropAuthorisationStub_CanUpdate_False();
			BOProp prop1 = (BOProp)shape.Props["ShapeName"];
			prop1.SetAuthorisationRules(propAuthorisationStub);
			//---------------Assert Precondition----------------
			Assert.IsTrue(propAuthorisationStub.IsAuthorised(prop1, BOPropActions.CanRead));
			Assert.IsFalse(propAuthorisationStub.IsAuthorised(prop1, BOPropActions.CanUpdate));
			//---------------Execute Test ----------------------
			textBoxMapper.BusinessObject = shape;
			//---------------Test Result -----------------------
			Assert.IsFalse(textBox.Enabled);
		}*/

	    private static IBOPropAuthorisation GetPropAuthorisationStub_CanUpdate_False()
	    {
	        IBOPropAuthorisation authorisationStub = new BOPropAuthorisationStub();
	        authorisationStub.AddAuthorisedRole("A Role", BOPropActions.CanRead);
	        return authorisationStub;
	    }

	    internal class BOPropAuthorisationStub : IBOPropAuthorisation
	    {
	        private readonly List<BOPropActions> actionsAllowed = new List<BOPropActions>();

	        public void AddAuthorisedRole(string authorisedRole, BOPropActions actionToPerform)
	        {
	            if (actionsAllowed.Contains(actionToPerform)) return;

	            actionsAllowed.Add(actionToPerform);
	        }

	        public bool IsAuthorised(IBOProp prop,  BOPropActions actionToPerform)
	        {
	            return (actionsAllowed.Contains(actionToPerform));
	        }
	    }
	}

    public class ControlMapperStub : ControlMapper
    {
        public delegate void VoidMethod();
        private VoidMethod _onUpdateControlValueFromBusinessObject;

        public ControlMapperStub(IControlHabanero ctl, string propName, bool isReadOnly, IControlFactory factory)
            : base(ctl, propName, isReadOnly, factory)
        {
        }

        public VoidMethod OnUpdateControlValueFromBusinessObject
        {
            get { return _onUpdateControlValueFromBusinessObject; }
            set { _onUpdateControlValueFromBusinessObject = value; }
        }


        public override void UpdateControlValueFromBusinessObject()
        {
            if (_onUpdateControlValueFromBusinessObject != null)
            {
                _onUpdateControlValueFromBusinessObject();
            }
            base.UpdateControlValueFromBusinessObject();
        }

        public override void ApplyChangesToBusinessObject()
        {
            throw new NotImplementedException();
        }

        protected override void InternalUpdateControlValueFromBo()
        {
            this.Control.Text = Convert.ToString(base.GetPropertyValue());
        }

        public void TestSetPropertyValue(object value)
        {
            SetPropertyValue(value);
        }

    }

    /// <summary>
    /// Summary description for MyBO.
    /// </summary>
    [Serializable]
    public class MyTestBO : BusinessObject
    {


        public MyTestBO(IClassDef def)
            : base(def)
        {
            //_myRuleList = new List<IBusinessObjectRule>();
        }

        public MyTestBO()
        {
            //_myRuleList = new List<IBusinessObjectRule>();
        }

        protected MyTestBO(ConstructForFakes constructForFakes)
            : base(constructForFakes)
        {
        }
/*
		protected override IClassDef ConstructClassDef()
		{
			return _classDef;
		}*/

        public string TestProp
        {
            get
            {
                return (string)this.GetPropertyValue("TestProp");
            }
            set
            {
                this.SetPropertyValue("TestProp", value);
            }
        }
        public string TestIntProp
        {
            get
            {
                return (string)this.GetPropertyValue("TestIntProp");
            }
            set
            {
                this.SetPropertyValue("TestIntProp", value);
            }
        }
        public Guid? MyTestBoID
        {
            get
            {
                return (Guid?)this.GetPropertyValue("MyTestBoID");
            }
            set { this.SetPropertyValue("MyTestBoID", value); }
        }


        public bool TestBoolean
        {
            get { return (bool)this.GetPropertyValue("TestBoolean"); }
            set { this.SetPropertyValue("TestBoolean", value); }
        }
        public string TestProp2
        {
            get
            {
                return (string)this.GetPropertyValue("TestProp2");
            }
            set
            {
                this.SetPropertyValue("TestProp2", value);
            }
        }


        protected static XmlClassLoader CreateXmlClassLoader()
        {
            return new XmlClassLoader(new DtdLoader(), new DefClassFactory());
        }
/*
		public static IClassDef LoadClassDefsNoUIDef()
		{
			XmlClassLoader itsLoader = CreateXmlClassLoader();
			IClassDef itsClassDef =
				itsLoader.LoadClass(
					@"
				<class name=""MyBO"" assembly=""Habanero.Faces.Test.Base.Mappers"">
					<property  name=""MyTestBoID""  type=""Guid"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<property  name=""TestIntProp"" type=""Int32""/>
					<property  name=""TestBoolean"" type=""Boolean"" />
					<primaryKey>
						<prop name=""MyTestBoID"" />
					</primaryKey>
				</class>
			");
			ClassDef.ClassDefs.Add(itsClassDef);
			return itsClassDef;
		}*/

        public static IClassDef LoadClassDefWithIntegerRule()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyTestBO"" assembly=""Habanero.Faces.Test.Base.Mappers"">
					<property  name=""MyTestBoID"" type=""Guid""/>
					<property  name=""TestProp2"" />
					<property  name=""TestIntProp"" type=""Int32"">
						<rule name=""TestProp"">
							<add key=""min"" value=""2"" />
							<add key=""max"" value=""5"" />
						</rule>
					</property>
					<primaryKey>
						<prop name=""MyTestBoID"" />
					</primaryKey>
					<ui />                    
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }



        ///<returns>
        ///A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override string ToString()
        {
            if (Props.Contains("TestProp"))
            {
                return this.TestProp + " - " + this.MyTestBoID;

            }
            if (this.MyTestBoID == null)
            {
                return this.ClassDef.ClassNameFull;
            }
            return StringUtilities.GuidToUpper(this.MyTestBoID.GetValueOrDefault());
        }
/*
		protected override void ConstructFromClassDef(bool newObject)
		{
			if (_classDef == null) _classDef = ConstructClassDef();
			ClassDef classDef = (ClassDef)_classDef;
			//CheckClassDefNotNull();

			_boPropCol = classDef.CreateBOPropertyCol(newObject);
			_keysCol = classDef.CreateBOKeyCol(_boPropCol);

			//SetPrimaryKeyForInheritedClass();
			if (classDef.IsUsingSingleTableInheritance())
			{
				try
				{
					this.SetPropertyValue(classDef.SuperClassDef.Discriminator, this.GetType().Name);
				}
				catch (InvalidPropertyNameException ex)
				{
					throw new HabaneroDeveloperException(ex.Message + " Your discriminator field is not included in the properties of the class and you are using Single Table Inheritance. Please include the discriminator field as a property.");
				}
			}
			_relationshipCol = classDef.CreateRelationshipCol(_boPropCol, this);
		}

		/// <summary>
		/// Constructs a class definition
		/// </summary>
		/// <returns>Returns a class definition</returns>
		protected override IClassDef ConstructClassDef()
		{
			return ClassDef.ClassDefs.Contains(GetType()) ? ClassDef.ClassDefs[GetType()] : null;
		}*/

        /*
					public static IClassDef LoadDefaultClassDef()
					{

						XmlClassLoader itsLoader = CreateXmlClassLoader();
						IClassDef itsClassDef =
							itsLoader.LoadClass(
								@"
						<class name=""MyBO"" assembly=""Habanero.Faces.Test.Base.Mappers"">
							<property  name=""MyBoID"" type=""Guid"" />
							<property  name=""TestProp"" />
							<property  name=""TestProp2"" />
							<property  name=""TestIntProp"" />
							<primaryKey>
								<prop name=""MyBoID"" />
							</primaryKey>
							<ui>
								<grid>
									<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
									<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
								</grid>
								<form>
									<tab name=""Tab1"">
										<columnLayout>
											<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
											<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
										</columnLayout>
									</tab>
								</form>
							</ui>
							<ui name=""Alternate"">
								<grid>
									<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
								</grid>
								<form>
									<tab name=""Tab1"">
										<columnLayout>
											<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
										</columnLayout>
									</tab>
								</form>
							</ui> 
							<ui name=""AlternateNoGrid"">
								<form>
									<tab name=""Tab1"">
										<columnLayout>
											<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
										</columnLayout>
									</tab>
								</form>
							</ui>   
							<ui name=""AlternateVirtualProp"">
								<form>
									<tab name=""Tab1"">
										<columnLayout>
											<field label=""Test Prop"" property=""-MyTestProp-"" type=""TextBox"" mapperType=""TextBoxMapper"" />
										</columnLayout>
									</tab>
								</form>
							</ui>
				 
						</class>
					");
						ClassDef.ClassDefs.Add(itsClassDef);
						return itsClassDef;
					}*/

    }
}