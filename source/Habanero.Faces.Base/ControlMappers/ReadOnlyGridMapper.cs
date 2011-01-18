namespace Habanero.Faces.Base
{
    /// <summary>
    /// Maps a multiple relationship collection onto an <see cref="IEditableGridControl"/>
    /// This is used for editing a Many relationship e.g. If an Invoice has many items
    /// then this control mapper can be used to provide an editable grid of Invoice Items.
    /// </summary>
    public class ReadOnlyGridMapper : ControlMapper
    {
        private readonly IReadOnlyGrid _readOnlyGrid;
        private readonly IGridInitialiser _gridInitialiser;

        /// <summary>
        /// Constructor for the mapper.
        /// </summary>
        /// <param name="ctl">The IEditableGridControl</param>
        /// <param name="relationshipName">This is the relationship name to use - this relationship must be a multiple relationship and exist on the BusinessObject</param>
        /// <param name="isReadOnly">Whether the editable grid should be read only or not. Ignored</param>
        /// <param name="factory">The control factory to use</param>
        public ReadOnlyGridMapper(IReadOnlyGrid ctl, string relationshipName, bool isReadOnly, IControlFactory factory)
            : base(ctl, relationshipName, isReadOnly, factory)
        {
            _readOnlyGrid = ctl;
            _gridInitialiser = new GridBaseInitialiser(ctl, factory);
        }

        /// <summary>
        /// Updates the properties on the represented business object
        /// </summary>
        public override void ApplyChangesToBusinessObject() { }

        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="IControlMapper.BusinessObject"/>
        /// </summary>
        protected override void InternalUpdateControlValueFromBo()
        {
            if (_businessObject != null)
            {
                var relationship = _businessObject.Relationships[PropertyName];
                //if grid has columns then should not initialise.
                _gridInitialiser.InitialiseGrid(relationship.RelatedObjectClassDef);
                _readOnlyGrid.BusinessObjectCollection =_businessObject.Relationships.GetRelatedCollection(PropertyName);
            } else
            {
                _readOnlyGrid.BusinessObjectCollection = null;
            }
        }
    }
}