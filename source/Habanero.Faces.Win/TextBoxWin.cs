// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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

using System.Drawing;
using System.Windows.Forms;
using Habanero.Faces.Base;
using HorizontalAlignment=Habanero.Faces.Base.HorizontalAlignment;
using ScrollBars=Habanero.Faces.Base.ScrollBars;

namespace Habanero.Faces.Win
{
    /// <summary>
    /// Represents a TextBox control
    /// </summary>
    public class TextBoxWin : TextBox, ITextBox
    {
        /// <summary>
        /// Gets or sets the anchoring style.
        /// </summary>
        /// <value></value>
        Base.AnchorStyles IControlHabanero.Anchor
        {
            get { return (Base.AnchorStyles)base.Anchor; }
            set { base.Anchor = (System.Windows.Forms.AnchorStyles)value; }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlHabanero.Dock
        {
            get { return DockStyleWin.GetDockStyle(base.Dock); }
            set { base.Dock = DockStyleWin.GetDockStyle(value); }
        }

        /// <summary>
        /// Gets or sets which scroll bars should appear in a multiline TextBox control
        /// </summary>
        ScrollBars ITextBox.ScrollBars
        {
            get { return (ScrollBars)ScrollBars; }
            set { ScrollBars = (System.Windows.Forms.ScrollBars)value; }
        }

        /// <summary>
        /// Gets or sets the alignment of text in the TextBox control
        /// </summary>
        HorizontalAlignment ITextBox.TextAlign
        {
            get { return (HorizontalAlignment)TextAlign; }
            set { TextAlign = (System.Windows.Forms.HorizontalAlignment)value; }
        }

        public TextBoxWin()
        {
            this.ObserveGlobalUIHints();
        }
        private void ObserveGlobalUIHints()
        {
            if (GlobalUIRegistry.UIStyleHints != null)
            {
                var hints = GlobalUIRegistry.UIStyleHints.TextBoxHints;
                this.MinimumSize = new Size(hints.MinimumWidth, hints.MinimumHeight);
                if ((hints.MaximumHeight > hints.MinimumHeight) && (hints.MaximumWidth > hints.MinimumWidth))
                    this.MaximumSize = new Size(hints.MaximumWidth, hints.MaximumHeight);
                if (hints.DefaultFontFamily != null)
                    this.Font = new Font(hints.DefaultFontFamily, this.Font.Size);
                if (hints.MinimumFontSize > this.Font.Size)
                    this.Font = new Font(this.Font.FontFamily, hints.MinimumFontSize);
            }
        }
    }
}