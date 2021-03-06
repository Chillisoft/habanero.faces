using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Forms;
using Habanero.Faces.Base;
using Habanero.Faces.Base.ControlMappers;

namespace Habanero.Faces.Win
{
    /// <summary>
    /// A Text Box with a Search Button. This is typically used for cases where there is a large list of potential items and it is 
    /// not appropriate to use a ComboBox for selecting the items.
    /// </summary>
    public class ExtendedTextBoxWin : UserControlWin, IExtendedTextBox
    {
        /// <summary>
        /// Constructor with an unspecified Control Factory.
        /// </summary>
        public ExtendedTextBoxWin(): this(GlobalUIRegistry.ControlFactory)
        {
        }

        ///<summary>
        /// Constructor with a specified Control Factory
        ///</summary>
        ///<param name="factory"></param>
        public ExtendedTextBoxWin(IControlFactory factory)
        {
            Button = factory.CreateButton("...");
            TextBox = factory.CreateTextBox();
            Button.MinimumSize = new Size(0, 0);
            TextBox.Enabled = false;
            this.Height = TextBox.Height;
            BorderLayoutManager borderLayoutManager = factory.CreateBorderLayoutManager(this);
            this.Padding = Padding.Empty;
            borderLayoutManager.AddControl(TextBox, BorderLayoutManager.Position.Centre);
            borderLayoutManager.AddControl(Button, BorderLayoutManager.Position.East);
        }

        /// <summary>
        /// sets an icon on the picker button by a name of a resource to be found in one of the loaded assemblies
        /// </summary>
        /// <param name="resourceName">name of the resource to load, eg "doctor-icon.png"</param>
        public void SetButtonIcon(string resourceName)
        {
            var s = ResourceStreamer.GetResourceStreamByName(resourceName);
            if (s == null) return;
            var btn = Button as ButtonWin;
            if (btn != null)
                btn.SetIcon(resourceName);
        }

        ///<summary>
        /// The Extended Button typically a search button.
        ///</summary>
        public IButton Button { get; private set; }

        /// <summary>
        /// The Text box in which the result of the search are displayed.
        /// </summary>
        public ITextBox TextBox { get; private set; }

        public IControlMapper ControlMapper { get; set; }

        ///<summary>
        /// Gets or sets the text associated with this control.           
        ///</summary>
        public override string Text
        {
            get { return this.TextBox.Text; }
            set { this.TextBox.Text = value; }
        }
    }
}