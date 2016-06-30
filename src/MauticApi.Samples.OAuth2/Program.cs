using MauticApi.Client.Models;
using MauticApi.Client.Oauth;
using MauticApi.Client.OAuth.Models;

namespace MauticApi.Samples.OAuth2
{
    class Program
    {
        static void Main(string[] args)
        {
            var oauthCredentials = new OAuth2Credentials("CLIENT_ID",
                "CLIENT_SECRET",
                "REDIRECT",
                "USER", "PWD");
            var auth = new MauticAuthorization(oauthCredentials, new MauticClientConfiguration("URL_MAUTIC"));
            auth.Authorize().Wait();
        }
    }
}
