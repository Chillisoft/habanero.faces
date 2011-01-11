using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.HabaneroControls
{
    [TestFixture]
    public class TestInputFormComboBoxVWG : TestInputFormComboBox
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }

        [Test, Ignore("Minimumsize doesn't work for VWG")]
        public override void Test_CreateOKCancelForm_ShouldSetMinimumSize()
        {

        }

    }
}