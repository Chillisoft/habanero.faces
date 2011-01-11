using Habanero.Faces.Test.Base.ButtonsControl;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win
{
    [TestFixture]
    public class TestButtonSizePolicyUserDefinedWin : TestButtonSizePolicyUserDefined
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}