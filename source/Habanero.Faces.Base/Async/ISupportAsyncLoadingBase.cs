using System;
using Habanero.Base;

namespace Habanero.Faces.Base.Async
{
    public interface ISupportAsyncLoadingBase
    {
        void ExecuteOnUIThread(Delegate method);
        bool Enabled { get; set; }
        EventHandler AsyncOperationComplete { get; set; }
        EventHandler AsyncOperationStarted { get; set; }
    }
}
