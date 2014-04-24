using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.OneDrive
{
    public sealed class SharingAccessScope
    {
        [JsonProperty("access")]
        public string Access { get; set; }
    }
}
