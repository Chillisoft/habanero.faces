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
using Habanero.Faces.Test.Base.Mappers;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Mappers
{
    [TestFixture]
    public class TestNumericUpDownIntegerMapperWin : TestNumericUpDownIntegerMapper
    {
        public override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        [Test]
        public void Test_ValueChangedEvent_UpdatesBusinessObject()
        {
            //---------------Set up test pack-------------------
            INumericUpDown numUpDown = GetControlFactory().CreateNumericUpDownInteger();
            NumericUpDownIntegerMapper mapper = new NumericUpDownIntegerMapper(numUpDown, INT_PROP_NAME, false, GetControlFactory());
            Sample s = new Sample();
            s.SampleInt = 100;
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            int newValue = 555;
            numUpDown.Value = newValue;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(NumericUpDownMapperStrategyWin), mapper.MapperStrategy);
            Assert.AreEqual(newValue, s.SampleInt);
            //---------------Tear down -------------------------
        }


        [Test]
        public void Test_BusinessObjectChanged_UpdatesControl()
        {
            //---------------Set up test pack-------------------
            INumericUpDown numUpDown = GetControlFactory().CreateNumericUpDownInteger();
            NumericUpDownIntegerMapper mapper = new NumericUpDownIntegerMapper(numUpDown, INT_PROP_NAME, false, GetControlFactory());
            Sample s = new Sample();
            s.SampleInt = 100;
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            int newValue = 555;
            s.SampleInt = newValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(newValue, numUpDown.Value);
            //---------------Tear down -------------------------
        }
    }
}