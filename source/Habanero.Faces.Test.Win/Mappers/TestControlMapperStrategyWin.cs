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
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Test.Base.Mappers;
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Extensions.Forms;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Habanero.Faces.Test.Win.Mappers
{
    /// <summary>
    /// These are tested in their own class since the windows and gizmox behaviour are
    /// very different.
    /// </summary>
    [TestFixture]
    public class TestControlMapperStrategyWin//:TestBase
    {
        private ControlFactoryWin _factory = new Habanero.Faces.Win.ControlFactoryWin();

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
        }
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }
        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }

        [Test]
        public void Test_ControlMapperStrategy_AddBOPropHandlers()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            var strategyWin = new ControlMapperStrategyWin();
            var factory = new Habanero.Faces.Win.ControlFactoryWin();
            var tb = factory.CreateTextBox();
            const string testprop = "TestProp";
            var stubMapper = new ControlMapperStub(tb, testprop, false, factory);
            var bo = new MyBO();
            var prop = bo.Props[testprop];
            const string origvalue = "origValue";
            prop.Value = origvalue;
            stubMapper.BusinessObject = bo;
            strategyWin.RemoveCurrentBOPropHandlers(stubMapper, prop);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(origvalue, tb.Text);

            //---------------Execute Test ----------------------
            strategyWin.AddCurrentBOPropHandlers(stubMapper, prop);
            const string newValue = "New value";
            prop.Value = newValue;

            //---------------Test Result -----------------------
            Assert.AreEqual(newValue, tb.Text);

        }

        [Test]
        public void Test_ControlMapperStrategy_RemoveBOPropHandlers()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            var strategyWin = new ControlMapperStrategyWin();
            var tb = _factory.CreateTextBox();
            const string testprop = "TestProp";
            var stubMapper = new ControlMapperStub(tb, testprop, false, _factory);
            var bo = new MyBO();
            var prop = bo.Props[testprop];
            const string origvalue = "origValue";
            prop.Value = origvalue;
            stubMapper.BusinessObject = bo;


            const string newValue = "New value";
            //--------------Assert PreConditions----------------
            Assert.AreNotEqual(newValue, tb.Text);
            Assert.AreEqual(origvalue, prop.Value, "The text box value is set from the prop due to the fact that the BOPropHandler is on the ControlMapperStrategy");
            Assert.AreEqual(origvalue, tb.Text, "The text box value is set from the prop due to the fact that the BOPropHandler is on the ControlMapperStrategy");
            //---------------Execute Test ----------------------
            strategyWin.RemoveCurrentBOPropHandlers(stubMapper, prop);
            prop.Value = newValue;
            //---------------Test Result -----------------------
            Assert.AreNotEqual(newValue, tb.Text, "Updating the prop should not update the textbox since the handler has been removed");
            Assert.AreEqual(origvalue, tb.Text);
            Assert.AreEqual(newValue, prop.Value, "The text box value is not changed when the prop has changed due the the BOPropHandler being removed");
        }

        [Test]
        public void Test_GetFirstControl_OneControl_ShouldReturnControl()
        {
            //---------------Set up test pack-------------------
            var parentControl = _factory.CreateControl();
            var childControl = _factory.CreateControl();
            parentControl.Controls.Add(childControl);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(1, parentControl.Controls.Count);
            //---------------Execute Test ----------------------
            var firstControl = new ControlMapperStrategyWinSpy().CallGetFirstControl((Control)parentControl, (Control)childControl);
            //---------------Test Result -----------------------
            Assert.AreSame(childControl, firstControl);       
        }
        [Test]
        public void Test_GetFirstControl_TwoControl_ShouldReturnFirstControlInTabOrder()
        {
            //---------------Set up test pack-------------------
            var parentControl = _factory.CreateControl();
            var firstControlAdded = _factory.CreateControl();
            firstControlAdded.TabIndex = 0;
            parentControl.Controls.Add(firstControlAdded);
            var secondControlAdded = _factory.CreateControl();
            secondControlAdded.TabIndex = 1;
            parentControl.Controls.Add(secondControlAdded);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, parentControl.Controls.Count);
            Assert.Less(firstControlAdded.TabIndex, secondControlAdded.TabIndex);
            //---------------Execute Test ----------------------
            var actualFirstControl = new ControlMapperStrategyWinSpy().CallGetFirstControl((Control)parentControl, (Control)firstControlAdded);
            //---------------Test Result -----------------------
            Assert.AreSame(firstControlAdded, actualFirstControl);      
        }
        [Test]
        public void Test_GetFirstControl_TwoControl_ReverseTabOrder_ShouldReturnFirstControlInTabOrder()
        {
            //---------------Set up test pack-------------------
            var parentControl = _factory.CreateControl();
            var firstControlAddedToControls = _factory.CreateControl();
            firstControlAddedToControls.TabIndex = 1;
            parentControl.Controls.Add(firstControlAddedToControls);
            var secondControlAddedToControls = _factory.CreateControl();
            secondControlAddedToControls.TabIndex = 0;
            parentControl.Controls.Add(secondControlAddedToControls);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, parentControl.Controls.Count);
            Assert.Greater(firstControlAddedToControls.TabIndex, secondControlAddedToControls.TabIndex);
            //---------------Execute Test ----------------------
            Control firstControl = new ControlMapperStrategyWinSpy().CallGetFirstControl((Control)parentControl, (Control)firstControlAddedToControls);

            //---------------Test Result -----------------------
            Assert.AreSame(secondControlAddedToControls, firstControl);  
        }

        [Test]
        public void Test_GetFirstControl_WhenTabStopOnFirstControlIsFalse_ShouldReturnSecondControl_FIXBUG421()
        {
            //---------------Set up test pack-------------------
            var parentControl = _factory.CreateControl();

            var firstControl = _factory.CreateControl();
            firstControl.TabIndex = 0;
            firstControl.TabStop = false;
            parentControl.Controls.Add(firstControl);

            var secondControl = _factory.CreateControl();
            secondControl.TabIndex = 1;
            parentControl.Controls.Add(secondControl);
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, parentControl.Controls.Count);
            Assert.Less(firstControl.TabIndex, secondControl.TabIndex);
            Assert.IsFalse(firstControl.TabStop);
            Assert.IsTrue(secondControl.TabStop);
            //---------------Execute Test ----------------------
            var returnedFirstControl = new ControlMapperStrategyWinSpy().CallGetFirstControl((Control)parentControl, (Control)firstControl);
            //---------------Test Result -----------------------
            Assert.AreSame(secondControl, returnedFirstControl, "First Control should be ignored due to tab stop");
        }

        [Test]
        public void Test_GetFirstControl_MultipleControl_MixedTabOrder()
        {
            //---------------Set up test pack-------------------
            var parentControl = _factory.CreateControl();

            var childControl1 = _factory.CreateControl();
            childControl1.TabIndex = 2;
            parentControl.Controls.Add(childControl1);

            var childControl2 = _factory.CreateControl();
            childControl2.TabIndex = 0;
            childControl2.TabStop = false;
            parentControl.Controls.Add(childControl2);

            var childControl3 = _factory.CreateControl();
            childControl3.TabIndex = 1;
            parentControl.Controls.Add(childControl3);

            var childControl4 = _factory.CreateControl();
            childControl4.TabIndex = 3;
            parentControl.Controls.Add(childControl4);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(4, parentControl.Controls.Count);

            //---------------Execute Test ----------------------
            var firstControl = new ControlMapperStrategyWinSpy().CallGetFirstControl((Control)parentControl, (Control)childControl1);

            //---------------Test Result -----------------------
            Assert.AreSame(childControl3, firstControl);

            //---------------Tear Down -------------------------          
        }


        [Test]
        public void Test_GetNextControl_OneControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero parentControl = _factory.CreateControl();
            IControlHabanero childControl = _factory.CreateControl();
            parentControl.Controls.Add(childControl);

            //            ControlMapperStrategyWin strategyWin = new ControlMapperStrategyWin();
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(1, parentControl.Controls.Count);
            //---------------Execute Test ----------------------
            Control nextControl = new ControlMapperStrategyWinSpy().CallGetNextControlInTabOrder((Control)parentControl, (Control)childControl);
            //---------------Test Result -----------------------
            Assert.AreSame(childControl, nextControl);

            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_GetNextControl_TwoControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero parentControl = _factory.CreateControl();
            IControlHabanero childControl = _factory.CreateControl();
            childControl.TabIndex = 0;
            parentControl.Controls.Add(childControl);
            IControlHabanero childControl2 = _factory.CreateControl();
            childControl2.TabIndex = 1;
            parentControl.Controls.Add(childControl2);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, parentControl.Controls.Count);

            //---------------Execute Test ----------------------
            Control nextControl = new ControlMapperStrategyWinSpy().CallGetNextControlInTabOrder((Control)parentControl, (Control)childControl);

            //---------------Test Result -----------------------
            Assert.AreSame(childControl2, nextControl);

            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_GetNextControl_TwoControl_ReverseTabOrder()
        {
            //---------------Set up test pack-------------------
            IControlHabanero parentControl = _factory.CreateControl();
            IControlHabanero childControl = _factory.CreateControl();
            childControl.TabIndex = 1;
            parentControl.Controls.Add(childControl);
            IControlHabanero childControl2 = _factory.CreateControl();
            childControl2.TabIndex = 0;
            parentControl.Controls.Add(childControl2);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, parentControl.Controls.Count);

            //---------------Execute Test ----------------------
            Control nextControl = new ControlMapperStrategyWinSpy().CallGetNextControlInTabOrder((Control)parentControl, (Control)childControl);

            //---------------Test Result -----------------------
            Assert.AreSame(childControl2, nextControl);

            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_GetNextControl_MultipleControl_MixedTabOrder()
        {
            //---------------Set up test pack-------------------
            IControlHabanero parentControl = _factory.CreateControl();

            IControlHabanero childControl1 = _factory.CreateControl();
            childControl1.TabIndex = 2;
            parentControl.Controls.Add(childControl1);

            IControlHabanero childControl2 = _factory.CreateControl();
            childControl2.TabIndex = 0;
            childControl2.TabStop = false;
            parentControl.Controls.Add(childControl2);

            IControlHabanero childControl3 = _factory.CreateControl();
            childControl3.TabIndex = 1;
            parentControl.Controls.Add(childControl3);

            IControlHabanero childControl4 = _factory.CreateControl();
            childControl4.TabIndex = 1;
            childControl2.TabStop = false;
            parentControl.Controls.Add(childControl4);

            IControlHabanero childControl5 = _factory.CreateControl();
            childControl5.TabIndex = 3;
            parentControl.Controls.Add(childControl5);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(5, parentControl.Controls.Count);

            //---------------Execute Test ----------------------
            Control nextControl = new ControlMapperStrategyWinSpy().CallGetNextControlInTabOrder((Control)parentControl, (Control)childControl1);

            //---------------Test Result -----------------------
            Assert.AreSame(childControl5, nextControl);

            //---------------Tear Down -------------------------          
        }

        [Ignore("This test passes on the PC's, but not on the build server")]
        [Test]
        public void Test_KeyPressMovesFocusToNextControl()
        {
            //---------------Set up test pack-------------------
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
            var parentControl = _factory.CreateControl();
            var strategyWin = new ControlMapperStrategyWin();
            var textBox = _factory.CreateTextBox();
            textBox.Name = "TestTextBox";
            strategyWin.AddKeyPressEventHandler(textBox);

            parentControl.Controls.Add(textBox);

            var textBox2 = _factory.CreateTextBox();
            parentControl.Controls.Add(textBox2);
            var tbWin = (TextBoxWin)textBox2;
            var gotFocus = false;
            tbWin.GotFocus += delegate { gotFocus = true; };

            var frm = AddControlToForm(parentControl);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            frm.Show();
            var box = new TextBoxTester("TestTextBox");
            var eveArgsEnter = new KeyEventArgs(Keys.Enter);
            box.FireEvent("KeyUp", eveArgsEnter);

            //---------------Test Result -----------------------
            Assert.IsTrue(tbWin.ContainsFocus);
            Assert.IsTrue(gotFocus);
        }

        [Ignore("This test passes on the PC's, but not on the build server")]
        [Test]
        public void Test_HandleEnterKey_WhenTextBox_ShouldMoveToNextControl()
        {
            //---------------Set up test pack-------------------
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
            var parentControl = _factory.CreateControl();
            var strategyWin = new ControlMapperStrategyWinSpy();
            var textBox1 = GetTextBox(parentControl);
            strategyWin.AddKeyPressEventHandler(textBox1);

            var tbWin2 = (TextBoxWin)GetTextBox(parentControl);

            var gotFocus = false;
            tbWin2.GotFocus += delegate { gotFocus = true; };
            var frm = AddControlToForm(parentControl);
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, parentControl.Controls.Count);
            Assert.IsFalse(textBox1.Multiline);
            //---------------Execute Test ----------------------
            frm.Show();
            strategyWin.CallCtlKeyUpHandler();
            //---------------Test Result -----------------------
            Assert.IsTrue(tbWin2.ContainsFocus, "Textbox was expected to contain the focus");
            Assert.IsTrue(gotFocus, "Textbox should have received the gotfocus event");
        }

        [Ignore("This test passes on the PC's, but not on the build server")]
        [Test]
        public void Test_HandleEnterKey_WhenCheckBox_ShouldMoveToNextControl()
        {
            //---------------Set up test pack-------------------
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
            var parentControl = _factory.CreateControl();
            var strategyWin = new ControlMapperStrategyWinSpy();
            var checkBox = GetCheckBox(parentControl);
            strategyWin.AddKeyPressEventHandler(checkBox);

            var cbWin = (CheckBoxWin)GetCheckBox(parentControl);

            var gotFocus = false;
            cbWin.GotFocus += delegate { gotFocus = true; };
            var frm = AddControlToForm(parentControl);
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, parentControl.Controls.Count);
            //---------------Execute Test ----------------------
            frm.Show();
            strategyWin.CallCtlKeyUpHandler();
            //---------------Test Result -----------------------
            Assert.IsTrue(cbWin.ContainsFocus, "CheckBoxWin was expected to contain the focus");
            Assert.IsTrue(gotFocus, "CheckBoxWin should have received the gotfocus event");
        }

        [Test]
        public void Test_HandleKey_WhenNotEntryKey_WhenTextBox_ShouldNotMoveToNextControl()
        {
            //---------------Set up test pack-------------------
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
            var parentControl = _factory.CreateControl();
            var strategyWin = new ControlMapperStrategyWinSpy();
            var textBox1 = GetTextBox(parentControl);
            strategyWin.AddKeyPressEventHandler(textBox1);

            var tbWin2 = (TextBoxWin)GetTextBox(parentControl);

            var gotFocus = false;
            tbWin2.GotFocus += delegate { gotFocus = true; };
            var frm = AddControlToForm(parentControl);
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, parentControl.Controls.Count);
            Assert.IsFalse(textBox1.Multiline);
            //---------------Execute Test ----------------------
            frm.Show();
            strategyWin.CallCtlKeyUpHandler(Keys.Up);
            //---------------Test Result -----------------------
            Assert.IsFalse(tbWin2.ContainsFocus);
            Assert.IsFalse(gotFocus);
        }

        [Test]
        public void Test_HandleEnterKey_WhenTextBoxIsMultiLine_ShouldNotMoveFocusToNextControl_FixBug1419()
        {
            //---------------Set up test pack-------------------
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
            var parentControl = _factory.CreateControl();
            var strategyWin = new ControlMapperStrategyWinSpy();
            var textBox1 = GetTextBox(parentControl);
            textBox1.Multiline = true;
            strategyWin.AddKeyPressEventHandler(textBox1);

            var tbWin2 = (TextBoxWin)GetTextBox(parentControl);

            var gotFocus = false;
            tbWin2.GotFocus += delegate { gotFocus = true; };
            var frm = AddControlToForm(parentControl);
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, parentControl.Controls.Count);
            Assert.IsTrue(textBox1.Multiline);
            //---------------Execute Test ----------------------
            frm.Show();
            strategyWin.CallCtlKeyUpHandler(Keys.Enter);
            //---------------Test Result -----------------------
            Assert.IsFalse(tbWin2.ContainsFocus, "Second control should not have focus.");
            Assert.IsFalse(gotFocus);
        }

        private ITextBox GetTextBox(IControlHabanero parentControl)
        {
            var textBox2 = _factory.CreateTextBox();
            parentControl.Controls.Add(textBox2);
            return textBox2;
        }

        //[Test]
        //public void TestWin_CanPressKey()
        //{
        //    //---------------Set up test pack-------------------
        //    TextBox tb = new TextBox();
        //    tb.Name = "TestTextBox";
        //    Form frm = new Form();
        //    frm.Controls.Clear();
        //    frm.Controls.Add(tb);
        //    bool pressed = false;
        //    tb.KeyPress += delegate { pressed = true; };
        //    //--------------Assert PreConditions----------------            
        //    Assert.IsFalse(pressed);

        //    //---------------Execute Test ----------------------
        //    frm.Show();
        //    TextBoxTester box = new TextBoxTester("TestTextBox");
        //    Char pressChar = (char)0x013;
        //    KeyPressEventArgs eveArgs = new KeyPressEventArgs(pressChar);
        //    box.FireEvent("KeyPress", eveArgs);
        //    //                box.FireEvent("Click");

        //    //---------------Test Result -----------------------
        //    Assert.IsTrue(pressed);
        //}

        private Form AddControlToForm(IControlHabanero parentControl)
        {
            var frm = new Form();
            frm.Controls.Clear();
            frm.Controls.Add((Control)parentControl);
            return frm;
        }

        private ICheckBox GetCheckBox(IControlHabanero parentControl)
        {
            var checkBox = _factory.CreateCheckBox();
            parentControl.Controls.Add(checkBox);
            return checkBox;
        }

        class ControlMapperStrategyWinSpy : ControlMapperStrategyWin
        {
            public void CallCtlKeyUpHandler(Keys key)
            {
                this.CtlKeyUpHandler(this, new KeyEventArgs(key));
            }
            public void CallCtlKeyUpHandler()
            {
                CallCtlKeyUpHandler(Keys.Enter);
            }
            public Control CallGetNextControlInTabOrder(Control parentControl, Control control)
            {
                return ControlMapperStrategyWin.GetNextControlInTabOrder(parentControl, control);
            }
            public Control CallGetFirstControl(Control parentControl, Control control)
            {
                return ControlMapperStrategyWin.GetFirstControl(parentControl, control);
            }
        }

    }

  
}

// ReSharper restore InconsistentNaming