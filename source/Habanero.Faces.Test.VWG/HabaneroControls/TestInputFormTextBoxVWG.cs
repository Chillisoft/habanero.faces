using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.HabaneroControls
{
    [TestFixture]
    public class TestInputFormTextBoxVWG : TestInputFormTextBox
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}