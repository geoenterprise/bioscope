using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using Microsoft.Maui.Media;
using MobileApp.Pages;

namespace MobileApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    //The method to take a photo
    private async void TakePhoto(object sender, EventArgs e)
    {
        var status = await Permissions.RequestAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            await DisplayAlert("Permission Denied", "Camera permission is required", "OK");
            return;
        }

        try
        {
            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo != null)
            {
                await Navigation.PushAsync(new PreviewPage(photo));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error in take the photo: {ex.Message}", "OK");
        }
    }

    private async void LoginBioscope(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new LoginTest());
     
        
        

    }
    // private void OnCounterClicked(object sender, EventArgs e)
    // {
    //     _count++;
    //     CounterBtn.Text = $"Clicked {_count}";
    // }

    private async void OnGetWeatherClicked(object sender, EventArgs e)
    {
        try
        {
#if ANDROID
            var baseAddress = "http://10.0.2.2:5022/";  // my API's HTTP port
#else
            var baseAddress = "https://localhost:7022/"; // my API's HTTPS port on Windows
#endif
            using var client = new HttpClient { BaseAddress = new Uri(baseAddress) };

            var data = await client.GetFromJsonAsync<IEnumerable<WeatherForecast>>("weatherforecast");
            if (data != null)
            {
                ResultLabel.Text = string.Join(Environment.NewLine,
                    data.Select(w => $"{w.Date:d}: {w.TemperatureC}°C, {w.Summary}"));
            }
        }
        catch (Exception ex)
        {
            ResultLabel.Text = $"Error: {ex.Message}";
        }
    }
}


public class WeatherForecast
{
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
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
