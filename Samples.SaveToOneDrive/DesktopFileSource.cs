using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OneDriveSamples.SaveToOneDrive
{
    public class DesktopFileSource : Microsoft.Live.IFileSource
    {
        public string LocalPath { get; private set; }
        public DesktopFileSource(string path)
        {
            this.LocalPath = path;
        }

        public string Filename
        {
            get { return Path.GetFileName(LocalPath); }
        }

        public System.IO.Stream GetReadStream()
        {
            FileStream stream = new FileStream(this.LocalPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return stream;
        }
    }
}
