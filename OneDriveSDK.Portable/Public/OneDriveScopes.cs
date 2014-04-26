using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.OneDrive
{
    public enum OneDriveScopes
    {
        [OAuthScope("wl.basic")]
        BasicProfile,

        [OAuthScope("wl.photos")]
        Photos,

        [OAuthScope("wl.skydrive")]
        ReadOnly,

        [OAuthScope("wl.skydrive_update")]
        ReadWrite,

        [OAuthScope("wl.contacts_skydrive")]
        SharedItems,

        [OAuthScope("wl.offline_access")]
        OfflineAccess
    }

    public class OAuthScopeAttribute : Attribute 
    {
        public string OAuthScope { get; private set; }
        public OAuthScopeAttribute(string oauthScope)
        {
            this.OAuthScope = oauthScope;
        }

        public static string OAuthScopeForEnumValue<T>(T enumValue)
        {
            Type t = typeof(T);
            var memberInfo = t.GetMember(enumValue.ToString()).FirstOrDefault();
            if (null == memberInfo) return string.Empty;
            OAuthScopeAttribute attrib = (OAuthScopeAttribute)memberInfo.GetCustomAttributes(typeof(OAuthScopeAttribute), true).FirstOrDefault();
            if (null != attrib)
                return attrib.OAuthScope;
            return string.Empty;
        }

        public static string OAuthScopeForValues<T>(T[] values)
        {
            StringBuilder sb = new StringBuilder();
            foreach (T value in values)
            {
                string scopeName = OAuthScopeForEnumValue(value);
                if (sb.Length > 0 && !string.IsNullOrEmpty(scopeName))
                    sb.Append(" ");
                sb.Append(scopeName);
            }
            return sb.ToString();
        }
    }
}
