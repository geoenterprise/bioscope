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
    private readonly HttpClient _httpClient; // <-- HttpClient a nivel de clase

    public DiscoveriesPage(string userId)
    {
        InitializeComponent();

        _userId = Guid.Parse(userId);

        // 🧭 Configuración de HttpClient
        #if DEBUG
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://bioscopeapi.onrender.com/") // tu URL de debug
        };
        #else
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://bioscopeapi.onrender.com/") // tu URL de producción
        };
#endif

      
        ToolbarItems.Clear();
        ToolbarItems.Add(new ToolbarItem("Home", null, async () => await Navigation.PopToRootAsync()));
        ToolbarItems.Add(new ToolbarItem("My Discoveries", null, async () => await Navigation.PopAsync()));
        ToolbarItems.Add(new ToolbarItem
        {
            Text = "Logout",
            Command = new Command(async () => await OnLogoutClicked())
        });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDiscoveries();
    }

    private async Task LoadDiscoveries()
    {
        try
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
                    await DisplayAlert("Debug", "No discoveries found after deserialization!", "OK");

                DiscoveriesCollection.ItemsSource = discoveries;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", $"Could not load discoveries: {error}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Exception: {ex.Message}", "OK");
        }
    }

    private async void OnDetailsClick(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Discovery selectedDiscovery)
        {
            await Navigation.PushAsync(new DiscoveryDetailsPage(selectedDiscovery));
        }
    }

    private async void OnDescriptionClick(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Discovery selectedDiscovery)
        {
            await Navigation.PushAsync(new UpdateDescriptionPage(selectedDiscovery));
        }
    }

    private async void OnDeleteClick(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Discovery selectedDiscovery)
        {
            bool confirm = await DisplayAlert(
                "Confirm Delete", 
                $"Are you sure you want to delete '{selectedDiscovery.CommonName}'?", 
                "Yes", 
                "No"
            );

            if (!confirm)
                return;

            try
            {
                var response = await _httpClient.DeleteAsync($"api/discoveries/delete/{selectedDiscovery.DiscoveryId}");

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Deleted", "Discovery was deleted successfully.", "OK");
                    await LoadDiscoveries(); 
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"Could not delete: {error}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Exception: {ex.Message}", "OK");
            }
        }
    }

    private async Task OnLogoutClicked()
    {
        // Lógica de logout aquí
        await Navigation.PopToRootAsync();
    }
}









