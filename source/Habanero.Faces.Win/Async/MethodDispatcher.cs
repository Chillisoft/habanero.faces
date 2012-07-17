using System;
using System.Windows.Forms;

namespace Habanero.Faces.Win.Async
{
    public class MethodDispatcher
    {
        protected Control _dispatchControl;
        public MethodDispatcher(Control dispatchOn)
        {
            this._dispatchControl = dispatchOn;
        }
        public virtual void Dispatch(MethodInvoker method)
        {
            UIThreadExecutorWin.ExecuteOnUIThread(this._dispatchControl, method);
        }
    }
}
