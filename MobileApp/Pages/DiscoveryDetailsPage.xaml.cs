using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using MobileApp.Models;
using System.Text.Json.Serialization;



namespace MobileApp.Pages;

public partial class DiscoveryDetailsPage : ContentPage
{

    private Discovery _discovery;

    public DiscoveryDetailsPage(Discovery discovery)
    {
        InitializeComponent();

        _discovery = discovery ?? throw new ArgumentNullException(nameof(discovery));

        BindingContext = _discovery;

        LoadCommentsAsync(_discovery.DiscoveryId);

    }


    // private async Task LoadCommentsAsync(Guid discoveryId)
    // {
    // #if DEBUG
    //     var handler = new HttpClientHandler
    //     {
    //         ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
    //     };
    //     using var httpClient = new HttpClient(handler)
    //     {
    //         BaseAddress = new Uri("https://xxxxxxx:7022/")
    //     };
    // #else
    //     using var httpClient = new HttpClient
    //     {
    //         BaseAddress = new Uri("https://xxxxxxx:7022/")
    //     };
    // #endif

    //     try
    //     {
    //         var response = await httpClient.GetAsync($"api/comments/byDiscovery/{discoveryId}");

    //         if (response.IsSuccessStatusCode)
    //         {
    //             var json = await response.Content.ReadAsStringAsync();
    //             var comments = JsonSerializer.Deserialize<List<Comment>>(json, new JsonSerializerOptions
    //             {
    //                 PropertyNameCaseInsensitive = true
    //             });

    //             CommentsList.ItemsSource = comments;
    //         }
    //         else
    //         {
    //             await DisplayAlert("Error", "Could not load comments", "OK");
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         await DisplayAlert("Error", $"Network error: {ex.Message}", "OK");
    //     }
    // }


}







