using System.Windows.Forms;
using Habanero.Faces.Base;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Test.Win.StandardControls
{
    [TestFixture]
    public class TestErrorProviderWin : TestErrorProvider
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        [Test]
        public void Test_GetError_WhenControl_ShouldReturnTheErrorFromTheBaseControl()
        {
            //---------------Set up test pack-------------------
            var control = new Control();
            var errorProviderWin = new ErrorProviderWin();
            var expectedErrorMessage = TestUtil.GetRandomString();
            errorProviderWin.SetError(control, expectedErrorMessage);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualErrorMessage = errorProviderWin.GetError(control);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedErrorMessage, actualErrorMessage);
        }
        [Test]
        public void Test_GetError_WhenWrappedControl_ShouldReturnTheErrorFromTheBaseControl()
        {
            //---------------Set up test pack-------------------
            var winFormsControlAdapter = GetWinFormsControlAdapter();
            var errorProviderWin = new ErrorProviderWin();
            var expectedErrorMessage = TestUtil.GetRandomString();
            errorProviderWin.SetError(winFormsControlAdapter, expectedErrorMessage);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualErrorMessage = errorProviderWin.GetError(winFormsControlAdapter);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedErrorMessage, actualErrorMessage);
        }

        private static IWinFormsControlAdapter GetWinFormsControlAdapter()
        {
            var control = new Control();
            var winFormsControlAdapter = MockRepository.GenerateStub<IWinFormsControlAdapter>();
            winFormsControlAdapter.Stub(adapter => adapter.WrappedControl).Return(control);
            return winFormsControlAdapter;
        }
    }
}