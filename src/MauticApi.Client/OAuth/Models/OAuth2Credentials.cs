namespace MauticApi.Client.OAuth.Models
{
    public class OAuth2Credentials
    {
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string RedirectUri { get; }
        public string UserName { get; }
        public string Password { get; }

        public OAuth2Credentials(string clientId, string clientSecret, string redirectUri, string userName, string password)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            RedirectUri = redirectUri;
            UserName = userName;
            Password = password;
        }
    }
}