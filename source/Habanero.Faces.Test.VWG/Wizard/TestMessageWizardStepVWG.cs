using Habanero.Faces.Test.Base.Wizard;
using Habanero.Faces.Base.Wizard;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Wizard
{
    [TestFixture]
    public class TestMessageWizardStepVWG : TestMessageWizardStep
    {
        protected override IMessageWizardStep CreateWizardStep()
        {
            return new MessageWizardStepVWG();
        }
    }
}