using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habanero.Faces.Base.UIHints
{
    public class StaticDataEditorManagerHints : HintsBase
    {
        private bool _autoExpandSections;
        public bool AutoExpandSections
        {
            get { return this._autoExpandSections; }
            set { this._autoExpandSections = value; RunOnHintsChangedHandler(); }
        }
    }
}
