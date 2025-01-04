using System.Collections.ObjectModel;
using Lab6_Starter.Model;

namespace Lab6_Starter;

/// <summary>
/// Written by Hiba Seraj
/// Alexander Johnston forked and cloned the project, and added the navigation.
/// A page that displays nearby airports.
/// </summary>
public partial class NearbyAirportsPage : ContentPage
{
    IBusinessLogic BusinessLogic = MauiProgram.BusinessLogic;
    public ObservableCollection<WisconsinAirport> NearbyAirports { get; } = [];

    public NearbyAirportsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    /// <summary>
    /// Update Nearby airport when the user click on the button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnSearchNearbyAirportBtn(object sender, EventArgs e)
    {
        string airportName = AirportEntry.Text;
        string distanceMileText = DistanceEntry.Text;

        if (airportName == null)
        {
            await DisplayAlert("", "Please enter a valid airport name", "OK");
            return;
        }

        WisconsinAirport airport = BusinessLogic.SelectAirportByCode(airportName);
        bool isValidDistance = int.TryParse(distanceMileText, out int distanceMile);
        if (airport == null)
        {
            await DisplayAlert("Error", "Airport not found", "OK");
            return;
        }

        if (!isValidDistance)
        {
            await DisplayAlert("Error", "Distance is invalid", "OK");
            return;
        }

        if (distanceMile < 0)
        {
            await DisplayAlert("Error", "Distance must be greater than 0", "OK");
            return;
        }

        NearbyAirports.Clear();
        foreach (var nearbyAirport in BusinessLogic.CalculateNearbyAirports(airport, distanceMile))
        {
            NearbyAirports.Add(nearbyAirport);
        }

    }
}