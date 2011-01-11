using Habanero.Faces.Test.Base.Controllers;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.HabaneroControls
{
    [TestFixture]
    public class TestComboBoxCollectionSelectorVWG : TestComboBoxCollectionSelector
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.VWG.ControlFactoryVWG();
        }
    }
}