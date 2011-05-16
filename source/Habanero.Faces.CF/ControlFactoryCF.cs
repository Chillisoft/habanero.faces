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
using System.Reflection;
using System.Windows.Forms;
using Habanero.Base.Exceptions;
using Habanero.Faces.Adapters;
using Habanero.Faces.Base;
using Habanero.Faces.Controls;
using Habanero.Faces.Mappers;
using Habanero.Util;
using DialogResult = Habanero.Faces.Base.DialogResult;
using MessageBoxButtons = Habanero.Faces.Base.MessageBoxButtons;
using MessageBoxDefaultButton = System.Windows.Forms.MessageBoxDefaultButton;
using MessageBoxIcon = Habanero.Faces.Base.MessageBoxIcon;

namespace Habanero.Faces
{
    /// <summary>
    /// Creates controls for the System.Windows.Forms UI environment
    /// </summary>
    public class ControlFactoryCF : IControlFactory
    {
        //This looks like it was planned to move common functionality between Win and Giz to a 
        // manger but this has obviosly not been implemented
        //I would suggest it is remvoed Brett 24/02/2009
        //private readonly ControlFactoryManager _manager;
        //        /<summary>
        //        / Construct <see cref="ControlFactoryCF"/>
        //        /</summary>
        //        public virtual ControlFactoryCF()
        //        {
        //            //_manager = new ControlFactoryManager(this);
        //        }
        


        #region IControlFactory Members



        /// <summary>
        /// Creates a TextBox control
        /// </summary>
        public virtual ITextBox CreateTextBox()
        {
            return new WinFormsTextBoxAdapter(new TextBox());
        }


        /// <summary>
        /// Creates a control for the given type and assembly name
        /// </summary>
        /// <param name="typeName">The name of the control type</param>
        /// <param name="assemblyName">The assembly name of the control type</param>
        /// <returns>Returns either the control of the specified type or
        /// the default type, which is usually TextBox.</returns>
        public virtual IControlHabanero CreateControl(string typeName, string assemblyName)
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
        /// sub-type of <see cref="IControlHabanero"/></param>
        public virtual IControlHabanero CreateControl(Type controlType)
        {
            IControlHabanero ctl;
            if (controlType.IsSubclassOf(typeof (Control)))
            {
                if (controlType == typeof (ComboBox)) return CreateComboBox();
                if (controlType == typeof (CheckBox)) return CreateCheckBox();
                if (controlType == typeof (TextBox)) return CreateTextBox();
                if (controlType == typeof (ListBox)) return CreateListBox();
                if (controlType == typeof (DateTimePicker)) return CreateDateTimePicker();
                if (controlType == typeof (NumericUpDown)) return CreateNumericUpDownInteger();

                ctl = (IControlHabanero) Activator.CreateInstance(controlType);
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
        public virtual IDateTimePicker CreateDateTimePicker(DateTime defaultDate)
        {
            IDateTimePicker dateTimePickerWin = CreateDateTimePicker();
            dateTimePickerWin.Value = defaultDate;
            return dateTimePickerWin;
        }

        /// <summary>
        /// Creates a new DateTimePicker that is formatted to handle months
        /// and years
        /// </summary>
        /// <returns>Returns a new DateTimePicker object</returns>
        public virtual IDateTimePicker CreateMonthPicker()
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
        public virtual INumericUpDown CreateNumericUpDown()
        {
                            throw new NotImplementedException("CF Not implemented");
            //return new NumericUpDownWin();
        }

        /// <summary>
        /// Creates a new numeric up-down control that is formatted with
        /// zero decimal places for integer use
        /// </summary>
        public virtual INumericUpDown CreateNumericUpDownInteger()
        {
            INumericUpDown ctl = CreateNumericUpDown();
            ctl.DecimalPlaces = 0;
            ctl.Maximum = Int32.MaxValue;
            ctl.Minimum = Int32.MinValue;
            return ctl;
        }

        /// <summary>
        /// Creates a new numeric up-down control that is formatted with
        /// two decimal places for currency use
        /// </summary>
        public virtual INumericUpDown CreateNumericUpDownCurrency()
        {
            INumericUpDown ctl = CreateNumericUpDown();
            ctl.DecimalPlaces = 2;
            ctl.Maximum = Decimal.MaxValue;
            ctl.Minimum = Decimal.MinValue;
            return ctl;
        }

        /// <summary>
        /// Creates a new progress bar
        /// </summary>
        public virtual IProgressBar CreateProgressBar()
        {
            throw new NotImplementedException("Not implemented for CF");
            //return new ProgressBarWin();
        }

        /// <summary>
        /// Creates a new splitter which enables the user to resize 
        /// docked controls
        /// </summary>
        public virtual ISplitter CreateSplitter()
        {
            throw new NotImplementedException("Not implemented for CF");
            //return new SplitterWin();
        }

        /// <summary>
        /// Creates a new tab page
        /// </summary>
        /// <param name="title">The page title to appear in the tab</param>
        public virtual ITabPage CreateTabPage(string title)
        {
            throw new NotImplementedException("CF Not implemented");
           // return new TabPageWin {Text = title, Name = title};
        }

        /// <summary>
        /// Creates a new radio button
        /// </summary>
        /// <param name="text">The text to appear next to the radio button</param>
        public virtual IRadioButton CreateRadioButton(string text)
        {
            throw new NotImplementedException("Not implemented for CF");
/*            RadioButtonWin rButton = new RadioButtonWin();
            rButton.Text = text;
            //TODO_REmoved when porting rButton.AutoCheck = true;
            //TODO_REmoved when portingrButton.FlatStyle = FlatStyle.Standard;
            rButton.Width = CreateLabel(text, false).PreferredWidth + 25;
            return rButton;*/
        }


        /// <summary>
        /// Creates a TabControl
        /// </summary>
        public virtual ITabControl CreateTabControl()
        {
            throw new NotImplementedException("Not implemented for CF");
            //return new TabControlWin();
        }

        /// <summary>
        /// Creates a multi line textbox, setting the scrollbars to vertical
        /// </summary>
        /// <param name="numLines">The number of lines to show in the TextBox</param>
        public virtual ITextBox CreateTextBoxMultiLine(int numLines)
        {
            throw new NotImplementedException("Not implemented for CF");
/*            TextBoxWin tb = (TextBoxWin) CreateTextBox();
            tb.Multiline = true;
            tb.AcceptsReturn = true;
            tb.Height = tb.Height*numLines;
            tb.ScrollBars = ScrollBars.Vertical;
            return tb;*/
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
        /// Creates a FileChooser control
        /// </summary>
        public virtual IFileChooser CreateFileChooser()
        {
            throw new NotImplementedException("CF Not implemented");
           // return new FileChooserWin(this);
        }
/*

        /// <summary>
        /// Displays a business object collection in a tab control, with one
        /// business object per tab.  Each tab holds a business control, provided
        /// by the developer, that refreshes to display the business object for
        /// the current tab.
        /// <br/>
        /// This control is suitable for a business object collection with a limited
        /// number of objects.
        /// </summary>
        public virtual IBOColTabControl CreateBOColTabControl()
        {
            return new BOColTabControlWin(this);
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
            return new ControlMapperStrategyCF();
        }

        /// <summary>
        /// Returns a textbox mapper strategy that can be applied to a textbox
        /// </summary>
        public virtual ITextBoxMapperStrategy CreateTextBoxMapperStrategy()
        {
            return new TextBoxMapperStrategyCF();
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
        public virtual IFormHabanero CreateForm()
        {
            throw new NotImplementedException("CF Not implemented");
            //return new FormWin();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of a CheckBox for the environment
        /// </summary>
        public virtual ICheckBoxMapperStrategy CreateCheckBoxMapperStrategy()
        {
            return new CheckBoxStrategyCF();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of a ComboBox for the environment
        /// </summary>
        public virtual IListComboBoxMapperStrategy CreateListComboBoxMapperStrategy()
        {
            return new ListComboBoxMapperStrategyCF();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of a lookup ComboBox for the environment
        /// </summary>
        public virtual IComboBoxMapperStrategy CreateLookupComboBoxDefaultMapperStrategy()
        {

           // throw new NotImplementedException("CF Not implemented");
            return new ComboBoxDefaultMapperStrategyCF();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of key presses on a lookup ComboBox for the environment
        /// </summary>
        public virtual IComboBoxMapperStrategy CreateLookupKeyPressMapperStrategy()
        {

            return new ComboBoxKeyPressMapperStrategyCF();
        }

        /// <summary>
        /// Creates a strategy that customises behaviour of a NumericUpDown for the environment
        /// </summary>
        public virtual INumericUpDownMapperStrategy CreateNumericUpDownMapperStrategy()
        {
            throw new NotImplementedException("CF Not implemented");
           // return new NumericUpDownMapperStrategyWin();
        }

        /// <summary>
        /// Creates an OKCancelDialog
        /// </summary>
        public virtual IOKCancelDialogFactory CreateOKCancelDialogFactory()
        {
            throw new NotImplementedException("CF Not implemented");
           // return new OKCancelDialogFactoryWin(this);
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
        public virtual IComboBox CreateComboBox()
        {

            return new WinFormsComboBoxAdapter(new ComboBox());
/*            ComboBoxWin comboBoxWin = new ComboBoxWin();
            //Note_: This is a workaround in windows to avoid this default from breaking all the tests because if the Thread's ApartmentState is not STA then setting the AutoCompleteSource default gives an error
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                comboBoxWin.AutoCompleteSource =  AutoCompleteSource.ListItems;
                comboBoxWin.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }
            return comboBoxWin;*/
        }

        /// <summary>
        /// Creates a ListBox control
        /// </summary>
        /// <returns></returns>
        public virtual IListBox CreateListBox()
        {
            throw new NotImplementedException("CF Not implemented");
           // return new ListBoxWin();
        }


        /// <summary>
        /// Creates a button control
        /// </summary>
        public virtual IButton CreateButton()
        {throw new NotImplementedException("CF Not implemented");
            //return new ButtonWin();
        }

        /// <summary>
        /// Creates a button control
        /// </summary>
        /// <param name="text">The text to appear on the button</param>
        public virtual IButton CreateButton(string text)
        {
            IButton button = CreateButton();
            button.Text = text;
            button.Name = text;
            //((Button)button).FlatStyle = FlatStyle.Standard;
            button.Width = CreateLabel(text, false).PreferredWidth + 20;
            return button;
        }

        /// <summary>
        /// Creates a button control with an attached event handler to carry out
        /// further actions if the button is pressed
        /// </summary>
        /// <param name="text">The text to appear on the button</param>
        /// <param name="clickHandler">The method that handles the Click event</param>
        public virtual IButton CreateButton(string text, EventHandler clickHandler)
        {
            IButton button = CreateButton(text);
            button.Click += clickHandler;
            return button;
        }

        /// <summary>
        /// Creates a CheckBox control
        /// </summary>
        public virtual ICheckBox CreateCheckBox()
        {
            throw new NotImplementedException("CF Not implemented");
            //return new CheckBoxWin();
        }

        /// <summary>
        /// Creates a CheckBox control with a specified initial checked state
        /// </summary>
        /// <param name="defaultValue">Whether the initial box is checked</param>
        public virtual ICheckBox CreateCheckBox(bool defaultValue)
        {
            ICheckBox checkBox = CreateCheckBox();
            checkBox.Checked = defaultValue;
            return checkBox;
        }

        /// <summary>
        /// Creates a label without text
        /// </summary>
        public virtual ILabel CreateLabel()
        {
            throw new NotImplementedException("CF Not implemented");
/*            ILabel label = new LabelWin();
            label.TabStop = false;
            return label;*/
        }

        /// <summary>
        /// Creates a label with specified text
        /// </summary>
        public virtual ILabel CreateLabel(string labelText)
        {
            ILabel label = CreateLabel(labelText, false);
            label.Text = labelText;
            return label;
        }

        /// <summary>
        /// Creates a label
        /// </summary>
        /// <param name="labelText">The text to appear in the label</param>
        /// <param name="isBold">Whether the text appears in bold font</param>
        public virtual ILabel CreateLabel(string labelText, bool isBold)
        {
            var label = new Label();
            label.Text = labelText;
            if (isBold)
            {
                label.Font = new Font(label.Font.Name, label.Font.Size, FontStyle.Bold);
            }
            //label.Width = label.PreferredWidth;
            if (isBold)
            {
                label.Width += 10;
            }
            label.TextAlign = ContentAlignment.TopCenter;
            label.TabStop = false;
            return new WinFormsLabelAdapter(label);
        }

        /// <summary>
        /// Creates a DateTimePicker
        /// </summary>
        public virtual IDateTimePicker CreateDateTimePicker()
        {
            throw new NotImplementedException("CF Not implemented");
           // return new DateTimePickerWin(this);
        }

        /// <summary>
        /// Creates a Panel control
        /// </summary>
        public virtual IPanel CreatePanel()
        {throw new NotImplementedException("CF Not implemented");
         //   return new PanelWin();
        }

        /// <summary>
        /// Creates a Panel control
        /// </summary>
        /// <param name="controlFactory">The factory that this panel will use to create any controls on it</param>
        public virtual IPanel CreatePanel(IControlFactory controlFactory)
        {throw new NotImplementedException("CF Not implemented");
          //  return new PanelWin();
        }

        /// <summary>
        /// Creates a Panel control
        /// </summary>
        /// <param name="name">The name of the panel</param>
        /// <param name="controlFactory">The factory that this panel will use to create any controls on it</param>
        public virtual IPanel CreatePanel(string name, IControlFactory controlFactory)
        {
            IPanel panel = CreatePanel();
            panel.Name = name;
            return panel;
        }

        /// <summary>
        /// Creates a new PasswordTextBox that masks the letters as the user
        /// types them
        /// </summary>
        /// <returns>Returns the new PasswordTextBox object</returns>
        public virtual ITextBox CreatePasswordTextBox()
        {
            ITextBox tb = CreateTextBox();
            tb.PasswordChar = '*';
            return tb;
        }

        /// <summary>
        /// Creates a ToolTip
        /// </summary>
        public virtual IToolTip CreateToolTip()
        {throw new NotImplementedException("CF Not implemented");
         //   return new ToolTipWin();
        }

        /// <summary>
        /// Creates a generic control
        /// </summary>
        public virtual IControlHabanero CreateControl()
        {
            IControlHabanero control = new WinFormsControlAdapter(new Control());
            //IControlHabanero control = new ControlWin();
            control.Size = new Size(100, 10);
            return control;
        }

        /// <summary>
        /// Creates a user control
        /// </summary>
        public virtual IUserControlHabanero CreateUserControl()
        {throw new NotImplementedException("CF Not implemented");
         //   return new UserControlWin();
        }

        /// <summary>
        /// Creates a user control with the specified name.
        /// </summary>
        public virtual IUserControlHabanero CreateUserControl(string name)
        {
            IUserControlHabanero userControlHabanero = CreateUserControl();
            userControlHabanero.Name = name;
            return userControlHabanero;
        }

        #endregion

        /// <summary>
        /// Creates a TextBox that provides filtering of characters depending on the property type.
        /// </summary>
        /// <param name="propertyType">Type property being edited.</param>
        public virtual ITextBox CreateTextBox(Type propertyType)
        {throw new NotImplementedException("CF Not implemented");
         //   return new TextBoxWin();
        }

        /// <summary>
        /// Creates a TextBox that provides filtering of characters depending on the property type.
        /// </summary>
         public virtual IPictureBox CreatePictureBox()
        {throw new NotImplementedException("CF Not implemented");
            //return new PictureBoxWin();
        }

        ///<summary>
        /// Creates a <see cref="IDateTimePickerMapperStrategy"/>
        ///</summary>
        public virtual IDateTimePickerMapperStrategy CreateDateTimePickerMapperStrategy()
        {throw new NotImplementedException("CF Not implemented");
          //  return new DateTimePickerMapperStrategyWin();
        }
       
        /// <summary>
        /// Creates a new TabPage
        /// </summary>
        public virtual ITabPage createTabPage(string name)
        {
            throw new NotImplementedException();
        }

        #region Collapsible Panel Button Creators

        ///<summary>
        /// Creates a <see cref="IButton"/> configured with the collapsible style
        ///</summary>
        ///<returns>a <see cref="IButton"/> </returns>
        public virtual IButton CreateButtonCollapsibleStyle()
        {throw new NotImplementedException("CF Not implemented");
/*            ButtonWin button = (ButtonWin)CreateButton();
            ConfigureCollapsibleStyleButton(button);
            return button;*/
        }

        private static void ConfigureCollapsibleStyleButton(IButton button)
        {throw new NotImplementedException("CF Not implemented");
/*            ButtonWin buttonWin = ((ButtonWin)button);
            buttonWin.BackgroundImage = CollapsiblePanelResource.headergradient;
            buttonWin.FlatStyle = FlatStyle.Flat;*/
        }

        ///<summary>
        /// Creates a <see cref="ILabel"/> configured with the collapsible style
        ///</summary>
        ///<returns>a <see cref="ILabel"/> </returns>
        public virtual ILabel CreateLabelPinOffStyle()
        {throw new NotImplementedException("CF Not implemented");
/*            LabelWin label = (LabelWin)CreateLabel();
            ConfigurePinOffStyleLabel(label);
            return label;*/
        }

        ///<summary>
        /// Configures the <see cref="ILabel"/> with the pinoff style
        ///</summary>
        public virtual void ConfigurePinOffStyleLabel(ILabel label)
        {throw new NotImplementedException("CF Not implemented");
/*            LabelWin labelWin = (LabelWin)label;
            labelWin.BackgroundImage = CollapsiblePanelResource.pinoff_withcolour;
            labelWin.FlatStyle = FlatStyle.Flat;
            //labelWin.ForeColor = Color.White;
            labelWin.BackgroundImageLayout = ImageLayout.Center;
            labelWin.Width = 24;*/
        }

        ///<summary>
        ///</summary>
        ///<param name="label"></param>
        public virtual void ConfigurePinOnStyleLabel(ILabel label)
        {throw new NotImplementedException("CF Not implemented");
/*            LabelWin labelWin = (LabelWin)label;
            labelWin.BackgroundImage = CollapsiblePanelResource.pinon_withcolour;
            labelWin.FlatStyle = FlatStyle.Flat;
            //labelWin.ForeColor = Color.White;
            labelWin.BackgroundImageLayout = ImageLayout.Center;
            labelWin.Width = 24;*/
        }

        #endregion
        ///<summary>
        /// Creates an <see cref="IBOComboBoxSelector"/>
        ///</summary>
        ///<returns></returns>
        public virtual IBOComboBoxSelector CreateComboBoxSelector()
        {throw new NotImplementedException("CF Not implemented");
/*            ComboBoxSelectorWin comboBoxWin = new ComboBoxSelectorWin(this);
            //Note_: This is a workaround in windows to avoid this default from breaking all the tests 
            //  because if the Thread's ApartmentState is not STA then setting the AutoCompleteSource default 
            //  gives an error
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                comboBoxWin.AutoCompleteSource = AutoCompleteSource.ListItems;
                comboBoxWin.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }
            return comboBoxWin;*/
        }

        ///<summary>
        /// Creates an <see cref="IBOListBoxSelector"/>
        ///</summary>
        ///<returns></returns>
        public virtual IBOListBoxSelector CreateListBoxSelector()
        {throw new NotImplementedException("CF Not implemented");
          //  return new ListBoxSelectorWin(this);
        }

        ///<summary>
        /// Creates an <see cref="IBOCollapsiblePanelSelector"/>
        ///</summary>
        ///<returns></returns>
        public virtual IBOCollapsiblePanelSelector CreateCollapsiblePanelSelector()
        {throw new NotImplementedException("CF Not implemented");
         //   return new CollapsiblePanelSelectorWin(this);
        }

        /// <summary>
        /// Creates an <see cref="ISplitContainer"/>
        /// </summary>
        /// <returns>returns the created split container</returns>
        public virtual ISplitContainer CreateSplitContainer()
        {throw new NotImplementedException("CF Not implemented");
        //    return new SplitContainerWin();
        }

        /// <summary>
        /// Creates a <see cref="MainTitleIconControlWin"/>
        /// </summary>
        /// <returns></returns>
        public virtual IMainTitleIconControl CreateMainTitleIconControl()
        {throw new NotImplementedException("CF Not implemented");
       //     return new MainTitleIconControlWin(this);
        }

        ///<summary>
        /// Creates a <see cref="IExtendedComboBox"/>. This is essentially
        /// a disabled ComboBox with a Search button.
        ///</summary>
        ///<returns></returns>
        public IExtendedComboBox CreateExtendedComboBox()
        {throw new NotImplementedException("CF Not implemented");
         //   return new ExtendedComboBoxWin(this);
        }



    }

    public class NullErrorProvider : IErrorProvider
    {
        public string GetError(IControlHabanero objControl)
        {
            return string.Empty;
        }

        public ErrorIconAlignmentHabanero GetIconAlignment(IControlHabanero objControl)
        {
            return ErrorIconAlignmentHabanero.TopLeft;
        }

        public int GetIconPadding(IControlHabanero objControl)
        {
            return 0;
        }

        public void SetError(IControlHabanero objControl, string strValue)
        {

        }

        public void SetIconAlignment(IControlHabanero objControl, ErrorIconAlignmentHabanero enmValue)
        {
        }

        public void SetIconPadding(IControlHabanero objControl, int intPadding)
        {

        }

        public void UpdateBinding()
        {

        }

        public bool CanExtend(object objExtendee)
        {
            return false;
        }

        public int BlinkRate { get; set; }

        public ErrorBlinkStyleHabanero BlinkStyleHabanero { get; set; }

        public string DataMember { get; set; }

        public object DataSource { get; set; }
    }
}