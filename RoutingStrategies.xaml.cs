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

        public Route CurrentRoute { get; set; }

        public RoutingStrategies()
        {
            InitializeComponent();

            // Get the route from the business logic
            CurrentRoute = _businessLogic.GetRoute();

            // Set the BindingContext for data binding
            BindingContext = this;

            // Load OpenStreetMap directly into the WebView
            var mapUrl = GenerateOpenStreetMapUrl(44.2619, -88.4154, 10); // Appleton Airport coordinates
            MapView.Source = mapUrl;
        }

        private string GenerateOpenStreetMapUrl(double latitude, double longitude, int zoom)
        {
            return $"https://www.openstreetmap.org/?mlat={latitude}&mlon={longitude}#map={zoom}/{latitude}/{longitude}";
        }
    }
}
