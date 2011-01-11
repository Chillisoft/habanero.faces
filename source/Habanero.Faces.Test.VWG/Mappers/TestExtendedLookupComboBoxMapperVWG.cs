using System;
using Habanero.Faces.Test.Base.Mappers;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Mappers
{
    [Ignore("The ExtendedComboBox is not implemented for VWG")]
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
            throw new NotImplementedException();
        }
    }
}