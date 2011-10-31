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
using Habanero.Faces.Test.Base.Mappers;
using Habanero.Faces.Win;
using NUnit.Extensions.Forms;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Mappers
{
    [TestFixture]
    public class TestCheckBoxMapperWin : TestCheckBoxMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.Win.ControlFactoryWin();
        }

        [Test]
        public void TestCheckBox_Value_UpdatedwhenBusinessobjectUpdated()
        {
            //----------Setup test pack----------------------------
            _sampleBusinessObject.SampleBoolean = false;
            _mapper.BusinessObject = _sampleBusinessObject;
            //----------verify test pack --------------------------
            Assert.IsFalse(_cb.Checked);
            //----------Execute test ------------------------------
            _sampleBusinessObject.SampleBoolean = true;
            _mapper.UpdateControlValueFromBusinessObject();
            //----------verify test ------------------------------
            Assert.IsTrue(_cb.Checked);
        }

        [Test]
        public void TestSettingCheckBoxCheckedUpdatesBO()
        {
            _sampleBusinessObject.SampleBoolean = false;
            _mapper.BusinessObject = _sampleBusinessObject;
            _cb.Checked = true;
            _mapper.ApplyChangesToBusinessObject();
            Assert.IsTrue(_sampleBusinessObject.SampleBoolean);
        }

        [Test]
        public void TestCheckBoxHasCorrectStrategy()
        {
            //---------------Test Result -----------------------
            Assert.AreSame(typeof(CheckBoxStrategyWin), _mapper.GetStrategy().GetType());
        }


        [Test]
        public void TestClickingOfCheckBoxUpdatesBO()
        {
            //---------------Set up test pack-------------------
            _cb.Name = "TestCheckBox";
            _cb.Checked = false;
            _sampleBusinessObject.SampleBoolean = false;
            _mapper.BusinessObject = _sampleBusinessObject;
            Form frm = AddControlToForm(_cb);
            //---------------Execute Test ----------------------
            frm.Show();
            CheckBoxTester box = new CheckBoxTester("TestCheckBox");
            box.Click();
            box.Check();
            //---------------Test Result -----------------------
            Assert.IsTrue(_cb.Checked);
            Assert.IsTrue(_sampleBusinessObject.SampleBoolean);
            //---------------Tear down -------------------------
        }
        private static Form AddControlToForm(IControlHabanero parentControl)
        {
            Form frm = new Form();
            frm.Controls.Clear();
            frm.Controls.Add((Control)parentControl);
            return frm;
        }
    }
}