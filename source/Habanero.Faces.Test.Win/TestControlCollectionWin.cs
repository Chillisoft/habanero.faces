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
using System.Windows.Forms;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;
using Rhino.Mocks;

// ReSharper disable InconsistentNaming
namespace Habanero.Faces.Test.Win
{
    [TestFixture]
    public class TestControlCollectionWin : TestControlCollection
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
        [Test]
        public void TestAddControl()
        {
            TextBoxWin tb = (TextBoxWin)GetControlFactory().CreateTextBox();
            IControlCollection col = new ControlCollectionWin(new System.Windows.Forms.Control.ControlCollection(tb));
            IControlHabanero ctl = GetControlFactory().CreateControl();
            col.Add(ctl);
            Assert.AreSame(ctl, col[0], "Control added should be the same object.");
        }

        [Test]
        public void Test_Add_WhenIsAdaptedControl_ShouldUseWrappedControl()

        {
            //---------------Set up test pack-------------------
            var adapter = GetWinFormsControlAdapter();
            IControlCollection col = CreateControlCollectionWin();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(adapter.WrappedControl);
            Assert.AreEqual(0, col.Count);
            //---------------Execute Test ----------------------
            col.Add(adapter);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            var controlHabanero = col[0] as IWinFormsControlAdapter; 
            Assert.IsNotNull(controlHabanero);
            Assert.AreSame(adapter.WrappedControl, controlHabanero.WrappedControl);
        }

        [Test]
        public void Test_this_ShouldWrapControlIfItIsNotAlreadyWrapped()
        {
            //---------------Set up test pack-------------------
            IControlCollection col = CreateControlCollectionWin();
            var adapter = GetWinFormsControlAdapter();
            col.Add(adapter);
            Assert.AreEqual(1, col.Count);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(adapter.WrappedControl);
            Assert.AreEqual(1, col.Count);
            //---------------Execute Test ----------------------
            var controlHabanero = col[0] as IWinFormsControlAdapter;
            //---------------Test Result -----------------------
            Assert.IsNotNull(controlHabanero);
            Assert.AreSame(adapter.WrappedControl, controlHabanero.WrappedControl);
        }
        [Test]
        public void Test_IndexOf_ShouldUseWrappedControl()
        {
            //---------------Set up test pack-------------------
            IControlCollection col = CreateControlCollectionWin();
            var adapter = GetWinFormsControlAdapter();
            col.Add(adapter);
            Assert.AreEqual(1, col.Count);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(adapter.WrappedControl);
            Assert.AreEqual(1, col.Count);
            //---------------Execute Test ----------------------
            var indexOf = col.IndexOf(adapter);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, indexOf);
        }

        private ControlCollectionWin CreateControlCollectionWin()
        {
            return new ControlCollectionWin(new System.Windows.Forms.Control.ControlCollection(new Control()));
        }

        //index of
        //this
        private static IWinFormsControlAdapter GetWinFormsControlAdapter()
        {
            var control = new System.Windows.Forms.Control();
            var winFormsControlAdapter = MockRepository.GenerateStub<IWinFormsControlAdapter>();
            winFormsControlAdapter.Stub(adapter => adapter.WrappedControl).Return(control);
            return winFormsControlAdapter;
        }
    }
}