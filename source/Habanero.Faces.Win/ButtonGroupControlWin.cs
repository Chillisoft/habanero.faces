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
using System.Windows.Forms;
using Habanero.Faces.Base;

namespace Habanero.Faces.Win
{
    /// <summary>
    /// Manages a group of buttons that display next to each other
    /// </summary>
    public class ButtonGroupControlWin : PanelWin, IButtonGroupControl
    {
        private readonly IControlFactory _controlFactory;
        private readonly ButtonGroupControlManager _buttonGroupControlManager;
        /// <summary>
        /// Constructor for the <see cref="ButtonGroupControlWin"/>
        /// </summary>
        /// <param name="controlFactory"></param>
        public ButtonGroupControlWin(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            _buttonGroupControlManager = new ButtonGroupControlManager(this, controlFactory);
            ButtonSizePolicy = new ButtonSizePolicyWin(_controlFactory);
        }

        /// <summary>
        /// Adds a new button to the control with a specified name
        /// </summary>
        /// <param name="buttonName">The name to appear on the button</param>
        /// <returns>Returns the Button object created</returns>
        public IButton AddButton(string buttonName)
        {
            IButton button = _buttonGroupControlManager.AddButton(buttonName);
            Controls.Add((Control)button);
            ButtonSizePolicy.RecalcButtonSizes(_buttonGroupControlManager.LayoutManager.ManagedControl.Controls);
            return button;
        }

        /// <summary>
        /// A facility to index the buttons in the control so that they can
        /// be accessed like an array (eg. button["name"])
        /// </summary>
        /// <param name="buttonName">The name of the button</param>
        /// <returns>Returns the button found by that name, or null if not
        /// found</returns>
        public IButton this[string buttonName]
        {
            get { return (IButton) this.Controls[buttonName]; }
        }

        /// <summary>
        /// Sets the default button in this control that would be chosen
        /// if the user pressed Enter without changing the focus
        /// </summary>
        /// <param name="buttonName">The name of the button</param>
        public void SetDefaultButton(string buttonName)
        {
            Form form = this.FindForm();
            if (form != null) form.AcceptButton = (Button)this[buttonName];
        }

        /// <summary>
        /// The <see cref="IButtonSizePolicy"/> to use when resizing the buttons. The default on will size all the buttons equally based on the widest one.
        /// </summary>
        public IButtonSizePolicy ButtonSizePolicy { get; set; }

        /// <summary>
        /// Returns the <see cref="FlowLayoutManager"/> used to layout the buttons. You can use this, for example, to adjust the alignment of the buttons (left or right).
        /// </summary>
        public FlowLayoutManager LayoutManager
        {
            get { return _buttonGroupControlManager.LayoutManager; }
        }

        /// <summary>
        /// Adds a new button to the control with a specified name and
        /// with an attached event handler to carry out
        /// further actions if the button is pressed
        /// </summary>
        /// <param name="buttonName">The name to appear on the button</param>
        /// <param name="clickHandler">The method that handles the Click event</param>
        /// <returns>Returns the Button object created</returns>
        public IButton AddButton(string buttonName, EventHandler clickHandler)
        {
             return AddButton(buttonName, buttonName, clickHandler);
        }

        /// <summary>
        /// Adds a new button to the control with a specified name, specified text and
        /// with an attached event handler to carry out
        /// further actions if the button is pressed
        /// </summary>
        /// <param name="buttonName">The name that the button is created with</param>
        /// <param name="buttonText">The text to appear on the button</param>
        /// <param name="clickHandler">The method that handles the Click event</param>
        /// <returns>Returns the Button object created</returns>
        public IButton AddButton(string buttonName, string buttonText, EventHandler clickHandler)
        {
            IButton button = _buttonGroupControlManager.AddButton(buttonName, buttonText, clickHandler);
            ButtonSizePolicy.RecalcButtonSizes(_buttonGroupControlManager.LayoutManager.ManagedControl.Controls);
            ((Button)button).UseMnemonic = true;
            return button;
        }
    }
}
