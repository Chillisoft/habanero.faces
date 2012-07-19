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

namespace Habanero.Faces.Base
{
    /// <summary>
    /// Manages the layout of controls in a user interface by having a
    /// component assigned a compass position.  For instance, having the
    /// "east" position assigned will result in the control being placed
    /// against the right border.
    /// </summary>
    public abstract class BorderLayoutManager : LayoutManager
    {
        /// <summary>
        /// An enumeration to specify different layout positions that can
        /// be assigned
        /// </summary>
        public enum Position
        {
            ///<summary>
            /// Centre position
            ///</summary>
            Centre = 0,
            ///<summary>
            /// East position (right)
            ///</summary>
            East = 1,
            ///<summary>
            /// West position (left)
            ///</summary>
            West = 2,
            ///<summary>
            /// North position (top)
            ///</summary>
            North = 3,
            ///<summary>
            /// South position (bottom)
            ///</summary>
            South = 4
        }

        private readonly IControlHabanero[] _controls;
        private readonly bool[] _splitters;

        /// <summary>
        /// Constructor to initalise a new layout manager
        /// </summary>
        /// <param name="managedControl">The control to manage (eg. use "this"
        /// if you create the manager inside a form class that you will be
        /// managing)</param>
        /// <param name="controlFactory">The control factory that will be used to create controls</param>
        public BorderLayoutManager(IControlHabanero managedControl, IControlFactory controlFactory)
            : base(managedControl, controlFactory)
        {
            _controls = new IControlHabanero[5];
            _splitters = new bool[5];
        }

        /// <summary>
        /// Updates the layout and appearance of the managed controls
        /// </summary>
        protected override void RefreshControlPositions()
        {
           // resize the managed control to handle all of the docked controls
            //if (this.ManagedControl == null) return;
            //var rightMost = 0;
            //var bottomMost = 0;
            //foreach (var control in this.ManagedControl.Controls)
            //{
            //    var ctl = control as IControlHabanero;
            //    if (ctl == null) continue;
            //    if (ctl.Right > rightMost)
            //        rightMost = ctl.Right;
            //    if (ctl.Bottom > bottomMost)
            //        bottomMost = ctl.Bottom;
            //}
            //if ((rightMost > 0) || (bottomMost > 0))
            //{
            //    var managedMin = this.ManagedControl.MinimumSize;
            //    var minWidth = (managedMin.Width > rightMost) ? managedMin.Width : rightMost;
            //    var minHeight = (managedMin.Height > bottomMost) ? managedMin.Height : bottomMost;
            //    this.ManagedControl.MinimumSize = new Size(minWidth, minHeight);
            //}
        }

        /// <summary>
        /// Add a control to the layout
        /// </summary>
        /// <param name="control">The control to add</param>
        /// <returns>Returns the control added</returns>
        public override IControlHabanero AddControl(IControlHabanero control)
        {
            if (control == null) throw new ArgumentNullException("control");
            this.ManagedControl.Controls.Add(control);
            return control;
        }

        /// <summary>
        /// Add a control to the layout at the specified position
        /// </summary>
        /// <param name="control">The control to add</param>
        /// <param name="pos">The position at which to add the control</param>
        /// <returns>Returns the control added</returns>
        public IControlHabanero AddControl(IControlHabanero control, Position pos)
        {
            if (control == null) throw new ArgumentNullException("control");
            AddControl(control,pos,false);
            return control;
        }

        /// <summary>
        /// Add a control to the layout
        /// </summary>
        /// <param name="control">The control to add</param>
        /// /// <param name="pos">The position at which to add the control</param>
        /// <param name="includeSplitter">True to include a splitter between the controls</param>
        /// <returns>Returns the control added</returns>
        public abstract IControlHabanero AddControl(IControlHabanero control, Position pos, bool includeSplitter);

        /// <summary>
        /// Sets how the specified control is docked within its parent
        /// </summary>
        protected abstract void SetupDockOfControl(IControlHabanero control, Position pos);


        //public IControlHabanero AddControl(IControlHabanero ctl, Position pos, bool includeSplitter)
        //{
        //    ctl.Dock = _controlFactory.GetDockStyle(pos);
        //        this.ManagedControl.Controls.Add(ctl);

        //    //switch (pos)
        //    //{
        //    //    case Position.Centre:
        //    //        ctl.Dock = DockStyle.Fill;
        //    //        break;
        //    //    case Position.North:
        //    //        ctl.Dock = DockStyle.Top;
        //    //        break;
        //    //    case Position.South:
        //    //        ctl.Dock = DockStyle.Bottom;
        //    //        break;
        //    //    case Position.East:
        //    //        ctl.Dock = DockStyle.Right;
        //    //        break;
        //    //    case Position.West:
        //    //        ctl.Dock = DockStyle.Left;
        //    //        break;
        //    //}
        //    //_controls[(int)pos] = ctl;
        //    //_splitters[(int)pos] = includeSplitter;
        //    //this.ManagedControl.Controls.Clear();
        //    //for (int i = 0; i < 5; i++)
        //    //{
        //    //    if (_controls[i] != null)
        //    //    {
        //    //        if (_splitters[i])
        //    //        {
        //    //            Splitter splt = new Splitter();
        //    //            Color newBackColor =
        //    //                Color.FromArgb(Math.Min(splt.BackColor.R - 30, 255), Math.Min(splt.BackColor.G - 30, 255),
        //    //                               Math.Min(splt.BackColor.B - 30, 255));
        //    //            splt.BackColor = newBackColor;

        //    //            splt.Dock = _controls[i].Dock;
        //    //            ManagedControl.Controls.Add(splt);
        //    //        }
        //    //        ManagedControl.Controls.Add(_controls[i]);
        //    //    }
        //    //}
        //    return ctl;
        //}

        //public override void AddGlue()
        //{
        //    throw new NotImplementedException();
        //}

    }
}