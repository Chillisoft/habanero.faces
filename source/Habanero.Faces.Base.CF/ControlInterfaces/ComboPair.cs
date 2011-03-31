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

namespace Habanero.Faces.Base
{
    


    /// <summary>
    /// Holds a key-value pair that provides a single item
    /// for a ComboBox.  The key is the string value shown and the value
    /// holds the underlying object, such as a BusinessObject or specific Guid
    /// </summary>
    public class ComboPair
    {
        private readonly string _key;
        private readonly object _value;

        ///<summary>
        /// The pair of values shown in the Combo Box (i.e. the Key, Value Pair) <see cref="ComboPair"/>
        ///</summary>
        ///<param name="key"></param>
        ///<param name="value"></param>
        public ComboPair(string key, object value)
        {
            _key = key;
            _value = value;
        }

        /// <summary>
        /// Gets the key to display to the user
        /// </summary>
        public string Key
        {
            get { return _key; }
        }

        /// <summary>
        /// Gets the underlying value being represented.  Typically
        /// this value might be a BusinessObject, a Guid or even the same
        /// string as that being shown to the user.
        /// </summary>
        public object Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Returns the key value being displayed
        /// </summary>
        public override string ToString()
        {
            return _key;
        }

        /// <summary>
        /// Indicates whether this ComboPair has exactly the same
        /// key and value as another
        /// </summary>
        /// <param name="obj">The ComboPair to compare with</param>
        /// <returns>Returns true if equal in content, false if not</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (this.GetType() != obj.GetType()) return false;

            ComboPair other = obj as ComboPair;
            if (other == null) return false;
            return String.Compare(other.Key, Key) == 0 && (other.Value == Value);
        }

        /// <summary>
        /// Returns the hashcode of the key and value
        /// </summary>
        public override int GetHashCode()
        {
            return Key.GetHashCode() | Value.GetHashCode();
        }

        /// <summary>
        /// Indicates if two ComboPairs have the same key and value
        /// </summary>
        public static bool operator ==(ComboPair v1, ComboPair v2)
        {
            if ((object)v1 == null)
                if ((object)v2 == null)
                    return true;
                else
                    return false;

            return (v1.Equals(v2));
        }

        /// <summary>
        /// Indicates if two ComboPairs differ in either their keys or values
        /// </summary>
        public static Boolean operator !=(ComboPair v1, ComboPair v2)
        {
            return !(v1 == v2);
        }

    }
}