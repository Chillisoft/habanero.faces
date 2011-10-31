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
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Test.Win.StandardControls
{
    [TestFixture]
    public class TestTextBoxMapperStrategyWin 
    {

        [Test]
        public void Test_AddKeyPressEventHandler_WhenMapperUsingHabaneroControl_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var txt = new TextBoxWin { Name = "TestTextBox", Enabled = true };
            var textBoxMapper = new TextBoxMapperStub(txt);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<ITextBox>(textBoxMapper.Control);
        //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new TextBoxMapperStrategyWin();
            comboBoxStrategyWin.AddKeyPressEventHandler(textBoxMapper, GenerateStub<IBOProp>());
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        private static T GenerateStub<T>() where T : class
        {
            return MockRepository.GenerateStub<T>();
        }

        [Test]
        public void Test_AddKeyPressEventHandler_WhenMapperUsingControlAdapter_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var txt = GetWinFormsControlAdapter();
            var textBoxMapper = new TextBoxMapperStub(txt);
            //---------------Assert textBoxMapperStub----------------
            Assert.IsInstanceOf<TextBox>(textBoxMapper.Control.GetControl());
            Assert.IsInstanceOf<TextBox>(textBoxMapper.GetControl());
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new TextBoxMapperStrategyWin();
            comboBoxStrategyWin.AddKeyPressEventHandler(textBoxMapper, GenerateStub<IBOProp>());
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }
        [Test]
        public void Test_AddUpdateBoPropOnValueChangedHandler_WhenMapperUsingHabaneroControl_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var txt = new TextBoxWin { Name = "TestTextBox", Enabled = true };
            var textBoxMapper = new TextBoxMapperStub(txt);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<ITextBox>(textBoxMapper.Control);
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new TextBoxMapperStrategyWin();
            comboBoxStrategyWin.AddUpdateBoPropOnTextChangedHandler(textBoxMapper, GenerateStub<IBOProp>());
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        [Test]
        public void Test_AddUpdateBoPropOnValueChangedHandler_WhenMapperUsingControlAdapter_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var txt = GetWinFormsControlAdapter();
            var textBoxMapper = new TextBoxMapperStub(txt);
            //---------------Assert textBoxMapperStub----------------
            Assert.IsInstanceOf<TextBox>(textBoxMapper.Control.GetControl());
            Assert.IsInstanceOf<TextBox>(textBoxMapper.GetControl());
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new TextBoxMapperStrategyWin();
            comboBoxStrategyWin.AddUpdateBoPropOnTextChangedHandler(textBoxMapper, GenerateStub<IBOProp>());
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        private static IWinFormsTextBoxAdapter GetWinFormsControlAdapter()
        {
            var control = new TextBox();
            var winFormsControlAdapter = MockRepository.GenerateStub<IWinFormsTextBoxAdapter>();
            winFormsControlAdapter.Stub(adapter => adapter.WrappedControl).Return(control);
            return winFormsControlAdapter;
        }
    }

    class TextBoxMapperStub : TextBoxMapper
    {
        public TextBoxMapperStub(ITextBox tb) : base(tb, "", false, new ControlFactoryWin())
        {
        }
    }
}