using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Live
{
    public interface IFileSource
    {
        string Filename { get; }

        System.IO.Stream GetReadStream();
        System.IO.Stream GetWriteStream();
    }
}
