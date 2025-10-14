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


        LoadDiscoveries();

    }
    
    private async void LoadDiscoveries()
    {
        #if DEBUG
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://192.168.1.72:7022/")
        };
        #else
        using var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://192.168.1.72:7022/")
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
                await DisplayAlert("Debug", $"Loaded {discoveries.Count} discoveries", "OK");
            }
            
            DiscoveriesCollection.ItemsSource = discoveries;
        }

    }
    
    private async void OnDiscoveryClick(object sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is Discovery selectedDiscovery)
        {
            // Navegar a la página de detalles del discovery
            await Navigation.PushAsync(new DiscoveryDetailsPage(selectedDiscovery));
        }
    }

}








