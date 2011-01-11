using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.LayoutManager
{
    [TestFixture]
    public class TestColumnLayoutManagerVWG : TestColumnLayoutManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.VWG.ControlFactoryVWG();
        }
    }
}