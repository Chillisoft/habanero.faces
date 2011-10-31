#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Grid
{
    public class TestEditableGridVWG : TestEditableGrid
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override IEditableGrid CreateEditableGrid()
        {
            EditableGridVWG editableGridVWG = new EditableGridVWG();
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add(editableGridVWG);
            return editableGridVWG;
        }

        protected override IFormHabanero AddControlToForm(IGridBase gridBase)
        {
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add((Gizmox.WebGUI.Forms.Control)gridBase);
            return null;
        }


        protected override void LoadMyBoDefaultClassDef()
        {
            MyBO.LoadDefaultClassDefVWG();
        }


        protected override void AssertIsTextBoxColumnType(IDataGridViewColumn dataGridViewColumn)
        {
            DataGridViewColumnVWG dataGridViewColumnVWG = (DataGridViewColumnVWG)dataGridViewColumn;
            Assert.IsInstanceOf
                (typeof(Gizmox.WebGUI.Forms.DataGridViewTextBoxColumn), dataGridViewColumnVWG.DataGridViewColumn);
        }

        protected override void AssertIsCheckBoxColumnType(IDataGridViewColumn dataGridViewColumn)
        {
            DataGridViewColumnVWG dataGridViewColumnVWG = (DataGridViewColumnVWG)dataGridViewColumn;
            Assert.IsInstanceOf
                (typeof(Gizmox.WebGUI.Forms.DataGridViewCheckBoxColumn), dataGridViewColumnVWG.DataGridViewColumn);
        }

        protected override void AssertIsComboBoxColumnType(IDataGridViewColumn dataGridViewColumn)
        {
            DataGridViewColumnVWG dataGridViewColumnVWG = (DataGridViewColumnVWG)dataGridViewColumn;
            Assert.IsInstanceOf
                (typeof(Gizmox.WebGUI.Forms.DataGridViewComboBoxColumn), dataGridViewColumnVWG.DataGridViewColumn);
        }

        protected override void AssertCorrectSelectionMode(IGridBase dataGridView)
        {
            Gizmox.WebGUI.Forms.DataGridView grid = (Gizmox.WebGUI.Forms.DataGridView)dataGridView;
            Assert.AreEqual(Gizmox.WebGUI.Forms.DataGridViewSelectionMode.RowHeaderSelect, grid.SelectionMode);
        }
    }
}