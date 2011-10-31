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
using Habanero.Base;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// A filter that comprises a TextBox and filters on a string type.
    /// </summary>
    public class StringTextBoxFilter : ICustomFilter
    {
        private readonly IControlFactory _controlFactory;
        private readonly string _propertyName;
        private readonly FilterClauseOperator _filterClauseOperator;
        private readonly ITextBox _textBox;

        ///<summary>
        /// Constructor for <see cref="StringTextBoxFilter"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="propertyName"></param>
        ///<param name="filterClauseOperator"></param>
        public StringTextBoxFilter(IControlFactory controlFactory, string propertyName, FilterClauseOperator filterClauseOperator)
        {
            _controlFactory = controlFactory;
            _propertyName = propertyName;
            _filterClauseOperator = filterClauseOperator;
            _textBox = _controlFactory.CreateTextBox();
            _textBox.TextChanged += (sender, e) => FireValueChanged();
        }

        private void FireValueChanged()
        {
            if (ValueChanged != null)
            {
                this.ValueChanged(this, new EventArgs());
            }
        }

        ///<summary>
        /// The control that has been constructed by this Control Manager.
        ///</summary>
        public IControlHabanero Control { get { return _textBox; } }

        ///<summary>
        /// Returns the filter clause for this control
        ///</summary>
        ///<param name="filterClauseFactory"></param>
        ///<returns></returns>
        public IFilterClause GetFilterClause(IFilterClauseFactory filterClauseFactory) {
            return _textBox.Text.Length > 0
                       ? filterClauseFactory.CreateStringFilterClause(_propertyName, _filterClauseOperator, _textBox.Text)
                       : filterClauseFactory.CreateNullFilterClause();
        }

        ///<summary>
        /// Clears the <see cref="IDateRangeComboBox"/> of its value
        ///</summary>
        public void Clear() { _textBox.Text = ""; }

        /// <summary>
        /// Event handler that fires when the value in the Filter control changes
        /// </summary>
        public event EventHandler ValueChanged;

        ///<summary>
        /// The name of the property being filtered by.
        ///</summary>
        public string PropertyName { get { return _propertyName; } }

        ///<summary>
        /// Returns the operator <see cref="ICustomFilter.FilterClauseOperator"/> e.g.OpEquals to be used by for creating the Filter Clause.
        ///</summary>
        public FilterClauseOperator FilterClauseOperator { get { return _filterClauseOperator; } }
    }
}