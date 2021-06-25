using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;

namespace ConsumerApplicationService.Authentication
{
    public class Auth
    {
        private string Domain { get; }
        private string Audience { get; }
        private string ClientId { get; }
        private string ClientSecret { get; }


        public Auth(string domain, string audience, string clientId, string clientSecret)
        {
            Domain = domain;
            Audience = audience;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }


        public string GetAccessToken()
        {
            var auth0Client = new AuthenticationApiClient(Domain);
            var tokenRequest = new ClientCredentialsTokenRequest()
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Audience = Audience
            };
            var tokenResponse =auth0Client.GetTokenAsync(tokenRequest).Result;
         
            return tokenResponse.AccessToken;
        }
    }
}