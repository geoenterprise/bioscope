using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using MobileApp.Models;
using System.Text.Json.Serialization;



namespace MobileApp;

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





    
    
    

}







