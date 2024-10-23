using Mapsui.UI.Maui;
using Mapsui.Layers;
using Mapsui.Tiling;
using Mapsui.Utilities;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using Mapsui.Projections;


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
                new Route { ICAO = "KATW", City = "Appleton", Distance = 0 },
                new Route { ICAO = "KFLD", City = "Los Angeles", Distance = 28 },
                new Route { ICAO = "KUNN", City = "Dodge County", Distance = 23 },
                new Route { ICAO = "KUBB", City = "Burlington", Distance = 47 },
                new Route { ICAO = "KATW", City = "Appleton", Distance = 95 }
            };

            // Binding the CollectionView to the data source
            RoutesCollectionView.ItemsSource = Routes;


            // Load OpenStreetMap directly into the WebView without requiring an HTML file
            var mapUrl = GenerateOpenStreetMapUrl(44.2619, -88.4154, 10); // Appleton Airport coordinates
            MapView.Source = mapUrl;
        }

        // Generates the OpenStreetMap URL for the given latitude, longitude, and zoom level
        private string GenerateOpenStreetMapUrl(double latitude, double longitude, int zoom)
        {
            return $"https://www.openstreetmap.org/?mlat={latitude}&mlon={longitude}#map={zoom}/{latitude}/{longitude}";
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
