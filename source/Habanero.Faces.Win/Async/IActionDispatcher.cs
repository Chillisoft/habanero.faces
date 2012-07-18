using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habanero.Faces.Win.Async
{
    public interface IActionDispatcher
    {
        void Dispatch(Action method);
    }
}
