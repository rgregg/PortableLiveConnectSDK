using Microsoft.Live;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.OneDrive
{

    /// <summary>
    /// Represents the base class of object in OneDrive. This can be either any item 
    /// represented in OneDrive, such as a folder, shared folder, or file.
    /// </summary>
    public class OneDriveItem
    {
        internal OneDriveClient Client { get; set; }
        internal static OneDriveItem CreateFromRawJson(string rawJsonData, OneDriveClient sourceClient)
        {
            OneDriveItem item = JsonConvert.DeserializeObject<OneDriveItem>(rawJsonData);
            item.Client = sourceClient;

            return item;
        }

        #region Common Item Properties
        [JsonProperty("id")]
        public string Identifier { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("parent_id")]
        public string ParentIdentifier { get; set; }
        [JsonProperty("size")]
        public long Size { get; set; }
        [JsonProperty("upload_location")]
        public Uri UploadLocation { get; set; }
        [JsonProperty("comments_count")]
        public int CommentsCount { get; set; }
        [JsonProperty("comments_enabled")]
        public bool CommentsEnabled { get; set; }
        [JsonProperty("is_embeddable")]
        public bool IsEmbeddable { get; set; }
        [JsonProperty("count")]
        public long ChildItemCount { get; set; }
        [JsonProperty("link")]
        public Uri Link { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("shared_with")]
        public SharingAccessScope SharedWith { get; set; }
        [JsonProperty("created_time")]
        public string CreatedTimeUtc { get; set; }
        [JsonProperty("updated_time")]
        public string UpdatedTimeUtc { get; set; }
        [JsonProperty("client_updated_time")]
        public string ClientUpdatedTime { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Enum that indicates the type of object represented by this OneDriveItem.
        /// </summary>
        public OneDriveItemType ItemType
        {
            get
            {
                switch (this.Type)
                {
                    case "folder": 
                        return OneDriveItemType.Folder;
                    case "album":
                        return OneDriveItemType.Album;
                    case "file":
                        return OneDriveItemType.GenericFile;
                    default:
                        return OneDriveItemType.Unspecified;
                }
            }
        }

        /// <summary>
        /// Downloads the contents of a file represented by this OneDriveItem.
        /// </summary>
        /// <param name="btp"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        public async Task<System.IO.Stream> DownloadContentsAsync(IBackgroundTransferProvider btp = null, IProgress<LiveOperationProgress> progress = null)
        {
            if (this.ItemType != OneDriveItemType.GenericFile)
            {
                throw new InvalidOperationException("Cannot download items that are not files");
            }

            LiveDownloadOperationResult result = await Client.LiveClient.DownloadAsync(string.Format("/{0}/content", this.Identifier), btp, progress);
            return result.Stream;
        }

        /// <summary>
        /// Delete the item represented by this OneDriveItem. If the item is a folder or album, the contents of the folder will also be deleted.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Upload changes to this items meta data back to the service.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Moves the current item to a new parent item. Can move a folder to a new parent, or move a file into a new folder.
        /// </summary>
        /// <param name="parentItem"></param>
        /// <returns></returns>
        public async Task<bool> MoveToNewParentAsync(OneDriveItem parentItem)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the collection of OneDriveItem objects that are parented to the current item. The current item
        /// must be a folder or album, otherwise this command will fail.
        /// </summary>
        /// <returns></returns>
        public async Task<OneDriveItem[]> GetChildItemsAsync()
        {
            var result = await Client.LiveClient.GetAsync(string.Format("/{0}/files", this.Identifier));
            System.Diagnostics.Debug.WriteLine("json: {0}", result.RawResult);

            return OneDriveClient.ConvertDataResponseToItems(result.RawResult, this.Client);
        }

        /// <summary>
        /// Uploads a file into the OneDriveItem this command is called on. This command is only available on OneDriveItem instances that represents
        /// a folder or album.
        /// </summary>
        /// <param name="sourceItem"></param>
        /// <param name="option"></param>
        /// <param name="btp"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        public async Task<OneDriveItem> UploadFileAsync(IFileSource sourceItem, OverwriteOption option, IBackgroundTransferProvider btp = null, IProgress<LiveOperationProgress> progress = null)
        {
            if (this.ItemType != OneDriveItemType.Folder && this.ItemType != OneDriveItemType.Album)
            {
                throw new InvalidOperationException("Cannot upload files to items that do not represent a folder or album");
            }

            LiveOperationResult result = await Client.LiveClient.UploadAsync(
                string.Format("/{0}", this.Identifier),
                sourceItem,
                option,
                btp,
                progress);

            System.Diagnostics.Debug.WriteLine("UploadFile Result: {0}", result.RawResult);

            FileUploadResult fur = FileUploadResult.FromJson(result.RawResult);
            return await this.Client.GetItemFromIdentifier(fur.Identifier);
        }

        /// <summary>
        /// Creates a new subfolder as a child of the current item. If the item is not a folder or alubm, the command will throw an error.
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public async Task<OneDriveItem> CreateChildFolderAsync(string folderName)
        {
            throw new NotImplementedException();
        }



        #endregion


    }
}

