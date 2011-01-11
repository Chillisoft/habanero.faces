using Habanero.Faces.Test.Base.Controllers;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
    [TestFixture]
    public class TestListBoxCollectionManagerWin : TestListBoxCollectionManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.Win.ControlFactoryWin();
        }
    }
}