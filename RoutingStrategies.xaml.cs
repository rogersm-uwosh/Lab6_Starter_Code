using Mapsui.UI.Maui;
using Mapsui.Layers;
using Mapsui.Tiling;
using Mapsui.Utilities;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using Mapsui.Projections;
using Lab6_Starter.Model;


namespace Lab6_Starter
{
    public partial class RoutingStrategies : ContentPage
    {
        private IBusinessLogic _businessLogic = MauiProgram.BusinessLogic;

        public ObservableCollection<Route> Routes { get; set; }


        public RoutingStrategies()
        {
            InitializeComponent();

            // Get routes from the business logic
            Routes = _businessLogic.GetRoutes();

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

}


