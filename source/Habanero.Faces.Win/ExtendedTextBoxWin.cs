using System;
using System.Drawing;
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
            TextBox.Enabled = false;
            this.Height = TextBox.Height;
            BorderLayoutManager borderLayoutManager = factory.CreateBorderLayoutManager(this);
            this.Padding = Padding.Empty;
            borderLayoutManager.AddControl(TextBox, BorderLayoutManager.Position.Centre);
            borderLayoutManager.AddControl(Button, BorderLayoutManager.Position.East);
        }

        public void SetButtonIcon(string resourceName)
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var rName in asm.GetManifestResourceNames())
                {
                    if (rName.EndsWith(resourceName))
                    {
                        var s = asm.GetManifestResourceStream(rName);
                        var btn = Button as Button;
                        if (btn != null)
                        {
                            btn.ResetBackColor();
                            btn.ResetForeColor();
                            btn.Image = Image.FromStream(s);
                            btn.Text = "";
                            btn.Width = btn.Image.Width + 10;
                            btn.Height = btn.Image.Height + 10;
                        }
                    }
                }
            }
        }

        ///<summary>
        /// The Extended Button typically a search button.
        ///</summary>
        public IButton Button { get; private set; }

        /// <summary>
        /// The Text box in which the result of the search are displayed.
        /// </summary>
        public ITextBox TextBox { get; private set; }

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