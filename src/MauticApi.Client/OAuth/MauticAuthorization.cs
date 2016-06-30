using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Flurl;
using Flurl.Http;
using MauticApi.Client.Models;
using MauticApi.Client.OAuth.Models;


namespace MauticApi.Client.Oauth
{
    public class MauticAuthorization
    {
        private readonly OAuth2Credentials _oAuth2Credentials;
        private readonly MauticClientConfiguration _configuration;


        private static string _accessToken;

        public MauticAuthorization(OAuth2Credentials oAuth2Credentials, MauticClientConfiguration configuration)
        {
            _oAuth2Credentials = oAuth2Credentials;
            _configuration = configuration;
        }

        public async Task Authorize()
            => await GenerateToken(await GetCodeAsync());

        private async Task GenerateToken(string code)
        {
            var token = await _configuration.BaseUrl.AppendPathSegments(MauticClientConfiguration.OAuth2AccessTokenUrl)
                .PostUrlEncodedAsync(new
                {
                    client_id = _oAuth2Credentials.ClientId,
                    client_secret = _oAuth2Credentials.ClientSecret,
                    grant_type = "authorization_code",
                    redirect_uri = _oAuth2Credentials.RedirectUri,
                    code
                }).ReceiveJson();

            _accessToken = token.access_token;
        }

        private async Task<string> GetCodeAsync()
        {
            var responseLogin = await DoLogin();
            return HttpUtility.ParseQueryString(responseLogin.RequestMessage.RequestUri.Query)["code"];
        }

        private async Task<HttpResponseMessage> DoLogin()
        {
            var client = _configuration.BaseUrl
                .AppendPathSegments(MauticClientConfiguration.OAuth2Url)
                .SetQueryParam("client_id", _oAuth2Credentials.ClientId)
                .SetQueryParam("grant_type", "authorization_code")
                .SetQueryParam("redirect_uri", _oAuth2Credentials.RedirectUri, true)
                .SetQueryParam("response_type", "code").EnableCookies();

            await client.GetStringAsync();

            client.Url = _configuration.BaseUrl.AppendPathSegments(MauticClientConfiguration.OAuth2LoginUrl);

            var httpResponseMessage = await client.PostUrlEncodedAsync(new { _username = _oAuth2Credentials.UserName, _password = _oAuth2Credentials.Password });
            return httpResponseMessage;
        }
    }
}
