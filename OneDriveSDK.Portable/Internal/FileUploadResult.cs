using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Microsoft.OneDrive
{
    internal class FileUploadResult
    {
        public static FileUploadResult FromJson(string json)
        {
            return JsonConvert.DeserializeObject<FileUploadResult>(json);
        }

        [JsonProperty("id")]
        public string Identifier { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("source")]
        public Uri Source { get; set; }

    }
}
