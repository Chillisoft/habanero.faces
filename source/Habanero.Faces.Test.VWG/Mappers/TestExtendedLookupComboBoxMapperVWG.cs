using Habanero.Faces.Test.Base.Mappers;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Mappers
{
    [TestFixture]
    public class TestExtendedLookupComboBoxMapperVWG : TestExtendedLookupComboBoxMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override IExtendedComboBox CreateExtendedComboBox()
        {
            return GetControlFactory().CreateExtendedComboBox();
        }
    }
}