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


using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{



    /// <summary>
    /// This test class tests the ListBox class.
    /// </summary>
    public abstract class TestListBox
    {
        protected abstract IControlFactory GetControlFactory();




        [Test]
        public void TestCreateListBox()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IListBox myListBox = GetControlFactory().CreateListBox();

            //---------------Test Result -----------------------
            Assert.IsNotNull(myListBox);

            //---------------Tear Down -------------------------   
        }

        [Test]
        public void TestListBoxItems()
        {
            //---------------Set up test pack-------------------
            IListBox myListBox = GetControlFactory().CreateListBox();

            //---------------Execute Test ----------------------
            myListBox.Items.Add("testitem");

            //---------------Test Result -----------------------
            Assert.AreEqual(1, myListBox.Items.Count);
            //---------------Tear Down -------------------------   
        }


      
        
    }
}
