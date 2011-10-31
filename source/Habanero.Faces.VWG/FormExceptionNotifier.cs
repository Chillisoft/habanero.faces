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
using System.Drawing;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.Faces.Base;
using Habanero.Util;
using FormStartPosition=Gizmox.WebGUI.Forms.FormStartPosition;

namespace Habanero.Faces.VWG
{
    /// <summary>
    /// Provides a form that displays an exception to the user
    /// </summary>
    public class FormExceptionNotifier : IExceptionNotifier
    {
        private string exceptionMessage ;

        ///<summary>
        /// The last exception logged by the exception notifier
        ///</summary>
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
        }

        /// <summary>
        /// Displays a dialog with exception information to the user
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="furtherMessage">Additional error messages</param>
        /// <param name="title">The title</param>
        public void Notify(Exception ex, string furtherMessage, string title)
        {
            exceptionMessage = furtherMessage + Environment.NewLine + ex.Message;
            if (Gizmox.WebGUI.Common.Global.Context == null)
            {
                // If the VWG Global Context has not been initialised yet, then no VWG forms can be displayed.
                // As a result, then best action to take is to just rethrow this error and let the ASP error handler display it.
                throw ex;
            }
            if (ex is UserException || ex is BusinessObjectException)
            {
                MessageBox.Show(furtherMessage + Environment.NewLine + ex.Message, title);
            }
            else
            {
                CollapsibleExceptionNotifyForm form = new CollapsibleExceptionNotifyForm(ex, furtherMessage, title);
                form.ShowDialog();
            }
        }

        /// <summary>
        /// Provides a form to display the exception message, using a "More Detail"
        /// button that collapses or uncollapses the error detail panel
        /// </summary>
        private class CollapsibleExceptionNotifyForm : FormVWG, IControlHabanero
        {
            private readonly Exception _exception;
            private readonly PanelVWG _summaryPanel;
            private PanelVWG _fullDetailPanel;
            private readonly IButton _moreDetailButton;
            private TextBoxVWG _errorDetails;
            private CheckBoxVWG _showStackTrace;
            private const int SUMMARY_HEIGHT = 150;
            private const int FULL_DETAIL_HEIGHT = 300;
            private const int BUTTONS_HEIGHT = 50;

            /// <summary>
            /// Constructor that sets up the error message form
            /// </summary>
            public CollapsibleExceptionNotifyForm(Exception ex, string furtherMessage, string title)
            {
                _exception = ex;

                _summaryPanel = new PanelVWG();
                _summaryPanel.Text = title;
                _summaryPanel.Height = SUMMARY_HEIGHT;
                ITextBox messageTextBox = GetSimpleMessage(ex.Message);
                ILabel messageLabel = GetErrorLabel(furtherMessage);
                BorderLayoutManager summaryManager = new BorderLayoutManagerVWG(_summaryPanel, GlobalUIRegistry.ControlFactory);
                summaryManager.AddControl(messageLabel, BorderLayoutManager.Position.North);
                summaryManager.AddControl(messageTextBox, BorderLayoutManager.Position.Centre);

                IButtonGroupControl buttonsOK = GlobalUIRegistry.ControlFactory.CreateButtonGroupControl();
                buttonsOK.AddButton("&OK", OKButtonClickHandler);
                buttonsOK.Height = BUTTONS_HEIGHT;

                IButtonGroupControl buttonsDetail = GlobalUIRegistry.ControlFactory.CreateButtonGroupControl();
                buttonsDetail.AddButton("Email Error", EmailErrorClickHandler);
                _moreDetailButton = buttonsDetail.AddButton("More Detail �", MoreDetailClickHandler);
                buttonsDetail.Height = BUTTONS_HEIGHT;
                buttonsDetail.Width = 2 * (_moreDetailButton.Width + 9);

                SetFullDetailsPanel();

                BorderLayoutManager manager = //new BorderLayoutManagerVWG(this, GlobalUIRegistry.ControlFactory);
                    GlobalUIRegistry.ControlFactory.CreateBorderLayoutManager(this);
                manager.AddControl(_summaryPanel, BorderLayoutManager.Position.North);
                manager.AddControl(buttonsDetail, BorderLayoutManager.Position.West);
                manager.AddControl(buttonsOK, BorderLayoutManager.Position.East);
                manager.AddControl(_fullDetailPanel, BorderLayoutManager.Position.South);


                Text = title;
                Width = 600;
                Height = SUMMARY_HEIGHT + BUTTONS_HEIGHT + 16;
                StartPosition = FormStartPosition.Manual;
                Location = new Point(50, 50);
                Resize += ResizeForm;
            }

            private void EmailErrorClickHandler(object sender, EventArgs e)
            {
                try
                {
                        
                    string emailTo = GlobalRegistry.Settings.GetString("EMAIL_TO");
                    string[] emailAddresses = emailTo.Split(new char[] { ';' });
                    string emailFrom = GlobalRegistry.Settings.GetString("EMAIL_FROM");

                    string emailContent = ExceptionUtilities.GetExceptionString(_exception, 0, true);

                    EmailSender emailSender = new EmailSender(emailAddresses, emailFrom, _exception.Source, emailContent, "");
                    //Todo : check Send Authenticated for security purposes?
                    emailSender.SmtpServerHost = GlobalRegistry.Settings.GetString("SMTP_SERVER");
                    string port = GlobalRegistry.Settings.GetString("SMTP_SERVER_PORT");
                    if (!String.IsNullOrEmpty(port))
                    {
                        emailSender.SmtpServerPort = Convert.ToInt32(port);
                    }
                    bool enableSSL = GlobalRegistry.Settings.GetBoolean("SMTP_ENABLE_SSL");
                    emailSender.EnableSSL = enableSSL;
                    //string authUsername = GlobalRegistry.Settings.GetString("SMTP_AUTH_USERNAME");
                    //string authPassword = GlobalRegistry.Settings.GetString("SMTP_AUTH_PASSWORD");
                    //if (!String.IsNullOrEmpty(authPassword))
                    //{
                    //    authPassword = Encryption.Decrypt(authPassword);
                    //}
                    //if (!String.IsNullOrEmpty(authUsername))
                    //{
                    //    emailSender.SendAuthenticated(authUsername, authPassword);
                    //}
                    //else
                    //{
                    emailSender.Send();
                    //}
                }
                catch(Exception ex)
                {
                    MessageBox.Show("The error message was not sent due to the following error : " + Environment.NewLine + ex.Message);
                }
            }

            /// <summary>
            /// Creates the red error label that appears at the top
            /// </summary>
            private static ILabel GetErrorLabel(string message)
            {
                ILabel messageLabel = GlobalUIRegistry.ControlFactory.CreateLabel(" " + message, true);
                messageLabel.TextAlign = ContentAlignment.BottomLeft;
                messageLabel.BackColor = Color.Red;
                messageLabel.ForeColor = Color.White;
                messageLabel.Font = new Font(messageLabel.Font.FontFamily, 10);
                messageLabel.Height = 18;
                return messageLabel;
            }

            /// <summary>
            /// Creates the text box that shows the error summary at the top
            /// </summary>
            private static ITextBox GetSimpleMessage(string message)
            {
                TextBoxVWG messageTextBox = new TextBoxVWG();
                messageTextBox.Text = message;
                messageTextBox.Multiline = true;
                messageTextBox.ScrollBars = Gizmox.WebGUI.Forms.ScrollBars.Both;
                messageTextBox.ReadOnly = true;
                messageTextBox.Font = new Font(messageTextBox.Font.FontFamily, 10);
                return messageTextBox;
            }

            /// <summary>
            /// Sets up the panel that shows the error details
            /// </summary>
            private void SetFullDetailsPanel()
            {
                _fullDetailPanel = new PanelVWG();
                _fullDetailPanel.Text = "Error Detail";
                _fullDetailPanel.Height = FULL_DETAIL_HEIGHT;
                _fullDetailPanel.Visible = false;
                _errorDetails = new TextBoxVWG();
                _errorDetails.Text = ExceptionUtilities.GetExceptionString(_exception, 0, false);
                _errorDetails.Multiline = true;
                _errorDetails.ScrollBars = Gizmox.WebGUI.Forms.ScrollBars.Both;
                _showStackTrace = new CheckBoxVWG();
                _showStackTrace.Text = "&Show stack trace";
                _showStackTrace.CheckedChanged += ShowStackTraceClicked;
                BorderLayoutManager detailsManager = new BorderLayoutManagerVWG(_fullDetailPanel, GlobalUIRegistry.ControlFactory);
                detailsManager.AddControl(_errorDetails, BorderLayoutManager.Position.Centre);
                detailsManager.AddControl(_showStackTrace, BorderLayoutManager.Position.South);
            }

            /// <summary>
            /// Handles the event of the OK button being pressed on the
            /// exception form, which closes the form
            /// </summary>
            private void OKButtonClickHandler(object sender, EventArgs e)
            {
                Close();
            }

            /// <summary>
            /// Expands the form when the "More Details" button is clicked
            /// </summary>
            private void MoreDetailClickHandler(object sender, EventArgs e)
            {
                if (!_fullDetailPanel.Visible)
                {
                    Height = _summaryPanel.Height + BUTTONS_HEIGHT + 16 + FULL_DETAIL_HEIGHT;
                    Width = 750;
                    _fullDetailPanel.Visible = true;
                    _moreDetailButton.Text = "� &Less Detail";
                }
                else
                {
                    Height = _summaryPanel.Height + BUTTONS_HEIGHT + 16;
                    _fullDetailPanel.Visible = false;
                    _moreDetailButton.Text = "&More Detail �";
                }
            }

            /// <summary>
            /// Toggles the showing of the stack trace in the error details
            /// </summary>
            private void ShowStackTraceClicked(object sender, EventArgs e)
            {
                _errorDetails.Text = ExceptionUtilities.GetExceptionString(_exception, 0, _showStackTrace.Checked);
            }

            /// <summary>
            /// Scales the components when the form is resized
            /// </summary>
            private void ResizeForm(object sender, EventArgs e)
            {
                int sdHeight = Height - BUTTONS_HEIGHT - 16;
                if (sdHeight > SUMMARY_HEIGHT)
                {
                    sdHeight = SUMMARY_HEIGHT;
                }
                _summaryPanel.Height = sdHeight;
                int heightRemaining = Height - BUTTONS_HEIGHT - sdHeight - 16;
                _fullDetailPanel.Height = heightRemaining > 0 ? heightRemaining : 0;
            }
        }
    }
}