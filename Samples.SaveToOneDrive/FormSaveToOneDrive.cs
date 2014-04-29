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
    public partial class FormSaveToOneDrive : Form, IProgress<LiveOperationProgress>
    {

        private OneDriveClient ODClient { get; set; }
        
        public FormSaveToOneDrive()
        {
            InitializeComponent();
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
            OneDriveItem uploadedItem = await UploadToOneDriveAsync(selectedFile);

            if (null != uploadedItem)
            {
                textBoxFileIdentifer.Text = uploadedItem.Identifier;
                MessageBox.Show("Upload complete");
            }
        }

        private async void buttonDownload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFileIdentifer.Text))
            {
                MessageBox.Show("You need to specify a file ID to download.");
                return;
            }

            try
            {
                OneDriveItem itemToDownload = await ODClient.GetItemFromIdentifier(textBoxFileIdentifer.Text);
                if (null != itemToDownload)
                {
                    Stream contentsOfFile = await DownloadFileAsync(itemToDownload);
                    
                    var outputStream = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), itemToDownload.Name), FileMode.Create, FileAccess.Write);
                    await contentsOfFile.CopyToAsync(outputStream);
                    await outputStream.FlushAsync();
                    outputStream.Close();
                }
                
                MessageBox.Show("Downloaded to your desktop as " + itemToDownload.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error downloading file: " + ex.Message);
            }
        }

        private async Task<OneDriveItem> UploadToOneDriveAsync(DesktopFileSource selectedFile)
        {
            bool isConnected = await ConnectToOneDriveAsync();
            if (!isConnected) return null;

            OneDriveItem uploadedItem = null;
            try
            {
                OneDriveItem rootFolder = await ODClient.GetOneDriveRootAsync();
                OneDriveItem[] itemsInRootFolder = await rootFolder.GetChildItemsAsync();
                uploadedItem = await rootFolder.UploadFileAsync(selectedFile, OverwriteOption.Rename, null, this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return uploadedItem;
        }

        private async Task<bool> ConnectToOneDriveAsync()
        {
            if (null == ODClient)
            {
                this.ODClient = await FormMicrosoftAccountAuth.GetOneDriveClientAsync(Properties.Resources.ONEDRIVE_CLIENT_ID,
                    new OneDriveScopes[] { OneDriveScopes.ReadWrite });
                if (null == this.ODClient)
                {
                    MessageBox.Show("Failed to connect to OneDrive. Try again later.");
                    return false;
                }
            }
            return true;
        }

        private async Task<Stream> DownloadFileAsync(OneDriveItem file)
        {
            bool isConnected = await ConnectToOneDriveAsync();
            if (!isConnected) return null;

            return await file.DownloadContentsAsync(null, this);
        }

        public void Report(LiveOperationProgress value)
        {
            this.Invoke(new MethodInvoker(() => { progressBar.Value = (int)value.ProgressPercentage; }));
        }

 
    }
}
