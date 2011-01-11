using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.StandardControls
{
    [TestFixture]
    public class TestLabelWin : TestLabel
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.Win.ControlFactoryWin();
        }
    }
}