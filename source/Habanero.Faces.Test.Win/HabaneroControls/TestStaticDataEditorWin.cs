using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
    [TestFixture]
    public class TestStaticDataEditorWin : TestStaticDataEditor
    {
        protected override IStaticDataEditor CreateEditorOnForm(out IFormHabanero frm)
        {
            frm = GetControlFactory().CreateForm();
            IStaticDataEditor editor = GetControlFactory().CreateStaticDataEditor();
            frm.Controls.Add(editor);
            frm.Show();
            return editor;
        }

        protected override IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = new ControlFactoryWin();
            return GlobalUIRegistry.ControlFactory;
        }

        protected override void TearDownForm(IFormHabanero frm)
        {
            frm.Close();
            frm.Dispose();
        }
    }
}