using Habanero.Base;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// Provides a common interface that is specialised for showing a collection of 
    /// Business Objects and allowing the user to select and edit one.
    /// The common controls used for selecting business objects are ComboBox, ListBox, ListView, Grid,
    ///  <see cref="ICollapsiblePanelGroupControl"/>, <see cref="IBOColTabControl"/>, a <see cref="IMultiSelector{T}"/>
    ///  or an <see cref="ITreeView"/>.
    /// For an <see cref="IEditableGridControl"/> the business objects can be edited directly in the grid. In the case of 
    ///  a <see cref="IReadOnlyGridControl"/> the business objects are edited via pop up forms that open when appropriate.
    /// The <see cref="IBOGridAndEditorControl"/> has a grid selector with the functionality to edit the business obects to the right.
    /// This interface inherits from the <see cref="IBOColSelectorControl"/> and adds three additional methods.
    /// <li><see cref="AllowUsersToAddBO"/></li>"
    /// <li><see cref="AllowUsersToDeleteBO"/></li>"
    /// <li><see cref="AllowUsersToEditBO"/></li>"
    /// </summary>
    /// <remarks>
    /// Should possibly allow the user to set an adding, editing or deleting delegate
    /// </remarks>
    public interface IBOSelectorAndEditor : IBOColSelectorControl
    {
        ///<summary>
        /// Gets and sets whether the user can add Business objects via this control
        ///</summary>
        bool AllowUsersToAddBO { get; set; }

        /// <summary>
        /// Gets and sets whether the user can Delete (<see cref="IBusinessObject.MarkForDelete"/>) <see cref="IBusinessObject"/>s via this control
        /// </summary>
        bool AllowUsersToDeleteBO { get; set; }

        /// <summary>
        /// Gets and sets whether the user can edit <see cref="IBusinessObject"/>s via this control
        /// </summary>
        bool AllowUsersToEditBO { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that determines whether to confirm
        /// deletion with the user when they have chosen to delete a row
        /// </summary>
        bool ConfirmDeletion { get; set; }

        /// <summary>
        /// Gets or sets the delegate that checks whether the user wants to delete selected rows.
        /// If <see cref="ConfirmDeletion"/> is true and no specific <see cref="CheckUserConfirmsDeletionDelegate"/> is set then
        /// a default <see cref="CheckUserConfirmsDeletion"/> is used.
        /// </summary>
        CheckUserConfirmsDeletion CheckUserConfirmsDeletionDelegate { get; set; }
    }
}