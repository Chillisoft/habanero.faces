using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
    [TestFixture]
    public class TestMainEditorPanelWin : TestMainEditorPanel
    {
        protected override IMainEditorPanel CreateControl(IControlFactory controlFactory)
        {
            return new MainEditorPanelWin(controlFactory);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}