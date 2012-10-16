using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habanero.Faces.Base.UIHints
{
    public class FormHints : HintsBase
    {
        private string _defaultIconResourceName;
        public string DefaultIconResourceName
        {
            get { return this._defaultIconResourceName; }
            set { this._defaultIconResourceName = value; RunOnHintsChangedHandler();  }
        }

        private string _mdiChildIconResourceName;
        public string MDIChildIconResourceName
        {
            get { return this._mdiChildIconResourceName; }
            set { this._mdiChildIconResourceName = value; RunOnHintsChangedHandler(); }
        }

        private bool _escapeClosesDialogs;
        public bool EscapeClosesDialogs
        {
            get { return this._escapeClosesDialogs; }
            set { this._escapeClosesDialogs = value; RunOnHintsChangedHandler();  }
        }

        private bool _escapeClosesMdiForms;
        public bool EscapeClosesMDIForms
        {
            get { return this._escapeClosesMdiForms; }
            set { this._escapeClosesMdiForms = value; RunOnHintsChangedHandler(); }
        }

        private bool _bindFirstOkButtonToAcceptButton;
        public bool BindFirstOKButtonToAcceptButton
        {
            get { return this._bindFirstOkButtonToAcceptButton; }
            set { this._bindFirstOkButtonToAcceptButton = value; RunOnHintsChangedHandler(); }
        }

        private bool _bindFirstCancelButtonToCancelButton;
        public bool BindFirstCancelButtonToCancelButton
        {
            get { return this._bindFirstCancelButtonToCancelButton; }
            set { this._bindFirstCancelButtonToCancelButton = value; RunOnHintsChangedHandler(); }
        }
    }
}
