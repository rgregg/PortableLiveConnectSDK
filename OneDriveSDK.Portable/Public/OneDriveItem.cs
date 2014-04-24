using Microsoft.Live;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

        public async Task<OneDriveItem> UploadFileFromPathAsync(IFileSource sourceItem, OverwriteOption option, IBackgroundTransferProvider btp)
        {
            var result = await Client.LiveClient.UploadAsync(string.Format("/{0}/files/{1}", 
                this.Identifier, 
                sourceItem.Filename), 
                sourceItem, 
                option, 
                btp);

            return null;
        }



    }
}

