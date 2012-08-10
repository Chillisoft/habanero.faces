using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Habanero.Faces.Base;
using Habanero.Faces.Base.ControlInterfaces;
using Habanero.Faces.Win;
using Timer = System.Windows.Forms.Timer;
using System.Linq;

namespace Habanero.Faces.Win
{
    public class GenericGridFilterControlWin: PanelWin, IGenericGridFilterControl
    {

        public EventHandler FilterStarted { get; set; }
        public EventHandler FilterCompleted { get; set; }

        public IGridBase Grid { get; set; }
        public string FilterText
        {
            get { return this._filterTextBox.Text; }
            set { this._filterTextBox.Text = value ?? ""; }
        }
        private ITextBox _filterTextBox;
        private ILabel _filterLabel;
        private Timer _timer;
        private bool _inFilter;
        private bool _cancelCurrentFilter;
        private bool _filterRequired;
        private DateTime _lastFilterChanged;
        private DataGridViewCellStyle _gridOriginalAlternatingStyle;
        private DateTime _lastForcedEvents;
        private DataView _originalView;
        private string _lastFilterText;

        private void DataSourceChanged(object sender, EventArgs e)
        {
            this._originalView = null;
            var _ds = this.Grid.DataSource as DataView;
            if (_ds != null)
                this._originalView = _ds;
        }

        public GenericGridFilterControlWin(IGridBase grid)
        {
            var ds = grid.DataSource as DataView;
            if (ds != null) this._originalView = ds;
            (grid as DataGridView).DataSourceChanged += this.DataSourceChanged;

            this._lastForcedEvents = DateTime.Now;
            this.Grid = grid;
            this._timer = new Timer()
            {
                Enabled = true,
                Interval = 500,
            };
            this._timer.Tick += (sender, e) => 
            {
                if ((this._filterRequired) && (this._lastFilterChanged.AddMilliseconds(this._timer.Interval) < DateTime.Now))
                {
                    if (this._inFilter)
                        this._cancelCurrentFilter = true;
                    this._filterRequired = false;
                    this.DoFilter();
                }
            };

            var factory = new ControlFactoryWin();
            this._filterLabel = factory.CreateLabel("Filter:");
            this._filterTextBox = factory.CreateTextBox();
            var txt = this._filterTextBox as TextBox;
            txt.TextChanged += (sender, e) =>
                {
                    if (txt.Text == this._lastFilterText) return;
                    this._lastFilterChanged = DateTime.Now;
                    this._filterRequired = true;
                };
            var manager = factory.CreateBorderLayoutManager(this);
            manager.AddControl(this._filterLabel, BorderLayoutManager.Position.West);
            manager.AddControl(this._filterTextBox, BorderLayoutManager.Position.Centre);
            var vgap = manager.VerticalGapSize + manager.BorderSize;
            this.Height = this._filterTextBox.Height + 2 * vgap;

            this.FilterStarted += (sender, e) =>
                {
                    this.SetUIState(true);
                };
            this.FilterCompleted += (sender, e) =>
                {
                    this.SetUIState(false);
                };
            var wingrid = Grid as DataGridView;
            if (wingrid != null)
            {
                this._gridOriginalAlternatingStyle = wingrid.AlternatingRowsDefaultCellStyle;
                wingrid.AlternatingRowsDefaultCellStyleChanged += this.RecordGridAltStyle;
            }
        }

        private void RecordGridAltStyle(object sender, EventArgs e)
        {
            var wingrid = this.Grid as DataGridView;
            if (wingrid != null)
                this._gridOriginalAlternatingStyle = wingrid.AlternatingRowsDefaultCellStyle;
        }

        private void SetUIState(bool filtering)
        {
            //this.Enabled = !filtering;
            this.Grid.Enabled = !filtering;
            this.UseWaitCursor = filtering;
            this.Cursor = filtering ? Cursors.WaitCursor : Cursors.Default;
            var grid = this.Grid as DataGridView;
            if (grid != null)
            {
                grid.UseWaitCursor = filtering;
                grid.Cursor = this.Cursor;
            }
            Application.DoEvents();
            this._filterTextBox.Focus();
        }

        private void ClearFilter()
        {
            if (this.Grid == null)
                return;
            if (this.FilterStarted != null)
                this.FilterStarted(this, new EventArgs());
            var wingrid = this.Grid as DataGridView;
            if (wingrid != null)
            {
                wingrid.DataSource = this._originalView;
                this.SetAlternatingStyle(wingrid, this._gridOriginalAlternatingStyle);
            }
            if (this.FilterCompleted != null)
                this.FilterCompleted(this, new EventArgs());
        }
        private void DoFilter()
        {
            lock (this)
            {
                this._inFilter = true;
                this._cancelCurrentFilter = false;
                var filter = this._filterTextBox.Text.Trim().ToLower();
                if (filter == this._lastFilterText) return;
                this._lastFilterText = filter;
                if (String.IsNullOrEmpty(filter))
                {
                    this.ClearFilter();
                    this._inFilter = false;
                    return;
                }
                if (this.Grid == null)
                {
                    this._inFilter = false;
                    return;
                }
                if (this.FilterStarted != null)
                    this.FilterStarted(this, new EventArgs());
                FilterGrid(filter);
                this._inFilter = false;
                if (this.FilterCompleted != null)
                    this.FilterCompleted(this, new EventArgs());
            }
        }

        private void FilterGrid(string filter)
        {
            var wingrid = this.Grid as DataGridView;
            if (wingrid == null) return;
            if (wingrid.Rows.Count == 0) return;
            wingrid.CurrentCell = null;
            var parts = filter.Split(new char[] { ' ' });

            this.SetAlternatingStyle(wingrid, null);

            var somethingHidden = false;
            var somethingVisible = false;
            var searchColumns = GetSearchColumnIndexes(wingrid);
            var searchCells = new List<string>();
            var subTable = this._originalView.Table.Clone();
            foreach (DataRow row in this._originalView.Table.Rows)
            {
                this.DoEvents();
                if (this._cancelCurrentFilter)
                    break;
                var hits = 0;
                foreach (var part in parts)
                {
                    searchCells.Clear();
                    foreach (var i in searchColumns)
                    {
                        var cell = row[i];
                        var v = cell as String;
                        if (v == null || v.Length == 0) continue;
                        searchCells.Add(v);
                    }
                    var searchRow = String.Join("\n", searchCells);
                    if (searchRow.ToLower().Contains(part))
                        hits++;
                }
                if (hits == parts.Length)
                {
                    subTable.ImportRow(row);
                    somethingVisible = true;
                }
                else
                    somethingHidden = true;
            }
            this.SetNewDataSource(subTable, wingrid);
            this.SetupFilteredAlternatingRowColors(wingrid, somethingVisible, somethingHidden);
        }

        private void SetNewDataSource(DataTable subTable, DataGridView wingrid)
        {
            var vw = new DataView(subTable);
            wingrid.DataSourceChanged -= this.DataSourceChanged;
            wingrid.DataSource = vw;
            var newCols = new List<DataGridViewColumn>();
            var visibility = new List<bool>();
            foreach (DataGridViewColumn col in wingrid.Columns)
            {
                var newCol = (Activator.CreateInstance(col.GetType())) as DataGridViewColumn;
                newCol.DataPropertyName = col.DataPropertyName;
                newCol.HeaderText = col.HeaderText;
                newCol.Visible = col.Visible;
                visibility.Add(col.Visible);
                newCols.Add(newCol);
            }
            wingrid.Columns.Clear();
            wingrid.Columns.AddRange(newCols.ToArray());
            //for (var i = 0; i < visibility.Count; i++)
            //    wingrid.Columns[0].Visible = visibility[i];
            wingrid.DataSourceChanged += this.DataSourceChanged;
        }

        private List<int> GetSearchColumnIndexes(DataGridView dataGrid)
        {
            var ret = new List<int>();
            for (var i = 0; i < dataGrid.Columns.Count; i++)
            {
                if (dataGrid.Columns[i].Visible)
                    ret.Add(i);
            }
            return ret;
        }

        private void SetupFilteredAlternatingRowColors(DataGridView wingrid, bool somethingVisible, bool somethingHidden)
        {
            if (wingrid != null)
            {
                if (somethingHidden && somethingVisible && this._gridOriginalAlternatingStyle != null)
                {
                    bool defaultStyle = true;
                    for (var i = 0; i < wingrid.Rows.Count; i++)
                    {
                        //this.DoEvents();
                        if (this._cancelCurrentFilter)
                            break;
                        var row = wingrid.Rows[i];
                        if (!row.Visible) continue;
                        var currentStyle = (defaultStyle) ? wingrid.DefaultCellStyle : this._gridOriginalAlternatingStyle;
                        defaultStyle = !defaultStyle;
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = currentStyle.BackColor;
                            cell.Style.ForeColor = currentStyle.ForeColor;
                        }
                    }
                }
                else
                {
                    foreach (DataGridViewRow row in wingrid.Rows)
                    {
                        //this.DoEvents();
                        foreach (DataGridViewCell cell in row.Cells)
                            cell.Style = null;
                    }
                    this.SetAlternatingStyle(wingrid, this._gridOriginalAlternatingStyle);
                }
            }
        }

        private void SetAlternatingStyle(DataGridView wingrid, DataGridViewCellStyle newStyle)
        {
            wingrid.AlternatingRowsDefaultCellStyleChanged -= this.RecordGridAltStyle;
            wingrid.AlternatingRowsDefaultCellStyle = newStyle;
            wingrid.AlternatingRowsDefaultCellStyleChanged += this.RecordGridAltStyle;
        }

        private void DoEvents()
        {
            var now = DateTime.Now;
            if (this._lastForcedEvents.AddMilliseconds(500) < now)
            {
                Application.DoEvents();
                this._lastForcedEvents = now;
            }
        }

    }
}
