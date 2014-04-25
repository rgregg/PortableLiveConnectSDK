using Microsoft.Live.Operations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Live
{

    /// <summary>
    /// This class contains the methods for LiveConnectClient to enable file and stream 
    /// transfers with the Live SDK.
    /// </summary>
    public sealed partial class LiveConnectClient
    {
        #region DownloadAsync Overrides

        public Task<LiveDownloadOperationResult> DownloadAsync(string path, IBackgroundTransferProvider btu)
        {
            return this.DownloadAsync(path, btu, new CancellationToken(false), null);
        }

        public Task<LiveDownloadOperationResult> DownloadAsync(string path, IFileSource destination, IBackgroundTransferProvider btu)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException(
                    "path",
                    String.Format(CultureInfo.CurrentUICulture, ResourceHelper.GetString("UrlInvalid"), "path"));
            }

            if (null == destination)
            {
                throw new ArgumentNullException("destination",
                   String.Format(CultureInfo.CurrentUICulture, ResourceHelper.GetString("InvalidNullParameter"), "destination"));
            }

            return this.InternalDownloadAsync(path, destination, btu, new CancellationToken(false), null);
        }

        public Task<LiveDownloadOperationResult> DownloadAsync(string path, IBackgroundTransferProvider btu, CancellationToken ct, IProgress<LiveOperationProgress> progress)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException(
                    "path",
                    String.Format(CultureInfo.CurrentUICulture, ResourceHelper.GetString("UrlInvalid"), "path"));
            }

            return this.InternalDownloadAsync(path, null, btu, ct, progress);
        }

        internal Task<LiveDownloadOperationResult> InternalDownloadAsync(string path, IFileSource destination, IBackgroundTransferProvider btu, CancellationToken ct, IProgress<LiveOperationProgress> progress)
        {
            if (this.Session == null)
            {
                throw new LiveConnectException(ApiOperation.ApiClientErrorCode, ResourceHelper.GetString("UserNotLoggedIn"));
            }

            var tcs = new TaskCompletionSource<LiveDownloadOperationResult>();

            var op = btu.GetDownloadOperation(this, this.GetResourceUri(path, ApiMethod.Download), destination, progress, syncContext);
            op.OperationCompletedCallback = (LiveDownloadOperationResult result) =>
            {
                if (result.IsCancelled)
                {
                    tcs.TrySetCanceled();
                }
                else if (result.Error != null)
                {
                    tcs.TrySetException(result.Error);
                }
                else
                {
                    tcs.TrySetResult(result);
                }
            };

            ct.Register(op.Cancel);

            op.Execute();

            return tcs.Task;
        }

        #endregion

        #region UploadAsync Overrides

        public Task<LiveOperationResult> UploadAsync(string path, IFileSource fileSource, OverwriteOption option, IBackgroundTransferProvider btu)
        {
            return this.UploadAsync(path, fileSource, option, btu, new CancellationToken(false), null);
        }

        public Task<LiveOperationResult> UploadAsync(string path, IFileSource fileSource, OverwriteOption option, IBackgroundTransferProvider btu, CancellationToken ct, IProgress<LiveOperationProgress> progress)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(
                    "path",
                    String.Format(CultureInfo.CurrentUICulture, ResourceHelper.GetString("UrlInvalid"),
                    "path"));
            }

            if (null == fileSource)
            {
                throw new ArgumentNullException(
                    "fileSource",
                   String.Format(CultureInfo.CurrentUICulture, ResourceHelper.GetString("InvalidNullParameter"),
                   "fileSource"));
            }

            if (null != fileSource && string.IsNullOrEmpty(fileSource.Filename))
            {
                throw new ArgumentException(
                    "fileName",
                    String.Format(CultureInfo.CurrentUICulture, ResourceHelper.GetString("InvalidNullOrEmptyParameter"),
                    "fileName"));
            }

            if (null == btu)
            {
                btu = new Microsoft.Live.Transfers.BasicTransferProvider();
            }

            ApiOperation op = btu.GetUploadOperation(this, this.GetResourceUri(path, ApiMethod.Upload), fileSource, option, progress, syncContext);
            return this.ExecuteApiOperation(op, ct);
        }

        #endregion
    }
}
