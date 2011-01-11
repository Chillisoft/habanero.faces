using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win
{
    [TestFixture]
    public class TestCalendarCellWin : TestCalendarCell
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.Win.ControlFactoryWin();
        }
    }
}