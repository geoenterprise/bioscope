using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace MobileApp.Pages
{
    public partial class SignupPage : ContentPage
    {
        private readonly HttpClient _httpClient;

        private sealed record SignupResponse(string token, Guid userId, string displayName);
        private sealed record SignupRequest(string Email, string Password, string DisplayName);

        public SignupPage(HttpClient httpClient)
        {
            InitializeComponent();
            _httpClient = httpClient;
        }

        private async void OnSignupClicked(object sender, EventArgs e)
        {
            var name = DisplayNameEntry.Text?.Trim();
            var email = EmailEntry.Text?.Trim();
            var password = PasswordEntry.Text;
            var confirm = ConfirmPasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "All fields are required.", "OK");
                return;
            }

            if (password != confirm)
            {
                await DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            var signupReq = new SignupRequest(email, password, name);

            try
            {
                var resp = await _httpClient.PostAsJsonAsync("api/auth/register", signupReq);
                if (!resp.IsSuccessStatusCode)
                {
                    var msg = await resp.Content.ReadAsStringAsync();
                    await DisplayAlert("Sign Up Failed", msg, "OK");
                    return;
                }

                var body = await resp.Content.ReadFromJsonAsync<SignupResponse>();
                if (body is null || string.IsNullOrWhiteSpace(body.token))
                {
                    await DisplayAlert("Error", "Unable to create account. Please try again.", "OK");
                    return;
                }

                // Save token & user info
                Preferences.Set("auth_token", body.token);
                Preferences.Set("user_id", body.userId.ToString());
                Preferences.Set("display_name", body.displayName);

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", body.token);

                await DisplayAlert("Welcome!", $"Account created for {body.displayName}", "Continue");

                // Redirect to main or discoveries page
                // await Navigation.PushAsync(new HomePage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Something went wrong: {ex.Message}", "OK");
            }
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage(_httpClient));
        }
    }
}
