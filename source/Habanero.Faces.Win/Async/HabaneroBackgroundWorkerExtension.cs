using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Faces.Win.Async;

namespace Habanero.Base
{
    public static class HabaneroBackgroundWorkerExtension
    {
        public static HabaneroBackgroundWorker Run(this HabaneroBackgroundWorker self, Control uiControl,
            ConcurrentDictionary<string, object> data, 
            HabaneroBackgroundWorker.BackgroundWorkerMethodDelegate backgroundWorker,
            HabaneroBackgroundWorker.UIWorkerMethodDelegate onSuccess,
            HabaneroBackgroundWorker.UIWorkerMethodDelegate onCancel,
            HabaneroBackgroundWorker.BackgroundWorkerExceptionHandlerDelegate onException)
        {
            return HabaneroBackgroundWorker.Run(new ActionDispatcher(uiControl), data, backgroundWorker, onSuccess, onCancel, onException);
        }
    }
}
