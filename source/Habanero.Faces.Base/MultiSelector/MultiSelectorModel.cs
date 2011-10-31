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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Habanero.Base;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// The model for the multiselector control, which manages the lists of
    /// items in the multi-selector. The type of the items in 
    /// the lists is set by the template type.
    /// </summary>
    public class MultiSelectorModel<T>
    {
        private IList<T> _allOptions;
        private IList<T> _selectedOptions;
        /// <summary>
        /// Event raised when the available options have been changed
        /// </summary>
        public event EventHandler OptionsChanged;
        /// <summary>
        /// Event raised when the current selection has been changed.
        /// </summary>
        public event EventHandler SelectionsChanged;
        /// <summary>
        /// Event raised when a new option has been added
        /// </summary>
        public event EventHandler<ModelEventArgs<T>> OptionAdded;
        /// <summary>
        /// An event raised when an option has been removed
        /// </summary>
        public event EventHandler<ModelEventArgs<T>> OptionRemoved;
        /// <summary>
        /// An event raised when an option has been selected
        /// </summary>
        public event EventHandler<ModelEventArgs<T>> Selected;
        /// <summary>
        /// An event raised when an option has been deselected.
        /// </summary>
        public event EventHandler<ModelEventArgs<T>> Deselected;

#pragma warning disable 693
        /// <summary>
        /// The Event Arguements for the Multiselector Model
        /// </summary>
        /// <typeparam name="T">The object type used for the item</typeparam>
        public class ModelEventArgs<T> : EventArgs
#pragma warning restore 693
        {
            private readonly T _item;

            ///<summary>
            /// The constructor for the Event Arguements for the Multiselector Model
            ///</summary>
            ///<param name="item">The item in the model to which the event applies</param>
            public ModelEventArgs(T item)
            {
                _item = item;
            }

            /// <summary>
            /// Gets the affected item
            /// </summary>
            public T Item
            {
                get { return _item; }
            }
        }

        /// <summary>
        /// Constructor to initialise a new model
        /// </summary>
        public MultiSelectorModel()
        {
            OriginalSelections = new List<T>();
            _selectedOptions = new List<T>();
            _allOptions = new List<T>();
        }

        /// <summary>
        /// Sets the list of options (left hand side list).
        /// Note_ that this creates a shallow copy of the List.
        /// </summary>
        public IList<T> AllOptions
        {
            get { return _allOptions; }
            set
            {
                if (value == null)
                {
                    _allOptions = new List<T>();
                }
                else
                {
                    _allOptions = ShallowCopy(value);
                }
                if (_selectedOptions != null)
                {
                    for (int i = _selectedOptions.Count - 1; i >= 0; i--)
                    {
                        //Remove all selections that dont exist in the options list
                        if (!_allOptions.Contains(_selectedOptions[i]))
                        {
                            _selectedOptions.RemoveAt(i);
                        }
                    }
                }

                FireOptionsChanged();
            }
        }

        private void FireOptionsChanged()
        {
            if (OptionsChanged != null) OptionsChanged(this, new EventArgs());
        }

        /// <summary>
        /// Returns a view of the AllOptions collection
        /// </summary>
        public ReadOnlyCollection<T> OptionsView
        {
            get { return new ReadOnlyCollection<T>(_allOptions); }
        }
 
        /// <summary>
        /// Sets the list of selected items (right hand side list).
        /// </summary>
        public IList<T> SelectedOptions
        {
            get { return _selectedOptions; }
            set
            {
                _selectedOptions = value;
                if (value == null)
                {
                    _selectedOptions = new List<T>();
                }
                OriginalSelections = ShallowCopy(_selectedOptions);
                FireSelectionsChanged();
            }
        }
        
        private void FireSelectionsChanged()
        {
            if (SelectionsChanged != null) SelectionsChanged(this, new EventArgs());
        }

        /// <summary>
        /// Gets a view of the SelectedOptions collection
        /// </summary>
        public ReadOnlyCollection<T> SelectionsView
        {
            get
            {
                return new ReadOnlyCollection<T>(_selectedOptions);
            }
        }


        private IList<T> OriginalSelections { get; set; }

        /// <summary>
        /// Returns the list of available options, which is the set 
        /// of AllOptions minus the set of SelectedOptions
        /// </summary>
        public IList<T> AvailableOptions
        {
            get
            {
                IList<T> availableOptions = new List<T>();
                foreach (T item in _allOptions)
                {
                    if (!_selectedOptions.Contains(item))
                    {
                        availableOptions.Add(item);
//                        yield return item;
                    }
                }
                return availableOptions;
            }
        }

        /// <summary>
        /// Returns the list of added selections (items selected since 
        /// setting the selections)
        /// </summary>
        public IList<T> Added
        {
            get {
                IList<T> addedItems = new List<T>();
                foreach (T item in _selectedOptions)
                {
                    if (!OriginalSelections.Contains(item))
                    {
                        addedItems.Add(item);
                    }
                }
                return addedItems;
            }
        }

        /// <summary>
        /// Returns the list of removed selections (items deselected 
        /// since setting the selections)
        /// </summary>
        public IList<T> Removed
        {
            get
            {
                IList<T> removedItems = new List<T>();
                foreach (T item in OriginalSelections)
                {
                    if (!_selectedOptions.Contains(item))
                    {
                        removedItems.Add(item);
                    }
                }
                return removedItems;
            }
        }

        /// <summary>
        /// Selects multiple items at the same time.
        /// </summary>
        /// <param name="items">The list of items to select</param>
        public void Select(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                Select(item);
            }
        }

        /// <summary>
        /// Selects an option, removing it from the AllOptions and adding 
        /// it to the SelectedOptions
        /// </summary>
        public void Select(T item)
        {
            if (_allOptions.Contains(item) && !_selectedOptions.Contains(item))
            {
                if (_selectedOptions is IBusinessObjectCollection)
                {
                    IBusinessObjectCollection col = (IBusinessObjectCollection)_selectedOptions;
                    col.Add( (IBusinessObject) item);

                }
                else
                {
                    _selectedOptions.Add(item);
                }
                FireSelected(item);
            }
        }

        private void FireSelected(T item)
        {
            if (Selected != null) Selected(this, new ModelEventArgs<T>(item));
        }

        /// <summary>
        /// Deselects a list of items at once
        /// </summary>
        /// <param name="items">The list of items to deselect</param>
        public void Deselect(IList<T> items)
        {
            foreach (T item in items)
            {
                Deselect(item);
            }
        }

        /// <summary>
        /// Deselects an option, removing it from the SelectedOptions and 
        /// adding it to the AllOptions
        /// </summary>
        public void Deselect(T item)
        {
            if (_selectedOptions.Remove(item)) FireDeselected(item);
        }

        private void FireDeselected(T item)
        {
            if (Deselected != null) Deselected(this, new ModelEventArgs<T>(item));
        }

        /// <summary>
        /// Selects all available options
        /// </summary>
        public void SelectAll()
        {
            Select(AvailableOptions);
        }

        /// <summary>
        /// Deselects all options
        /// </summary>
        public void DeselectAll()
        {
            Deselect(ShallowCopy(_selectedOptions));
        }

        /// <summary>
        /// Adds an option to the collection of AllOptions
        /// </summary>
        /// <param name="item"></param>
        public void AddOption(T item)
        {
            _allOptions.Add(item);
            FireOptionAdded(item);
        }

        private void FireOptionAdded(T item)
        {
            if (OptionAdded != null) OptionAdded(this, new ModelEventArgs<T>(item));
        }

        ///<summary>
        /// Removes an option from the collection of options. 
        ///</summary>
        ///<param name="item">The item to remove</param>
        public void RemoveOption(T item)
        {
            Deselect(item);
            _allOptions.Remove(item);
            FireOptionRemoved(item);
        }

        private void FireOptionRemoved(T item)
        {
            if (OptionRemoved != null) OptionRemoved(this, new ModelEventArgs<T>(item));
        }

        private static IList<T> ShallowCopy(IList<T> list)
        {
            IList<T> newList = new List<T>(list.Count);
            foreach (T item in list)
            {
                newList.Add(item);
            }
            return newList;
        }
    }
}