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
using Habanero.Faces.Base;
using Habanero.Test.Structure;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Faces.Test.Base
{
	[TestFixture]
	public class TestDateTimePickerUtil
	{
		[Test]
		public void Test_GetDateFormatString_WhenLongFormat_ShouldReturnCorrectString()
		{
			//---------------Set up test pack-------------------
			var dateTimePicker = MockRepository.GenerateStub<IDateTimePicker>();
			dateTimePicker.Format = DateTimePickerFormat.Long;
			dateTimePicker.CustomFormat = "something";
			//---------------Assert Precondition----------------
			//---------------Execute Test ----------------------
			string dateFormatString = DateTimePickerUtil.GetDateFormatString(dateTimePicker);
			//---------------Test Result -----------------------
			Assert.AreEqual("D", dateFormatString);
		}

		[Test]
		public void Test_GetDateFormatString_WhenShortFormat_ShouldReturnCorrectString()
		{
			//---------------Set up test pack-------------------
			var dateTimePicker = MockRepository.GenerateStub<IDateTimePicker>();
			dateTimePicker.Format = DateTimePickerFormat.Short;
			dateTimePicker.CustomFormat = "something";
			//---------------Assert Precondition----------------
			//---------------Execute Test ----------------------
			string dateFormatString = DateTimePickerUtil.GetDateFormatString(dateTimePicker);
			//---------------Test Result -----------------------
			Assert.AreEqual("d", dateFormatString);
		}

		[Test]
		public void Test_GetDateFormatString_WhenTimeFormat_ShouldReturnCorrectString()
		{
			//---------------Set up test pack-------------------
			var dateTimePicker = MockRepository.GenerateStub<IDateTimePicker>();
			dateTimePicker.Format = DateTimePickerFormat.Time;
			dateTimePicker.CustomFormat = "something";
			//---------------Assert Precondition----------------
			//---------------Execute Test ----------------------
			string dateFormatString = DateTimePickerUtil.GetDateFormatString(dateTimePicker);
			//---------------Test Result -----------------------
			Assert.AreEqual("T", dateFormatString);
		}

		[Test]
		public void Test_GetDateFormatString_WhenCustomFormat_ShouldReturnCorrectString()
		{
			//---------------Set up test pack-------------------
			var dateTimePicker = MockRepository.GenerateStub<IDateTimePicker>();
			dateTimePicker.Format = DateTimePickerFormat.Custom;
			string customFormat = RandomValueGen.GetRandomString();
			dateTimePicker.CustomFormat = customFormat;
			//---------------Assert Precondition----------------
			//---------------Execute Test ----------------------
			string dateFormatString = DateTimePickerUtil.GetDateFormatString(dateTimePicker);
			//---------------Test Result -----------------------
			Assert.AreEqual(customFormat, dateFormatString);
		}
	}
}