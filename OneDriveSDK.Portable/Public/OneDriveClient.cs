using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Live;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.OneDrive
{
    public class OneDriveClient
    {
        public LiveConnectClient LiveClient { get; private set; }

        /// <summary>
        /// Create a new instance of the OneDrive client connected to a LiveConnect client that has signed in.
        /// </summary>
        /// <param name="client"></param>
        public OneDriveClient(LiveConnectClient client)
        {
            // We should validate that the client is actually signed in at this point.
            this.LiveClient = client;
        }

        #region Public Methods
        /// <summary>
        /// Retrieve a reference to the root of the OneDrive namespace
        /// </summary>
        /// <returns></returns>
        public async Task<OneDriveItem> GetOneDriveRootAsync()
        {
            OneDriveItem rootItem = await GetObjectFromRequest<OneDriveItem>("/me/skydrive",
                    (json) => OneDriveItem.CreateFromRawJson(json, this));

            rootItem.Name = "OneDrive";     // Force the name to OneDrive, since the service returns SkyDrive
            return rootItem;
        }

        /// <summary>
        /// Return an item from a particular identifier
        /// </summary>
        /// <param name="fileResourceIdentifier"></param>
        /// <returns></returns>
        public async Task<OneDriveItem> GetItemFromIdentifier(string fileResourceIdentifier)
        {
            string pathForFileProperties = string.Format("/{0}", fileResourceIdentifier);
            return await GetObjectFromRequest<OneDriveItem>(pathForFileProperties,
                (json) => OneDriveItem.CreateFromRawJson(json, this));
        }

        /// <summary>
        /// Fetches the item represented by a particular path. Depending on the depth of the path, this may result in multiple calls to the service.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<OneDriveItem> GetItemFromPath(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return the total and remaining quota for the user
        /// </summary>
        /// <returns></returns>
        public async Task<Quota> GetQuota()
        {
            return await GetObjectFromRequest<Quota>("/me/skydrive/quota",
                (json) => JsonConvert.DeserializeObject<Quota>(json));
        }

        /// <summary>
        /// Return a collection of items that are shared with the logged in user.
        /// </summary>
        /// <returns></returns>
        public async Task<OneDriveItem[]> GetSharedItems()
        {
            LiveOperationResult result = await this.LiveClient.GetAsync("/me/skydrive/shared");
            System.Diagnostics.Debug.WriteLine("json: {0}", result.RawResult);
            return ConvertDataResponseToItems(result.RawResult, this);
        }

        /// <summary>
        /// Return an item that represents a special named folder from the service.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public async Task<OneDriveItem> GetNamedFolderProperties(NamedFolder folder)
        {
            string pathToNamedFolder = string.Concat("/me" + FolderPathAttribute.FolderPathForValue(folder));
            return await GetObjectFromRequest<OneDriveItem>(pathToNamedFolder,
                (json) => OneDriveItem.CreateFromRawJson(json, this));
        }

        /// <summary>
        /// Return the collection of recently opened items from the service.
        /// </summary>
        /// <returns></returns>
        public async Task<OneDriveItem[]> GetRecentItems()
        {
            LiveOperationResult result = await this.LiveClient.GetAsync("me/skydrive/recent_docs");
            System.Diagnostics.Debug.WriteLine("json: {0}", result.RawResult);
            return ConvertDataResponseToItems(result.RawResult, this);
        }

        /// <summary>
        /// Retrieves a thumbnail for a sharing URL to a folder, album, or file. Can accept either the full or shortened sharing URL.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="sharingUrlForItem"></param>
        /// <returns></returns>
        public async Task<byte[]> GetThumbnailAsync(ThumbnailSize size, Uri sharingUrlForItem)
        {
            // GET skydrive/get_item_preview?type=normal&url=http&3A%2F%2Fsdrv.ms%2F.....
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods
        private async Task<T> GetObjectFromRequest<T>(string getPath, Func<string, T> converter)
        {
            LiveOperationResult result = await this.LiveClient.GetAsync(getPath);
            System.Diagnostics.Debug.WriteLine("json: {0}", result.RawResult);
            return converter(result.RawResult);
        }

        internal static OneDriveItem[] ConvertDataResponseToItems(string jsonWithData, OneDriveClient client)
        {
            JObject obj = (JObject)JsonConvert.DeserializeObject(jsonWithData);
            var dataReader = obj["data"].CreateReader();

            var serializer = new JsonSerializer();
            var objects = serializer.Deserialize<List<OneDriveItem>>(dataReader);

            foreach (OneDriveItem item in objects)
            {
                item.Client = client;
            }

            return objects.ToArray();
        }
        #endregion
    }
}
