using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.OneDrive
{
    public enum NamedFolder
    {
        [FolderPath("/skydrive/camera_roll")]
        CameraRoll,

        [FolderPath("/skydrive/my_documents")]
        Documents,

        [FolderPath("/skydrive/my_photos")]
        Pictures,

        [FolderPath("/skydrive/public_documents")]
        PublicDocuments
    }

    public class FolderPathAttribute : Attribute
    {
        public string Path { get; private set; }
        public FolderPathAttribute(string path)
        {
            this.Path = path;
        }

        public static string FolderPathForValue<T>(T enumValue)
        {
            Type t = typeof(T);
            var memberInfo = t.GetMember(enumValue.ToString()).FirstOrDefault();
            if (null == memberInfo) return string.Empty;
            FolderPathAttribute attrib = (FolderPathAttribute)memberInfo.GetCustomAttributes(typeof(FolderPathAttribute), true).FirstOrDefault();
            if (null != attrib)
                return attrib.Path;
            return string.Empty;
        }
    }
}
