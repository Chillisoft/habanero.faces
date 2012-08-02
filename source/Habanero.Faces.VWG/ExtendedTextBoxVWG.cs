using System;
using System.Drawing;
using Gizmox.WebGUI.Forms;
using Habanero.Faces.Base;
using Habanero.Faces.Base.ControlMappers;

namespace Habanero.Faces.VWG
{
    /// <summary>
    /// A Text Box with a Search Button. This is typically used for cases where there is a large list of potential items and it is 
    /// not appropriate to use a ComboBox for selecting the items.
    /// </summary>
    public class ExtendedTextBoxVWG : UserControlVWG, IExtendedTextBox
    {
        /// <summary>
        /// Constructor with an unspecified Control Factory.
        /// </summary>
        public ExtendedTextBoxVWG(): this(GlobalUIRegistry.ControlFactory)
        {
        }

        ///<summary>
        /// Constructor with a specified Control Factory
        ///</summary>
        ///<param name="factory"></param>
        public ExtendedTextBoxVWG(IControlFactory factory)
        {
            Button = factory.CreateButton("...");
            Button.MinimumSize = new Size(0, 0);
            TextBox = factory.CreateTextBox();
            TextBox.Enabled = false;
            this.Height = TextBox.Height;
            TextBox.BackColor = Color.White;
            BorderLayoutManager borderLayoutManager = factory.CreateBorderLayoutManager(this);
            this.Padding = Padding.Empty;
            borderLayoutManager.AddControl(TextBox, BorderLayoutManager.Position.Centre);
            borderLayoutManager.AddControl(Button, BorderLayoutManager.Position.East);
        }

        ///<summary>
        /// The Search Button
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