using Habanero.Base;

namespace Habanero.Faces.Base.Async
{
    public interface IHasCurrentBusinessObject
    {
        /// <summary>
        /// Returns the business object represented in the currently
        /// selected tab page
        /// </summary>
        IBusinessObject CurrentBusinessObject { get; set; }

    }
}
