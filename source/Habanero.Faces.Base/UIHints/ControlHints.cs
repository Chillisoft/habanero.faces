using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Habanero.Faces.Base.UIHints
{
    public class ControlHints
    {
        public int MinimumWidth { get; set; }
        public int MinimumHeight { get; set; }
        public FontFamily DefaultFontFamily { get; set; }
        public float MinimumFontSize { get; set; }
        public bool AttemptToMakeTextFit { get; set; }
        public int MaximumWidth 
        { 
            get 
            { 
                if (_maximumWidth > this.MinimumWidth) 
                    return this._maximumWidth;
                return 0;
            } 
            set { this._maximumWidth = value; }
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
