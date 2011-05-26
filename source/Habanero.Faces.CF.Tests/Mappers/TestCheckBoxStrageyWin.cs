using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Logging;
using Habanero.Faces.Adapters;
using Habanero.Faces.Base;
using Habanero.Faces.Mappers;
using Habanero.Testability.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Tests.Mappers
{
    [TestFixture]
    public class TestCheckBoxStrageyWin 
    {

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            GlobalRegistry.LoggerFactory = new HabaneroLoggerFactoryStub();

        }
        protected IControlFactory GetControlFactory()
        {
            return new ControlFactoryCF();
        }

        [Test]
        public void Test_AddClickEventHandler_WhenMapperUsingHabaneroControl_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var checkBoxWin =  GetWinFormsControlAdapter();
            var checkBoxMapper = new CheckBoxMapperStub(checkBoxWin);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<ICheckBox>(checkBoxMapper.Control);
            //---------------Execute Test ----------------------
            var checkBoxStrategyWin = new CheckBoxStrategyCF();
            checkBoxStrategyWin.AddClickEventHandler(checkBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        [Test]
        public void Test_AddClickEventHandler_WhenMapperUsingControlAdapter_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var checkBoxWin = GetWinFormsControlAdapter();
            var checkBoxMapper = new CheckBoxMapperStub(checkBoxWin);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<CheckBox>(checkBoxMapper.GetControl());
            //---------------Execute Test ----------------------
            var checkBoxStrategyWin = new CheckBoxStrategyCF();
            checkBoxStrategyWin.AddClickEventHandler(checkBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        private static IWinFormsCheckBoxAdapter GetWinFormsControlAdapter()
        {
            var control = new CheckBox();
            var winFormsControlAdapter = MockRepository.GenerateStub<IWinFormsCheckBoxAdapter>();
            winFormsControlAdapter.Stub(adapter => adapter.WrappedControl).Return(control);
            return winFormsControlAdapter;
        }
    }

    public class CheckBoxMapperStub : CheckBoxMapper
    {
        public CheckBoxMapperStub(ICheckBox cb) : base(cb, "Fdfad", false, new ControlFactoryCF())
        {
        }
/*
        public override void ApplyChangesToBusinessObject()
        {
            ApplyChangesToBusinessObjectCalled = true;
        }

        public bool ApplyChangesToBusinessObjectCalled { get; private set; }

        public override void UpdateControlValueFromBusinessObject()
        {
            UpdateControlValueFromBusinessObjectCalled = true;
        }

        public bool UpdateControlValueFromBusinessObjectCalled { get; private set; }*/
    }
}