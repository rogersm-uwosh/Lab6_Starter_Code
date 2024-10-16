using System.Collections.ObjectModel;
using Lab2_Solution.Model;

namespace Lab2_Solution.NearbyAirports;

/// <summary>
/// Written by Hiba Seraj
/// Alexander Johnston forked and cloned the project, and added the navigation.
/// A page that displays nearby airports.
/// </summary>
public partial class NearbyAirportsPage : ContentPage
{
    public ObservableCollection<NearbyAirport> NearbyAirports { get; } = [];

    public NearbyAirportsPage()
    {
        InitializeComponent();
        BindingContext = this;
        NearbyAirports.Add(new NearbyAirport("KFLD", "Fond du Lac", 10, true));
        NearbyAirports.Add(new NearbyAirport("KMTW", "Manitowac", 15,true));
        NearbyAirports.Add(new NearbyAirport("79C", "Brenner", 18,false));
        NearbyAirports.Add(new NearbyAirport("KUNU", "Dodge County", 64,true));
    }
    
}


