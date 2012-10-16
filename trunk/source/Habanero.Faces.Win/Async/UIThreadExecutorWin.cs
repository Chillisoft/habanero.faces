using System;
using System.Threading;
using System.Windows.Forms;

namespace Habanero.Faces.Win.Async
{
    class UIThreadExecutorWin
    {
        /// <summary>
        /// Attempts to run a delegate method on the UI thread of a System.Windows.Forms.Control
        /// Basically a wrapper around BeginInvoke with testing for readiness / availability of the
        /// control itself
        /// </summary>
        /// <param name="control">Control to run on</param>
        /// <param name="method">delegate method to run</param>
        public static void ExecuteOnUIThread(Control control, Delegate method)
        {
            for (var i = 0; i < 30; i++)
            {
                if (control.IsHandleCreated)
                    break;
                if (control.IsDisposed || control.Disposing)
                    return;
                Thread.Sleep(100);
            }
            control.BeginInvoke(method);
        }
    }
}
