using System;

namespace Habanero.Faces.Base.Async
{
    public interface ISupportAsyncLoadingBase
    {
        void ExecuteOnUIThread(Delegate method);
        EventHandler OnAsyncOperationComplete { get; set; }
        EventHandler OnAsyncOperationStarted { get; set; }
    }
}
