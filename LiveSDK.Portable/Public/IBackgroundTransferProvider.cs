using Microsoft.Live.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Live
{
    public interface IBackgroundTransferProvider
    {
        DownloadOperation GetDownloadOperation(LiveConnectClient client, Uri url, IFileSource outputFile, IProgress<LiveOperationProgress> progress);
        ApiOperation GetUploadOperation(LiveConnectClient client, Uri url, IFileSource intputFile, OverwriteOption option, IProgress<LiveOperationProgress> progress);
    }
}
