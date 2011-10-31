#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using System.Drawing;
using System.Windows.Forms;
using Habanero.Faces.Base;
using AnchorStyles = Habanero.Faces.Base.AnchorStyles;
using DockStyle = Habanero.Faces.Base.DockStyle;

namespace Habanero.Faces.Win
{
    /// <summary>
    /// This is an interface used specificaly for adapting a any control that inherits for Control in 
    /// so that it can be treated as an IControlHabanero and can therefore be used by Faces for Habanero.Binding,
    /// <see cref="PanelBuilder"/>
    /// or any other such required behaviour.
    /// </summary>
    public interface IWinFormsControlAdapter : IControlHabanero, IEquatable<Control>//, IComparable<Control>, IComparable<IWinFormsControlAdapter>
    {
        /// <summary>
        /// The Control being wrapped
        /// </summary>
        Control WrappedControl { get; }
    }

    /// <summary>
    /// This is an interface used specificaly for adapting a any control that inherits from System.Windows.Control 
    /// so that it can be treated as an IControlHabanero and can therefore be used by Faces for Habanero.Binding,
    /// <see cref="PanelBuilder"/>
    /// or any other such required behaviour.
    /// </summary>
    public interface IWinFormsDateTimePickerAdapter : IDateTimePicker, IWinFormsControlAdapter
    {
    }
    /// <summary>
    /// This is an interface used specificaly for adapting a any control that inherits from System.Windows.Control 
    /// so that it can be treated as an IControlHabanero and can therefore be used by Faces for Habanero.Binding,
    /// <see cref="PanelBuilder"/>
    /// or any other such required behaviour.
    /// </summary>
    public interface IWinFormsComboBoxAdapter : IComboBox, IWinFormsControlAdapter
    {
    }

    /// <summary>
    /// This is an interface used specificaly for adapting a any control that inherits from System.Windows.Control 
    /// so that it can be treated as an IControlHabanero and can therefore be used by Faces for Habanero.Binding,
    /// <see cref="PanelBuilder"/>
    /// or any other such required behaviour.
    /// </summary>
    public interface IWinFormsDataGridViewAdapter : IDataGridView, IWinFormsControlAdapter
    {
    }


    /// <summary>
    /// This is an interface used specificaly for adapting a any control that inherits from System.Windows.Control 
    /// so that it can be treated as an IControlHabanero and can therefore be used by Faces for Habanero.Binding,
    /// <see cref="PanelBuilder"/>
    /// or any other such required behaviour.
    /// </summary>
    public interface IWinFormsGridBaseAdapter : IGridBase, IWinFormsControlAdapter
    {
    }
    /// <summary>
    /// This is an interface used specificaly for adapting a any control that inherits from System.Windows.Control 
    /// so that it can be treated as an IControlHabanero and can therefore be used by Faces for Habanero.Binding,
    /// <see cref="PanelBuilder"/>
    /// or any other such required behaviour.
    /// </summary>
    public interface IWinFormsTextBoxAdapter : ITextBox, IWinFormsControlAdapter
    {
    }
    /// <summary>
    /// This is an interface used specificaly for adapting a any control that inherits from System.Windows.Control 
    /// so that it can be treated as an IControlHabanero and can therefore be used by Faces for Habanero.Binding,
    /// <see cref="PanelBuilder"/>
    /// or any other such required behaviour.
    /// </summary>
    public interface IWinFormsCheckBoxAdapter : ICheckBox, IWinFormsControlAdapter
    {
    }
    /// <summary>
    /// This is an interface used specificaly for adapting a any control that inherits from System.Windows.Forms.Control 
    /// so that it can be treated as an IControlHabanero and can therefore be used by Faces for Habanero.Binding,
    /// <see cref="PanelBuilder"/>
    /// or any other such required behaviour.
    /// </summary>
    public interface IWinFormsNumericUpDownAdapter : INumericUpDown, IWinFormsControlAdapter
    {
    }
    /// <summary>
    /// This is an interface used specificaly for adapting a any control that inherits from System.Windows.Forms.Panel 
    /// so that it can be treated as an Habanero.Faces IPanel and can therefore be used by Habanero.Faces for Habanero.Binding,
    /// <see cref="PanelBuilder"/>
    /// or any other such required behaviour.
    /// </summary>
    public interface IWinFormsPanelAdapter : IPanel, IWinFormsControlAdapter
    {
    }
    /// <summary>
    /// This is an interface used specificaly for adapting a control that inherits from System.Windows.Forms.Label 
    /// so that it can be treated as an Habanero.Faces ILable and can therefore be used by Habanero.Faces for Habanero.Binding,
    /// <see cref="PanelBuilder"/>
    /// or any other such required behaviour.
    /// </summary>
    public interface IWinFormsLabelAdapter : ILabel, IWinFormsControlAdapter
    {
    }
    /// <summary>
    /// This is an interface used specificaly for adapting a control that inherits from System.Windows.Forms.GroupBox 
    /// so that it can be treated as an Habanero.Faces IGroupBox and can therefore be used by Habanero.Faces for Habanero.Binding,
    /// <see cref="PanelBuilder"/>
    /// or any other such required behaviour.
    /// </summary>
    public interface IWinFormsGroupBoxAdapter : IGroupBox, IWinFormsControlAdapter
    {
    }
    /// <summary>
    /// This is a ControlWraper for Any Control that Inherits from System.Windows.Forms.Control
    /// It wraps this Control behind a standard interface that allows any Control in a Windows Environment 
    /// to take advantage of the Habanero ControlMappers <see cref="IControlMapper"/>
    /// </summary>
    public class WinFormsControlAdapter : IWinFormsControlAdapter
    {
        public Control WrappedControl { get; private set; }

        ///<summary>
        ///</summary>
        ///<param name="control"></param>
        ///<exception cref="ArgumentNullException"></exception>
        public WinFormsControlAdapter(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            WrappedControl = control;
        }

        public bool Focus()
        {
            return WrappedControl.Focus();
        }

        public void Select()
        {
            WrappedControl.Select();
        }

        public void SuspendLayout()
        {
            WrappedControl.SuspendLayout();
        }

        public void ResumeLayout(bool performLayout)
        {
            WrappedControl.ResumeLayout(performLayout);
        }

        public void Invalidate()
        {
            WrappedControl.Invalidate();
        }

        public void Dispose()
        {
            WrappedControl.Dispose();
        }

        public AnchorStyles Anchor
        {
            get { return (AnchorStyles)WrappedControl.Anchor; }
            set { WrappedControl.Anchor = (System.Windows.Forms.AnchorStyles)value; }
        }

        public int Width
        {
            get { return WrappedControl.Width; }
            set { WrappedControl.Width = value; }
        }

        public IControlCollection Controls
        {
            get { return new ControlCollectionWin(WrappedControl.Controls); }
        }

        public bool Visible
        {
            get { return WrappedControl.Visible; }
            set { WrappedControl.Visible = value; }
        }

        public int TabIndex
        {
            get { return WrappedControl.TabIndex; }
            set { WrappedControl.TabIndex = value; }
        }

        public bool Focused
        {
            get { return WrappedControl.Focused; }
        }

        public int Height
        {
            get { return WrappedControl.Height; }
            set { WrappedControl.Height = value; }
        }

        public int Top
        {
            get { return WrappedControl.Top; }
            set { WrappedControl.Top = value; }
        }

        public int Bottom
        {
            get { return WrappedControl.Bottom; }
        }

        public int Left
        {
            get { return WrappedControl.Top; }
            set { WrappedControl.Top = value; }
        }

        public int Right
        {
            get { return WrappedControl.Top; }
        }

        public string Text
        {
            get { return WrappedControl.Text; }
            set { WrappedControl.Text = value; }
        }

        public string Name
        {
            get { return WrappedControl.Name; }
            set { WrappedControl.Name = value; }
        }

        public bool Enabled
        {
            get { return WrappedControl.Enabled; }
            set { WrappedControl.Enabled = value; }
        }

        public Color ForeColor
        {
            get { return WrappedControl.ForeColor; }
            set { WrappedControl.ForeColor = value; }
        }

        public Color BackColor
        {
            get { return WrappedControl.BackColor; }
            set { WrappedControl.BackColor = value; }
        }

        public bool TabStop
        {
            get { return WrappedControl.TabStop; }
            set { WrappedControl.TabStop = value; }
        }

        public Size Size
        {
            get { return WrappedControl.Size; }
            set { WrappedControl.Size = value; }
        }

        public Size ClientSize
        {
            get { return WrappedControl.ClientSize; }
            set { WrappedControl.ClientSize = value; }
        }

        public bool HasChildren
        {
            get { return WrappedControl.HasChildren; }
        }

        public Size MaximumSize
        {
            get { return WrappedControl.MaximumSize; }
            set { WrappedControl.MaximumSize = value; }
        }

        public Size MinimumSize
        {
            get { return WrappedControl.MinimumSize; }
            set { WrappedControl.MinimumSize = value; }
        }

        public Font Font
        {
            get { return WrappedControl.Font; }
            set { WrappedControl.Font = value; }
        }

        public Point Location
        {
            get { return WrappedControl.Location; }
            set { WrappedControl.Location = value; }
        }

        public DockStyle Dock
        {
            get { return DockStyleWin.GetDockStyle(WrappedControl.Dock); }
            set { WrappedControl.Dock = DockStyleWin.GetDockStyle(value); }
        }


        public event EventHandler Click;
        public event EventHandler DoubleClick;
        public event EventHandler Resize;
        public event EventHandler VisibleChanged;
        public event EventHandler TextChanged;

        public bool Equals(Control other)
        {
            return ReferenceEquals(this.WrappedControl, other);
        }

        public override bool Equals(object obj)
        {
            if (obj is IWinFormsControlAdapter) return base.Equals(obj);
            var control = obj as Control;
            return control != null && Equals(control);
        }

        public override int GetHashCode()
        {
            return this.WrappedControl != null ? this.WrappedControl.GetHashCode() : base.GetHashCode();
        }
    }
}