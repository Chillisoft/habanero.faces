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
using System.Collections.Generic;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.HabaneroControls
{
    [TestFixture]
    public class TestMultiSelectorVWG : TestMultiSelector
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }
        //There are lots of different tests in giz because we do not want the event handling
        //overhead of hitting the server all the time to enable and disable buttons.
        [Test]
        public void TestVWG_SelectButtonStateAtSet()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();

            //---------------Execute Test ----------------------
            _selector.AllOptions = CreateListWithTwoOptions();

            //---------------Test Result -----------------------
            Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Select).Enabled);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestVWG_SelectButtonStateUponSelection()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.AllOptions = CreateListWithTwoOptions();
            //---------------Execute Test ----------------------

            _selector.AvailableOptionsListBox.SelectedIndex = 0;

            //---------------Test Result -----------------------
            Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Select).Enabled);
        }

        [Test]
        public void TestVWG_SelectButtonIsEnabledWhenItemIsDeselected()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.AllOptions = CreateListWithTwoOptions();
            _selector.AvailableOptionsListBox.SelectedIndex = 0;
            //---------------Execute Test ----------------------
            _selector.AvailableOptionsListBox.SelectedIndex = -1;
            //---------------Test Result -----------------------
            Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Select).Enabled);
        }

        [Test]
        public void TestVWG_ClickSelectButtonWithNoItemSelected()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.AllOptions = CreateListWithTwoOptions();
            _selector.AvailableOptionsListBox.SelectedIndex = -1;
            //---------------Execute Test ----------------------
            _selector.GetButton(MultiSelectorButton.Select).PerformClick();
            //---------------Test Result -----------------------
            AssertNoneSelected(_selector);
        }


        [Test]
        public void TestVWG_DeselectButtonStateAtSet()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            List<TestT> options = CreateListWithTwoOptions();
            _selector.AllOptions = options;
            //---------------Execute Test ----------------------
            _selector.SelectedOptions = options;
            //---------------Test Result -----------------------
            Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Deselect).Enabled);
        }

        [Test]
        public void TestVWG_DeselectButtonStateUponSelection()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            List<TestT> options = CreateListWithTwoOptions();
            _selector.AllOptions = options;
            _selector.SelectedOptions = options;
            //---------------Execute Test ----------------------
            _selector.SelectedOptionsListBox.SelectedIndex = 0;
            //---------------Test Result -----------------------
            Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Deselect).Enabled);
        }

        [Test]
        public void TestVWG_DeselectButtonIsDisabledWhenItemIsDeselected()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            List<TestT> options = CreateListWithTwoOptions();
            _selector.AllOptions = options;
            _selector.SelectedOptions = options;
            _selector.SelectedOptionsListBox.SelectedIndex = 0;
            //---------------Execute Test ----------------------
            _selector.SelectedOptionsListBox.SelectedIndex = -1;
            //---------------Test Result -----------------------
            Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Deselect).Enabled);
            //---------------Tear Down -------------------------          
        }
        //There is a bug in giz that does not allow you to programmattically select 
        //multiple items in a list
        [Test, Ignore("Problem selecting multiple items from code in gizmox")]
        public override void TestSelectingMultipleItemsAtOnce_Click()
        {
        }
        //There is a bug in giz that does not allow you to programmattically select 
        //multiple items in a list
        [Test, Ignore("Problem selecting multiple items from code in gizmox")]
        public override void TestDeselectingMultipleItemsAtOnce_Click()
        {
        }
    }
}