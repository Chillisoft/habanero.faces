using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.HabaneroControls
{
    [TestFixture]
    public class TestMainEditorPanelVWG : TestMainEditorPanel
    {
        protected override IMainEditorPanel CreateControl(IControlFactory controlFactory)
        {
            return new MainEditorPanelVWG(controlFactory);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}