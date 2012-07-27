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
using System.Drawing;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Faces.Base;
using Habanero.Util;
using log4net.Appender;
using FormStartPosition=System.Windows.Forms.FormStartPosition;
using MessageBoxButtons=System.Windows.Forms.MessageBoxButtons;
using MessageBoxIcon=System.Windows.Forms.MessageBoxIcon;
using ScrollBars=Habanero.Faces.Base.ScrollBars;

namespace Habanero.Faces.Win
{
    /// <summary>
    /// Provides a form that displays an exception to the user
    /// </summary>
    public class FormExceptionNotifier : IExceptionNotifier
    {
        private static readonly IControlFactory _controlFactory = GlobalUIRegistry.ControlFactory;
        private string _exceptionMessage ;

        ///<summary>
        /// The last exception logged by the exception notifier
        ///</summary>
        public string ExceptionMessage
        {
            get { return _exceptionMessage; }
        }
        /// <summary>
        /// Displays a dialog with exception information to the user
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="furtherMessage">Additional error messages</param>
        /// <param name="title">The title</param>
        public void Notify(Exception ex, string furtherMessage, string title)
        {
            //new ExceptionNotifyForm(ex, furtherMessage, title).ShowDialog();
            _exceptionMessage = furtherMessage + Environment.NewLine + ex.Message;
            if (ex is UserException)
            {
                string message = ex.Message;
                if (!String.IsNullOrEmpty(furtherMessage))
                {
                    furtherMessage = furtherMessage.TrimEnd('.', ':');
                    message = furtherMessage + ":" + Environment.NewLine + message;
                }
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private class CollapsibleExceptionNotifyForm : FormWin
        {
            private readonly Exception _exception;
            private readonly IPanel _summary;
            private IPanel _fullDetail;
            private readonly IButton _moreDetailButton;
            private ITextBox _errorDetails;
            private ICheckBox _showStackTrace;
            private const int SUMMARY_HEIGHT = 150;
            private const int FULL_DETAIL_HEIGHT = 300;
            private const int BUTTONS_HEIGHT = 50;
            BorderLayoutManager _layoutManager;
            private bool _fullDetailsVisible;
            IButtonGroupControl _buttonsDetail;
            IButtonGroupControl _buttonsOK;

            /// <summary>
            /// Constructor that sets up the error message form
            /// </summary>
            public CollapsibleExceptionNotifyForm(Exception ex, string furtherMessage, string title)
            {
                this._exception = ex;

                this._summary = _controlFactory.CreatePanel();
                this._summary.Text = title;
                this._summary.Height = SUMMARY_HEIGHT;
                ITextBox messageTextBox = GetSimpleMessage(ex.Message);
                messageTextBox.ScrollBars = ScrollBars.Vertical;
                ILabel messageLabel = GetErrorLabel(furtherMessage);
                BorderLayoutManager summaryManager = _controlFactory.CreateBorderLayoutManager(_summary);
                summaryManager.AddControl(messageLabel, BorderLayoutManager.Position.North);
                summaryManager.AddControl(messageTextBox, BorderLayoutManager.Position.Centre);

                this._buttonsOK = _controlFactory.CreateButtonGroupControl();
                this._buttonsOK.AddButton("&OK", new EventHandler(OKButtonClickHandler));

                this._buttonsDetail = _controlFactory.CreateButtonGroupControl();
                this._buttonsDetail.AddButton("Email Error", EmailErrorClickHandler);
                this._moreDetailButton = _buttonsDetail.AddButton("More Detail �", MoreDetailClickHandler);
                this._buttonsDetail.Width = 2 * (_moreDetailButton.Width + 9);
                this._fullDetailsVisible = false;

                this.SetFullDetailsPanel();
                this.LayoutForm();

                this.Text = title;
                this.Width = 600;
                this.Height = SUMMARY_HEIGHT + BUTTONS_HEIGHT + 16;
                this.StartPosition = FormStartPosition.CenterScreen;
                //this.Resize += ResizeForm;
            }

            private void LayoutForm()
            {
                while (this.Controls.Count > 0)
                    this.Controls.Remove(this.Controls[0]);
                var heightBeforeLayout = this._summary.Height;
                var buttonsPanel = _controlFactory.CreatePanel();
                var buttonsManager = _controlFactory.CreateBorderLayoutManager(buttonsPanel);
                buttonsManager.AddControl(this._buttonsDetail, BorderLayoutManager.Position.West);
                buttonsManager.AddControl(this._buttonsOK, BorderLayoutManager.Position.East);
                buttonsPanel.Height = this._buttonsDetail.Height;

                var topPanel = _controlFactory.CreatePanel();
                var topManager = _controlFactory.CreateBorderLayoutManager(topPanel);
                topManager.AddControl(this._summary, BorderLayoutManager.Position.Centre);
                topManager.AddControl(buttonsPanel, BorderLayoutManager.Position.South);

                this._layoutManager = _controlFactory.CreateBorderLayoutManager(this);
                if (this._fullDetailsVisible)
                {
                    var detailsPanel = _controlFactory.CreatePanel();
                    var detailsManager = _controlFactory.CreateBorderLayoutManager(detailsPanel);
                    detailsManager.AddControl(this._fullDetail, BorderLayoutManager.Position.Centre);
                    this._layoutManager.AddControl(topPanel, BorderLayoutManager.Position.North);
                    this._layoutManager.AddControl(detailsPanel, BorderLayoutManager.Position.Centre);
                    this.MinimumSize = new Size(640, 400);
                }
                else
                {
                    this._layoutManager.AddControl(topPanel, BorderLayoutManager.Position.Centre);
                    this.MinimumSize = new Size(640, 250);
                }
            }

            private void EmailErrorClickHandler(object sender, EventArgs e)
            {
                try
                {
                    string userDescription = "";
                    ErrorDescriptionForm errorDescriptionForm = new ErrorDescriptionForm();
                    errorDescriptionForm.Closing +=
                        delegate { userDescription = errorDescriptionForm.ErrorDescriptionTextBox.Text; };
                    errorDescriptionForm.ShowDialog(this);

                    IDictionary dictionary = GetEmailErrorSettings();
                    string exceptionString = ExceptionUtilities.GetExceptionString(_exception, 0, true);
                    if (!string.IsNullOrEmpty(userDescription))
                    {
                        exceptionString = "User Description : " + Environment.NewLine + userDescription +
                                          Environment.NewLine + "  -  Exception : " + exceptionString;
                    }

                    if (dictionary != null)
                    {
                        try
                        {
                            SendErrorMessage(dictionary, exceptionString);
                            return;
                        }
                        catch (Exception ex)
                        {
                            exceptionString += Environment.NewLine + "  -  Error sending mail via smtp: " +
                                               Environment.NewLine + ex.Message;
                        }
                    }
                    System.Diagnostics.Process.Start("mailto:?subject=" + _exception.Source + "&body=" + exceptionString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The error message was not sent due to the following error : " + Environment.NewLine +
                                    ex.Message);
                }
            }

            private void SendErrorMessage(IDictionary dictionary, string emailContent)
            {
                string smtpServer = (string)dictionary["smtp_server"];
                string emailTo = (string)dictionary["email_to"];
                string[] emailAddresses = emailTo.Split(new char[]{';'});

                string emailFrom = (string)dictionary["email_from"];

                //string emailContent = ExceptionUtilities.GetExceptionString(_exception, 0, true);

                EmailSender emailSender = new EmailSender(emailAddresses, emailFrom, _exception.Source, emailContent, "");
                ////Todo : check Send Authenticated for security purposes?
                
                emailSender.SmtpServerHost = smtpServer;
                string port = (string)dictionary["smtp_port"];
                if (!String.IsNullOrEmpty(port))
                {
                    emailSender.SmtpServerPort = Convert.ToInt32(port);
                }
                bool enableSSL = Convert.ToBoolean(dictionary["smtp_enable_ssl"]); 
                emailSender.EnableSSL = enableSSL;
                emailSender.Send();
            }

            /// <summary>
            /// Creates the red error label that appears at the top
            /// </summary>
            private static ILabel GetErrorLabel(string message)
            {
                ILabel messageLabel = _controlFactory.CreateLabel(" " + message, true);
                messageLabel.TextAlign = ContentAlignment.BottomLeft;
                messageLabel.BackColor = Color.Red;
                messageLabel.ForeColor = Color.White;
                return messageLabel;
            }

            /// <summary>
            /// Creates the text box that shows the error summary at the top
            /// </summary>
            private static ITextBox GetSimpleMessage(string message)
            {
                ITextBox messageTextBox = _controlFactory.CreateTextBox();
                messageTextBox.Text = message;
                messageTextBox.Multiline = true;
                return messageTextBox;
            }

            /// <summary>
            /// Sets up the panel that shows the error details
            /// </summary>
            private void SetFullDetailsPanel()
            {
                _fullDetail = _controlFactory.CreatePanel();
                _fullDetail.Text = "Error Detail";

                _errorDetails = _controlFactory.CreateTextBox();
                _errorDetails.Text = ExceptionUtilities.GetExceptionString(_exception, 0, false);
                _errorDetails.Multiline = true;
                _showStackTrace = _controlFactory.CreateCheckBox();
                _showStackTrace.Text = "&Show stack trace";
                _showStackTrace.CheckedChanged += ShowStackTraceClicked;

                var manager = _controlFactory.CreateBorderLayoutManager(_fullDetail);
                manager.AddControl(_errorDetails, BorderLayoutManager.Position.Centre);
                var chkPanel = _controlFactory.CreatePanel();
                var vgap = LayoutManager.DefaultGapSize + LayoutManager.DefaultBorderSize;
                if (GlobalUIRegistry.UIStyleHints != null)
                    vgap = GlobalUIRegistry.UIStyleHints.LayoutHints.DefaultVerticalGap + GlobalUIRegistry.UIStyleHints.LayoutHints.DefaultBorderSize;
                chkPanel.Height = _showStackTrace.Height + 2 * vgap;
                var chkManager = _controlFactory.CreateBorderLayoutManager(chkPanel);
                chkManager.AddControl(_showStackTrace, BorderLayoutManager.Position.West);
                manager.AddControl(chkPanel, BorderLayoutManager.Position.South);
            }

            /// <summary>
            /// Handles the event of the OK button being pressed on the
            /// exception form, which closes the form
            /// </summary>
            private void OKButtonClickHandler(object sender, EventArgs e)
            {
                this.Close();
            }

            /// <summary>
            /// Expands the form when the "More Details" button is clicked
            /// </summary>
            private void MoreDetailClickHandler(object sender, EventArgs e)
            {
                this._fullDetailsVisible = !this._fullDetailsVisible;
                this._moreDetailButton.Text = (this._fullDetailsVisible) ? "� &Less Detail" : "&More Detail �";
                this.LayoutForm();
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
                /*
                int sdHeight = Height - BUTTONS_HEIGHT - 16;
                if (sdHeight > SUMMARY_HEIGHT)
                {
                    sdHeight = SUMMARY_HEIGHT;
                }
                _summary.Height = sdHeight;
                int heightRemaining = Height - BUTTONS_HEIGHT - sdHeight - 16;
                _fullDetail.Height = heightRemaining > 0 ? heightRemaining : 0;
                 * */
            }

            private static IDictionary GetEmailErrorSettings()
            {
                IDictionary dictionary = ((IDictionary)System.Configuration.ConfigurationManager.GetSection("EmailErrorConfig"));
                return dictionary;
            }
        }
    }
}