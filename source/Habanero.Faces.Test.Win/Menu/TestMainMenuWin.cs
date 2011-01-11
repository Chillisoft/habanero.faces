using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Menu
{
    /// <summary>
    /// This test class tests the classes that implement IMainMenuHabanero.
    /// </summary>
    [TestFixture]
    public class TestMainMenuWin : TestMainMenu
    {

        protected override IControlFactory GetControlFactory()
        {
            IControlFactory factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
    }
}