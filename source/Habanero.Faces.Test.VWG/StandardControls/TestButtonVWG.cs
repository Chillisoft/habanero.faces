using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.StandardControls
{
    [TestFixture]
    public class TestButtonVWG : TestButton
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}