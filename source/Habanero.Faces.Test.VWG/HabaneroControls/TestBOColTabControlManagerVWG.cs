using System;
using Habanero.Base;
using Habanero.Faces.Test.Base.Controllers;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.HabaneroControls
{
    [TestFixture]
    public class TestBOColTabControlManagerVWG : TestBOColTabControlManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.VWG.ControlFactoryVWG();
        }
        protected override IBusinessObjectControl GetBusinessObjectControl()
        {
            return new BusinessObjectControlStubVWG();
        }

        protected override Type TypeOfBusinessObjectControl()
        {
            return typeof(BusinessObjectControlStubVWG);
        }

		protected override IBusinessObjectControl GetBusinessObjectControlSpy(Action<IBusinessObject> onBusinessObjectSet)
		{
			return new BusinessObjectControlSpyVWG(onBusinessObjectSet);
		}
    }
}