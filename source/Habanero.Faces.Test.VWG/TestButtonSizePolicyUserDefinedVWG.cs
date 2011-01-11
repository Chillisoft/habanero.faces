using Habanero.Faces.Test.Base.ButtonsControl;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG
{
    [TestFixture]
    public class TestButtonSizePolicyUserDefinedVWG : TestButtonSizePolicyUserDefined
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}