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
        protected OneDriveClient Client { get; set; }
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


        public async Task<OneDriveItem[]> GetChildItemsAsync()
        {
            var result = await Client.LiveClient.GetAsync(string.Format("/{0}/files", this.Identifier));
            System.Diagnostics.Debug.WriteLine("json: {0}", result.RawResult);

            JObject obj = (JObject)JsonConvert.DeserializeObject(result.RawResult);
            var dataReader = obj["data"].CreateReader();

            var serializer = new JsonSerializer();
            var objects = serializer.Deserialize<List<OneDriveItem>>(dataReader);
            return objects.ToArray();
        }

        

        public async Task<OneDriveItem> UploadFileAsync(IFileSource sourceItem, OverwriteOption option, IBackgroundTransferProvider btp = null, IProgress<LiveOperationProgress> progress = null)
        {
            if (this.ItemType != OneDriveItemType.Folder && this.ItemType != OneDriveItemType.Album)
            {
                throw new InvalidOperationException("Cannot upload files to items that do not represent a folder or album");
            }

            LiveOperationResult result = await Client.LiveClient.UploadAsync(
                string.Format("/{0}",  this.Identifier), 
                sourceItem, 
                option, 
                btp, 
                progress);

            System.Diagnostics.Debug.WriteLine("UploadFile Result: {0}", result.RawResult);

            FileUploadResult fur = FileUploadResult.FromJson(result.RawResult);
            return await this.Client.GetItemFromIdentifier(fur.Identifier);
        }

        public async Task<System.IO.Stream> DownloadFileAsync(IBackgroundTransferProvider btp = null, IProgress<LiveOperationProgress> progress = null)
        {
            if (this.ItemType != OneDriveItemType.GenericFile)
            {
                throw new InvalidOperationException("Cannot download items that are not files");
            }

            LiveDownloadOperationResult result = await Client.LiveClient.DownloadAsync(string.Format("/{0}/content", this.Identifier), btp, progress);
            return result.Stream;
        }



    }
}

