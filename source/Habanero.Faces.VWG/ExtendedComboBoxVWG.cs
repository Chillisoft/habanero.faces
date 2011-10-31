#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
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