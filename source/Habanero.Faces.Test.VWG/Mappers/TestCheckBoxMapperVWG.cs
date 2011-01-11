using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Test.Base.Mappers;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Mappers
{
    [TestFixture]
    public class TestCheckBoxMapperVWG : TestCheckBoxMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.VWG.ControlFactoryVWG();
        }

        [Test]
        public void TestSettingCheckBoxCheckedUpdatesBO()
        {
            //----------Setup test pack----------------------------
            _sampleBusinessObject.SampleBoolean = false;
            _mapper.BusinessObject = _sampleBusinessObject;
            //----------verify test pack --------------------------
            //----------Execute test ------------------------------
            _cb.Checked = true;
            _mapper.ApplyChangesToBusinessObject();
            //----------verify test ------------------------------
            Assert.IsTrue(_sampleBusinessObject.SampleBoolean);
        }
    }
}