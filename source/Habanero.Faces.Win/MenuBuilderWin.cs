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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Faces.Base;

namespace Habanero.Faces.Win
{
        ///<summary>
        /// 
        ///</summary>
        public class ContextMenuBuilderWin : IMenuBuilder
        {
            private readonly IControlFactory _controlFactory;

            /// <summary>
            /// Constructs a <see cref="MenuBuilderWin"/> with the appropriate controlFactory
            /// </summary>
            /// <param name="controlFactory"></param>
            public ContextMenuBuilderWin(IControlFactory controlFactory)
            {
                if (controlFactory == null) throw new ArgumentNullException("controlFactory");
                _controlFactory = controlFactory;
            }

            ///<summary>
            /// Builds the Context Menu based on a <paramref name="habaneroMenu"/>
            ///</summary>
            ///<param name="habaneroMenu"></param>
            ///<returns></returns>
            public IMainMenuHabanero BuildMainMenu(HabaneroMenu habaneroMenu)
            {
                IMainMenuHabanero mainMenu = ControlFactory.CreateContextMenu();
                mainMenu.Name = habaneroMenu.Name;

                foreach (HabaneroMenu.Item habaneroMenuItem in habaneroMenu.MenuItems)
                {
                    IMenuItem menuItem = this.ControlFactory.CreateMenuItem(habaneroMenuItem);
                    menuItem.Click += delegate { menuItem.DoClick(); };
                    mainMenu.MenuItems.Add(menuItem);
                }

                return mainMenu;
            }

            /// <summary>
            /// Returns the control factory being used to create the Menu and the MenuItems
            /// </summary>

            public IControlFactory ControlFactory
            {
                get { return _controlFactory; }
            }
        }    


    ///<summary>
    /// 
    ///</summary>
    public class MenuBuilderWin : IMenuBuilder
    {
        private readonly IControlFactory _controlFactory;

        /// <summary>
        /// Constructs a <see cref="MenuBuilderWin"/> with the appropriate controlFactory
        /// </summary>
        /// <param name="controlFactory"></param>
        public MenuBuilderWin(IControlFactory controlFactory)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            _controlFactory = controlFactory;
        }

        ///<summary>
        /// Builds the Main Menu based on a <paramref name="habaneroMenu"/>
        ///</summary>
        ///<param name="habaneroMenu"></param>
        ///<returns></returns>
        public IMainMenuHabanero BuildMainMenu(HabaneroMenu habaneroMenu)
        {
            IMainMenuHabanero mainMenu = ControlFactory.CreateMainMenu();
            mainMenu.Name = habaneroMenu.Name;
            foreach (HabaneroMenu submenu in habaneroMenu.Submenus)
            {
                mainMenu.MenuItems.Add(BuildMenu(submenu));
            }
            return mainMenu;
        }

        /// <summary>
        /// Returns the control factory being used to create the Menu and the MenuItems
        /// </summary>

        public IControlFactory ControlFactory
        {
            get { return _controlFactory; }
        }

        private IMenuItem BuildMenu(HabaneroMenu habaneroMenu)
        {
            //MenuItemWin menuItem = new MenuItemWin(habaneroMenu.Name);
            IMenuItem menuItem = this.ControlFactory.CreateMenuItem(habaneroMenu.Name);
            foreach (HabaneroMenu submenu in habaneroMenu.Submenus)
            {
                menuItem.MenuItems.Add(BuildMenu(submenu));
            }
            foreach (HabaneroMenu.Item habaneroMenuItem in habaneroMenu.MenuItems)
            {
                IMenuItem childMenuItem = this.ControlFactory.CreateMenuItem(habaneroMenuItem);
                childMenuItem.Click += delegate { childMenuItem.DoClick(); };
                menuItem.MenuItems.Add(childMenuItem);
            }
            return menuItem;
        }
    }

    ///<summary>
    /// The standard windows Context Menu structure object. <see cref="MainMenu"/>
    ///</summary>
    internal class ContextMenuWin : ContextMenu, IMainMenuHabanero
    {
        protected readonly HabaneroMenu _habaneroMenu;
        private readonly MenuItemCollectionWin _menuItems;
        private const string DOCK_IN_FORM_ERROR_MESSAGE = "Context menu's cannot be docked";

        public ContextMenuWin()
        {
            _menuItems = new MenuItemCollectionWin(base.MenuItems);
        }

        public ContextMenuWin(HabaneroMenu habaneroMenu)
            : this()
        {
            _habaneroMenu = habaneroMenu;
            if (_habaneroMenu != null) this.Name = _habaneroMenu.Name;
        }

        ///<summary>
        /// The collection of menu items for this menu <see cref="Menu.MenuItemCollection"/>
        ///</summary>
        public new IMenuItemCollection MenuItems
        {
            get { return _menuItems; }
        }

        /// <summary>
        /// This method sets up the form so that the menu is displayed and the form is able to 
        /// display the controls loaded when the menu item is clicked.
        /// </summary>
        /// <param name="form">The form to set up with the menu</param>
        public void DockInForm(IControlHabanero form)
        {
            //Do Nothing
            
            throw new NotImplementedException(DOCK_IN_FORM_ERROR_MESSAGE);
        }

        /// <summary>
        /// Not implemented as it is not supported to set a width for
        /// this style of menu.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="menuWidth"></param>
        public void DockInForm(IControlHabanero form, int menuWidth)
        {
            //Do Nothing
            throw new NotImplementedException(DOCK_IN_FORM_ERROR_MESSAGE);
        }
    }

    ///<summary>
    /// The standard windows main menu structure object. <see cref="MainMenu"/>
    ///</summary>
    internal class MainMenuWin : MainMenu, IMainMenuHabanero
    {
        protected readonly HabaneroMenu _habaneroMenu;
        private readonly MenuItemCollectionWin _menuItems;

        public MainMenuWin()
        {
            _menuItems = new MenuItemCollectionWin(base.MenuItems);
        }

        public MainMenuWin(HabaneroMenu habaneroMenu) : this()
        {
            _habaneroMenu = habaneroMenu;
            if (_habaneroMenu != null) this.Name = _habaneroMenu.Name;
        }

        ///<summary>
        /// The collection of menu items for this menu <see cref="Menu.MenuItemCollection"/>
        ///</summary>
        public new IMenuItemCollection MenuItems
        {
            get { return _menuItems; }
        }

        /// <summary>
        /// This method sets up the form so that the menu is displayed and the form is able to 
        /// display the controls loaded when the menu item is clicked.
        /// </summary>
        /// <param name="form">The form to set up with the menu</param>
        public void DockInForm(IControlHabanero form)
        {
            Form formWin = (Form) form;
            ((IFormHabanero)form).IsMdiContainer = true;
            formWin.Menu = this;
        }

        /// <summary>
        /// Not implemented as it is not supported to set a width for
        /// this style of menu.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="menuWidth"></param>
        public void DockInForm(IControlHabanero form, int menuWidth)
        {
            throw new NotImplementedException();
    }
    }

    /// <summary>
    /// A menuItems Collection representing the <see cref="Menu.MenuItemCollection"/>
    /// </summary>
    internal class MenuItemCollectionWin : IMenuItemCollection
    {
        private readonly Menu.MenuItemCollection _menuItemCollection;

        public MenuItemCollectionWin(Menu.MenuItemCollection menuItemCollection)
        {
            _menuItemCollection = menuItemCollection;
        }

        public int Count
        {
            get { return _menuItemCollection.Count; }
        }

        public IMenuItem OwnerMenuItem
        {
            get { throw new NotImplementedException(); }
        }

        public IMenuItem this[int index]
        {
            get { return (IMenuItem) _menuItemCollection[index]; }
        }

        public void Add(IMenuItem menuItem)
        {
            _menuItemCollection.Add((MenuItem) menuItem);
        }


        #region Implementation of IEnumerable

        IEnumerator<IMenuItem> IEnumerable<IMenuItem>.GetEnumerator()
        {
            return _menuItemCollection.Cast<IMenuItem>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _menuItemCollection.GetEnumerator();
        }

        #endregion
/*
        #region Implementation of IEnumerable<IMenuItem>

        IEnumerator<IMenuItem> IEnumerable<IMenuItem>.GetEnumerator()
        {
            return ((IEnumerable<IMenuItem>)_menuItemCollection).GetEnumerator();
        }

        #endregion*/
    }

    internal class MenuItemWin : MenuItem, IMenuItem
    {
        private readonly HabaneroMenu.Item _habaneroMenuItem;
        private IFormHabanero _createdForm;
        private IFormControl _formControl;
        private IControlManager _controlManager;

        public MenuItemWin(HabaneroMenu.Item habaneroMenuItem) : this(habaneroMenuItem.Name)
        {
            _habaneroMenuItem = habaneroMenuItem;
        }

        public MenuItemWin(string text) : base(text)
        {
        }

        public new IMenuItemCollection MenuItems
        {
            get { return new MenuItemCollectionWin(base.MenuItems); }
        }

        public void DoClick()
        {
            try
            {
                if (_habaneroMenuItem.CustomHandler != null)
                {
                    _habaneroMenuItem.CustomHandler(this, new EventArgs());
                }
                else
                {
                    if (ReshowForm()) return;
                    if (_habaneroMenuItem.Form == null || _habaneroMenuItem.Form.Controls.Count <= 0) return;
                    _createdForm = _habaneroMenuItem.ControlFactory.CreateForm();
                    _createdForm.Width = 800;
                    _createdForm.Height = 600;
                    _createdForm.MdiParent = (IFormHabanero) _habaneroMenuItem.Form;
                    _createdForm.WindowState = Habanero.Faces.Base.FormWindowState.Maximized;
                    _createdForm.Text = _habaneroMenuItem.Name;
                    //Deerasha 2009/11/26: Line below prevents the std VS icon from appearing on form's upper right
                    ((Form) _createdForm).ShowIcon = false;
                    _createdForm.Controls.Clear();

                    BorderLayoutManager layoutManager = _habaneroMenuItem.ControlFactory.CreateBorderLayoutManager
                        (_createdForm);

                    IControlHabanero control;
                    if (_habaneroMenuItem.FormControlCreator != null)
                    {
                        _formControl = _habaneroMenuItem.FormControlCreator();
                        _formControl.SetForm(_createdForm);
                        control = (IControlHabanero) _formControl;
                    }
                    else if (_habaneroMenuItem.ControlManagerCreator != null)
                    {
                        _controlManager = _habaneroMenuItem.ControlManagerCreator(_habaneroMenuItem.ControlFactory);
                        control = _controlManager.Control;
                    }
                    else
                    {
                        throw new Exception
                            ("Please set up the MenuItem with at least one Creational or custom handling delegate");
                    }
                    layoutManager.AddControl(control, BorderLayoutManager.Position.Centre);
                    _createdForm.Show();
                    _createdForm.Closed += delegate
                                           {
                                               _createdForm = null;
                                               _formControl = null;
                                               _controlManager = null;
                                           };
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, null, null);
            }
        }

        private bool ReshowForm()
        {
            if (_createdForm != null)
            {
                try
                {
                    _createdForm.Show();
                    _createdForm.Refresh();
                    _createdForm.Focus();
                    _createdForm.PerformLayout();
                }
                catch (Win32Exception)
                {
                    //note_: it will throw this error in testing.
                }
                catch (ObjectDisposedException)
                {
                    //the window has been disposed, we need to create a new one
                }
                return true;
            }
            return false;
        }
    }
}