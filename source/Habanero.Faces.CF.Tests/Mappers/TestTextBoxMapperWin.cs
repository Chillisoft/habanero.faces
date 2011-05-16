using System;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Faces.Base.CF.Tests;
using Habanero.Faces.Base.CF.Tests;
using Habanero.Faces.Base.CF.Tests.Selectors;
using Habanero.Faces.Mappers;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Test.Base.Mappers;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Mappers
{
    [TestFixture]
    public class TestTextBoxMapperCF : TestTextBoxMapper
    {
        // ReSharper disable InconsistentNaming
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryCF();
        }

        [Test]
        public void TestIsValidChar_ReturnsTrueIfBOPropNotSet()
        {
            //---------------Set up test pack-------------------
            var strategy =
                (TextBoxMapperStrategyCF)GetControlFactory().CreateTextBoxMapperStrategy();

            //---------------Assert pre-condition---------------
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(strategy.IsValidCharacter('a'));
            Assert.IsTrue(strategy.IsValidCharacter(' '));
            Assert.IsTrue(strategy.IsValidCharacter('.'));
            Assert.IsTrue(strategy.IsValidCharacter('-'));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_ReturnsTrueIfTextBoxNotSet()
        {
            //---------------Set up test pack-------------------
            var strategy =
                (TextBoxMapperStrategyCF)GetControlFactory().CreateTextBoxMapperStrategy();

            //---------------Assert pre-condition---------------
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(strategy.IsValidCharacter('a'));
            Assert.IsTrue(strategy.IsValidCharacter(' '));
            Assert.IsTrue(strategy.IsValidCharacter('.'));
            Assert.IsTrue(strategy.IsValidCharacter('-'));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithString_ReturnsTrueForNonNumericTypes()
        {
            //---------------Set up test pack-------------------
            TextBoxMapperStrategyCF strategy =
                (TextBoxMapperStrategyCF)GetControlFactory().CreateTextBoxMapperStrategy();
            BOProp boProp = CreateBOPropForType(typeof(string));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(strategy.IsValidCharacter('a'));
            Assert.IsTrue(strategy.IsValidCharacter(' '));
            Assert.IsTrue(strategy.IsValidCharacter('.'));
            Assert.IsTrue(strategy.IsValidCharacter('-'));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithInt_ReturnsTrueForNumber()
        {
            //---------------Set up test pack-------------------
            TextBoxMapperStrategyCF strategy =
                (TextBoxMapperStrategyCF)GetControlFactory().CreateTextBoxMapperStrategy();
            BOProp boProp = CreateBOPropForType(typeof(int));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(strategy.IsValidCharacter('0'));
            Assert.IsTrue(strategy.IsValidCharacter('9'));
            Assert.IsTrue(strategy.IsValidCharacter('-'));
            Assert.IsTrue(strategy.IsValidCharacter(Convert.ToChar(8)));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithInt_ReturnsFalseForNonNumber()
        {
            //---------------Set up test pack-------------------
            TextBoxMapperStrategyCF strategy =
                (TextBoxMapperStrategyCF)GetControlFactory().CreateTextBoxMapperStrategy();
            BOProp boProp = CreateBOPropForType(typeof(int));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(strategy.IsValidCharacter('a'));
            Assert.IsFalse(strategy.IsValidCharacter('A'));
            Assert.IsFalse(strategy.IsValidCharacter('+'));
            Assert.IsFalse(strategy.IsValidCharacter('.'));
            Assert.IsFalse(strategy.IsValidCharacter(Convert.ToChar(7)));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithInt_ReturnsTrueForNegativeAtStart()
        {
            //---------------Set up test pack-------------------
            var strategy =
                (TextBoxMapperStrategyCF)GetControlFactory().CreateTextBoxMapperStrategy();
            var boProp = CreateBOPropForType(typeof(int));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            _mapper.Control.Text = "123";
            ((TextBox)_mapper.Control.GetControl()).SelectionStart = 0;
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(strategy.IsValidCharacter('-'));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithInt_ReturnsFalseForNegativeAfterStart()
        {
            //---------------Set up test pack-------------------
            var strategy =
                (TextBoxMapperStrategyCF)GetControlFactory().CreateTextBoxMapperStrategy();
            var boProp = CreateBOPropForType(typeof(int));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            _mapper.Control.Text = "123";
            ((TextBox)_mapper.Control.GetControl()).SelectionStart = 2;
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(strategy.IsValidCharacter('-'));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithDecimal_ReturnsTrueForNumber()
        {
            //---------------Set up test pack-------------------
            var strategy =
                (TextBoxMapperStrategyCF)GetControlFactory().CreateTextBoxMapperStrategy();
            var boProp = CreateBOPropForType(typeof(decimal));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(strategy.IsValidCharacter('0'));
            Assert.IsTrue(strategy.IsValidCharacter('9'));
            Assert.IsTrue(strategy.IsValidCharacter('-'));
            Assert.IsTrue(strategy.IsValidCharacter(Convert.ToChar(8)));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithDecimal_ReturnsFalseForNonNumber()
        {
            //---------------Set up test pack-------------------
            var strategy =
                (TextBoxMapperStrategyCF)GetControlFactory().CreateTextBoxMapperStrategy();
            var boProp = CreateBOPropForType(typeof(decimal));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(strategy.IsValidCharacter('a'));
            Assert.IsFalse(strategy.IsValidCharacter('A'));
            Assert.IsFalse(strategy.IsValidCharacter('+'));
            Assert.IsFalse(strategy.IsValidCharacter(Convert.ToChar(7)));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithDecimal_ReturnsTrueForNegativeAtStart()
        {
            //---------------Set up test pack-------------------
            var strategy =
                (TextBoxMapperStrategyCF)GetControlFactory().CreateTextBoxMapperStrategy();
            var boProp = CreateBOPropForType(typeof(decimal));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            _mapper.Control.Text = "123";
            ((TextBox)_mapper.Control.GetControl()).SelectionStart = 0;
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(strategy.IsValidCharacter('-'));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestBoProp_ChangesWhen_TextBoxTextChanges()
        {
            //---------------Set up test pack-------------------
            _mapper.BusinessObject = _shape;
            var strategy =
                (TextBoxMapperStrategyCF)GetControlFactory().CreateTextBoxMapperStrategy();
            var boProp = _shape.Props["ShapeName"];
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            strategy.AddUpdateBoPropOnTextChangedHandler(_mapper, boProp);
            _mapper.Control.Text = "TestString";
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.AreEqual("TestString", boProp.Value);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithDecimal_ReturnsFalseForNegativeAfterStart()
        {
            //---------------Set up test pack-------------------
            var strategy =
                (TextBoxMapperStrategyCF)GetControlFactory().CreateTextBoxMapperStrategy();
            var boProp = CreateBOPropForType(typeof(decimal));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            _mapper.Control.Text = "123";
            ((TextBox)_mapper.Control.GetControl()).SelectionStart = 2;
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(strategy.IsValidCharacter('-'));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithDecimal_ReturnsTrueForDotNotAtStart()
        {
            //---------------Set up test pack-------------------
            var strategy =
                (TextBoxMapperStrategyCF)GetControlFactory().CreateTextBoxMapperStrategy();
            var boProp = CreateBOPropForType(typeof(decimal));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            _mapper.Control.Text = "123";
            ((TextBox)_mapper.Control.GetControl()).SelectionStart = 3;
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(strategy.IsValidCharacter('.'));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithDecimal_ReturnsFalseForMultipleDots()
        {
            //---------------Set up test pack-------------------
            var strategy =
                (TextBoxMapperStrategyCF)GetControlFactory().CreateTextBoxMapperStrategy();
            var boProp = CreateBOPropForType(typeof(decimal));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            _mapper.Control.Text = "12.3";
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(strategy.IsValidCharacter('.'));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithDecimal_AddsZeroForDotAtStart()
        {
            //---------------Set up test pack-------------------
            var strategy =
                (TextBoxMapperStrategyCF)GetControlFactory().CreateTextBoxMapperStrategy();
            var boProp = CreateBOPropForType(typeof(decimal));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            _mapper.Control.Text = "";
            var textBox = (TextBox)_mapper.Control.GetControl();
            textBox.SelectionStart = 0;
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(strategy.IsValidCharacter('.'));
            Assert.AreEqual("0.", textBox.Text);
            Assert.AreEqual(2, textBox.SelectionStart);
            Assert.AreEqual(0, textBox.SelectionLength);
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_TextBoxHasMapperStrategy()
        {
            //---------------Test Result -----------------------
            Assert.IsNotNull(_mapper.TextBoxMapperStrategy);
        }

        [Test]
        public void Test_CorrectTextBoxMapperStrategy()
        {
            //---------------Execute Test ----------------------
            var strategy = _mapper.TextBoxMapperStrategy;
            //---------------Test Result -----------------------
            Assert.AreSame(typeof(TextBoxMapperStrategyCF), strategy.GetType());
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_MapperStrategy_Returns_Correct_BoProp_WhenChangingBOs()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithIntegerRule();
            var myBo = new MyBO();
            _mapper = new TextBoxMapper(_textBox, "TestProp2", false, GetControlFactory());
            _mapper.BusinessObject = myBo;
            _textBox.Name = "TestTextBox";
            //---------------Assert pre-conditions--------------
            Assert.AreEqual(_mapper.CurrentBOProp(), ((TextBoxMapperStrategyCF)_mapper.TextBoxMapperStrategy).BoProp);
            Assert.AreEqual(_mapper.Control, ((TextBoxMapperStrategyCF)_mapper.TextBoxMapperStrategy).TextBoxControl);
            //---------------Execute Test ----------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            var myNewBo = new MyBO();
            _mapper.BusinessObject = myNewBo;
            //---------------Test Result -----------------------
            Assert.AreEqual(_mapper.CurrentBOProp(), ((TextBoxMapperStrategyCF)_mapper.TextBoxMapperStrategy).BoProp);
            //---------------Tear down -------------------------
        }

        private static BOProp CreateBOPropForType(Type type)
        {
            var propDef = new PropDef("Prop", type, PropReadWriteRule.ReadWrite, null);
            return new BOProp(propDef);
        }


    }
}