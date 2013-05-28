//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Drawing;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{

    public abstract class TestCalendarCell:TestBaseWithDisposing //: TestBase
    {
        protected abstract IControlFactory GetControlFactory();




        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
        }

    

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }
    }
 
    public interface ICalendarCell
    {
        /// <summary>
        /// Initialises the editing control
        /// </summary>
        /// <param name="rowIndex">The row index number</param>
        /// <param name="initialFormattedValue">The initial value</param>
        /// <param name="dataGridViewCellStyle">The cell style</param>
        void InitializeEditingControl
            (int rowIndex, object initialFormattedValue, IDataGridViewCellStyle dataGridViewCellStyle);

        /// <summary>
        /// Returns the type of editing control that is used
        /// </summary>
        Type EditType { get; }

        /// <summary>
        /// Returns the type of value contained in the cell
        /// </summary>
        Type ValueType { get; }

        /// <summary>
        /// Returns the default value for a new row, which in this case is
        /// the current date and time
        /// </summary>
        object DefaultNewRowValue { get; }
    }
}