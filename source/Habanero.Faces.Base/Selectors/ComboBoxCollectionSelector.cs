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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// This class provides mapping from a business object collection to a
    /// user interface ComboBox.  This mapper is used at code level when
    /// you are explicitly providing a business object collection.
    /// This Class is typically used by the <see cref="IBOComboBoxSelector"/> control.
    /// </summary>
    public class ComboBoxCollectionSelector : IBOColSelector
    {
        private readonly IControlFactory _controlFactory;
        private IBusinessObjectCollection _businessObjectCollection;

        /// <summary>
        /// A handler for the SelectedIndexChanged Event
        /// </summary>
        protected EventHandler _selectedIndexChanged;

        /// <summary>
        /// Constructor to create a new collection ComboBox mapper object.
        /// </summary>
        /// <param name="comboBox">The ComboBox object to map</param>
        /// <param name="controlFactory">The control factory used to create controls</param>
        public ComboBoxCollectionSelector(IComboBox comboBox, IControlFactory controlFactory) : this(comboBox, controlFactory, true)
        {
        }

        /// <summary>
        /// Constructor to create a new collection ComboBox mapper object.
        /// </summary>
        /// <param name="comboBox">The ComboBox object to map</param>
        /// <param name="controlFactory">The control factory used to create controls</param>
        /// <param name="autoSelectFirstItem"></param>
        public ComboBoxCollectionSelector(IComboBox comboBox, IControlFactory controlFactory, bool autoSelectFirstItem)
        {
            if (comboBox == null) throw new ArgumentNullException("comboBox");
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            _selectedIndexChanged = Control_OnSelectedIndexChanged;
            Control = comboBox;
            AutoSelectFirstItem = autoSelectFirstItem;
            _controlFactory = controlFactory;
            RegisterForControlEvents();
        }
        /// <summary>
        /// Registers this controller for the <see cref="IComboBox.SelectedIndexChanged"/> event.
        /// </summary>
        public void RegisterForControlEvents()
        {
            Control.SelectedIndexChanged += _selectedIndexChanged;
        }
        /// <summary>
        /// Deregisters this controller for the <see cref="IComboBox.SelectedIndexChanged"/> event.
        /// </summary>
        public void DeregisterForControlEvents()
        {
            Control.SelectedIndexChanged -= _selectedIndexChanged;
        }

        private void Control_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            FireBusinessObjectSelected();
        }

        /// <summary>
        /// Returns the collection used to populate the items shown in the ComboBox
        /// </summary>
        public IBusinessObjectCollection Collection
        {
            get { return _businessObjectCollection; }
        }

        /// <summary>
        /// Sets the collection being represented to a specific collection
        /// of business objects
        /// </summary>
        /// <param name="collection">The collection to represent</param>
        /// <param name="includeBlank">Whether to a put a blank item at the
        /// top of the list</param>
		public void SetCollection(IBusinessObjectCollection collection, bool includeBlank)
        {
            if (_businessObjectCollection != null)
            {
                _businessObjectCollection.BusinessObjectAdded -= BusinessObjectAddedHandler;
                _businessObjectCollection.BusinessObjectRemoved -= BusinessObjectRemovedHandler;
                _businessObjectCollection.BusinessObjectPropertyUpdated -= Collection_OnBusinessObjectPropUpdated;
            }
            _businessObjectCollection = collection;
            SetComboBoxCollectionPrivate(Control, Collection, includeBlank);
            if (Collection == null) return;
            Collection.BusinessObjectAdded += BusinessObjectAddedHandler;
            Collection.BusinessObjectRemoved += BusinessObjectRemovedHandler;
            Collection.BusinessObjectPropertyUpdated += Collection_OnBusinessObjectPropUpdated;
        }

        /// <summary>
        /// Set the list of objects in the ComboBox to a specific collection of
        /// business objects.<br/>
        /// Important: If you are changing the business object collection,
        /// use the SetBusinessObjectCollection method instead, which will call this method
        /// automatically.
        /// </summary>
        /// <param name="cbx">The ComboBox being controlled</param>
        /// <param name="col">The business object collection used to populate the items list</param>
        /// <param name="includeBlank">Whether to include a blank item at the
        /// top of the list</param>
        private void SetComboBoxCollectionPrivate(IComboBox cbx, IBusinessObjectCollection col, bool includeBlank)
        {
            int width = cbx.Width;

            IBusinessObject selectedBusinessObject = SelectedBusinessObject;

            cbx.SelectedIndex = -1;
            cbx.Text = null;
            cbx.Items.Clear();
            int numBlankItems = 0;
            if (includeBlank)
            {
                cbx.Items.Add("");
                numBlankItems++;
            }
            if (col == null) return;

            //This is a bit of a hack but is used to get the 
            //width of the dropdown list when it drops down
            // uses the preferedwith calculation on the 
            //Label to do this. Makes drop down width equal to the 
            // width of the longest name shown.
            ILabel lbl = _controlFactory.CreateLabel("", false);
            foreach (IBusinessObject businessObject in col)
            {
                lbl.Text = businessObject.ToString();
                if (lbl.PreferredWidth > width)
                {
                    width = lbl.PreferredWidth;
                }
                cbx.Items.Add(businessObject);
            }
            if (PreserveSelectedItem)
            {
                SelectedBusinessObject = col.Contains(selectedBusinessObject) ? selectedBusinessObject : null;
            }
            else if (col.Count > 0 && AutoSelectFirstItem) cbx.SelectedIndex = numBlankItems;
            if (width == 0) width = 1;
            cbx.DropDownWidth = width > cbx.Width ? width : cbx.Width;
        }
        
        private void Collection_OnBusinessObjectPropUpdated(object sender, BOPropUpdatedEventArgs e)
        {
            IBusinessObject businessObject = e.BusinessObject;
            int selectedIndex = this.Control.SelectedIndex;
            int indexOf = this.Control.Items.IndexOf(businessObject);
            if (indexOf == -1) return;
            this.Control.Items.Remove(businessObject);
            this.Control.Items.Insert(indexOf, businessObject);
            this.Control.SelectedIndex = selectedIndex;
        }

        /// <summary>
        /// Event Occurs when a business object is selected
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectSelected;
                    
        
        private void FireBusinessObjectSelected()
        {
            if (this.BusinessObjectSelected != null)
            {
                this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
            }
        }

        /// <summary>
        /// This handler is called when a business object has been removed from
        /// the collection - it subsequently removes the item from the ComboBox
        /// list as well.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectRemovedHandler(object sender, BOEventArgs e)
        {
            Control.Items.Remove(e.BusinessObject);
        }

        /// <summary>
        /// This handler is called when a business object has been added to
        /// the collection - it subsequently adds the item to the ComboBox
        /// list as well.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectAddedHandler(object sender, BOEventArgs e)
        {
            IComboBoxObjectCollection items = Control.Items;
            IBusinessObject businessObject = e.BusinessObject;
            if (businessObject == null) return;

//            log.Debug("BusinessObjectAddedHandler : ComboItems.Count = " + items.Count);
            if (items.Contains(businessObject)) return;
            if (string.IsNullOrEmpty(businessObject.ToString()))
            {
                string message = string.Format("Cannot add a business object of type '{0}' to the '{1}' if its ToString is null or zero length", businessObject.ClassDef.ClassName, this.GetType().Name);
                throw new HabaneroDeveloperException(message, message);
            }

            Control.Items.Add(e.BusinessObject);
        }

        /// <summary>
        /// Gets and Sets the business object collection displayed in the grid.  This
        /// collection must be pre-loaded using the collection's Load() command or from the
        /// <see cref="IBusinessObjectLoader"/>.
        /// The default UI definition will be used, that is a 'ui' element 
        /// without a 'name' attribute.
        /// </summary>
        public IBusinessObjectCollection BusinessObjectCollection
        {
            get { return _businessObjectCollection; }
            set { SetCollection(value, true); }
        }

        /// <summary>
        /// Returns the business object, in object form, that is currently 
        /// selected in the ComboBox list, or null if none is selected
        /// </summary>
        public IBusinessObject SelectedBusinessObject
        {
            get
            {
                if (NoItemSelected() || NullItemSelected()) return null;

                return Control.SelectedItem as IBusinessObject;
            }
            set
            {
                Control.SelectedItem = ContainsValue(value) ? value : null;
                if (Control.SelectedItem == null) Control.Text = null;
            }
        }

        private bool ContainsValue(IBusinessObject value)
        {
            return (value != null && Control.Items.Contains(value));
        }

        private bool NullItemSelected()
        {
            return Control.SelectedItem is string && (Control.SelectedItem == null || (string)Control.SelectedItem == "");
        }

        private bool NoItemSelected()
        {
            return Control.SelectedIndex == -1;
        }

        /// <summary>
        /// Returns the ComboBox control
        /// </summary>
        public IComboBox Control { get; private set; }
        /// <summary>
        /// Must the first combo box item be auto selected or not. If it is autoselect then this item will be shown as selected
        /// when the combo box is loaded and the Selected event will be fired.
        /// </summary>
        public bool AutoSelectFirstItem { get; set; }

        /// <summary>Gets the number of items displayed in the <see cref="IBOColSelector"></see>.</summary>
        /// <returns>The number of items in the <see cref="IBOColSelector"></see>.</returns>
        public int NoOfItems
        {
            get { return Control.Items.Count; }
        }

        /// <summary>
        /// Returns the business object at the specified row number
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        public IBusinessObject GetBusinessObjectAtRow(int row)
        {
            if (IndexOutOfRange(row)) return null;
            return (IBusinessObject)Control.Items[row];
        }

        private bool IndexOutOfRange(int row)
        {
            return row < 0 || row >= NoOfItems;
        }

        /// <summary>
        /// Returns the control factory used to generate controls
        /// such as the label
        /// </summary>
        public IControlFactory ControlFactory
        {
            get { return _controlFactory; }
        }

        ///<summary>
        /// Clears all items in the Combo Box and sets the selected item and <see cref="Collection"/>
        /// to null
        ///</summary>
        public void Clear()
        {
            SetCollection (null, false);
        }

        ///<summary>
        /// Gets or sets whether the current <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see> should be preserved in the selector when the 
        /// <see cref="IBOColSelectorControl.BusinessObjectCollection">BusinessObjectCollection</see> 
        /// is changed to a new collection which contains the current <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see>.
        /// If the <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see> doesn't exist in the new collection then the
        /// <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see> is set to null.
        /// If the current <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see> is null then this will also be preserved.
        /// This overrides the <see cref="IBOColSelectorControl.AutoSelectFirstItem">AutoSelectFirstItem</see> property.
        ///</summary>
        public bool PreserveSelectedItem { get; set; }
    }
}
