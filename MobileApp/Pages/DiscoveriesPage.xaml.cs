using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using MobileApp.Models;
using System.Text.Json.Serialization;

namespace MobileApp.Pages;

public partial class DiscoveriesPage : ContentPage
{
    private readonly MobileApp.Services.AuthService _auth = new(); // ✅ must be inside the class
    private readonly Guid _userId;

    // If you have a class-level HttpClient, declare it here
    private readonly HttpClient _httpClient;

    public DiscoveriesPage(string userId)
    {
        InitializeComponent();

        _userId = Guid.Parse(userId);

#if DEBUG
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://xxxxxxxxxx:7022/")
        };
#else
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://xxxxxxxxx:7022/")
        };
#endif

        // 🧭 Toolbar (simple navigation)
        ToolbarItems.Clear();
        ToolbarItems.Add(new ToolbarItem("Home", null, async () => await Navigation.PopToRootAsync()));
        ToolbarItems.Add(new ToolbarItem("My Discoveries", null, async () => await Navigation.PopAsync()));
        ToolbarItems.Add(new ToolbarItem
        {
            Text = "Logout",
            Command = new Command(async () => await OnLogoutClicked())
        });
        // ToolbarItems.Add(new ToolbarItem("Logout", null, OnLogoutClicked));

        LoadDiscoveries();
    }

    private async Task OnLogoutClicked()
    {
        var confirm = await DisplayAlert("Sign out", "Are you sure you want to log out?", "Sign out", "Cancel");
        if (!confirm) return;

        // clear prefs and auth header
        _auth.ClearSession(_httpClient);

        // ✅ so, this is better than PopToRootAsync — ensures a full reset
        Application.Current!.MainPage = new NavigationPage(new MainPage());
    }

    private async void LoadDiscoveries()
    {
        var response = await _httpClient.GetAsync($"api/discoveries/user/{_userId}");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var discoveries = JsonSerializer.Deserialize<List<Discovery>>(json, options);

            if (discoveries == null || discoveries.Count == 0)
            {
                await DisplayAlert("Debug", "No discoveries found after deserialization!", "OK");
            }
            else
            {
               
                // await DisplayAlert("Debug", $"Loaded {discoveries.Count} discoveries", "OK");
            }

            DiscoveriesCollection.ItemsSource = discoveries;
        }
    }

    private async void OnDiscoveryClick(object sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is Discovery selectedDiscovery)
        {
            await Navigation.PushAsync(new DiscoveryDetailsPage(selectedDiscovery));
        }
    }
}
