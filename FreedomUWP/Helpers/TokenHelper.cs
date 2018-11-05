using FreedomUWP.Core;
using Medium.Authentication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FreedomUWP.Helpers
{
    public class TokenHelper
    {
        static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public static bool InitiateTokenSystem()
        {
            bool tokensExist = false;

            // A null refresh date means that tokens were never generated
            // in the first place. (App never authenticated).

            if (localSettings.Values[Constants.refreshDateKey] != null)
            {
                tokensExist = true;

                string refreshToken = (string)localSettings.Values[Constants.refreshTokenKey];

                // Keep everything to one Time zone (UTC) to make it
                // easy to deal with time.

                DateTimeOffset refreshDate = (DateTimeOffset)localSettings.Values[Constants.refreshDateKey];
                if (DateTimeOffset.UtcNow > refreshDate)
                {
                    RefreshAccessToken(refreshToken);
                }

            }

            return tokensExist;
        }

        private static void RefreshAccessToken(string refreshToken)
        {
            Token refreshedAccessToken = OAuth.RefreshAccessToken(refreshToken);

            // Using DateTimeOffset so token expiry date
            // can be serialised in Local Settings

            DateTimeOffset expiryDate = new DateTimeOffset(refreshedAccessToken.ExpiresAt);

            SaveTokens(refreshedAccessToken);
            SaveExpiryDate(expiryDate);
        }

        public static void SaveTokenData(Token accessTokenData)
        {
            localSettings.Values[Constants.accessTokenKey] = accessTokenData.AccessToken;
            localSettings.Values[Constants.refreshTokenKey] = accessTokenData.RefreshToken;

            localSettings.Values[Constants.refreshDateKey] = new DateTimeOffset(accessTokenData.ExpiresAt);
            localSettings.Values[Constants.TokenTypeKey] = accessTokenData.TokenType;

            localSettings.Values[Constants.ScopeKey] = CreateScopeString(accessTokenData.Scope);

        }

        private static string CreateScopeString(Scope[] scope)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < scope.Length; i++)
            {
                sb.Append((int)scope[i]);
                if (i != scope.Length - 1)
                {
                    sb.Append(',');
                }
            }

            Debug.WriteLine("Scope: " + sb.ToString());
            return sb.ToString();

        }

        private static void SaveExpiryDate(DateTimeOffset expiryDate)
        {
            localSettings.Values[Constants.refreshDateKey] = expiryDate;
        }

        private static void SaveTokens(Token refreshedAccessToken)
        {
            localSettings.Values[Constants.accessTokenKey] = refreshedAccessToken.AccessToken;
            localSettings.Values[Constants.refreshTokenKey] = refreshedAccessToken.RefreshToken;
        }

        public static Token GetToken()
        {
            string accessToken = (string)localSettings.Values[Constants.accessTokenKey];
            string refreshToken = (string)localSettings.Values[Constants.refreshTokenKey];
            DateTime expiryDate = ((DateTimeOffset)localSettings.Values[Constants.refreshDateKey]).DateTime;
            string tokenType = (string)localSettings.Values[Constants.TokenTypeKey];
            Scope[] scope = GetScopeFromSettings();

            Token token = new Token()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = expiryDate,
                Scope = scope,
                TokenType = tokenType
            };

            return token;
        }

        private static Scope[] GetScopeFromSettings()
        {
            string scopeString = (string)localSettings.Values[Constants.ScopeKey];
            string[] scopeStringItems = scopeString.Split(',');
            Scope[] scopeItems = new Scope[scopeStringItems.Length];

            for (int i = 0; i < scopeStringItems.Length; i++)
            {
                scopeItems[i] = (Scope)int.Parse(scopeStringItems[i]);
            }

            return scopeItems;
        }

        public static void ClearTokenSettings()
        {
            localSettings.Values[Constants.TokenTypeKey] = null;
            localSettings.Values[Constants.ScopeKey] = null;
            localSettings.Values[Constants.refreshTokenKey] = null;
            localSettings.Values[Constants.accessTokenKey] = null;
            localSettings.Values[Constants.refreshDateKey] = null;
        }
    }
}
