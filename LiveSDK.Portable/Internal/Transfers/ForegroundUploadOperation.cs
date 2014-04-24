using Microsoft.Live.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Live.Transfers
{
    internal class ForegroundUploadOperation : ApiOperation
    {

        public string Filename { get; private set; }
        public IProgress<LiveOperationProgress> Progress { get; private set; }
        public OverwriteOption OverwriteOption { get; private set; }
        public IFileSource FileSource { get; private set; }

        public ForegroundUploadOperation(LiveConnectClient client, Uri url, string filename, IFileSource inputFile, OverwriteOption option, IProgress<LiveOperationProgress> progress, SynchronizationContextWrapper syncContext)
            : base(client, url, ApiMethod.Upload, null, syncContext)
        {
            this.Filename = filename;
            this.Progress = progress;
            this.OverwriteOption = option;
            this.FileSource = inputFile;
        }




    }
}
