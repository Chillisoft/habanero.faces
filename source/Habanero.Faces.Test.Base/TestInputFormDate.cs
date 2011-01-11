// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using Habanero.Faces.Base;


using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    public abstract class TestInputFormDate
    {
        protected abstract IControlFactory GetControlFactory();




        [Test]
        public void TestSimpleConstructor()
        {
            //---------------Set up test pack-------------------
            const string message = "testMessage";
            //---------------Execute Test ----------------------
            InputFormDate inputFormDate  = new InputFormDate(GetControlFactory(), message);

            //---------------Test Result -----------------------
            Assert.AreEqual(message, inputFormDate.Message);
            Assert.AreEqual(DateTime.Now.Date, inputFormDate.DateTimePicker.Value.Date);

        }

        [Test]
        public void TestLayout()
        {
            //---------------Set up test pack-------------------
            const string message = "testMessage";


            InputFormDate inputFormDate = new InputFormDate(GetControlFactory(), message);
            //---------------Execute Test ----------------------
            IPanel panel = inputFormDate.createControlPanel();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, panel.Controls.Count);
            Assert.IsInstanceOf(typeof(ILabel), panel.Controls[0]);
            Assert.IsInstanceOf(typeof(IDateTimePicker), panel.Controls[1]);
            Assert.Greater(panel.Controls[0].Top, panel.Top);
            Assert.IsFalse(panel.Controls[0].Font.Bold);
            int width = GetControlFactory().CreateLabel(message, true).PreferredWidth + 20;
            Assert.AreEqual(panel.Width, width);
        }

        [Test]
        public void TestSelectedValue()
        {
            //---------------Set up test pack-------------------
            const string message = "testMessage";

            InputFormDate inputFormDate = new InputFormDate(GetControlFactory(), message);
            //---------------Assert pre conditions--------------
            Assert.AreEqual(DateTime.Now.Date, inputFormDate.DateTimePicker.Value.Date);
            //---------------Execute Test ----------------------
            DateTime value = DateTime.Now.Date.AddDays(-5);
            inputFormDate.DateTimePicker.Value = value;
            //---------------Test Result -----------------------
            Assert.AreEqual(value, inputFormDate.Value );
            //---------------Tear Down -------------------------
        }
    }

}