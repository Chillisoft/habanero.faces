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
using System.Drawing;
using System.Windows.Forms;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using Habanero.Util;
using NUnit.Framework;
using DockStyle = Habanero.Faces.Base.DockStyle;

namespace Habanero.Faces.Test.Win.StandardControls
{
    [TestFixture]
    public class TestDateTimePickerWin : TestDateTimePicker
    {
        protected override void SetBaseDateTimePickerValue(IDateTimePicker dateTimePicker, DateTime value)
        {
            System.Windows.Forms.DateTimePicker picker = (System.Windows.Forms.DateTimePicker)dateTimePicker;
            picker.Value = value;
        }

        protected override void SetBaseDateTimePickerCheckedValue(IDateTimePicker dateTimePicker, bool value)
        {
            System.Windows.Forms.DateTimePicker picker = (System.Windows.Forms.DateTimePicker)dateTimePicker;
            picker.Checked = value;
        }

    	protected override void SubscribeToBaseValueChangedEvent(IDateTimePicker dateTimePicker, EventHandler onValueChanged)
    	{
            System.Windows.Forms.DateTimePicker picker = (System.Windows.Forms.DateTimePicker)dateTimePicker;
            picker.ValueChanged += onValueChanged;
    	}

    	protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override EventArgs GetKeyDownEventArgsForDeleteKey()
        {
            return new System.Windows.Forms.KeyEventArgs(System.Windows.Forms.Keys.Delete);
        }

        protected override EventArgs GetKeyDownEventArgsForBackspaceKey()
        {
            return new System.Windows.Forms.KeyEventArgs(System.Windows.Forms.Keys.Back);
        }

        protected override EventArgs GetKeyDownEventArgsForOtherKey()
        {
            return new System.Windows.Forms.KeyEventArgs(System.Windows.Forms.Keys.A);
        }

		//TODO: Add To Known Issues: There is no event that responds to changing the value of the Checkbox on the control.
        [Test]
		[Ignore("This is a known issue. There is no event that responds to changing the value of the Checkbox on the control")]
        public override void Test_BaseChecked_WhenSetToFalse_ShouldFireValueChangedEvent()
        {
        	
        }

        [Test, Ignore("Only for visual testing")]
        public void TestShowWithEvents()
        {
            //---------------Set up test pack-------------------
            System.Windows.Forms.DateTimePicker dateTimePicker = new System.Windows.Forms.DateTimePicker();
            dateTimePicker.ShowCheckBox = true;
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
            form.Controls.Add(textBox);
            textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox.Multiline = true;
            form.Controls.Add(dateTimePicker);
            dateTimePicker.Dock = System.Windows.Forms.DockStyle.Top;
            dateTimePicker.ValueChanged += delegate
                                               {
                                                   textBox.Text += "EventFired";
                                               };
            System.Windows.Forms.Button button = new System.Windows.Forms.Button();
            form.Controls.Add(button);
            button.Dock = System.Windows.Forms.DockStyle.Bottom;
            button.Click += delegate
                                {
                                    dateTimePicker.Checked = !dateTimePicker.Checked;
                                };
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            form.ShowDialog();
            //---------------Test Result -----------------------

        }

        [Test, Ignore("Only for visual testing")]
        public void TestShowDatePickerForm()
        {
            //---------------Set up test pack-------------------
            IFormHabanero formWin = new FormWin();
            IDateTimePicker dateTimePicker = GetControlFactory().CreateDateTimePicker();
            dateTimePicker.Format = Habanero.Faces.Base.DateTimePickerFormat.Custom;
            dateTimePicker.CustomFormat = @"Aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa: dd MMM yyyy";
            dateTimePicker.NullDisplayValue = "Please Click";
            //dateTimePicker.ShowCheckBox = true;
            ITextBox textBox = GetControlFactory().CreateTextBox();
            IButton button = GetControlFactory().CreateButton("Check/Uncheck", delegate
                                                                                   {
                                                                                       //dateTimePicker.Checked = !dateTimePicker.Checked;
                                                                                       if (dateTimePicker.ValueOrNull.HasValue)
                                                                                       {
                                                                                           dateTimePicker.ValueOrNull = null;
                                                                                       }
                                                                                       else
                                                                                       {
                                                                                           dateTimePicker.ValueOrNull = dateTimePicker.Value;
                                                                                       }
                                                                                   });
            IButton enableButton = GetControlFactory().CreateButton("Enable/Disable", delegate
                                                                                          {
                                                                                              dateTimePicker.Enabled = !dateTimePicker.Enabled;
                                                                                          });
            GridLayoutManager gridLayoutManager = new GridLayoutManager(formWin, GetControlFactory());
            gridLayoutManager.SetGridSize(5, 1);
            gridLayoutManager.AddControl(dateTimePicker);
            gridLayoutManager.AddControl(button);
            gridLayoutManager.AddControl(textBox);
            gridLayoutManager.AddControl(enableButton);
            gridLayoutManager.AddControl(GetControlFactory().CreateButton("ChangeColor", delegate
                                                                                             {
                                                                                                 Random random = new Random();
                                                                                                 dateTimePicker.ForeColor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                                                                                                 dateTimePicker.BackColor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                                                                                             }));
            dateTimePicker.ValueChanged += delegate
                                               {
                                                   textBox.Text = dateTimePicker.ValueOrNull.HasValue ? dateTimePicker.Value.ToString() : "";
                                               };
            //---------------Execute Test ----------------------
            formWin.ShowDialog();
            //---------------Test Result -----------------------

            //---------------Tear down -------------------------

        }

		[Test, Ignore("Only for visual testing")]
		public void TestShowDatePickersFormWithVisualStyles()
		{
			//---------------Set up test pack-------------------
			//Application.EnableVisualStyles();
			IFormHabanero formWin = new FormWin();
			IControlFactory controlFactory = GetControlFactory();
			IDateTimePicker dateTimePicker = controlFactory.CreateDateTimePicker();
			dateTimePicker.Format = Habanero.Faces.Base.DateTimePickerFormat.Custom;
			dateTimePicker.CustomFormat = @"dddd MMMM yyyy";
			dateTimePicker.NullDisplayValue = "Please Click";
			//dateTimePicker.ShowCheckBox = true;
			ITextBox textBox = controlFactory.CreateTextBox();
			IButton button = controlFactory.CreateButton("Check/Uncheck", delegate
			{
				//dateTimePicker.Checked = !dateTimePicker.Checked;
				if (dateTimePicker.ValueOrNull.HasValue)
				{
					dateTimePicker.ValueOrNull = null;
				}
				else
				{
					dateTimePicker.ValueOrNull = dateTimePicker.Value;
				}
			});
			IButton enableButton = controlFactory.CreateButton("Enable/Disable", delegate
			{
				dateTimePicker.Enabled = !dateTimePicker.Enabled;
			});
			

			IPanel panel = controlFactory.CreatePanel();
			panel.Dock = DockStyle.Fill;
			formWin.Controls.Add(panel);

			GridLayoutManager gridLayoutManager = new GridLayoutManager(panel, controlFactory);
			gridLayoutManager.SetGridSize(9, 1);
			gridLayoutManager.BorderSize = 25;
			gridLayoutManager.AddControl(dateTimePicker);
			gridLayoutManager.AddControl(button);
			gridLayoutManager.AddControl(textBox);
			gridLayoutManager.AddControl(enableButton);
			gridLayoutManager.AddControl(controlFactory.CreateButton("ChangeColor", delegate
			{
				Random random = new Random();
				dateTimePicker.ForeColor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
				dateTimePicker.BackColor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
			}));


			IDateTimePicker anotherDateTimePicker;
			anotherDateTimePicker = controlFactory.CreateDateTimePicker();
			anotherDateTimePicker.Format = Habanero.Faces.Base.DateTimePickerFormat.Long;
			gridLayoutManager.AddControl(anotherDateTimePicker);
			anotherDateTimePicker = controlFactory.CreateDateTimePicker();
			anotherDateTimePicker.Format = Habanero.Faces.Base.DateTimePickerFormat.Short;
			gridLayoutManager.AddControl(anotherDateTimePicker);
			anotherDateTimePicker = controlFactory.CreateDateTimePicker();
			anotherDateTimePicker.Format = Habanero.Faces.Base.DateTimePickerFormat.Time;
			gridLayoutManager.AddControl(anotherDateTimePicker);

			anotherDateTimePicker = controlFactory.CreateDateTimePicker();
			anotherDateTimePicker.Format = Habanero.Faces.Base.DateTimePickerFormat.Long;
			anotherDateTimePicker.Font = new Font(FontFamily.GenericSansSerif, 18);
			anotherDateTimePicker.Height = 60;
			gridLayoutManager.AddControl(anotherDateTimePicker);

			dateTimePicker.ValueChanged += delegate
			{
				textBox.Text = dateTimePicker.ValueOrNull.HasValue ? dateTimePicker.Value.ToString() : "";
			};
			//---------------Execute Test ----------------------
			formWin.ShowDialog();
			//---------------Test Result -----------------------

			//---------------Tear down -------------------------

		}
    }
}