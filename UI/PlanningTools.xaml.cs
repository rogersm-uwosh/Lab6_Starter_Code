namespace FWAPPA.UI;

public partial class PlanningTools : ContentPage
{
    public const string FLY_WI_EMAIL_ADDRESS = "FlyWI@dot.wi.gov";
    
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
        // use the email API to send an email with the number of airports visited
        if (!Email.Default.IsComposeSupported)
        {
            await DisplayAlert("Error creating email", "Composing an email is not supported", "OK");
            return;
        }

        var visitedAirports = await MauiProgram.BusinessLogic.GetVisitedAirports(); 
        string subject = "Request to Redeem a T-Shirt";
        string body = $"I have visited {visitedAirports.Count} airports and would like a prize";
        string[] recipients = [ FLY_WI_EMAIL_ADDRESS ];

        EmailMessage message = new()
        {
            Subject = subject,
            Body = body,
            BodyFormat = EmailBodyFormat.PlainText,
            To = new List<string>(recipients)
        };

        await Email.Default.ComposeAsync(message);
    }

        void Logout_Clicked(System.Object sender, System.EventArgs e)
    {
        MauiProgram.BusinessLogic.VisitedAirports.Clear(); // otherwise, when logging in again, 
        Application.Current!.MainPage = new LoginPage();
        // await Navigation.PopAsync();
    }
}