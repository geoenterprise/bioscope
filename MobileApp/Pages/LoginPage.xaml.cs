using System;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Maui.Controls;
// Import your DTO or User class namespace
using MobileApp.Models;  // or wherever your shared User/DTO is


namespace MobileApp.Pages
{
    public partial class LoginPage : ContentPage
    {
        private readonly HttpClient _httpClient;

        public LoginPage()
        {
            InitializeComponent();

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://<your-api-url-or-localhost>:<port>/")
            };
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
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<User>();
                    await DisplayAlert("Welcome", $"Hello, {user.DisplayName}", "Continue");
                    // Navigate to a home page, e.g.:
                    // await Navigation.PushAsync(new HomePage());
                }
                else
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Login Failed", msg, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Unable to login: {ex.Message}", "OK");
            }
        }

        private async void OnSignupClicked(object sender, EventArgs e)
        {
            Console.WriteLine("All is fine");
            // await Navigation.PushAsync(new SignupPage());
        }
    }
}
