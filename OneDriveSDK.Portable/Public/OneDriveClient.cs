using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Live;

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
            LiveOperationResult result = await this.LiveClient.GetAsync("/me/skydrive");
            System.Diagnostics.Debug.WriteLine("json: {0}", result.RawResult);
            return OneDriveItem.CreateFromRawJson(result.RawResult, this);
        }
    }
}
