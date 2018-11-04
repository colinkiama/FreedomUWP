using FreedomUWP.Core;
using Medium.Authentication;
using System;
using System.Collections.Generic;
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
            SaveExpiryDate(accessTokenData.ExpiresAt.ToUniversalTime());
            SaveTokens(accessTokenData);
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


    }
}
