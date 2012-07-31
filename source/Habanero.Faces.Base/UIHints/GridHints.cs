using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habanero.Faces.Base.UIHints
{
    public class GridHints
    {
        public GridColumnAutoSizingStrategies ColumnAutoSizingStrategy { get; set; }
        public int ColumnAutoSizingPadding { get; set; }
        public bool EnableAlternateRowColoring { get; set; }
        public bool HideObjectIDColumn { get; set; }
    }
}
