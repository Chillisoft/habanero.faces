using System.Windows.Forms;
using Habanero.Faces.CF;
using Habanero.Faces.CF.Controls;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using NUnit.Framework;
using Rhino.Mocks;

// ReSharper disable InconsistentNaming
namespace Habanero.Faces.Test.Win
{
    [TestFixture]
    public class TestControlCollectionCF : TestControlCollection
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryCF();
        }
        [Test]
        public void TestAddControl()
        {
            var tb = GetControlFactory().CreateTextBox();
            IControlCollection col = new ControlCollectionCF(new System.Windows.Forms.Control.ControlCollection(tb.GetControl()));
            IControlHabanero ctl = GetControlFactory().CreateControl();
            col.Add(ctl);
            Assert.AreSame(ctl.GetControl(), col[0].GetControl(), "Control added should be the same object.");
        }

        [Test]
        public void Test_Add_WhenIsAdaptedControl_ShouldUseWrappedControl()

        {
            //---------------Set up test pack-------------------
            var adapter = GetWinFormsControlAdapter();
            IControlCollection col = CreateControlCollectionCF();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(adapter.WrappedControl);
            Assert.AreEqual(0, col.Count);
            //---------------Execute Test ----------------------
            col.Add(adapter);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            var controlHabanero = col[0] as IWinFormsControlAdapter; 
            Assert.IsNotNull(controlHabanero);
            Assert.AreSame(adapter.WrappedControl, controlHabanero.WrappedControl);
        }

        [Test]
        public void Test_this_ShouldWrapControlIfItIsNotAlreadyWrapped()
        {
            //---------------Set up test pack-------------------
            IControlCollection col = CreateControlCollectionCF();
            var adapter = GetWinFormsControlAdapter();
            col.Add(adapter);
            Assert.AreEqual(1, col.Count);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(adapter.WrappedControl);
            Assert.AreEqual(1, col.Count);
            //---------------Execute Test ----------------------
            var controlHabanero = col[0] as IWinFormsControlAdapter;
            //---------------Test Result -----------------------
            Assert.IsNotNull(controlHabanero);
            Assert.AreSame(adapter.WrappedControl, controlHabanero.WrappedControl);
        }
        [Test]
        public void Test_IndexOf_ShouldUseWrappedControl()
        {
            //---------------Set up test pack-------------------
            IControlCollection col = CreateControlCollectionCF();
            var adapter = GetWinFormsControlAdapter();
            col.Add(adapter);
            Assert.AreEqual(1, col.Count);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(adapter.WrappedControl);
            Assert.AreEqual(1, col.Count);
            //---------------Execute Test ----------------------
            var indexOf = col.IndexOf(adapter);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, indexOf);
        }

        private ControlCollectionCF CreateControlCollectionCF()
        {
            return new ControlCollectionCF(new System.Windows.Forms.Control.ControlCollection(new Control()));
        }

        //index of
        //this
        private static IWinFormsControlAdapter GetWinFormsControlAdapter()
        {
            var control = new System.Windows.Forms.Control();
            var winFormsControlAdapter = MockRepository.GenerateStub<IWinFormsControlAdapter>();
            winFormsControlAdapter.Stub(adapter => adapter.WrappedControl).Return(control);
            return winFormsControlAdapter;
        }
    }
}