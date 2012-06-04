using System;
using Habanero.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
	internal class BusinessObjectControlSpyWin : UserControlWin, IBusinessObjectControl
	{
		private readonly Action<IBusinessObject> _onBusinessObjectSet;

		public BusinessObjectControlSpyWin(Action<IBusinessObject> onBusinessObjectSet)
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