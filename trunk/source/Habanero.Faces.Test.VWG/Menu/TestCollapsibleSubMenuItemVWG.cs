using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Menu
{
    [TestFixture]
    public class TestCollapsibleSubMenuItemVWG : TestCollapsibleSubMenuItem
    {
        protected override IMenuItem CreateControl()
        {
            return new CollapsibleSubMenuItemVWG(GetControlFactory(), TestUtil.GetRandomString());
        }
        protected override IMenuItem CreateControl(string name)
        {
            return new CollapsibleSubMenuItemVWG(GetControlFactory(), name);
        }
        protected override IMenuItem CreateControl(HabaneroMenu.Item item)
        {
            return new CollapsibleSubMenuItemVWG(GetControlFactory(), item);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}