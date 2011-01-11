using System;
using Habanero.Faces.Base;
using Habanero.Faces.Test.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Selectors
{
    [TestFixture]
    public class TestComboBoxLinkerVWG : TestComboBoxLinker
    {
        protected override IBOComboBoxSelector CreateControl()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return new ComboBoxSelectorVWG();
        }

    }
}