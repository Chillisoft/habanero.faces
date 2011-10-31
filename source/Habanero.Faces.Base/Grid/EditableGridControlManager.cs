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
using Habanero.Faces.Base.Grid;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// This manager groups common logic for IEditableGridControl objects.
    /// Do not use this object in working code - rather call CreateEditableGridControl
    /// in the appropriate control factory.
    /// </summary>
    public class EditableGridControlManager
    {
//        private readonly IEditableGridControl _gridControl;
        private readonly IGridInitialiser _gridInitialiser;
        private string _additionalSearchCriterial;
        private bool _allowUserToAddRows;

        ///<summary>
        /// Constructor for <see cref="EditableGridControlManager"/>
        ///</summary>
        ///<param name="gridControl"></param>
        ///<param name="controlFactory"></param>
        public EditableGridControlManager(IEditableGridControl gridControl, IControlFactory controlFactory)
        {
            GridControl = gridControl;
            _gridInitialiser = new GridInitialiser(gridControl, controlFactory);
        }

        private IEditableGridControl GridControl { get; set; }

        /// <summary>
        /// See <see cref="IGridControl.Initialise(IClassDef)"/>
        /// </summary>
        public void Initialise(IClassDef classDef)
        {
            _gridInitialiser.InitialiseGrid(classDef);
        }

        /// <summary>
        /// See <see cref="IGridControl.Initialise(IClassDef,string)"/>
        /// </summary>
        public void Initialise(IClassDef classDef, string uiDefName)
        {
            _gridInitialiser.InitialiseGrid(classDef, uiDefName);
        }


        /// <summary>
        /// See <see cref="IGridControl.Initialise(IClassDef,IUIGrid,string)"/>
        /// </summary>
        public void Initialise(IClassDef classDef, IUIGrid uiGridDef, string uiDefName)
        {
            _gridInitialiser.InitialiseGrid(classDef, uiGridDef, uiDefName);
        }

        /// <summary>
        /// Gets and sets the standard search criteria used for loading the grid when the <see cref="FilterModes"/>
        /// is set to Search. This search criteria will be appended with an AND to any search criteria returned
        /// by the FilterControl.
        /// </summary>
        public string AdditionalSearchCriterial
        {
            get
            {
                return _additionalSearchCriterial;
            }
            set
            {
                _additionalSearchCriterial = value;
                var staticCustomFilter = new StringLiteralCustomFilter(value);
                this.GridControl.FilterControl.AddCustomFilter("", staticCustomFilter);
            }
        }
        ///<summary>
        /// Reapplies the current filter to the Grid.
        ///</summary>
        public void RefreshFilter()
        {

            try
            {
                if (GridControl.FilterMode == FilterModes.Search)
                {
                    GridControl.Grid.ApplySearch(GridControl.FilterControl.GetFilterClause(), this.GridControl.OrderBy);
                    EnableButtonsAndFilter();
                }
                else
                {
                    GridControl.Grid.ApplyFilter(GridControl.FilterControl.GetFilterClause());
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error filtering");
            }
        }

        /// <summary>
        /// Sets the business object collection to display.  Loading of
        /// the collection needs to be done before it is assigned to the
        /// grid.  This method assumes a default UI definition is to be
        /// used, that is a 'ui' element without a 'name' attribute.
        /// </summary>
        /// <param name="boCollection">The business object collection
        /// to be shown in the grid</param>
        public void SetBusinessObjectCollection(IBusinessObjectCollection boCollection)
        {
            if (boCollection == null)
            {
                GridControl.Grid.BusinessObjectCollection = null;
                DisableGrid();
                return;
            }
            if (this.GridControl.ClassDef == null)//If the class Def is null then grid has not been initialised?
            {
                Initialise(boCollection.ClassDef);
            }
            else
            {
                if (this.GridControl.ClassDef != boCollection.ClassDef)
                {
                    throw new ArgumentException(
                        "You cannot call set collection for a collection that has a different class def than is initialised");
                }
            }

            GridControl.Grid.BusinessObjectCollection = boCollection;
            EnableButtonsAndFilter();
        }

        private void DisableGrid()
        {
            GridControl.Grid.AllowUserToAddRows = false;
            GridControl.Buttons.Enabled = false;
            GridControl.FilterControl.Enabled = false;
        }

        private void EnableButtonsAndFilter()
        {
            GridControl.Buttons.Enabled = true;
            GridControl.FilterControl.Enabled = true;
            GridControl.Grid.AllowUserToAddRows = _allowUserToAddRows;
        }

        ///<summary>
        /// Gets and sets whether the user can add Business objects via this control
        ///</summary>
        public bool AllowUsersToAddBO
        {
            get { return this.GridControl.Grid.AllowUserToAddRows; }
            set
            {
                this.GridControl.Grid.AllowUserToAddRows = value;
                _allowUserToAddRows = value;
            }
        }
    }
}