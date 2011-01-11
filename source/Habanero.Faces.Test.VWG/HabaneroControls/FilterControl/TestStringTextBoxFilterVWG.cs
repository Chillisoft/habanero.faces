using Habanero.Faces.Test.Base.FilterController;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.HabaneroControls
{
    [TestFixture]
    public class TestStringTextBoxFilterVWG : TestStringTextBoxFilter
    {
        protected override IControlFactory GetControlFactory() { return new ControlFactoryVWG(); }
    }
}