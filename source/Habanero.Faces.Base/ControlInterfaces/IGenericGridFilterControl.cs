using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habanero.Faces.Base.ControlInterfaces
{
    public interface IGenericGridFilterControl: IPanel
    {
        EventHandler FilterStarted { get; set; }
        EventHandler FilterCompleted { get; set; }

        IGridBase Grid { get; set; }
        string FilterText { get; set; }
    }
}
