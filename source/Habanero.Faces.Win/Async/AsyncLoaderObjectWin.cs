using System;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.Base;

namespace Habanero.Faces.Base.Async
{
    /// <summary>
    /// Loader for setting CurrentBusinessObject on a control asynchronously
    /// </summary>
    /// <typeparam name="T">BO type</typeparam>
    public class AsyncLoaderObjectWin<T> : AsyncLoaderBase<T> where T : class, IBusinessObject, new()
    {
        /// <summary>
        /// Delegate type used for the data retrieval method
        /// </summary>
        public DataRetrieverObjectDelegate DataRetriever { get; set; }
        /// <summary>
        /// UI control which will be used to synchronise the UI portion of the work after the
        /// long background operation completes
        /// </summary>
        public ISupportAsyncLoadingObject DisplayObject { get; set; }

        protected override void AsyncFetchWorker<T>()
        {
            IBusinessObject bo;
            if (this.DataRetriever == null)
                bo = Broker.GetBusinessObject<T>(this.Criteria);
            else
                bo = this.DataRetriever();
            this.DisplayObject.ExecuteOnUIThread((MethodInvoker)delegate
              {
                  this.DisplayObject.CurrentBusinessObject = bo;
                  this.OnAsyncOperationsCompleted();
              });
        }
        protected override void OnAsyncOperationStarted()
        {
            var ctl = this.DisplayObject as Control;
            if (ctl != null)
                ctl.Cursor = Cursors.WaitCursor;
            if (this.AsyncOperationStarted != null)
                this.AsyncOperationStarted(this.DisplayObject, new EventArgs());
        }
        protected override void OnAsyncOperationsCompleted()
        {
            this.DisplayObject.Enabled = true;
            var ctl = this.DisplayObject as Control;
            if (ctl != null)
                ctl.Cursor = Cursors.Default;
            if (this.AsyncOperationComplete != null)
                this.AsyncOperationComplete(this.DisplayObject, new EventArgs());
        }
    }
}
