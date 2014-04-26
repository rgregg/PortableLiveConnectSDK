using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.OneDrive;

namespace OneDriveSamples.BrowseFolders
{
    public partial class FormBrowseOneDrive : Form
    {
        private OneDriveClient ODClient { get; set; }
        private OneDriveItem CurrentFolder { get; set; }

        private enum RootFolders
        {
            Files = 0,
            RecentDocs = 1,
            AllPhotos = 2,
            Shared = 3
        }

        public FormBrowseOneDrive()
        {
            InitializeComponent();
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            if (null == ODClient)
            {
                ODClient = await FormMicrosoftAccountAuth.GetOneDriveClientAsync(Properties.Resources.ONEDRIVE_CLIENT_ID, new OneDriveScopes[] { OneDriveScopes.ReadWrite, OneDriveScopes.Photos, OneDriveScopes.SharedItems, OneDriveScopes.OfflineAccess });
                if (null != ODClient)
                {
                    buttonLogin.Text = "Logout";
                    OnRootFolderChange((RootFolders)comboBoxScope.SelectedIndex);
                }
            }
            else
            {
                ODClient = null;
                buttonLogin.Text = "Login";
            }
        }

        private async void OnRootFolderChange(RootFolders root)
        {
            if (null == ODClient) return;
            
            OneDriveItem[] sharedItems;
            OneDriveItem parentItem;
            switch (root)
            {
                case RootFolders.Files:
                    parentItem = await ODClient.GetOneDriveRootAsync();
                    AddFolderToHierarchy(parentItem, true);
                    await LoadContentsOfFolder(parentItem);
                    break;

                case RootFolders.RecentDocs:
                    sharedItems = await ODClient.GetRecentItems();
                    AddFolderToHierarchy(new OneDriveItem() { Name = "Recent" }, true);
                    PopulateContents(sharedItems);
                    break;

                case RootFolders.AllPhotos:
                    parentItem = await ODClient.GetNamedFolderProperties(NamedFolder.Pictures);
                    AddFolderToHierarchy(parentItem, true);
                    await LoadContentsOfFolder(parentItem);
                    break;

                case RootFolders.Shared: 
                    sharedItems = await ODClient.GetSharedItems();
                    AddFolderToHierarchy(new OneDriveItem() { Name = "Shared" }, true);
                    PopulateContents(sharedItems);
                    break;
            }
        }

        private bool IsLoggedIn()
        {
            return null != ODClient;
        }

        private void AddFolderToHierarchy(OneDriveItem item, bool isFirstItem = false)
        {
            LinkLabel label = new LinkLabel();

            if (!isFirstItem)
            {
                label.Text = "> " + item.Name;
                label.LinkArea = new LinkArea(2, item.Name.Length);
            }
            else
            {
                label.Text = item.Name;
                flowPanelHierarchy.Controls.Clear();
            }
            label.AutoSize = true;
            label.Tag = item;
            label.LinkClicked += (sender, args) =>
            {
                flowPanelHierarchy.SuspendLayout();
                int index = flowPanelHierarchy.Controls.IndexOf((Control)sender);
                for (int i = flowPanelHierarchy.Controls.Count - 1; i > index; i--)
                {
                    flowPanelHierarchy.Controls.RemoveAt(i);
                }
                flowPanelHierarchy.ResumeLayout();
                LoadContentsOfFolder(((Control)sender).Tag as OneDriveItem);
            };

            flowPanelHierarchy.Controls.Add(label);
        }

        private async Task LoadContentsOfFolder(OneDriveItem parentItem)
        {
            if (!IsLoggedIn()) return;
            if (null == parentItem) return;

            this.CurrentFolder = parentItem;

            this.UseWaitCursor = true;
           
            OneDriveItem[] contentsOfFolder = await parentItem.GetChildItemsAsync();

            PopulateContents(contentsOfFolder);

            this.UseWaitCursor = false;
        }

        private void PopulateContents(OneDriveItem[] contentsOfFolder)
        {
            bool waitCursor = this.UseWaitCursor;
            this.UseWaitCursor = true;

            flowPanelItems.SuspendLayout();
            flowPanelItems.Controls.Clear();
            foreach (OneDriveItem item in contentsOfFolder.OrderBy(odi => odi.Name))
            {
                Label itemLabel = CreateLabelForItem(item);
                flowPanelItems.Controls.Add(itemLabel);
            }
            flowPanelItems.ResumeLayout();
            this.UseWaitCursor = waitCursor;
        }

        private Label CreateLabelForItem(OneDriveItem item)
        {
            Label itemLabel = new Label();
            itemLabel.Text = item.Name;
            itemLabel.Font = new Font("Segoe UI", 9.75f);
            itemLabel.TextAlign = ContentAlignment.BottomLeft;
            itemLabel.ForeColor = Color.White;
            itemLabel.BackColor = Color.RoyalBlue;
            itemLabel.AutoSize = false;
            itemLabel.Size = new Size(150, 100);
            itemLabel.Margin = new Padding(10);
            itemLabel.Padding = new Padding(3);
            itemLabel.Tag = item;
            itemLabel.Cursor = Cursors.Hand;

            itemLabel.Click += (sender, args) =>
            {
                OneDriveItem selectedItem = ((Control)sender).Tag as OneDriveItem;
                if (null == selectedItem) return;

                if (selectedItem.ItemType == OneDriveItemType.Album
                    || selectedItem.ItemType == OneDriveItemType.Folder)
                {
                    OnSelectFolder(selectedItem);
                }
                else
                {
                    OnSelectFile(selectedItem);
                }
            };

            return itemLabel;
        }

        private void OnSelectFolder(OneDriveItem folder)
        {
            if (folder == this.CurrentFolder) return;

            AddFolderToHierarchy(folder);
            LoadContentsOfFolder(folder);
        }

        private void OnSelectFile(OneDriveItem file)
        {
            // TODO: Do something when the user selects a file.
        }

        private void FormBrowseOneDrive_Load(object sender, EventArgs e)
        {
            comboBoxScope.SelectedIndex = 0;
        }

        private void comboBoxScope_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnRootFolderChange((RootFolders)comboBoxScope.SelectedIndex);
        }
    }
}
