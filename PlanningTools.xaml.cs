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
        // I do not know why this doesn't work

        // if(!Email.Default.IsComposeSupported){
        //     await DisplayAlert("Error creating email", "Composing an email is not supported", "OK");
        //     return;
        // }

        // string subject = "";
        // string body = "";
        // string[] recipients = new[] {"FlyWI@dot.wi.gov"};

        // EmailMessage message = new EmailMessage
        // {
        //     Subject = subject,
        //     Body = body,
        //     BodyFormat = EmailBodyFormat.PlainText,
        //     To = new List<string>(recipients)
        // };


        // await Email.Default.ComposeAsync(message);

        // And this does

        string email = "FlyWI@dot.wi.gov";
        string subject = "Request to Redeem a T-Shirt";
        string body = $"I have visited {MauiProgram.BusinessLogic.GetAirports().Count()} airports and would like a prize";
       
        try
        {
            string mailto = $"mailto:{email}?subject={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(body)}";
            await Launcher.OpenAsync(mailto);
        }
        catch (Exception ex)
        {
            DisplayAlert($"An error occurred", "{ex.Message}", "OK");
        }
        // Email FlyWI@dot.wi.gov
    }
}