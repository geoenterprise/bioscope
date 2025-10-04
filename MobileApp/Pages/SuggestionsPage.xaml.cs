using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using MobileApp.Models;
using System.Text.Json.Serialization;



namespace MobileApp;

public partial class SuggestionsPage : ContentPage
{
    
    public SuggestionsPage(string json)
    {
        InitializeComponent();

        try
        {
            var root = JsonDocument.Parse(json).RootElement;
            var result = root.GetProperty("result");
            var suggestionsJson = result.GetProperty("suggestions").GetRawText();
            var suggestions = JsonSerializer.Deserialize<List<Suggestion>>(suggestionsJson);

            
            if (suggestions != null)
            {
                SuggestionsCollection.ItemsSource = suggestions;
            }
            
            else
            {
                WarningLabel.Text = "We did not find any suggestion.";
              

            }



            // foreach (var suggestion in suggestions)
            // {
            //     var name = suggestion.GetProperty("plant_name").GetString();
            //     var prob = suggestion.GetProperty("probability").GetDouble();
            //     Console.WriteLine($"{name} - {prob}");
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
