using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Habanero.Faces.Base.UIHints
{
    public class ControlHints : HintsBase
    {
        private int _minimumWidth;
        public int MinimumWidth
        {
            get { return this._minimumWidth; }
            set { this._minimumWidth = value; }
        }

        private int _minimumHeight;
        public int MinimumHeight
        {
            get { return this._minimumHeight; }
            set { this._minimumHeight = value; RunOnHintsChangedHandler(); }
        }

        private FontFamily _defaultFontFamily;
        public FontFamily DefaultFontFamily
        {
            get { return this._defaultFontFamily; }
            set { this._defaultFontFamily = value; RunOnHintsChangedHandler(); }
        }

        private float _minimumFontSize;
        public float MinimumFontSize
        {
            get { return this._minimumFontSize; }
            set { this._minimumFontSize = value; RunOnHintsChangedHandler();  }
        }

        private bool _attemptToMakeTextFit;
        public bool AttemptToMakeTextFit
        {
            get { return this._attemptToMakeTextFit; }
            set { this._attemptToMakeTextFit = value; RunOnHintsChangedHandler(); }
        }

        public int MaximumWidth 
        { 
            get 
            { 
                if (_maximumWidth > this.MinimumWidth) 
                    return this._maximumWidth;
                return 0;
            }
            set { this._maximumWidth = value; RunOnHintsChangedHandler(); }
        }
        private int _maximumWidth;
        public int MaximumHeight 
        { 
            get
            {
                if (this._maximumHeight > this.MinimumHeight)
                    return this._maximumHeight;
                return 0;
            }
            set
            {
                this._maximumHeight = value;
                RunOnHintsChangedHandler();
            }
        }
        private int _maximumHeight;

        public void Clone(ControlHints src)
        {
            this.MinimumHeight = src.MinimumHeight;
            this.MaximumHeight = src.MaximumHeight;
            this.MinimumWidth = src.MinimumWidth;
            this.MaximumWidth = src.MaximumWidth;
            this.MinimumFontSize = src.MinimumFontSize;
            this.DefaultFontFamily = src.DefaultFontFamily;
        }

        public override bool Equals(object obj)
        {
            var cmp = obj as ControlHints;
            if (obj == null) return false;
            var ret = (
                    this.MinimumHeight == cmp.MinimumHeight &&
                    this.MaximumHeight == cmp.MaximumHeight &&
                    this.MinimumWidth == cmp.MinimumWidth &&
                    this.MaximumWidth == cmp.MaximumWidth &&
                    this.MinimumFontSize == cmp.MinimumFontSize &&
                    this.DefaultFontFamily == cmp.DefaultFontFamily
                );
            return ret;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Join(",", new string[] {
                "hmin = " + this.MinimumHeight.ToString(),
                "wmin = " + this.MinimumWidth.ToString(),
                "hmax = " + this.MaximumHeight.ToString(),
                "wmax = " + this.MaximumWidth.ToString(),
                "mfont = " + this.MinimumFontSize.ToString(),
                "default font = " + ((this.DefaultFontFamily == null) ? "(system default)" : this.DefaultFontFamily.ToString())
            });
        }
    }
}
