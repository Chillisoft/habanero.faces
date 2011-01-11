using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Menu
{
    /// <summary>
    /// This test class tests the CollapsiblePanel for Win.
    /// </summary>
    [TestFixture]
    public class TestCollapsiblePanelWin : TestCollapsiblePanel
    {
        protected override IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = new Habanero.Faces.Win.ControlFactoryWin();
            return GlobalUIRegistry.ControlFactory;
        }

        protected override ICollapsiblePanel CreateControl()
        {
            return GetControlFactory().CreateCollapsiblePanel();
        }
    }
}