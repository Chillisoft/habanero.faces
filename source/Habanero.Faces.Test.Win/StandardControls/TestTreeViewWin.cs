using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.StandardControls
{
    /// <summary>
    /// This test class tests the TreeView class.
    /// </summary>
    [TestFixture]
    public class TestTreeViewWin : TestTreeView
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
    }
}