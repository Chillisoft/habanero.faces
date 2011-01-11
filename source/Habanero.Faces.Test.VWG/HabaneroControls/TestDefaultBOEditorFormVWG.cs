using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.HabaneroControls
{
    [TestFixture]
    public class TestDefaultBOEditorFormVWG : TestDefaultBOEditorForm
    {

        protected override void ShowFormIfNecessary(IFormHabanero form)
        {

        }

        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new Habanero.Faces.VWG.ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        [Ignore("This cannot be tested for VWG because you cannot show a form to close it")]
        public override void Test_CloseForm_ShouldCallDelegateWithCorrectInformation()
        {
        }

        protected override void LoadMyBOClassDef()
        {
            _classDefMyBo = MyBO.LoadDefaultClassDefVWG();
        }
    }
}