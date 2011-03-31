//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System.Windows.Forms;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base.Mappers
{
    [TestFixture]
    public class TestNumericUpDownCurrencyMapper
    {
        protected IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
        protected const string CURRENCY_PROP_NAME = "SampleMoney";

   


        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            NumericUpDown numUpDown = GetControlFactory().CreateNumericUpDownCurrency();
            //---------------Execute Test ----------------------
            NumericUpDownCurrencyMapper mapper =
                new NumericUpDownCurrencyMapper(numUpDown, CURRENCY_PROP_NAME, false, GetControlFactory());

            //---------------Test Result -----------------------
            Assert.AreSame(numUpDown, mapper.Control);
            Assert.AreSame(CURRENCY_PROP_NAME, mapper.PropertyName);
            Assert.AreEqual(decimal.MinValue, numUpDown.Minimum);
            Assert.AreEqual(decimal.MaxValue, numUpDown.Maximum);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSetBusinessObject()
        {
            //---------------Set up test pack-------------------
            NumericUpDown numUpDown = GetControlFactory().CreateNumericUpDownCurrency();
            NumericUpDownCurrencyMapper mapper =
                new NumericUpDownCurrencyMapper(numUpDown, CURRENCY_PROP_NAME, false, GetControlFactory());
            MyBO s = new MyBO();
            decimal val = 100.5m;
            s.SampleMoney = val;
            //---------------Execute Test ----------------------
            mapper.BusinessObject = s;
            //---------------Test Result -----------------------
            Assert.AreEqual(val, numUpDown.Value, "Value is not set.");

            //---------------Tear Down -------------------------
        }

/*//TODO brett 30 Mar 2011: CF        [Test]
        public void TestApplyChangesToBO()
        {
            //---------------Set up test pack-------------------
            NumericUpDown numUpDown = GetControlFactory().CreateNumericUpDownCurrency();
            NumericUpDownCurrencyMapper mapper =
                new NumericUpDownCurrencyMapper(numUpDown, CURRENCY_PROP_NAME, false, GetControlFactory());
            Sample s = new Sample();
            decimal val = 100.5m;
            s.SampleMoney = val;
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            decimal newVal = 200.2m;
            numUpDown.Value = newVal;
            mapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreEqual(newVal, s.SampleMoney, "Value is not set.");

            //---------------Tear Down -------------------------
        }*/
    }
}