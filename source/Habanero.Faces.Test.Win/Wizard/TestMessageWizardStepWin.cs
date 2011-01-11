using Habanero.Faces.Test.Base.Wizard;
using Habanero.Faces.Base.Wizard;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Wizard
{
    [TestFixture]
    public class TestMessageWizardStepWin : TestMessageWizardStep
    {
        protected override IMessageWizardStep CreateWizardStep()
        {
            return new MessageWizardStepWin();
        }
    }
}