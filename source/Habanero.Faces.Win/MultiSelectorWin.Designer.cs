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
namespace Habanero.Faces.Win
{
    partial class MultiSelectorWin<T>
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private GroupBoxWin _availableOptionsGroupBox;
        private ListBoxWin _availableOptionsListbox;
        private GroupBoxWin _selectionsGroupBox;
        private ListBoxWin _selectionsListbox;
        private ButtonWin _btnSelect;
        private ButtonWin _btnSelectAll;
        private ButtonWin _btnDeselectAll;
        private ButtonWin _btnDeselect;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this._availableOptionsGroupBox = new GroupBoxWin();
            this._availableOptionsListbox = new ListBoxWin();
            this._selectionsGroupBox = new GroupBoxWin();
            this._selectionsListbox = new ListBoxWin();
            this._btnSelect = new ButtonWin();
            this._btnSelectAll = new ButtonWin();
            this._btnDeselectAll = new ButtonWin();
            this._btnDeselect = new ButtonWin();
            this.SuspendLayout();
            // 
            // _availableOptionsGroupBox
            // 
            this._availableOptionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this._availableOptionsGroupBox.Controls.Add(this._availableOptionsListbox);
            this._availableOptionsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._availableOptionsGroupBox.Location = new System.Drawing.Point(3, 3);
            this._availableOptionsGroupBox.Name = "_availableOptionsGroupBox";
            this._availableOptionsGroupBox.Size = new System.Drawing.Size(200, 369);
            this._availableOptionsGroupBox.TabIndex = 0;
            this._availableOptionsGroupBox.Text = "Available AllOptions";
            // 
            // _availableOptionsListbox
            // 
            this._availableOptionsListbox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._availableOptionsListbox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._availableOptionsListbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._availableOptionsListbox.Location = new System.Drawing.Point(3, 16);
            this._availableOptionsListbox.Name = "_availableOptionsListbox";
            this._availableOptionsListbox.SelectionMode = Habanero.Faces.Base.ListBoxSelectionMode.MultiExtended;
            this._availableOptionsListbox.Size = new System.Drawing.Size(194, 342);
            this._availableOptionsListbox.TabIndex = 0;
            // 
            // _selectionsGroupBox
            // 
            this._selectionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._selectionsGroupBox.Controls.Add(this._selectionsListbox);
            this._selectionsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._selectionsGroupBox.Location = new System.Drawing.Point(290, 3);
            this._selectionsGroupBox.Name = "_selectionsGroupBox";
            this._selectionsGroupBox.Size = new System.Drawing.Size(200, 369);
            this._selectionsGroupBox.TabIndex = 1;
            this._selectionsGroupBox.Text = "Selected AllOptions";
            // 
            // _selectionsListbox
            // 
            this._selectionsListbox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._selectionsListbox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._selectionsListbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._selectionsListbox.Location = new System.Drawing.Point(3, 16);
            this._selectionsListbox.Name = "_selectionsListbox";
            this._selectionsListbox.SelectionMode = Habanero.Faces.Base.ListBoxSelectionMode.MultiExtended;
            this._selectionsListbox.Size = new System.Drawing.Size(194, 342);
            this._selectionsListbox.TabIndex = 0;
            // 
            // _btnSelect
            // 
            this._btnSelect.Location = new System.Drawing.Point(209, 94);
            this._btnSelect.Name = "_btnSelect";
            this._btnSelect.Size = new System.Drawing.Size(75, 23);
            this._btnSelect.TabIndex = 2;
            this._btnSelect.Text = ">";
            this._btnSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // _btnSelectAll
            // 
            this._btnSelectAll.Location = new System.Drawing.Point(209, 142);
            this._btnSelectAll.Name = "_btnSelectAll";
            this._btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this._btnSelectAll.TabIndex = 3;
            this._btnSelectAll.Text = ">>";
            this._btnSelectAll.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // _btnDeselectAll
            // 
            this._btnDeselectAll.Location = new System.Drawing.Point(209, 238);
            this._btnDeselectAll.Name = "_btnDeselectAll";
            this._btnDeselectAll.Size = new System.Drawing.Size(75, 23);
            this._btnDeselectAll.TabIndex = 4;
            this._btnDeselectAll.Text = "<<";
            this._btnDeselectAll.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // _btnDeselect
            // 
            this._btnDeselect.Location = new System.Drawing.Point(209, 190);
            this._btnDeselect.Name = "_btnDeselect";
            this._btnDeselect.Size = new System.Drawing.Size(75, 23);
            this._btnDeselect.TabIndex = 5;
            this._btnDeselect.Text = "<";
            this._btnDeselect.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // MultiSelectorGiz
            // 
            //this.Controls.Add(this._btnDeselect);
            //this.Controls.Add(this._btnDeselectAll);
            //this.Controls.Add(this._btnSelectAll);
            //this.Controls.Add(this._btnSelect);
            //this.Controls.Add(this._selectionsGroupBox);
            //this.Controls.Add(this._availableOptionsGroupBox);
            this.Size = new System.Drawing.Size(497, 381);
            this.Text = "MyMultiSelector";
            this.ResumeLayout(false);
        }

        #endregion
    }
}
