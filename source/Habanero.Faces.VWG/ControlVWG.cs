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
using Gizmox.WebGUI.Forms;
using Habanero.Faces.Base;
using AnchorStyles=Habanero.Faces.Base.AnchorStyles;
using DockStyle=Habanero.Faces.Base.DockStyle;

namespace Habanero.Faces.VWG
{
    /// <summary>
    /// Defines controls, which are components with visual representation
    /// </summary>
    [MetadataTag("P")]
    public class ControlVWG : Control, IControlHabanero
    {
        #region IControlHabanero

        /// <summary>
        /// Gets or sets the anchoring style.
        /// </summary>
        /// <value></value>
        AnchorStyles IControlHabanero.Anchor
        {
            get { return (AnchorStyles)base.Anchor; }
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
        DockStyle IControlHabanero.Dock
        {
            get { return DockStyleVWG.GetDockStyle(base.Dock); }
            set { base.Dock = DockStyleVWG.GetDockStyle(value); }
        }

        /// <summary>
        /// Gets the distance, in pixels, between the bottom edge of the
        /// control and the top edge of its container's client area
        /// </summary>
        int IControlHabanero.Bottom
        {
            get { return this.Top + this.Height; }
        }

        /// <summary>
        /// Gets the distance, in pixels, between the right edge of the
        /// control and the left edge of its container's client area
        /// </summary>
        int IControlHabanero.Right
        {
            get { return this.Left + this.Width; }
        }

        #endregion

    }
}