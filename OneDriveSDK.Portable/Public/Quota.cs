using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.OneDrive
{
    public class Quota
    {
        [JsonProperty("quota")]
        public long TotalSpace { get; set; }
        
        [JsonProperty("available")]
        public long AvailableSpace { get; set; }
    }
}
