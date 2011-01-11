using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.StandardControls
{
    [TestFixture]
    public class TestListBoxWin : TestListBox
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}