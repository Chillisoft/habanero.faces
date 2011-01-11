using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Menu
{
    [TestFixture]
    public class TestCollapsiblePanelGroupControlWin : TestCollapsiblePanelGroupControl
    {
        protected override IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = new Habanero.Faces.Win.ControlFactoryWin();
            return GlobalUIRegistry.ControlFactory;
        }
    }
}