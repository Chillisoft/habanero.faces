using System;
using System.Windows.Forms;
using Habanero.Base;

namespace Habanero.Faces.Win.Async
{
    public class ActionDispatcher : IActionDispatcher
    {
        protected Control _dispatchControl;
        public ActionDispatcher(Control dispatchOn)
        {
            this._dispatchControl = dispatchOn;
        }
        public virtual void Dispatch(Action method)
        {
            UIThreadExecutorWin.ExecuteOnUIThread(this._dispatchControl, method);
        }
    }
}
