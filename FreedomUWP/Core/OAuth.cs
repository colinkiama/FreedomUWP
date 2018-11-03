using FreedomUWP.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FreedomUWP.Core
{
    public static class OAuth
    {
        public async static Task Authenticate()
        {
            // Contact developers@medium.com to get your client ID and client secret.
            var oAuthClient = new Medium.OAuthClient(Constants._clientID, Constants._clientSecret);

            // Build the URL where you can send the user to obtain an authorization code.
            var url = oAuthClient.GetAuthorizeUrl(
                "secretstate",
                Constants.callbackURL,
                new[]
                {
                    Medium.Authentication.Scope.BasicProfile,
                    Medium.Authentication.Scope.PublishPost
                });

            url = url.Remove(url.LastIndexOf('&'));
            // (Send the user to the authorization URL to obtain an authorization code.)
            string authCode = await GetAuthCode(url);

            Debug.WriteLine(authCode);

            // Exchange the authorization code for an access token.
            //var accessToken = oAuthClient.GetAccessToken("YOUR_AUTHORIZATION_CODE", Constants.callbackURL);

            //// When your access token expires, use the refresh token to get a new one.
            //var newAccessToken = oAuthClient.GetAccessToken(accessToken.RefreshToken);
        }

        private async static Task<string> GetAuthCode(string startURL)
        {

            string endURL = Constants.callbackURL;

            System.Uri startURI = new System.Uri(startURL);
            System.Uri endURI = new System.Uri(endURL);

            string result;

            AuthView authView = new AuthView();
            Frame currentFrame = (Frame)Window.Current.Content;
            Shell thisShell = (Shell)currentFrame.Content;
            thisShell.AddAuthView(authView);
            result = authView.Authenticate(startURL, endURL);
            
            return result;
        }
    }
}
