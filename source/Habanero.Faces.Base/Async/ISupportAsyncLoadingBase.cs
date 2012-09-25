using System;

namespace Habanero.Faces.Base.Async
{
    public class ExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; protected set; }
        public ExceptionEventArgs(Exception ex)
        {
            this.Exception = ex;
        }
    }
    public interface ISupportAsyncLoadingBase
    {
        void ExecuteOnUIThread(Delegate method);
        EventHandler OnAsyncOperationComplete { get; set; }
        EventHandler OnAsyncOperationStarted { get; set; }
        EventHandler OnAsyncOperationException { get; set; }
    }
}
