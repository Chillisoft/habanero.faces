using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Faces.Base;
using Habanero.Faces.Test.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG
{
    [TestFixture]
    public class TestExtendedTextBoxVWG : TestExtendedTextBox
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}
