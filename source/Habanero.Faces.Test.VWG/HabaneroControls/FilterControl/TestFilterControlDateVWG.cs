using Habanero.Faces.Test.Base.FilterController;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.HabaneroControls
{
    [TestFixture]
    public class TestFilterControlDateVWG : TestFilterControlDate
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }
        //[Test]
        //public void TestLabelAndDateTimePickerAreOnPanel()
        //{
        //    TestLabelAndDateTimePickerAreOnPanel(3);
        //}
    }
}