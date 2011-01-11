using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Menu
{
    /// <summary>
    /// This test class tests the CollapsiblePanelGroupControl for VWG.
    /// </summary>
    [TestFixture]
    public class TestCollapsiblePanelGroupControlVWG : TestCollapsiblePanelGroupControl
    {
        protected override IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = new Habanero.Faces.VWG.ControlFactoryVWG();
            return GlobalUIRegistry.ControlFactory;
        }
    }
}