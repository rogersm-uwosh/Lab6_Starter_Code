using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Map = Microsoft.Maui.Controls.Maps.Map;


namespace Lab6_Starter
{
    public partial class RoutingStrategies : ContentPage
    {
         public ObservableCollection<Route> Routes { get; set; }

        public RoutingStrategies()
        {
            InitializeComponent();

            // Fill data for ListView (Hannah)
            // Example data to bind
            Routes = new ObservableCollection<Route>
            {
                new Route { ICAO = "KATL", City = "Atlanta", Distance = 100 },
                new Route { ICAO = "KLAX", City = "Los Angeles", Distance = 200 },
                new Route { ICAO = "KJFK", City = "New York", Distance = 300 }
            };

            // Binding the CollectionView to the data source
            RoutesCollectionView.ItemsSource = Routes;

            // Set initial map location (Pachia)
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(44.2619, 88.4154), Distance.FromMiles(10)));
            //Content = map;
        }
    }

    // Get airport data (Hannah)
    public class Route
    {
        public string ICAO { get; set; }
        public string City { get; set; }
        public int Distance { get; set; }
    }
}
