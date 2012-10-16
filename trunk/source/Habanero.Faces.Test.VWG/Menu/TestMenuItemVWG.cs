using Habanero.Faces.Base;
using Habanero.Faces.Test.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Menu
{
    [TestFixture]
    public class TestMenuItemVWG : TestMenuItem
    {
        private IControlFactory _factory;

        protected override IControlFactory GetControlFactory()
        {
            if ((_factory == null)) _factory = new ControlFactoryVWG();

            GlobalUIRegistry.ControlFactory = _factory;
            return _factory;
        }
    }
}