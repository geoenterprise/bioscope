using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using MobileApp.Models;
using System.Text.Json.Serialization;



namespace MobileApp.Pages;

public partial class UpdateDescriptionPage : ContentPage
{

    private Discovery _discovery;

    private string _originalDescription;

    public UpdateDescriptionPage(Discovery discovery)
    {
        InitializeComponent();

        _discovery = discovery ?? throw new ArgumentNullException(nameof(discovery));

        BindingContext = _discovery;

        _originalDescription = _discovery.WikiDescription;

    }


    private async void UpdateDescriptionAsync(object sender, EventArgs e)
    {
        if (_discovery.WikiDescription==_originalDescription)
        {
            await DisplayAlert("Note", "No changes in description", "OK");
            return;
        }       


        #if DEBUG
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://192.168.1.72:4077/")
        };
        #else
            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://192.168.1.72:4077/")
            };
        #endif
        
        try
        {
            var updateData = new
            {
                WikiDescription = _discovery.WikiDescription
            };

            var content = new StringContent(
                JsonSerializer.Serialize(updateData),
                Encoding.UTF8,
                "application/json"
            );        

            var response = await httpClient.PatchAsync($"api/discoveries/updateDescription/{_discovery.DiscoveryId}", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Great!", "Your Description was updated", "OK");

                _originalDescription = _discovery.WikiDescription;

                await Navigation.PushAsync(new DiscoveriesPage(_discovery.UserId.ToString()));
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", $"No updated: {error}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Exception: {ex.Message}", "OK");
        }
        
        

 
      }
    
}







