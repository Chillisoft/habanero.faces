using Habanero.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
    internal class BusinessObjectControlStubWin : UserControlWin, IBusinessObjectControl
    {
        public IBusinessObject BusinessObject { get; set; }

    }
}