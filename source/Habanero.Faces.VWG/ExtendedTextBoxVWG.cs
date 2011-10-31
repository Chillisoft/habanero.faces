#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
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