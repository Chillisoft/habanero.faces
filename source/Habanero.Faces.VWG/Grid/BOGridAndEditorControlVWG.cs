using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Faces.Base.Async;
using Habanero.Faces.Base.ControlInterfaces;

namespace Habanero.Faces.VWG.Grid
{

    ///<summary>
    /// Control For VWG that displays a collection of a Business Object along side an editor/creator panel.
    /// The collection of business objects can be shown using any selector control e.g. an <see cref="IEditableGridControl"/>,
    ///   <see cref="IGridControl"/> etc.
    ///</summary>
    public class BOGridAndEditorControlVWG : UserControlVWG, IBOGridAndEditorControl
    {
        private readonly IClassDef _classDef;
        private IControlFactory _controlFactory;
        private IBOEditorControl _iboEditorControl;
        private IReadOnlyGridControl _readOnlyGridControl;
        private IButtonGroupControl _buttonGroupControl;
        private IButton _newButton;
        private IButton _deleteButton;
        private IButton _cancelButton;
        private IButton _saveButton;
        private IBusinessObject _lastSelectedBusinessObject;
        private IBusinessObject _newBO;

        /// <summary>
        /// Event that is raised when a business objects is selected.
        /// </summary>
        public event EventHandler<BOEventArgs<IBusinessObject>> BusinessObjectSelected;

        ///<summary>
        /// Constructor for the <see cref="BOGridAndEditorControlVWG"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="classDef"></param>
        ///<param name="uiDefName"></param>
        ///<exception cref="ArgumentNullException"></exception>
        public BOGridAndEditorControlVWG(IControlFactory controlFactory, IClassDef classDef, string uiDefName)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            if (classDef == null) throw new ArgumentNullException("classDef");
            _classDef = classDef;
            BOEditorControlVWG boEditorControlVWG = new BOEditorControlVWG(controlFactory, classDef, uiDefName);
            SetupGridAndBOEditorControlVWG(controlFactory, boEditorControlVWG, classDef, uiDefName);
        }

        ///<summary>
        /// Constructor for the <see cref="BOGridAndEditorControlVWG"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="iboEditorControl"></param>
        public BOGridAndEditorControlVWG(IControlFactory controlFactory, IBOEditorControl iboEditorControl)
            : this(controlFactory, iboEditorControl, null, "default")
        {
        }

        ///<summary>
        /// Constructor for the <see cref="BOGridAndEditorControlVWG"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="iboEditorControl"></param>
        ///<param name="classDef"></param>
        ///<param name="gridUiDefName"></param>
        public BOGridAndEditorControlVWG(IControlFactory controlFactory, IBOEditorControl iboEditorControl, IClassDef classDef, string gridUiDefName)
        {
            SetupGridAndBOEditorControlVWG(controlFactory, iboEditorControl, classDef, gridUiDefName);
        }

        private void SetupGridAndBOEditorControlVWG(IControlFactory controlFactory, IBOEditorControl iboEditorControl, IClassDef classDef, string gridUiDefName)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            if (iboEditorControl == null) throw new ArgumentNullException("iboEditorControl");

            _controlFactory = controlFactory;
            _iboEditorControl = iboEditorControl;

            IPanel panel = _controlFactory.CreatePanel();
            SetupReadOnlyGridControl(classDef, gridUiDefName);
            SetupButtonGroupControl();
            UpdateControlEnabledState();

            GridLayoutManager manager = new GridLayoutManager(panel, _controlFactory);
            manager.SetGridSize(2, 1);
            manager.FixAllRowsBasedOnContents();
            manager.AddControl(_iboEditorControl);
            manager.AddControl(_buttonGroupControl);

            this.FilterControl = _controlFactory.CreateGenericGridFilter(_readOnlyGridControl.Grid);

            BorderLayoutManager layoutManager = _controlFactory.CreateBorderLayoutManager(this);
            layoutManager.AddControl(this.FilterControl, BorderLayoutManager.Position.North);
            layoutManager.AddControl(_readOnlyGridControl, BorderLayoutManager.Position.West);
            layoutManager.AddControl(panel, BorderLayoutManager.Position.Centre);
            //layoutManager.AddControl(_selectButtonGroupControl, BorderLayoutManager.Position.South);

            _readOnlyGridControl.BusinessObjectSelected +=
                ((sender, e) => FireBusinessObjectSelected(e.BusinessObject));

            _readOnlyGridControl.Grid.SelectionChanged += GridSelectionChanged;
        }

        private void SetupButtonGroupControl()
        {
            _buttonGroupControl = _controlFactory.CreateButtonGroupControl();
            _cancelButton = _buttonGroupControl.AddButton("Cancel", CancelButtonClicked);
            _saveButton = _buttonGroupControl.AddButton("Save", SaveClickHandler);
            _deleteButton = _buttonGroupControl.AddButton("Delete", DeleteButtonClicked);
            _newButton = _buttonGroupControl.AddButton("New", NewButtonClicked);
            _cancelButton.Enabled = false;
            _deleteButton.Enabled = false;
            _newButton.Enabled = false;
        }

        private void SaveClickHandler(object sender, EventArgs e)
        {
            try
            {
                IBusinessObject currentBO = CurrentBusinessObject;
                currentBO.Save();
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        ///<summary>
        /// Returns the Total width of the Selector. <see cref="IGridBase"/>. This is used so that the 
        ///   Selector and BOEditor can be layed out.
        ///</summary>
        ///<param name="grid"></param>
        ///<returns></returns>
        public static int GetGridWidthToFitColumns(IGridBase grid)
        {
            int width = 0;
            if (grid.RowHeadersVisible)
            {
                width = grid.RowHeadersWidth;
            }
            foreach (IDataGridViewColumn column in grid.Columns)
            {
                if (column.Visible) width += column.Width;
            }
            return width;
        }

        private void SetupReadOnlyGridControl(IClassDef classDef, string gridUiDefName)
        {
            _readOnlyGridControl = _controlFactory.CreateReadOnlyGridControl();
            _readOnlyGridControl.Width = 300;
            _readOnlyGridControl.Grid.RowHeadersWidth = 25;
            _readOnlyGridControl.Buttons.Visible = false;
            _readOnlyGridControl.FilterControl.Visible = false;
            if (_iboEditorControl != null)
            {
                if (!string.IsNullOrEmpty(gridUiDefName))
                {
                    _readOnlyGridControl.Initialise(classDef, gridUiDefName);
                    int width = GetGridWidthToFitColumns(_readOnlyGridControl.Grid) + 2;
                    if (width < 300)
                    {
                        _readOnlyGridControl.Width = width;
                    }
                }
            }
            _readOnlyGridControl.DoubleClickEditsBusinessObject = false;
        }

        private void FireBusinessObjectSelected(IBusinessObject businessObject)
        {
            if (BusinessObjectSelected != null)
                this.BusinessObjectSelected(this, new BOEventArgs<IBusinessObject>(businessObject));
        }

        private void GridSelectionChanged(object sender, EventArgs e)
        {
            if (this.SkipSaveOnSelectionChanged)
            {
                SetSelectedBusinessObject();
                return;
            }
            try
            {
                if (_newBO != null)
                {
                    if (_newBO.Status.IsDirty)
                    {
                        if (_newBO.IsValid())
                        {
                            _newBO.Save();
                        }
                        else
                        {
                            _newBO = null;
                        }
                    }
                }
                if (_lastSelectedBusinessObject == null || _readOnlyGridControl.SelectedBusinessObject != _lastSelectedBusinessObject)
                {
                    if (!CheckRowSelectionCanChange()) return;
                    SavePreviousBusinessObject();
                    SetSelectedBusinessObject();
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        /// <summary>
        /// Using the RowValidating event did not work as expected, so this method provides
        /// a way to check whether the grid selection should be forced back to the previous selection
        /// </summary>
        private bool CheckRowSelectionCanChange()
        {
            if (_lastSelectedBusinessObject != null && _readOnlyGridControl.SelectedBusinessObject != _lastSelectedBusinessObject)
            {
                if (!_lastSelectedBusinessObject.IsValid())
                {
                    bool errors = _iboEditorControl.HasErrors;
                    _readOnlyGridControl.SelectedBusinessObject = _lastSelectedBusinessObject;
                    return false;
                }
            }
            return true;
        }

        private void SetSelectedBusinessObject()
        {
            IBusinessObject businessObject = CurrentBusinessObject ?? _newBO;
            _iboEditorControl.BusinessObject = businessObject;
            if (businessObject != null)
            {
                businessObject.PropertyUpdated += PropertyUpdated;
            }
            UpdateControlEnabledState();
            _lastSelectedBusinessObject = businessObject;
        }

        private void PropertyUpdated(object sender, BOPropUpdatedEventArgs eventArgs)
        {
            try
            {
                _cancelButton.Enabled = true;
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        private void SavePreviousBusinessObject()
        {
            if (_lastSelectedBusinessObject != null && !_lastSelectedBusinessObject.Status.IsDeleted)
            {
                _lastSelectedBusinessObject.Save();
            }
        }

        private void CancelButtonClicked(object sender, EventArgs e)
        {
            try
            {
                IBusinessObject currentBO = CurrentBusinessObject ?? _newBO;
                if (currentBO.Status.IsNew)
                {
                    _lastSelectedBusinessObject = null;
                    _readOnlyGridControl.Grid.BusinessObjectCollection.Remove(currentBO);
                    SelectLastRowInGrid();
                }
                else
                {
                    currentBO.CancelEdits();
                }
                UpdateControlEnabledState();
                RefreshGrid();
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        ///<summary>
        /// Refreshes the Selector control (i.e. the 
        ///</summary>
        public void RefreshGrid()
        {
            //Note_: Removing the event prevents the flicker event that happens while a grid is being refreshed
            //  - perhaps the SelectionChanged event should be temporarily removed inside the RefreshGrid method itself?
            RemoveGridSelectionChangedEvent();
            _readOnlyGridControl.Grid.RefreshGrid();
            AddGridSelectionChangedEvent();
        }

        ///<summary>
        /// Adds the <see cref="GridSelectionChanged"/> Event Handler to the <see cref="IDataGridView.SelectionChanged"/> event
        ///</summary>
        internal void AddGridSelectionChangedEvent()
        {
            _readOnlyGridControl.Grid.SelectionChanged += GridSelectionChanged;
        }

        ///<summary>
        /// Removes the <see cref="GridSelectionChanged"/> Event Handler from the <see cref="IDataGridView.SelectionChanged"/> event
        ///</summary>
        public void RemoveGridSelectionChangedEvent()
        {
            _readOnlyGridControl.Grid.SelectionChanged -= GridSelectionChanged;
        }

        private void SelectLastRowInGrid()
        {
            int rowCount = _readOnlyGridControl.Grid.Rows.Count - 1;
            IBusinessObject lastObjectInGrid = _readOnlyGridControl.Grid.GetBusinessObjectAtRow(rowCount);
            _readOnlyGridControl.SelectedBusinessObject = lastObjectInGrid;
        }

        private void UpdateControlEnabledState()
        {
            IBusinessObject selectedBusinessObject = CurrentBusinessObject ?? _newBO;
            bool selectedBusinessObjectNotNull = (selectedBusinessObject != null);
            if (selectedBusinessObjectNotNull)
            {
                string message;
                bool isEditable = selectedBusinessObject.IsEditable(out message);
                this.IBOEditorControl.Enabled = isEditable;
                _saveButton.Enabled = isEditable;
                _cancelButton.Enabled = selectedBusinessObject.Status.IsDirty;
                _deleteButton.Enabled = !selectedBusinessObject.Status.IsNew;
            }
            else
            {
                _iboEditorControl.Enabled = false;
                _deleteButton.Enabled = false;
                _cancelButton.Enabled = false;
            }
            //_iboEditorControl.Enabled = selectedBusinessObjectNotNull;
        }

        /// <summary>
        /// Deliberately adds BO to the grid's collection because current habanero model only
        /// adds BO to grid when it is saved.  This causes a problem when you call col.CreateBO(), since
        /// it adds the BO twice and throws a duplicate key exception.
        /// </summary>a
        private void NewButtonClicked(object sender, EventArgs e)
        {
            try
            {
                _newBO = null;
                IBusinessObject currentBO = CurrentBusinessObject;
                if (currentBO != null)
                {
                    if (!currentBO.IsValid())
                    {
                        bool errors = _iboEditorControl.HasErrors;
                        return;
                    }
                    currentBO.Save();
                }

                IBusinessObjectCollection collection = _readOnlyGridControl.Grid.BusinessObjectCollection;
                _readOnlyGridControl.Grid.SelectionChanged -= GridSelectionChanged;
                _newBO = collection.CreateBusinessObject();
                // _readOnlyGridControl.Grid.GetBusinessObjectCollection().Add(businessObject);
                _readOnlyGridControl.SelectedBusinessObject = _newBO;
                CurrentBusinessObject = _newBO;
                UpdateControlEnabledState();
                //collection.Add(businessObject);
                _readOnlyGridControl.Grid.SelectionChanged += GridSelectionChanged;
                GridSelectionChanged(null, null);
                _iboEditorControl.Focus();
                //_iboEditorControl.ClearErrors();
                _cancelButton.Enabled = true;
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        private void DeleteButtonClicked(object sender, EventArgs e)
        {
            try
            {
                IBusinessObject businessObject = CurrentBusinessObject;
                businessObject.MarkForDelete();
                businessObject.Save();

                if (CurrentBusinessObject == null && _readOnlyGridControl.Grid.Rows.Count > 0)
                {
                    SelectLastRowInGrid();
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        /// <summary>
        /// Sets the business object collection to populate the grid.  If the grid
        /// needs to be cleared, set an empty collection rather than setting to null.
        /// Until you set a collection, the controls are disabled, since any given
        /// collection needs to be provided by a suitable context.
        /// </summary>
        public IBusinessObjectCollection BusinessObjectCollection
        {
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _readOnlyGridControl.BusinessObjectCollection = value;
                _newButton.Enabled = true;
            }
            get
            {
                return _readOnlyGridControl.BusinessObjectCollection;
            }
        }

        /// <summary>
        /// Returns the <see cref="IGridControl"/> that is being used along side of the <see cref="IBOGridAndEditorControl.IBOEditorControl"/>
        ///  to provide bo editing behaviour.
        /// </summary>
        public IGridControl GridControl
        {
            get { return _readOnlyGridControl; }
        }

        /// <summary>
        /// The <see cref="IBOGridAndEditorControl.IBOEditorControl"/> that is being used to 
        /// edit the <see cref="IBusinessObject"/>.
        /// </summary>
        public IBOEditorControl IBOEditorControl
        {
            get { return _iboEditorControl; }
        }

        /// <summary>
        /// The <see cref="IButtonGroupControl"/> that is has the individual buttons that
        ///   are shown at the bottom of this control.
        /// </summary>
        public IButtonGroupControl ButtonGroupControl
        {
            get { return _buttonGroupControl; }
        }

        /// <summary>
        /// Method to create a new Business Object that is part of the collection.
        /// </summary>
        IBusinessObject IBOGridAndEditorControl.CurrentBusinessObject
        {
            get { return _readOnlyGridControl.SelectedBusinessObject; }
        }

        public bool SkipSaveOnSelectionChanged { get; set; }
        public IGenericGridFilterControl FilterControl { get; protected set; }

        ///<summary>
        /// Gets and Sets the currently selected business object
        ///</summary>
        public IBusinessObject CurrentBusinessObject
        {
            get { return _readOnlyGridControl.SelectedBusinessObject; }
            set { _readOnlyGridControl.SelectedBusinessObject = value; }
        }

        public void ExecuteOnUIThread(Delegate method)
        {
            method.DynamicInvoke();
        }

        public EventHandler OnAsyncOperationComplete { get; set; }
        public EventHandler OnAsyncOperationStarted { get; set; }
        public EventHandler OnAsyncOperationException { get; set; }

        public void PopulateCollectionAsync(DataRetrieverCollectionDelegate dataRetrieverCallback, Action afterPopulation = null)
        {
            if (this.OnAsyncOperationStarted != null) this.OnAsyncOperationStarted(this, new EventArgs());
            try
            {
                this.BusinessObjectCollection = dataRetrieverCallback();
                if (afterPopulation != null) afterPopulation();
                if (this.OnAsyncOperationComplete != null) this.OnAsyncOperationComplete(this, new EventArgs());
            }
            catch (Exception ex)
            {
                if (this.OnAsyncOperationException != null)
                    this.OnAsyncOperationException(this, new ExceptionEventArgs(ex));
            }
        }

        public void PopulateCollectionAsync<T>(Criteria criteria, IOrderCriteria order = null, Action afterPopulation = null) where T : class, IBusinessObject, new()
        {
            if (this.OnAsyncOperationStarted != null) this.OnAsyncOperationStarted(this, new EventArgs());
            try
            {
                this.BusinessObjectCollection = Broker.GetBusinessObjectCollection<T>(criteria, order);
                if (afterPopulation != null) afterPopulation();
                if (this.OnAsyncOperationComplete != null) this.OnAsyncOperationComplete(this, new EventArgs());
            }
            catch (Exception ex)
            {
                if (this.OnAsyncOperationException != null)
                    this.OnAsyncOperationException(this, new ExceptionEventArgs(ex));
            }
        }

        public void PopulateCollectionAsync<T>(string criteria, string order = null, Action afterPopulation = null) where T : class, IBusinessObject, new()
        {
            try
            {
                if (this.OnAsyncOperationStarted != null) this.OnAsyncOperationStarted(this, new EventArgs());
                this.BusinessObjectCollection = Broker.GetBusinessObjectCollection<T>(criteria, order);
                if (afterPopulation != null) afterPopulation();
                if (this.OnAsyncOperationComplete != null) this.OnAsyncOperationComplete(this, new EventArgs());
            }
            catch (Exception ex)
            {
                if (this.OnAsyncOperationException != null)
                    this.OnAsyncOperationException(this, new ExceptionEventArgs(ex));
            }
        }
    }

    /// <summary>
    /// Provides a generic control with a grid on the left, a BO control on the right and buttons at the bottom right.
    /// This is used by a number of screens in Firestarter, but differs from typical Habanero controls in the fact that
    /// there is no Save button - all changes go to the InMemory database.
    /// TODO: This uses ReadOnlyGridControl due to some flaw in ReadOnlyGrid. Look at switching back
    /// to the grid in the future.  What happens when you double-click?
    /// 
    /// TODO:
    /// - grid caret moves all over, even though selected row is correct
    /// </summary>
    public class BOGridAndEditorControlVWG<TBusinessObject> : UserControlVWG, IBOGridAndEditorControl
        where TBusinessObject : class, IBusinessObject
    {
        private IControlFactory _controlFactory;
        private IReadOnlyGridControl _readOnlyGridControl;
        private IButton _newButton;
        private IButton _deleteButton;
        private IButton _cancelButton;
        private IButton _saveButton;
        private TBusinessObject _lastSelectedBusinessObject;
        /// <summary>
        /// Event for when the Business object is selected
        /// </summary>
        public event EventHandler<BOEventArgs<TBusinessObject>> BusinessObjectSelected;

        ///<summary>
        /// Constructor for <see cref="BOGridAndEditorControlVWG"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="uiDefName"></param>
        public BOGridAndEditorControlVWG(IControlFactory controlFactory, string uiDefName)
        {
            BOEditorControlVWG<TBusinessObject> boEditorControl = new BOEditorControlVWG<TBusinessObject>(controlFactory, uiDefName);
            SetupGridAndBOEditorControlVWG(controlFactory, boEditorControl, uiDefName);
        }

        ///<summary>
        ///  Constructor for <see cref="BOGridAndEditorControlVWG"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="iboEditorControl"></param>
        public BOGridAndEditorControlVWG(IControlFactory controlFactory, IBOEditorControl iboEditorControl)
            : this(controlFactory, iboEditorControl, "default")
        {
        }

        //        ///<summary>
        //        ///  Constructor for <see cref="BOGridAndEditorControlVWG"/>
        //        ///</summary>
        //        ///<param name="controlFactory"></param>
        //        ///<param name="businessObjectControl"></param>
        //        ///<param name="businessObject"></param>
        //        public BOGridAndEditorControlVWG(IControlFactory controlFactory, IBOEditorControl businessObjectControl, IBusinessObject businessObject)
        //            :this(controlFactory,businessObjectControl,"default")
        //        {
        //            
        //        }

        ///<summary>
        ///  Constructor for <see cref="BOGridAndEditorControlVWG"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="iboEditorControl"></param>
        ///<param name="gridUiDefName"></param>
        public BOGridAndEditorControlVWG(IControlFactory controlFactory, IBOEditorControl iboEditorControl, string gridUiDefName)
        {
            SetupGridAndBOEditorControlVWG(controlFactory, iboEditorControl, gridUiDefName);
        }

        private void SetupGridAndBOEditorControlVWG(IControlFactory controlFactory, IBOEditorControl iboEditorControl, string gridUiDefName)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            if (iboEditorControl == null) throw new ArgumentNullException("iboEditorControl");

            _controlFactory = controlFactory;
            this.IBOEditorControl = iboEditorControl;

            SetupReadOnlyGridControl(gridUiDefName);
            SetupButtonGroupControl();
            UpdateControlEnabledState();

            this.FilterControl = controlFactory.CreateGenericGridFilter(_readOnlyGridControl.Grid);
            BorderLayoutManager layoutManager = _controlFactory.CreateBorderLayoutManager(this);
            layoutManager.AddControl(this.FilterControl, BorderLayoutManager.Position.North);
            layoutManager.AddControl(_readOnlyGridControl, BorderLayoutManager.Position.West);
            layoutManager.AddControl(IBOEditorControl, BorderLayoutManager.Position.Centre);
            layoutManager.AddControl(ButtonGroupControl, BorderLayoutManager.Position.South);

            _readOnlyGridControl.Grid.BusinessObjectSelected +=
                ((sender, e) => FireBusinessObjectSelected(e.BusinessObject));
        }


        private void SetupButtonGroupControl()
        {
            ButtonGroupControl = _controlFactory.CreateButtonGroupControl();
            _cancelButton = ButtonGroupControl.AddButton("Cancel", CancelButtonClicked);
            _saveButton = ButtonGroupControl.AddButton("Save", SaveClickHandler);
            _deleteButton = ButtonGroupControl.AddButton("Delete", DeleteButtonClicked);
            _newButton = ButtonGroupControl.AddButton("New", NewButtonClicked);
            _cancelButton.Enabled = false;
            _deleteButton.Enabled = false;
            _newButton.Enabled = false;
        }

        private void SaveClickHandler(object sender, EventArgs e)
        {
            IBusinessObject currentBO = CurrentBusinessObject;
            if (currentBO != null) currentBO.Save();
        }

        ///<summary>
        ///</summary>
        ///<param name="grid"></param>
        ///<returns></returns>
        public static int GetGridWidthToFitColumns(IGridBase grid)
        {
            int width = 0;
            if (grid.RowHeadersVisible)
            {
                width = grid.RowHeadersWidth;
            }
            foreach (IDataGridViewColumn column in grid.Columns)
            {
                if (column.Visible) width += column.Width;
            }
            return width;
        }

        private void SetupReadOnlyGridControl(string gridUiDefName)
        {
            _readOnlyGridControl = _controlFactory.CreateReadOnlyGridControl();
            _readOnlyGridControl.Width = 300;
            _readOnlyGridControl.Grid.RowHeadersWidth = 25;
            _readOnlyGridControl.Buttons.Visible = false;
            _readOnlyGridControl.FilterControl.Visible = false;
            IClassDef classDef = ClassDef.Get<TBusinessObject>();
            if (!string.IsNullOrEmpty(gridUiDefName))
            {
                _readOnlyGridControl.Initialise(classDef, gridUiDefName);
                int width = GetGridWidthToFitColumns(_readOnlyGridControl.Grid) + 2;
                if (width < 300)
                {
                    _readOnlyGridControl.Width = width;
                }
            }
            _readOnlyGridControl.Grid.SelectionChanged += GridSelectionChanged;
            _readOnlyGridControl.DoubleClickEditsBusinessObject = false;
        }

        private void FireBusinessObjectSelected(IBusinessObject businessObject)
        {
            if (BusinessObjectSelected != null)
                this.BusinessObjectSelected(this, new BOEventArgs<TBusinessObject>((TBusinessObject)businessObject));
        }

        private void GridSelectionChanged(object sender, EventArgs e)
        {
            if (this.SkipSaveOnSelectionChanged)
            {
                SetSelectedBusinessObject();
                return;
            }
            if (_lastSelectedBusinessObject == null || _readOnlyGridControl.SelectedBusinessObject != _lastSelectedBusinessObject)
            {
                if (!CheckRowSelectionCanChange()) return;
                SavePreviousBusinessObject();
                SetSelectedBusinessObject();
            }
        }

        /// <summary>
        /// Using the RowValidating event did not work as expected, so this method provides
        /// a way to check whether the grid selection should be forced back to the previous selection
        /// </summary>
        private bool CheckRowSelectionCanChange()
        {
            if (_lastSelectedBusinessObject != null && _readOnlyGridControl.SelectedBusinessObject != _lastSelectedBusinessObject)
            {
                if (!_lastSelectedBusinessObject.IsValid())
                {
                    bool hasErrors = this.IBOEditorControl.HasErrors;
                    _readOnlyGridControl.SelectedBusinessObject = _lastSelectedBusinessObject;
                    return false;
                }
            }
            return true;
        }

        private void SetSelectedBusinessObject()
        {
            TBusinessObject businessObject = CurrentBusinessObject;
            this.IBOEditorControl.BusinessObject = businessObject;
            if (businessObject != null)
            {
                businessObject.PropertyUpdated += PropertyUpdated;
            }
            UpdateControlEnabledState();
            _lastSelectedBusinessObject = businessObject;
        }

        private void PropertyUpdated(object sender, BOPropUpdatedEventArgs eventArgs)
        {
            _cancelButton.Enabled = true;
        }

        private void SavePreviousBusinessObject()
        {
            if (_lastSelectedBusinessObject != null && !_lastSelectedBusinessObject.Status.IsDeleted)
            {
                _lastSelectedBusinessObject.Save();
            }
        }

        private void CancelButtonClicked(object sender, EventArgs e)
        {
            IBusinessObject currentBO = CurrentBusinessObject;
            if (currentBO.Status.IsNew)
            {
                //                _readOnlyGridControl.BusinessObjectCollection.RestoreAll();
                _lastSelectedBusinessObject = null;
                _readOnlyGridControl.BusinessObjectCollection.Remove(currentBO);
                SelectLastRowInGrid();
            }
            else
            {
                //                _readOnlyGridControl.BusinessObjectCollection.RestoreAll();
                currentBO.CancelEdits();
            }
            UpdateControlEnabledState();

            RefreshGrid();
        }

        ///<summary>
        /// Refreshes the Grid. I.e. Reloads the collection from the Datastore
        ///</summary>
        public void RefreshGrid()
        {
            RemoveGridSelectionChangedEvent();
            _readOnlyGridControl.Grid.RefreshGrid();
            AddGridSelectionChangedEvent();
        }

        ///<summary>
        /// Adds the <see cref="GridSelectionChanged"/> Event Handler to the <see cref="IDataGridView.SelectionChanged"/> event
        ///</summary>
        internal void AddGridSelectionChangedEvent()
        {
            _readOnlyGridControl.Grid.SelectionChanged += GridSelectionChanged;
        }

        ///<summary>
        /// Removes the <see cref="GridSelectionChanged"/> Event Handler from the <see cref="IDataGridView.SelectionChanged"/> event
        ///</summary>
        public void RemoveGridSelectionChangedEvent()
        {
            _readOnlyGridControl.Grid.SelectionChanged -= GridSelectionChanged;
        }

        private void SelectLastRowInGrid()
        {
            int rowCount = _readOnlyGridControl.Grid.Rows.Count - 1;
            IBusinessObject lastObjectInGrid = _readOnlyGridControl.Grid.GetBusinessObjectAtRow(rowCount);
            _readOnlyGridControl.SelectedBusinessObject = lastObjectInGrid;
        }

        private void UpdateControlEnabledState()
        {
            IBusinessObject selectedBusinessObject = CurrentBusinessObject;
            bool selectedBusinessObjectNotNull = (selectedBusinessObject != null);
            if (selectedBusinessObjectNotNull)
            {
                string message;
                bool isEditable = selectedBusinessObject.IsEditable(out message);
                this.IBOEditorControl.Enabled = isEditable;
                _saveButton.Enabled = isEditable;
                _cancelButton.Enabled = selectedBusinessObject.Status.IsDirty;
                _deleteButton.Enabled = !selectedBusinessObject.Status.IsNew;
            }
            else
            {
                this.IBOEditorControl.Enabled = false;
                _deleteButton.Enabled = false;
                _cancelButton.Enabled = false;
            }
            //this.IBOEditorControl.Enabled = selectedBusinessObjectNotNull;
        }

        /// <summary>
        /// Deliberately adds BO to the grid's collection because current habanero model only
        /// adds BO to grid when it is saved.  This causes a problem when you call col.CreateBO(), since
        /// it adds the BO twice and throws a duplicate key exception.
        /// </summary>a
        private void NewButtonClicked(object sender, EventArgs e)
        {
            IBusinessObject currentBO = CurrentBusinessObject;
            if (currentBO != null)
            {
                if (!currentBO.IsValid())
                {
                    bool hasErrors = this.IBOEditorControl.HasErrors;
                    return;
                }
                currentBO.Save();
            }

            IBusinessObjectCollection collection = _readOnlyGridControl.Grid.BusinessObjectCollection;
            _readOnlyGridControl.Grid.SelectionChanged -= GridSelectionChanged;
            IBusinessObject businessObject = collection.CreateBusinessObject();
            UpdateControlEnabledState();
            collection.Add(businessObject);
// ReSharper disable RedundantCheckBeforeAssignment
            // Peter: the following check is necessary - depending on the previous state of the grid the SelectedBusinessObject will
            // already be the same object as the new one. The check is also necessary because setting the SelectedBusinessObject runs through
            // the code to save the old one, which is incorrect behaviour if they're both the same object.
            if (_readOnlyGridControl.SelectedBusinessObject != businessObject) _readOnlyGridControl.SelectedBusinessObject = businessObject;
// ReSharper restore RedundantCheckBeforeAssignment
            _readOnlyGridControl.Grid.SelectionChanged += GridSelectionChanged;
            GridSelectionChanged(null, null);
            this.IBOEditorControl.Focus();
            //            this.IBOEditorControl.ClearErrors();
            _cancelButton.Enabled = true;
        }

        private void DeleteButtonClicked(object sender, EventArgs e)
        {
            IBusinessObject businessObject = CurrentBusinessObject;
            businessObject.MarkForDelete();
            businessObject.Save();

            if (CurrentBusinessObject == null && _readOnlyGridControl.Grid.Rows.Count > 0)
            {
                SelectLastRowInGrid();
            }
        }

        /// <summary>
        /// Sets the business object collection to populate the grid.  If the grid
        /// needs to be cleared, set an empty collection rather than setting to null.
        /// Until you set a collection, the controls are disabled, since any given
        /// collection needs to be provided by a suitable context.
        /// </summary>
        public IBusinessObjectCollection BusinessObjectCollection
        {
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _readOnlyGridControl.BusinessObjectCollection = value;
                _newButton.Enabled = true;
            }
            get
            {
                return _readOnlyGridControl.BusinessObjectCollection;
            }
        }

        /// <summary>
        /// Returns the <see cref="IGridControl"/> that is being used along side of the <see cref="IBOGridAndEditorControl.IBOEditorControl"/>
        ///  to provide bo editing behaviour.
        /// </summary>
        public IGridControl GridControl
        {
            get { return _readOnlyGridControl; }
        }

        /// <summary>
        /// The <see cref="IBOGridAndEditorControl.IBOEditorControl"/> that is being used to 
        /// edit the <see cref="IBusinessObject"/>.
        /// </summary>
        public IBOEditorControl IBOEditorControl { get; private set; }

        ///<summary>
        /// Returns the <see cref="IButtonGroupControl"/> for the 
        ///</summary>
        public IButtonGroupControl ButtonGroupControl { get; private set; }

        /// <summary>
        /// Method to create a new Business Object that is part of the collection.
        /// </summary>
        IBusinessObject IBOGridAndEditorControl.CurrentBusinessObject
        {
            get { return _readOnlyGridControl.SelectedBusinessObject; }
        }

        public bool SkipSaveOnSelectionChanged { get; set; }
        public IGenericGridFilterControl FilterControl { get; private set; }

        ///<summary>
        /// The Current Business Object that is selected in the grid.
        ///</summary>
        public TBusinessObject CurrentBusinessObject
        {
            get { return (TBusinessObject)_readOnlyGridControl.SelectedBusinessObject; }
            set { _readOnlyGridControl.SelectedBusinessObject = value; }
        }

        public void PopulateCollectionAsync(DataRetrieverCollectionDelegate dataRetrieverCallback, Action afterPopulation = null)
        {
            try
            {
                if (this.OnAsyncOperationStarted != null) this.OnAsyncOperationStarted(this, new EventArgs());
                this.BusinessObjectCollection = dataRetrieverCallback();
                if (afterPopulation != null) afterPopulation();
                if (this.OnAsyncOperationComplete != null) this.OnAsyncOperationComplete(this, new EventArgs());
            }
            catch (Exception e)
            {
                if (this.OnAsyncOperationException != null)
                    this.OnAsyncOperationException(this, new ExceptionEventArgs(e));
            }
        }

        public void PopulateCollectionAsync<T>(Criteria criteria, IOrderCriteria order = null, Action afterPopulation = null) where T : class, IBusinessObject, new()
        {
            try
            {
                if (this.OnAsyncOperationStarted != null) this.OnAsyncOperationStarted(this, new EventArgs());
                this.BusinessObjectCollection = Broker.GetBusinessObjectCollection<T>(criteria, order);
                if (afterPopulation != null) afterPopulation();
                if (this.OnAsyncOperationComplete != null) this.OnAsyncOperationComplete(this, new EventArgs());
            }
            catch (Exception e)
            {
                if (this.OnAsyncOperationException != null)
                    this.OnAsyncOperationException(this, new ExceptionEventArgs(e));
            }
        }

        public void PopulateCollectionAsync<T>(string criteria, string order = null, Action afterPopulation = null) where T : class, IBusinessObject, new()
        {
            try
            {
                if (this.OnAsyncOperationStarted != null) this.OnAsyncOperationStarted(this, new EventArgs());
                this.BusinessObjectCollection = Broker.GetBusinessObjectCollection<T>(criteria, order);
                if (afterPopulation != null) afterPopulation();
                if (this.OnAsyncOperationComplete != null) this.OnAsyncOperationComplete(this, new EventArgs());
            }
            catch (Exception e)
            {
                if (this.OnAsyncOperationException != null)
                    this.OnAsyncOperationException(this, new ExceptionEventArgs(e));
            }
        }

        public void ExecuteOnUIThread(Delegate method)
        {
            method.DynamicInvoke();
        }

        public EventHandler OnAsyncOperationComplete { get; set; }
        public EventHandler OnAsyncOperationStarted { get; set; }
        public EventHandler OnAsyncOperationException { get; set; }
    }
}
