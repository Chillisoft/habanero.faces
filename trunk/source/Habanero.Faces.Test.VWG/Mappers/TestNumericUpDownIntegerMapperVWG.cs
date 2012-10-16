using Habanero.Faces.Test.Base.Mappers;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Mappers
{
    [TestFixture]
    public class TestNumericUpDownIntegerMapperVWG : TestNumericUpDownIntegerMapper
    {
        public override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}