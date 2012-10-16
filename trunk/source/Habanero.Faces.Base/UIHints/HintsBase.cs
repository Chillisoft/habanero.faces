using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habanero.Faces.Base.UIHints
{
    public abstract class HintsBase
    {
        public EventHandler OnHintsChanged { get; set; }
        protected void RunOnHintsChangedHandler()
        {
            if (this.OnHintsChanged != null)
                this.OnHintsChanged(this, new EventArgs());
        }
    }
}
