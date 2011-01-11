using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Mappers
{
    [TestFixture]
    public class TestTextBoxMapperVWG : TestTextBoxMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.VWG.ControlFactoryVWG();
        }
    }
}