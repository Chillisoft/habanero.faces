// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using System.Linq.Expressions;
using System.Reflection;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.Util;

namespace Habanero.Faces.Base
{
    ///<summary>
    /// This class solves the fairly generic problem of filtering one selector based on the selected item in another selector.
    /// The problem is when you have two selectors that are linked 
    ///     i.e. the data from the first selector filters the data in the second selector.
    ///      e.g. Select a Country and you get a filtered list of States for that country.
    ///</summary>
    ///<typeparam name="TParentType">The Type of the Parent Business Object(In our example the Country) </typeparam>
    ///<typeparam name="TChildType">The Type of the Child Business Object (in our example the State)</typeparam>
    public class BOColSelectorLinker<TParentType, TChildType> 
            where TChildType : class, IBusinessObject, new()
            where TParentType: IBusinessObject
    {
        ///<summary>
        /// The Parent Selector (in our example the "Countries").
        ///</summary>
        public IBOColSelector ParentSelector { get; private set; }

        /// <summary>
        /// The Child Selector (in our example the "States").
        /// </summary>
        public IBOColSelector ChildSelector { get; private set; }

        /// <summary>
        /// The name of the relationship that is linking these two Business Objects and hence these two ComboBoxes.
        /// in our example the "States" relationship on the Business Object "Country".
        /// </summary>
        public string RelationshipName { get; private set; }

        /// <summary>
        /// The Constructor
        /// </summary>
        /// <param name="parentSelector">The Parent Selector (in our example the "Countries").</param>
        /// <param name="childSelector">The Child Selector (in our example the "States").</param>
        /// <param name="relationshipName">The name of the relationship that is linking these two Business Objects and hence these two Selectors.
        ///   (in our example the "States" relationship on the Business Object "Country")</param>
        public BOColSelectorLinker(IBOColSelector parentSelector, IBOColSelector childSelector, string relationshipName)
        {
            if (parentSelector == null) throw new ArgumentNullException("parentSelector");
            if (childSelector == null) throw new ArgumentNullException("childSelector");
            if (string.IsNullOrEmpty(relationshipName)) throw new ArgumentNullException("relationshipName");
            ParentSelector = parentSelector;
            ChildSelector = childSelector;
            RelationshipName = relationshipName;
            parentSelector.BusinessObjectSelected += this.ParentComboBoxBusinessObjectSelected;
            this.Enabled = true;
        }

        private void ParentComboBoxBusinessObjectSelected(object sender, System.EventArgs e)
        {
            try
            {
                if(this.Enabled) UpdateChildSelectorCollection();
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        ///<summary>
        /// Force an update of the ChildSelector's collection from the ParentSelector's current SelectedBusinessObject
        ///</summary>
        public void UpdateChildSelectorCollection()
        {
            var selectedItem = ParentSelector.SelectedBusinessObject;
            var childSelectedItem = ChildSelector.SelectedBusinessObject;
            if (selectedItem == null || Convert.ToString(selectedItem) == "")
            {
                ChildSelector.BusinessObjectCollection = null;
                ChildSelector.ControlEnabled = false;
                return;
            }
            var selectedParentBusinessObject = (TParentType) selectedItem;
            ChildSelector.BusinessObjectCollection = GetBusinessObjectCollection(selectedParentBusinessObject);
            if (childSelectedItem != null && ChildSelector.BusinessObjectCollection.Contains(childSelectedItem))
            {
                ChildSelector.SelectedBusinessObject = childSelectedItem;
            }
            ChildSelector.ControlEnabled = HasCollectionSet;
        }

        private bool HasCollectionSet
        {
            get { return (ChildSelector.BusinessObjectCollection != null); }
        }
        /// <summary>
        /// Gets and Sets whether Linking is currently enabled. This can be used for fine grained control e.g. where
        /// the user would want to disable the linker during control loading etc.
        /// </summary>
        public bool Enabled { get; set; }

        protected virtual BusinessObjectCollection<TChildType> GetBusinessObjectCollection(TParentType selectedParentBusinessObject)
        {
            var childRelationship = GetChildRelationship(selectedParentBusinessObject);
            if (childRelationship == null) return null;
            return childRelationship.BusinessObjectCollection;
        }

        private MultipleRelationship<TChildType> GetChildRelationship(TParentType selectedParentBusinessObject)
        {
            return selectedParentBusinessObject.Relationships[RelationshipName] as MultipleRelationship<TChildType>;
        }
    }

}