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

namespace Habanero.Faces.Base
{
    /// <summary>
    /// An interface for a Group on <see cref="ICollapsiblePanel"/>s. 
    ///   The collapsible Panels are placed one above the other and when the one opens the panels below
    ///   are moved down.
    /// The Interface works simply, you add the controls (AddControl) that will be displayed on 
    ///     an <see cref="ICollapsiblePanel"/> on this control in the order that you want them displayed.
    /// This control will then create the <see cref="ICollapsiblePanel"/> with the appropriate heading text and
    ///     the appropriate Expanded (<see cref="ICollapsiblePanel.ExpandedHeight"/> and Collapsed height.
    /// </summary>
    public interface ICollapsiblePanelGroupControl : IPanel, IGroupControl
    {
        /// <summary>
        /// Event handler for the Uncollapsed Event on any of the Collapsible Panels.
        /// </summary>
        event EventHandler ItemSelected;
        /// <summary>
        /// A List of all <see cref="ICollapsiblePanel"/>s that are being managed and displayed by this Control.
        /// This must be treated as a ReadOnly List i.e. Never use PanelList.Add or PanelList.Remove.
        /// Since this will cause the Panel List to be out of sync with the ControlsCollection.
        /// </summary>
        List<ICollapsiblePanel> PanelsList { get; }

        /// <summary>
        /// The <see cref="IControlFactory"/> being used to create the <see cref="ICollapsiblePanel"/>s
        /// </summary>
        IControlFactory ControlFactory { get; }

        /// <summary>
        /// Returns the <see cref="ColumnLayoutManager"/> that is used for Laying out the <see cref="ICollapsiblePanel"/>s
        ///   on this control.
        /// </summary>
        ColumnLayoutManager ColumnLayoutManager { get; }
        /// <summary>
        /// Returns the Total Expanded Height of this Control. I.e. the total height of this control required
        /// if all the <see cref="ICollapsiblePanel"/> controls are fully expanded.
        /// </summary>
        int TotalExpandedHeight { get; }

        /// <summary>
        /// Sets whether all the <see cref="ICollapsiblePanel"/> controls are collapsed or expanded AllCollapsed = true will 
        ///   <see cref="ICollapsiblePanel.Collapsed"/> = true for all the <see cref="ICollapsiblePanel"/>s.
        /// </summary>
        bool AllCollapsed { set; }

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
        ICollapsiblePanel AddControl(IControlHabanero contentControl, string headingText, int minimumControlHeight);

        /// <summary>
        /// Adds an <see cref="ICollapsiblePanel"/> to this control. The <paramref name="collapsiblePanel"/> is
        ///   added to this <see cref="ICollapsiblePanelGroupControl"/>
        /// </summary>
        /// <param name="collapsiblePanel"></param>
        /// <returns>The collapsible Panel</returns>
        ICollapsiblePanel AddControl(ICollapsiblePanel collapsiblePanel);
    }
}