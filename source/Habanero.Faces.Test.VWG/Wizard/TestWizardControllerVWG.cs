using Habanero.Faces.Test.Base.Wizard;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Wizard
{
    [TestFixture]
    public class TestWizardControllerVWG : TestWizardController
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}