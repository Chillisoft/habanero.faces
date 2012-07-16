using Habanero.Base;

namespace Habanero.Faces.Base.Async
{
    public delegate IBusinessObjectCollection DataRetrieverCollectionDelegate();
    public interface IAsyncLoaderCollection<T> : IAsyncLoaderBase<T> where T: class, IBusinessObject, new()
    {
        DataRetrieverCollectionDelegate DataRetriever { get; set; }
    }
}
