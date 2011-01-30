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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// This manager groups common logic for IEditableGridControl objects.
    /// Do not use this object in working code.
    /// </summary>
    public class GridBaseManager
    {
        private readonly IGridBase _gridBase;
        private IBusinessObjectCollection _boCol;

        /// <summary>
        /// Occurs when a business object is selected
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectSelected;

        /// <summary>
        /// Handler for the CollectionChanged Event
        /// </summary>
        public event EventHandler CollectionChanged;

        private readonly EventHandler _gridBaseOnSelectionChangedHandler;

        /// <summary>
        /// Boolean so that we can switch on and off the firing of certain events e.g
        /// during loading. This is required to prevent the Grid or other controls 
        /// responding to these events innappropriately.
        /// </summary>
        private bool _fireBusinessObjectSelectedEvent = true;

        ///<summary>
        /// Constructor
        ///</summary>
        ///<param name="gridBase"></param>
        ///<param name="uiDefName"></param>
        public GridBaseManager(IGridBase gridBase, string uiDefName)
        {
            _gridBase = gridBase;
            UiDefName = uiDefName;
            _gridBase.AutoGenerateColumns = false;
            GridLoader = DefaultGridLoader;
            _gridBase.AllowUserToAddRows = false;

            _gridBaseOnSelectionChangedHandler = GridBase_OnSelectionChanged;
            _gridBase.SelectionChanged += _gridBaseOnSelectionChangedHandler;
            AutoSelectFirstItem = true;
        }

        private void GridBase_OnSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                FireBusinessObjectSelected();
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        ///<summary>
        /// Constructor
        ///</summary>
        ///<param name="gridBase"></param>
        public GridBaseManager(IGridBase gridBase) : this(gridBase, "default")
        {
        }

        private void FireBusinessObjectSelected()
        {
            if (this.BusinessObjectSelected != null && _fireBusinessObjectSelectedEvent)
            {
                this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
            }
        }

        /// <summary>
        /// See <see cref="IGridBase"/>.<see cref="IBusinessObjectCollection"/>
        /// </summary>
        public void SetBusinessObjectCollection(IBusinessObjectCollection col)
        {
            if (_gridBase.Columns.Count <= 0 && col != null)
            {
                //if (col == null) return;
                throw new GridBaseSetUpException
                    ("You cannot call SetBusinessObjectCollection if the grid's columns have not been set up");
            }
            _boCol = col;
            if (col == null)
            {
                ClearItems();
                return;
            }
            //Hack_: this is to overcome a bug_ in Gizmox(VWG) where the grid was freezing after delete
            // but should not cause a problem with win since it removed the currently selected 
            // item which is the deleted item
            col.BusinessObjectRemoved += delegate { SelectedBusinessObject = null; };

            GridLoader(_gridBase, _boCol);

            if (_gridBase.Rows.Count > 0)
            {
                if (!IsFirstRowSelected())
                {
                    SelectFirstRow();
                }
                if (AutoSelectFirstItem)
                {
                    FireBusinessObjectSelected();
                }
            }
            FireCollectionChanged();
        }

        private void ClearItems()
        {
            SelectedBusinessObject = null;
            GridLoader(_gridBase, null);
            FireCollectionChanged();
        }

        private void SelectFirstRow()
        {
            try
            {
                _gridBase.SelectionChanged -= _gridBaseOnSelectionChangedHandler;
                _fireBusinessObjectSelectedEvent = false;
                SelectedBusinessObject = null;
                _fireBusinessObjectSelectedEvent = false;
                _gridBase.Rows[0].Selected = AutoSelectFirstItem;
            }
            finally
            {
                _gridBase.SelectionChanged += _gridBaseOnSelectionChangedHandler;
                _fireBusinessObjectSelectedEvent = true;
            }
        }

        private bool IsFirstRowSelected()
        {
            return _gridBase.Rows[0].Selected;
        }

        /// <summary>
        /// See <see cref="IGridBase.DataSetProvider"/>
        /// </summary>
        public IDataSetProvider DataSetProvider { get; private set; }

        /// <summary>
        /// Sets the default grid loader which is used as the default for the GridLoader delegate.
        /// If you want to load in any other way then please set the <see cref="GridLoader"/>
        /// delegate to load your business objects as you require.
        /// </summary>
        public void DefaultGridLoader(IGridBase gridBase, IBusinessObjectCollection boCol)
        {
            if (boCol == null)
            {
                gridBase.DataSource = null;
                return;
            }
            var bindingListView = GetBindingListView(boCol);
            try
            {
                gridBase.SelectionChanged -= _gridBaseOnSelectionChangedHandler;
                _fireBusinessObjectSelectedEvent = false;
                gridBase.DataSource = bindingListView;
                if (!AutoSelectFirstItem) gridBase.SelectedBusinessObject = null;
            }
            finally
            {
                gridBase.SelectionChanged += _gridBaseOnSelectionChangedHandler;
                _fireBusinessObjectSelectedEvent = true;
            }
        }

        ///<summary>
        /// Refreshes the row values for the specified <see cref="IBusinessObject"/>.
        ///</summary>
        ///<param name="businessObject">The <see cref="IBusinessObject"/> for which the row must be refreshed.</param>
        public void RefreshBusinessObjectRow(IBusinessObject businessObject)
        {
            if (DataSetProvider == null) return;
            DataSetProvider.UpdateBusinessObjectRowValues(businessObject);
        }

        /// <summary>
        /// Returns a DataView based on the <see cref="IBusinessObjectCollection"/> defined by <paramref name="boCol"/>.
        /// The Columns in the <see cref="DataView"/> will be the collumns defined in the Grids <see cref="UiDefName"/>
        /// </summary>
        /// <param name="boCol">The collection that the DataView is based on</param>
        /// <returns></returns>
        protected virtual IBindingListView GetBindingListView(IBusinessObjectCollection boCol)
        {
            DataSetProvider = _gridBase.CreateDataSetProvider(boCol);
            if (this.ClassDef == null || this.ClassDef != _boCol.ClassDef)
            {
                this.ClassDef = _boCol.ClassDef;
            }
            var uiDef = ((ClassDef) this.ClassDef).GetUIDef(UiDefName);
            if (uiDef == null)
            {
                throw new ArgumentException
                    (String.Format
                         ("You cannot Get the data for the grid {0} since the uiDef {1} cannot be found for the classDef {2}",
                          this._gridBase.Name, UiDefName, ((ClassDef) this.ClassDef).ClassName));
            }
            return DataSetProvider.GetDataView(uiDef.UIGrid);
        }

        /// <summary>
        /// See <see cref="IBOColSelectorControl.SelectedBusinessObject"/>
        /// </summary>
        public IBusinessObject SelectedBusinessObject
        {
            get
            {
//                int rownum = -1;
//                for (int i = 0; i < _gridBase.Rows.Count; i++)
//                    if (_gridBase.Rows[i].Selected) rownum = i;
                for (int i = _gridBase.Rows.Count - 1; i >= 0; i--)
                {
                    if (_gridBase.Rows[i].Selected)
                    {
                        return this.GetBusinessObjectAtRow(i);
                    }
                }
                return null;
            }
            set
            {
                if (_boCol == null && value != null)
                {
                    throw new GridBaseInitialiseException
                        ("You cannot call SelectedBusinessObject if the collection is not set");
                }
                IDataGridViewRowCollection gridRows = _gridBase.Rows;
                try
                {
                    _gridBase.SelectionChanged -= _gridBaseOnSelectionChangedHandler;
                    _fireBusinessObjectSelectedEvent = false;
                    ClearAllSelectedRows(gridRows);
                    if (value == null)
                    {
                        _gridBase.CurrentCell = null;
                        return;
                    }
                }
                finally
                {
                    _fireBusinessObjectSelectedEvent = true;
                    _gridBase.SelectionChanged += _gridBaseOnSelectionChangedHandler;
                }
                BusinessObject bo = (BusinessObject) value;
                bool boFoundAndHighlighted = false;
                int rowNum = 0;
                foreach (IDataGridViewRow row in gridRows)
                {
                    if (GetRowObjectIDValue(row) == bo.ID.ObjectID)
                    {
                        gridRows[rowNum].Selected = true;
                        boFoundAndHighlighted = true;
                        _gridBase.ChangeToPageOfRow(rowNum);
                        break;
                    }
                    rowNum++;
                }

                if (boFoundAndHighlighted && rowNum >= 0 && rowNum < gridRows.Count)
                {
                    if (_gridBase != null)
                    {
                        IDataGridViewRow row = _gridBase.Rows[rowNum];
                        if (row != null)
                        {
                            IDataGridViewCell cell = row.Cells[1];
                            if (cell != null && cell.RowIndex >= 0) _gridBase.CurrentCell = cell;
                        }
                    }
                    if (_gridBase != null)
                        if (_gridBase.CurrentRow != null && !_gridBase.CurrentRow.Displayed)
                        {
                            try
                            {
                                _gridBase.FirstDisplayedScrollingRowIndex = _gridBase.Rows.IndexOf(_gridBase.CurrentRow);
                                gridRows[rowNum].Selected = true; //Getting turned off for some reason
                            }
                            catch (InvalidOperationException)
                            {
                                //Do nothing - designed to catch error "No room is available to display rows"
                                //  when grid height is insufficient
                            }
                        }
                }
            }
        }


        private static void ClearAllSelectedRows(IDataGridViewRowCollection gridRows)
        {
            for (int i = 0; i < gridRows.Count; i++)
            {
                IDataGridViewRow row = gridRows[i];
                row.Selected = false;
            }
        }

        /// <summary>
        /// See <see cref="IGridBase.SelectedBusinessObjects"/>
        /// </summary>
        public IList<BusinessObject> SelectedBusinessObjects
        {
            get
            {
                if (_boCol == null) return new List<BusinessObject>();
                BusinessObjectCollection<BusinessObject> busObjects = new BusinessObjectCollection<BusinessObject>
                    (this.ClassDef);
                foreach (IDataGridViewRow row in _gridBase.SelectedRows)
                {
                    BusinessObject businessObject = (BusinessObject) GetBusinessObjectAtRow(row.Index);
                    busObjects.Add(businessObject);
                }
                return busObjects;
            }
        }

        /// <summary>
        /// See <see cref="IGridBase.GridLoader"/>
        /// </summary>
        public GridLoaderDelegate GridLoader { get; set; }

        /// <summary>
        /// See <see cref="IGridControl.UiDefName"/>
        /// </summary>
        public string UiDefName { get; set; }

        /// <summary>
        /// See <see cref="IGridControl.ClassDef"/>
        /// </summary>
        public IClassDef ClassDef { get; set; }

        private void FireCollectionChanged()
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// See <see cref="IBOColSelectorControl.BusinessObjectCollection"/>
        /// </summary>
        public IBusinessObjectCollection GetBusinessObjectCollection()
        {
            return _boCol;
        }

        /// <summary>
        /// See <see cref="IBOColSelectorControl.GetBusinessObjectAtRow"/>
        /// </summary>
        public IBusinessObject GetBusinessObjectAtRow(int rowIndex)
        {
            IDataGridViewRow findRow = GetGridRow(rowIndex);

            if (HasObjectIDColumn(findRow))
            {
                Guid value = GetRowObjectIDValue(findRow);
                if (this.DataSetProvider != null)
                {
                    return this.DataSetProvider.Find(value);
                }
                if (_boCol != null)
                {
                    var businessObject = this._boCol.Find(value);
                    return businessObject ?? LoadBusinessObject(value);
                }
            }
            if (_gridBase.DataSource is DataView && this.DataSetProvider != null)
            {
                DataRowView dataRowView = GetDataRowView(rowIndex);
                if (dataRowView == null) return null;

                var result = GetRowObjectIDValue(dataRowView);
                return this.DataSetProvider.Find(result);
            }
            return null;
        }

        private IBusinessObject LoadBusinessObject(Guid value)
        {
            var classDef = _boCol.ClassDef;
            var primaryKeyDef = classDef.PrimaryKeyDef;
            if (primaryKeyDef.IsGuidObjectID)
            {
                var keyPropName = primaryKeyDef.KeyName;
                Criteria criteria = new Criteria(keyPropName, Criteria.ComparisonOp.Equals, value);
                return BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, criteria);
            }
            return null;
        }

        private DataRowView GetDataRowView(int rowIndex)
        {
            try
            {
                var bindingList = _gridBase.DataSource as DataView;
                return bindingList != null ? bindingList[rowIndex] : null;
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        private IDataGridViewRow GetGridRow(int rowIndex)
        {
            try
            {
                return _gridBase.Rows[rowIndex];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        // ReSharper disable EmptyGeneralCatchClause
#pragma warning disable 168
        private bool HasObjectIDColumn(IDataGridViewRow findRow)
        {
            try
            {
                var dataGridViewCell = findRow.Cells[IDColumnName];

                return true;
            }
            catch (Exception)

            {
            }
            return false;
        }
#pragma warning restore 168
        // ReSharper restore EmptyGeneralCatchClause
        /// <summary>
        /// See <see cref="IGridBase.GetBusinessObjectRow"/>
        /// </summary>
        public IDataGridViewRow GetBusinessObjectRow(IBusinessObject businessObject)
        {
            if (businessObject == null) return null;
            Guid boIdGuid = businessObject.ID.ObjectID;
            foreach (IDataGridViewRow row in _gridBase.Rows)
            {
                if (GetRowObjectIDValue(row) == boIdGuid)
                {
                    return row;
                }
            }
            return null;
        }

        private Guid GetRowObjectIDValue(DataRowView dataRowView)
        {
            object idValue = dataRowView.Row[IDColumnName];
            return ConvertToGuid(idValue);
        }

        /// <summary>
        /// Gets the Object ID for a given row.
        /// This assumes that the row has a column <see cref="IDColumnName"/>.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public Guid GetRowObjectIDValue(IDataGridViewRow row)
        {
            object idValue = row.Cells[IDColumnName].Value;
            return ConvertToGuid(idValue);
        }

        private static Guid ConvertToGuid(object idValue)
        {
            if (idValue == null) return Guid.Empty;
            if (idValue is Guid) return (Guid) idValue;
            Guid result;
            StringUtilities.GuidTryParse(idValue.ToString(), out result);
            return result;
        }

        ///<summary>
        /// Returns the name of the column being used for tracking the business object identity.
        /// If a <see cref="IDataSetProvider"/> is used then it will be the <see cref="IDataSetProvider.IDColumnName"/>
        /// Else it will be "HABANERO_OBJECTID".
        ///</summary>
        public string IDColumnName
        {
            get { return DataSetProvider != null ? DataSetProvider.IDColumnName : "HABANERO_OBJECTID"; }
        }

        /// <summary>
        /// Gets and sets whether this selector autoselects the first item or not when a new collection is set.
        /// </summary>
        public bool AutoSelectFirstItem { get; set; }

        /// <summary>
        /// See <see cref="IBOColSelectorControl.Clear"/>
        /// </summary>
        public void Clear()
        {
            SetBusinessObjectCollection(null);
        }

        /// <summary>
        /// See <see cref="IGridBase.ApplyFilter"/>
        /// </summary>
        public void ApplyFilter(IFilterClause filterClause)
        {
            var bindingList = _gridBase.DataSource as IBindingListView;
            if (bindingList == null)
            {
                throw new GridBaseInitialiseException
                    ("You cannot apply filters as the grid DataSource has not been set with a IBindingListView");
            }
            var filterClauseString = filterClause != null ? filterClause.GetFilterClauseString("%", "#") : null;

            try
            {
                bindingList.Filter = filterClauseString;
            }
            catch (Exception e)
            {
                throw new HabaneroApplicationException(
                    e.Message + Environment.NewLine +
                    "An Error Occured while trying to Filter the grid with filterClause '" +
                    filterClauseString + "'", e);
            }
        }

        /// <summary>
        /// Applies a search clause to the underlying collection and reloads the grid.
        /// The search allows you to determine which objects to display using
        /// some criteria.  This is typically generated by the an <see cref="IFilterControl"/>.
        /// </summary>
        /// <param name="searchClause">The search clause</param>
        /// <param name="orderBy"></param>
        public void ApplySearch(string searchClause, string orderBy)
        {
            IBusinessObjectCollection collection = BORegistry.DataAccessor.BusinessObjectLoader.
                GetBusinessObjectCollection(this.ClassDef, searchClause, orderBy);
            SetBusinessObjectCollection(collection);
        }

        /// <summary>
        /// Applies a search clause to the underlying collection and reloads the grid.
        /// The search allows you to determine which objects to display using
        /// some criteria.  This is typically generated by the an <see cref="IFilterControl"/>.
        /// </summary>
        /// <param name="searchClause">The search clause</param>
        /// <param name="orderBy"></param>
        public void ApplySearch(IFilterClause searchClause, string orderBy)
        {
            string filterClauseString = searchClause.GetFilterClauseString("%", "'");
            ApplySearch(filterClauseString, orderBy);
        }

        /// <summary>
        /// See <see cref="IGridBase.RefreshGrid"/>.
        /// This actually just Cancels all edits and reloads the 
        /// current BusinessObjectCollection into the 
        /// grid after the grid has been cleared. This thus only really usefull
        /// if the grid has gotten out of sync with it collection in some way.
        /// </summary>
        public void RefreshGrid()
        {
            _gridBase.CancelEdit();
            IBusinessObjectCollection col = this._gridBase.BusinessObjectCollection;
            IBusinessObject bo = this._gridBase.SelectedBusinessObject;
            Clear();
            SetBusinessObjectCollection(col);
            SelectedBusinessObject = bo;
        }
    }


    /// <summary>
    /// Thrown when a failure occurs while setting up a grid
    /// </summary>
    public class GridBaseSetUpException : Exception
    {
/*        ///<summary>
        /// Base constructor with info and context for seraialisation
        ///</summary>
        ///<param name="info"></param>
        ///<param name="context"></param>
        public GridBaseSetUpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        ///<summary>
        /// Constructor with message and inner exception
        ///</summary>
        ///<param name="message"></param>
        ///<param name="innerException"></param>
        public GridBaseSetUpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        ///<summary>
        /// Base constructor with no parameters
        ///</summary>
        public GridBaseSetUpException()
        {
        }*/

        ///<summary>
        /// Constructor with a basic message
        ///</summary>
        ///<param name="message"></param>
        public GridBaseSetUpException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Thrown when a failure occurs on a grid, indicating that Habanero developers
    /// need to pay attention to aspects of the code
    /// </summary>
    public class GridDeveloperException : HabaneroDeveloperException
    {
        ///<summary>
        ///</summary>
        ///<param name="message"></param>
        public GridDeveloperException(string message) : base(message, "")
        {
        }

/*        ///<summary>
        ///</summary>
        ///<param name="message"></param>
        ///<param name="inner"></param>
        public GridDeveloperException(string message, Exception inner) : base(message, "", inner)
        {
        }

        ///<summary>
        ///</summary>
        ///<param name="info"></param>
        ///<param name="context"></param>
        public GridDeveloperException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        ///<summary>
        ///</summary>
        public GridDeveloperException()
        {
        }*/
    }

    /// <summary>
    /// Thrown when a failure occurred during the initialisation of a grid
    /// </summary>
    public class GridBaseInitialiseException : HabaneroDeveloperException
    {
/*        ///<summary>
        ///</summary>
        ///<param name="info"></param>
        ///<param name="context"></param>
        public GridBaseInitialiseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        ///<summary>
        ///</summary>
        ///<param name="message"></param>
        ///<param name="inner"></param>
        public GridBaseInitialiseException(string message, Exception inner) : base(message, "", inner)
        {
        }

        ///<summary>
        ///</summary>
        public GridBaseInitialiseException()
        {
        }*/

        ///<summary>
        ///</summary>
        ///<param name="message"></param>
        public GridBaseInitialiseException(string message) : base(message, "")
        {
        }
    }
}