using System;
using Habanero.Base;

namespace Habanero.Faces.Base.Async
{
    public delegate IBusinessObject DataRetrieverObjectDelegate();
    public interface ISupportAsyncLoadingObject : ISupportAsyncLoadingBase, IHasCurrentBusinessObject
    {

        /// <summary>
        /// Asynchronous populate via callback
        /// </summary>
        /// <typeparam name="T">Business object type</typeparam>
        /// <param name="dataRetrieverCallback">Delegate callback which returns the IBusinessObjectCollection to be used by the grid</param>
        /// <param name="afterPopulation">(optional) Action to be called after population completes without exception. Will be called on the UI thread.</param>
        void PopulateObjectAsync<T>(DataRetrieverObjectDelegate dataRetrieverCallback, Action afterPopulation) where T : class, IBusinessObject, new();

        /// <summary>
        /// Asynchronous population routine
        /// </summary>
        /// <typeparam name="T">Your business object type (must implement IBusinessObject)</typeparam>
        /// <param name="criteria">Criteria object for query.</param>
        /// <param name="order">(optional) order</param>
        /// <param name="afterPopulation">(optional) Action to be called after population completes without exception. Will be called on the UI thread.</param>
        void PopulateObjectAsync<T>(Criteria criteria, Action afterPopulation = null) where T: class, IBusinessObject, new();

        /// <summary>
        /// Asynchronous population routine (convenience)
        /// </summary>
        /// <typeparam name="T">Your business object type (must implement IBusinessObject)</typeparam>
        /// <param name="criteria">Criteria string for query</param>
        /// <param name="afterPopulation">(optional) Action to be called after population completes without exception. Will be called on the UI thread.</param>
        void PopulateObjectAsync<T>(string criteria, Action afterPopulation = null) where T: class, IBusinessObject, new();
    }
}
