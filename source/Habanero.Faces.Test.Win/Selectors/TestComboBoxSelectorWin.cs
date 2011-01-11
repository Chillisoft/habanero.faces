using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Selectors
{
    /// <summary>
    /// This test class tests the ComboBoxSelector class.
    /// </summary>
    [TestFixture]
    public class TestComboBoxSelectorWin : TestComboBoxSelector
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
            
    }
}