using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habanero.Faces.Base.UIHints
{
    public class LayoutHints : HintsBase
    {
        private int _defaultBorderSize;
        public int DefaultBorderSize
        {
            get { return this._defaultBorderSize; }
            set { this._defaultBorderSize = value; RunOnHintsChangedHandler(); }
        }

        private int _defaultHorizontalGap;
        public int DefaultHorizontalGap
        {
            get { return this._defaultHorizontalGap; }
            set { this._defaultHorizontalGap = value; RunOnHintsChangedHandler(); }
        }

        private int _defaultVerticalGap;
        public int DefaultVerticalGap
        {
            get { return this._defaultVerticalGap; }
            set { this._defaultVerticalGap = value; RunOnHintsChangedHandler(); }
        }
    }
}
