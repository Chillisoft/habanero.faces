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
using Gizmox.WebGUI.Forms;
using Habanero.Faces.Base;

namespace Habanero.Faces.VWG
{
    /// <summary>
    /// Represents a label
    /// </summary>
    public class LabelVWG : Label, ILabel
    {
        private const int WIDTH_FACTOR = 6;

        ///<summary>
        /// Constructs the Label
        ///</summary>
        ///<param name="labelText"></param>
        public LabelVWG(string labelText = "")
        {
            base.Text = labelText;
            this.ObserveGlobalUIHints();
        }

        private void ObserveGlobalUIHints()
        {
            if (GlobalUIRegistry.UIStyleHints != null)
            {
                var hints = GlobalUIRegistry.UIStyleHints.LabelHints;
                this.MinimumSize = new Size(hints.MinimumWidth, hints.MinimumHeight);
                if ((hints.MaximumHeight > hints.MinimumHeight) && (hints.MaximumWidth > hints.MinimumWidth))
                    this.MaximumSize = new Size(hints.MaximumWidth, hints.MaximumHeight);
                if (hints.DefaultFontFamily != null)
                    this.Font = new Font(hints.DefaultFontFamily, this.Font.Size);
                if (hints.MinimumFontSize > this.Font.Size)
                    this.Font = new Font(this.Font.FontFamily, hints.MinimumFontSize);
            }
        }

        /// <summary>
        /// Gets the preferred width of the control
        /// </summary>
        public virtual int PreferredWidth
        {
            get { return this.Text.Length * WIDTH_FACTOR; }
        }

        /// <summary>
        /// Gets or sets the anchoring style.
        /// </summary>
        /// <value></value>
        Base.AnchorStyles IControlHabanero.Anchor
        {
            get { return (Base.AnchorStyles)base.Anchor; }
            set { base.Anchor = (Gizmox.WebGUI.Forms.AnchorStyles)value; }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionVWG(base.Controls); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlHabanero.Dock
        {
            get { return DockStyleVWG.GetDockStyle(base.Dock); }
            set { base.Dock = DockStyleVWG.GetDockStyle(value); }
        }
    }
}