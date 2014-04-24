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
    internal class BasicTransferProvider : IBackgroundTransferProvider
    {

        public DownloadOperation GetDownloadOperation(LiveConnectClient client, Uri url, IFileSource outputFile, IProgress<LiveOperationProgress> progress)
        {
            throw new NotImplementedException();
        }

        public ApiOperation GetUploadOperation(LiveConnectClient client, Uri url, IFileSource intputFile, OverwriteOption option, IProgress<LiveOperationProgress> progress)
        {
            throw new NotImplementedException();
        }
    }
}
