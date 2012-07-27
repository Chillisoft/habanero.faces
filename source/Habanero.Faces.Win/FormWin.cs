// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Habanero.Faces.Base;
using FormBorderStyle = System.Windows.Forms.FormBorderStyle;

namespace Habanero.Faces.Win
{
    /// <summary>
    /// Represents a window or dialog box that makes up an application's user interface
    /// </summary>
    public class FormWin : Form, IFormHabanero
    {
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MAXIMIZE = 0xF030;
        public FormWin()
            :base()
        {
            this.ControlAdded += (sender, e) =>
                {
                    var btn = e.Control as Button;
                    if (btn == null) return;
                    switch (btn.Text.ToLower())
                    {
                        case "ok":
                            this.AcceptButton = btn;
                            break;
                        case "cancel":
                            this.CancelButton = btn;
                            break;
                    }
                };
            if (GlobalUIRegistry.UIStyleHints != null) 
            {
                var resourceName = GlobalUIRegistry.UIStyleHints.FormHints.DefaultIconResourceName;
                if (resourceName != null)
                    this.SetIcon(resourceName);
            }
            this.ParentChanged += (sender, e) =>
                {
                    if (GlobalUIRegistry.UIStyleHints != null)
                    {
                        var iconResourceName = GlobalUIRegistry.UIStyleHints.FormHints.MDIChildIconResourceName ?? GlobalUIRegistry.UIStyleHints.FormHints.DefaultIconResourceName;
                        if (iconResourceName != null)
                            this.SetIcon(iconResourceName);
                    }
                };
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        /// <summary>
        /// Gets or sets the anchoring style.
        /// </summary>
        /// <value></value>
        Base.AnchorStyles IControlHabanero.Anchor
        {
            get { return (Base.AnchorStyles)base.Anchor; }
            set { base.Anchor = (System.Windows.Forms.AnchorStyles)value; }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlHabanero.Dock
        {
            get { return DockStyleWin.GetDockStyle(base.Dock); }
            set { base.Dock = DockStyleWin.GetDockStyle(value); }
        }

        /// <summary>
        /// Shows the form with the specified owner to the user.
        /// </summary>
        /// <param name="owner">Any object that implements System.Windows.Forms.IWin32Window and represents the top-level window that will own this form.</param>
        /// <exception cref="System.ArgumentException">The form specified in the owner parameter is the same as the form being shown.</exception>
        public void Show(IControlHabanero owner)
        {
            base.Show((IWin32Window)owner);
        }

        /// <summary>
        /// Gets or sets the current multiple document interface (MDI) parent form of this form
        /// </summary>
        IFormHabanero IFormHabanero.MdiParent
        {
            get { throw new NotImplementedException(); }
            set { this.MdiParent = (Form)value; }
        }

        /// <summary>
        /// Gets or sets the form's window state.  The default is Normal.
        /// </summary>
        Base.FormWindowState IFormHabanero.WindowState
        {
            get { return (Base.FormWindowState)base.WindowState; }
            set { base.WindowState = (System.Windows.Forms.FormWindowState)value; }
        }

        /// <summary>
        /// Shows the form as a modal dialog box with the currently active window set as its owner
        /// </summary>
        /// <returns>One of the DialogResult values</returns>
        Base.DialogResult IFormHabanero.ShowDialog()
        {
            return (Base.DialogResult)base.ShowDialog();
        }

        /// <summary>
        /// Gets or sets the form start position.
        /// </summary>
        /// <value></value>
        Base.FormStartPosition IFormHabanero.StartPosition
        {
            get { return (Base.FormStartPosition)base.StartPosition; }
            set { base.StartPosition = (System.Windows.Forms.FormStartPosition)value; }
        }

        //event EventHandler IFormHabanero.Closed
        //{
        //    add {
        //        FormClosed += delegate(object sender, FormClosedEventArgs e) { value(sender, ) };
        //    }
        //    remove {
        //        throw new NotImplementedException();
        //    }
        //}

       
        /// <summary>
        /// Gets or sets the dialog result that indicates what action was
        /// taken to close the form
        /// </summary>
        public new Base.DialogResult DialogResult
        {
            get { return (Base.DialogResult)base.DialogResult; }
            set { base.DialogResult = (System.Windows.Forms.DialogResult)value; }
        }

        ///<summary>
        /// Gets or sets the border style of the form.
        ///</summary>
        ///<returns>A <see cref="Base.FormBorderStyle" /> that represents the style of border to display for the form. 
        /// The default is <see cref="Base.FormBorderStyle.Sizable" />.
        ///</returns>
        /// <exceptions>
        /// <see cref="InvalidEnumArgumentException"/>: The value specified is outside the range of valid values.
        /// </exceptions>
        Base.FormBorderStyle IFormHabanero.FormBorderStyle
        {
            get { return (Base.FormBorderStyle)base.FormBorderStyle; }
            set { base.FormBorderStyle = (System.Windows.Forms.FormBorderStyle)value; }
        }

        public void SetIcon(string resourceName)
        {
            var form = this as Form;
            if (form == null) return;
            var s = ResourceStreamer.GetResourceStreamByName(resourceName);
            if (s == null) return;

            try
            {
                var bmp = new Bitmap(Image.FromStream(s));
                form.Icon = Icon.FromHandle(bmp.GetHicon());
            }
            catch (Exception ex)
            {
                Trace.WriteLine(String.Join("", new string[] 
                    { "Unable to load resource by name for form icon: ", resourceName, "\n", ex.Message, "\n", ex.StackTrace}));
            }
        }
    }
}