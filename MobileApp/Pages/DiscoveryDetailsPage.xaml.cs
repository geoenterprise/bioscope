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


    private async Task LoadCommentsAsync(Guid discoveryId)
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

        try
        {
            var response = await httpClient.GetAsync($"api/comments/{discoveryId}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                //Only for testing
                // await DisplayAlert("Comments JSON", json, "OK");

                var comments = JsonSerializer.Deserialize<List<Comment>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // string allComments = string.Join("\n\n", comments.Select(c => $"{c.UserId}: {c.Body}"));
                // await DisplayAlert("Comments", allComments, "OK");

                CommentsList.ItemsSource = comments;
            }
            else
            {
                await DisplayAlert("Error", "Could not load comments", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Network error: {ex.Message}", "OK");
        }
    }
    
    private async void SaveCommentsAsync(object sender, EventArgs e)
    {
        #if DEBUG
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://192.168.1.77:4077/")
        };
        #else
            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://192.168.1.77:4077/")
            };
        #endif


        //
        var commentText = CommentEditor.Text;

        if (!string.IsNullOrWhiteSpace(commentText))
        {
            try
            {

                var comment = new
                {
                    Id = Guid.NewGuid(),
                    DiscoveryId = _discovery.DiscoveryId,
                    Body = commentText,
                    UserId = _discovery.UserId,
                    CreatedAt = DateTime.UtcNow
                };

                var response = await httpClient.PostAsJsonAsync("api/comments/create", comment);

                // var responseJson = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });

                // DisplayAlert("Comment", responseJson, "OK");
                if (response.IsSuccessStatusCode)
                {
                    CommentEditor.Text = string.Empty;

                    await DisplayAlert("Awesome!!", "Comment saved correctly", "OK");

                    await LoadCommentsAsync(_discovery.DiscoveryId);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"Error on save the comment: {error}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.Message, "OK");
            }
        }
        
    }


}







