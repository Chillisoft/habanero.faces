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