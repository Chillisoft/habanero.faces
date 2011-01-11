using Habanero.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;

namespace Habanero.Faces.Test.VWG.HabaneroControls
{
    internal class BusinessObjectControlStubVWG : UserControlVWG, IBusinessObjectControl
    {
        public IBusinessObject BusinessObject { get; set; }
    }
}