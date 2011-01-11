using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;

namespace Habanero.Faces.Test.VWG.HabaneroControls
{
    public class TestTreeViewControllerVWG : TestTreeViewController
    {

        protected override IControlFactory GetControlFactory()
        {
            IControlFactory controlFactory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = controlFactory;
            return controlFactory;
        }
    }
}