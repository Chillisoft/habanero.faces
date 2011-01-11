using Habanero.Faces.Test.Base.Mappers;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Mappers
{
    [TestFixture]
    public class TestRelationshipComboBoxMapperVWG : TestRelationshipComboBoxMapper
    {
        protected override void CreateControlFactory()
        {
            _controlFactory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = _controlFactory;
        }
    }
}