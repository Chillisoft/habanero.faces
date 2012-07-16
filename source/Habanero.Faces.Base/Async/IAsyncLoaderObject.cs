using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Base;

namespace Habanero.Faces.Base.Async
{
    public delegate IBusinessObject DataRetrieverObjectDelegate();
    interface IAsyncLoaderObject<T> : IAsyncLoaderBase<T> where T: class, IBusinessObject, new()
    {
        DataRetrieverObjectDelegate DataRetriever { get; set; }
    }
}
