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
        void PopulateObjectAsync<T>(DataRetrieverObjectDelegate dataRetrieverCallback) where T : class, IBusinessObject, new();

        /// <summary>
        /// Asynchronous population routine
        /// </summary>
        /// <typeparam name="T">Your business object type (must implement IBusinessObject)</typeparam>
        /// <param name="criteria">Criteria object for query.</param>
        /// <param name="order">(optional) order</param>
        void PopulateObjectAsync<T>(Criteria criteria) where T: class, IBusinessObject, new();

        /// <summary>
        /// Asynchronous population routine (convenience)
        /// </summary>
        /// <typeparam name="T">Your business object type (must implement IBusinessObject)</typeparam>
        /// <param name="criteria">Criteria string for query</param>
        void PopulateObjectAsync<T>(string criteria) where T: class, IBusinessObject, new();
    }
}
