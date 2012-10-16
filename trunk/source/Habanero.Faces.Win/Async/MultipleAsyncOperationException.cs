using System;

namespace Habanero.Faces.Win
{
    /// <summary>
    /// Provides a combination of read-only grid, filter and buttons used to edit a
    /// collection of business objects.
    /// <br/>
    /// Adding, editing and deleting objects is done by clicking the available
    /// buttons in the button control (accessed through the Buttons property).
    /// By default, this uses of a popup form for editing of the object, as defined
    /// in the "form" element of the class definitions for that object.  You can
    /// override the editing controls using the BusinessObjectEditor/Creator/Deletor
    /// properties in this class.
    /// <br/>
    /// A filter control is placed above the grid and is used to filter which rows
    /// are shown.
    /// </summary>
    public class MultipleAsyncOperationException : Exception
    {
        /// <summary>
        /// Exception thrown when the application developer allows multiple synchronous async calls to the grid
        /// (which would result in, at best, oddness)
        /// </summary>
        /// <param name="message"></param>
        public MultipleAsyncOperationException(string message)
            : base(message)
        {
        }
    }
}