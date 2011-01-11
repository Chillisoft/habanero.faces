using Habanero.Base;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Test.Base.Mappers;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Mappers
{
    [TestFixture]
    public class TestCollectionTabControlMapperWin : TestCollectionTabControlMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.Win.ControlFactoryWin();
        }

        protected override IBusinessObjectControl CreateBusinessObjectControl()
        {
            return new BusinessObjectControlWin();
        }

        class BusinessObjectControlWin : Habanero.Faces.Win.ControlWin, IBusinessObjectControl
        {

            #region Implementation of IBusinessObjectControl
            // ReSharper disable ValueParameterNotUsed
            /// <summary>
            /// Gets or sets the business object being represented
            /// </summary>
            public IBusinessObject BusinessObject
            {
                get { return null; }
                set { }
            }
            // ReSharper restore ValueParameterNotUsed

            #endregion
        }
    }
}