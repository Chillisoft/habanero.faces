using System;
using System.Windows.Forms;
using Habanero.Faces.Base;
using Habanero.Faces.Base.ControlInterfaces;
using Habanero.Faces.Win;

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
        private bool _filterRequired;
        private DateTime _lastFilterChanged;

        public GenericGridFilterControlWin(IGridBase grid)
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

            var factory = new ControlFactoryWin();
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
            foreach (DataGridViewWin.DataGridViewRowWin r in this.Grid.Rows)
                r.Visible = true;
            if (this.FilterCompleted != null)
                this.FilterCompleted(this, new EventArgs());
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
            FilterGrid(this.Grid, filter);
            this._inFilter = false;
            if (this.FilterCompleted != null)
                this.FilterCompleted(this, new EventArgs());
        }

        public static void FilterGrid(IGridBase dataGrid, string filter)
        {
            if (dataGrid.Rows.Count == 0) return;
            dataGrid.CurrentCell = null;
            var parts = filter.Split(new char[] { ' ' });
            DataGridViewWin.DataGridViewRowWin firstVisibleRow = null;
            var colCount = GetColumnCount(dataGrid);

            foreach (DataGridViewWin.DataGridViewRowWin r in dataGrid.Rows)
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
                if (firstVisibleRow == null)
                    firstVisibleRow = r;
            }
            if (firstVisibleRow != null)
                firstVisibleRow.Selected = true;
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
