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
using System;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Faces.Test.Base;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Grid
{
    [TestFixture]
    public class TestGridBaseInitialiserWin : TestGridBaseInitialiser
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.Win.ControlFactoryWin();
        }

        protected override void AddControlToForm(IControlHabanero cntrl)
        {
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add((System.Windows.Forms.Control)cntrl);
        }

        protected override Type GetDateTimeGridColumnType()
        {
            return typeof(Habanero.Faces.Win.DataGridViewDateTimeColumn);
        }

        protected override Type GetComboBoxGridColumnType()
        {
            return typeof(System.Windows.Forms.DataGridViewComboBoxColumn);
        }

        protected override void AssertGridColumnTypeAfterCast(IDataGridViewColumn createdColumn, Type expectedColumnType)
        {
            Habanero.Faces.Win.DataGridViewColumnWin columnWin = (Habanero.Faces.Win.DataGridViewColumnWin)createdColumn;
            System.Windows.Forms.DataGridViewColumn column = columnWin.DataGridViewColumn;
            Assert.AreEqual(expectedColumnType, column.GetType());
        }

        [Test]
        public void TestInitGrid_LoadsCustomColumnType()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithDateTimeParameterFormat();
            IEditableGridControl grid = GetControlFactory().CreateEditableGridControl();
            IGridInitialiser initialiser = new GridBaseInitialiser(grid.Grid, GetControlFactory());
            IUIDef uiDef = classDef.UIDefCol["default"];
            IUIGrid uiGridDef = uiDef.UIGrid;

            Type customColumnType = typeof(CustomDataGridViewColumnWin);
            uiGridDef[2].GridControlTypeName = customColumnType.Name; //"CustomDataGridViewColumn";
            uiGridDef[2].GridControlAssemblyName = "Habanero.Faces.Test.Win";
            AddControlToForm(grid);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            initialiser.InitialiseGrid(classDef);
            //---------------Test Result -----------------------
            Assert.AreEqual(6, grid.Grid.Columns.Count);
            IDataGridViewColumn column3 = grid.Grid.Columns[3];
            Assert.AreEqual("TestDateTime", column3.Name);
            Assert.IsInstanceOf(typeof(IDataGridViewColumn), column3);
            AssertGridColumnTypeAfterCast(column3, customColumnType);
        }
    }
}