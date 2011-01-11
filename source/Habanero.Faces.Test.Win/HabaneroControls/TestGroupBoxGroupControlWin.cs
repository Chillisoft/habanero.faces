using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
    [TestFixture]
    public class TestGroupBoxGroupControlWin : TestGroupBoxGroupControl
    {
        protected override IControlFactory GetControlFactory()
        {
            Habanero.Faces.Win.ControlFactoryWin factory = new Habanero.Faces.Win.ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
    }
}