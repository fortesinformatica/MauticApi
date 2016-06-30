using Flurl;

namespace MauticApi.Client.Models
{
    public class MauticClientConfiguration
    {
        public MauticClientConfiguration(string baseUrl)
        {
            BaseUrl = BuildUrl(baseUrl);
        }

        private static string BuildUrl(string baseUrl)
        {
            if (baseUrl.EndsWith("/index.php"))
                return baseUrl;

            return baseUrl.AppendPathSegment("index.php");
        }

        public string BaseUrl { get; }

        public static string OAuth2Url => "/oauth/v2/authorize";
        public static string OAuth2LoginUrl => "/oauth/v2/authorize_login_check";
        public static string OAuth2AccessTokenUrl => "/oauth/v2/token";
    }
}
