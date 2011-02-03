using System;
using System.Collections.Generic;
using System.Data;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// Initialises the structure of a IGridBase.  If a ClassDef is provided, the grid
    /// is initialised using the UI definition provided for that class.  If no
    /// ClassDef is provided, it is assumed that the grid will be set up in code
    /// by the developer.
    /// This only initialises the GridBase i.e. the Columns on the Grid and 
    /// does not initialise things like the FilterControl.
    /// </summary>
    public class GridBaseInitialiser : IGridInitialiser
    {
        private readonly IControlFactory _controlFactory;

        ///<summary>
        /// Initialise the grid with the appropriate control factory.
        ///</summary>
        ///<param name="gridBase"></param>
        ///<param name="controlFactory"></param>
        public GridBaseInitialiser(IGridBase gridBase, IControlFactory controlFactory)
        {
            if (gridBase == null) throw new ArgumentNullException("gridBase");
            this.GridBase = gridBase;
            _controlFactory = controlFactory;
        }

        /// <summary>
        /// Initialises the grid without a ClassDef. This is typically used where the columns are set up manually
        /// for purposes such as adding a column with images to indicate the state of the object or adding a
        /// column with buttons/links.
        /// <br/>
        /// The grid must already have at least one column added. At least one column must be a column with the name
        /// "HABANERO_OBJECTID", which is used to synchronise the grid with the business objects.
        /// </summary>
        /// <exception cref="GridBaseInitialiseException">Thrown in the case where the columns
        /// have not already been defined for the grid</exception>
        /// <exception cref="GridBaseSetUpException">Thrown in the case where the grid has already been initialised</exception>
        public void InitialiseGrid()
        {
            if (IsInitialised) throw new GridBaseSetUpException("You cannot initialise the grid more than once");
            if (GridBase.Columns.Count == 0)
                throw new GridBaseInitialiseException
                    ("You cannot call initialise with no classdef since the ID column has not been added to the grid");
            try
            {
                //Try to get the id column from the grid. If there is no id column or if the id column
                // is not set up with a header then an error should be thrown. This looks like checking if 
                // column is null and throwing the error would achieve this objective.
                IDataGridViewColumn idColumn = GetIDColumn();
#pragma warning disable 168
                string text = idColumn.HeaderText;
#pragma warning restore 168
            }
            catch (NullReferenceException)
            {
                throw new GridBaseInitialiseException
                    ("You cannot call initialise with no classdef since the ID column has not been added to the grid");
            }
            IsInitialised = true;
        }

        private IDataGridViewColumn GetIDColumn()
        {
            return GridBase.Columns[GetGridIDColumnName()];
        }

        /// <summary>
        /// Initialises the grid with the default UI definition for the class,
        /// as provided in the ClassDef
        /// </summary>
        /// <param name="classDef">The ClassDef used to initialise the grid</param>
        public void InitialiseGrid(IClassDef classDef)
        {
            InitialiseGrid(classDef, "default");
        }

        /// <summary>
        /// Initialises the grid with a specified alternate UI definition for the class,
        /// as provided in the ClassDef
        /// </summary>
        /// <param name="classDef">The Classdef used to initialise the grid</param>
        /// <param name="uiDefName">The name of the UI definition</param>
        public void InitialiseGrid(IClassDef classDef, string uiDefName)
        {
            IUIGrid gridDef = GetGridDef((ClassDef) classDef, uiDefName);
            InitialiseGrid(classDef, gridDef, uiDefName);
        }

        /// <summary>
        /// Initialises the grid with a given alternate UI definition for the class
        ///  </summary>
        /// <param name="classDef">The Classdef used to initialise the grid</param>
        /// <param name="uiGridDef">The <see cref="IUIGrid"/> that specifies the grid </param>
        /// <param name="uiDefName">The name of the <see cref="IUIGrid"/></param>
        public void InitialiseGrid(IClassDef classDef, IUIGrid uiGridDef, string uiDefName)
        {
            SetUpGridColumns(classDef, uiGridDef);
            GridBase.UiDefName = uiDefName;
            GridBase.ClassDef = classDef;

            IsInitialised = true;
        }

        /// <summary>
        /// Gets the value indicating whether the grid has been initialised already
        /// </summary>
        public bool IsInitialised { get; private set; }

        private IGridBase GridBase { get; set; }
        private IUIGrid GetGridDef(ClassDef classDef, string uiDefName)
        {
            IUIDef uiDef = classDef.GetUIDef(uiDefName);
            if (uiDef == null)
            {
                throw new ArgumentException
                    (String.Format
                         ("You cannot initialise {0} because it does not contain a definition for UIDef {1} for the class def {2}",
                          this.GridBase.Name, uiDefName, classDef.ClassName));
            }
            IUIGrid gridDef = uiDef.UIGrid;
            if (gridDef == null)
            {
                throw new ArgumentException
                    (String.Format
                         ("You cannot initialise {0} does not contain a grid definition for UIDef {1} for the class def {2}",
                          this.GridBase.Name, uiDefName, classDef.ClassName));
            }
            return gridDef;
        }

        private void SetUpGridColumns(IClassDef classDef, IUIGrid gridDef)
        {
            this.GridBase.Columns.Clear();
            CreateIDColumn();
            CreateColumnForUIDef(classDef, gridDef);
        }

        private void CreateIDColumn()
        {
            var gridIDColumnName = GetGridIDColumnName();
            IDataGridViewColumn col = CreateStandardColumn(gridIDColumnName, gridIDColumnName);
            col.Width = 0;
            col.Visible = false;
            col.ReadOnly = true;
            col.DataPropertyName = gridIDColumnName;
            col.ValueType = typeof (string);
        }

        private string GetGridIDColumnName()
        {
            if (GridBase == null)
            {
                const string errorMessage = "There was an attempt to access the ID field for a grid when the grid is not yet initialised";
                throw new HabaneroDeveloperException(errorMessage, errorMessage);
            }
            return GridBase.IDColumnName;
        }

        private IDataGridViewColumn CreateStandardColumn(string columnName, string columnHeader)
        {
            int colIndex = this.GridBase.Columns.Add(columnName, columnHeader);
            return this.GridBase.Columns[colIndex];
        }

        private IDataGridViewColumn CreateCustomColumn(IUIGridColumn columnDef)
        {
            IDataGridViewColumn newColumn;
            var uiGridColumn = columnDef as UIGridColumn;
            if (uiGridColumn != null && uiGridColumn.GridControlType != null)
            {
                newColumn = _controlFactory.CreateDataGridViewColumn(uiGridColumn.GridControlType);
            }
            else
            {
                newColumn = _controlFactory.CreateDataGridViewColumn
                    (columnDef.GridControlTypeName, columnDef.GridControlAssemblyName);
            }

            GridBase.Columns.Add(newColumn);
            return newColumn;
        }

        private void CreateColumnForUIDef(IClassDef classDef, IUIGrid gridDef)
        {
            foreach (IUIGridColumn gridColDef in gridDef)
            {
                IDataGridViewColumn col;
                if (gridColDef.GridControlTypeName == "DataGridViewComboBoxColumn")
                {
                    IDataGridViewComboBoxColumn comboBoxCol = _controlFactory.CreateDataGridViewComboBoxColumn();

                    IPropDef propDef = GetPropDef(classDef, gridColDef);
                    ILookupList source = null;
                    if (propDef != null) source = propDef.LookupList;
                    if (source != null)
                    {
                        DataTable table = new DataTable();
                        table.Columns.Add("id");
                        table.Columns.Add("str");

                        table.LoadDataRow(new object[] {"", ""}, true);
                        foreach (KeyValuePair<string, string> pair in source.GetLookupList())
                        {
                            table.LoadDataRow(new object[] {pair.Value, pair.Key}, true);
                        }

                        comboBoxCol.DataSource = table;
                        // ReSharper disable ConditionIsAlwaysTrueOrFalse
                        //Hack_: This null check has been placed because of a Gizmox bug_ 
                        //  We posted this at: http://www.visualwebgui.com/Forums/tabid/364/forumid/29/threadid/12420/scope/posts/Default.aspx
                        //  It is causing a StackOverflowException on ValueMember because the DataSource is still null
                       
                        if (comboBoxCol.DataSource != null)

                        {
                            comboBoxCol.ValueMember = "str";
                            comboBoxCol.DisplayMember = "str";
                        }
                        // ReSharper restore ConditionIsAlwaysTrueOrFalse
                    }
                    comboBoxCol.DataPropertyName = gridColDef.PropertyName;
                    col = comboBoxCol;
                    this.GridBase.Columns.Add(col);
                }
                else if (gridColDef.GridControlTypeName == "DataGridViewCheckBoxColumn")
                {
                    col = _controlFactory.CreateDataGridViewCheckBoxColumn();
                    this.GridBase.Columns.Add(col);
                }
                else
                {
                    col = CreateCustomColumn(gridColDef);
                }
                col.HeaderText = gridColDef.GetHeading();
                col.Name = gridColDef.PropertyName;
                col.DataPropertyName = gridColDef.PropertyName;
                col.Width = gridColDef.Width;
                col.Visible = gridColDef.Width != 0;
                col.SortMode = DataGridViewColumnSortMode.Automatic;
                col.ReadOnly = !gridColDef.Editable;
                Type propertyType = GetPropertyType(classDef, gridColDef.PropertyName);
                if (propertyType != typeof (object))
                {
                    col.ValueType = propertyType;
                }
                SetupColumnWithDefParameters(col, gridColDef, propertyType);
            }
        }
        private static Type GetPropertyType(IClassDef classDef, string propertyName)
        {
            ILookupList lookupList = classDef.GetLookupList(propertyName);
            Type propertyType = classDef.GetPropertyType(propertyName);
            if (lookupList != null && !(lookupList is NullLookupList))
            {
                propertyType = typeof(object);
            }
            return propertyType;
        }
        private static void SetupColumnWithDefParameters(IDataGridViewColumn col, IUIGridColumn gridColDef, Type propertyType)
        {
            SetupDateTimeWithParameters(propertyType, gridColDef, col);
            SetupCurrencyWithParameters(propertyType, gridColDef ,col);
        }

        protected static void SetupCurrencyWithParameters(Type propertyType, IUIGridColumn gridColDef, IDataGridViewColumn column)
        {
            if (propertyType != typeof(Double) && propertyType != typeof(Decimal)) return;
            string currencyFormat = gridColDef.GetParameterValue("currencyFormat") as string;
            if (currencyFormat != null)
            {
                column.DefaultCellStyle.Format = currencyFormat;
            }
        }

        private static void SetupDateTimeWithParameters(Type propertyType, IUIGridColumn gridColDef, IDataGridViewColumn col)
        {
            if (propertyType != typeof(DateTime)) return;
            string dateFormat = gridColDef.GetParameterValue("dateFormat") as string;
            if (string.IsNullOrEmpty(dateFormat) && GlobalUIRegistry.DateDisplaySettings != null)
            {
                dateFormat = GlobalUIRegistry.DateDisplaySettings.GridDateFormat;
            }
            if (dateFormat != null)
            {
                col.DefaultCellStyle.Format = dateFormat;
            }
        }

        private static IPropDef GetPropDef(IClassDef classDef, IUIGridColumn gridColumn)
        {
            IPropDef propDef = classDef.GetPropDef(gridColumn.PropertyName, false);
            return propDef;
        }
    }
}