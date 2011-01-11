using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.LayoutManager
{
    [TestFixture]
    public class TestBorderLayoutManagerVWG : TestBorderLayoutManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }
        //            [Test,Ignore("Needs to be implemented.")]
        [Test]
        public void TestSplitterIsIgnoredForVWG() { }
    }
}