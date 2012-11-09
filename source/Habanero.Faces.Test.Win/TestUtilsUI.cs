using System.Windows.Forms;

namespace Habanero.Faces.Test.Win
{
    internal static class TestUtilsUI
    {
        public static void ShowInVisualTestingForm(Control control, int width = 800, int height = 600)
        {
            var frm = new Form();
            frm.Width = width;
            frm.Height = height;
            control.Dock = DockStyle.Fill;
            frm.Controls.Add(control);
            Application.EnableVisualStyles();
            Application.Run(frm);
        }
    }
}