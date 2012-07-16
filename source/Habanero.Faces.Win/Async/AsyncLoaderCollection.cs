using System;
using System.Threading;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.Base;
using Habanero.Faces.Base.Async;
using Habanero.Faces.Win.Async;

namespace Habanero.Faces.Win.Async
{
    public class AsyncLoaderCollection<T> : AsyncLoaderBase<T>, IAsyncLoaderCollection<T> where T : class, IBusinessObject, new()
    {
        public DataRetrieverCollectionDelegate DataRetriever { get; set; }
        public ISupportAsyncLoadingCollection DisplayObject { get; set; }
        protected override void AsyncFetchWorker<T>()
        {
            IBusinessObjectCollection boCollection;
            if (this.DataRetriever != null)
                boCollection = this.DataRetriever();
            else
                boCollection = Broker.GetBusinessObjectCollection<T>(this.Criteria);
            this.DisplayObject.BeginInvoke((MethodInvoker)delegate
              {
                  this.DisplayObject.BusinessObjectCollection = boCollection;
              });
        }
        protected override void OnAsyncOperationStarted()
        {
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