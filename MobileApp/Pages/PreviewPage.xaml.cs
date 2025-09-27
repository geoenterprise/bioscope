using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;

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
