using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
    [TestFixture]
    public class TestInputFormDateWin : TestInputFormDate
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}