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
using System.Windows.Forms;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Test.Win.StandardControls
{
    [TestFixture]
    public class TestNumericUpDownMapperStrategyWin 
    {
        protected IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
        [Test]
        public void Test_AddValueChangedHandler_WhenMapperUsingHabaneroControl_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var updWin = new NumericUpDownWin() { Name = "TestNumericUpDown", Enabled = true };
            var comboBoxMapper = new NumericUpDownMapperStub(updWin);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<NumericUpDown>(comboBoxMapper.Control);
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new NumericUpDownMapperStrategyWin();
            comboBoxStrategyWin.ValueChanged(comboBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        [Test]
        public void Test_AddValueChangedHandler_WhenMapperUsingControlAdapter_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var updWin = GetWinFormsControlAdapter();
            var comboBoxMapper = new NumericUpDownMapperStub(updWin);
            //---------------Assert ComboBoxMapperStub----------------
            Assert.IsInstanceOf<NumericUpDown>(comboBoxMapper.Control.GetControl());
            Assert.IsInstanceOf<NumericUpDown>(comboBoxMapper.GetControl());
            //---------------Execute Test ----------------------
            var comboBoxStrategyWin = new NumericUpDownMapperStrategyWin();
            comboBoxStrategyWin.ValueChanged(comboBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        private static IWinFormsNumericUpDownAdapter GetWinFormsControlAdapter()
        {
            var control = new NumericUpDown();
            var winFormsControlAdapter = MockRepository.GenerateStub<IWinFormsNumericUpDownAdapter>();
            winFormsControlAdapter.Stub(adapter => adapter.WrappedControl).Return(control);
            return winFormsControlAdapter;
        }
    }

    internal class NumericUpDownMapperStub : NumericUpDownMapper
    {
        public NumericUpDownMapperStub(INumericUpDown ctl) 
            : base(ctl, "Fdaf", false, new ControlFactoryWin())
        {
        }

        public override void ApplyChangesToBusinessObject()
        {

        }
    }
}