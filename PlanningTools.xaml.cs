using Lab2_Solution.NearbyAirports;

namespace Lab6_Starter;

public partial class PlanningTools : ContentPage
{
	public PlanningTools()
	{
		InitializeComponent();
	}

    private async void OnNearbyAirportsClicked(object sender, EventArgs e)
    {
        // Navigate to the Nearby Airports page
        await Navigation.PushAsync(new NearbyAirportsPage());
    }
    private async void OnRoutingStrategiesClicked(object sender, EventArgs e)
    {
        // Navigate to the Routing Strategies page
        await Navigation.PushAsync(new RoutingStrategies());
    }

    private async void OnWeatherClicked(object sender, EventArgs e)
    {
        // Navigate to the Weather page
        await Navigation.PushAsync(new WeatherPage());
    }

    private async void OnTShirtClicked(object sender, EventArgs e)
    {
        // Navigate to the TShirts page
        //await Navigation.PushAsync(new WeatherPage());
    }
}