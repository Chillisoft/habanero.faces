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
using System;
using System.Drawing;
using System.Windows.Forms;
using Habanero.Faces.Base;

namespace Habanero.Faces.Win
{
    /// <summary>
    /// Represents a button control
    /// </summary>
    public class ButtonWin : Button, IButton
    {
        public event EventHandler VisibleChanged;

        /// <summary>
        /// Gets or sets the anchoring style.
        /// </summary>
        /// <value></value>
        Base.AnchorStyles IControlHabanero.Anchor
        {
            get { return (Base.AnchorStyles) base.Anchor; }
            set { base.Anchor = (System.Windows.Forms.AnchorStyles)value; }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionWin(base.Controls);  }

        }

        public void Select()
        {
            throw new NotImplementedException();
        }

        public bool HasChildren
        {
            get { throw new NotImplementedException(); }
        }

        public Size MaximumSize
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Size MinimumSize
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlHabanero.Dock
        {            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
/*            get { return DockStyleWin.GetDockStyle(base.Dock); }
            set { base.Dock = DockStyleWin.GetDockStyle(value); }*/
        }

        public void PerformClick()
        {
            throw new NotImplementedException();
        }

        public void NotifyDefault(bool b)
        {
            throw new NotImplementedException();
        }
    }
}