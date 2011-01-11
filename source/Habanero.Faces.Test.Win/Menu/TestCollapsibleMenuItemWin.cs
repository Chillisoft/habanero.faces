using System;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Menu
{
    [TestFixture]
    public class TestCollapsibleMenuItemWin : TestCollapsibleMenuItem
    {

        protected override IMenuItem CreateControl()
        {
            return new CollapsibleMenuItemWin(TestUtil.GetRandomString());
        }

        protected override IMenuItem CreateControl(string name)
        {
            return new CollapsibleMenuItemWin(name);
        }

        protected override IMenuItem CreateControl(HabaneroMenu.Item habaneroMenuItem)
        {
            return new CollapsibleMenuItemWin(habaneroMenuItem);
        }

        protected override IMenuItem CreateControl(IControlFactory controlFactory, HabaneroMenu.Item habaneroMenuItem)
        {
            return new CollapsibleMenuItemWin(controlFactory, habaneroMenuItem);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}