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
using Gizmox.WebGUI.Forms;
using Habanero.Faces.Base;

namespace Habanero.Faces.VWG
{
    /// <summary>
    /// Represents the formatting and style information applied to individual cells 
    /// within a DataGridView control.
    /// </summary>
    internal class DataGridViewCellStyleVWG : IDataGridViewCellStyle
    {
        private readonly DataGridViewCellStyle _dataGridViewCellStyle;

        public DataGridViewCellStyleVWG(DataGridViewCellStyle dataGridViewCellStyle)
        {
            _dataGridViewCellStyle = dataGridViewCellStyle;
        }
        
        public DataGridViewCellStyle DataGridViewCellStyle
        {
            get { return _dataGridViewCellStyle; }
        }

        /// <summary>Applies the specified <see cref="IDataGridViewCellStyle"></see> to the current 
        /// <see cref="IDataGridViewCellStyle"></see>.</summary>
        /// <param name="dataGridViewCellStyle">The <see cref="IDataGridViewCellStyle"></see> to apply 
        /// to the current <see cref="IDataGridViewCellStyle"></see>.</param>
        /// <exception cref="T:System.ArgumentNullException">dataGridViewCellStyle is null.</exception>
        /// <filterpriority>1</filterpriority>
        public void ApplyStyle(IDataGridViewCellStyle dataGridViewCellStyle)
        {
            _dataGridViewCellStyle.ApplyStyle(((DataGridViewCellStyleVWG)dataGridViewCellStyle).DataGridViewCellStyle);
        }

        /// <summary>Creates an exact copy of this <see cref="IDataGridViewCellStyle"></see>.</summary>
        /// <returns>A <see cref="IDataGridViewCellStyle"></see> that represents an exact copy of this cell style.</returns>
        public IDataGridViewCellStyle Clone()
        {
            return new DataGridViewCellStyleVWG(_dataGridViewCellStyle.Clone());
        }

        ///// <summary>Gets or sets the background color of a <see cref="IDataGridView"></see> cell.</summary>
        ///// <returns>A <see cref="T:System.Drawing.Color"></see> that represents the background color of a cell. The default is <see cref="F:System.Drawing.Color.Empty"></see>.</returns>
        ///// <filterpriority>1</filterpriority>
        //public Color BackColor
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        ///// <summary>Gets or sets the value saved to the data source when the user enters a null value into a cell.</summary>
        ///// <returns>The value saved to the data source when the user specifies a null cell value. The default is <see cref="F:System.DBNull.Value"></see>.</returns>
        //public object DataSourceNullValue
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        ///// <summary>Gets or sets the font applied to the textual content of a <see cref="IDataGridView"></see> cell.</summary>
        ///// <returns>The <see cref="T:System.Drawing.Font"></see> applied to the cell text. The default is null.</returns>
        ///// <filterpriority>1</filterpriority>
        //public Font Font
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        ///// <summary>Gets or sets the foreground color of a <see cref="IDataGridView"></see> cell.</summary>
        ///// <returns>A <see cref="T:System.Drawing.Color"></see> that represents the foreground color of a cell. The default is <see cref="F:System.Drawing.Color.Empty"></see>.</returns>
        ///// <filterpriority>1</filterpriority>
        //public Color ForeColor
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        /// <summary>Gets or sets the format string applied to the textual content of a <see cref="IDataGridView"></see> cell.</summary>
        /// <returns>A string that indicates the format of the cell value. The default is <see cref="F:System.String.Empty"></see>.</returns>
        /// <filterpriority>1</filterpriority>
        public string Format
        {
            get { return _dataGridViewCellStyle.Format; }
            set { _dataGridViewCellStyle.Format = value; }
        }

        public IFormatProvider FormatProvider
        {
            get { return _dataGridViewCellStyle.FormatProvider; }
            set { _dataGridViewCellStyle.FormatProvider = value; }
        }

        ///// <summary>Gets or sets the object used to provide culture-specific formatting of <see cref="IDataGridView"></see> cell values.</summary>
        ///// <returns>An <see cref="T:System.IFormatProvider"></see> used for cell formatting. The default is <see cref="P:System.Globalization.CultureInfo.CurrentUICulture"></see>.</returns>
        ///// <filterpriority>1</filterpriority>
        //public IFormatProvider FormatProvider
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        ///// <summary>Gets a value indicating whether the <see cref="IDataGridViewCellStyle.DataSourceNullValue"></see> property has been set.</summary>
        ///// <returns>true if the value of the <see cref="IDataGridViewCellStyle.DataSourceNullValue"></see> property is the default value; otherwise, false.</returns>
        //public bool IsDataSourceNullValueDefault
        //{
        //    get { throw new NotImplementedException(); }
        //}

        ///// <summary>Gets a value that indicates whether the <see cref="IDataGridViewCellStyle.FormatProvider"></see> property has been set.</summary>
        ///// <returns>true if the <see cref="IDataGridViewCellStyle.FormatProvider"></see> property is the default value; otherwise, false.</returns>
        //public bool IsFormatProviderDefault
        //{
        //    get { throw new NotImplementedException(); }
        //}

        ///// <summary>Gets a value indicating whether the <see cref="IDataGridViewCellStyle.NullValue"></see> property has been set.</summary>
        ///// <returns>true if the value of the <see cref="IDataGridViewCellStyle.NullValue"></see> property is the default value; otherwise, false.</returns>
        //public bool IsNullValueDefault
        //{
        //    get { throw new NotImplementedException(); }
        //}

        ///// <summary>Gets or sets the <see cref="IDataGridView"></see> cell display value corresponding to a cell value of <see cref="F:System.DBNull.Value"></see> or null.</summary>
        ///// <returns>The object used to indicate a null value in a cell. The default is <see cref="F:System.String.Empty"></see>.</returns>
        ///// <filterpriority>1</filterpriority>
        //public object NullValue
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        ///// <summary>Gets or sets the background color used by a <see cref="IDataGridView"></see> cell when it is selected.</summary>
        ///// <returns>A <see cref="T:System.Drawing.Color"></see> that represents the background color of a selected cell. The default is <see cref="F:System.Drawing.Color.Empty"></see>.</returns>
        ///// <filterpriority>1</filterpriority>
        //public Color SelectionBackColor
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        ///// <summary>Gets or sets the foreground color used by a <see cref="IDataGridView"></see> cell when it is selected.</summary>
        ///// <returns>A <see cref="T:System.Drawing.Color"></see> that represents the foreground color of a selected cell. The default is <see cref="F:System.Drawing.Color.Empty"></see>.</returns>
        ///// <filterpriority>1</filterpriority>
        //public Color SelectionForeColor
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        ///// <summary>Gets or sets an object that contains additional data related to the <see cref="IDataGridViewCellStyle"></see>.</summary>
        ///// <returns>An object that contains additional data. The default is null.</returns>
        ///// <filterpriority>1</filterpriority>
        //public object Tag
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}
    }
}
