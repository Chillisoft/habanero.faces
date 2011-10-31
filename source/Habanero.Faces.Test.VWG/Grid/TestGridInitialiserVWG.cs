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
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Grid
{
    [TestFixture]
    public class TestGridInitialiserVWG : TestGridInitialiser
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.VWG.ControlFactoryVWG();
        }

        protected override void AddControlToForm(IControlHabanero cntrl)
        {
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add((Gizmox.WebGUI.Forms.Control)cntrl);
        }


        protected override Type GetDateTimeGridColumnType()
        {
            throw new NotImplementedException("Not implemented for VWG");
            //return typeof(Habanero.Faces.VWG.DataGridViewDateTimeColumn);
        }

        protected override Type GetComboBoxGridColumnType()
        {
            return typeof(Gizmox.WebGUI.Forms.DataGridViewComboBoxColumn);
        }

        protected override void AssertGridColumnTypeAfterCast(IDataGridViewColumn createdColumn, Type expectedColumnType)
        {
            Habanero.Faces.VWG.DataGridViewColumnVWG columnWin = (Habanero.Faces.VWG.DataGridViewColumnVWG)createdColumn;
            Gizmox.WebGUI.Forms.DataGridViewColumn column = columnWin.DataGridViewColumn;
            Assert.AreEqual(expectedColumnType, column.GetType());
        }

        [Test, Ignore("DateTimeColumn needs to be implemented for VWG")]
        public override void TestInitGrid_LoadsDataGridViewDateTimeColumn()
        {
            base.TestInitGrid_LoadsDataGridViewDateTimeColumn();
        }


        [Test]
        public void TestInitGrid_LoadsCustomColumnType()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithDateTimeParameterFormat();
            IEditableGridControl grid = GetControlFactory().CreateEditableGridControl();
            IGridInitialiser initialiser = new GridInitialiser(grid, GetControlFactory());
            IUIDef uiDef = classDef.UIDefCol["default"];
            IUIGrid uiGridDef = uiDef.UIGrid;

            Type customColumnType = typeof(CustomDataGridViewColumnVWG);
            uiGridDef[2].GridControlTypeName = customColumnType.Name; //"CustomDataGridViewColumn";
            uiGridDef[2].GridControlAssemblyName = "Habanero.Faces.Test.VWG";
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