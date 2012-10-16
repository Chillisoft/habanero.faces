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
using System.Drawing;
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Extensions.Forms;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
	/// <summary>
    /// This class tests the DateTimePicker control.
    ///  - The issue of the control being nullable or not is tested.
    /// </summary>
    public abstract class TestDateTimePicker
    {
        protected abstract void SetBaseDateTimePickerValue(IDateTimePicker dateTimePicker, DateTime value);
        protected abstract void SetBaseDateTimePickerCheckedValue(IDateTimePicker dateTimePicker, bool value);
    	protected abstract void SubscribeToBaseValueChangedEvent(IDateTimePicker dateTimePicker, EventHandler onValueChanged);

        protected abstract IControlFactory GetControlFactory();

        protected abstract EventArgs GetKeyDownEventArgsForDeleteKey();
        protected abstract EventArgs GetKeyDownEventArgsForBackspaceKey();
        protected abstract EventArgs GetKeyDownEventArgsForOtherKey();

        protected IDateTimePicker CreateDateTimePicker()
        {
            IDateTimePicker dateTimePicker = GetControlFactory().CreateDateTimePicker();
            //IFormHabanero form = GetControlFactory().CreateForm();
            //form.Controls.Add(dateTimePicker);
            //form.Visible = true;
            return dateTimePicker;
        }


        //TODO: Test if anything intelligent needs to be done with the commented out events in the DateTimePickerManager

        [Test]
        public void TestCreateDateTimePicker()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            //---------------Test Result -----------------------
            Assert.IsNotNull(dateTimePicker);
            //---------------Tear Down   -----------------------
        }

        [Test]
        public void TestDefaultValue()
        {
            //-------------Setup Test Pack ------------------
            //-------------Execute test ---------------------
            DateTime beforeCreate = DateTime.Now.AddMilliseconds(-1);
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime afterCreate = DateTime.Now.AddMilliseconds(1);
            //-------------Test Result ----------------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            DateTime dateTime = dateTimePicker.Value;
            Assert.LessOrEqual(beforeCreate, dateTime, "Default value should be Now");
            Assert.GreaterOrEqual(afterCreate, dateTime, "Default value should be Now");
            //Assert.IsFalse(dateTimePicker.ShowCheckBox, "Default should not show checkbox");
        }

        [Test]
        public void TestSetBaseValue_ChangesValueForIDateTimePicker()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime dateTime = DateTime.Now;
            dateTimePicker.Value = dateTime;
            DateTime dateTimeNew = dateTime.AddDays(1);
            //---------------Execute Test ----------------------
            SetBaseDateTimePickerValue(dateTimePicker, dateTimeNew);
            //---------------Test Result -----------------------
            Assert.AreEqual(dateTimeNew, dateTimePicker.Value);
        }

        

        [Test]
        public void TestSetValue()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime testDate = new DateTime(2008, 01, 01, 01, 01, 01);
            //---------------Execute Test ----------------------
            dateTimePicker.Value = testDate;
            //---------------Test Result -----------------------
            Assert.AreEqual(testDate, dateTimePicker.Value);
            //---------------Tear Down -------------------------    
        }
        
        [Test]
        public void TestSetValueToNull()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime dateTime = DateTime.Now;
            dateTimePicker.Value = dateTime;
            //---------------Assert Precondition----------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            //---------------Execute Test ----------------------
            dateTimePicker.ValueOrNull = null;
            //---------------Test Result -----------------------
            Assert.IsNull(dateTimePicker.ValueOrNull);
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            Assert.AreEqual(dateTime, dateTimePicker.Value);
        }

        [Test]
        public void TestSetNullThenValue()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime dateTime = DateTime.Now;
            DateTime expectedDateTime = dateTime.AddDays(1);
            dateTimePicker.Value = dateTime;
            dateTimePicker.ValueOrNull = null;
            //---------------Assert Precondition----------------
            Assert.IsNull(dateTimePicker.ValueOrNull);
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            Assert.AreEqual(dateTime, dateTimePicker.Value);
            //---------------Execute Test ----------------------
            dateTimePicker.Value = expectedDateTime;
            //---------------Test Result -----------------------
            Assert.IsNotNull(dateTimePicker.ValueOrNull);
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            Assert.AreEqual(expectedDateTime, dateTimePicker.Value);
            Assert.AreEqual(expectedDateTime, dateTimePicker.ValueOrNull.Value);
        }

        [Test]
        public void TestSetNullThenValue_UsingValueOrNull()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime dateTime = DateTime.Now;
            DateTime expectedDateTime = dateTime.AddDays(1);
            dateTimePicker.Value = dateTime;
            dateTimePicker.ValueOrNull = null;
            //---------------Assert Precondition----------------
            Assert.IsNull(dateTimePicker.ValueOrNull);
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            Assert.AreEqual(dateTime, dateTimePicker.Value);
            //---------------Execute Test ----------------------
            dateTimePicker.ValueOrNull = expectedDateTime;
            //---------------Test Result -----------------------
            Assert.IsNotNull(dateTimePicker.ValueOrNull);
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            Assert.AreEqual(expectedDateTime, dateTimePicker.Value);
            Assert.AreEqual(expectedDateTime, dateTimePicker.ValueOrNull.Value);
        }

	    [Test]
	    public void TestUsesGlobalUIRegistryDefaultDateTimePickerFormat()
	    {
	        //---------------Set up test pack-------------------
            GlobalUIRegistry.DateDisplaySettings = new DateDisplaySettings() { DateTimePickerDefaultFormat = "yyyy/mm/dd" };
	        //---------------Assert Precondition----------------

	        //---------------Execute Test ----------------------
            IDateTimePicker picker = CreateDateTimePicker();
	        //---------------Test Result -----------------------
            Assert.AreEqual(picker.CustomFormat, GlobalUIRegistry.DateDisplaySettings.DateTimePickerDefaultFormat);
	    }
        
        #region Checkbox Tests

        [Test]
        public void TestSetToNull_UnChecksCheckbox()
        {
            //-------------Setup Test Pack ------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = true;
            DateTime dateTimeValue = DateTime.Now;
            dateTimePicker.Value = dateTimeValue;
            dateTimePicker.Checked = true;
            //-------------Test Pre-conditions --------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            Assert.IsTrue(dateTimePicker.Checked);
            //-------------Execute test ---------------------
            dateTimePicker.ValueOrNull = null;
            //-------------Test Result ----------------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            Assert.IsFalse(dateTimePicker.Checked);
            Assert.AreEqual(dateTimeValue, dateTimePicker.Value);
        }

        [Test]
        public void TestSetToNotNull_ChecksCheckbox()
        {
            //-------------Setup Test Pack ------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = true;
            DateTime dateTimeValue = DateTime.Now;
            dateTimePicker.Value = dateTimeValue;
            dateTimePicker.ValueOrNull = null;
            dateTimePicker.Checked = false;
            //-------------Test Pre-conditions --------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            Assert.IsFalse(dateTimePicker.Checked);
            //-------------Execute test ---------------------
            dateTimePicker.ValueOrNull = dateTimeValue;
            //-------------Test Result ----------------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            Assert.IsTrue(dateTimePicker.Checked);
            Assert.AreEqual(dateTimeValue, dateTimePicker.Value);
        }

        [Test]
        public void TestSetUnCheckedChangesValue_ToNull()
        {
            //-------------Setup Test Pack ------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = true;
            DateTime dateTimeValue = DateTime.Now;
            dateTimePicker.Value = dateTimeValue;
            dateTimePicker.Checked = true;
            //-------------Test Pre-conditions --------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            Assert.IsTrue(dateTimePicker.Checked);
            //-------------Execute test ---------------------
            dateTimePicker.Checked = false;
            //-------------Test Result ----------------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            Assert.IsFalse(dateTimePicker.Checked);
        }

        [Test]
        public void TestSetCheckedChangesValue_ToValue()
        {
            //-------------Setup Test Pack ------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = true;
            DateTime dateTimeValue = DateTime.Now;
            dateTimePicker.Value = dateTimeValue;
            dateTimePicker.ValueOrNull = null;
            dateTimePicker.Checked = false;
            //-------------Test Pre-conditions --------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            Assert.IsFalse(dateTimePicker.Checked);
            //-------------Execute test ---------------------
            dateTimePicker.Checked = true;
            //-------------Test Result ----------------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            Assert.IsTrue(dateTimePicker.Checked);
            Assert.AreEqual(dateTimeValue, dateTimePicker.ValueOrNull.Value);
        }

        [Test]
        public void TestSetBaseChecked_ChangesValueForIDateTimePicker()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = GetControlFactory().CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = true;
            DateTime dateTime = DateTime.Now;
            dateTimePicker.Value = dateTime;
            dateTimePicker.Checked = false;
            //---------------Assert Pre-Conditions -------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            //---------------Execute Test ----------------------
            SetBaseDateTimePickerCheckedValue(dateTimePicker, true);
            //---------------Test Result -----------------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            Assert.AreEqual(dateTime, dateTimePicker.ValueOrNull);
        }

        #endregion //Checkbox Tests

        #region Test Events

        [Test]
        public void Test_Value_WhenSet_ShouldFireValueChangedEvent()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime sampleDate = DateTime.Now.AddDays(1);
            dateTimePicker.Value = sampleDate.AddDays(1);
            bool isFired = false;
            int firedTimes = 0;
            dateTimePicker.ValueChanged += delegate
            {
                isFired = true;
                firedTimes++;
            };
            //---------------Execute Test ----------------------
            dateTimePicker.Value = sampleDate;
            //---------------Test Result -----------------------
            Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value.");
            //Assert.AreEqual(1, firedTimes, "The event should have fired only once.");
        }

        [Test]
        public void Test_BaseValue_WhenSet_ShouldFireValueChangedEvent()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime sampleDate = DateTime.Now.AddDays(1);
            dateTimePicker.Value = sampleDate.AddDays(1);
            bool isFired = false;
            int firedTimes = 0;
            dateTimePicker.ValueChanged += delegate
            {
                isFired = true;
                firedTimes++;
            };
            //---------------Execute Test ----------------------
            SetBaseDateTimePickerValue(dateTimePicker, sampleDate);
            //---------------Test Result -----------------------
            Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value.");
            //Assert.AreEqual(1, firedTimes, "The event should have fired only once.");
        }

        [Test]
        public virtual void Test_BaseChecked_WhenSetToFalse_ShouldFireValueChangedEvent()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime sampleDate = DateTime.Now.AddDays(1);
            dateTimePicker.Value = sampleDate.AddDays(1);
            bool isFired = false;
            int firedTimes = 0;
            dateTimePicker.ValueChanged += delegate
            {
                isFired = true;
                firedTimes++;
            };
            //---------------Execute Test ----------------------
            SetBaseDateTimePickerCheckedValue(dateTimePicker, false);

            //---------------Test Result -----------------------
            Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value.");
            //Assert.AreEqual(1, firedTimes, "The event should have fired only once.");
        }

        [Test]
        public void Test_Checked_WhenSetToFalse_ShouldFireValueChangedEvent()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            DateTime sampleDate = DateTime.Now.AddDays(1);
            dateTimePicker.Value = sampleDate.AddDays(1);
            bool isFired = false;
            int firedTimes = 0;
            dateTimePicker.ValueChanged += delegate
            {
                isFired = true;
                firedTimes++;
            };
            //---------------Execute Test ----------------------
            dateTimePicker.Checked = false;
            //---------------Test Result -----------------------
            Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value.");
            Assert.AreEqual(1, firedTimes, "The event should have fired only once.");
        }

		[Test]
		public void Test_ValueOrNull_WheSetToNull_ShouldFireValueChangedEvent()
		{
			//---------------Set up test pack-------------------
			IDateTimePicker dateTimePicker = CreateDateTimePicker();
			DateTime sampleDate = DateTime.Now.AddDays(1);
			dateTimePicker.Value = sampleDate;
			bool isFired = false;
			int firedTimes = 0;
			dateTimePicker.ValueChanged += delegate
			{
				isFired = true;
				firedTimes++;
			};
			//---------------Execute Test ----------------------
			dateTimePicker.ValueOrNull = null;
			//---------------Test Result -----------------------
			Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value to null.");
			//Assert.AreEqual(1, firedTimes, "The event should have fired only once.");
		}

		[Test]
		public void Test_ValueOrNull_WheSetToNull_ShouldFireBaseValueChangedEvent()
		{
			//---------------Set up test pack-------------------
			IDateTimePicker dateTimePicker = CreateDateTimePicker();
			DateTime sampleDate = DateTime.Now.AddDays(1);
			dateTimePicker.Value = sampleDate;
			bool isFired = false;
			int firedTimes = 0;
			SubscribeToBaseValueChangedEvent(dateTimePicker, delegate
			{
				isFired = true;
				firedTimes++;
			});
			//---------------Execute Test ----------------------
			dateTimePicker.ValueOrNull = null;
			//---------------Test Result -----------------------
			Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value to null.");
			//Assert.AreEqual(1, firedTimes, "The event should have fired only once.");
		}

    	#endregion // Test Events

        #region Visual State Tests

        [Test]
        public void TestSetup_NullDisplayControlIsCreated()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dateTimePicker.Controls.Count);
            Assert.IsInstanceOf(typeof(ILabel), dateTimePicker.Controls[0]);
            //IPanel pnl = (IPanel)dateTimePicker.Controls[0];
            //Assert.AreEqual(1, pnl.Controls.Count);
            //Assert.IsInstanceOf(typeof(ILabel), pnl.Controls[0]);
            //ILabel lbl = (ILabel)pnl.Controls[0];
            //Assert.AreEqual("", lbl.Text);
            //---------------Tear Down -------------------------          
        }

        private IControlHabanero GetNullDisplayControl(IDateTimePicker dateTimePicker)
        {
            if (dateTimePicker == null) return null;
            if (dateTimePicker.Controls.Count == 0) return null;
            return dateTimePicker.Controls[0];
        }

        [Test]
        public void TestVisualState_NullDisplayValue()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            string nullDisplayValue = TestUtil.GetRandomString();
            IControlHabanero nullDisplayControl = GetNullDisplayControl(dateTimePicker);
            //-------------Assert Preconditions -------------
            Assert.AreEqual("", dateTimePicker.NullDisplayValue);
            Assert.AreEqual("", nullDisplayControl.Text);
            //---------------Execute Test ----------------------
            dateTimePicker.NullDisplayValue = nullDisplayValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(nullDisplayValue, dateTimePicker.NullDisplayValue);
            Assert.AreEqual(nullDisplayValue, nullDisplayControl.Text);
        }

        [Test]
        public void TestVisualState_WhenCreated()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();

            //---------------Test Result -----------------------
            IControlHabanero nullDisplayControl = GetNullDisplayControl(dateTimePicker);
            Assert.IsNotNull(nullDisplayControl, "DateTimePicker should have a null display control.");
            Assert.IsTrue(nullDisplayControl.Visible, "Null display value control should be visible when there is a null value.");
        }

        [Test]
        public void TestVisualState_WhenNull()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = false;
            //---------------Execute Test ----------------------
            dateTimePicker.ValueOrNull = null;

            //---------------Test Result -----------------------
            IControlHabanero nullDisplayControl = GetNullDisplayControl(dateTimePicker);
            Assert.IsNotNull(nullDisplayControl, "DateTimePicker should have a null display control.");
            Assert.IsTrue(nullDisplayControl.Visible, "Null display value control should be visible when there is a null value.");
        }

        [Test]
        public void TestVisualState_WhenNotNull_ThenNull()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = false;
            dateTimePicker.ValueOrNull = DateTime.Now;
            //---------------Execute Test ----------------------
            dateTimePicker.ValueOrNull = null;

            //---------------Test Result -----------------------
            IControlHabanero nullDisplayControl = GetNullDisplayControl(dateTimePicker);
            Assert.IsNotNull(nullDisplayControl, "DateTimePicker should have a null display control.");
            Assert.IsTrue(nullDisplayControl.Visible, "Null display value control should be visible when there is a null value.");
        }

        [Test]
        public void TestVisualState_WhenNotNull()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = false;
            //---------------Execute Test ----------------------
            dateTimePicker.ValueOrNull = DateTime.Now;

            //---------------Test Result -----------------------
            IControlHabanero nullDisplayControl = GetNullDisplayControl(dateTimePicker);
            Assert.IsNotNull(nullDisplayControl, "DateTimePicker should have a null display control.");
            Assert.IsFalse(nullDisplayControl.Visible, "Null display value control should not be visible when there is a value.");
        }

        [Test]
        public void TestVisualState_WhenNull_ThenNotNull()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = false;
            dateTimePicker.ValueOrNull = null;
            //---------------Execute Test ----------------------
            dateTimePicker.ValueOrNull = DateTime.Now;

            //---------------Test Result -----------------------
            IControlHabanero nullDisplayControl = GetNullDisplayControl(dateTimePicker);
            Assert.IsNotNull(nullDisplayControl, "DateTimePicker should have a null display control.");
            Assert.IsFalse(nullDisplayControl.Visible, "Null display value control should not be visible when there is a value.");
        }

        [Test]
        public void TestVisualState_ResizesCorrectly()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ShowCheckBox = false;
            dateTimePicker.ValueOrNull = null;
            IControlHabanero nullDisplayControl = GetNullDisplayControl(dateTimePicker);
            //-------------Assert Preconditions -------------
            Assert.IsNotNull(nullDisplayControl, "DateTimePicker should have a null display control.");
            const int widthDifference = 24;
            const int heightDifference = 7;
            Assert.AreEqual(dateTimePicker.Width - widthDifference, nullDisplayControl.Width);
            Assert.AreEqual(dateTimePicker.Height - heightDifference, nullDisplayControl.Height);
            //---------------Execute Test ----------------------
            dateTimePicker.Size = Size.Add(dateTimePicker.Size, new Size(10, 4));
            //---------------Test Result -----------------------
            Assert.AreEqual(dateTimePicker.Width - widthDifference, nullDisplayControl.Width);
            Assert.AreEqual(dateTimePicker.Height - heightDifference, nullDisplayControl.Height);
        }

        [Test, Ignore("This does not seem to be testable")]
        public void TestVisualState_WhenNull_Selected()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ValueOrNull = null;
            IControlHabanero nullDisplayControl = GetNullDisplayControl(dateTimePicker);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(dateTimePicker.BackColor, nullDisplayControl.BackColor);
            Assert.AreEqual(dateTimePicker.ForeColor, nullDisplayControl.ForeColor);
            //---------------Execute Test ----------------------
            dateTimePicker.Focus();
            //EventHelper.RaiseEvent(dateTimePicker, "GotFocus");
            //---------------Test Result -----------------------
            Assert.AreEqual(SystemColors.Highlight, nullDisplayControl.BackColor);
            Assert.AreEqual(SystemColors.HighlightText, nullDisplayControl.ForeColor);

        }

        #endregion // Visual State Tests

        #region User Interaction Tests

        [Test]
        public void TestClick_ChangesNullToValue()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ValueOrNull = null;
            //-------------Assert Preconditions -------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            //---------------Execute Test ----------------------
            EventHelper.RaiseEvent(dateTimePicker, "Click");
            //---------------Test Result -----------------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
        }

        [Test]
        public void TestClick_WhenNotNullStaysAtValue()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ValueOrNull = DateTime.Now;
            //-------------Assert Preconditions -------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            //---------------Execute Test ----------------------
            EventHelper.RaiseEvent(dateTimePicker, "Click");
            //---------------Test Result -----------------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
        }

        [Test]
        public void TestClick_OnDisplayControlChangesNullToValue()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ValueOrNull = null;
            IControlHabanero nullDisplayControl = GetNullDisplayControl(dateTimePicker);
            //-------------Assert Preconditions -------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            //---------------Execute Test ----------------------
            EventHelper.RaiseEvent(nullDisplayControl, "Click");
            //---------------Test Result -----------------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
        }

        [Test]
        public void TestKeyPress_ChangesNullToValue()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ValueOrNull = null;
            //-------------Assert Preconditions -------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
            //---------------Execute Test ----------------------
            EventHelper.RaiseEvent(dateTimePicker, "KeyDown", GetKeyDownEventArgsForOtherKey());
            //---------------Test Result -----------------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
        }

        [Test]
        public void TestKeyPress_DeleteChangesValueToNull()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ValueOrNull = DateTime.Now;
            //-------------Assert Preconditions -------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            //---------------Execute Test ----------------------
            EventHelper.RaiseEvent(dateTimePicker, "KeyDown", GetKeyDownEventArgsForDeleteKey());
            //---------------Test Result -----------------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
        }

        [Test]
        public void TestKeyPress_BackSpaceChangesValueToNull()
        {
            //---------------Set up test pack-------------------
            IDateTimePicker dateTimePicker = CreateDateTimePicker();
            dateTimePicker.ValueOrNull = DateTime.Now;
            //-------------Assert Preconditions -------------
            Assert.IsTrue(dateTimePicker.ValueOrNull.HasValue);
            //---------------Execute Test ----------------------
            EventHelper.RaiseEvent(dateTimePicker, "KeyDown", GetKeyDownEventArgsForBackspaceKey());
            //---------------Test Result -----------------------
            Assert.IsFalse(dateTimePicker.ValueOrNull.HasValue);
        }

        #endregion User Interaction Tests

    }
}
