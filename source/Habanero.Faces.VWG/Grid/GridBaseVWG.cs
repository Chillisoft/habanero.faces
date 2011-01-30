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
using Habanero.Base;
using Habanero.BO;
using Habanero.Faces.Base;

namespace Habanero.Faces.VWG
{
    /// <summary>
    /// Provides a DataGridView that is adapted to show business objects
    /// </summary>
    public abstract class GridBaseVWG : DataGridViewVWG, IGridBase
    {
        /// <summary>
        /// Constructor for <see cref="GridBaseVWG"/>
        /// </summary>
        protected GridBaseVWG()
        {
            _manager = new GridBaseManager(this);
            GridBaseManager.BusinessObjectSelected += delegate { FireBusinessObjectSelected(); };
            GridBaseManager.CollectionChanged += delegate { FireCollectionChanged(); };
            ConfirmDeletion = false;
            CheckUserConfirmsDeletionDelegate = CheckUserWantsToDelete;
        }
        /// <summary>
        /// Displays a message box to the user to check if they want to proceed with
        /// deleting the selected rows.
        /// </summary>
        /// <returns>Returns true if the user does want to delete</returns>
        public abstract bool CheckUserWantsToDelete();
        /// <summary>
        /// Occurs when a business object is selected
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectSelected;

        /// <summary>
        /// Occurs when a row is double-clicked by the user
        /// </summary>
        public event RowDoubleClickedHandler RowDoubleClicked;
        // ReSharper disable UnusedMember.Local
        private void ToPreventCompilerWarnings()
        {
            RowDoubleClicked(new object(), new BOEventArgs(null));
        }
        // ReSharper restore UnusedMember.Local
        /// <summary>
        /// Occurs when the collection in the grid is changed
        /// </summary>
        public event EventHandler CollectionChanged;

/*        /// <summary>
        /// Fires an event indicating that the selected business object
        /// is being edited
        /// </summary>
        /// <param name="bo">The business object being edited</param>
        public void FireBusinessObjectEditedEvent(BusinessObject bo)
        {
            FireSelectedBusinessObjectEdited(bo);
        }*/

/*        /// <summary>
        /// Occurs when a business object is being edited
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectEdited;*/

        private readonly GridBaseManager _manager;

        /// <summary>
        /// Gets and sets the UI definition used to initialise the grid structure (the UI name is indicated
        /// by the "name" attribute on the UI element in the class definitions
        /// </summary>
        public string UiDefName
        {
            get { return GridBaseManager.UiDefName; }
            set { GridBaseManager.UiDefName = value; }
        }

        /// <summary>
        /// Gets and sets the class definition used to initialise the grid structure
        /// </summary>
        public IClassDef ClassDef
        {
            get { return GridBaseManager.ClassDef; }
            set { GridBaseManager.ClassDef = value; }
        }

        ///<summary>
        /// Refreshes the row values for the specified <see cref="IBusinessObject"/>.
        ///</summary>
        ///<param name="businessObject">The <see cref="IBusinessObject"/> for which the row must be refreshed.</param>
        public void RefreshBusinessObjectRow(IBusinessObject businessObject)
        {
            this._manager.RefreshBusinessObjectRow(businessObject);
        }

        /// <summary>
        /// Returns the grid base manager for this grid, which centralises common
        /// logic for the different implementations
        /// </summary>
        private GridBaseManager GridBaseManager
        {
            get { return _manager; }
        }

        /// <summary>
        /// Creates a dataset provider that is applicable to this grid. For example, a readonly grid would
        /// return a <see cref="ReadOnlyDataSetProvider"/>, while an editable grid would return an editable one.
        /// </summary>
        /// <param name="col">The collection to create the datasetprovider for</param>
        /// <returns>Returns the data set provider</returns>
        public abstract IDataSetProvider CreateDataSetProvider(IBusinessObjectCollection col);

        private void FireBusinessObjectSelected()
        {
            if (this.BusinessObjectSelected != null)
            {
                this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
            }
        }

        /// <summary>
        /// Reloads the grid based on the grid returned by GetBusinessObjectCollection
        /// </summary>
        public void RefreshGrid()
        {
            GridBaseManager.RefreshGrid();
        }

        /// <summary>
        /// Clears the business object collection and the rows in the data table
        /// </summary>
        public void Clear()
        {
            GridBaseManager.Clear();
        }

        /// <summary>
        /// Sets the business object collection displayed in the grid.  This
        /// collection must be pre-loaded using the collection's Load() command.
        /// The default UI definition will be used, that is a 'ui' element 
        /// without a 'name' attribute.
        /// </summary>
        /// <param name="col">The collection of business objects to display.  This
        /// collection must be pre-loaded.</param>
        public void SetBusinessObjectCollection(IBusinessObjectCollection col)
        {
            BusinessObjectCollection = col;
        }

        /// <summary>
        /// Gets and Sets the business object collection displayed in the grid.  This
        /// collection must be pre-loaded using the collection's Load() command or from the
        /// <see cref="IBusinessObjectLoader"/>.
        /// The default UI definition will be used, that is a 'ui' element 
        /// without a 'name' attribute.
        /// </summary>
        public IBusinessObjectCollection BusinessObjectCollection
        {
            get { return GridBaseManager.GetBusinessObjectCollection(); }
            set { GridBaseManager.SetBusinessObjectCollection(value); }
        }

        /// <summary>
        /// Returns the business object collection being displayed in the grid
        /// </summary>
        /// <returns>Returns a business collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCollection()
        {
            return GridBaseManager.GetBusinessObjectCollection();
        }

        private void FireCollectionChanged()
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets and sets the currently selected business object in the grid
        /// </summary>
        public IBusinessObject SelectedBusinessObject
        {
            get { return GridBaseManager.SelectedBusinessObject; }
            set
            {
                GridBaseManager.SelectedBusinessObject = value;
                this.FireBusinessObjectSelected();
            }
        }

        /// <summary>
        /// Gets a List of currently selected business objects
        /// </summary>
        public IList<BusinessObject> SelectedBusinessObjects
        {
            get { return GridBaseManager.SelectedBusinessObjects; }
        }

        /// <summary>
        /// Returns the business object at the specified row number
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        public IBusinessObject GetBusinessObjectAtRow(int row)
        {
            return GridBaseManager.GetBusinessObjectAtRow(row);
        }

        /// <summary>
        /// Gets and sets whether this selector autoselects the first item or not when a new collection is set.
        /// </summary>
        public bool AutoSelectFirstItem
        {
            get { return GridBaseManager.AutoSelectFirstItem; }
            set { GridBaseManager.AutoSelectFirstItem = value; }
        }

        ///<summary>
        /// Returns the row for the specified <see cref="IBusinessObject"/>.
        ///</summary>
        ///<param name="businessObject">The <see cref="IBusinessObject"/> to search for.</param>
        ///<returns>Returns the row for the specified <see cref="IBusinessObject"/>, 
        /// or null if the <see cref="IBusinessObject"/> is not found in the grid.</returns>
        public IDataGridViewRow GetBusinessObjectRow(IBusinessObject businessObject)
        {
            return GridBaseManager.GetBusinessObjectRow(businessObject);
        }

        /// <summary>
        /// Applies a filter clause to the data table and updates the filter.
        /// The filter allows you to determine which objects to display using
        /// some criteria.  This is typically generated by an <see cref="IFilterControl"/>.
        /// </summary>
        /// <param name="filterClause">The filter clause</param>
        public void ApplyFilter(IFilterClause filterClause)
        {
            GridBaseManager.ApplyFilter(filterClause);
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
            this.GridBaseManager.ApplySearch(searchClause, orderBy);
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
            this.GridBaseManager.ApplySearch(searchClause, orderBy);
        }

        /// <summary>
        /// Gets and sets the delegated grid loader for the grid.
        /// <br/>
        /// This allows the user to implememt a custom
        /// loading strategy. This can be used to load a collection of business objects into a grid with images or buttons
        /// that implement custom code. (Grids loaded with a custom delegate generally cannot be set up to filter 
        /// (grid filters a dataview based on filter criteria),
        /// but can be set up to search (a business object collection loaded with criteria).
        /// For a grid to be filterable the grid must load with a dataview.
        /// <br/>
        /// If no grid loader is specified then the default grid loader is employed. This consists of parsing the collection into 
        /// a dataview and setting this as the datasource.
        /// </summary>
        public GridLoaderDelegate GridLoader
        {
            get { return GridBaseManager.GridLoader; }
            set { GridBaseManager.GridLoader = value; }
        }

        /// <summary>
        /// Gets the grid's DataSet provider, which loads the collection's
        /// data into a DataSet suitable for the grid
        /// </summary>
        public IDataSetProvider DataSetProvider
        {
            get { return GridBaseManager.DataSetProvider; }
        }

        ///<summary>
        /// Returns the name of the column being used for tracking the business object identity.
        /// If a <see cref="IDataSetProvider"/> is used then it will be the <see cref="IDataSetProvider.IDColumnName"/>
        /// Else it will be "HABANERO_OBJECTID".
        ///</summary>
        public string IDColumnName
        {
            get { return GridBaseManager.IDColumnName; }
        }
/*
        /// <summary>
        /// Fires an event indicating that the selected business object
        /// is being edited
        /// </summary>
        /// <param name="bo">The business object being edited</param>
        public void SelectedBusinessObjectEdited(BusinessObject bo)
        {
            FireSelectedBusinessObjectEdited(bo);
        }*/
/*
        private void FireSelectedBusinessObjectEdited(IBusinessObject bo)
        {
            if (this.BusinessObjectEdited != null)
            {
                this.BusinessObjectEdited(this, new BOEventArgs(bo));
            }
        }*/
        /// <summary>Gets the number of items displayed in the <see cref="IBOColSelector"></see>.</summary>
        /// <returns>The number of items in the <see cref="IBOColSelector"></see>.</returns>
        int IBOColSelector.NoOfItems
        {
            get { return this.Rows.Count; }
        }
        /// <summary>
        /// Gets or sets the boolean value that determines whether to confirm
        /// deletion with the user when they have chosen to delete a row
        /// </summary>
        public bool ConfirmDeletion { get; set; }

        /// <summary>
        /// Gets or sets the delegate that checks whether the user wants to delete selected rows
        /// </summary>
        public CheckUserConfirmsDeletion CheckUserConfirmsDeletionDelegate { get; set; }

        /// <summary>
        /// Gets and sets whether the Control is enabled or not
        /// </summary>
        public bool ControlEnabled
        {
            get { return this.Enabled; }
            set { this.Enabled = value; }
        }
    }
}