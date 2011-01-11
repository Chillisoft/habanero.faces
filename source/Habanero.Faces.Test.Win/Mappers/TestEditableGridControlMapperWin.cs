using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Mappers
{
    /// <summary>
    /// Summary description for TestTextBoxMapper.
    /// </summary>
    [TestFixture]
    public class TestEditableGridControlMapperWin : TestEditableGridControlMapper
    {

        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}