using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace MobileApp.Pages
{
    public partial class LoginPage : ContentPage
    {
        private readonly HttpClient _httpClient;

        // Match the JSON returned by /api/auth/login.
        private sealed record LoginResponse(string token, Guid userId, string displayName);
        
        // Use DI: HttpClient is provided by MauiProgram
        public LoginPage(HttpClient httpClient)
        {
            InitializeComponent();
            _httpClient = httpClient;

            // Attach saved token (if any)
            var savedToken = Preferences.Get("auth_token", null);
            if (!string.IsNullOrWhiteSpace(savedToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", savedToken);
            }
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text?.Trim();
            var password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Email and password are required.", "OK");
                return;
            }

            var loginRequest = new { Email = email, Password = password };

            try
            {
                var resp = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

                if (!resp.IsSuccessStatusCode)
                {
                    // Keep generic in prod
                    await DisplayAlert("Login Failed", "Invalid email or password.", "OK");
                    return;
                }

                var body = await resp.Content.ReadFromJsonAsync<LoginResponse>();
                if (body is null || string.IsNullOrWhiteSpace(body.token))
                {
                    await DisplayAlert("Login Failed", "Please try again.", "OK");
                    return;
                }

                // Store token and attach to future requests
                Preferences.Set("auth_token", body.token);
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", body.token);

                await DisplayAlert("Welcome", "Login successful!", "Continue");

                // TODO: navigate to main page
                // await Navigation.PushAsync(new HomePage());
            }
            catch (Exception ex)
            {
                // Keep this generic in prod
                await DisplayAlert("Error", $"Unable to login. {ex.Message}", "OK");
            }
        }

        private async void OnSignupClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignupPage());
        }
    }
}
