using Habanero.Base;

namespace Habanero.Faces.Base.Async
{
    public interface ISupportAsyncLoadingCollection : ISupportAsyncLoadingBase
    {
        IBusinessObjectCollection BusinessObjectCollection { get; set; }
        /// <summary>
        /// Provides a mechanism for the developer to provide a callback for the data collection
        /// of the asynchronous loader
        /// </summary>
        /// <typeparam name="T">Type of business object to load with</typeparam>
        /// <param name="dataRetrieverCallback">delegate method to do the actual data retrieval.
        /// This delegate will not be called on the UI thread so be wary of UI threading concerns</param>
        void PopulateAsync<T>(DataRetrieverCollectionDelegate dataRetrieverCallback) where T : class, IBusinessObject, new();
    }
}
