using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
    [TestFixture]
    public class TestTreeViewControllerWin : TestTreeViewController
    {

        protected override IControlFactory GetControlFactory()
        {
            IControlFactory controlFactory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = controlFactory;
            return controlFactory;
        }
    }
}