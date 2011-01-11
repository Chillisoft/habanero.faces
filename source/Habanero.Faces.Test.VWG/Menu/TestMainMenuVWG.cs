using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Menu
{
    /// <summary>
    /// This test class tests the classes that implement IMainMenuHabanero.
    /// </summary>
    [TestFixture]
    public class TestMainMenuVWG : TestMainMenu
    {
        protected override IControlFactory GetControlFactory()
        {
            IControlFactory factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
    }
}