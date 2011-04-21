using System.Windows.Forms;
using Habanero.Faces.Base;
using Habanero.Faces.CF;
using Habanero.Faces.CF.Adapters;
using Habanero.Faces.CF.Controls;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Test.Win.StandardControls
{
    [TestFixture]
    public class TestComboBoxKeyPressMapperStrategyWin 
    {
        protected IControlFactory GetControlFactory()
        {
            return new ControlFactoryCF();
        }

        [Test]
        public void Test_AddHandlers_WhenMapperUsingHabaneroControl_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var comboBoxWin = GetWinFormsControlAdapter();
            var comboBoxMapper = new ComboBoxMapperStub(comboBoxWin);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<IComboBox>(comboBoxMapper.Control);
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new ComboBoxKeyPressMapperStrategyCF();
            comboBoxStrategyWin.AddHandlers(comboBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        [Test]
        public void Test_AddHandlers_WhenMapperUsingControlAdapter_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var comboBoxWin = GetWinFormsControlAdapter();
            var comboBoxMapper = new ComboBoxMapperStub(comboBoxWin);
            //---------------Assert ComboBoxMapperStub----------------
            Assert.IsInstanceOf<ComboBox>(comboBoxMapper.Control.GetControl());
            Assert.IsInstanceOf<ComboBox>(comboBoxMapper.GetControl());
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new ComboBoxKeyPressMapperStrategyCF();
            comboBoxStrategyWin.AddHandlers(comboBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        [Test]
        public void Test_RemoveCurrentHandlers_WhenMapperUsingHabaneroControl_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var comboBoxWin = GetWinFormsControlAdapter();
            var comboBoxMapper = new ComboBoxMapperStub(comboBoxWin);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<IComboBox>(comboBoxMapper.Control);
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new ComboBoxKeyPressMapperStrategyCF();
            comboBoxStrategyWin.RemoveCurrentHandlers(comboBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        [Test]
        public void Test_RemoveCurrentHandlers_WhenMapperUsingControlAdapter_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var comboBoxWin = GetWinFormsControlAdapter();
            var comboBoxMapper = new ComboBoxMapperStub(comboBoxWin);
            //---------------Assert ComboBoxMapperStub----------------
            Assert.IsInstanceOf<ComboBox>(comboBoxMapper.Control.GetControl());
            Assert.IsInstanceOf<ComboBox>(comboBoxMapper.GetControl());
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new ComboBoxKeyPressMapperStrategyCF();
            comboBoxStrategyWin.RemoveCurrentHandlers(comboBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        private static IWinFormsComboBoxAdapter GetWinFormsControlAdapter()
        {
            var control = new ComboBox();
            var winFormsControlAdapter = MockRepository.GenerateStub<IWinFormsComboBoxAdapter>();
            winFormsControlAdapter.Stub(adapter => adapter.WrappedControl).Return(control);
            return winFormsControlAdapter;
        }
    }
}