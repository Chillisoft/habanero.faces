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
    public class ReadOnlyGridControlManager
    {
        private string _additionalSearchCriterial;

        ///<summary>
        /// The <see cref="IReadOnlyGridControl"/> that this manager is managing.
        ///</summary>
        private IReadOnlyGridControl GridControl { get; set; }
        /// <summary>
        /// The factory used to create other control such as popup forms.
        /// </summary>
        private IControlFactory ControlFactory { get; set; }

        /// <summary>
        /// The Inititialiser used to setup the Grid Columns and the FilterControlControl based 
        /// on the ClassDef.
        /// </summary>
        public IGridInitialiser GridInitialiser { get; private set; }

        ///<summary>
        /// Constructor for <see cref="EditableGridControlManager"/>
        ///</summary>
        ///<param name="gridControl"></param>
        ///<param name="controlFactory"></param>
        public ReadOnlyGridControlManager(IReadOnlyGridControl gridControl, IControlFactory controlFactory)
        {
            GridControl = gridControl;
            ControlFactory = controlFactory;
            if (gridControl == null) throw new ArgumentNullException("gridControl");
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            GridInitialiser = new GridInitialiser(gridControl, controlFactory);
        }

        /// <summary>
        /// Gets and sets the standard search criteria used for loading the grid when the <see cref="FilterModes"/>
        /// is set to Search. This search criteria will be appended with an AND to any search criteria returned
        /// by the FilterControlControl.
        /// </summary>
        public string AdditionalSearchCriterial
        {
            get {
                return _additionalSearchCriterial;
            }
            set {
                _additionalSearchCriterial = value;
                var staticCustomFilter = new StringLiteralCustomFilter(value);
                this.GridControl.FilterControl.AddCustomFilter("", staticCustomFilter);
            }
        }

        ///<summary>
        /// Deletes the <paramref name="selectedBo"/> using the
        /// <see cref="IReadOnlyGridControl.BusinessObjectDeletor"/>.
        /// Rolls back the delete and notifies the user if any errors occur
        ///</summary>
        ///<param name="selectedBo"></param>
        public void DeleteBusinessObject(IBusinessObject selectedBo)
        {
            GridControl.SelectedBusinessObject = null;
            try
            {
                GridControl.BusinessObjectDeletor.DeleteBusinessObject(selectedBo);
            }
            catch (Exception ex)
            {
                try
                {
                    selectedBo.CancelEdits();
                    GridControl.SelectedBusinessObject = selectedBo;
                }
                    // ReSharper disable EmptyGeneralCatchClause
                catch
                {
                    //Do nothing
                }
                // ReSharper restore EmptyGeneralCatchClause
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "There was a problem deleting",
                                                          "Problem Deleting");
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
                    SetupBOEditors(GridControl.Grid.BusinessObjectCollection);
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
                this.GridInitialiser.InitialiseGrid(boCollection.ClassDef);
            }
            else
            {
                if (this.GridControl.ClassDef != boCollection.ClassDef)
                {
                    throw new ArgumentException(
                        "You cannot call set collection for a collection that has a different class def than is initialised");
                }
            }
            SetupBOEditors(boCollection);

            GridControl.Grid.BusinessObjectCollection = boCollection;
            EnableButtonsAndFilter();
        }

        private void DisableGrid()
        {
            GridControl.Buttons.Enabled = false;
            GridControl.FilterControl.Enabled = false;
        }

        private void EnableButtonsAndFilter()
        {
            GridControl.Buttons.Enabled = true;
            GridControl.FilterControl.Enabled = true;
        }

        private void SetupBOEditors(IBusinessObjectCollection boCollection)
        {
            if (GridControl.BusinessObjectCreator is DefaultBOCreator)
            {
                GridControl.BusinessObjectCreator = new DefaultBOCreator(boCollection);
            }
            if (GridControl.BusinessObjectCreator == null) GridControl.BusinessObjectCreator = new DefaultBOCreator(boCollection);
            if (GridControl.BusinessObjectEditor == null) GridControl.BusinessObjectEditor = new DefaultBOEditor(this.ControlFactory);
            if (GridControl.BusinessObjectDeletor == null) GridControl.BusinessObjectDeletor = new DefaultBODeletor();
        }

        ///<summary>
        /// Returns true if <see cref="IReadOnlyGridControl.ConfirmDeletion"/> is false or if
        ///  <see cref="IReadOnlyGridControl.ConfirmDeletion"/> is true and the  <see cref="IReadOnlyGridControl.CheckUserConfirmsDeletionDelegate"/> 
        /// returns true.
        ///</summary>
        ///<returns></returns>
        public bool MustDeleteSelectedBusinessObject()
        {
            return !GridControl.ConfirmDeletion || (GridControl.ConfirmDeletion && GridControl.CheckUserConfirmsDeletionDelegate());
        }

    }
}