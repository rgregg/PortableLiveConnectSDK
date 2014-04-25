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
                this.OneDrive = await FormMicrosoftAccountAuth.GetOneDriveClientAsync(Properties.Resources.ONEDRIVE_CLIENT_ID, new string[] { "wl.skydrive_update" });
                if (null == this.OneDrive)
                {
                    MessageBox.Show("Failed to connect to OneDrive. Try again later.");
                    return;
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

        private async Task UploadFileToOneDrive(IFileSource fileToUpload)
        {
            OneDriveItem rootFolder = await OneDrive.GetOneDriveRootAsync();
            OneDriveItem[] itemsInRootFolder = await rootFolder.GetChildItemsAsync();

            var uploadedItem = await rootFolder.UploadFileAsync(fileToUpload, OverwriteOption.Rename, null);

            Stream downloadDataStream = await uploadedItem.DownloadFileAsync();

            var outputStream = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "download.txt"), FileMode.Create, FileAccess.Write);
            await downloadDataStream.CopyToAsync(outputStream);
            await outputStream.FlushAsync();
            outputStream.Close();
        }

        

        private async Task LogInToOneDrive()
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
