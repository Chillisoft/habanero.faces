using System;
using Habanero.Base;

namespace Habanero.Faces.Base.Async
{
    public interface ISupportAsyncLoadingBase
    {
        IAsyncResult BeginInvoke(Delegate method);
        bool Enabled { get; set; }
        EventHandler AsyncOperationComplete { get; set; }
        EventHandler AsyncOperationStarted { get; set; }

        /// <summary>
        /// Asynchronous population routine
        /// </summary>
        /// <typeparam name="T">Your business object type (must implement IBusinessObject)</typeparam>
        /// <param name="criteria">Criteria object for query.</param>
        /// <param name="order">(optional) order</param>
        void PopulateAsync<T>(Criteria criteria, IOrderCriteria order = null) where T: class, IBusinessObject, new();

        /// <summary>
        /// Asynchronous population routine (convenience)
        /// </summary>
        /// <typeparam name="T">Your business object type (must implement IBusinessObject)</typeparam>
        /// <param name="criteria">Criteria string for query</param>
        void PopulateAsync<T>(string criteria, string order) where T: class, IBusinessObject, new();

    }
}
