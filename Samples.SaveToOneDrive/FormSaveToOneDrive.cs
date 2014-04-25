using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.OneDrive;

namespace OneDriveSamples.SaveToOneDrive
{
    public partial class FormSaveToOneDrive : Form, IRefreshTokenHandler
    {
        private const string OAuthDesktopEndPoint = "https://login.live.com/oauth20_desktop.srf";
        private LiveAuthClient AuthClient;
        private OneDriveClient OneDrive;
        
        public FormSaveToOneDrive()
        {
            InitializeComponent();

            AuthClient = new Microsoft.Live.LiveAuthClient(Properties.Resources.ONEDRIVE_CLIENT_ID, this);
        }

        private string[] GetAuthScopes()
        {
            return new string[] { "wl.skydrive_update" };
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All Files (*.*)|*.*";
            var result = ofd.ShowDialog();
            if (DialogResult.OK == result)
            {
                textBoxLocalFile.Text = ofd.FileName;
            }
        }

        private async void buttonUploadToOneDrive_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxLocalFile.Text))
            {
                MessageBox.Show("Select a file to upload first");
                return;
            }

            if (!File.Exists(textBoxLocalFile.Text))
            {
                MessageBox.Show("File path is invalid. Select a valid file and try again.");
                return;
            }

            DesktopFileSource selectedFile = new DesktopFileSource(textBoxLocalFile.Text);
            
            if (null == OneDrive)
            {
                if (string.IsNullOrEmpty(Properties.Settings.Default.RefreshToken))
                {
                    await PromptForLogin();
                }
                else
                {
                    LiveLoginResult result = await AuthClient.IntializeAsync(this.GetAuthScopes());
                    LiveConnectSession session = result.Session;
                    OneDrive = new OneDriveClient(new LiveConnectClient(session));
                }
            }
            try
            {
                await UploadFileToOneDrive(selectedFile);
                MessageBox.Show("File uploaded: " + selectedFile.Filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            
        }


        private async Task PromptForLogin()
        {
            string startUrl = AuthClient.GetLoginUrl(this.GetAuthScopes());
            string endUrl = OAuthDesktopEndPoint;
            FormMicrosoftAccountAuth authForm = new FormMicrosoftAccountAuth(startUrl, endUrl);
            DialogResult result = authForm.ShowDialog(this);
            if (DialogResult.OK == result)
            {
                await OnAuthComplete(authForm.AuthResult);
            }

            authForm.Dispose();
        }

        private async Task UploadFileToOneDrive(IFileSource fileToUpload)
        {
            OneDriveItem rootFolder = await OneDrive.GetOneDriveRootAsync();
            OneDriveItem[] itemsInRootFolder = await rootFolder.GetChildItemsAsync();

            await rootFolder.UploadFileAsync(fileToUpload, OverwriteOption.Rename, null);
        }

        private async Task<bool> OnAuthComplete(AuthResult result)
        {
            if (null != result.AuthorizeCode)
            {
                try
                {
                    LiveConnectSession session = await this.AuthClient.ExchangeAuthCodeAsync(result.AuthorizeCode);
                    OneDrive = new OneDriveClient(new LiveConnectClient(session));
                    Properties.Settings.Default.RefreshToken = session.RefreshToken;
                    return true;
                }
                catch (LiveAuthException aex)
                {
                    MessageBox.Show(aex.Message);
                }
                catch (LiveConnectException cex)
                {
                    MessageBox.Show(cex.Message);
                }
            }

            return false;
        }

        private void LogInToOneDrive()
        {
            
        }

        public async Task SaveRefreshTokenAsync(RefreshTokenInfo tokenInfo)
        {
            Properties.Settings.Default.RefreshToken = tokenInfo.RefreshToken;
            Properties.Settings.Default.Save();
        }

        public async Task<RefreshTokenInfo> RetrieveRefreshTokenAsync()
        {
            string storedToken = Properties.Settings.Default.RefreshToken;
            if (!string.IsNullOrEmpty(storedToken))
            {
                return new RefreshTokenInfo(storedToken);
            }
            
            return null;
        }
    }
}
