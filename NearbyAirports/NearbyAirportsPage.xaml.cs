using System.Collections.ObjectModel;
using Lab6_Starter.Model;

namespace Lab2_Solution.NearbyAirports;

/// <summary>
/// Written by Hiba Seraj
/// Alexander Johnston forked and cloned the project, and added the navigation.
/// A page that displays nearby airports.
/// </summary>
public partial class NearbyAirportsPage : ContentPage
{
    public ObservableCollection<Airport> NearbyAirports { get; } = [];

    public NearbyAirportsPage()
    {
        InitializeComponent();
        BindingContext = this;
        NearbyAirports.Add(new Airport("KFLD", "Fond du Lac", DateTime.Now, 1));
        NearbyAirports.Add(new Airport("KMTW", "Manitowac", DateTime.Now, 1));
        NearbyAirports.Add(new Airport("79C", "Brenner", DateTime.Now, 5));
        NearbyAirports.Add(new Airport("KUNU", "Dodge County", DateTime.Now, 1));
    }
    
}


