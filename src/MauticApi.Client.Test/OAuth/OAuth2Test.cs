using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Flurl.Http.Testing;
using MauticApi.Client.Models;
using MauticApi.Client.Oauth;
using MauticApi.Client.OAuth.Models;
using NUnit.Framework;

namespace MauticApi.Client.Test.Oauth
{
    [TestFixture]
    public class OAuth2Test
    {
        private HttpTest _httpTest;
        private MauticAuthorization _sut;
        private MauticClientConfiguration _mauticClientConfiguration;
        private OAuth2Credentials _oAuth2Credentials;

        [SetUp]
        public void CreateHttpTest()
        {

            _mauticClientConfiguration = new MauticClientConfiguration("http://test.com");
            _oAuth2Credentials = new OAuth2Credentials("clientId", "clientSecret", "http://callbackUri.com", "userName", "password");
            _sut = new MauticAuthorization(_oAuth2Credentials, _mauticClientConfiguration);

        }

        private void StubDefaultResponses()
        {
            _httpTest = new HttpTest();
            _httpTest
                .RespondWith(200, "OK")
                .RespondWith(200, "OK")
                .RespondWithJson(200,
                    new
                    {
                        access_token = "1234"
                    });
        }

        [Test]
        public async Task WhenAuthorizeShouldCallAuthorizeUrlWithRequiredParameters()
        {
            StubDefaultResponses();
            await _sut.Authorize();
            _httpTest.ShouldHaveCalled(_mauticClientConfiguration.BaseUrl + MauticClientConfiguration.OAuth2Url)
                .With(call => call.Request.RequestUri.Query.Contains($"client_id={_oAuth2Credentials.ClientId}"))
                .With(call => call.Request.RequestUri.Query.Contains("grant_type=authorization_code"))
                .With(call => call.Request.RequestUri.Query.Contains($"redirect_uri={_oAuth2Credentials.RedirectUri}"))
                .With(call => call.Request.RequestUri.Query.Contains("response_type=code"));
        }

        [Test]
        public async Task WhenAuthorizeShouldCallLoginUrlAsPOSTWithUserNameAndPassword()
        {
            StubDefaultResponses();
            await _sut.Authorize();
            _httpTest.ShouldHaveCalled(_mauticClientConfiguration.BaseUrl + MauticClientConfiguration.OAuth2LoginUrl)
                .WithVerb(HttpMethod.Post)
                .With(call => HttpUtility.ParseQueryString(call.RequestBody)["_username"] == _oAuth2Credentials.UserName)
                .With(call => HttpUtility.ParseQueryString(call.RequestBody)["_password"] == _oAuth2Credentials.Password);
        }

        [Test]
        public async Task WhenAuthorizeShouldCallAccessTokenUrlUrlWithParameters()
        {
            StubDefaultResponses();
            await _sut.Authorize();
            _httpTest.ShouldHaveCalled(_mauticClientConfiguration.BaseUrl + MauticClientConfiguration.OAuth2AccessTokenUrl)
                .With(call => HttpUtility.ParseQueryString(call.RequestBody)["client_id"] == _oAuth2Credentials.ClientId)
                .With(call => HttpUtility.ParseQueryString(call.RequestBody)["client_secret"] == _oAuth2Credentials.ClientSecret)
                .With(call => HttpUtility.ParseQueryString(call.RequestBody)["grant_type"] == "authorization_code")
                .With(call => HttpUtility.ParseQueryString(call.RequestBody)["redirect_uri"] == _oAuth2Credentials.RedirectUri);
        }

        [TearDown]
        public void DisposeHttpTest()
        {
            _httpTest.Dispose();
        }
    }
}
