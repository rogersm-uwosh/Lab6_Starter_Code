using System.Collections.ObjectModel;
using Lab6_Starter;
using Lab6_Starter.Model;

namespace FWAPPA.NearbyAirports;

/// <summary>
/// Written by Hiba Seraj
/// Alexander Johnston forked and cloned the project, and added the navigation.
/// A page that displays nearby airports.
/// </summary>
public partial class NearbyAirportsPage : ContentPage
{
    IBusinessLogic BusinessLogic = MauiProgram.BusinessLogic;
    public ObservableCollection<Airport> NearbyAirports { get; } = [];

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
    private void OnSearchNearbyAirportBtn(object sender, EventArgs e)
    {
        string airportName = AirportEntry.Text.ToUpper();
        string distanceMileText = DistanceEntry.Text;
        
        Airport airport = BusinessLogic.FindAirport(airportName);
        bool isValidDistance = int.TryParse(distanceMileText, out int distanceMile);

        if (airport == null)
        {
            DisplayAlert("Error", "Airport not found", "OK");
        }
        else if (!isValidDistance)
        {
            DisplayAlert("Error", "Distance is invalid", "OK");
        }
        else if (distanceMile < 0)
        {
            DisplayAlert("Error", "Distance must be greater than 0", "OK");
        }
        else
        {
            NearbyAirports.Clear();
            foreach (var nearbyAirport in BusinessLogic.CalculateNearbyAirports(airport, distanceMile))
            {
                NearbyAirports.Add(nearbyAirport);
            }
        }
    }
}