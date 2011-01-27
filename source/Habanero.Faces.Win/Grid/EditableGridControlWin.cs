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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;

namespace Habanero.Faces.Win
{
    /// <summary>
    /// Provides a combination of editable grid, filter and buttons used to edit a
    /// collection of business objects
    /// </summary>
    public class EditableGridControlWin : UserControlWin, IEditableGridControl
    {
        private readonly IEditableGridButtonsControl _buttons;
        private readonly IControlFactory _controlFactory;
        private readonly EditableGridControlManager _editableGridManager;
        private readonly IFilterControl _filterControl;
        private readonly IEditableGrid _grid;
        //private bool _allowUserToAddRows;

        ///<summary>
        /// Constructs a new instance of a <see cref="EditableGridControlWin"/>.
        /// This uses the <see cref="IControlFactory"/> from the <see cref="GlobalUIRegistry"/> to construct the control.
        ///</summary>
        public EditableGridControlWin() : this(GlobalUIRegistry.ControlFactory)
        {
        }

        ///<summary>
        /// Constructs a new instance of a <see cref="EditableGridControlWin"/>.
        ///</summary>
        ///<param name="controlFactory">The <see cref="IControlFactory"/> to use to construct the control.</param>
        public EditableGridControlWin(IControlFactory controlFactory)
        {
            if (controlFactory == null)
                throw new HabaneroArgumentException
                    ("controlFactory", "Cannot create an editable grid control if the control factory is null");
            _controlFactory = controlFactory;
            _grid = _controlFactory.CreateEditableGrid();
            _editableGridManager = new EditableGridControlManager(this, controlFactory);
            _buttons = _controlFactory.CreateEditableGridButtonsControl();
            _filterControl = _controlFactory.CreateFilterControl();
            InitialiseButtons();
            InitialiseFilterControl();
            BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(this);
            layoutManager.AddControl(_filterControl, BorderLayoutManager.Position.North);
            layoutManager.AddControl(_grid, BorderLayoutManager.Position.Centre);
            layoutManager.AddControl(_buttons, BorderLayoutManager.Position.South);
            this.Grid.BusinessObjectSelected += Grid_OnBusinessObjectSelected;
            this.AllowUsersToAddBO = true;
            //this.Grid.Rows[0].
        }

        /// <summary>
        /// Initiliases the grid structure using the default UI class definition (implicitly named "default")
        /// </summary>
        /// <param name="classDef">The class definition of the business objects shown in the grid</param>
        public void Initialise(IClassDef classDef)
        {
            _editableGridManager.Initialise(classDef);
        }

        /// <summary>
        /// Initialises the grid structure using the specified UI class definition
        /// </summary>
        /// <param name="classDef">The class definition of the business objects shown in the grid</param>
        /// <param name="uiDefName">The UI definition with the given name</param>
        public void Initialise(IClassDef classDef, string uiDefName)
        {
            _editableGridManager.Initialise(classDef, uiDefName);
        }

        /// <summary>
        /// Initialises the grid structure using the given UI class definition
        /// </summary>
        /// <param name="classDef">The class definition of the business objects shown in the grid</param>
        /// <param name="gridDef">The grid definition to use</param>
        /// <param name="uiDefName">The name of the grid definition.</param>
        public void Initialise(IClassDef classDef, IUIGrid gridDef, string uiDefName) {
            _editableGridManager.Initialise(classDef, gridDef, uiDefName);
        }

        /// <summary>
        /// Gets and sets the UI definition used to initialise the grid structure (the UI name is indicated
        /// by the "name" attribute on the UI element in the class definitions
        /// </summary>
        public string UiDefName
        {
            get { return _grid.UiDefName; }
            //set { _grid.UiDefName = value; }
        }

        /// <summary>
        /// Gets and sets the class definition used to initialise the grid structure
        /// </summary>
        public IClassDef ClassDef
        {
            get { return _grid.ClassDef; }
            //set { _grid.ClassDef = value; }
        }

        /// <summary>
        /// Returns the grid object held. This property can be used to
        /// access a range of functionality for the grid
        /// (eg. myGridWithButtons.Grid.AddBusinessObject(...)).
        /// </summary>    
        public IEditableGrid Grid
        {
            get { return _grid; }
        }

        /// <summary>
        /// Returns the editable grid object held. This property can be used to
        /// access a range of functionality for the grid
        /// (eg. myGridWithButtons.Grid.AddBusinessObject(...)).
        /// </summary>    
        IGridBase IGridControl.Grid
        {
            get { return _grid; }
        }

        /// <summary>
        /// Gets the button control, which contains a set of default buttons for
        /// editing the objects and can be customised
        /// </summary>
        IButtonGroupControl IGridControl.Buttons
        {
            get { return Buttons; }
        }

        ///<summary>
        /// Returns a Flag indicating whether this control has been initialised yet or not.
        ///</summary>
        public bool IsInitialised
        {
            get { return true; }
        }

        /// <summary>
        /// Gets and Sets the business object collection displayed in the grid.  This
        /// collection must be pre-loaded using the collection's Load() command or from the
        /// <see cref="IBusinessObjectLoader"/>.<br/>
        /// If the grid has not been <see cref="IGridControl.Initialise(IClassDef,string)"/>d.
        /// The default UI definition will be used for the class def defined by the business object collection.
        /// </summary>
        public IBusinessObjectCollection BusinessObjectCollection
        {
            get { return this.Grid.BusinessObjectCollection; }
            set { SetBusinessObjectCollection(value); }
        }

        /// <summary>
        /// Gets and sets the currently selected business object in the grid
        /// </summary>
        public IBusinessObject SelectedBusinessObject
        {
            get { return this.Grid.SelectedBusinessObject; }
            set { this.Grid.SelectedBusinessObject = value; }
        }

        /// <summary>
        /// Event Occurs when a business object is selected
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectSelected;

        private void Grid_OnBusinessObjectSelected(object sender, BOEventArgs e)
        {
            if (this.BusinessObjectSelected != null)
            {
                this.BusinessObjectSelected(this, e);
            }
        }

        /// <summary>
        /// Clears the business object collection and the rows in the data table
        /// </summary>
        public void Clear()
        {
            this.BusinessObjectCollection = null;
        }

        /// <summary>Gets the number of items displayed in the <see cref="IBOColSelector"></see>.</summary>
        /// <returns>The number of items in the <see cref="IBOColSelector"></see>.</returns>
        public int NoOfItems
        {
            get { return this.Grid.Rows.Count; }
        }

        /// <summary>
        /// Returns the business object at the specified row number
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        public IBusinessObject GetBusinessObjectAtRow(int row)
        {
            return this.Grid.GetBusinessObjectAtRow(row);
        }

        /// <summary>
        /// Gets and sets whether this selector autoselects the first item or not when a new collection is set.
        /// </summary>
        public bool AutoSelectFirstItem
        {
            get { return this.Grid.AutoSelectFirstItem; }
            set { this.Grid.AutoSelectFirstItem = value; }
        }

        /// <summary>
        /// Sets the business object collection to display.  Loading of
        /// the collection needs to be done before it is assigned to the
        /// grid.  This method assumes a default UI definition is to be
        /// used, that is a 'ui' element without a 'name' attribute.
        /// </summary>
        /// <param name="boCollection">The new business object collection
        /// to be shown in the grid</param>
        public void SetBusinessObjectCollection(IBusinessObjectCollection boCollection)
        {
            _editableGridManager.SetBusinessObjectCollection(boCollection);
        }

        ///<summary>
        /// Returns the <see cref="IBusinessObjectCollection"/> that has been set for this <see cref="IGridControl"/>.
        ///</summary>
        ///<returns>Returns the <see cref="IBusinessObjectCollection"/> that has been set for this <see cref="IGridControl"/>.</returns>
        public IBusinessObjectCollection GetBusinessObjectCollection()
        {
            return Grid.BusinessObjectCollection;
        }

        /// <summary>
        /// Gets the buttons control used to save and cancel changes
        /// </summary>
        public IEditableGridButtonsControl Buttons
        {
            get { return _buttons; }
        }

        /// <summary>
        /// Gets the filter control used to filter which rows are shown in the grid
        /// </summary>
        public IFilterControl FilterControl
        {
            get { return _filterControl; }
        }

        /// <summary>
        /// Gets and sets the filter modes for the grid (i.e. filter or search). See <see cref="FilterModes"/>.
        /// </summary>
        public FilterModes FilterMode
        {
            get { return _filterControl.FilterMode; }
            set { _filterControl.FilterMode = value; }
        }

        /// <summary>
        /// Gets and sets the default order by clause used for loading the grid when the <see cref="FilterModes"/>
        /// is set to Search
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Gets and sets the standard search criteria used for loading the grid when the <see cref="FilterModes"/>
        /// is set to Search. This search criteria will be appended with an AND to any search criteria returned
        /// by the FilterControl.
        /// </summary>
        public string AdditionalSearchCriteria
        {
            get { return _editableGridManager.AdditionalSearchCriterial; }
            set { _editableGridManager.AdditionalSearchCriterial = value; }
        }

        private void InitialiseButtons()
        {
            _buttons.CancelClicked += Buttons_CancelClicked;
            _buttons.SaveClicked += Buttons_SaveClicked;
        }

        private void InitialiseFilterControl()
        {
            _filterControl.Filter += FilterControl_OnFilter;
        }

        private void FilterControl_OnFilter(object sender, EventArgs e)
        {
            RefreshFilter();
        }

        ///<summary>
        /// Refresh the Filter.
        ///</summary>
        public void RefreshFilter()
        {
            this._editableGridManager.RefreshFilter();
        }

        private void Buttons_CancelClicked(object sender, EventArgs e)
        {
            _grid.RejectChanges();
        }

        private void Buttons_SaveClicked(object sender, EventArgs e)
        {
            _grid.SaveChanges();
        }

        #region Implementation of IBOSelectorAndEditor
        /// <summary>
        /// Gets and sets whether the Control is enabled or not
        /// </summary>
        public bool ControlEnabled
        {
            get { return this.Enabled; }
            set { this.Enabled = value; }
        }
        ///<summary>
        /// Gets and sets whether the user can add Business objects via this control
        ///</summary>
        public bool AllowUsersToAddBO
        {
            get { return _editableGridManager.AllowUsersToAddBO; }
            set { _editableGridManager.AllowUsersToAddBO = value; }
        }

        /// <summary>
        /// Gets and sets whether the user can Delete (<see cref="IBusinessObject.MarkForDelete"/>) <see cref="IBusinessObject"/>s via this control
        /// </summary>
        public bool AllowUsersToDeleteBO
        {
            get { return this.Grid.AllowUserToDeleteRows; }
            set { this.Grid.AllowUserToDeleteRows = value; }
        }

        /// <summary>
        /// Gets and sets whether the user can edit <see cref="IBusinessObject"/>s via this control
        /// </summary>
        public bool AllowUsersToEditBO
        {
            get { return !this.Grid.ReadOnly; }
            set { this.Grid.ReadOnly = !value; }
        }

        /// <summary>
        /// Gets or sets a boolean value that determines whether to confirm
        /// deletion with the user when they have chosen to delete a row
        /// </summary>
        public bool ConfirmDeletion
        {
            get { return this.Grid.ConfirmDeletion; }
            set { this.Grid.ConfirmDeletion = value; }
        }

        /// <summary>
        /// Gets or sets the delegate that checks whether the user wants to delete selected rows.
        /// If <see cref="IBOSelectorAndEditor.ConfirmDeletion"/> is true and no specific <see cref="IBOSelectorAndEditor.CheckUserConfirmsDeletionDelegate"/> is set then
        /// a default <see cref="CheckUserConfirmsDeletion"/> is used.
        /// </summary>
        public CheckUserConfirmsDeletion CheckUserConfirmsDeletionDelegate
        {
            get { return this.Grid.CheckUserConfirmsDeletionDelegate; }
            set { this.Grid.CheckUserConfirmsDeletionDelegate = value; }
        }

        #endregion
    }
}
