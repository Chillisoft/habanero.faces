using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Menu
{
    [TestFixture]
    public class TestCollapsibleMenuVWG : TestCollapsibleMenu
    {
        protected override IMainMenuHabanero CreateControl()
        {
            return new CollapsibleMenuVWG();
        }

        protected override IMainMenuHabanero CreateControl(HabaneroMenu menu)
        {
            return new CollapsibleMenuVWG(menu);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}