using Habanero.Base;

namespace Habanero.Faces.Base.Async
{
    public interface ISupportAsyncLoadingObject : ISupportAsyncLoadingBase
    {
        IBusinessObject CurrentBusinessObject { get; set; }

        /// <summary>
        /// Asynchronous populate via callback
        /// </summary>
        /// <typeparam name="T">Business object type</typeparam>
        /// <param name="dataRetrieverCallback">Delegate callback which returns</param>
        void PopulateAsync<T>(DataRetrieverObjectDelegate dataRetrieverCallback) where T : class, IBusinessObject, new();
    }
}
