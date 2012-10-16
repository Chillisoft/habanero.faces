using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habanero.Faces.Base.UIHints
{
    public class GridHints : HintsBase
    {
        public class CellPadding
        {
            public int Left { get; protected set; }
            public int Right { get; protected set; }
            public int Top { get; protected set; }
            public int Bottom { get; protected set; }
            public CellPadding(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Right = right;
                this.Top = top;
                this.Bottom = bottom;
            }
        }

        public GridHints()
        {
            this.Padding = new CellPadding(0, 0, 0, 0);
        }
        private GridColumnAutoSizingStrategies _columnAutoSizingStrategy;
        public GridColumnAutoSizingStrategies ColumnAutoSizingStrategy
        {
            get { return this._columnAutoSizingStrategy; }
            set { this._columnAutoSizingStrategy = value; RunOnHintsChangedHandler();  }
        }

        private int _columnAutoSizingPadding;
        public int ColumnAutoSizingPadding
        {
            get { return this._columnAutoSizingPadding; }
            set { this._columnAutoSizingPadding = value; RunOnHintsChangedHandler(); }
        }

        private bool _enableAlternateRowColoring;
        public bool EnableAlternateRowColoring
        {
            get { return this._enableAlternateRowColoring; }
            set { this._enableAlternateRowColoring = value; RunOnHintsChangedHandler(); }
        }

        private bool _hideObjectIdColumn;
        public bool HideObjectIDColumn
        {
            get { return this._hideObjectIdColumn; }
            set { this._hideObjectIdColumn = value; RunOnHintsChangedHandler(); }
        }

        private bool _autoDisableEditAndDeleteWhenNoSelectedObject;
        public bool AutoDisableEditAndDeleteWhenNoSelectedObject
        {
            get { return this._autoDisableEditAndDeleteWhenNoSelectedObject; }
            set { this._autoDisableEditAndDeleteWhenNoSelectedObject = value; RunOnHintsChangedHandler(); }
        }

        private bool _showDisabledOperationButtons;
        public bool ShowDisabledOperationButtons
        {
            get { return this._showDisabledOperationButtons; }
            set { this._showDisabledOperationButtons = value; RunOnHintsChangedHandler(); }
        }

        private CellPadding _padding;
        public CellPadding Padding
        {
            get { return this._padding; }
            set { this._padding = value; RunOnHintsChangedHandler(); }
        }
    }
}
