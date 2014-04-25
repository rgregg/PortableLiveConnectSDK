using Microsoft.Live;
using Microsoft.OneDrive;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OneDriveSamples
{
    public partial class FormMicrosoftAccountAuth : Form
    {
        private const string OAuthDesktopEndPoint = "https://login.live.com/oauth20_desktop.srf";

        #region Properties
        private string StartUrl { get; set; }
        private string EndUrl { get; set; }
        public AuthResult AuthResult { get; private set; }
        #endregion

        #region Constructor
        public FormMicrosoftAccountAuth(string startUrl, string endUrl)
        {
            InitializeComponent();

            this.StartUrl = startUrl;
            this.EndUrl = endUrl;
            this.FormClosing += FormMicrosoftAccountAuth_FormClosing;
        }
        #endregion

        #region Private Methods
        private void FormMicrosoftAccountAuth_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void FormMicrosoftAccountAuth_Load(object sender, EventArgs e)
        {
            this.webBrowser.Navigated += webBrowser_Navigated;
            this.webBrowser.Navigate(this.StartUrl);
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            this.Text = webBrowser.DocumentTitle;

            if (this.webBrowser.Url.AbsoluteUri.StartsWith(EndUrl))
            {
                this.AuthResult = new AuthResult(this.webBrowser.Url);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }
        #endregion

        public Task<DialogResult> ShowDialogAsync()
        {
            TaskCompletionSource<DialogResult> tcs = new TaskCompletionSource<DialogResult>();

            this.FormClosed += (s, e) =>
            {
                tcs.SetResult(this.DialogResult);
            };
            this.Show();

            return tcs.Task;
        }

        #region Static Methods

        
        public static async Task<Microsoft.OneDrive.OneDriveClient> GetOneDriveClientAsync(string clientId, string[] scopes, string refreshToken = null)
        {
            LiveAuthClient authClient = new LiveAuthClient(clientId);

            if (null == refreshToken)
            {
                string startUrl = authClient.GetLoginUrl(scopes);
                string endUrl = OAuthDesktopEndPoint;
                FormMicrosoftAccountAuth authForm = new FormMicrosoftAccountAuth(startUrl, endUrl);

                DialogResult result = await authForm.ShowDialogAsync();

                if (DialogResult.OK == result)
                {
                    return await OnAuthComplete(authClient, authForm.AuthResult);
                }
            }
            else
            {
                LiveLoginResult result = await authClient.IntializeAsync(scopes);
                LiveConnectSession session = result.Session;
                return new OneDriveClient(new LiveConnectClient(session));
            }

            return null;
        }

        private static async Task<OneDriveClient> OnAuthComplete(LiveAuthClient authClient, AuthResult result)
        {
            if (null != result.AuthorizeCode)
            {
                try
                {
                    LiveConnectSession session = await authClient.ExchangeAuthCodeAsync(result.AuthorizeCode);
                    var OneDrive = new OneDriveClient(new LiveConnectClient(session));
                    return OneDrive;
                }
                catch (LiveAuthException aex)
                {
                    throw;
                }
                catch (LiveConnectException cex)
                {
                    throw;
                }
            }

            return null; ;
        }
        #endregion
    }

    public class AuthResult
    {
        public AuthResult(Uri resultUri)
        {
            string[] queryParams = resultUri.Query.TrimStart('?').Split('&');
            foreach (string param in queryParams)
            {
                string[] kvp = param.Split('=');
                switch (kvp[0])
                {
                    case "code":
                        this.AuthorizeCode = kvp[1];
                        break;
                    case "error":
                        this.ErrorCode = kvp[1];
                        break;
                    case "error_description":
                        this.ErrorDescription = Uri.UnescapeDataString(kvp[1]);
                        break;
                }
            }
        }

        public string AuthorizeCode { get; private set; }
        public string ErrorCode { get; private set; }
        public string ErrorDescription { get; private set; }
    }
}
