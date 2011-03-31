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
namespace Habanero.Faces.Base
{

    ///<summary>
    /// Provides a common control interface that is specialised for showing a collection of 
    /// Business Objects and allowing the user to select one (Or in some cases more than one)
    /// The common controls used for selecting business objects are ComboBox, ListBox, ListView, Grid,
    ///  <see cref="ICollapsiblePanelGroupControl"/>, <see cref="IBOColTabControl"/>, a <see cref="IMultiSelector{T}"/>
    ///  or an <see cref="ITreeView"/>
    ///</summary>
    public interface IBOColSelectorControl : IBOColSelector
    {
    }
}