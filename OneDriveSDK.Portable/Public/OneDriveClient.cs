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

        public OneDriveClient(LiveConnectClient client)
        {
            this.LiveClient = client;
        }

        public async Task<OneDriveItem> GetOneDriveRootAsync()
        {
            OneDriveItem rootItem = await GetObjectFromRequest<OneDriveItem>("/me/skydrive",
                    (json) => OneDriveItem.CreateFromRawJson(json, this));

            rootItem.Name = "OneDrive";     // Force the name to OneDrive, since the service returns SkyDrive
            return rootItem;
        }

        public async Task<OneDriveItem> GetItemFromIdentifier(string fileResourceIdentifier)
        {
            string pathForFileProperties = string.Format("/{0}", fileResourceIdentifier);
            return await GetObjectFromRequest<OneDriveItem>(pathForFileProperties,
                (json) => OneDriveItem.CreateFromRawJson(json, this));
        }


        private async Task<T> GetObjectFromRequest<T>(string getPath, Func<string, T> converter)
        {
            LiveOperationResult result = await this.LiveClient.GetAsync(getPath);
            System.Diagnostics.Debug.WriteLine("json: {0}", result.RawResult);
            return converter(result.RawResult);
        }

        public async Task<Quota> GetQuota()
        {
            return await GetObjectFromRequest<Quota>("/me/skydrive/quota", 
                (json) =>JsonConvert.DeserializeObject<Quota>(json));
        }

        public async Task<OneDriveItem[]> GetSharedItems()
        {
            LiveOperationResult result = await this.LiveClient.GetAsync("/me/skydrive/shared");
            System.Diagnostics.Debug.WriteLine("json: {0}", result.RawResult);
            return ConvertDataResponseToItems(result.RawResult, this);
        }

        public async Task<OneDriveItem> GetNamedFolderProperties(NamedFolder folder)
        {
            string pathToNamedFolder = string.Concat("/me" + FolderPathAttribute.FolderPathForValue(folder));
            return await GetObjectFromRequest<OneDriveItem>(pathToNamedFolder,
                (json) => OneDriveItem.CreateFromRawJson(json, this));
        }

        public async Task<OneDriveItem[]> GetRecentItems()
        {
            LiveOperationResult result = await this.LiveClient.GetAsync("me/skydrive/recent_docs");
            System.Diagnostics.Debug.WriteLine("json: {0}", result.RawResult);
            return ConvertDataResponseToItems(result.RawResult, this);
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
        
    }
}
