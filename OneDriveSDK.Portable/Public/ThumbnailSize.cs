using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.OneDrive
{
    public enum ThumbnailSize
    {

        [OneDriveMetadata("thumbnail")]
        Thumbnail,
        
        /// <summary>
        /// Gets an item preview up to 100x100 pixels
        /// </summary>
        [OneDriveMetadata("small")]
        Small,

        /// <summary>
        /// Gets an item preview up to 200x200 pixels
        /// </summary>
        [OneDriveMetadata("album")]
        Album,

        /// <summary>
        /// Gets an item preview up to 800x800 pixels
        /// </summary>
        [OneDriveMetadata("normal")]
        Normal
    }

    public class OneDriveMetadataAttribute : Attribute
    {
        public string Data { get; private set; }
        public OneDriveMetadataAttribute(string data)
        {
            this.Data = data;
        }

        public static string MetadataForValue<T>(T enumValue)
        {
            Type t = typeof(T);
            var memberInfo = t.GetMember(enumValue.ToString()).FirstOrDefault();
            if (null == memberInfo) return string.Empty;
            OneDriveMetadataAttribute attrib = (OneDriveMetadataAttribute)memberInfo.GetCustomAttributes(typeof(OneDriveMetadataAttribute), true).FirstOrDefault();
            if (null != attrib)
                return attrib.Data;
            return string.Empty;
        }
    }
}
