using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
    [TestFixture]
    public class TestDefaultBOEditorFormWin : TestDefaultBOEditorForm
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new Habanero.Faces.Win.ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override void LoadMyBOClassDef()
        {
            _classDefMyBo = MyBO.LoadClassDefWithNoLookup();
        }
    }
}