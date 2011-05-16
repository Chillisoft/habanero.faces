using System.Drawing;
using System.Windows.Forms;
using Habanero.Faces.Base;

namespace Habanero.Faces.Adapters
{
    /// <summary>
    /// This is a ControlWraper for Any Control that Inherits from System.Windows.Forms.Control
    /// It wraps this Control behind a standard interface that allows any Control in a Windows Environment 
    /// to take advantage of the Habanero ControlMappers <see cref="IControlMapper"/>
    /// </summary>
    public class WinFormsLabelAdapter : WinFormsControlAdapter, IWinFormsLabelAdapter
    {
        private readonly Label _label;
        public WinFormsLabelAdapter(Label control)
            : base(control)
        {
            _label = control;
        }

        public int PreferredWidth
        {
            get { return 100; }
        }

        public bool AutoSize { get; set; }
/*        public bool AutoSize
        {
            get { return _label.A }
            set { throw new NotImplementedException(); }
        }*/

        public ContentAlignment TextAlign
        {
            get { return _label.TextAlign; }
            set { _label.TextAlign = value; }
        }
    }
}