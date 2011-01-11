using Habanero.Faces.Test.Base.Wizard;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Wizard
{
    [TestFixture]
    public class TestNullWizardStepVWG : TestNullWizardStep
    {
        protected override IWizardStep CreateWizardStep()
        {
            return new NullWizardStepVWG();
        }
    }
}