using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.HabaneroControls
{
    /// <summary>
    /// This test class tests the GroupBoxGroupControl class.
    /// </summary>
    [TestFixture]
    public class TestGroupBoxGroupControlVWG : TestGroupBoxGroupControl
    {
        protected override IControlFactory GetControlFactory()
        {
            Habanero.Faces.VWG.ControlFactoryVWG factory = new Habanero.Faces.VWG.ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
    }
}