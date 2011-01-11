using System;
using Habanero.Faces.Base;
using Habanero.Faces.Test.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Selectors
{
    [TestFixture]
    public class TestBOColSelectorLinkerComboBoxVWG : TestBOColSelectorLinkerComboBox
    {
        protected override IBOComboBoxSelector CreateComboBoxControl()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return new ComboBoxSelectorVWG();
        }

    }
}