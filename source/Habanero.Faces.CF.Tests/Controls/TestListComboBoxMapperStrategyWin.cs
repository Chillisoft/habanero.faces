using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.Faces.Adapters;
using Habanero.Faces.Base;
using Habanero.Faces.Controls;
using Habanero.Testability.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Test.Win.StandardControls
{
    [TestFixture]
    public class TestListComboBoxMapperStrategyWin
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GlobalRegistry.LoggerFactory = new HabaneroLoggerFactoryStub();

        }
        protected IControlFactory GetControlFactory()
        {
            return new ControlFactoryCF();
        }
        [Test]
        public void Test_AddItemSelectedEventHandler_WhenMapperUsingHabaneroControl_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var cmbWin = GetWinFormsControlAdapter();
            var comboBoxMapper = new ListComboBoxMapperStub(cmbWin);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<IComboBox>(comboBoxMapper.Control);
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new ListComboBoxMapperStrategyCF();
            comboBoxStrategyWin.AddItemSelectedEventHandler(comboBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        [Test]
        public void Test_AddItemSelectedEventHandler_WhenMapperUsingControlAdapter_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var cmbWin = GetWinFormsControlAdapter();
            var comboBoxMapper = new ListComboBoxMapperStub(cmbWin);
            //---------------Assert ComboBoxMapperStub----------------
            Assert.IsInstanceOf<ComboBox>(comboBoxMapper.Control.GetControl());
            Assert.IsInstanceOf<ComboBox>(comboBoxMapper.GetControl());
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new ListComboBoxMapperStrategyCF();
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

    internal class ListComboBoxMapperStub : ListComboBoxMapper
    {
        public ListComboBoxMapperStub(IControlHabanero ctl) : base(ctl, "PropName", false, new ControlFactoryCF())
        {
        }
    }
}