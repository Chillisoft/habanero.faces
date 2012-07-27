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
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Faces.Base.Async;
using DialogResult = Gizmox.WebGUI.Forms.DialogResult;
using MessageBoxButtons = Gizmox.WebGUI.Forms.MessageBoxButtons;

namespace Habanero.Faces.VWG
{
    /// <summary>
    /// Provides a combination of read-only grid, filter and buttons used to edit a
    /// collection of business objects.
    /// <br/>
    /// Adding, editing and deleting objects is done by clicking the available
    /// buttons in the button control (accessed through the Buttons property).
    /// By default, this uses of a popup form for editing of the object, as defined
    /// in the "form" element of the class definitions for that object.  You can
    /// override the editing controls using the BusinessObjectEditor/Creator/Deletor
    /// properties in this class.
    /// <br/>
    /// A filter control is placed above the grid and is used to filter which rows
    /// are shown.
    /// </summary>
    [MetadataTag("P")]
    public class ReadOnlyGridControlVWG : PanelVWG, IReadOnlyGridControl, ISupportInitialize
    {
        public EventHandler OnAsyncOperationComplete { get; set; }
        public EventHandler OnAsyncOperationStarted { get; set; }
        private readonly IReadOnlyGridButtonsControl _buttons;
        private readonly IControlFactory _controlFactory;
        private readonly ReadOnlyGridVWG _grid;
        private bool _allowUsersToAddBo = true;
        private bool _allowUsersToEditBo = true;
        private readonly ReadOnlyGridControlManager _readOnlyGridControlManager;

        ///<summary>
        /// Constructs a new instance of a <see cref="ReadOnlyGridControlVWG"/>.
        /// This uses the <see cref="IControlFactory"/> from the <see cref="GlobalUIRegistry"/> to construct the control.
        ///</summary>
        public ReadOnlyGridControlVWG() : this(GlobalUIRegistry.ControlFactory)
        {
        }

        ///<summary>
        /// Constructs a new instance of a <see cref="ReadOnlyGridControlVWG"/>.
        ///</summary>
        ///<param name="controlFactory">The <see cref="IControlFactory"/> to use to construct the control.</param>
        public ReadOnlyGridControlVWG(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            _grid = new ReadOnlyGridVWG();
            _readOnlyGridControlManager = new ReadOnlyGridControlManager(this, _controlFactory);
            FilterControl = _controlFactory.CreateFilterControl();
            _buttons = _controlFactory.CreateReadOnlyGridButtonsControl();
            InitialiseButtons();
            InitialiseFilterControl();
            BorderLayoutManager borderLayoutManager = new BorderLayoutManagerVWG(this, _controlFactory);
            borderLayoutManager.AddControl(_grid, BorderLayoutManager.Position.Centre);
            borderLayoutManager.AddControl(_buttons, BorderLayoutManager.Position.South);
            borderLayoutManager.AddControl(FilterControl, BorderLayoutManager.Position.North);
            FilterMode = FilterModes.Filter;
            _grid.Name = "GridControl";
            this.Grid.BusinessObjectSelected += Grid_OnBusinessObjectSelected;
            this.Buttons["Add"].Visible = _allowUsersToAddBo;
            this.Buttons["Edit"].Visible = _allowUsersToEditBo;
        }

        #region IReadOnlyGridControl Members

        /// <summary>
        /// Initiliases the grid structure using the default UI class definition (implicitly named "default")
        /// </summary>
        /// <param name="classDef">The class definition of the business objects shown in the grid</param>
        public void Initialise(IClassDef classDef)
        {
            GridInitialiser.InitialiseGrid(classDef);
        }

        /// <summary>
        /// Initialises the grid structure using the specified UI class definition
        /// </summary>
        /// <param name="classDef">The class definition of the business objects shown in the grid</param>
        /// <param name="uiDefName">The UI definition with the given name</param>
        public void Initialise(IClassDef classDef, string uiDefName)
        {
            GridInitialiser.InitialiseGrid(classDef, uiDefName);
        }

        /// <summary>
        /// Initialises the grid structure using the given UI class definition
        /// </summary>
        /// <param name="classDef">The class definition of the business objects shown in the grid</param>
        /// <param name="gridDef">The grid defition to use</param>
        /// <param name="uiDefName">The name of the grid definition</param>
        public void Initialise(IClassDef classDef, IUIGrid gridDef, string uiDefName)
        {
            GridInitialiser.InitialiseGrid(classDef, gridDef, uiDefName);
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
            get { return Grid.BusinessObjectCollection; }
            set { InternalSetBOCol(value); }
        }

        /// <summary>
        /// Gets or sets the single selected business object (null if none are selected)
        /// denoted by where the current selected cell is
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
                this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
            }
        }

        /// <summary>
        /// Clears the business object collection and the rows in the data table
        /// </summary>
        public void Clear()
        {
            InternalSetBOCol(null);
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
        /// Gets the button control, which contains a set of default buttons for
        /// editing the objects and can be customised
        /// </summary>
        public IReadOnlyGridButtonsControl Buttons
        {
            get { return _buttons; }
        }

        /// <summary>
        /// Gets and sets the business object editor used to edit the object when the edit button is clicked
        /// If no editor is set then the <see cref="DefaultBOEditor"/> is used.
        /// </summary>
        public IBusinessObjectEditor BusinessObjectEditor { get; set; }

        /// <summary>
        /// Gets and sets the business object creator used to create the object when the add button is clicked.
        /// If no creator is set then the <see cref="DefaultBOCreator"/> is used.
        /// </summary>
        public IBusinessObjectCreator BusinessObjectCreator { get; set; }

        /// <summary>
        /// Gets and sets the business object deletor used to delete the object when the delete button is clicked
        /// If no deletor is set then the <see cref="DefaultBODeletor"/> is used.  The default delete button
        /// is hidden unless programmatically shown (using Buttons.ShowDefaultDeleteButton).
        /// </summary>
        public IBusinessObjectDeletor BusinessObjectDeletor { get; set; }

        /// <summary>
        /// Gets and sets the UI definition used to initialise the grid structure (the UI name is indicated
        /// by the "name" attribute on the UI element in the class definitions
        /// </summary>
        public string UiDefName
        {
            get { return this.Grid.UiDefName; }
        }

        /// <summary>
        /// Gets and sets the class definition used to initialise the grid structure
        /// </summary>
        public IClassDef ClassDef
        {
            get { return this.Grid.ClassDef; }
        }

        /// <summary>
        /// Gets the filter control for the readonly grid, which is used to filter
        /// which rows are shown in the grid
        /// </summary>
        public IFilterControl FilterControl { get; private set; }

        /// <summary>
        /// Gets the value indicating whether one of the overloaded initialise
        /// methods been called for the grid
        /// </summary>
        public bool IsInitialised
        {
            get { return GridInitialiser.IsInitialised; }
        }

        /// <summary>
        /// Gets and sets the filter modes for the grid (i.e. filter or search).  See <see cref="FilterModes"/>.
        /// </summary>
        public FilterModes FilterMode
        {
            get { return FilterControl.FilterMode; }
            set { FilterControl.FilterMode = value; }
        }

        /// <summary>
        /// Returns the grid object held. This property can be used to
        /// access a range of functionality for the grid
        /// (eg. myGridWithButtons.Grid.AddBusinessObject(...)).
        /// </summary>    
        public IReadOnlyGrid Grid
        {
            get { return _grid; }
        }

        /// <summary>
        /// Returns the <see cref="IReadOnlyGrid"/> object held. This property can be used to
        /// access a range of functionality for the <see cref="IReadOnlyGrid"/>.
        /// </summary>    
        IGridBase IGridControl.Grid
        {
            get { return this.Grid; }
        }

        /// <summary>
        /// Gets and sets the default order by clause used for loading the grid when the <see cref="FilterModes"/>
        /// is set to Search
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Gets and sets the standard search criteria used for loading the grid when the <see cref="FilterModes"/>
        /// is set to Search or Filter. This search criteria will be appended with an AND to any search criteria returned
        /// by the FilterControl.
        /// </summary>
        public string AdditionalSearchCriteria
        {
            get { return _readOnlyGridControlManager.AdditionalSearchCriterial; }
            set { _readOnlyGridControlManager.AdditionalSearchCriterial = value; }
        }

        /// <summary>
        /// Gets the button control, which contains a set of default buttons for
        /// editing the objects and can be customised
        /// </summary>
        IButtonGroupControl IGridControl.Buttons
        {
            get { return Buttons; }
        }

        /// <summary>
        /// Sets the business object collection to display.  Loading of
        /// the collection needs to be done before it is assigned to the
        /// grid.  This method assumes a default UI definition is to be
        /// used, that is a 'ui' element without a 'name' attribute.
        /// </summary>
        /// <param name="boCollection">The business object collection
        /// to be shown in the grid</param>
        [Obsolete("Please use BusinessObjectCollection")]
        public void SetBusinessObjectCollection(IBusinessObjectCollection boCollection)
        {
            InternalSetBOCol(boCollection);
        }

        private void InternalSetBOCol(IBusinessObjectCollection boCollection)
        {
            _readOnlyGridControlManager.SetBusinessObjectCollection(boCollection);
        }

        /// <summary>
        /// Initialises the grid without a ClassDef. This is used where the columns are set up manually.
        /// A typical case of when you would want to set the columns manually would be when the grid
        /// requires alternate columns, such as images to indicate the state of the object or buttons/links.
        /// The grid must already have at least one column added with the name "HABANERO_OBJECTID". This column is used
        /// to synchronise the grid with the business objects.
        /// </summary>
        /// <exception cref="GridBaseInitialiseException">Occurs where the columns have not
        /// already been defined for the grid</exception>
        public void Initialise()
        {
            GridInitialiser.InitialiseGrid();
        }

        ///<summary>
        /// Enable or disable the default double click handler for the grid where the <see cref="IBusinessObjectEditor"/>
        /// is used to edit the <see cref="IBusinessObject"/> represented by the row that was double clicked.
        /// If you want to implement a custom handler on double click, you should set this to false so that 
        /// the default handler does not interfere with your custom handler. 
        ///</summary>
        public bool DoubleClickEditsBusinessObject
        {
            get { return false; }
            set { }
        }

        public void PopulateCollectionAsync<T>(Criteria criteria, IOrderCriteria order, Action afterPopulation = null) where T : class, IBusinessObject, new()
        {
            this.RunAsyncOperationStarted();
            this.BusinessObjectCollection = Broker.GetBusinessObjectCollection<T>(criteria, order);
            this.RunAsyncOperationComplete();
            if (afterPopulation != null) afterPopulation();
        }

        private void RunAsyncOperationStarted()
        {
            if (this.OnAsyncOperationStarted != null)
                this.OnAsyncOperationStarted(this, new EventArgs());
        }

        private void RunAsyncOperationComplete()
        {
            if (this.OnAsyncOperationComplete != null)
                this.OnAsyncOperationComplete(this, new EventArgs());
        }

        public void PopulateCollectionAsync<T>(string criteria, string order, Action afterPopulation) where T : class, IBusinessObject, new()
        {
            this.PopulateCollectionAsync<T>(CriteriaParser.CreateCriteria(criteria), OrderCriteria.FromString(order), afterPopulation);
        }

        public void PopulateCollectionAsync(DataRetrieverCollectionDelegate dataRetrieverCallback, Action afterPopulation)
        {
            this.RunAsyncOperationStarted();
            this.BusinessObjectCollection = dataRetrieverCallback();
            this.RunAsyncOperationComplete();
            if (afterPopulation != null) afterPopulation();
        }

        ///<summary>
        /// Returns the <see cref="IBusinessObjectCollection"/> that has been set for this <see cref="IGridControl"/>.
        ///</summary>
        ///<returns>Returns the <see cref="IBusinessObjectCollection"/> that has been set for this <see cref="IGridControl"/>.</returns>
        [Obsolete("Please use BusinessObjectCollection")]
        public IBusinessObjectCollection GetBusinessObjectCollection()
        {
            return Grid.BusinessObjectCollection;
        }

        #endregion

        ///<summary>
        ///Signals the object that initialization is starting.
        ///</summary>
        public void BeginInit()
        {
            ((ISupportInitialize) Grid).BeginInit();
        }

        ///<summary>
        ///Signals the object that initialization is complete.
        ///</summary>
        public void EndInit()
        {
            ((ISupportInitialize) Grid).EndInit();
        }

        private void InitialiseFilterControl()
        {
            FilterControl.Filter += FilterControl_OnFilter;
        }

        private void FilterControl_OnFilter(object sender, EventArgs e)
        {
            RefreshFilter();
        }

        ///<summary>
        /// Reapplies the current filter to the Grid.
        /// If in Search mode then the data is entirely reloaded from the DataStore.
        /// Else the BusinessObjectCollection is filtered.
        ///</summary>
        public void RefreshFilter()
        {
            this._readOnlyGridControlManager.RefreshFilter();
        }

        private void InitialiseButtons()
        {
            _buttons.AddClicked += Buttons_AddClicked;
            _buttons.EditClicked += Buttons_EditClicked;
            _buttons.DeleteClicked += Buttons_DeleteClicked;
            _buttons.Name = "ButtonControl";
        }

        private void Buttons_DeleteClicked(object sender, EventArgs e)
        {
            try
            {
                if (Grid.BusinessObjectCollection == null)
                {
                    throw new GridDeveloperException("You cannot call delete since the grid has not been set up");
                }
                IBusinessObject selectedBo = SelectedBusinessObject;

                if (selectedBo != null)
                {
                    if (!ConfirmDeletion)
                    {
                        _readOnlyGridControlManager.DeleteBusinessObject(selectedBo);
                        return;
                    }
                    //This cannot be generalised for Win an Web due to the fact that you cannot 
                    // have a true Modal dialogue in VWG see VWG website for more details.
                    ConfirmDeleteObjectMethod(selectedBo);
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "There was a problem deleting", "Problem Deleting");
            }
        }

        /// <summary>
        /// Default Delegate method for Confirming the Deletion of an object.
        /// </summary>
        /// <param name="selectedBo"></param>
        protected virtual void ConfirmDeleteObjectMethod(IBusinessObject selectedBo)
        {
            MessageBox.Show("Are you certain you want to delete the object '" + selectedBo + "'",
                            "Delete Object", MessageBoxButtons.YesNo,
                            (msgBoxSender, e1) => DeleteObjectMethod(((Form) msgBoxSender).DialogResult, selectedBo));
        }

        /// <summary>
        /// Default method for deleting an object.
        /// </summary>
        /// <param name="dialogResult"></param>
        /// <param name="selectedBo"></param>
        protected void DeleteObjectMethod(DialogResult dialogResult, IBusinessObject selectedBo)
        {
            if (dialogResult != DialogResult.Yes) return;

            _readOnlyGridControlManager.DeleteBusinessObject(selectedBo);
            _grid.SelectedBusinessObject = null;
        }

        private void Buttons_EditClicked(object sender, EventArgs e)
        {
            try
            {
                if (Grid.BusinessObjectCollection == null)
                {
                    throw new GridDeveloperException("You cannot call edit since the grid has not been set up");
                }
                IBusinessObject selectedBo = SelectedBusinessObject;
                if (selectedBo != null)
                {
                    if (BusinessObjectEditor != null)
                    {
                        //This cannot be generalised for Win an Web due to the fact that you cannot 
                        // have a true Modal dialogue in VWG see VWG website for more details.
                        BusinessObjectEditor.EditObject(selectedBo, UiDefName, delegate
                                                                                    {
                                                                                        _grid.RefreshBusinessObjectRow(
                                                                                            selectedBo);
                                                                                        _grid.Update();
                                                                                        _grid.SelectedBusinessObject =
                                                                                            selectedBo;
                                                                                    });
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error trying to edit an item");
            }
        }

        private void Buttons_AddClicked(object sender, EventArgs e)
        {
            try
            {
                if (Grid.BusinessObjectCollection == null)
                {
                    throw new GridDeveloperException("You cannot call add since the grid has not been set up");
                }
                if (BusinessObjectCreator == null)
                {
                    throw new GridDeveloperException(
                        "You cannot call add as there is no business object creator set up for the grid");
                }
                _grid.SelectedBusinessObject = null;
                var newBo = BusinessObjectCreator.CreateBusinessObject();
                if (BusinessObjectEditor != null && newBo != null)
                {
                    //This cannot be generalised for Win an Web due to the fact that you cannot 
                    // have a true Modal dialogue in VWG see VWG website for more details.
                    BusinessObjectEditor.EditObject(newBo, UiDefName, delegate(IBusinessObject bo, bool cancelled)
                                {
                                    IBusinessObjectCollection collection = this.Grid.BusinessObjectCollection;
                                    if (cancelled)
                                    {
                                        collection.Remove(bo);
                                    }
                                    else
                                    {
                                        if (!collection.Contains(bo))
                                        {
                                            collection.Add(bo);
                                        }
                                        _grid.RefreshBusinessObjectRow(bo);
                                        _grid.Update();
                                        Grid.SelectedBusinessObject = bo;
                                    }
                                });
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error trying to add an item");
            }
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
        /// Gets and sets whether the user can add Business objects via this control.
        /// Note_This method is implemented so as to support the interface but always returns False and the set always sets false.
        ///   This is a readOnly Grid and it makes no sense.
        ///</summary>
        public bool AllowUsersToAddBO
        {
            get { return _allowUsersToAddBo; }
            set
            {
                this.Grid.AllowUserToAddRows = false;
                _allowUsersToAddBo = value;
                this.Buttons["Add"].Visible = value;
            }
        }

        /// <summary>
        /// Gets and sets whether the user can Delete (<see cref="IBusinessObject.MarkForDelete"/>) <see cref="IBusinessObject"/>s via this control
        /// </summary>
        public bool AllowUsersToDeleteBO
        {
            get { return this.Grid.AllowUserToDeleteRows; }
            set
            {
                this.Grid.AllowUserToDeleteRows = value;
                this.Buttons["Delete"].Visible = value;
            }
        }

        /// <summary>
        /// Gets and sets whether the user can edit <see cref="IBusinessObject"/>s via this control
        /// Note_This method is implemented so as to support the interface but always returns False and the set always sets false.
        ///   This is a readOnly Grid and it makes no sense.
        /// </summary>
        public bool AllowUsersToEditBO
        {
            get { return _allowUsersToEditBo; }
            set
            {
                this.Grid.ReadOnly = true;
                _allowUsersToEditBo = value;
                this.Buttons["Edit"].Visible = value;
                this.DoubleClickEditsBusinessObject = value;
            }
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

        private IGridInitialiser GridInitialiser
        {
            get { return _readOnlyGridControlManager.GridInitialiser; }
        }

        #endregion

        public void ExecuteOnUIThread(Delegate method)
        {
            method.DynamicInvoke();
        }

    }
}