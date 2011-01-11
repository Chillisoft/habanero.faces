using Habanero.Faces.Test.Base.FilterController;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
    [TestFixture]
    public class TestStringTextBoxFilterWin : TestStringTextBoxFilter
    {
        protected override IControlFactory GetControlFactory() { return new ControlFactoryWin(); }
    }
}