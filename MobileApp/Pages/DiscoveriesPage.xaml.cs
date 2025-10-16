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
    private readonly Guid _userId;
    public DiscoveriesPage(string userId)
    {
        InitializeComponent();

        _userId = Guid.Parse(userId);

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDiscoveries();
    }

    private async Task LoadDiscoveries()
    {
        #if DEBUG
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            using var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://bioscopeapi.onrender.com/")
            };
        #else
            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://192.168.1.72:4077/")
            };
        #endif

        //alert with the user id. Only for testing
        // await DisplayAlert("UserId", _userId.ToString(), "OK");

        var response = await httpClient.GetAsync($"api/discoveries/user/{_userId}");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();

            // await DisplayAlert("Raw JSON", json, "OK");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var discoveries = JsonSerializer.Deserialize<List<Discovery>>(json, options);

            // var discoveries = JsonSerializer.Deserialize<List<Discovery>>(json, options);
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
                #if DEBUG
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                using var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri("https://bioscopeapi.onrender.com/") 
                };
                #else
                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri("https://bioscopeapi.onrender.com/")
                };
                #endif

                var response = await httpClient.DeleteAsync($"api/discoveries/delete/{selectedDiscovery.DiscoveryId}");

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




}








