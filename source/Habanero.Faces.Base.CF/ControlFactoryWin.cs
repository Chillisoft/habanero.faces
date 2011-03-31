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
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Base.Util;
using Habanero.BO;
using Habanero.Faces.Base;
using Habanero.Faces.Base;
using Habanero.Util;
using MessageBoxDefaultButton = System.Windows.Forms.MessageBoxDefaultButton;
using ScrollBars=System.Windows.Forms.ScrollBars;

namespace Habanero.Faces.Win
{
    /// <summary>
    /// Creates controls for the System.Windows.Forms UI environment
    /// </summary>
    public class ControlFactoryWin : IControlFactory
    {
        //This looks like it was planned to move common functionality between Win and Giz to a 
        // manger but this has obviosly not been implemented
        //I would suggest it is remvoed Brett 24/02/2009
        //private readonly ControlFactoryManager _manager;
        //        /<summary>
        //        / Construct <see cref="ControlFactoryWin"/>
        //        /</summary>
        //        public virtual ControlFactoryWin()
        //        {
        //            //_manager = new ControlFactoryManager(this);
        //        }
        


        #region IControlFactory Members



        /// <summary>
        /// Creates a TextBox control
        /// </summary>
        public virtual TextBox CreateTextBox()
        {
            throw new NotImplementedException("CF Not implemented");
            //return new TextBoxWin();
        }


        /// <summary>
        /// Creates a control for the given type and assembly name
        /// </summary>
        /// <param name="typeName">The name of the control type</param>
        /// <param name="assemblyName">The assembly name of the control type</param>
        /// <returns>Returns either the control of the specified type or
        /// the default type, which is usually TextBox.</returns>
        public virtual Control CreateControl(string typeName, string assemblyName)
        {
            Type controlType = null;

            if (String.IsNullOrEmpty(typeName)) return CreateControl(typeof (TextBox));

            if (String.IsNullOrEmpty(assemblyName))
            {
                assemblyName = "System.Windows.Forms";
            }
            TypeLoader.LoadClassType(ref controlType, assemblyName, typeName,
                                         "field", "field definition");
            

            return CreateControl(controlType);
        }

        /// <summary>
        /// Creates a new control of the type specified
        /// </summary>
        /// <param name="controlType">The control type, which must be a
        /// sub-type of <see cref="Control"/></param>
        public virtual Control CreateControl(Type controlType)
        {
            Control ctl;
            if (controlType.IsSubclassOf(typeof (Control)))
            {
                if (controlType == typeof (ComboBox)) return CreateComboBox();
                if (controlType == typeof (CheckBox)) return CreateCheckBox();
                if (controlType == typeof (TextBox)) return CreateTextBox();
                if (controlType == typeof (ListBox)) return CreateListBox();
                if (controlType == typeof (DateTimePicker)) return CreateDateTimePicker();
                if (controlType == typeof (NumericUpDown)) return CreateNumericUpDownInteger();

                ctl = (Control) Activator.CreateInstance(controlType);
                PropertyInfo infoFlatStyle =
                    ctl.GetType().GetProperty("FlatStyle", BindingFlags.Public | BindingFlags.Instance);
                if (infoFlatStyle != null)
                {
                    infoFlatStyle.SetValue(ctl, new object(), new object[] {});
                }
            }
            else
            {
                throw new UnknownTypeNameException(
                    String.Format(
                        "The control type name {0} does not inherit from {1}.", controlType.FullName, typeof (Control)));
            }
            return ctl;
        }

        /// <summary>
        /// Creates a new DateTimePicker with a specified date
        /// </summary>
        /// <param name="defaultDate">The initial date value</param>
        public virtual DateTimePicker CreateDateTimePicker(DateTime defaultDate)
        {
            DateTimePicker dateTimePickerWin = CreateDateTimePicker();
            dateTimePickerWin.Value = defaultDate;
            return dateTimePickerWin;
        }

        /// <summary>
        /// Creates a new DateTimePicker that is formatted to handle months
        /// and years
        /// </summary>
        /// <returns>Returns a new DateTimePicker object</returns>
        public virtual DateTimePicker CreateMonthPicker()
        {
                            throw new NotImplementedException("CF Not implemented");
/*            DateTimePickerWin editor = (DateTimePickerWin)CreateDateTimePicker();
            editor.Format = (System.Windows.Forms.DateTimePickerFormat) DateTimePickerFormat.Custom;
            editor.CustomFormat = "MMM yyyy";
            return editor;*/
        }

        ///<summary>
        /// Creates a new numeric up-down control
        ///</summary>
        ///<returns>The created NumericUpDown control</returns>
        public virtual NumericUpDown CreateNumericUpDown()
        {
            return new NumericUpDown();
        }

        /// <summary>
        /// Creates a new numeric up-down control that is formatted with
        /// zero decimal places for integer use
        /// </summary>
        public virtual NumericUpDown CreateNumericUpDownInteger()
        {
            NumericUpDown ctl = CreateNumericUpDown();
            ctl.Maximum = Int32.MaxValue;
            ctl.Minimum = Int32.MinValue;
            return ctl;
        }

        /// <summary>
        /// Creates a new numeric up-down control that is formatted with
        /// two decimal places for currency use
        /// </summary>
        public virtual NumericUpDown CreateNumericUpDownCurrency()
        {
            NumericUpDown ctl = CreateNumericUpDown();
            ctl.Maximum = Decimal.MaxValue;
            ctl.Minimum = Decimal.MinValue;
            return ctl;
        }

        /// <summary>
        /// Creates a new progress bar
        /// </summary>
        public virtual ProgressBar CreateProgressBar()
        {
            return new ProgressBar();
        }

        /// <summary>
        /// Creates a new splitter which enables the user to resize 
        /// docked controls
        /// </summary>
        public virtual Splitter CreateSplitter()
        {
            return new Splitter();
        }

        /// <summary>
        /// Creates a new tab page
        /// </summary>
        /// <param name="title">The page title to appear in the tab</param>
        public virtual TabPage CreateTabPage(string title)
        {
            return new TabPage {Text = title, Name = title};
        }

        /// <summary>
        /// Creates a new radio button
        /// </summary>
        /// <param name="text">The text to appear next to the radio button</param>
        public virtual RadioButton CreateRadioButton(string text)
        {
            RadioButton rButton = new RadioButton();
            rButton.Text = text;
            return rButton;
        }


        /// <summary>
        /// Creates a TabControl
        /// </summary>
        public virtual TabControl CreateTabControl()
        {
            return new TabControl();
        }

        /// <summary>
        /// Creates a multi line textbox, setting the scrollbars to vertical
        /// </summary>
        /// <param name="numLines">The number of lines to show in the TextBox</param>
        public virtual TextBox CreateTextBoxMultiLine(int numLines)
        {
            TextBox tb = (TextBox) CreateTextBox();
            tb.Multiline = true;
            tb.AcceptsReturn = true;
            tb.Height = tb.Height*numLines;
            tb.ScrollBars = ScrollBars.Vertical;
            return tb;
        }
/*

        /// <summary>
        /// Creates a control that can be placed on a form or a panel to implement a wizard user interface.
        /// The wizard control will have a next and previous button and a panel to place the wizard step on.
        /// </summary>
        /// <param name="wizardController">The controller that manages the wizard process</param>
        public virtual IWizardControl CreateWizardControl(IWizardController wizardController)
        {
            return new WizardControlWin(wizardController, this);
        }

        /// <summary>
        /// Creates a form that will be used to display the wizard user interface.
        /// </summary>
        /// <param name="wizardController"></param>
        /// <returns></returns>
        public virtual IWizardForm CreateWizardForm(IWizardController wizardController)
        {
            return new WizardFormWin(wizardController, this);
        }

        /// <summary>
        /// Creates a form that will be used to display the wizard user interface.
        /// </summary>
        /// <param name="wizardControl">The Wizard control that will be displayed on the form</param>
        /// <returns></returns>
        public virtual IWizardForm CreateWizardForm(IWizardControl wizardControl)
        {
            return new WizardFormWin((WizardControlWin) wizardControl);
        }
        /// <summary>
        /// Creates a form in which a business object can be edited
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <param name="uiDefName">The name of the set of UI definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a UI definition with no name attribute specified.</param>
        /// <param name="action">Action to be performed when the editing is completed or cancelled. Typically used if you want to update
        /// a grid or a list in an asynchronous environment (E.g. to select the recently edited item in the grid)</param>
        public virtual IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo, string uiDefName,
                                                       PostObjectEditDelegate action)
        {
            return new DefaultBOEditorFormWin(bo, uiDefName, this, action);
        }

        /// <summary>
        /// Creates a form in which a business object can be edited
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <param name="uiDefName">The name of the set of UI definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a UI definition with no name attribute specified.</param>
        /// <param name="groupControlCreator">The Creator that will be used to Create the <see cref="IGroupControl"/></param>
        public virtual IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo, string uiDefName, GroupControlCreator groupControlCreator)
        {
            return new DefaultBOEditorFormWin(bo, uiDefName, this, groupControlCreator);
        }

        /// <summary>
        /// Returns a BOEditor form. This is a form that the business object can be edited in  a grid, list etc in an asynchronous environment. E.g. to select the recently edited item in the grid
        /// </summary>
        /// <param name="bo"> </param>
        /// <returns></returns>
        public virtual IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo)
        {
            return new DefaultBOEditorFormWin(bo, "default", this);
        }

        /// <summary>
        /// Creates a form in which a business object can be edited
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <param name="uiDefName">The name of the set of UI definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a UI definition with no name attribute specified.</param>
        public virtual IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo, string uiDefName)
        {
            return new DefaultBOEditorFormWin(bo, uiDefName, this);
        }

        //TODO: Port
//        public virtual IListView CreateListView()
//        {
//            throw new NotImplementedException();
//        }

        /// <summary>
        /// Creates an editable grid
        /// </summary>
        public virtual IEditableGrid CreateEditableGrid()
        {
            return new EditableGridWin();
        }

        /// <summary>
        /// Creates an EditableGridControl
        /// </summary>
        public virtual IEditableGridControl CreateEditableGridControl()
        {
            return new EditableGridControlWin(this);
        }
*/



        /// <summary>
        /// Creates a control mapper strategy for the management of how
        /// business object properties and their related controls update each other.
        /// For example, a windows strategy might be to update the control value whenever the property 
        /// is updated, whereas an internet strategy might be to update the control value only
        /// when the business object is loaded.
        /// </summary>
        public virtual IControlMapperStrategy CreateControlMapperStrategy()
        {
            return new ControlMapperStrategyWin();
        }

        /// <summary>
        /// Returns a textbox mapper strategy that can be applied to a textbox
        /// </summary>
        public virtual ITextBoxMapperStrategy CreateTextBoxMapperStrategy()
        {
            return new TextBoxMapperStrategyWin();
        }

        /// <summary>
        /// Creates an ErrorProvider
        /// </summary>
        public virtual IErrorProvider CreateErrorProvider()
        {
            return new NullErrorProvider();
        }

        /// <summary>
        /// Creates a Form control
        /// </summary>
        public virtual Form CreateForm()
        {
            return new Form();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of a CheckBox for the environment
        /// </summary>
        public virtual ICheckBoxMapperStrategy CreateCheckBoxMapperStrategy()
        {
            return new CheckBoxStrategyWin();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of a ComboBox for the environment
        /// </summary>
        public virtual IListComboBoxMapperStrategy CreateListComboBoxMapperStrategy()
        {
            return new ListComboBoxMapperStrategyWin();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of a lookup ComboBox for the environment
        /// </summary>
        public virtual IComboBoxMapperStrategy CreateLookupComboBoxDefaultMapperStrategy()
        {
            return new ComboBoxDefaultMapperStrategyWin();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of key presses on a lookup ComboBox for the environment
        /// </summary>
        public virtual IComboBoxMapperStrategy CreateLookupKeyPressMapperStrategy()
        {
            return new ComboBoxKeyPressMapperStrategyWin();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of a NumericUpDown for the environment
        /// </summary>
        public virtual INumericUpDownMapperStrategy CreateNumericUpDownMapperStrategy()
        {
            return new NumericUpDownMapperStrategyWin();
        }


        ///<summary>
        /// Displays a message box with specified text, caption, buttons, and icon.
        ///</summary>
        ///<param name="message">The text to display in the message box.</param>
        ///<param name="title">The text to display in the title bar of the message box.</param>
        ///<param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
        ///<param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the message box.</param>
        ///<returns>The message box result.</returns>
        public virtual DialogResult ShowMessageBox(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return (DialogResult)MessageBox.Show(message, title, 
                (System.Windows.Forms.MessageBoxButtons)buttons, (System.Windows.Forms.MessageBoxIcon)icon, MessageBoxDefaultButton.Button1);
        }
        
        ///<summary>
        /// Displays a message box with specified text, caption, buttons, and icon.
        /// Once the user is has responded, the provided delegate is called with an indication of the <see cref="DialogResult"/>.
        ///</summary>
        ///<param name="message">The text to display in the message box.</param>
        ///<param name="title">The text to display in the title bar of the message box.</param>
        ///<param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
        ///<param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the message box.</param>
        ///<param name="dialogCompletionDelegate">A delegate to be called when the MessageBox has been completed.</param>
        ///<returns>The message box result.</returns>
        public virtual DialogResult ShowMessageBox(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon, DialogCompletionDelegate dialogCompletionDelegate)
        {
            System.Windows.Forms.MessageBoxButtons messageBoxButtons = (System.Windows.Forms.MessageBoxButtons)buttons;
            System.Windows.Forms.MessageBoxIcon messageBoxIcon = (System.Windows.Forms.MessageBoxIcon)icon;
            DialogResult dialogResult = (DialogResult)MessageBox.Show(message, title, messageBoxButtons, messageBoxIcon, MessageBoxDefaultButton.Button1);
            dialogCompletionDelegate(null, dialogResult);
            return dialogResult;
        }

        ///<summary>
        /// Displays a message box with specified text.
        ///</summary>
        ///<param name="message">The text to display in the message box.</param>
        ///<returns>The message box result.</returns>
        public virtual DialogResult ShowMessageBox(string message)
        {
            Cursor.Current = Cursors.Default;
            return (DialogResult)MessageBox.Show(message);
        }

        /// <summary>
        /// Creates a new empty ComboBox
        /// </summary>
        public virtual ComboBox CreateComboBox()
        {
            ComboBox comboBoxWin = new ComboBox();
            return comboBoxWin;
        }

        /// <summary>
        /// Creates a ListBox control
        /// </summary>
        /// <returns></returns>
        public virtual ListBox CreateListBox()
        {
            return new ListBox();
        }

/*        /// <summary>
        /// Creates a multi-selector control
        /// </summary>
        /// <typeparam name="T">The business object type being managed in the control</typeparam>
        public virtual IMultiSelector<T> CreateMultiSelector<T>()
        {
            return new MultiSelectorWin<T>(this);
        }*/

        /// <summary>
        /// Creates a button control
        /// </summary>
        public virtual Button CreateButton()
        {
            return new Button();
        }

        /// <summary>
        /// Creates a button control
        /// </summary>
        /// <param name="text">The text to appear on the button</param>
        public virtual Button CreateButton(string text)
        {
            Button button = CreateButton();
            button.Text = text;
            button.Name = text;
            return button;
        }

        /// <summary>
        /// Creates a button control with an attached event handler to carry out
        /// further actions if the button is pressed
        /// </summary>
        /// <param name="text">The text to appear on the button</param>
        /// <param name="clickHandler">The method that handles the Click event</param>
        public virtual Button CreateButton(string text, EventHandler clickHandler)
        {
            Button button = CreateButton(text);
            button.Click += clickHandler;
            return button;
        }

        /// <summary>
        /// Creates a CheckBox control
        /// </summary>
        public virtual CheckBox CreateCheckBox()
        {
            return new CheckBox();
        }

        /// <summary>
        /// Creates a CheckBox control with a specified initial checked state
        /// </summary>
        /// <param name="defaultValue">Whether the initial box is checked</param>
        public virtual CheckBox CreateCheckBox(bool defaultValue)
        {
            CheckBox checkBox = CreateCheckBox();
            checkBox.Checked = defaultValue;
            return checkBox;
        }

        /// <summary>
        /// Creates a label without text
        /// </summary>
        public virtual Label CreateLabel()
        {
            Label label = new Label();
            label.TabStop = false;
            return label;
        }

        /// <summary>
        /// Creates a label with specified text
        /// </summary>
        public virtual Label CreateLabel(string labelText)
        {
            Label label = CreateLabel(labelText, false);
            label.Text = labelText;
            return label;
        }

        /// <summary>
        /// Creates a label
        /// </summary>
        /// <param name="labelText">The text to appear in the label</param>
        /// <param name="isBold">Whether the text appears in bold font</param>
        public virtual Label CreateLabel(string labelText, bool isBold)
        {
            Label label = (Label) CreateLabel();
            label.Text = labelText;
            if (isBold)
            {
                //label.Font = new Font(label.Font, 12.4, FontStyle.Bold);
            }
            if (isBold)
            {
                label.Width += 10;
            }
            label.TextAlign = ContentAlignment.TopCenter;
            label.TabStop = false;
            return label;
        }

        /// <summary>
        /// Creates a DateTimePicker
        /// </summary>
        public virtual DateTimePicker CreateDateTimePicker()
        {
            return new DateTimePicker();
        }

        /// <summary>
        /// Creates a Panel control
        /// </summary>
        public virtual Panel CreatePanel()
        {
            return new Panel();
        }

        /// <summary>
        /// Creates a Panel control
        /// </summary>
        /// <param name="controlFactory">The factory that this panel will use to create any controls on it</param>
        public virtual Panel CreatePanel(IControlFactory controlFactory)
        {
            return new Panel();
        }

        /// <summary>
        /// Creates a Panel control
        /// </summary>
        /// <param name="name">The name of the panel</param>
        /// <param name="controlFactory">The factory that this panel will use to create any controls on it</param>
        public virtual Panel CreatePanel(string name, IControlFactory controlFactory)
        {
            Panel panel = CreatePanel();
            panel.Name = name;
            return panel;
        }

        /// <summary>
        /// Creates a new PasswordTextBox that masks the letters as the user
        /// types them
        /// </summary>
        /// <returns>Returns the new PasswordTextBox object</returns>
        public virtual TextBox CreatePasswordTextBox()
        {
            TextBox tb = CreateTextBox();
            tb.PasswordChar = '*';
            return tb;
        }


        /// <summary>
        /// Creates a generic control
        /// </summary>
        public virtual Control CreateControl()
        {
            Control control = new Control();
            control.Size = new Size(100, 10);
            return control;
        }

        /// <summary>
        /// Creates a user control
        /// </summary>
        public virtual UserControl CreateUserControl()
        {
            return new UserControl();
        }

        /// <summary>
        /// Creates a user control with the specified name.
        /// </summary>
        public virtual UserControl CreateUserControl(string name)
        {
            UserControl userControlHabanero = CreateUserControl();
            userControlHabanero.Name = name;
            return userControlHabanero;
        }

        #endregion

        /// <summary>
        /// Creates a TextBox that provides filtering of characters depending on the property type.
        /// </summary>
        /// <param name="propertyType">Type property being edited.</param>
        public virtual TextBox CreateTextBox(Type propertyType)
        {
            return new TextBox();
        }

        /// <summary>
        /// Creates a TextBox that provides filtering of characters depending on the property type.
        /// </summary>
         public virtual PictureBox CreatePictureBox()
        {
            return new PictureBox();
        }
    }

}