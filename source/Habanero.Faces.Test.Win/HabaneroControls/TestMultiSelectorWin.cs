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
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
    [TestFixture]
    public class TestMultiSelectorWin : TestMultiSelector
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        //There are lots of different tests in giz and win because we do not want the event handling
        //overhead of hitting the server all the time to enable and disable buttons.
        [Test]
        public void Test_Win_SelectButtonStateAtSet()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();

            //---------------Execute Test ----------------------
            _selector.AllOptions = CreateListWithTwoOptions();

            //---------------Test Result -----------------------
            Assert.IsFalse(_selector.GetButton(MultiSelectorButton.Select).Enabled);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_Win_SelectButtonStateUponSelection()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.AllOptions = CreateListWithTwoOptions();
            //---------------Execute Test ----------------------

            _selector.AvailableOptionsListBox.SelectedIndex = 0;

            //---------------Test Result -----------------------
            Assert.IsTrue(_selector.GetButton(MultiSelectorButton.Select).Enabled);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_Win_SelectButtonIsDisabledWhenItemIsDeselected()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            _selector.AllOptions = CreateListWithTwoOptions();
            _selector.AvailableOptionsListBox.SelectedIndex = 0;
            //---------------Execute Test ----------------------
            _selector.AvailableOptionsListBox.SelectedIndex = -1;
            //---------------Test Result -----------------------
            Assert.IsFalse(_selector.GetButton(MultiSelectorButton.Select).Enabled);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_Win_DeselectButtonStateAtSet()
        {
            //---------------Set up test pack-------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            List<TestT> options = CreateListWithTwoOptions();
            _selector.AllOptions = options;
            //---------------Execute Test ----------------------
            _selector.SelectedOptions = options;

            //---------------Test Result -----------------------
            Assert.IsFalse(_selector.GetButton(MultiSelectorButton.Deselect).Enabled);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_Win_DeselectButtonStateUponSelection()
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
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_Win_DeselectButtonIsDisabledWhenItemIsDeselected()
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
            Assert.IsFalse(_selector.GetButton(MultiSelectorButton.Deselect).Enabled);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_DoubleClickingHandlersAssigned()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IMultiSelector<TestT> _selector = GetControlFactory().CreateMultiSelector<TestT>();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, TestUtil.CountEventSubscribers(_selector.AvailableOptionsListBox, "DoubleClick"));
            Assert.AreEqual(1, TestUtil.CountEventSubscribers(_selector.SelectedOptionsListBox, "DoubleClick"));

            Assert.IsTrue(TestUtil.EventHasSubscriber(_selector.AvailableOptionsListBox, "DoubleClick", "DoSelect"));
            Assert.IsTrue(TestUtil.EventHasSubscriber(_selector.SelectedOptionsListBox, "DoubleClick", "DoDeselect"));
        }

    }
}