using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habanero.Faces.Base.UIHints
{
    public class FormHints
    {
        public string DefaultIconResourceName { get; set; }
        public string MDIChildIconResourceName { get; set; }
        public bool EscapeClosesDialogs { get; set; }
        public bool EscapeClosesMDIForms { get; set; }
        public bool BindFirstOKButtonToAcceptButton { get; set; }
        public bool BindFirstCancelButtonToCancelButton { get; set; }
    }
}
