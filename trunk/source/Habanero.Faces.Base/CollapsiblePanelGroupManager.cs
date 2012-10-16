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
using System.Collections.Generic;

namespace Habanero.Faces.Base
{
    ///<summary>
    /// The Manager for the <see cref="ICollapsiblePanel"/> that handles the Common Logic for either VWG or Win.
    ///</summary>
    public class CollapsiblePanelGroupManager
    {
        /// <summary>
        /// A List of all <see cref="ICollapsiblePanel"/>s that are being managed and displayed by this Control.
        /// Warning: This must be treated as a ReadOnly List. I.e. Adding or removing items from this list
        /// will result in the Panel being in an inconsitent state use AddControl instead.
        /// </summary>
        public List<ICollapsiblePanel> PanelsList { get; private set; }

        /// <summary>
        /// The <see cref="IControlFactory"/> being used to create the <see cref="ICollapsiblePanel"/>s
        /// </summary>
        public IControlFactory ControlFactory { get; private set; }

        /// <summary>
        /// Returns the <see cref="ColumnLayoutManager"/> that is used for Laying out the <see cref="ICollapsiblePanel"/>s
        ///   on this control.
        /// </summary>
        public ColumnLayoutManager ColumnLayoutManager { get; private set; }

        /// <summary>
        /// Constructs the <see cref="CollapsiblePanelGroupManager"/>
        /// </summary>
        // ReSharper disable SuggestBaseTypeForParameter
        public CollapsiblePanelGroupManager
            (ICollapsiblePanelGroupControl collapsiblePanelGroup, IControlFactory controlFactory)
        {
            ControlFactory = controlFactory;
            PanelsList = new List<ICollapsiblePanel>();
            ColumnLayoutManager = new ColumnLayoutManager(collapsiblePanelGroup, ControlFactory);
        }

        // ReSharper restore SuggestBaseTypeForParameter
        /// <summary>
        /// Returns the Total Expanded Height of this Control. I.e. the total height of this control required
        /// if all the <see cref="ICollapsiblePanel"/> controls are fully expanded.
        /// </summary>
        public int TotalExpandedHeight
        {
            get
            {
                int totalHeight = ColumnLayoutManager.BorderSize;

                foreach (ICollapsiblePanel panel in PanelsList)
                {
                    totalHeight += panel.ExpandedHeight + ColumnLayoutManager.VerticalGapSize;
                }
                return totalHeight;
            }
        }

        /// <summary>
        /// Adds an <see cref="IControlHabanero"/> to this control. The <paramref name="contentControl"/> is
        ///    wrapped in an <see cref="ICollapsiblePanel"/> control.
        /// </summary>
        /// <param name="contentControl"></param>
        /// <param name="headingText"></param>
        /// <param name="minimumControlHeight">The minimum height that the <paramref name="contentControl"/> can be.
        ///   This height along with the <see cref="ICollapsiblePanel.CollapseButton"/>.Height are give the 
        ///   <see cref="ICollapsiblePanel.ExpandedHeight"/> that the <see cref="ICollapsiblePanel"/> will be when it is 
        ///   expanded </param>
        /// <returns></returns>
        public ICollapsiblePanel AddControl
            (IControlHabanero contentControl, string headingText, int minimumControlHeight)
        {
            ICollapsiblePanel collapsiblePanel = ControlFactory.CreateCollapsiblePanel();
            collapsiblePanel.ContentControl = contentControl;
            collapsiblePanel.CollapseButton.Text = headingText;
            AddCollapsiblePanel(collapsiblePanel, minimumControlHeight);
            return collapsiblePanel;
        }

        private void AddCollapsiblePanel(ICollapsiblePanel collapsiblePanel, int minimumControlHeight)
        {
            ColumnLayoutManager.AddControl(collapsiblePanel);
            this.PanelsList.Add(collapsiblePanel);
            collapsiblePanel.Height = collapsiblePanel.CollapseButton.Height + minimumControlHeight;
            collapsiblePanel.Collapsed = true;
            collapsiblePanel.Uncollapsed += delegate
                                            {
                                                CollapseUnpinnedPanels(collapsiblePanel);
                                            };
        }

        /// <summary>
        /// Adds an <see cref="ICollapsiblePanel"/> to this control. The <paramref name="collapsiblePanel"/> is
        ///    added to the CollapsiblePanelGroupControl.
        /// </summary>
        /// <param name="collapsiblePanel">The collapsiblePanelBeingAdded</param>
        /// <returns></returns>
        public ICollapsiblePanel AddControl(ICollapsiblePanel collapsiblePanel)
        {
            int minimumControlHeight = collapsiblePanel.MinimumSize.Height - collapsiblePanel.CollapseButton.Height;
            AddCollapsiblePanel(collapsiblePanel, minimumControlHeight);
            return collapsiblePanel;
        }


        private void CollapseUnpinnedPanels(ICollapsiblePanel collapsiblePanel)
        {
            PanelsList.ForEach
                (delegate(ICollapsiblePanel obj)
                 {
                     if (obj != collapsiblePanel)
                     {
                         if (!obj.Pinned && !obj.Collapsed) obj.Collapsed = true;
                     }
                 });
            //Hack to get the layout manager to resize
            ColumnLayoutManager.ManagedControl.Height = ColumnLayoutManager.ManagedControl.Height + 1;
            ColumnLayoutManager.ManagedControl.Height = ColumnLayoutManager.ManagedControl.Height - 1;
        }

        /// <summary>
        /// Sets whether all the <see cref="ICollapsiblePanel"/> controls are collapsed or expanded AllCollapsed = true will 
        ///   <see cref="ICollapsiblePanel.Collapsed"/> = true for all the <see cref="ICollapsiblePanel"/>s.
        /// </summary>
        public bool AllCollapsed
        {
            set
            {
                PanelsList.ForEach(delegate(ICollapsiblePanel obj) { obj.Pinned = !value; });
                PanelsList.ForEach(delegate(ICollapsiblePanel obj) { obj.Collapsed = value; });
            }
        }
    }
}