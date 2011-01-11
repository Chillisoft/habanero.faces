using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.LayoutManager
{
    [TestFixture]
    public class TestLayoutManagerWin : TestLayoutManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}