using Microsoft.Live.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Live.Transfers
{
    /// <summary>
    /// Implements a transfer provider that does not support background transfers. Used on platforms where no
    /// special background upload/download semantics exist.
    /// </summary>
    public class BasicTransferProvider : IBackgroundTransferProvider
    {
        public DownloadOperation GetDownloadOperation(LiveConnectClient client, Uri url, IFileSource outputFile, IProgress<LiveOperationProgress> progress, SynchronizationContextWrapper syncContext)
        {
            // TODO: This is include since we aren't download to outputFile.
            var downloadOp = new DownloadOperation(client, url, progress, syncContext);
            return downloadOp;
        }

        public ApiOperation GetUploadOperation(LiveConnectClient client, Uri url, IFileSource inputFile, OverwriteOption option, IProgress<LiveOperationProgress> progress, SynchronizationContextWrapper syncContext)
        {
            return new UploadOperation(client, url, inputFile.Filename, inputFile.GetReadStream(), option, progress, syncContext);
        }
    }
}
