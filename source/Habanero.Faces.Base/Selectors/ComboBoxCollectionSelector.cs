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
	/// This Class is typically used by the <see cref="IBOComboBoxSelector"/> control and
	/// <see cref="ComboBoxMapper"/>.
	/// </summary>
	public class ComboBoxCollectionSelector : ListControlCollectionManager<IComboBox>
	{
		private readonly IControlFactory _controlFactory;

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
		public ComboBoxCollectionSelector(IComboBox comboBox, IControlFactory controlFactory, bool autoSelectFirstItem):base(comboBox)
		{
			if (controlFactory == null) throw new ArgumentNullException("controlFactory");
			AutoSelectFirstItem = autoSelectFirstItem;
			_controlFactory = controlFactory;
			IncludeBlankItem = true;
		}

		/// <summary>
		/// Sets the collection being represented to a specific collection
		/// of business objects
		/// </summary>
		/// <param name="collection">The collection to represent</param>
		/// <param name="includeBlanItems"></param>
		[Obsolete("V2.7.0 This does not make sense set the IncludeBlankItems Property")]
		public void SetCollection(IBusinessObjectCollection collection, bool includeBlanItems)
		{
			this.IncludeBlankItem = includeBlanItems;
			this.SetCollection(collection);
		}
		/// <summary>
		/// Sets the collection being represented to a specific collection
		/// of business objects
		/// </summary>
		/// <param name="collection">The collection to represent</param>
		public override void SetCollection(IBusinessObjectCollection collection)
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
		/// Set the list of objects in the ComboBox to a specific collection of
		/// business objects.<br/>
		/// Important: If you are changing the business object collection,
		/// use the SetBusinessObjectCollection method instead, which will call this method
		/// automatically.
		/// </summary>
		/// <param name="cbx">The ComboBox being controlled</param>
		/// <param name="col">The business object collection used to populate the items list</param>
		private void SetComboBoxCollectionInternal(IComboBox cbx, IBusinessObjectCollection col)
		{
			int width = cbx.Width;

			IBusinessObject selectedBusinessObject = SelectedBusinessObject;

			cbx.SelectedIndex = -1;
			cbx.Text = null;
			cbx.Items.Clear();
			int numBlankItems = 0;
			if (this.IncludeBlankItem)
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
