using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habanero.Faces.Base
{
    public class AsyncSettings
    {
        public bool SynchroniseBackgroundOperations { get; set; }

        public AsyncSettings()
        {
            this.SynchroniseBackgroundOperations = false;
        }
    }
}
