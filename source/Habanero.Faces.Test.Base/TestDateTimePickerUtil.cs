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