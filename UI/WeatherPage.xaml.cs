using Lab6_Starter.Model;
namespace Lab6_Starter;

public partial class WeatherPage : ContentPage
{
    public WeatherPage()
    {
        InitializeComponent();

        // We've set the BindingContext for the entire page to be the BusinessLogic layer
        // So any control on the page can bind to the BusinessLogic layer
        // There's really only one control that needs to talk to the BusinessLogic layer, and that's the CollectionView

        BindingContext = MauiProgram.BusinessLogic;
    }

    // Various event handlers for the buttons on the main page

    void Fetch_Clicked(object sender, EventArgs e)
    {
        MauiProgram.BusinessLogic.GetClosestAirportWeather();
    }

}


