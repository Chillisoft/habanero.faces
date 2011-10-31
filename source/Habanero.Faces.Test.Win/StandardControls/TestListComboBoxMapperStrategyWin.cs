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
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Test.Win.StandardControls
{
    [TestFixture]
    public class TestListComboBoxMapperStrategyWin 
    {
        protected IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
        [Test]
        public void Test_AddItemSelectedEventHandler_WhenMapperUsingHabaneroControl_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var cmbWin = new ComboBoxWin() { Name = "TestComboBox", Enabled = true };
            var comboBoxMapper = new ListComboBoxMapperStub(cmbWin);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<IComboBox>(comboBoxMapper.Control);
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new ListComboBoxMapperStrategyWin();
            comboBoxStrategyWin.AddItemSelectedEventHandler(comboBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        [Test]
        public void Test_AddItemSelectedEventHandler_WhenMapperUsingControlAdapter_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var cmbWin = GetWinFormsControlAdapter();
            var comboBoxMapper = new ListComboBoxMapperStub(cmbWin);
            //---------------Assert ComboBoxMapperStub----------------
            Assert.IsInstanceOf<ComboBox>(comboBoxMapper.Control.GetControl());
            Assert.IsInstanceOf<ComboBox>(comboBoxMapper.GetControl());
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new ListComboBoxMapperStrategyWin();
            comboBoxStrategyWin.AddItemSelectedEventHandler(comboBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        private static IWinFormsComboBoxAdapter GetWinFormsControlAdapter()
        {
            var control = new ComboBox();
            var winFormsControlAdapter = MockRepository.GenerateStub<IWinFormsComboBoxAdapter>();
            winFormsControlAdapter.Stub(adapter => adapter.WrappedControl).Return(control);
            return winFormsControlAdapter;
        }
    }

    internal class ListComboBoxMapperStub : ListComboBoxMapper
    {
        public ListComboBoxMapperStub(IControlHabanero ctl) : base(ctl, "PropName", false, new ControlFactoryWin())
        {
        }
    }
}