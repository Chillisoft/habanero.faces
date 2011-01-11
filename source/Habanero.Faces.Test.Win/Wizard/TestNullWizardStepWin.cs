using Habanero.Faces.Test.Base.Wizard;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Wizard
{
    [TestFixture]
    public class TestNullWizardStepWin : TestNullWizardStep
    {
        protected override IWizardStep CreateWizardStep()
        {
            return new NullWizardStepWin();
        }
    }
}