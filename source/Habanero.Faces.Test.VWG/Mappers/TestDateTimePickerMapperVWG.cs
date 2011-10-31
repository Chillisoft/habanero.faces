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
using Habanero.Faces.Test.Base.Mappers;
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Mappers
{
    [TestFixture]
    public class TestDateTimePickerMapperVWG : TestDateTimePickerMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.VWG.ControlFactoryVWG();
            //return null;
        }

        [Test, Ignore("ShowUpDown property does not exist for VWG : June 2008")]
        public override void TestAttribute_ShowUpDown()
        {
            base.TestAttribute_ShowUpDown();
        }

        [Test]
        public void TestSetBusinessObjectValue_DoesNotChangeDateTimePickerImmediately_InVWG()
        {
            //---------------Set up test pack-------------------
            Sample sampleBusinessObject = new Sample();
            sampleBusinessObject.SampleDate = DateTime.Today;
            DateTimePickerMapper dtpMapper;
            IDateTimePicker dateTimePicker = GetDateTimePicker(out dtpMapper);
            dtpMapper.BusinessObject = sampleBusinessObject;

            //---------------Verify test pack-------------------
            Assert.AreEqual(DateTime.Today, dateTimePicker.Value.Date);
            //---------------Execute Test ----------------------
            DateTime testDateChangedValue = new DateTime(2000, 1, 1);
            sampleBusinessObject.SampleDate = testDateChangedValue;

            //---------------Test Result -----------------------
            Assert.AreEqual(DateTime.Today, dateTimePicker.Value);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestUpdateValueInPicker_DoesNotChangeValueInBO_ForVWG()
        {
            //---------------Set up test pack-------------------
            Sample sampleBusinessObject = new Sample();
            DateTime origionalDate = new DateTime(2000, 1, 1);
            sampleBusinessObject.SampleDate = origionalDate;
            DateTimePickerMapper dtpMapper;
            IDateTimePicker dateTimePicker = GetDateTimePicker(out dtpMapper);
            dtpMapper.BusinessObject = sampleBusinessObject;
            //---------------Verify Preconditions -------------------
            Assert.AreEqual(origionalDate, dateTimePicker.Value.Date);
            //---------------Execute Test ----------------------
            DateTime newDate = DateTime.Today.AddDays(+3);
            dateTimePicker.Value = newDate;
            //---------------Test Result -----------------------
            Assert.AreEqual(origionalDate, sampleBusinessObject.SampleDate);
            //---------------Tear Down -------------------------          
        }

    }
}