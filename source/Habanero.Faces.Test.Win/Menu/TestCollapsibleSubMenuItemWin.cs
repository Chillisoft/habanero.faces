using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Menu
{
    [TestFixture]
    public class TestCollapsibleSubMenuItemWin : TestCollapsibleSubMenuItem
    {

        protected override IMenuItem CreateControl()
        {
            return new CollapsibleMenuItemWin(TestUtil.GetRandomString());
        }
        protected override IMenuItem CreateControl(string name)
        {
            return new CollapsibleSubMenuItemWin(GetControlFactory(), name);
        }
        protected override IMenuItem CreateControl(HabaneroMenu.Item item)
        {
            return new CollapsibleSubMenuItemWin(GetControlFactory(), item);
        }
        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}