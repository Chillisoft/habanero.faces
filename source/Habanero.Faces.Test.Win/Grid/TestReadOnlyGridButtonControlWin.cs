using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Grid
{
    [TestFixture]
    public class TestReadOnlyGridButtonControlWin : TestReadOnlyGridButtonControl
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.Win.ControlFactoryWin();
        }

        protected override void AddControlToForm(IControlHabanero cntrl)
        {

        }

    }
}