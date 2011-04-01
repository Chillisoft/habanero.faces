using System;
using System.Drawing;
using Habanero.Base;
using Habanero.Faces.Base;

namespace Habanero.Faces.Win
{


    /// <summary>
    /// This is a Dialog Box that is specialiased for dealing with the
    /// Closing of any form or application that is editing Business Objects.
    /// The dialogue box will display a sensible message to the user to determine
    /// whether they want to Close the Original form without saving, Save the BO and then
    /// Close or Cancel the Closing of the original form.
    /// </summary>
    public class CloseBOEditorDialogWin : FormWin, ICloseBOEditorDialog
    {
        private ILabel _label;

        /// <summary>
        /// The CancelClose Button.
        /// </summary>
        public IButton CancelCloseBtn { get; private set; }
        /// <summary>
        /// The Save and Close Button.
        /// </summary>
        public IButton SaveAndCloseBtn { get; private set; }
        /// <summary>
        /// The Close without saving Button.
        /// </summary>
        public IButton CloseWithoutSavingBtn { get; private set; }

        public CloseBOEditorDialogResult ShowDialog(IBusinessObject businessObject)
        {
            if (businessObject == null)
            {
                BOEditorDialogResult = CloseBOEditorDialogResult.CloseWithoutSaving;
                this.Close();
                return BOEditorDialogResult;
            }


            var isInValidState = businessObject.Status.IsValid();
            var isDirty = businessObject.Status.IsDirty;
            SaveAndCloseBtn.Enabled = isInValidState;
            this.BOEditorDialogResult = CloseBOEditorDialogResult.CancelClose;


            if (!isDirty)
            {
                this.BOEditorDialogResult = CloseBOEditorDialogResult.CloseWithoutSaving;
                this.Close();
                return this.BOEditorDialogResult;
            }
            string isValidString;
            if (isInValidState)
            {
                isValidString = " and is in a valid state to be saved";
            }

            else
            {
                string isValidMessage = businessObject.Status.IsValidMessage;

                isValidString = " and is not in a valid state to be saved: " + Environment.NewLine +
                                isValidMessage + Environment.NewLine;
            }
            var fullDisplayName = businessObject.ClassDef.DisplayName
                    + " '" + businessObject.ToString() + "'";
            _label.Text = "The " + fullDisplayName + " is has been edited" + isValidString +
                          ". Please choose the appropriate action";
            this.SaveAndCloseBtn.Enabled = isInValidState;
            ShowForm();
            return this.BOEditorDialogResult;
        }

        protected virtual void ShowForm()
        {
            base.ShowDialog();
        }
        /// <summary>
        /// The Result from this Form.
        /// </summary>
        public CloseBOEditorDialogResult BOEditorDialogResult { get; private set; }
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="controlFactory">The control Factory used to construct buttons, labels etc by ths control</param>
        public CloseBOEditorDialogWin(IControlFactory controlFactory)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");

            ConstructControl(controlFactory);
            SetSize();
        }

        private void SetSize()
        {
            this.MinimumSize = new Size(400, 200);
            this.Size = this.MinimumSize;
        }
/*
        ///<summary>
        /// Construct the Dialog form for any situation e.g. where the Form being closed has 
        /// Mutliple Business Objects is a wizard etc.
        ///</summary>
        /// <param name="controlFactory">The control Factory used to construct buttons, labels etc by ths control</param>
        ///<param name="fullDisplayName">Full display name for the BusienssObject(s)</param>
        ///<param name="isInValidState">Are the BusinessObject(s) in a valid state</param>
        ///<param name="isDirty"></param>
        ///<exception cref="ArgumentNullException">control Factory must not be null</exception>
        public CloseBOEditorDialogWin(IControlFactory controlFactory, string fullDisplayName, bool isInValidState, bool isDirty)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            ConstructControl(controlFactory, fullDisplayName, isInValidState, isDirty);
            SetSize();
        }*/



        private void ConstructControl(IControlFactory controlFactory)
        {
//TODO brett 31 Mar 2011: CF
                        throw new NotImplementedException(); 

/*            IButtonGroupControl buttonGroupControl = controlFactory.CreateButtonGroupControl();
            CancelCloseBtn = buttonGroupControl.AddButton("CancelClose", "Cancel Close", ButtonClick);
            CloseWithoutSavingBtn = buttonGroupControl.AddButton("CloseWithoutSaving", "&Close without saving", ButtonClick);
            SaveAndCloseBtn = buttonGroupControl.AddButton("SaveAndClose","&Save & Close", ButtonClick);
            
            _label = controlFactory.CreateLabel();
            BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(this);
            layoutManager.AddControl(_label, BorderLayoutManager.Position.Centre);
            layoutManager.AddControl(buttonGroupControl, BorderLayoutManager.Position.South);*/
        }

        private void ButtonClick(object sender, EventArgs eventArgs)
        {
            var button = sender as IButton;
            if (button == null) return;
            BOEditorDialogResult = (CloseBOEditorDialogResult)Enum.Parse(typeof(CloseBOEditorDialogResult), button.Name, true);
            this.Close();
        }
    }

}