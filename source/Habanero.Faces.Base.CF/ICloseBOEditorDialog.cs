using System.Windows.Forms;
using Habanero.Base;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// The Result returned from the <see cref="ICloseBOEditorDialog"/>
    /// </summary>
    public enum CloseBOEditorDialogResult
    {
        ///<summary>
        /// Cancel Closing of the Origional form.
        ///</summary>
        CancelClose,
        /// <summary>
        /// Save the BusinessObject(s) and then Close the Form.
        /// </summary>
        SaveAndClose,
        /// <summary>
        /// Close the form and lose all changes to the Busienss Object(s).
        /// </summary>
        CloseWithoutSaving

    }
    /// <summary>
    /// This the interface for a Dialog Box that is specialiased for dealing with the
    /// Closing of any form or application that is editing Business Objects.
    /// The dialogue box will display a sensible message to the user to determine
    /// whether they want to Close the Origional form without saving, Save the BO and then
    /// Close or Cancel the Closing of the origional form.
    /// </summary>
    public interface ICloseBOEditorDialog
    {
        /// <summary>
        /// The CancelClose Button.
        /// </summary>
        Button CancelCloseBtn { get; }

        /// <summary>
        /// The Save and Close Button.
        /// </summary>
        Button SaveAndCloseBtn { get; }

        /// <summary>
        /// The Close without saving Button.
        /// </summary>
        Button CloseWithoutSavingBtn { get; }

        /// <summary>
        /// Shows the Dialog form with the relevant options and messages for the business object.
        /// In certain circumstances this dialog form will not be shown/visble e.g. if the business object is null
        /// Or if the business object is not dirty.
        /// In these circumstances the Dialog should make its own decision e.g. CloseWithoutSaving.
        /// </summary>
        /// <param name="businessObject">The business Object whose Dirty state is being checked.</param>
        /// <returns>Returns the option selected by the user.</returns>
        CloseBOEditorDialogResult ShowDialog(IBusinessObject businessObject);

        /// <summary>
        /// The Result from this Form.
        /// </summary>
        CloseBOEditorDialogResult BOEditorDialogResult { get; }
    }
}