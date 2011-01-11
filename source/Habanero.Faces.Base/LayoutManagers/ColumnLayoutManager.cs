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
    /// This allows you to add controls to a layout where the controls will be added to the columns from
    /// left to right until the number of columns is exceeded. When this happens the control will be 
    /// added on the next row. The row height is determined by the heighest control in the row.
    /// All controls added will have an identical width (width = managed control width - borders size)
    /// </summary>
    public class ColumnLayoutManager : LayoutManager
    {
        private int _columnCount;

        /// <summary>
        /// Constructor to initialise a new layout manager
        /// </summary>
        /// <param name="managedControl">The control to manage</param>
        /// <param name="controlFactory">The control factory used by the layout manager to create controls</param>
        public ColumnLayoutManager(IControlHabanero managedControl, IControlFactory controlFactory)
            : base(managedControl, controlFactory)
        {
            _columnCount = 1;
        }
        /// <summary>
        /// Gets and Sets the number of columns in the layout
        /// </summary>
        public int ColumnCount
        {
            get { return _columnCount; }
            set
            {
                if (value < 1)
                {
                    throw new LayoutManagerException(
                        "You cannot set the column count for a column layout manager to less than 1");
                }
                _columnCount = value;
            }
        }

        /// <summary>
        /// Updates the layout and appearance of the managed controls
        /// </summary>
        protected override void RefreshControlPositions()
        {
            int totalWhiteSpace = BorderSize * 2 + (ColumnCount - 1) * GapSize;
            int columnWidth = (ManagedControl.Width - totalWhiteSpace) / ColumnCount;
            int currentColumn = 1;
            int currentLeft = BorderSize;
            int currentTop = BorderSize;
            int maxControlHeight = 0;
            try
            {
                this.ManagedControl.SuspendLayout();
                foreach (IControlHabanero control in this.ManagedControl.Controls)
                {

                    if (currentColumn > ColumnCount)
                    {
                        currentColumn = 1;
                        currentLeft = BorderSize;
                        currentTop += maxControlHeight + GapSize;
                        maxControlHeight = 0;
                    }
                    if (control.Height > maxControlHeight)
                    {
                        maxControlHeight = control.Height;
                    }
                    control.Location = new Point(currentLeft, currentTop);
                    control.Width = columnWidth;
                    currentLeft += columnWidth + GapSize;
                    currentColumn++;
                }
            }
            finally 
            {
                this.ManagedControl.ResumeLayout(true);
            }
        }

        /// <summary>
        /// Add a control to those being managed in the layout
        /// </summary>
        /// <param name="control">The control to add</param>
        /// <returns>Returns the control after it has been added</returns>
        public override IControlHabanero AddControl(IControlHabanero control)
        {
            if (control == null) throw new ArgumentNullException("control");
            this.ManagedControl.Controls.Add(control);
            RefreshControlPositions();
            control.Resize += delegate { RefreshControlPositions(); };
            return control;
        }
        //Todo : This needs to be moved up to the base class
        /// <summary>
        /// Removes a control from layout manager.
        /// </summary>
        /// <param name="control">The Control to be removed.</param>
        /// <returns>Returns the control being removed.</returns>
        public IControlHabanero RemoveControl(IControlHabanero control)
        {
            this.ManagedControl.Controls.Remove(control);
            RefreshControlPositions();
            control.Resize += delegate { RefreshControlPositions(); };
            return control;
        }
    }
}