using Habanero.Faces.Base;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Menu
{
    [TestFixture]
    public class TestMenuItemWin : TestMenuItem
    {
        private IControlFactory _factory;

        protected override IControlFactory GetControlFactory()
        {
            if ((_factory == null)) _factory = new ControlFactoryWin();

            GlobalUIRegistry.ControlFactory = _factory;
            return _factory;
        }
    }
}