using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json; 


namespace MobileApp;

public partial class SuggestionsPage : ContentPage
{
    public SuggestionsPage(string json)
    {
        InitializeComponent();

        // Show raw JSON in the label
        JsonLabel.Text = json;
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
