using System;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// This manager groups common logic for IEditableGridControl objects.
    /// Do not use this object in working code - rather call CreateEditableGridControl
    /// in the appropriate control factory.
    /// </summary>
    public class ReadOnlyGridControlManager
    {
        ///<summary>
        /// The <see cref="IReadOnlyGridControl"/> that this manager is managing.
        ///</summary>
        public IReadOnlyGridControl GridControl { get; set; }
        //        private readonly IEditableGridControl _gridControl;
        private readonly IGridInitialiser _gridInitialiser;

        ///<summary>
        /// Constructor for <see cref="EditableGridControlManager"/>
        ///</summary>
        ///<param name="gridControl"></param>
        ///<param name="controlFactory"></param>
        public ReadOnlyGridControlManager(IReadOnlyGridControl gridControl, IControlFactory controlFactory)
        {
            GridControl = gridControl;
            if (gridControl == null) throw new ArgumentNullException("gridControl");
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
//            _gridControl = gridControl;
            _gridInitialiser = new GridInitialiser(gridControl, controlFactory);
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