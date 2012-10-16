using Habanero.Faces.Base;

namespace Habanero.Faces.VWG
{
    /// <summary>
    /// A Text Box with a Search Button. This is typically used for cases where there is a large list of potential items and it is 
    /// not appropriate to use a ComboBox for selecting the items.
    /// </summary>
    public class ExtendedComboBoxVWG : UserControlVWG, IExtendedComboBox
    {
        private readonly IControlFactory _controlFactory;

        /// <summary>
        /// Constructor with an unspecified Control Factory.
        /// </summary>
        public ExtendedComboBoxVWG(): this(GlobalUIRegistry.ControlFactory)
        {
        }

        ///<summary>
        /// Constructor with a specified Control Factory
        ///</summary>
        ///<param name="controlFactory"></param>
        public ExtendedComboBoxVWG(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            IUserControlHabanero userControlHabanero = this;
            ComboBox = _controlFactory.CreateComboBox();
            Button = _controlFactory.CreateButton("...");
            BorderLayoutManager borderLayoutManager = controlFactory.CreateBorderLayoutManager(userControlHabanero);
            borderLayoutManager.AddControl(ComboBox, BorderLayoutManager.Position.Centre);
            borderLayoutManager.AddControl(Button, BorderLayoutManager.Position.East);
        }

        ///<summary>
        /// Returns the <see cref="IExtendedComboBox.ComboBox"/> in the control
        ///</summary>
        public IComboBox ComboBox { get; private set; }

        ///<summary>
        /// Returns the <see cref="IExtendedComboBox.Button"/> in the control
        ///</summary>
        public IButton Button { get; private set; }
    }
}