using FreedomUWP.View;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FreedomUWP.Core
{
    public static class OAuth
    {
        public async static Task<bool> Authenticate()
        {
            bool hasAuthenticated = false;

            const string _authCodeString = "code";

            // Contact developers@medium.com to get your client ID and client secret.
            var oAuthClient = new Medium.OAuthClient(Constants._clientID, Constants._clientSecret);

            // Build the URL where you can send the user to obtain an authorization code.
            var url = oAuthClient.GetAuthorizeUrl(
                "secretstate",
                Constants.callbackURL,
                new[]
                {
                    Medium.Authentication.Scope.BasicProfile,
                    Medium.Authentication.Scope.ListPublications,
                    Medium.Authentication.Scope.PublishPost,
                });

            url = url.Remove(url.LastIndexOf('&'));
            
            
            // (Send the user to the authorization URL to obtain an authorization code.)
            string result = await GetAuthCode(url);

            // Attempt to parse authorisation code from result
            // (Since result may not be a URL)
            try
            {
                
                Uri resultUri = new Uri(result);
                NameValueCollection queryDictionary = HttpUtility.ParseQueryString(resultUri.Query);
                string authCode = queryDictionary[_authCodeString];
                Debug.WriteLine(authCode);

                //Exchange the authorization code for an access token.


                var accessToken = oAuthClient.GetAccessToken(authCode, Constants.callbackURL);

                // When your access token expires, use the refresh token to get a new one.
                var newAccessToken = oAuthClient.GetAccessToken(accessToken.RefreshToken);

                hasAuthenticated = true;
            }
            catch (Exception)
            {

                
            }

            return hasAuthenticated;

        }

        private async static Task<string> GetAuthCode(string startURL)
        {

            string endURL = Constants.callbackURL;

            System.Uri startURI = new System.Uri(startURL);
            System.Uri endURI = new System.Uri(endURL);

            string result;


            try
            {
                var webAuthenticationResult =
                    await Windows.Security.Authentication.Web.WebAuthenticationBroker.AuthenticateAsync(
                    Windows.Security.Authentication.Web.WebAuthenticationOptions.None,
                    startURI,
                    endURI);

                switch (webAuthenticationResult.ResponseStatus)
                {
                    case Windows.Security.Authentication.Web.WebAuthenticationStatus.Success:
                        // Successful authentication. 
                        result = webAuthenticationResult.ResponseData.ToString();
                        break;
                    case Windows.Security.Authentication.Web.WebAuthenticationStatus.ErrorHttp:
                        // HTTP error. 
                        result = webAuthenticationResult.ResponseErrorDetail.ToString();
                        break;
                    default:
                        // Other error.
                        result = webAuthenticationResult.ResponseData.ToString();
                        break;
                }
            }
            catch (Exception ex)
            {
                // Authentication failed. Handle parameter, SSL/TLS, and Network Unavailable errors here. 
                result = ex.Message;
            }

            return result;
        }
    }
}
