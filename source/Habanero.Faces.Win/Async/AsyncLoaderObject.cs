using System;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.Base;
using Habanero.Faces.Base.Async;

namespace Habanero.Faces.Win.Async
{
    public class AsyncLoaderObject<T> : AsyncLoaderBase<T> where T : class, IBusinessObject, new()
    {
        public DataRetrieverObjectDelegate DataRetriever { get; set; }
        public ISupportAsyncLoadingObject DisplayObject { get; set; }
        protected override void AsyncFetchWorker<T>()// where T: class, IBusinessObject, new()
        {
            IBusinessObject bo;
            if (this.DataRetriever == null)
                bo = Broker.GetBusinessObject<T>(this.Criteria);
            else
                bo = this.DataRetriever();
            this.DisplayObject.BeginInvoke((MethodInvoker)delegate
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
