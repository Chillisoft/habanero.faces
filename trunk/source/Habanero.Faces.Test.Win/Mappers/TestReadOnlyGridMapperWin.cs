using Habanero.Faces.Base;
using Habanero.Faces.Test.Base.Mappers;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Mappers
{
    /// <summary>
    /// Summary description for TestTextBoxMapper.
    /// </summary>
    [TestFixture]
    public class TestReadOnlyGridMapperWin : TestReadOnlyGridMapper
    {

        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}