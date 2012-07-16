using Habanero.Base;

namespace Habanero.Faces.Base.Async
{
    public interface ISupportAsyncLoadingCollection : ISupportAsyncLoadingBase, IHasBusinessObjectCollection
    {
        /// <summary>
        /// Provides a mechanism for the developer to provide a callback for the data collection
        /// of the asynchronous loader
        /// </summary>
        /// <typeparam name="T">Type of business object to load with</typeparam>
        /// <param name="dataRetrieverCallback">delegate method to do the actual data retrieval.
        /// This delegate will not be called on the UI thread so be wary of UI threading concerns</param>
        void PopulateCollectionAsync<T>(DataRetrieverCollectionDelegate dataRetrieverCallback) where T : class, IBusinessObject, new();

        /// <summary>
        /// Asynchronous population routine
        /// </summary>
        /// <typeparam name="T">Your business object type (must implement IBusinessObject)</typeparam>
        /// <param name="criteria">Criteria object for query.</param>
        /// <param name="order">(optional) order</param>
        void PopulateCollectionAsync<T>(Criteria criteria, IOrderCriteria order = null) where T: class, IBusinessObject, new();

        /// <summary>
        /// Asynchronous population routine (convenience)
        /// </summary>
        /// <typeparam name="T">Your business object type (must implement IBusinessObject)</typeparam>
        /// <param name="criteria">Criteria string for query</param>
        void PopulateCollectionAsync<T>(string criteria, string order = null) where T: class, IBusinessObject, new();
    }
}
