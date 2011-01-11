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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Habanero.Base.Exceptions;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// Manages the layout of controls in a user interface by assigning
    /// them to positions in a grid with rows and columns
    /// </summary>
    public class GridLayoutManager : LayoutManager
    {
        private readonly List<IControlHabanero> _controls;
        private readonly Hashtable _controlInfoTable;
        private Point _currentPos;
        private int[] _columnWidths;
        private int[] _rowHeights;
        private bool[] _fixedColumnsBasedOnContents;
        private bool _fixAllRowsBasedOnContents;
        private bool[] _fixedRowsBasedOnContents;
        private bool[,] _positionsOccupied;

        /// <summary>
        /// Constructor to initialise a new grid layout
        /// </summary>
        /// <param name="managedControl">The control to manage</param>
        /// <param name="controlFactory">The control factory used to create any controls</param>
        public GridLayoutManager(IControlHabanero managedControl, IControlFactory controlFactory)
            : base(managedControl, controlFactory)
        {
            _controls = new List<IControlHabanero>();
            _controlInfoTable = new Hashtable();
            this.SetGridSize(2, 2);
        }

        /// <summary>
        /// Sets the grid size as a number of rows and columns
        /// </summary>
        /// <param name="rows">The number of rows</param>
        /// <param name="cols">The number of columns</param>
        public void SetGridSize(int rows, int cols)
        {
            _columnWidths = new int[cols];
            _fixedColumnsBasedOnContents = new bool[cols];
            for (int i = 0; i < _columnWidths.Length; i++)
            {
                _columnWidths[i] = -1;
                _fixedColumnsBasedOnContents[i] = false;
            }
            _rowHeights = new int[rows];
            _fixedRowsBasedOnContents = new bool[rows];
            for (int i = 0; i < _rowHeights.Length; i++)
            {
                _rowHeights[i] = -1;
                _fixedRowsBasedOnContents[i] = false;
            }
            _positionsOccupied = new bool[rows,cols];
        }

        /// <summary>
        /// Returns the number of rows
        /// </summary>
        private int RowCount
        {
            get { return _rowHeights.Length; }
        }

        /// <summary>
        /// Returns the number of columns
        /// </summary>
        private int ColumnCount
        {
            get { return _columnWidths.Length; }
        }

        /// <summary>
        /// Returns an IList object containing all the controls row by row
        /// </summary>
        public IList<List<IControlHabanero>> Rows
        {
            get
            {
                IList<List<IControlHabanero>> rows = new List<List<IControlHabanero>>();
                for (int i = 0; i < RowCount; i++)
                {
                    List<IControlHabanero> row = new List<IControlHabanero>();
                    for (int j = 0; j < ColumnCount; j++)
                    {
                        if ((i*ColumnCount + j) < this._controls.Count)
                        {
                            row.Add(this._controls[i*ColumnCount + j]);
                        }
                        else
                        {
                            row.Add(null);
                        }
                    }
                    rows.Add(row);
                }
                return rows;
            }
        }

        /// <summary>
        /// Returns an IList object containing all the controls column by column
        /// </summary>
        public IList<List<IControlHabanero>> Columns
        {
            get
            {
                IList<List<IControlHabanero>> cols = new List<List<IControlHabanero>>();
                for (int i = 0; i < ColumnCount; i++)
                {
                    List<IControlHabanero> col = new List<IControlHabanero>();
                    for (int j = 0; j < RowCount; j++)
                    {
                        if ((ColumnCount*j + i) < this._controls.Count)
                        {
                            col.Add(this._controls[ColumnCount*j + i]);
                        }
                        else
                        {
                            col.Add(null);
                        }
                    }
                    cols.Add(col);
                }
                return cols;
            }
        }

        /// <summary>
        /// Adds a control with a row and column span of 1 to the next position in the grid.
        /// </summary>
        /// <param name="control">The control to add</param>
        /// <returns>Returns the control once it has been added</returns>
        public override IControlHabanero AddControl(IControlHabanero control)
        {
            return AddControl(new ControlInfo(control, 1, 1));
        }

        /// <summary>
        /// Adds a control to the next position in the grid.
        /// The number of rows or columns to span are specified as parameters.
        /// </summary>
        /// <param name="control">The control to add</param>
        /// <param name="rowSpan">The row span for the control</param>
        /// <param name="columnSpan">The column span for the control</param>
        /// <returns>Returns the control once it has been added</returns>
        public IControlHabanero AddControl(IControlHabanero control, int rowSpan, int columnSpan)
        {
            return AddControl(new ControlInfo(control, columnSpan, rowSpan));
        }

        /// <summary>
        /// Adds a control as specified by the ControlInfo provided (which can provide some context for the control)
        /// such as number of rows or columns to span. 
        /// </summary>
        /// <param name="controlInfo">The information about the control to add to the next position in the grid.</param>
        public IControlHabanero AddControl(ControlInfo controlInfo)
        {
            IControlHabanero control = null;
            int rowSpan = 1;
            int columnSpan = 1;
            if (controlInfo != null)
            {
                control = controlInfo.Control;
                rowSpan = controlInfo.RowSpan;
                columnSpan = controlInfo.ColumnSpan;
            }
  

            if (control == null)
            {
                control = _controlFactory.CreateControl();
                control.Visible = false;
            }
            int currentColNum = (this._controls.Count)%ColumnCount;
            int currentRowNum = (this._controls.Count)/ColumnCount;
            if (currentRowNum >= _rowHeights.Length)
            {
                string errorMessage = string.Format("You cannot add a control to the grid layout manager since it exceeds the grids size of '{0}' row and '{1}' column", 
                        _rowHeights.Length, _columnWidths.Length);
                throw new HabaneroDeveloperException(
                    "There is a serious application error. Please contact your system administrator" + Environment.NewLine + errorMessage, errorMessage);
            }

            if (_positionsOccupied[currentRowNum, currentColNum])
            {
                IControlHabanero nullControl = _controlFactory.CreateControl();
                //FixRow(currentRowNum,0);
                nullControl.Visible = false;
                nullControl.Name = "Null";
                _controls.Add(nullControl);
                this.ManagedControl.Controls.Add(nullControl);
                this._controlInfoTable.Add(nullControl, new ControlInfo(nullControl, columnSpan, rowSpan));
                return AddControl(controlInfo);
            }

            if (_fixedColumnsBasedOnContents[currentColNum])
            {
                if (control.Width > _columnWidths[currentColNum])
                {
                    FixColumn(currentColNum, control.Width);
                }
            }
            if (_fixAllRowsBasedOnContents)
            {
                if (control.Height > _rowHeights[currentRowNum])
                {
                    FixRow(currentRowNum, control.Height);
                }
            }
            else if (_fixedRowsBasedOnContents[currentRowNum])
            {
                if (control.Height > _rowHeights[currentRowNum])
                {
                    FixRow(currentRowNum, control.Height);
                }
            }
            this._controls.Add(control);

            this.ManagedControl.Controls.Add(control);
            this._controlInfoTable.Add(control, new ControlInfo(control, columnSpan, rowSpan));

            for (int i = currentRowNum; i < currentRowNum + rowSpan; i++)
            {
                for (int j = currentColNum; j < currentColNum + columnSpan; j++)
                {
                    _positionsOccupied[i, j] = true;
                }
            }
           

            RefreshControlPositions();
            return control;
        }

        /// <summary>
        /// Updates the positions and settings of the controls in the interface
        /// </summary>
        protected override void RefreshControlPositions()
        {
            try
            {
                ManagedControl.SuspendLayout();
                _currentPos = new Point(BorderSize, BorderSize);
                int lastColSpan = 0;
                for (int i = 0; i < _controls.Count; i++)
                {
                    int currentRow = i/ColumnCount;
                    int currentCol = i%ColumnCount;
                    IControlHabanero ctl = this._controls[i];

                    if ((i > 0) && (currentCol == 0))
                    {
                        _currentPos.X = BorderSize;
                        _currentPos.Y += this._controls[i - 1].Height + GapSize;
                    }
                    int width = 0;
                    ControlInfo ctlInfo = (ControlInfo) _controlInfoTable[ctl];
                    for (int columnNumber = currentCol;
                         columnNumber < Math.Min(this.ColumnCount, currentCol + ctlInfo.ColumnSpan);
                         columnNumber++)
                    {
                        if (IsFixedColumn(columnNumber))
                        {
                            width += _columnWidths[columnNumber];
                        }
                        else
                        {
                            width += CalcColumnWidth();
                        }
                    }
                    width += (this.GapSize*(ctlInfo.ColumnSpan - 1));

                    int height = 0;

                    for (int rows = currentRow; rows < Math.Min(RowCount, currentRow + ctlInfo.RowSpan); rows++)
                    {
                        if (IsFixedRow(currentRow))
                        {
                            height += _rowHeights[currentRow];
                        }
                        else
                        {
                            height += CalcRowHeight();
                        }
                    }

                    height += (GapSize*(ctlInfo.RowSpan - 1));
                    //height += GapSize;

                    //ctl.Left = _currentPos.X;
                    //ctl.Top = _currentPos.Y;
                    //ctl.Width = width;
                    //ctl.Height = height;
                    ctl.Location = new Point(_currentPos.X, _currentPos.Y);
                    ctl.Size = new Size(width, height);

                    int posIncrement = 0;
                    if (lastColSpan > 1)
                    {
                        //if (IsFixedColumn(currentCol)) posIncrement = _columnWidths[currentCol];
                        lastColSpan--;
                    }
                    else
                    {
                        posIncrement = ctl.Width + GapSize;
                        lastColSpan = ctlInfo.ColumnSpan;
                    }
                    _currentPos.X += posIncrement;
                }
            }
            finally
            {
                ManagedControl.ResumeLayout(true);
            }
        }

        /// <summary>
        /// Calculates the average row height
        /// </summary>
        /// <returns>Returns the average row height</returns>
        private int CalcRowHeight()
        {
            return (ManagedControl.Height - GetFixedHeightIncludingGaps())/GetNumVariableRows();
        }

        /// <summary>
        /// Calculates the average column width
        /// </summary>
        /// <returns>Returns the average column width</returns>
        private int CalcColumnWidth()
        {

            return GetNumVariableColumns() == 0 ? 
                    ManagedControl.Width - GetFixedWidthIncludingGaps() : 
                    (ManagedControl.Width - GetFixedWidthIncludingGaps()) / GetNumVariableColumns();
        }

        /// <summary>
        /// Indicates whether the specified row has a fixed height
        /// </summary>
        /// <param name="rowNumber">The row number in question</param>
        /// <returns>Returns true if fixed, false if not</returns>
        private bool IsFixedRow(int rowNumber)
        {
            return _rowHeights[rowNumber] > -1;
        }

        /// <summary>
        /// Indicates whether the specified column has a fixed width
        /// </summary>
        /// <param name="columnNumber">The column number in question</param>
        /// <returns>Returns true if fixed, false if not</returns>
        private bool IsFixedColumn(int columnNumber)
        {
            return _columnWidths[columnNumber%ColumnCount] > -1;
        }

        /// <summary>
        /// Fixes the width of a column to a specified size
        /// </summary>
        /// <param name="columnNumber">The column in question</param>
        /// <param name="columnWidth">The width to fix the column at</param>
        public void FixColumn(int columnNumber, int columnWidth)
        {
            _columnWidths[columnNumber] = columnWidth;
            RefreshControlPositions();
        }

        /// <summary>
        /// Fixes the height of a row to a specified size
        /// </summary>
        /// <param name="rowNumber">The row in question</param>
        /// <param name="rowHeight">The height to fix the row at</param>
        public void FixRow(int rowNumber, int rowHeight)
        {
            _rowHeights[rowNumber] = rowHeight;
            RefreshControlPositions();
        }

        /// <summary>
        /// Returns the total width of the fixed-width columns added together
        /// </summary>
        /// <returns>Returns the total width</returns>
        private int GetFixedWidth()
        {
            return GetFixedAmount(_columnWidths);
        }

        /// <summary>
        /// Returns the total height of the fixed-height rows added together
        /// </summary>
        /// <returns>Returns the total height</returns>
        private int GetFixedHeight()
        {
            return GetFixedAmount(_rowHeights);
        }

        /// <summary>
        /// Returns the total width of the fixed-width columns added 
        /// together, including the gaps and borders
        /// </summary>
        /// <returns>Returns the total width</returns>
        public int GetFixedWidthIncludingGaps()
        {
            return GetFixedWidth() + (2 * BorderSize) + ((ColumnCount - 1) * GapSize);
        }
        
        /// <summary>
        /// Returns the total height of the fixed-height rows added 
        /// together, including the gaps and borders
        /// </summary>
        /// <returns>Returns the total height</returns>
        public int GetFixedHeightIncludingGaps()
        {
            return GetFixedAmount(_rowHeights) + (2*BorderSize) + ((RowCount - 1)*GapSize);
        }

        /// <summary>
        /// Adds the values in the array provided, as long as the values are
        /// above -1.  This method is used to add up fixed-height/width items.
        /// </summary>
        /// <param name="arr">The array of values</param>
        /// <returns>Returns the total added value</returns>
        private int GetFixedAmount(int[] arr)
        {
            int total = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                int val = arr[i];
                if (val > -1)
                {
                    total += val;
                }
            }
            return total;
        }

        /// <summary>
        /// Counts the number of columns that have not been assigned a
        /// fixed width
        /// </summary>
        /// <returns>Returns the count</returns>
        private int GetNumVariableColumns()
        {
            return GetNumVariableEntries(_columnWidths);
        }

        /// <summary>
        /// Counts the number of rows that have not been assigned a
        /// fixed height
        /// </summary>
        /// <returns>Returns the count</returns>
        private int GetNumVariableRows()
        {
            return GetNumVariableEntries(_rowHeights);
        }

        /// <summary>
        /// Counts the number of items in the array provided that have
        /// a value of -1.  This method is used to count the number of rows
        /// or columns that have not been assigned a fixed width/height
        /// </summary>
        /// <param name="arr">The array of sizes</param>
        /// <returns>Returns the count</returns>
        private int GetNumVariableEntries(int[] arr)
        {
            int num = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                int val = arr[i];
                if (val == -1)
                {
                    num++;
                }
            }
            return num;
        }

        /// <summary>
        /// Fixes a specified column's width based on current or future contents
        /// </summary>
        /// <param name="columnNumber">The column in question</param>
        public void FixColumnBasedOnContents(int columnNumber)
        {
            _fixedColumnsBasedOnContents[columnNumber] = true;
        }

        /// <summary>
        /// Causes the fixed height of all the rows to be determined by the
        /// current or future contents
        /// </summary>
        public void FixAllRowsBasedOnContents()
        {
            _fixAllRowsBasedOnContents = true;
        }

        /// <summary>
        /// Fixes a specified row's height based on current or future contents
        /// </summary>
        /// <param name="rowNumber">The row in question</param>
        public void FixRowBasedOnContents(int rowNumber)
        {
            _fixedRowsBasedOnContents[rowNumber] = true;
        }

        /// <summary>
        /// Gets the fixed width set for a specified column.  The return
        /// value will be -1 if the width has not been fixed.
        /// </summary>
        /// <param name="columnNumber">The column in question</param>
        /// <returns>Returns the fixed width or -1</returns>
        public int GetFixedColumnWidth(int columnNumber)
        {
            return this._columnWidths[columnNumber];
        }

        /// <summary>
        /// Manages specific grid information for a control
        /// </summary>
        public class ControlInfo
        {
            private IControlHabanero _control;
            private int _columnSpan;
            private readonly int _rowSpan;

            /// <summary>
            /// Constructor to initialise a new instance.  Sets the spans 
            /// to (1,1)
            /// </summary>
            /// <param name="control">The control in question</param>
            public ControlInfo(IControlHabanero control)
                : this(control, 1, 1)
            {
            }

            /// <summary>
            /// Constructor as before, but requiring the row and column
            /// spans to be specified
            /// </summary>
            public ControlInfo(IControlHabanero control, int columnSpan, int rowSpan)
            {
                _control = control;
                _columnSpan = columnSpan;
                _rowSpan = rowSpan;
            }

            /// <summary>
            /// Returns the control being represented
            /// </summary>
            public IControlHabanero Control
            {
                get { return _control; }
            }

            /// <summary>
            /// Returns the row span (how many rows this cell spans across)
            /// </summary>
            public int RowSpan
            {
                get { return _rowSpan; }
            }

            /// <summary>
            /// Returns the column span (how many columns this cell spans
            /// across)
            /// </summary>
            public int ColumnSpan
            {
                get { return _columnSpan; }
            }
        }

    }
}
