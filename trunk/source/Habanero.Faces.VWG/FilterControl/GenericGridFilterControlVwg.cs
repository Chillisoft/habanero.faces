using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gizmox.WebGUI.Forms;
using Habanero.Faces.Base;
using Habanero.Faces.Base.ControlInterfaces;

namespace Habanero.Faces.VWG.FilterControl
{
    class GenericGridFilterControlVwg : PanelVWG, IGenericGridFilterControl
    {
        public EventHandler FilterStarted { get; set; }
        public EventHandler FilterCompleted { get; set; }
        public IGridBase Grid { get; set; }
        public string FilterText { get; set; }

        private ITextBox _filterTextBox;
        private ILabel _filterLabel;
        private Timer _timer;
        private bool _inFilter;
        private bool _filterRequired;
        private DateTime _lastFilterChanged;
        private DataGridViewCellStyle _gridOriginalAlternatingStyle;

        public GenericGridFilterControlVwg(IGridBase grid)
        {
            this.Grid = grid;
            this._timer = new Timer()
            {
                Enabled = true,
                Interval = 500,
            };
            this._timer.Tick += (sender, e) => 
            {
                if ((!this._inFilter) && (this._filterRequired) && (this._lastFilterChanged.AddMilliseconds(this._timer.Interval) < DateTime.Now))
                {
                    this._filterRequired = false;
                    this.DoFilter();
                }
            };

            var factory = new ControlFactoryVWG();
            this._filterLabel = factory.CreateLabel("Filter:");
            this._filterTextBox = factory.CreateTextBox();
            var txt = this._filterTextBox as TextBox;
            txt.KeyPress += (sender, e) =>
                {
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
        }

        private void SetUIState(bool filtering)
        {
            this.Enabled = !filtering;
            this.Grid.Enabled = !filtering;
            var grid = this.Grid as DataGridView;
            this._filterTextBox.Focus();
            Application.DoEvents();
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

        private void ClearFilter()
        {
            if (this.Grid == null)
                return;
            if (this.FilterStarted != null)
                this.FilterStarted(this, new EventArgs());
            foreach (DataGridViewVWG.DataGridViewRowVWG r in this.Grid.Rows)
                r.Visible = true;
            var wingrid = this.Grid as DataGridView;
            if (wingrid != null)
                this.SetAlternatingStyle(wingrid, this._gridOriginalAlternatingStyle);
            if (this.FilterCompleted != null)
                this.FilterCompleted(this, new EventArgs());
        }

        public void FilterGrid(string filter)
        {
            var dataGrid = this.Grid;
            if (dataGrid.Rows.Count == 0) return;
            dataGrid.CurrentCell = null;
            var parts = filter.Split(new char[] { ' ' });
            DataGridViewVWG.DataGridViewRowVWG firstVisibleRow = null;
            DataGridView gridControl = this.Grid as DataGridView;
            if (gridControl != null)
                this.SetAlternatingStyle(gridControl, null);

            var colCount = GetColumnCount(dataGrid);
            var somethingHidden = false;
            var somethingVisible = false;

            foreach (DataGridViewVWG.DataGridViewRowVWG r in dataGrid.Rows)
            {
                var hits = 0;
                foreach (var part in parts)
                {
                    for (var i = 0; i < colCount; i++)
                    {
                        var c = r.Cells[i];
                        if (c.Value.ToString().ToLower().Contains(part))
                        {
                            hits++;
                            break;
                        }
                    }
                }
                r.Visible = (hits == parts.Length);
                if (!r.Visible) somethingHidden = true;
                if (r.Visible) somethingVisible = true;
                r.Selected = false;
                if ((firstVisibleRow == null) && r.Visible)
                    firstVisibleRow = r;
            }
            if (firstVisibleRow != null)
                firstVisibleRow.Selected = true;
            this.SetupFilteredAlternatingRowColors(gridControl, somethingVisible, somethingHidden);
        }

        private void SetAlternatingStyle(DataGridView wingrid, DataGridViewCellStyle newStyle)
        {
            wingrid.AlternatingRowsDefaultCellStyleChanged -= this.RecordGridAltStyle;
            wingrid.AlternatingRowsDefaultCellStyle = newStyle;
            wingrid.AlternatingRowsDefaultCellStyleChanged += this.RecordGridAltStyle;
        }

        private void RecordGridAltStyle(object sender, EventArgs e)
        {
            var wingrid = this.Grid as DataGridView;
            if (wingrid != null)
                this._gridOriginalAlternatingStyle = wingrid.AlternatingRowsDefaultCellStyle;
        }

        private void SetupFilteredAlternatingRowColors(DataGridView gridcontrol, bool somethingVisible, bool somethingHidden)
        {
            if (gridcontrol != null)
            {
                if (somethingHidden && somethingVisible && this._gridOriginalAlternatingStyle != null)
                {
                    bool defaultStyle = true;
                    for (var i = 0; i < gridcontrol.Rows.Count; i++)
                    {
                        var row = gridcontrol.Rows[i];
                        if (!row.Visible) continue;
                        var currentStyle = (defaultStyle) ? gridcontrol.DefaultCellStyle : this._gridOriginalAlternatingStyle;
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
                    foreach (DataGridViewRow row in gridcontrol.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                            cell.Style = null;
                    }
                    this.SetAlternatingStyle(gridcontrol, this._gridOriginalAlternatingStyle);
                }
            }
        }


        private static int GetColumnCount(IGridBase dataGrid)
        {
            var test = 0;
            var error = false;
            while (!error)
            {
                try
                {
                    var testVal = dataGrid.Rows[0].Cells[test];
                    test++;
                }
                catch (Exception)
                {
                    error = true;
                }
            }
            return test;
        }
    }
}
