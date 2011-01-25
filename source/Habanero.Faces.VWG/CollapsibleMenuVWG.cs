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
using System.Drawing;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.Faces.Base;
using DockStyle=Habanero.Faces.Base.DockStyle;

namespace Habanero.Faces.VWG
{
    /// <summary>
    /// A collection of <see cref="CollapsibleMenuItemVWG"/>s used by the CollapsibleMenu.
    /// </summary>
    [MetadataTag("P")]
    public class CollapsibleMenuItemCollectionVWG : IMenuItemCollection
    {
        private readonly IMenuItem _ownerMenuItem;

        private readonly IList<IMenuItem> _list = new List<IMenuItem>();
        private ColumnLayoutManager _columnLayoutManager;

        /// <summary>
        /// Creates a <see cref="CollapsibleMenuItemCollectionVWG"/>
        /// </summary>
        /// <param name="menu"></param>
        public CollapsibleMenuItemCollectionVWG(IMenuItem menu)
        {
            _ownerMenuItem = menu;
            this.ControlFactory = GlobalUIRegistry.ControlFactory;
        }

        private IControlFactory ControlFactory { get; set; }

        #region Implementation of IMenuItemCollection

        /// <summary>
        /// Adds a Menu item to the <see cref="T:Habanero.Faces.Base.IMenuItemCollection" />.
        /// </summary>
        /// <param name="menuItem"></param>
        public void Add(IMenuItem menuItem)
        {
            _list.Add(menuItem);
            ICollapsiblePanelGroupControl cpGroupMenuItem = _ownerMenuItem as ICollapsiblePanelGroupControl;
            if (cpGroupMenuItem != null && menuItem is CollapsibleSubMenuItemVWG)
            {
                CollapsibleSubMenuItemVWG outlookStyleSubMenuItem = (CollapsibleSubMenuItemVWG) menuItem;
                cpGroupMenuItem.AddControl(outlookStyleSubMenuItem);
                return;
            }
            ICollapsiblePanel cpMenuItem = _ownerMenuItem as ICollapsiblePanel;
            if (cpMenuItem != null && menuItem is CollapsibleMenuItemVWG)
            {
                if (cpMenuItem.ContentControl == null)
                {
                    cpMenuItem.ContentControl = this.ControlFactory.CreatePanel();
                    _columnLayoutManager = new ColumnLayoutManager(cpMenuItem.ContentControl, this.ControlFactory);
                        // this.ControlFactory.CreateBorderLayoutManager(cpMenuItem);
                    _columnLayoutManager.GapSize = 0;
                    _columnLayoutManager.BorderSize = 0;
                }
                _columnLayoutManager.AddControl((CollapsibleMenuItemVWG) menuItem);
                cpMenuItem.Height += ((CollapsibleMenuItemVWG) menuItem).Height;
                cpMenuItem.ExpandedHeight = cpMenuItem.Height;
            }
        }

        /// <summary>
        ///The number of <see cref="IMenuItem"/>s in this <see cref="IMenuItemCollection"/>
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// The Menu Item that owns this colleciton of Menu Items.
        /// </summary>
        public IMenuItem OwnerMenuItem
        {
            get { return _ownerMenuItem; }
        }

        /// <summary>
        /// Returns the Actual Menu item identified by the index.
        /// </summary>
        /// <param name="index"></param>
        public IMenuItem this[int index]
        {
            get { return _list[index]; }
        }

        #endregion

        #region Implementation of IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        #region Implementation of IEnumerable<IMenuItem>

        IEnumerator<IMenuItem> IEnumerable<IMenuItem>.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// The Collapsible Menu is essentially a <see cref="ICollapsiblePanelGroupControl"/>
    /// that has Collapsible Panels (See <see cref="ICollapsiblePanel"/>) placed inside of it.
    /// The Collapsible Panels have menu Items (Leaf menu Items) which when clicked will
    /// perform the Doclick of the Habanero Menu Item.
    /// The CollapsibleMenu is built from a Habanero Menu (see <see cref="HabaneroMenu"/>) using
    ///  the <see cref="CollapsibleMenuBuilderVWG"/>
    /// </summary>
    [MetadataTag("P")]
    public class CollapsibleMenuVWG : CollapsiblePanelGroupControlVWG, IMainMenuHabanero, IMenuItem
    {
        private readonly IMenuItemCollection _menuItemCollection;
        private ISplitContainer _splitContainer;
        private MainEditorPanelVWG _mainEditorPanel;

        /// <summary>
        /// Constructs a <see cref="CollapsibleMenuVWG"/>
        /// </summary>
        public CollapsibleMenuVWG()
        {
            _menuItemCollection = new CollapsibleMenuItemCollectionVWG(this);
        }

        /// <summary>
        /// Constructs a <see cref="CollapsibleMenuVWG"/>
        /// </summary>
        public CollapsibleMenuVWG(HabaneroMenu habaneroMenu) : this()
        {
            if (habaneroMenu != null) this.Name = habaneroMenu.Name;
        }

        #region Implementation of IMainMenuHabanero

        /// <summary>
        ///This method sets up the form so that the menu is displayed and the form is able to 
        ///display the controls loaded when the menu item is clicked.
        /// </summary>
        /// <param name="form">The form to set up with the menu</param>
        public void DockInForm(IControlHabanero form)
        {
            DockInForm(form, 250);
        }
        /// <summary>
        ///This method sets up the form so that the menu is displayed and the form is able to 
        ///display the controls loaded when the menu item is clicked.
        /// </summary>
        /// <param name="form">The form to set up with the menu</param>
        /// <param name="menuWidth">The width of the menu - configurable to so that each application can set its menu width</param>
        public virtual void DockInForm(IControlHabanero form, int menuWidth)
        {
            if (form == null) throw new ArgumentNullException("form");
            _splitContainer = this.ControlFactory.CreateSplitContainer();
            _splitContainer.Name = "SplitContainer";
            BorderLayoutManager layoutManager = this.ControlFactory.CreateBorderLayoutManager(form);
            layoutManager.AddControl(_splitContainer, BorderLayoutManager.Position.Centre);
            SplitContainer splitContainer1 = (SplitContainer) _splitContainer;
            //splitContainer1.IsSplitterFixed = true;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Size = new System.Drawing.Size(400, 450);
            splitContainer1.SplitterDistance = menuWidth;
            splitContainer1.Panel1MinSize = menuWidth;
            splitContainer1.Orientation = Gizmox.WebGUI.Forms.Orientation.Vertical;
            this.Dock = Gizmox.WebGUI.Forms.DockStyle.Fill;
            splitContainer1.Panel1.Controls.Add(this);
            _mainEditorPanel = new MainEditorPanelVWG(this.ControlFactory);
            _mainEditorPanel.Dock = Gizmox.WebGUI.Forms.DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(_mainEditorPanel);
        }

        /// <summary>
        /// Performs the Click event for this <see cref="T:Habanero.Faces.Base.IMenuItem" />.
        /// </summary>
        public void PerformClick()
        {
        }

        /// <summary>
        ///             This actually executes the Code when PerformClick is selected <see cref="T:Habanero.Faces.Base.IMenuItem" />.
        /// </summary>
        public void DoClick()
        {
        }

        /// <summary>
        ///              The collection of menu items for this menu
        /// </summary>
        public IMenuItemCollection MenuItems
        {
            get { return _menuItemCollection; }
        }
        /// <summary>
        /// This is to set the text of the main title.
        /// </summary>
        public string MainTitleText
        {
            get { return _mainEditorPanel.MainTitleIconControl.Title.Text; }
            set { _mainEditorPanel.MainTitleIconControl.Title.Text = value;}
        }
        #endregion
    }

    /// <summary>
    /// This is an individual item on an Collapsible Menu. i.e. this is the control that when clicked.
    /// it opens the editor control in the <see cref="MainEditorPanelVWG"/>
    /// </summary>
    public class CollapsibleMenuItemVWG : ButtonVWG, IMenuItem
    {
        private readonly HabaneroMenu.Item _habaneroMenuItem;
        private IFormControl _formControl;
        private IControlManager _controlManager;

        /// <summary>
        /// Creates a <see cref="CollapsibleMenuItemCollectionVWG"/> with a name.
        /// </summary>
        /// <param name="name"></param>
        public CollapsibleMenuItemVWG(string name)
        {
            this.Text = name;
            MenuItems = new CollapsibleMenuItemCollectionVWG(this);
        }

        /// <summary>
        /// Creates a <see cref="CollapsibleMenuItemCollectionVWG"/> for a habaneroMenuItem.
        /// </summary>
        /// <param name="habaneroMenuItem"></param>
        public CollapsibleMenuItemVWG(HabaneroMenu.Item habaneroMenuItem)
            : this(GlobalUIRegistry.ControlFactory, habaneroMenuItem)
        {
        }

        /// <summary>
        /// Creates a <see cref="CollapsibleMenuItemCollectionVWG"/> for a habaneroMenuItem.
        /// </summary>
        /// <param name="controlFactory"></param>
        /// <param name="habaneroMenuItem"></param>
        public CollapsibleMenuItemVWG(IControlFactory controlFactory, HabaneroMenu.Item habaneroMenuItem)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            _habaneroMenuItem = habaneroMenuItem;
            if (habaneroMenuItem != null)
            {
                this.Text = _habaneroMenuItem.Name;
                this.FlatStyle = FlatStyle.Flat;
                this.BackgroundImage = "Images.smBack-white.gif";
                this.Image = "Images.nbItemBullet.gif";
                this.TextImageRelation = TextImageRelation.ImageBeforeText;
                this.TextAlign = ContentAlignment.MiddleLeft;
                this.Font = new Font("Verdana", 9);
                this.Dock = DockStyleVWG.GetDockStyle(DockStyle.Top);
                this.Click += ChangeButtonIcon;
            }
            MenuItems = new CollapsibleMenuItemCollectionVWG(this);
        }

        private void ChangeButtonIcon(object sender, EventArgs e)
        {
            foreach (Control control in this.Parent.Controls.Owner.Controls)
            {
                Button button = (Button) control;
                if (button != this) button.Image = "Images.nbItemBullet.gif";
            }
            this.Image = "Images.nav_icon.png";
        }

        #region Implementation of IMenuItem

        /// <summary>
        /// This actually executes the Code when PerformClick is selected <see cref="T:Habanero.Faces.Base.IMenuItem" />.
        /// </summary>
        public virtual void DoClick()
        {
            try
            {
                if (_habaneroMenuItem.CustomHandler != null)
                {
                    _habaneroMenuItem.CustomHandler(this, new EventArgs());
                }
                else
                {
                    IControlHabanero control;
                    if (_habaneroMenuItem.Form == null || _habaneroMenuItem.Form.Controls.Count <= 0) return;
                    if (_habaneroMenuItem.FormControlCreator != null)
                    {
                        if (_formControl == null) _formControl = _habaneroMenuItem.FormControlCreator();
                        _formControl.SetForm(null);
                        control = (IControlHabanero) _formControl;
                    }
                    else if (_habaneroMenuItem.ControlManagerCreator != null)
                    {
                        if (_controlManager == null)
                            _controlManager = _habaneroMenuItem.ControlManagerCreator(_habaneroMenuItem.ControlFactory);
                        control = _controlManager.Control;
                    }
                    else
                    {
                        throw new Exception
                            ("Please set up the MenuItem with at least one Creational or custom handling delegate");
                    }
                    control.Dock = Habanero.Faces.Base.DockStyle.Fill;

                    // support the menu control either being the top control of the form, or the first subcontrol of the top control
                    SplitContainer splitContainer;
                    if (_habaneroMenuItem.Form.Controls[0] is SplitContainer)
                        splitContainer = (SplitContainer)_habaneroMenuItem.Form.Controls[0];
                    else
                        splitContainer = (SplitContainer)_habaneroMenuItem.Form.Controls[0].Controls[0];
                    SplitterPanel panel2 = splitContainer.Panel2;
                    MainEditorPanelVWG mainEditorPanel = (MainEditorPanelVWG) panel2.Controls[0];
                    mainEditorPanel.MainTitleIconControl.Title.Text = _habaneroMenuItem.ParentMenu.Name + " > " + this.Text;
                    mainEditorPanel.EditorPanel.Controls.Clear();
                    mainEditorPanel.EditorPanel.Controls.Add(control);
                    mainEditorPanel.Width -= 1;
                    mainEditorPanel.Width += 1;
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, null, null);
            }
        }

//
//        /// <summary>
//        ///              The text displayed for this <see cref="T:Habanero.Faces.Base.IMenuItem" />.
//        /// </summary>
//        public string Text { get; set; }

        /// <summary>
        ///             The Child Menu items for this <see cref="T:Habanero.Faces.Base.IMenuItem" />.
        /// </summary>
        public IMenuItemCollection MenuItems { get; private set; }

        #endregion
    }


    /// <summary>
    /// This is an individual sub menu item on an Collapsible Menu. i.e. this is the control that when clicked,
    ///  it will expand and show the Menu items underneath it.
    /// </summary>
    public class CollapsibleSubMenuItemVWG : CollapsiblePanelVWG, IMenuItem
    {
        /// <summary>
        /// Creates a <see cref="CollapsibleSubMenuItemVWG"/>
        /// </summary>
        /// <param name="controlFactory"></param>
        /// <param name="name"></param>
        public CollapsibleSubMenuItemVWG(IControlFactory controlFactory, string name) : base(controlFactory)
        {
            InitialiseSubMenuItem(name);
        }

        /// <summary>
        /// Creates a <see cref="CollapsibleSubMenuItemVWG"/>
        /// </summary>
        public CollapsibleSubMenuItemVWG(IControlFactory controlFactory, HabaneroMenu.Item habaneroMenuItem)
            : base(controlFactory)
        {
            string text = "";
            if (habaneroMenuItem != null)
            {
                text = habaneroMenuItem.Name;
            }
            InitialiseSubMenuItem(text);
        }

        private void InitialiseSubMenuItem(string name)
        {
            this.Text = name;
            this.Name = name;
            this.PinLabel.Visible = false;
            this.CollapseButton.Text = name;
            this.Dock = DockStyleVWG.GetDockStyle(DockStyle.Top);
            this.CollapseButton.ForeColor = Color.White;
            this.CollapseButton.Font = new Font("Verdana", 10);
            ((ButtonVWG)this.CollapseButton).BackgroundImage = "Images.headergradient.png";
            ((ButtonVWG) this.CollapseButton).FlatStyle = FlatStyle.Flat;
            this.Collapsed = true;
            this.CollapseButton.Click += delegate { if (this.Collapsed) this.Collapsed = false; };
            this.MinimumSize = new System.Drawing.Size(this.CollapseButton.Width, this.CollapseButton.Height);
            this.Size = new System.Drawing.Size(this.CollapseButton.Width, this.CollapseButton.Height);
            MenuItems = new CollapsibleMenuItemCollectionVWG(this);
        }

        #region Implementation of IMenuItem

        /// <summary>
        /// Performs the Click event for this <see cref="T:Habanero.Faces.Base.IMenuItem" />.
        /// </summary>
        public void PerformClick()
        {
            DoClick();
        }

        /// <summary>
        /// This actually executes the Code when PerformClick is selected <see cref="T:Habanero.Faces.Base.IMenuItem" />.
        /// In this case it expands or collapses the Menu.
        /// </summary>
        public void DoClick()
        {
            this.Collapsed = !this.Collapsed;
        }

        /// <summary>
        /// The Child Menu items for this <see cref="T:Habanero.Faces.Base.IMenuItem" />.
        /// </summary>
        public IMenuItemCollection MenuItems { get; private set; }

        #endregion
    }

    /// <summary>
    /// This is a Main Editor Panel that consists of a Header control that can be styled and takes an Icon and a Title.
    /// </summary>
    public class MainEditorPanelVWG : PanelVWG, IMainEditorPanel
    {
        /// <summary>
        /// Constructs a <see cref="MainEditorPanelVWG"/>
        /// </summary>
        /// <param name="controlFactory"></param>
        public MainEditorPanelVWG(IControlFactory controlFactory)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            this.EditorPanel = controlFactory.CreatePanel();
            this.MainTitleIconControl = new MainTitleIconControlVWG(controlFactory);
            BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(this);
            layoutManager.AddControl(this.MainTitleIconControl, BorderLayoutManager.Position.North);
            layoutManager.AddControl(this.EditorPanel, BorderLayoutManager.Position.Centre);
        }

        /// <summary>
        /// The Control that is positioned at the top of this panel that can be used to set an icon and title for the
        ///  information being displayed on the MainEditorPanelVWG
        /// </summary>
        public IMainTitleIconControl MainTitleIconControl { get; private set; }

        /// <summary>
        /// The Panel in which the control being set is placed in.
        /// </summary>
        public IPanel EditorPanel { get; private set; }
    }

    /// <summary>
    /// Builds an Outlook style Menu based on a Standard <see cref="HabaneroMenu"/>
    /// </summary>
    public class CollapsibleMenuBuilderVWG : IMenuBuilder
    {
        private readonly IControlFactory _controlFactory;

        /// <summary>
        /// Creates a <see cref="CollapsibleMenuBuilderVWG"/>
        /// </summary>
        /// <param name="controlFactory"></param>
        public CollapsibleMenuBuilderVWG(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        ///<summary>
        ///Builds the Main Menu based on a <paramref name="habaneroMenu" />
        ///</summary>
        ///<param name="habaneroMenu"></param>
        ///<returns>
        ///</returns>
        public virtual IMainMenuHabanero BuildMainMenu(HabaneroMenu habaneroMenu)
        {
            CollapsibleMenuVWG mainMenu = new CollapsibleMenuVWG(habaneroMenu);
            BuildSubMenu(habaneroMenu, mainMenu.MenuItems);
            return mainMenu;
        }
        /// <summary>
        /// Builds a CollapsibleMenu for Visual Web Gui.
        /// </summary>
        /// <param name="habaneroMenu"></param>
        /// <returns></returns>
        protected virtual IMenuItem BuildMenu(HabaneroMenu habaneroMenu)
        {
            IMenuItem subMenuItem = new CollapsibleSubMenuItemVWG(this.ControlFactory, habaneroMenu.Name);
            BuildSubMenu(habaneroMenu, subMenuItem.MenuItems);
            CreateLeafMenuItems(habaneroMenu, subMenuItem);
            return subMenuItem;
        }

        /// <summary>
        /// Creates the Leaf Items defined by the <see cref="HabaneroMenu"/> and addts them 
        /// to the <paramref name="menuItem"/>'s MenuItems Collection.
        /// </summary>
        /// <param name="habaneroMenu">The definition of the Menu</param>
        /// <param name="menuItem">The Menu Item that the MenuItems will be added to.</param>
        public virtual void CreateLeafMenuItems(HabaneroMenu habaneroMenu, IMenuItem menuItem)
        {
            foreach (HabaneroMenu.Item habaneroMenuItem in habaneroMenu.MenuItems)
            {
                IMenuItem childMenuItem = new CollapsibleMenuItemVWG(habaneroMenuItem);
                childMenuItem.Click += delegate { childMenuItem.DoClick(); };
                menuItem.MenuItems.Add(childMenuItem);
            }
        }

        /// <summary>
        /// Builds the Sub Menu based on the <paramref name="habaneroMenu"/> definition.
        /// The Sub Menu items created are added to the menuItems Collection <see cref="IMenuItemCollection"/>
        /// </summary>
        /// <param name="habaneroMenu">The definition of the Sub Menu</param>
        /// <param name="menuItems">The collection to which the Sub Menu items are added</param>
        public virtual void BuildSubMenu(HabaneroMenu habaneroMenu, IMenuItemCollection menuItems)
        {
            foreach (HabaneroMenu submenu in habaneroMenu.Submenus)
            {
                menuItems.Add(BuildMenu(submenu));
            }
        }

        /// <summary>
        /// Returns the control factory being used to create the Menu and the MenuItems
        /// </summary>
        public IControlFactory ControlFactory
        {
            get { return _controlFactory; }
        }
    }
}
