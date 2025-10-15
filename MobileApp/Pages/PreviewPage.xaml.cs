using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;



namespace MobileApp.Pages;

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
            HttpClient client;

            #if DEBUG
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                client = new HttpClient(handler);
            #else
                client = new HttpClient();
            #endif
    
            
            using var form = new MultipartFormDataContent();
            using var stream = await _photo.OpenReadAsync();
            var imageContent = new StreamContent(stream);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            form.Add(imageContent, "photo", _photo.FileName);



            var response = await client.PostAsync("https://xxxxxxxxxx:7022/api/organisms/upload", form);
            var json = await response.Content.ReadAsStringAsync();

            await Navigation.PushAsync(new SuggestionsPage(json));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errorrrrrr: {ex.Message}");
            await DisplayAlert("Errorrrrrrr", ex.Message, "OK");
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
