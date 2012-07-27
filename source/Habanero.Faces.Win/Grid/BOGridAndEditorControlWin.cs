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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using System.Linq;
using Habanero.Faces.Base.Async;
using Habanero.Faces.Win.Async;

namespace Habanero.Faces.Win
{

    ///<summary>
    /// Control For Win that displays a collection of a Business Object along side an editor/creator panel.
    /// The collection of business objects can be shown using any selector control e.g. an <see cref="IEditableGridControl"/>,
    ///   <see cref="IGridControl"/> etc.
    ///</summary>
    public class BOGridAndEditorControlWin : UserControlWin, IBOGridAndEditorControl
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
        private Timer _filterTimer;
        private bool _filterRequired;
        private bool _inFilter;
        private DateTime _lastFilterChanged;
        private ITextBox _filterTextBox;
        private bool _inAsyncOperation;
        private ILabel _filterMessageLabel;
        /// <summary>
        /// Event that is raised when a business objects is selected.
        /// </summary>
        public event EventHandler<BOEventArgs<IBusinessObject>> BusinessObjectSelected;

        ///<summary>
        /// Constructor for the <see cref="BOGridAndEditorControlWin"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="classDef"></param>
        ///<param name="uiDefName"></param>
        ///<exception cref="ArgumentNullException"></exception>
        public BOGridAndEditorControlWin(IControlFactory controlFactory, IClassDef classDef, string uiDefName)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            if (classDef == null) throw new ArgumentNullException("classDef");
            _classDef = classDef;
            BOEditorControlWin boEditorControlWin = new BOEditorControlWin(controlFactory, classDef, uiDefName);
            SetupGridAndBOEditorControlWin(controlFactory, boEditorControlWin, classDef, uiDefName);
            this._filterTimer = new Timer() { Enabled = true, Interval = 500 };
            this._filterTimer.Tick += (sender, e) => 
            {
                if ((!this._inFilter) && (this._filterRequired) && (this._lastFilterChanged.AddMilliseconds(this._filterTimer.Interval) < DateTime.Now))
                {
                    this._filterRequired = false;
                    this.DoFilter();
                }
            };

            this.OnAsyncOperationComplete += (sender, e) =>
                {
                    lock (this)
                    {
                        this._inAsyncOperation = false;
                    }
                    this.Enabled = true;
                    this.UseWaitCursor = false;
                    this.Cursor = Cursors.Default;
                };
            this.OnAsyncOperationStarted += (sender, e) =>
                {
                    lock (this)
                    {
                        if (this._inAsyncOperation) throw new Exception("Already performing an asynchronous operation");
                        this._inAsyncOperation = true;
                    }
                    this.Enabled = false;
                    this.UseWaitCursor = true;
                    this.Cursor = Cursors.WaitCursor;
                };
        }

        ///<summary>
        /// Constructor for the <see cref="BOGridAndEditorControlWin"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="iboEditorControl"></param>
        public BOGridAndEditorControlWin(IControlFactory controlFactory, IBOEditorControl iboEditorControl)
            : this(controlFactory, iboEditorControl, null, "default")
        {
        }

        ///<summary>
        /// Constructor for the <see cref="BOGridAndEditorControlWin"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="iboEditorControl"></param>
        ///<param name="classDef"></param>
        ///<param name="gridUiDefName"></param>
        public BOGridAndEditorControlWin(IControlFactory controlFactory, IBOEditorControl iboEditorControl, IClassDef classDef,string gridUiDefName)
        {
            SetupGridAndBOEditorControlWin(controlFactory, iboEditorControl,classDef ,gridUiDefName);
        }

        private void SetupGridAndBOEditorControlWin(IControlFactory controlFactory, IBOEditorControl iboEditorControl, IClassDef classDef,string gridUiDefName)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            if (iboEditorControl == null) throw new ArgumentNullException("iboEditorControl");
            _controlFactory = controlFactory;
            _iboEditorControl = iboEditorControl;

            SetupReadOnlyGridControl(classDef ,gridUiDefName);
            SetupButtonGroupControl();
            UpdateControlEnabledState();

            this.RemoveObjectIDColumn();
            this._filterTextBox = _controlFactory.CreateTextBox();
            var txtBox = this._filterTextBox as TextBox;
            if (txtBox != null)
            {
                txtBox.KeyPress += (sender, e) =>
                    {
                        if (e.KeyChar == (char)27)
                            this.ClearFilter();
                        else
                        {
                            this._lastFilterChanged = DateTime.Now;
                            this._filterRequired = true;
                        }
                    };
            }

            var filterLabel = _controlFactory.CreateLabel("Filter:");
            this._filterMessageLabel = _controlFactory.CreateLabel("");
            this._filterMessageLabel.Font = new Font(this._filterMessageLabel.Font, FontStyle.Italic);
            var grid = new GridLayoutManager(this, _controlFactory);
            grid.SetGridSize(3, 4);
            grid.FixRow(2, _buttonGroupControl.Height);
            grid.FixRow(0, this._filterTextBox.Height + 5);
            grid.FixColumn(0, filterLabel.Width);

            var leftMost = this.Width;
            var rightMost = 0;
            var bottomMost = 0;
            foreach (var control in this._iboEditorControl.Controls)
            {
                var ctl = control as Control;
                if (ctl == null) continue;
                if (ctl.Left < leftMost) leftMost = ctl.Left;
                if (ctl.Right > rightMost) rightMost = ctl.Right;
                if (ctl.Bottom > bottomMost) bottomMost = ctl.Bottom;
            }
            var editorWidth = rightMost - leftMost;
            grid.FixColumn(3, editorWidth);

            grid.AddControl(filterLabel);
            grid.AddControl(this._filterTextBox, 1, 2);
            grid.AddControl(this._filterMessageLabel);
            grid.AddControl(_readOnlyGridControl, 1, 3);
            grid.AddControl(_iboEditorControl);
            grid.AddControl(_buttonGroupControl, 1, 4);

            this.MinimumSize = new Size(editorWidth, bottomMost);

            _readOnlyGridControl.BusinessObjectSelected +=
                ((sender, e) => FireBusinessObjectSelected(e.BusinessObject));

            _readOnlyGridControl.Grid.SelectionChanged += GridSelectionChanged;
        }

        private void DoFilter()
        {
            this._inFilter = true;
            var filter = this._filterTextBox.Text.Trim().ToLower();
            if (String.IsNullOrEmpty(filter))
            {
                this.ClearFilter();
                this._inFilter = false;
                return;
            }
            var dataGrid = _readOnlyGridControl.Grid as DataGridView;
            if (dataGrid == null)
            {
                this._inFilter = false;
                return;
            }
            this.SetFilteringUIState();

            dataGrid.CurrentCell = null;
            var parts = filter.Split(new char[] { ' ' });
            DataGridViewRow firstVisibleRow = null;
            foreach (DataGridViewRow r in dataGrid.Rows)
            {
                var hits = 0;
                foreach (DataGridViewCell c in r.Cells)
                {
                    var hit = false;
                    foreach (var part in parts)
                    {
                        if (c.Value.ToString().ToLower().Contains(part))
                        {
                            hits++;
                            hit = true;
                            break;
                        }
                    }
                    if (hit)
                        break;
                }
                r.Visible = (hits == parts.Length);
                if (firstVisibleRow == null)
                    firstVisibleRow = r;
            }
            if (firstVisibleRow != null)
                firstVisibleRow.Selected = true;
            this._inFilter = false;
            this.SetFilteringUIState();
        }

        private void SetFilteringUIState()
        {
            this._filterMessageLabel.Text = (this._inFilter) ? "Filtering... please wait..." : "";
            this._filterTextBox.Enabled = !this._inFilter;
            this._iboEditorControl.Enabled = !this._inFilter;
            this._readOnlyGridControl.Enabled = !this._inFilter;
            this._buttonGroupControl.Enabled = !this._inFilter;
            this.UseWaitCursor = this._inFilter;
            Application.DoEvents();
        }

        private void ClearFilter()
        {
            var dataGrid = _readOnlyGridControl.Grid as DataGridView;
            if (dataGrid == null)
                return;
            foreach (DataGridViewRow r in dataGrid.Rows)
                r.Visible = true;
        }

        private void RemoveObjectIDColumn()
        {
            var gridView = this._readOnlyGridControl.Grid as DataGridView;
            if (gridView != null)
            {
                var toRemove = new List<DataGridViewColumn>();
                foreach (DataGridViewColumn col in gridView.Columns)
                {
                    if (col.Name == "HABANERO_OBJECTID")
                    {
                        toRemove.Add(col);
                    }
                }
                foreach (var col in toRemove)
                    gridView.Columns.Remove(col);
            }
        }

        private void SetupButtonGroupControl()
        {
            _buttonGroupControl = _controlFactory.CreateButtonGroupControl();
            _cancelButton = _buttonGroupControl.AddButton("Cancel", CancelButtonClicked);
            _saveButton = _buttonGroupControl.AddButton("Save", SaveClickHandler);
            _deleteButton = _buttonGroupControl.AddButton("Delete", DeleteButtonClicked);
            _newButton = _buttonGroupControl.AddButton("New", NewButtonClicked);
            _saveButton.Enabled = false;
            _cancelButton.Enabled = false;
            _deleteButton.Enabled = false;
            _newButton.Enabled = false;
        }

        private void SaveClickHandler(object sender, EventArgs e)
        {
            try
            {
                IBusinessObject currentBO = CurrentBusinessObject;
                if (currentBO!=null) currentBO.Save();
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
            try
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
            catch (Exception) { }
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
                _readOnlyGridControl.BusinessObjectCollection =value;
                _newButton.Enabled = true;
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

        ///<summary>
        /// Gets and Sets the currently selected business object
        ///</summary>
        public IBusinessObject CurrentBusinessObject
        {
            get { return _readOnlyGridControl.SelectedBusinessObject; }
            set { _readOnlyGridControl.SelectedBusinessObject = value; }
        }

        public EventHandler OnAsyncOperationComplete { get; set; }
        public EventHandler OnAsyncOperationStarted { get; set; }
        IBusinessObjectCollection IHasBusinessObjectCollection.BusinessObjectCollection { get; set; }
        public void PopulateCollectionAsync(DataRetrieverCollectionDelegate dataRetrieverCallback, Action afterPopulation = null)
        {
            if (this.OnAsyncOperationStarted != null) this.OnAsyncOperationStarted(this, new EventArgs());
            var data = new ConcurrentDictionary<string, object>();
            data["collection"] = null;
            BackgroundWorkerWin.Run(this, data,
                (d) => { 
                    d["collection"] = dataRetrieverCallback(); 
                    return true; 
                },
                (d) =>
                {
                    this.PopulateCollectionUICallback(afterPopulation, d);
                },
                null,
                this.NotifyPopulationException);
            if (this.OnAsyncOperationComplete != null) this.OnAsyncOperationComplete(this, new EventArgs());
        }

        public void PopulateCollectionAsync<T>(Criteria criteria, IOrderCriteria order = null, Action afterPopulation = null) where T : class, IBusinessObject, new()
        {
            if (this.OnAsyncOperationStarted != null) this.OnAsyncOperationStarted(this, new EventArgs());
            var data = new ConcurrentDictionary<string, object>();
            data["collection"] = null;
            BackgroundWorkerWin.Run(this, data,
                (d) => { d["collection"] = Broker.GetBusinessObjectCollection<T>(criteria, order); return true; },
                (d) =>
                {
                    this.PopulateCollectionUICallback(afterPopulation, d);
                },
                null,
                this.NotifyPopulationException);
            if (this.OnAsyncOperationComplete != null) this.OnAsyncOperationComplete(this, new EventArgs());
        }

        private void PopulateCollectionUICallback(Action afterPopulation, ConcurrentDictionary<string, object> d)
        {
            this.BusinessObjectCollection = d["collection"] as IBusinessObjectCollection;
            if (afterPopulation != null) afterPopulation();
            if (this.OnAsyncOperationComplete != null) this.OnAsyncOperationComplete(this, new EventArgs());
            this.UseWaitCursor = false;
            this.Enabled = true;
        }

        private void NotifyPopulationException(Exception ex)
        {
            GlobalRegistry.UIExceptionNotifier.Notify(ex, "Unable to load data grid", "Error loading data grid");
        }

        public void PopulateCollectionAsync<T>(string criteria, string order = null, Action afterPopulation = null) where T : class, IBusinessObject, new()
        {
            this.PopulateCollectionAsync<T>(CriteriaParser.CreateCriteria(criteria), OrderCriteria.FromString(order), afterPopulation);
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
    public class BOGridAndEditorControlWin<TBusinessObject> : UserControlWin, IBOGridAndEditorControl
        where TBusinessObject : class, IBusinessObject
    {
        private IControlFactory _controlFactory;
        private IReadOnlyGridControl _readOnlyGridControl;
        private IButtonGroupControl _buttonGroupControl;
        private IButton _newButton;
        private IButton _deleteButton;
        private IButton _cancelButton;
        private IButton _saveButton;
        private TBusinessObject _lastSelectedBusinessObject;
        private bool _inAsyncOperation;
        /// <summary>
        /// Event for when the Business object is selected
        /// </summary>
        public event EventHandler<BOEventArgs<TBusinessObject>> BusinessObjectSelected;

        ///<summary>
        /// Constructor for <see cref="BOGridAndEditorControlWin"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="uiDefName"></param>
        public BOGridAndEditorControlWin(IControlFactory controlFactory, string uiDefName)
        {
            BOEditorControlWin<TBusinessObject> boEditorControl = new BOEditorControlWin<TBusinessObject>(controlFactory, uiDefName);
            SetupGridAndBOEditorControlWin(controlFactory, boEditorControl, uiDefName);
            this.OnAsyncOperationComplete += (sender, e) =>
                {
                    lock (this)
                    {
                        this._inAsyncOperation = false;
                    }
                    this.Enabled = true;
                    this.UseWaitCursor = false;
                    this.Cursor = Cursors.Default;
                    this.AddGridSelectionChangedEvent();
                };
            this.OnAsyncOperationStarted += (sender, e) =>
                {
                    this.RemoveGridSelectionChangedEvent();
                    lock (this)
                    {
                        this._inAsyncOperation = true;
                    }
                    this.Enabled = false;
                    this.UseWaitCursor = true;
                    this.Cursor = Cursors.WaitCursor;
                };
        }

        ///<summary>
        ///  Constructor for <see cref="BOGridAndEditorControlWin"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="iboEditorControl"></param>
        public BOGridAndEditorControlWin(IControlFactory controlFactory, IBOEditorControl iboEditorControl)
            : this(controlFactory, iboEditorControl, "default")
        {
        }

//        ///<summary>
//        ///  Constructor for <see cref="BOGridAndEditorControlWin"/>
//        ///</summary>
//        ///<param name="controlFactory"></param>
//        ///<param name="businessObjectControl"></param>
//        ///<param name="businessObject"></param>
//        public BOGridAndEditorControlWin(IControlFactory controlFactory, IBOEditorControl businessObjectControl, IBusinessObject businessObject)
//            :this(controlFactory,businessObjectControl,"default")
//        {
//            
//        }

        ///<summary>
        ///  Constructor for <see cref="BOGridAndEditorControlWin"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="iboEditorControl"></param>
        ///<param name="gridUiDefName"></param>
        public BOGridAndEditorControlWin(IControlFactory controlFactory, IBOEditorControl iboEditorControl, string gridUiDefName)
        {
            SetupGridAndBOEditorControlWin(controlFactory, iboEditorControl, gridUiDefName);
        }

        private void SetupGridAndBOEditorControlWin(IControlFactory controlFactory, IBOEditorControl iboEditorControl, string gridUiDefName)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            if (iboEditorControl == null) throw new ArgumentNullException("iboEditorControl");

            _controlFactory = controlFactory;
            this.IBOEditorControl = iboEditorControl;

            SetupReadOnlyGridControl(gridUiDefName);
            SetupButtonGroupControl();
            UpdateControlEnabledState();

            BorderLayoutManager layoutManager = _controlFactory.CreateBorderLayoutManager(this);
            layoutManager.AddControl(_readOnlyGridControl, BorderLayoutManager.Position.West);
            layoutManager.AddControl(IBOEditorControl, BorderLayoutManager.Position.Centre);
            layoutManager.AddControl(_buttonGroupControl, BorderLayoutManager.Position.South);

            _readOnlyGridControl.Grid.BusinessObjectSelected +=
                ((sender, e) => FireBusinessObjectSelected(e.BusinessObject));
        }


        private void SetupButtonGroupControl()
        {
            _buttonGroupControl = _controlFactory.CreateButtonGroupControl();
            _cancelButton = _buttonGroupControl.AddButton("Cancel", CancelButtonClicked);
            _saveButton = _buttonGroupControl.AddButton("Save", SaveClickHandler);
            _deleteButton = _buttonGroupControl.AddButton("Delete", DeleteButtonClicked);
            _newButton = _buttonGroupControl.AddButton("New", NewButtonClicked);
            _saveButton.Enabled = false;
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
            _readOnlyGridControl.SelectedBusinessObject = businessObject;
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

        ///<summary>
        /// The Current Business Object that is selected in the grid.
        ///</summary>
        public TBusinessObject CurrentBusinessObject
        {
            get { return (TBusinessObject)_readOnlyGridControl.SelectedBusinessObject; }
            set { _readOnlyGridControl.SelectedBusinessObject = value; }
        }

        public EventHandler OnAsyncOperationComplete { get; set; }
        public EventHandler OnAsyncOperationStarted { get; set; }
        IBusinessObjectCollection IHasBusinessObjectCollection.BusinessObjectCollection { get; set; }
        public void PopulateCollectionAsync(DataRetrieverCollectionDelegate dataRetrieverCallback, Action afterPopulation = null)
        {
            if (this.OnAsyncOperationStarted != null) this.OnAsyncOperationStarted(this, new EventArgs());
            var data = new ConcurrentDictionary<string, object>();
            data["collection"] = null;
            BackgroundWorkerWin.Run(this, data,
                (d) => { d["collection"] = dataRetrieverCallback(); return true; },
                (d) =>
                {
                    this.PopulateCollectionUICallback(afterPopulation, d);
                },
                null,
                this.NotifyPopulationException);
            if (this.OnAsyncOperationComplete != null) this.OnAsyncOperationComplete(this, new EventArgs());
        }

        public void PopulateCollectionAsync<T>(Criteria criteria, IOrderCriteria order = null, Action afterPopulation = null) where T : class, IBusinessObject, new()
        {
            if (this.OnAsyncOperationStarted != null) this.OnAsyncOperationStarted(this, new EventArgs());
            var data = new ConcurrentDictionary<string, object>();
            data["collection"] = null;
            BackgroundWorkerWin.Run(this, data,
                (d) => { d["collection"] = Broker.GetBusinessObjectCollection<T>(criteria, order); return true; },
                (d) =>
                {
                    this.PopulateCollectionUICallback(afterPopulation, d);
                },
                null,
                this.NotifyPopulationException);
            if (this.OnAsyncOperationComplete != null) this.OnAsyncOperationComplete(this, new EventArgs());
        }

        private void PopulateCollectionUICallback(Action afterPopulation, ConcurrentDictionary<string, object> d)
        {
            var boCollection = d["collection"] as IBusinessObjectCollection;
            this.BusinessObjectCollection = boCollection;
            if (afterPopulation != null) afterPopulation();
            if (this.OnAsyncOperationComplete != null) this.OnAsyncOperationComplete(this, new EventArgs());
            this.UseWaitCursor = false;
            this.Enabled = true;
        }

        private void NotifyPopulationException(Exception ex)
        {
            GlobalRegistry.UIExceptionNotifier.Notify(ex, "Unable to load data grid", "Error loading data grid");
        }

        public void PopulateCollectionAsync<T>(string criteria, string order = null, Action afterPopulation = null) where T : class, IBusinessObject, new()
        {
            this.PopulateCollectionAsync<T>(CriteriaParser.CreateCriteria(criteria), OrderCriteria.FromString(order), afterPopulation);
        }
    }
}