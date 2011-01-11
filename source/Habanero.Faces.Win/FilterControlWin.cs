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
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Util;
using Habanero.Faces.Base;

namespace Habanero.Faces.Win
{
    /// <summary>
    /// Manages a group of filter controls that create a filter clause used to limit
    /// which rows of data to show on a DataGridView
    /// </summary>
    public class FilterControlWin : PanelWin, IFilterControl
    {
        private readonly IControlFactory _controlFactory;
        private readonly IPanel _controlPanel;
        private readonly IPanel _filterButtonPanel;
        private readonly FilterControlManager _filterControlManager;
        private FilterModes _filterMode;

        /// <summary>
        /// The event that is fired when the filter is ready so that another control e.g. a grid can be filtered.
        /// </summary>
        public event EventHandler Filter;

        ///<summary>
        /// Constructor for <see cref="FilterControlWin"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        public FilterControlWin(IControlFactory controlFactory)
        {
            Height = 50;
            _controlFactory = controlFactory;
            FilterGroupBox = _controlFactory.CreateGroupBox();
//            _groupBox = _controlFactory.CreatePanel();
            _controlFactory.CreateBorderLayoutManager(this).AddControl(FilterGroupBox, BorderLayoutManager.Position.Centre);
            FilterGroupBox.Text = "Filter the Grid";
            BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(FilterGroupBox);
            layoutManager.BorderSize = 20;
            _filterButtonPanel = controlFactory.CreatePanel();
            _filterButtonPanel.Height = 50;
            _filterButtonPanel.Width = 120;   //110;
            _filterButtonPanel.Visible = false;
            CreateFilterButtons(_filterButtonPanel);

            layoutManager.AddControl(_filterButtonPanel, BorderLayoutManager.Position.West);

            _controlPanel = controlFactory.CreatePanel();
            _controlPanel.Width = Width;

            layoutManager.AddControl(_controlPanel, BorderLayoutManager.Position.Centre);

            Height = 50;
            _filterControlManager = new FilterControlManager(controlFactory,
                                                             new FlowLayoutManager(_controlPanel, controlFactory));


        }

        /// <summary>
        /// Adds a TextBox filter in which users can specify text that
        /// a string-value column will be filtered on.  This uses a "like"
        /// operator and accepts any strings that contain the provided clause.
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="propertyName">The business object property on which to filter</param>
        /// <returns>Returns the new TextBox added</returns>
        public ITextBox AddStringFilterTextBox(string labelText, string propertyName)
        {
            ICustomFilter filter = _filterControlManager.AddStringFilterTextBox(labelText, propertyName);
            if (this.FilterMode == FilterModes.Filter )
            {
                filter.ValueChanged += (sender,e) => FireFilterEvent();
            } 
            return (ITextBox) filter.Control;
        }

        /// <summary>
        /// Adds a TextBox filter in which users can specify text that
        /// a string-value column will be filtered on.
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="propertyName">The business object property on which to filter</param>
        /// <param name="filterClauseOperator">The operator to use for the filter clause</param>
        /// <returns>Returns the new TextBox added</returns>
        public ITextBox AddStringFilterTextBox(string labelText, string propertyName,
                                               FilterClauseOperator filterClauseOperator)
        {
            ICustomFilter filter =  _filterControlManager.AddStringFilterTextBox(labelText, propertyName, filterClauseOperator);
            return (ITextBox)filter.Control;
        }

        /// <summary>
        /// Adds a TextBox filter in which users can specify text that
        /// multiple string-value columns will be filtered on.  This uses a "like"
        /// operator and accepts any strings that contain the provided clause.
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="propertyNames">The business object propertys on which to filter</param>
        /// <returns>Returns the new TextBox added</returns>
        public ITextBox AddMultiplePropStringTextBox(string labelText, List<string> propertyNames)
        {
            ICustomFilter filter = _filterControlManager.AddMultiplePropStringTextBox(labelText, propertyNames);
            return (ITextBox) filter.Control;
        }

        /// <summary>
        /// Adds a TextBox filter in which users can specify text that
        /// multiple string-value columns will be filtered on.
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="propertyNames">The business object propertys on which to filter</param>
        /// <param name="filterClauseOperator">The operator to use for the filter clause</param>
        /// <returns>Returns the new TextBox added</returns>
        public ITextBox AddMultiplePropStringTextBox(string labelText, List<string> propertyNames, FilterClauseOperator filterClauseOperator)
        {
            ICustomFilter filter = _filterControlManager.AddMultiplePropStringTextBox(labelText, propertyNames,filterClauseOperator);
            return (ITextBox)filter.Control;
        }

        /// <summary>
        /// Returns the filter clause as a composite of all the specific
        /// clauses in each filter control in the set
        /// </summary>
        /// <returns>Returns the filter clause</returns>
        public IFilterClause GetFilterClause()
        {
            return _filterControlManager.GetFilterClause();
        }

        /// <summary>
        /// Adds a ComboBox filter control
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="propertyName">The business object property on which to filter</param>
        /// <param name="options">The collection of items used to fill the combo box.</param>
        /// <param name="strictMatch">Whether to filter the DataGridView column on a strict match or using a LIKE operator</param>
        /// <returns>Returns the new ComboBox added</returns>
        public IComboBox AddStringFilterComboBox(string labelText, string propertyName, ICollection options,
                                                 bool strictMatch)
        {
            ICustomFilter filter =
                _filterControlManager.AddStringFilterComboBox(labelText, propertyName, options, strictMatch);
            filter.ValueChanged += (sender, e) => { if (this.FilterMode == FilterModes.Filter) FireFilterEvent(); };
            //comboBox.TextChanged += delegate { if (this.FilterMode == FilterModes.Filter) FireFilterEvent(); };
            //comboBox.SelectedIndexChanged += delegate { if (this.FilterMode == FilterModes.Filter) FireFilterEvent(); };
            return (IComboBox) filter.Control;
        }

        ///<summary>
        /// Adds a Filter Combo box for filtering an enum data type.
        ///</summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="propertyName">The business object property on which to filter</param>
        ///<param name="enumType">The Type being showen in the ComboBox</param>
        ///<returns></returns>
        public IComboBox AddEnumFilterComboBox(string labelText, string propertyName, Type enumType)
        {
            ICustomFilter filter =
    _filterControlManager.AddEnumFilterComboBox(labelText, propertyName, enumType);
            filter.ValueChanged += (sender, e) => { if (this.FilterMode == FilterModes.Filter) FireFilterEvent(); };
            return (IComboBox)filter.Control;
        }

        /// <summary>
        /// Adds a ComboBox filter that displays only rows whose boolean value
        /// matches the true/false or null value in the ComboBox. The column (propertyName) of data must
        /// have "true" or "false" as its values (boolean database fields are
        /// usually converted to true/false string values by the Habanero
        /// object manager).
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="propertyName">The business object property on which to filter</param>
        /// <param name="defaultValue">Whether the CheckBox is checked</param>
        /// <returns>Returns the new CheckBox added</returns>
        public IComboBox AddBooleanFilterComboBox(string labelText, string propertyName, bool? defaultValue)
        {
            ICustomFilter filter =
                _filterControlManager.AddBooleanFilterComboBox(labelText, propertyName, defaultValue);
            filter.ValueChanged += (sender, e) => { if (this.FilterMode == FilterModes.Filter) FireFilterEvent(); };
            return (IComboBox)filter.Control;
        }

        /// <summary>
        /// Adds a CheckBox filter that displays only rows whose boolean value
        /// matches the on-off state of the CheckBox. The column of data must
        /// have "true" or "false" as its values (boolean database fields are
        /// usually converted to true/false string values by the Habanero
        /// object manager).
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="propertyName">The business object property on which to filter</param>
        /// <param name="defaultValue">Whether the CheckBox is checked</param>
        /// <returns>Returns the new CheckBox added</returns>
        public ICheckBox AddBooleanFilterCheckBox(string labelText, string propertyName, bool defaultValue)
        {
            ICustomFilter filter = _filterControlManager.AddBooleanFilterCheckBox(labelText, propertyName, defaultValue);

            filter.ValueChanged += delegate { if (this.FilterMode == FilterModes.Filter) FireFilterEvent(); };
            return (ICheckBox)filter.Control;
        }

        /// <summary>
        /// Adds a date-time picker that filters a date column on the date
        /// chosen by the user.  The given operator compares the chosen date
        /// with the date shown in the given column name.
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="propertyName">The business object property on which to filter</param>
        /// <param name="defaultValue">The default date or null</param>
        /// <param name="filterClauseOperator">The operator used to compare
        /// with the date chosen by the user.  The chosen date is on the
        /// right side of the equation.</param>
        /// <param name="nullable">Whether the datetime picker allows null values</param>
        /// <returns>Returns the new DateTimePicker added</returns>
        public IDateTimePicker AddDateFilterDateTimePicker(string labelText, string propertyName, DateTime? defaultValue,
                                                           FilterClauseOperator filterClauseOperator, bool nullable)
        {

            ICustomFilter filter =
                _filterControlManager.AddDateFilterDateTimePicker(labelText, propertyName, filterClauseOperator, defaultValue);
            
            filter.ValueChanged += delegate { if (this.FilterMode == FilterModes.Filter) FireFilterEvent(); };
            return (IDateTimePicker)filter.Control;
        }

        /// <summary>
        /// Applies the filter that has been captured.
        /// This allows an external control (e.g. another button click) to be used as the event that causes the filter to fire.
        /// Typically used when the filter controls are being set manually.
        /// </summary>
        public void ApplyFilter()
        {
            FireFilterEvent();
        }

        /// <summary>
        /// The header text that will be set above the filter.  Defaults to 'Filter'.
        /// </summary>
        public string HeaderText
        {
            get { return FilterGroupBox.Text; }
            set { FilterGroupBox.Text = value; }
        }

        /// <summary>
        /// The number of controls used for filtering that are on the filter control. <see cref="IFilterControl.FilterControls"/>
        /// </summary>
        [Obsolete("Please use FilterControls.Count")]
        public int CountOfFilters
        {
            get { return _filterControlManager.CountOfFilters; }
        }

        /// <summary>
        /// Returns the filter button that when clicked applies the filter
        /// </summary>
        public IButton FilterButton { get; private set; }
        /// <summary>
        /// The Group box that surrounds the Filter Controls
        /// </summary>
        public IGroupBox FilterGroupBox { get; private set; }

        /// <summary>
        /// Returns the clear button that when clicked clears the filter
        /// </summary>
        public IButton ClearButton { get; private set; }

        /// <summary>
        /// Gets and sets the FilterMode <see cref="FilterModes"/>, which determines the
        /// behaviour of the filter control
        /// </summary>
        public FilterModes FilterMode
        {
            get { return _filterMode; }
            set
            {
                _filterMode = value;
                _filterButtonPanel.Visible = (_filterMode == FilterModes.Search);

                if (_filterMode == FilterModes.Filter)
                {
                    FilterButton.Text = "Filter";
                    FilterGroupBox.Text = "Filter the Grid";
                }
                else
                {
                    FilterButton.Text = "Search";
                    FilterGroupBox.Text = "Search the Grid";
                }
            }
        }

        /// <summary>
        /// Gets the collection of individual filters
        /// </summary>
        public List<ICustomFilter> FilterControls
        {
            get { return _filterControlManager.FilterControls; }
        }

        /// <summary>
        /// Returns the filter control used to filter the column for the given property name
        /// </summary>
        /// <param name="propertyName">The property name on the business object</param>
        public IControlHabanero GetChildControl(string propertyName)
        {
            return _filterControlManager.GetChildControl(propertyName);
        }

        /// <summary>
        /// Clears all the values from the filter and calls <see cref="IFilterControl.ApplyFilter"/>
        /// </summary>
        public void ClearFilters()
        {
            _filterControlManager.ClearFilters();
        }


        /// <summary>
        /// Clears all the values from the filter and calls <see cref="IFilterControl.ApplyFilter"/>
        /// </summary>
        public void Clear_Filter_Click(object sender, EventArgs e)
        {
            _filterControlManager.ClearFilters();
            FireFilterEvent();
        }

        /// <summary>
        /// Returns the layout manager used to lay the controls out on the filter control panel.
        /// The default layout manager is the FlowLayoutManager.
        /// </summary>
        public LayoutManager LayoutManager
        {
            get { return _filterControlManager.LayoutManager; }
            set { _filterControlManager.LayoutManager = value; }
        }

        /// <summary>
        /// Returns the panel onto which the filter controls will be placed
        /// </summary>
        public IPanel FilterPanel
        {
            get { return _controlPanel; }
        }

        /// <summary>
        /// Adds a DateRangeComboBox filter which provides common date ranges such as "Today" or "This Year",
        /// so that the grid will only show rows having a date property in the given range
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="columnName">The business object property on which to filter</param>
        /// <param name="includeStartDate">Includes all dates that match the start date exactly</param>
        /// <param name="includeEndDate">Includes all dates that match the end date exactly</param>
        /// <returns>Returns the new DateRangeComboBox added</returns>
        public IDateRangeComboBox AddDateRangeFilterComboBox(string labelText, string columnName, bool includeStartDate,
                                                             bool includeEndDate)
        {
            return AddDateRangeFilterComboBox(labelText, columnName, null, includeStartDate,
                                                                    includeEndDate);
        }

        /// <summary>
        /// Adds a DateRangeComboBox filter which provides common date ranges such as "Today" or "This Year",
        /// so that the grid will only show rows having a date property in the given range
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="columnName">The business object property on which to filter</param>
        /// <param name="options">Provides a specific set of date range options to show</param>
        /// <param name="includeStartDate">Includes all dates that match the start date exactly</param>
        /// <param name="includeEndDate">Includes all dates that match the end date exactly</param>
        /// <returns>Returns the new DateRangeComboBox added</returns>
        public IDateRangeComboBox AddDateRangeFilterComboBox(string labelText, string columnName,
                                                             List<DateRangeOptions> options, bool includeStartDate,
                                                             bool includeEndDate)
        {
            ICustomFilter filter = _filterControlManager.AddDateRangeFilterComboBox(labelText, columnName, options, includeStartDate,
                includeEndDate);
           
            filter.ValueChanged += delegate { if (this.FilterMode == FilterModes.Filter) FireFilterEvent(); };
            return (IDateRangeComboBox)filter.Control;
        }

        /// <summary>
        /// Adds a DateRangeComboBox filter which provides common date ranges such as "Today" or "This Year",
        /// so that the grid will only show rows having a date property in the given range.<br/>
        /// Includes all dates that match the start date exactly.<br/>
        /// Includes all dates that match the end date exactly.<br/>
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="columnName">The business object property on which to filter</param>
        /// <returns>Returns the new DateRangeComboBox added</returns>
        public IDateRangeComboBox AddDateRangeFilterComboBox(string labelText, string columnName)
        {
            return AddDateRangeFilterComboBox(labelText, columnName, null, true, true);
        }

        ///<summary>
        /// Adds a custom filter which allows filtering using an ICustomFilter 
        ///</summary>
        ///<param name="labelText">The Label to appear before the control</param>
        ///<param name="propertyName">The property of the Business Object to filter</param>
        ///<param name="customFilter">The custom filter</param>
        ///<returns>Returns the new Custom Filter Control </returns>
        [Obsolete("Please use the overload without the propertyName parameter")] 
        public IControlHabanero AddCustomFilter(string labelText,string propertyName, ICustomFilter customFilter)
        {
            AddCustomFilter(labelText, customFilter);
            return customFilter.Control;
        }


        ///<summary>
        /// Adds a custom filter which allows filtering using an ICustomFilter 
        ///</summary>
        ///<param name="labelText">The Label to appear before the control</param>
        ///<param name="customFilter">The custom filter</param>
        public void AddCustomFilter(string labelText, ICustomFilter customFilter)
        {
            _filterControlManager.AddCustomFilter(labelText, customFilter);
            customFilter.ValueChanged += delegate { if (this.FilterMode == FilterModes.Filter) FireFilterEvent(); };
        }

        ///<summary>
        /// Adds a static string filter <see cref="StringStaticFilter"/> to the Filter Control.
        /// This allows the developer to set a filter that is always applied and is not modifiable by or visible to the end user.
        ///</summary>
        ///<param name="propertyName"></param>
        ///<param name="filterClauseOperator"></param>
        ///<param name="filterValue"></param>
        public void AddStaticStringFilterClause(string propertyName, FilterClauseOperator filterClauseOperator, string filterValue)
        {
            _filterControlManager.AddStaticStringFilterClause(propertyName, filterClauseOperator, filterValue);
        }

        private void CreateFilterButtons(IPanel filterButtonPanel)
        {
            const int buttonHeight = 20;
            const int buttonWidth = 50; //45;
            FilterButton = CreateFilterButton(buttonWidth, buttonHeight);
            ClearButton = CreateClearButton(buttonWidth, buttonHeight);

            FlowLayoutManager layoutManager = new FlowLayoutManager(filterButtonPanel, _controlFactory);
            layoutManager.AddControl(FilterButton);
            layoutManager.AddControl(ClearButton);
        }

        private IButton CreateClearButton(int buttonWidth, int buttonHeight)
        {
            IButton clearButton = _controlFactory.CreateButton();
            clearButton.Width = buttonWidth;
            clearButton.Height = buttonHeight;
            clearButton.Top = FilterButton.Height + 2;
            clearButton.Text = "Clear";
            clearButton.Click += Clear_Filter_Click;
            return clearButton;
        }

        private IButton CreateFilterButton(int buttonWidth, int buttonHeight)
        {
            IButton filterButton = _controlFactory.CreateButton();
            filterButton.Width = buttonWidth;
            filterButton.Height = buttonHeight;
            filterButton.Top = 0;
            filterButton.Text = "Filter";
            filterButton.Click += Filter_Button_Click;
            return filterButton;
        }

        private void Filter_Button_Click(object sender, EventArgs e)
        {
            FireFilterEvent();
        }

        //private void AssignControlEventHandlers(IControlHabanero control)
        //{
            
        //}

        /// <summary>
        /// Removes the default Click Event. 
        /// Enables the developer to add custom functionality on the button click 
        /// before the filter or search happens.
        /// </summary>
        public void RemoveDefaultFilterClickEvent()
        {
            FilterButton.Click -= Filter_Button_Click;
        }

        /// <summary>
        /// Adds the default Click Event. 
        /// Enables the developer to add the default functionality back on the button click 
        /// if previously removed.
        /// </summary>
        public void AddDefaultFilterClickEvent()
        {
            FilterButton.Click += Filter_Button_Click;
        }

        /// <summary>
        /// Removes the default Click Event. 
        /// Enables the developer to add custom functionality on the button click 
        /// before the Clear or search happens.
        /// </summary>
        public void RemoveDefaultClearClickEvent()
        {
            ClearButton.Click -= Clear_Filter_Click;
        }

        /// <summary>
        /// Adds the default Click Event. 
        /// Enables the developer to add the default functionality back on the button click 
        /// if previously removed.
        /// </summary>
        public void AddDefaultClearClickEvent()
        {
            ClearButton.Click += Clear_Filter_Click;
        }


        private void FireFilterEvent()
        {
            if (Filter != null)
            {
                Filter(this, new EventArgs());
            }
        }
    }
}