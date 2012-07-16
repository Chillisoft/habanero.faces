using System;
using System.Threading;
using Habanero.Base;

namespace Habanero.Faces.Base.Async
{
    public abstract class AsyncLoaderBase<T> : IAsyncLoaderBase<T> where T : class, IBusinessObject, new()
    {
        public Criteria Criteria { get; set; }
        public EventHandler AsyncOperationComplete { get; set; }
        public EventHandler AsyncOperationStarted { get; set; }
        public void FetchAsync()
        {
            this.OnAsyncOperationStarted();
            var t = new Thread(this.AsyncFetchWorker<T>);
            t.IsBackground = true;  // allow application termination to terminate this thread
            t.Start();
        }
        protected abstract void AsyncFetchWorker<T>() where T : class, IBusinessObject, new();
        protected abstract void OnAsyncOperationStarted();
        protected abstract void OnAsyncOperationsCompleted();
    }
}
