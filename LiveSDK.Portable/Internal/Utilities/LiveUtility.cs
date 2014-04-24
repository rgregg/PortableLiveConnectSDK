namespace Microsoft.Live
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    internal static class LiveUtility
    {
        /// <summary>
        /// Validates a given string parameter where null is not allowed.
        /// </summary>
        public static void ValidateNotNullParameter(object value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Validates a given Enumerable string parameter where null or empty is not allowed.
        /// </summary>
        public static void ValidateNotEmptyStringEnumeratorArguement(IEnumerable<string> value, string paramName)
        {
            ValidateNotNullParameter(value, paramName);
            if (!value.GetEnumerator().MoveNext())
            {
                throw new ArgumentException(ErrorText.ParameterInvalidEnumerablerEmpty);
            }
        }
        
        /// <summary>
        /// Validates a given string parameter where null or white space is not allowed.
        /// </summary>
        public static void ValidateNotNullOrWhiteSpaceString(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (value == null)
                {
                    throw new ArgumentNullException(paramName);
                }
                else
                {
                    throw new ArgumentException(ErrorText.ParameterInvalidStringNullOrEmpty, paramName);
                }
            }
        }

        private const string UriSchemeHttp = "http";
        private const string UriSchemeHttps = "https";

        /// <summary>
        /// Validate a given Url parameter value.
        /// </summary>
        public static void ValidateUrl(string value, string paramName)
        {
            ValidateNotNullOrWhiteSpaceString(value, paramName);
            
            if (!value.StartsWith(UriSchemeHttp, StringComparison.OrdinalIgnoreCase) &&
                !value.StartsWith(UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(ErrorText.ParameterInvalidUrlFormat, paramName);
            }
        }
    }
}
