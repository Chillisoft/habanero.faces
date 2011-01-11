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
using System.Collections.Generic;
using System.Data;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// Initialises the structure of a grid control (i.e. a Grid with a Filter Control).  If a ClassDef is provided, the grid
    /// is initialised using the UI definition provided for that class.  If no
    /// ClassDef is provided, it is assumed that the grid will be set up in code
    /// by the developer.
    /// </summary>
    public class GridInitialiser : IGridInitialiser
    {
        private readonly IControlFactory _controlFactory;
        private readonly GridBaseInitialiser _gridBaseInitialiser;

        ///<summary>
        /// Initialise the grid with the appropriate control factory.
        ///</summary>
        ///<param name="gridControl"></param>
        ///<param name="controlFactory"></param>
        public GridInitialiser(IGridControl gridControl, IControlFactory controlFactory)
        {
            if (gridControl == null) throw new ArgumentNullException("gridControl");
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            Grid = gridControl;
            _controlFactory = controlFactory;
            _gridBaseInitialiser = new GridBaseInitialiser(Grid.Grid, controlFactory);
        }

        /// <summary>
        /// Initialises the grid without a ClassDef. This is typically used where the columns are set up manually
        /// for purposes such as adding a column with images to indicate the state of the object or adding a
        /// column with buttons/links.
        /// <br/>
        /// The grid must already have at least one column added. At least one column must be a column with the name
        /// "HABANERO_OBJECTID", which is used to synchronise the grid with the business objects.
        /// </summary>
        /// <exception cref="GridBaseInitialiseException">Thrown in the case where the columns
        /// have not already been defined for the grid</exception>
        /// <exception cref="GridBaseSetUpException">Thrown in the case where the grid has already been initialised</exception>
        public void InitialiseGrid()
        {         
            _gridBaseInitialiser.InitialiseGrid();
        }

        /// <summary>
        /// Initialises the grid with the default UI definition for the class,
        /// as provided in the ClassDef
        /// </summary>
        /// <param name="classDef">The ClassDef used to initialise the grid</param>
        public void InitialiseGrid(IClassDef classDef)
        {
            InitialiseGrid(classDef, "default");
        }

        /// <summary>
        /// Initialises the grid with a specified alternate UI definition for the class,
        /// as provided in the ClassDef
        /// </summary>
        /// <param name="classDef">The Classdef used to initialise the grid</param>
        /// <param name="uiDefName">The name of the UI definition</param>
        public void InitialiseGrid(IClassDef classDef, string uiDefName)
        {
            IUIGrid gridDef = GetGridDef((ClassDef) classDef, uiDefName);
            InitialiseGrid(classDef, gridDef, uiDefName);
        }

        /// <summary>
        /// Initialises the grid with a given alternate UI definition for the class
        ///  </summary>
        /// <param name="classDef">The Classdef used to initialise the grid</param>
        /// <param name="uiGridDef">The <see cref="IUIGrid"/> that specifies the grid </param>
        /// <param name="uiDefName">The name of the <see cref="IUIGrid"/></param>
        public void InitialiseGrid(IClassDef classDef, IUIGrid uiGridDef, string uiDefName)
        {
            if (uiGridDef.FilterDef != null)
            {
                FilterControlBuilder builder = new FilterControlBuilder(_controlFactory);
                builder.BuildFilterControl(uiGridDef.FilterDef, Grid.FilterControl);
                ShowFilterControl();
            }
            else if (!FilterControlSetupProgramatically())
            {
                HideFilterControl();
            }
            _gridBaseInitialiser.InitialiseGrid(classDef, uiGridDef, uiDefName);
        }

        private void ShowFilterControl()
        {
            Grid.FilterControl.Visible = true;
        }

        private void HideFilterControl()
        {
            Grid.FilterControl.Visible = false;
        }

        private bool FilterControlSetupProgramatically()
        {
            return Grid.FilterControl.FilterControls.Count != 0;
        }

        /// <summary>
        /// Gets the value indicating whether the grid has been initialised already
        /// </summary>
        public bool IsInitialised { get { return _gridBaseInitialiser.IsInitialised; } }

        /// <summary>
        /// Gets the grid that is being initialised
        /// </summary>
        private IGridControl Grid { get; set; }
        private IUIGrid GetGridDef(ClassDef classDef, string uiDefName)
        {
            IUIDef uiDef = classDef.GetUIDef(uiDefName);
            if (uiDef == null)
            {
                throw new ArgumentException
                    (String.Format
                         ("You cannot initialise {0} because it does not contain a definition for UIDef {1} for the class def {2}",
                          this.Grid.Grid.Name, uiDefName, classDef.ClassName));
            }
            IUIGrid gridDef = uiDef.UIGrid;
            if (gridDef == null)
            {
                throw new ArgumentException
                    (String.Format
                         ("You cannot initialise {0} does not contain a grid definition for UIDef {1} for the class def {2}",
                          this.Grid.Grid.Name, uiDefName, classDef.ClassName));
            }
            return gridDef;
        }
    }
}