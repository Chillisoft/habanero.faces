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
using System.Drawing;
using Gizmox.WebGUI.Forms;
using Habanero.Faces.Base;

namespace Habanero.Faces.VWG
{
    /// <summary>
    /// Used to group collections of controls
    /// </summary>
    [MetadataTag("P")]
    public class PanelVWG : Panel, IPanel
    {
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
        /// Gets or sets the size that is the lower limit that
        /// GetPreferredSize(System.Drawing.Size) can specify
        /// </summary>
        /// <returns>An ordered pair of type System.Drawing.Size
        /// representing the width and height of a rectangle</returns>
        /// <filterpriority>1</filterpriority>
        public new Size MinimumSize { //This has been replaced since the MinimumSize in Giz does not work
            get; set; }

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
