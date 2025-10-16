using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using MobileApp.Models;
using System.Text.Json.Serialization;



namespace MobileApp.Pages;

public partial class SuggestionDetailsPage : ContentPage
{

    private Suggestion _suggestion;

    public SuggestionDetailsPage(Suggestion suggestion)
    {
        InitializeComponent();

        _suggestion = suggestion ?? throw new ArgumentNullException(nameof(suggestion));


        if (_suggestion.PlantDetails == null)
            _suggestion.PlantDetails = new PlantDetails();

        if (_suggestion.PlantDetails.Common_Names == null || !_suggestion.PlantDetails.Common_Names.Any())
            _suggestion.PlantDetails.Common_Names = new List<string> { "Unknown" };

        if (_suggestion.PlantDetails.Wiki_Description == null)
            _suggestion.PlantDetails.Wiki_Description = new WikiDescription
            {
                Value = "No description available."
            };


        BindingContext = _suggestion;
    }

    private async void SaveSuggestion(object sender, EventArgs e)
    {
        try
        {
            var discoveryData = new
            {
                WikiDescription = _suggestion.PlantDetails.Wiki_Description?.Value ?? "",
                Confidence = _suggestion.Probability,
                AssetUrl = _suggestion.Similar_Images.FirstOrDefault()?.Url ?? "",
                CommonName = _suggestion.PlantDetails?.Common_Names?.FirstOrDefault() ?? "Unknown",
                ScientificName = _suggestion.PlantDetails?.Scientific_Name ?? "",
                Comment = CommentEditor.Text?.Trim()
            };

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

            var response = await httpClient.PostAsJsonAsync("api/discoveries/create", discoveryData);

            var responseBody = await response.Content.ReadAsStringAsync();
            // await DisplayAlert("Response", responseBody, "OK");

            using var doc = JsonDocument.Parse(responseBody);

            var userId = doc.RootElement
                .GetProperty("discovery")
                .GetProperty("user")
                .GetProperty("id")
                .GetGuid();


            // await DisplayAlert("UserId", userId.ToString(), "OK");

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Great!", "Discovery was created correctly.", "OK");

                await Navigation.PushAsync(new DiscoveriesPage(userId.ToString()));
            }
            else
            {
                await DisplayAlert("Error", $"Status: {response.StatusCode}\n{responseBody}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.ToString(), "OK");
        }
    }

    private async void OnCancelClick(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    





}







