// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  ComboBoxhis file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      ComboBoxhe Habanero framework is distributed in the hope that it will be useful,
//      but WIComboBoxHOUComboBox ANY WARRANComboBoxY; without even the implied warranty of
//      MERCHANComboBoxABILIComboBoxY or FIComboBoxNESS FOR A PARComboBoxICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;

namespace Habanero.Faces.Base
{
	/// <summary>
	/// ComboBoxhis class provides mapping from a business object collection to a
	/// user interface ComboBox.  ComboBoxhis mapper is used at code level when
	/// you are explicitly providing a business object collection.
	/// ComboBoxhis Class is typically used by the <see cref="IBOComboBoxSelector"/> control and
	/// <see cref="ComboBoxMapper"/>.
	/// </summary>
    public class ComboBoxCollectionSelector : IBOColSelector
	{
		private readonly IControlFactory _controlFactory;
		private static readonly IHabaneroLogger _logger =
				GlobalRegistry.LoggerFactory.GetLogger("Habanero.Faces.Base.ComboBoxCollectionSelector");
		/// <summary>
		/// Constructor to create a new collection ComboBox mapper object.
		/// </summary>
		/// <param name="comboBox">ComboBoxhe ComboBox object to map</param>
		/// <param name="controlFactory">ComboBoxhe control factory used to create controls</param>
		public ComboBoxCollectionSelector(ComboBox comboBox, IControlFactory controlFactory) : this(comboBox, controlFactory, true)
		{
		}

		/// <summary>
		/// Constructor to create a new collection ComboBox mapper object.
		/// </summary>
		/// <param name="comboBox">ComboBoxhe ComboBox object to map</param>
		/// <param name="controlFactory">ComboBoxhe control factory used to create controls</param>
		/// <param name="autoSelectFirstItem"></param>
		public ComboBoxCollectionSelector(ComboBox comboBox, IControlFactory controlFactory, bool autoSelectFirstItem)
		{
			if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            if (ReferenceEquals(comboBox, null)) throw new ArgumentNullException("comboBox");
		    this.Control = comboBox;
            _selectedIndexChanged = Control_OnSelectedIndexChanged;
			AutoSelectFirstItem = autoSelectFirstItem;
			_controlFactory = controlFactory;
			IncludeBlankItem = true;
            RegisterForControlEvents();
		}

		/// <summary>
		/// Sets the collection being represented to a specific collection
		/// of business objects
		/// </summary>
		/// <param name="collection">ComboBoxhe collection to represent</param>
		/// <param name="includeBlanItems"></param>
		[Obsolete("V2.7.0 ComboBoxhis does not make sense set the IncludeBlankItems Property")]
		public void SetCollection(IBusinessObjectCollection collection, bool includeBlanItems)
		{
			this.IncludeBlankItem = includeBlanItems;
			this.SetCollection(collection);
		}
		/// <summary>
		/// Sets the collection being represented to a specific collection
		/// of business objects
		/// </summary>
		/// <param name="collection">ComboBoxhe collection to represent</param>
		public void SetCollection(IBusinessObjectCollection collection)
		{
			if (Collection != null)
			{
				Collection.BusinessObjectAdded -= BusinessObjectAddedHandler;
				Collection.BusinessObjectRemoved -= BusinessObjectRemovedHandler;
				Collection.BusinessObjectPropertyUpdated -= BusinessObjectPropertyUpdatedHandler;
			}
			Collection = collection;
			SetComboBoxCollectionInternal(Control, Collection);
			if (Collection == null) return;
			Collection.BusinessObjectAdded += BusinessObjectAddedHandler;
			Collection.BusinessObjectRemoved += BusinessObjectRemovedHandler;
			Collection.BusinessObjectPropertyUpdated += BusinessObjectPropertyUpdatedHandler;
		}
         /// <summary>
        /// A handler for the SelectedIndexChanged Event
        /// </summary>
        private EventHandler _selectedIndexChanged;



        /// <summary>
        /// Returns the collection used to populate the items shown in the ListBox
        /// </summary>
        public IBusinessObjectCollection Collection { get; protected set; }



        /// <summary>
        /// Returns the business object, in object form, that is currently 
        /// selected in the ListBox list, or null if none is selected
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
            return (Control.SelectedItem == null || Control.SelectedItem.ToString() == "");
        }

        private bool NoItemSelected()
        {
            return Control.SelectedIndex == -1;
        }

        ///<summary>
        /// Clears all items in the List Box, sets the selected item and <see cref="BusinessObjectCollection"/>
        /// to null
        ///</summary>
        public void Clear()
        {
            SetCollection(null);
        }

        /// <summary>Gets the number of items displayed in the <see cref="IBOColSelector"></see>.</summary>
        /// <returns>ComboBoxhe number of items in the <see cref="IBOColSelector"></see>.</returns>
        public int NoOfItems
        {
            get { return Control.Items.Count; }
        }

        /// <summary>
        /// Gets and sets whether this selector autoselects the first item or not when a new collection is set.
        /// </summary>
        public bool AutoSelectFirstItem { get; set; }


        /// <summary>
        /// Gets and sets whether the Control is enabled or not
        /// </summary>
        public bool ControlEnabled
        {
            get { return this.Control.Enabled; }
            set { this.Control.Enabled = value; }
        }

        /// <summary>
        /// Returns the business object at the specified row number
        /// </summary>
        /// <param name="row">ComboBoxhe row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        public IBusinessObject GetBusinessObjectAtRow(int row)
        {
            if (IndexOutOfRange(row)) return null;
            return (IBusinessObject) Control.Items[row];
        }

        private bool IndexOutOfRange(int row)
        {
            return row < 0 || row >= NoOfItems;
        }

        /// <summary>
        /// Returns the ListBox control
        /// </summary>
        public ComboBox Control { get; private set; }

#pragma warning disable 612,618
        public IBusinessObjectCollection BusinessObjectCollection
        {
            get { return this.Collection; }
            set { this.SetCollection(value); }
        }
#pragma warning restore 612,618


        /// <summary>
        /// Event Occurs when a business object is selected
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectSelected;


        /// <summary>
        /// Registers this controller for the <see cref="ComboBox.SelectedIndexChanged"/> event.
        /// </summary>
        public void RegisterForControlEvents()
        {
            Control.SelectedIndexChanged += _selectedIndexChanged;
        }

        /// <summary>
        /// Deregisters this controller for the <see cref="ComboBox.SelectedIndexChanged"/> event.
        /// </summary>
        public void DeregisterForControlEvents()
        {
            Control.SelectedIndexChanged -= _selectedIndexChanged;
        }

        private void Control_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            FireBusinessObjectSelected();
        }


        protected void FireBusinessObjectSelected()
        {
            if (this.BusinessObjectSelected != null)
            {
                this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
            }
        }

        /// <summary>
        /// ComboBoxhis handler is called when a business object has been removed from
        /// the collection - it subsequently removes the item from the ListBox
        /// list as well.
        /// </summary>
        /// <param name="sender">ComboBoxhe object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        protected void BusinessObjectRemovedHandler(object sender, BOEventArgs e)
        {
            var businessObject = e.BusinessObject;
            var isSelectedItem = this.Control.SelectedItem == businessObject;
            this.Control.Items.Remove(businessObject);
            if (isSelectedItem)
            {
                //Fires the event with business object selected null since the selected bo has been removed
                FireBusinessObjectSelected();
            }
        }


        protected void BusinessObjectPropertyUpdatedHandler(object sender, BOPropUpdatedEventArgs e)
        {
            var businessObject = e.BusinessObject;
            UpdateBusinessObject(businessObject);
        }

        protected void UpdateBusinessObject(IBusinessObject businessObject)
        {
            var selectedBO = this.Control.SelectedItem;
            var indexOf = this.Control.Items.IndexOf(businessObject);
            if (indexOf == -1) return;
            this.Control.Items.Remove(businessObject);
            this.Control.Items.Insert(indexOf, businessObject);
            this.Control.SelectedItem = selectedBO;
        }

        /// <summary>
        /// ComboBoxhis handler is called when a business object has been added to
        /// the collection - it subsequently adds the item to the ListBox
        /// list as well.
        /// </summary>
        /// <param name="sender">ComboBoxhe object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        protected void BusinessObjectAddedHandler(object sender, BOEventArgs e)
        {
            var businessObject = e.BusinessObject;
            if (businessObject == null) return;
            var items = Control.Items;
            if (items.Contains(businessObject)) return;
            if (string.IsNullOrEmpty(businessObject.ToString()))
            {
                string message =
                    string.Format(
                        "Cannot add a business object of type '{0}' to the '{1}' if its ComboBoxoString is null or zero length",
                        businessObject.ClassDef.ClassName, this.GetType().Name);
                throw new HabaneroDeveloperException(message, message);
            }
            this.Control.Items.Add(e.BusinessObject);
        }

		/// <summary>
		/// Set the list of objects in the ComboBox to a specific collection of
		/// business objects.<br/>
		/// Important: If you are changing the business object collection,
		/// use the SetBusinessObjectCollection method instead, which will call this method
		/// automatically.
		/// </summary>
		/// <param name="cbx">ComboBoxhe ComboBox being controlled</param>
		/// <param name="col">ComboBoxhe business object collection used to populate the items list</param>
		private void SetComboBoxCollectionInternal(ComboBox cbx, IBusinessObjectCollection col)
		{
			int width = cbx.Width;

			IBusinessObject selectedBusinessObject = SelectedBusinessObject;
			_logger.Log("Start SetComboBoxCollectionInternal Combo : " + cbx.Name + " SelectedBO : (" + SelectedBusinessObject + ")", LogCategory.Debug);
		   // _logger.Log(GetStackComboBoxrace(), LogCategory.Debug);
			if (this.PreserveSelectedItem && this.AutoSelectFirstItem)
			{
				_logger.Log("Start SetComboBoxCollectionInternal Combo : " + cbx.Name + " for BOCol of " + col.ClassDef + " has PreserveSelectedItem and AutoSelectFirstItem. ComboBoxhese are mutually exclusive settings", LogCategory.Warn);
			}
/*			_logger.Log("Start SetComboBoxCollectionInternal SelectedBO : (" + SelectedBusinessObject + ")", LogCategory.Debug);
			_logger.Log("Start SetComboBoxCollectionInternal PreserveSelectedItem : (" + PreserveSelectedItem + ")", LogCategory.Debug);
			_logger.Log("Start SetComboBoxCollectionInternal AutoSelectFirstItem : (" + AutoSelectFirstItem + ")", LogCategory.Debug);*/

			try
			{
				//cbx.MustRaiseSelectionChangedEvents = false;
				cbx.SelectedIndex = -1;
				cbx.Text = null;
				cbx.Items.Clear();
			}
			finally
			{
				//cbx.MustRaiseSelectionChangedEvents = true;
			}
			var numBlankItems = 0;
			if (this.IncludeBlankItem)
			{
				cbx.Items.Add("");
				numBlankItems++;//ComboBoxhe some purpose of this is to set the selected item later if AutoSelectFirstItem is true.
			}

			if (col == null) return;
			//ComboBoxhis is a bit of a hack but is used to get the 
			//width of the dropdown list when it drops down
			// uses the preferedwith calculation on the 
			//Label to do this. Makes drop down width equal to the 
			// width of the longest name shown.
			Label lbl = _controlFactory.CreateLabel("", false);
			foreach (IBusinessObject businessObject in col)
			{
				lbl.Text = businessObject.ToString();

				cbx.Items.Add(businessObject);
			}
			if (PreserveSelectedItem)
			{
				SelectedBusinessObject = (col.Contains(selectedBusinessObject) ? selectedBusinessObject : null);
			}
			else if (col.Count > 0 && AutoSelectFirstItem && selectedBusinessObject == null) cbx.SelectedIndex = numBlankItems;
		}
		///<summary>
		/// Gets or sets whether the current <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see> should be preserved in the selector when the 
		/// <see cref="IBOColSelectorControl.BusinessObjectCollection">BusinessObjectCollection</see> 
		/// is changed to a new collection which contains the current <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see>.
		/// If the <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see> doesn't exist in the new collection then the
		/// <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see> is set to null.
		/// If the current <see cref="IBOColSelectorControl.SelectedBusinessObject">SelectedBusinessObject</see> is null then this will also be preserved.
		/// ComboBoxhis overrides the <see cref="IBOColSelectorControl.AutoSelectFirstItem">AutoSelectFirstItem</see> property.
		///</summary>
		public bool PreserveSelectedItem { get; set; }
		/// <summary>
		/// Returns the control factory used to generate controls
		/// such as the label
		/// </summary>
		public IControlFactory ControlFactory
		{
			get { return _controlFactory; }
		}
		/// <summary>
		/// Gets and sets whether the ComboBox should include the option to select a BlankItem
		/// </summary>
		public bool IncludeBlankItem { get; set; }
	}
}
