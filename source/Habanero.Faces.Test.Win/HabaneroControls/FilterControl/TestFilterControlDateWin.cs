using Habanero.Faces.Test.Base.FilterController;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
    [TestFixture]
    public class TestFilterControlDateWin : TestFilterControlDate
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
        //[Test]
        //public void TestLabelAndDateTimePickerAreOnPanel()
        //{
        //    TestLabelAndDateTimePickerAreOnPanel(2);
        //}
    }
}