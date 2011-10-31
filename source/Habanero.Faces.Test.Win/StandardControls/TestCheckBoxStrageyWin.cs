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
using Habanero.Faces.Test.Base;
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Extensions.Forms;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Test.Win.StandardControls
{
    [TestFixture]
    public class TestCheckBoxStrageyWin 
    {
        protected IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        [Test]
        public void Test_AddClickEventHandler_WhenMapperUsingHabaneroControl_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var checkBoxWin = new CheckBoxWin {Name = "TestCheckBox", Checked = false, Enabled = true};
            var checkBoxMapper = new CheckBoxMapperStub(checkBoxWin);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<ICheckBox>(checkBoxMapper.Control);
            //---------------Execute Test ----------------------
            var checkBoxStrategyWin = new CheckBoxStrategyWin();
            checkBoxStrategyWin.AddClickEventHandler(checkBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        [Test]
        public void Test_AddClickEventHandler_WhenMapperUsingControlAdapter_ShouldAddBehaviours()
        {
            //---------------Set up test pack-------------------
            var checkBoxWin = GetWinFormsControlAdapter();
            var checkBoxMapper = new CheckBoxMapperStub(checkBoxWin);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<CheckBox>(checkBoxMapper.GetControl());
            //---------------Execute Test ----------------------
            var checkBoxStrategyWin = new CheckBoxStrategyWin();
            checkBoxStrategyWin.AddClickEventHandler(checkBoxMapper);
            //---------------Assert Result----------------------
            Assert.IsTrue(true, "If an error was not thrown then we are OK");
        }

        private static IWinFormsCheckBoxAdapter GetWinFormsControlAdapter()
        {
            var control = new CheckBox();
            var winFormsControlAdapter = MockRepository.GenerateStub<IWinFormsCheckBoxAdapter>();
            winFormsControlAdapter.Stub(adapter => adapter.WrappedControl).Return(control);
            return winFormsControlAdapter;
        }
    }

    public class CheckBoxMapperStub : CheckBoxMapper
    {
        public CheckBoxMapperStub(ICheckBox cb) : base(cb, "Fdfad", false, new ControlFactoryWin())
        {
        }
/*
        public override void ApplyChangesToBusinessObject()
        {
            ApplyChangesToBusinessObjectCalled = true;
        }

        public bool ApplyChangesToBusinessObjectCalled { get; private set; }

        public override void UpdateControlValueFromBusinessObject()
        {
            UpdateControlValueFromBusinessObjectCalled = true;
        }

        public bool UpdateControlValueFromBusinessObjectCalled { get; private set; }*/
    }
}