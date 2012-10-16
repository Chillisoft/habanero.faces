using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habanero.Faces.Base.UIHints
{
    public class ComboBoxHints : ControlHints
    {
        private bool _allowTextEditing;
        public bool AllowTextEditing
        {
            get { return this._allowTextEditing; }
            set { this._allowTextEditing = value; RunOnHintsChangedHandler(); }
        }
    }
}
