using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Menu
{
    [TestFixture]
    public class TestCollapsibleMenuWin : TestCollapsibleMenu
    {

        protected override IMainMenuHabanero CreateControl()
        {
            return new CollapsibleMenuWin();
        }

        protected override IMainMenuHabanero CreateControl(HabaneroMenu menu)
        {
            return new CollapsibleMenuWin(menu);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}