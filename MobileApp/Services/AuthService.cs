using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Maui.Storage;

namespace MobileApp.Services
{
    public sealed class AuthService
    {
        private const string TokenKey = "auth_token";
        private const string UserIdKey = "user_id";
        private const string DisplayNameKey = "display_name";

        public bool IsLoggedIn => !string.IsNullOrWhiteSpace(Token);

        public string? Token => Preferences.Get(TokenKey, null);
        public Guid? UserId
        {
            get
            {
                var s = Preferences.Get(UserIdKey, null);
                return Guid.TryParse(s, out var g) ? g : null;
            }
        }
        public string? DisplayName => Preferences.Get(DisplayNameKey, null);

        public void SaveSession(string token, Guid userId, string displayName)
        {
            Preferences.Set(TokenKey, token);
            Preferences.Set(UserIdKey, userId.ToString());
            Preferences.Set(DisplayNameKey, displayName);
        }

        public void AttachHeader(HttpClient http)
        {
            var token = Token;
            if (string.IsNullOrWhiteSpace(token))
            {
                http.DefaultRequestHeaders.Authorization = null;
            }
            else
            {
                http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public void ClearSession(HttpClient http)
        {
            Preferences.Remove(TokenKey);
            Preferences.Remove(UserIdKey);
            Preferences.Remove(DisplayNameKey);
            http.DefaultRequestHeaders.Authorization = null;
        }
    }
}
