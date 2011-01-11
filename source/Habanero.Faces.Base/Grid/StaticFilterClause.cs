using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Base;

namespace Habanero.Faces.Base.Grid
{
    /// <summary>
    /// Supplies you with a base class to build a static custom filter.
    /// This allows you to add filters to the filter control where these filters are not
    /// associated with a particular property in the filter control.
    /// A typicall example is where the grid must show only the Assets that have not been disposed off.
    /// </summary>
    public abstract class StaticCustomFilter : ICustomFilter
    {

        public virtual IControlHabanero Control
        {
            get { return null; }
        }

        public abstract IFilterClause GetFilterClause(IFilterClauseFactory filterClauseFactory);

        public virtual void Clear()
        {
        }

        public virtual string PropertyName
        {
            get { return ""; }
        }

        public virtual FilterClauseOperator FilterClauseOperator
        {
            get { return FilterClauseOperator.OpLike; }
        }
        // ReSharper disable UnusedMember.Local
        private void ToPreventCompilerWarnings()
        {
            ValueChanged(new object(),new EventArgs());
        }
        // ReSharper restore UnusedMember.Local

        /// <summary>
        /// Event handler that fires when the value in the Filter control changes.
        /// In this particular case it is never fired since the static filter is never changed.
        /// </summary>
        public event EventHandler ValueChanged;
    }
    /// <summary>
    /// Builds a <see cref="StringLiteralFilterClause"/> with the <see cref="StringLiteral"/>.
    /// See <see cref="StringLiteralFilterClause"/> for a detailed explanation.
    /// </summary>
    public class StringLiteralCustomFilter : StaticCustomFilter
    {
        /// <summary>
        /// Constructs with a defined <see cref="StringLiteral"/>
        /// </summary>
        /// <param name="stringLiteral"></param>
        public StringLiteralCustomFilter(string stringLiteral)
        {
            StringLiteral = stringLiteral;
        }
        /// <summary>
        /// Constructs without a <see cref="StringLiteral"/>.
        /// StringLiteral will b set to Empty
        /// </summary>
        public StringLiteralCustomFilter(): this(string.Empty)
        {
        }

        /// <summary>
        /// Returns the string literal that will be used by the <see cref="GetFilterClause"/>.
        /// to construct the <see cref="StringLiteralFilterClause"/>.
        /// </summary>
        public string StringLiteral { get; set; }

        ///<summary>
        /// Returns the filter clause for this control
        ///</summary>
        ///<param name="filterClauseFactory"></param>
        ///<returns></returns>
        public override IFilterClause GetFilterClause(IFilterClauseFactory filterClauseFactory)
        {
            return new StringLiteralFilterClause(StringLiteral);
        }
    }
    /// <summary>
    /// This is supplies a <see cref="IFilterClause"/> that can be supplied any 
    /// string literal. The GetFilterClauseString will then supply the <see cref="StringLiteral"/>
    /// exactly as is.<br/>
    /// This is typically used when you want to build a custom static filter clause which is more complex than
    /// a set of filters Anded. <br/>
    /// E.g. (ParentID is null Or ParentID != 'fdafdas') and AssetID != 'fdafdas').<br/>
    /// The potential limitations of using this is that you may limit the databases that this will port to.
    /// (In cases where the <see cref="IFilterControl"/>  is being used in <see cref="IFilterControl.FilterMode"/>
    /// = <see cref="FilterModes.Search"/>.<br/>
    /// This is typically used by the <see cref="StringLiteralCustomFilter"/> but can also be used independently 
    /// 
    /// <example><code>
    ///  public class ExcludeAssetCustomFilter : StaticCustomFilter
    ///  {
    ///      public override IFilterClause GetFilterClause(IFilterClauseFactory filterClauseFactory)
    ///      {
    ///          var stringLiteral = "";
    ///          if (AssetID != null)
    ///          {
    ///              stringLiteral = string.Format("AssetID &lt;&gt; '{0}'", AssetID.GetValueOrDefault().ToString("B"));
    ///          }
    ///          return new StringLiteralFilterClause(stringLiteral);
    ///      }
    ///
    ///      public Guid? AssetID { get; set; }
    ///  }
    /// </code></example>
    /// </summary>
    public class StringLiteralFilterClause : IFilterClause
    {
        /// <summary>
        /// Constructs with a prescribed string literal.
        /// </summary>
        /// <param name="stringLiteral"></param>
        public StringLiteralFilterClause(string stringLiteral)
        {
            StringLiteral = stringLiteral;
        }

        public string GetFilterClauseString()
        {
            return StringLiteral;
        }

        public string GetFilterClauseString(string stringLikeDelimiter, string dateTimeDelimiter)
        {
            return GetFilterClauseString();
        }
        /// <summary>
        /// Returns the string literal that will be used by the <see cref="GetFilterClauseString()"/>.
        /// </summary>
        public string StringLiteral { get; private set; }
    }
}
