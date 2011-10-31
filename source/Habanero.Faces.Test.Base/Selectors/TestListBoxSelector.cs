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
using Habanero.Base;
using Habanero.BO;
using Habanero.Faces.Base;


using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    public abstract class TestListBoxSelector : TestBOColSelector
    {
        protected override void SetSelectedIndex(IBOColSelectorControl colSelector, int index)
        {
            ((IBOListBoxSelector)colSelector).ListBox.SelectedIndex = index;
        }

        protected override int SelectedIndex(IBOColSelectorControl colSelector)
        {
            return ((IBOListBoxSelector)colSelector).ListBox.SelectedIndex;
        }

        protected override int NumberOfLeadingBlankRows()
        {
            return 0;
        }

        protected override int NumberOfTrailingBlankRows()
        {
            return 0;
        }

        protected override IBOColSelectorControl CreateSelector()
        {
            return GetControlFactory().CreateListBoxSelector();
        }

        [Test]
        public void Test_Constructor_ListBoxSet()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOListBoxSelector selector = (IBOListBoxSelector) CreateSelector();
            //---------------Test Result -----------------------
            Assert.IsNotNull(selector.ListBox);
            Assert.IsInstanceOf(typeof(IListBox), selector.ListBox);
        }

        [Ignore(" Not Yet implemented")] //TODO  01 Mar 2009:
        [Test]
        public void TestEditItemFromCollectionUpdatesItemInSelector()
        {
        }
    }

}