using System;
using Habanero.Base;
using Habanero.Faces.Test.Base.Controllers;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
    [TestFixture]
    public class TestBOColTabControlManagerWin : TestBOColTabControlManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.Win.ControlFactoryWin();
        }

        protected override Type TypeOfBusinessObjectControl()
        {
            return typeof(BusinessObjectControlStubWin);
        }

        protected override IBusinessObjectControl GetBusinessObjectControl()
        {
            return new BusinessObjectControlStubWin();
        }

		protected override IBusinessObjectControl GetBusinessObjectControlSpy(Action<IBusinessObject> onBusinessObjectSet)
    	{
			return new BusinessObjectControlSpyWin(onBusinessObjectSet);
    	}
    }
}