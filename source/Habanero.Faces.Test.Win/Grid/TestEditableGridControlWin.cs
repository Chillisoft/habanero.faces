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
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Grid
{
    [TestFixture]
    public class TestEditableGridControlWin : TestEditableGridControl
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        //protected override IGridBase CreateGridBaseStub()
        //{
        //    GridBaseWinStub gridBase = new GridBaseWinStub();
        //    System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
        //    frm.Controls.Add(gridBase);
        //    return gridBase;
        //}

        //private static System.Windows.Forms.DataGridViewCell GetCell(int rowIndex, string propName,
        //                                                             IGridBase gridBase)
        //{
        //    System.Windows.Forms.DataGridView dgv = (System.Windows.Forms.DataGridView) gridBase;
        //    System.Windows.Forms.DataGridViewRow row = dgv.Rows[rowIndex];
        //    return row.Cells[propName];
        //}

        //protected override void AddControlToForm(IGridBase gridBase)
        //{
        //    throw new NotImplementedException();
        //}
        protected override IFormHabanero AddControlToForm(IControlHabanero cntrl)
        {
            //System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            //frm.Controls.Add((System.Windows.Forms.Control)cntrl);

            IFormHabanero frm = GetControlFactory().CreateForm();
            frm.Controls.Add(cntrl);
            return frm;
        }

        protected override void AssertIsTextBoxColumnType(IDataGridViewColumn dataGridViewColumn)
        {
            DataGridViewColumnWin dataGridViewColumnWin = (DataGridViewColumnWin)dataGridViewColumn;
            Assert.IsInstanceOf
                (typeof(DataGridViewTextBoxColumn), dataGridViewColumnWin.DataGridViewColumn);
        }

        protected override void AssertIsCheckBoxColumnType(IDataGridViewColumn dataGridViewColumn)
        {
            DataGridViewColumnWin dataGridViewColumnWin = (DataGridViewColumnWin)dataGridViewColumn;
            Assert.IsInstanceOf
                (typeof(DataGridViewCheckBoxColumn), dataGridViewColumnWin.DataGridViewColumn);
        }

        protected override void AssertIsComboBoxColumnType(IDataGridViewColumn dataGridViewColumn)
        {
            DataGridViewColumnWin dataGridViewColumnWin = (DataGridViewColumnWin)dataGridViewColumn;
            Assert.IsInstanceOf
                (typeof(DataGridViewComboBoxColumn), dataGridViewColumnWin.DataGridViewColumn);
        }

        protected override void AssertComboBoxItemCount(IDataGridViewColumn dataGridViewColumn, int expectedCount)
        {
            DataGridViewColumnWin dataGridViewColumnWin = (DataGridViewColumnWin)dataGridViewColumn;
            Assert.AreEqual(expectedCount,
                            ((DataGridViewComboBoxColumn)dataGridViewColumnWin.DataGridViewColumn).Items.Count);
        }

        protected override void AssertMainControlsOnForm(IFormHabanero form)
        {
            Assert.AreEqual(3, form.Controls[0].Controls.Count);
            Assert.IsInstanceOf(typeof(IFilterControl), form.Controls[0].Controls[1]);
            Assert.IsInstanceOf(typeof(IEditableGrid), form.Controls[0].Controls[0]);
            Assert.IsInstanceOf(typeof(IButtonGroupControl), form.Controls[0].Controls[2]);
        }

        protected override IEditableGridControl CreateEditableGridControl()
        {
            EditableGridControlWin editableGridControlWin = new EditableGridControlWin(GetControlFactory());
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add(editableGridControlWin);
            return editableGridControlWin;
        }
        [Test, Ignore("This does not work for win for some reason.")]
        public void TestButtonsControl_ClickCancelRestoresGridToOriginalState()
        {
            //---------------Set up test pack-------------------
            //Get Grid with 4 items
            BusinessObjectCollection<MyBO> col;
            IEditableGridControl gridControl = GetGridWith_5_Rows(out col);
            AddControlToForm(gridControl);
            //---------------Assert Precondition----------------
            Assert.AreEqual(5, gridControl.Grid.Rows.Count);
            Assert.AreEqual("b", gridControl.Grid.Rows[0].Cells[1].Value);
            //---------------Execute Test ----------------------
            gridControl.Grid.Rows[0].Cells[1].Value = "test";
            //---------------Assert Precondition----------------
            Assert.AreEqual("test", gridControl.Grid.Rows[0].Cells[1].Value);
            //---------------Execute Test ----------------------
            gridControl.Buttons["Cancel"].PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual("b", gridControl.Grid.Rows[0].Cells[1].Value);
        }


        [Test]
        public void Test_RaiseErrorIfControlFactoryNull()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new EditableGridControlWin(null);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroArgumentException ex)
            {
                StringAssert.Contains
                    ("Cannot create an editable grid control if the control factory is null", ex.Message);
            }
        }

        protected override IClassDef LoadMyBoDefaultClassDef()
        {
          return MyBO.LoadDefaultClassDef();
        }
    }
}