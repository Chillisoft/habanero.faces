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
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Util;
using Habanero.BO;
using Habanero.Faces.Base.ControlMappers;

namespace Habanero.Faces.Base
{
    ///<summary>
    /// This delegate provides a signature for a method to be called when a Dialog completes.
    /// It provides a parameter for a reference to the actal dialog that completed, and 
    /// the <see cref="DialogResult"/> of the dialog.
    ///</summary>
    ///<param name="sender">A reference to the actual dialog that was completed, resulting in this delegate being called.
    /// This may be null if the particular Dialog implementation does not allow references to the Dialog type. eg. MessageBox in windows</param>
    ///<param name="dialogResult">The <see cref="DialogResult"/> of the dialog when it was completed.</param>
    public delegate void DialogCompletionDelegate(object sender, DialogResult dialogResult);

    /// <summary>
    /// Creates controls for a specific UI environment.
    /// The control Factory provides a specific piece of functionality fundamental to the 
    /// ability of Habanero to swap between Windows, Web and WPF. If the control factory is 
    /// used for creating all controls in the application, then moving the application from windows to web
    /// or vice versa is trivial. The control factory also provides a simple and easy way to 
    /// style an application: swap out the control factory and create 
    /// controls with any image, etc. you want.
    /// </summary>
    public interface IControlFactory
    {

        /// <summary>
        /// Creates a new empty ComboBox
        /// </summary>
        ComboBox CreateComboBox();

        /// <summary>
        /// Creates a ListBox control
        /// </summary>
        /// <returns></returns>
        ListBox CreateListBox();

        /// <summary>
        /// Creates a button control
        /// </summary>
        Button CreateButton();

        /// <summary>
        /// Creates a button control
        /// </summary>
        /// <param name="text">The text to appear on the button</param>
        Button CreateButton(string text);

        /// <summary>
        /// Creates a button control with an attached event handler to carry out
        /// further actions if the button is pressed
        /// </summary>
        /// <param name="text">The text to appear on the button</param>
        /// <param name="clickHandler">The method that handles the Click event</param>
        Button CreateButton(string text, EventHandler clickHandler);

        /// <summary>
        /// Creates a CheckBox control
        /// </summary>
        CheckBox CreateCheckBox();

        /// <summary>
        /// Creates a CheckBox control with a specified initial checked state
        /// </summary>
        /// <param name="defaultValue">Whether the initial box is checked</param>
        CheckBox CreateCheckBox(bool defaultValue);

        /// <summary>
        /// Creates a label without text
        /// </summary>
        Label CreateLabel();

        /// <summary>
        /// Creates a label with specified text
        /// </summary>
        Label CreateLabel(string labelText);

        /// <summary>
        /// Creates a label
        /// </summary>
        /// <param name="labelText">The text to appear in the label</param>
        /// <param name="isBold">Whether the text appears in bold font</param>
        Label CreateLabel(string labelText, bool isBold);


        /// <summary>
        /// Creates a Panel control
        /// </summary>
        Panel CreatePanel();

        /// <summary>
        /// Creates a Panel control
        /// </summary>
        /// <param name="controlFactory">The factory that this panel will use to create any controls on it</param>
        Panel CreatePanel(IControlFactory controlFactory);

        /// <summary>
        /// Creates a Panel control
        /// </summary>
        /// <param name="name">The name of the panel</param>
        /// <param name="controlFactory">The factory that this panel will use to create any controls on it</param>
        Panel CreatePanel(string name, IControlFactory controlFactory);


        /// <summary>
        /// Creates a ToolTip
        /// </summary>
        IToolTip CreateToolTip();

        /// <summary>
        /// Creates a TextBox control
        /// </summary>
        TextBox CreateTextBox();

        /// <summary>
        /// Creates a multi line textbox, setting the scrollbars to vertical
        /// </summary>
        /// <param name="numLines">The number of lines to show in the TextBox</param>
        TextBox CreateTextBoxMultiLine(int numLines);

        /// <summary>
        /// Creates a new PasswordTextBox that masks the letters as the user
        /// types them
        /// </summary>
        /// <returns>Returns the new PasswordTextBox object</returns>
        TextBox CreatePasswordTextBox();

        /// <summary>
        /// Creates a generic control
        /// </summary>
        Control CreateControl();

        /// <summary>
        /// Creates a user control
        /// </summary>
        UserControl CreateUserControl();

        /// <summary>
        /// Creates a user control with the specified name.
        /// </summary>
        UserControl CreateUserControl(string name);

        /// <summary>
        /// Creates a control for the given type and assembly name
        /// </summary>
        /// <param name="typeName">The name of the control type</param>
        /// <param name="assemblyName">The assembly name of the control type</param>
        /// <returns>Returns either the control of the specified type or
        /// the default type, which is usually TextBox.</returns>
        Control CreateControl(String typeName, String assemblyName);

        /// <summary>
        /// Creates a new control of the type specified
        /// </summary>
        /// <param name="controlType">The control type, which must be a
        /// sub-type of <see cref="Control"/></param>
        Control CreateControl(Type controlType);

        /// <summary>
        /// Creates a DateTimePicker
        /// </summary>
        DateTimePicker CreateDateTimePicker();

        /// <summary>
        /// Creates a new DateTimePicker with a specified date
        /// </summary>
        /// <param name="defaultDate">The initial date value</param>
        DateTimePicker CreateDateTimePicker(DateTime defaultDate);

        /// <summary>
        /// Creates a new DateTimePicker that is formatted to handle months
        /// and years
        /// </summary>
        /// <returns>Returns a new DateTimePicker object</returns>
        DateTimePicker CreateMonthPicker();

        ///<summary>
        /// Creates a new numeric up-down control
        ///</summary>
        ///<returns>The created NumericUpDown control</returns>
        NumericUpDown CreateNumericUpDown();

        /// <summary>
        /// Creates a new numeric up-down control that is formatted with
        /// zero decimal places for integer use
        /// </summary>
        NumericUpDown CreateNumericUpDownInteger();

        /// <summary>
        /// Creates a new numeric up-down control that is formatted with
        /// two decimal places for currency use
        /// </summary>
        NumericUpDown CreateNumericUpDownCurrency();

        /// <summary>
        /// Creates a new progress bar
        /// </summary>
        ProgressBar CreateProgressBar();

        /// <summary>
        /// Creates a new splitter which enables the user to resize 
        /// docked controls
        /// </summary>
        Splitter CreateSplitter();

        /// <summary>
        /// Creates a new radio button
        /// </summary>
        /// <param name="text">The text to appear next to the radio button</param>
        RadioButton CreateRadioButton(string text);

        /// <summary>
        /// Creates a TabControl
        /// </summary>
        TabControl CreateTabControl();

        /// <summary>
        /// Creates a new tab page
        /// </summary>
        /// <param name="title">The page title to appear in the tab</param>
        TabPage CreateTabPage(string title);

/* //TODO brett 31 Mar 2011: CF
 /// <summary>
        /// Creates a control that can be placed on a form or a panel to implement a wizard user interface.
        /// The wizard control will have a next and previous button and a panel to place the wizard step on.
        /// </summary>
        /// <param name="wizardController">The controller that manages the wizard process</param>
        IWizardControl CreateWizardControl(IWizardController wizardController);

        /// <summary>
        /// Creates a form that will be used to display the wizard user interface.
        /// </summary>
        /// <param name="wizardController"></param>
        /// <returns></returns>
        IWizardForm CreateWizardForm(IWizardController wizardController);

        /// <summary>
        /// Creates a form that will be used to display the wizard user interface.
        /// </summary>
        /// <param name="wizardControl">The Wizard control that will be displayed on the form</param>
        /// <returns></returns>
        IWizardForm CreateWizardForm(IWizardControl wizardControl);*/



        /// <summary>
        /// Creates a Form control
        /// </summary>
        Form CreateForm();

        /// <summary>
        /// Creates a control mapper strategy for the management of how
        /// business object properties and their related controls update each other.
        /// For example, a windows strategy might be to update the control value whenever the property 
        /// is updated, whereas an internet strategy might be to update the control value only
        /// when the business object is loaded.
        /// </summary>
        IControlMapperStrategy CreateControlMapperStrategy();

        /// <summary>
        /// Returns a textbox mapper strategy that can be applied to a textbox
        /// </summary>
        ITextBoxMapperStrategy CreateTextBoxMapperStrategy();

        /// <summary>
        /// Creates a strategy that customises behaviour of a CheckBox for the environment
        /// </summary>
        ICheckBoxMapperStrategy CreateCheckBoxMapperStrategy();

        /// <summary>
        /// Creates a strategy that customises behaviour of a ComboBox for the environment
        /// </summary>
        IListComboBoxMapperStrategy CreateListComboBoxMapperStrategy();

        /// <summary>
        /// Creates a strategy that customises behaviour of a lookup ComboBox for the environment
        /// </summary>
        IComboBoxMapperStrategy CreateLookupComboBoxDefaultMapperStrategy();

        /// <summary>
        /// Creates a strategy that customises behaviour of key presses on a lookup ComboBox for the environment
        /// </summary>
        IComboBoxMapperStrategy CreateLookupKeyPressMapperStrategy();

        /// <summary>
        /// Creates a strategy that customises behaviour of a NumericUpDown for the environment
        /// </summary>
        INumericUpDownMapperStrategy CreateNumericUpDownMapperStrategy();

        ///<summary>
        /// Displays a message box with specified text, caption, buttons, and icon.
        ///</summary>
        ///<param name="message">The text to display in the message box.</param>
        ///<param name="title">The text to display in the title bar of the message box.</param>
        ///<param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
        ///<param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the message box.</param>
        ///<returns>The message box result.</returns>
        DialogResult ShowMessageBox(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon);

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
        DialogResult ShowMessageBox(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon,
                                    DialogCompletionDelegate dialogCompletionDelegate);

        ///<summary>
        /// Displays a message box with specified text.
        ///</summary>
        ///<param name="message">The text to display in the message box.</param>
        ///<returns>The message box result.</returns>
        DialogResult ShowMessageBox(string message);

        /// <summary>
        /// Creates a TextBox that provides filtering of characters depending on the property type.
        /// </summary>
        PictureBox CreatePictureBox();


        ///<summary>
        /// Creates an <see cref="IBOListBoxSelector"/>
        ///</summary>
        ///<returns></returns>
        IBOListBoxSelector CreateListBoxSelector();


        ///<summary>
        /// Creates a <see cref="IExtendedComboBox"/>. This is essentially
        /// a disabled ComboBox with a Search button.
        ///</summary>
        ///<returns></returns>
        IExtendedComboBox CreateExtendedComboBox();
    }
/*
    /// <summary>
    /// Provides a screen
    /// </summary>
    public interface IScreen
    {
    }*/

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a control
    /// depending on the environment
    /// </summary>
    public interface IControlMapperStrategy
    {
        /// <summary>
        /// Adds handlers to events of a current business object property.
        /// </summary>
        /// <param name="mapper">The control mapper that maps the business object property to the control</param>
        /// <param name="boProp">The business object property being mapped to the control</param>
        void AddCurrentBOPropHandlers(ControlMapper mapper, IBOProp boProp);

        /// <summary>
        /// Removes handlers to events of a current business object property.
        /// It is essential that if the AddCurrentBoPropHandlers is implemented then this 
        /// is implemented such that editing a business object that is no longer being shown on the control does not
        /// does not update the value in the control.
        /// </summary>
        /// <param name="mapper">The control mapper that maps the business object property to the control</param>
        /// <param name="boProp">The business object property being mapped to the control</param>
        void RemoveCurrentBOPropHandlers(ControlMapper mapper, IBOProp boProp);

        /// <summary>
        /// Handles the default key press behaviours on a control.
        /// This is typically used to change the handling of the enter key (such as having
        /// the enter key cause focus to move to the next control).
        /// </summary>
        /// <param name="control">The control whose events will be handled</param>
        void AddKeyPressEventHandler(Control control);
    }

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a TextBox
    /// depending on the environment
    /// </summary>
    public interface ITextBoxMapperStrategy
    {
        /// <summary>
        /// Adds key press event handlers that carry out actions like
        /// limiting the input of certain characters, depending on the type of the
        /// property
        /// </summary>
        /// <param name="mapper">The TextBox mapper</param>
        /// <param name="boProp">The property being mapped</param>
        void AddKeyPressEventHandler(TextBoxMapper mapper, IBOProp boProp);

        ///<summary>
        /// Add a handler to the <see cref="TextBox"/> TextChanged Event that
        /// automatically updates the Business Object with this change.
        /// This is only applicable in Windows not for VWG (Web).
        ///</summary>
        ///<param name="mapper"></param>
        ///<param name="boProp"></param>
        void AddUpdateBoPropOnTextChangedHandler(TextBoxMapper mapper, IBOProp boProp);
    }

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a CheckBox
    /// depending on the environment
    /// </summary>
    public interface ICheckBoxMapperStrategy
    {
        /// <summary>
        /// Adds click event handler
        /// </summary>
        /// <param name="mapper">The checkbox mapper</param>
        void AddClickEventHandler(CheckBoxMapper mapper);
    }

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a list ComboBox
    /// depending on the environment
    /// </summary>
    public interface IListComboBoxMapperStrategy
    {
        /// <summary>
        /// Adds an ItemSelected event handler.
        /// For Windows Forms you may want the business object to be updated immediately, however
        /// for a web environment with low bandwidth you may choose to only update when the user saves.
        ///</summary>
        void AddItemSelectedEventHandler(ListComboBoxMapper mapper);
    }

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a lookup ComboBox
    /// depending on the environment
    /// </summary>
    public interface IComboBoxMapperStrategy
    {
        /// <summary>
        /// Adds event handlers to the ComboBox that are suitable for the UI environment
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        void AddHandlers(IComboBoxMapper mapper);

        /// <summary>
        /// Removes event handlers previously assigned to the ComboBox
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        void RemoveCurrentHandlers(IComboBoxMapper mapper);
    }

    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a NumericUpDown
    /// depending on the environment
    /// </summary>
    public interface INumericUpDownMapperStrategy
    {
        /// <summary>
        /// Handles the value changed event suitably for the UI environment
        /// </summary>
        /// <param name="mapper">The mapper for the NumericUpDown</param>
        void ValueChanged(NumericUpDownMapper mapper);
    }
}