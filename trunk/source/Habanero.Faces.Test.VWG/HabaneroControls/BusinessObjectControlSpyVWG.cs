using System;
using Habanero.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;

namespace Habanero.Faces.Test.VWG.HabaneroControls
{
	internal class BusinessObjectControlSpyVWG : UserControlVWG, IBusinessObjectControl
	{
		private readonly Action<IBusinessObject> _onBusinessObjectSet;

		public BusinessObjectControlSpyVWG(Action<IBusinessObject> onBusinessObjectSet)
		{
			_onBusinessObjectSet = onBusinessObjectSet;
		}

		private IBusinessObject _businessObject;
		public IBusinessObject BusinessObject
		{
			get { return _businessObject; }
			set
			{
				_businessObject = value;
				_onBusinessObjectSet(_businessObject);
			}
		}
	}
}