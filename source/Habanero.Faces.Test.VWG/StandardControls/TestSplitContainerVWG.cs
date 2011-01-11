using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.StandardControls
{
    /// <summary>
    /// This test class tests the SplitContainer class.
    /// </summary>
    [TestFixture]
    public class TestSplitContainerVWG : TestSplitContainer
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}