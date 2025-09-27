using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json; 


namespace MobileApp;

public partial class PreviewPage : ContentPage
{
    private FileResult _photo;
    public PreviewPage(FileResult photo)
    {
        InitializeComponent();
        _photo = photo;
        LoadImage();
    }

    private async void LoadImage()
    {
        if (_photo != null)
        {
            var stream = await _photo.OpenReadAsync();
            PreviewImage.Source = ImageSource.FromStream(() => stream);
        }
    }

    private async void IdentifyClicked(object sender, EventArgs e)
    {
        if (_photo == null) return;

        try
        {
            string apiKey = "2dYRCiCaBfbLN6xm0I6drq23cZwVEkI88B1UhqmH6L6Idqvq5q";
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Api-Key", apiKey);

            using var form = new MultipartFormDataContent();
            using var stream = await _photo.OpenReadAsync();
            var imageContent = new StreamContent(stream);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            form.Add(imageContent, "images[]", _photo.FileName);

            var response = await client.PostAsync("https://api.plant.id/v2/identify", form);
            var json = await response.Content.ReadAsStringAsync();

            await Navigation.PushAsync(new SuggestionsPage(json));


            // var doc = JsonDocument.Parse(json);
            // var suggestions = doc.RootElement.GetProperty("suggestions");


            // if (suggestions.GetArrayLength() > 0)
            // {
            //     var suggestionList = new List<string>();
            //     foreach (var suggestion in suggestions.EnumerateArray())
            //     {
            //         var plantName = suggestion.GetProperty("plant_name").GetString();
            //         var probability = suggestion.GetProperty("probability").GetDouble();
            //         Console.WriteLine($"- {plantName} (confidence: {probability:P0})");
            //     }

            // }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

}







//namespace MobileApp
//{
//    public partial class MainPage : ContentPage
//    {
//        int count = 0;

//        public MainPage()
//        {
//            InitializeComponent();
//        }

//        private void OnCounterClicked(object sender, EventArgs e)
//        {
//            count++;

//            if (count == 1)
//                CounterBtn.Text = $"Clicked {count} time";
//            else
//                CounterBtn.Text = $"Clicked {count} times";

//            SemanticScreenReader.Announce(CounterBtn.Text);
//        }
//    }

//}
