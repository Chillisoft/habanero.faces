using System.Windows.Forms;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Test.Win.StandardControls
{
    [TestFixture]
    public class TestListComboBoxMapperStrategyWin 
    {
        protected IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
        [Test]
        public void Test_AddItemSelectedEventHandler_WhenMapperUsingHabaneroControl_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var cmbWin = new ComboBoxWin() { Name = "TestComboBox", Enabled = true };
            var comboBoxMapper = new ListComboBoxMapperImpl(cmbWin);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<IComboBox>(comboBoxMapper.Control);
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new ListComboBoxMapperStrategyWin();
            comboBoxStrategyWin.AddItemSelectedEventHandler(comboBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        [Test]
        public void Test_AddItemSelectedEventHandler_WhenMapperUsingControlAdapter_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var cmbWin = GetWinFormsControlAdapter();
            var comboBoxMapper = new ListComboBoxMapperImpl(cmbWin);
            //---------------Assert ComboBoxMapperStub----------------
            Assert.IsInstanceOf<ComboBox>(comboBoxMapper.Control.GetControl());
            Assert.IsInstanceOf<ComboBox>(comboBoxMapper.GetControl());
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new ListComboBoxMapperStrategyWin();
            comboBoxStrategyWin.AddItemSelectedEventHandler(comboBoxMapper);
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

    internal class ListComboBoxMapperImpl : ListComboBoxMapper
    {
        public ListComboBoxMapperImpl(IControlHabanero ctl) : base(ctl, "PropName", false, new ControlFactoryWin())
        {
        }
    }
}