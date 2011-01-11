using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Menu
{
    /// <summary>
    /// This test class tests the CollapsiblePanel for VWG.
    /// </summary>
    [TestFixture]
    public class TestCollapsiblePanelVWG : TestCollapsiblePanel
    {

        protected override IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = new Habanero.Faces.VWG.ControlFactoryVWG();
            return GlobalUIRegistry.ControlFactory;
        }
        protected override ICollapsiblePanel CreateControl()
        {
            return GetControlFactory().CreateCollapsiblePanel();
        }
    }
}