using System;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Menu
{
    [TestFixture]
    public class TestCollapsibleMenuItemVWG : TestCollapsibleMenuItem
    {
        protected override IMenuItem CreateControl()
        {
            return new CollapsibleMenuItemVWG(TestUtil.GetRandomString());
        }

        protected override IMenuItem CreateControl(string name)
        {
            return new CollapsibleMenuItemVWG(name);
        }

        protected override IMenuItem CreateControl(HabaneroMenu.Item habaneroMenuItem)
        {
            return new CollapsibleMenuItemVWG(habaneroMenuItem);
        }

        protected override IMenuItem CreateControl(IControlFactory controlFactory, HabaneroMenu.Item habaneroMenuItem)
        {
            return new CollapsibleMenuItemVWG(controlFactory, habaneroMenuItem);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}