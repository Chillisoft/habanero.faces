using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Base;

namespace Habanero.Faces.Base.Async
{
    public interface IAsyncLoaderBase<T> where T: class, IBusinessObject, new()
    {
        Criteria Criteria { get; set; }
        void FetchAsync();
    }
}
