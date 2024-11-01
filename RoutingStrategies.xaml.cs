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
        public ObservableCollection<Airport> WisconsinAirports { get; set; }

        public RoutingStrategies()
        {
            InitializeComponent();
            WisconsinAirports = _businessLogic.GetAllWisconsinAirports();
            BindingContext = this;

            var mapUrl = GenerateOpenStreetMapUrl(44.2619, -88.4154, 10); // Appleton Airport coordinates
            MapView.Source = mapUrl;
        }

        private void OnMaxDistanceChanged(object sender, TextChangedEventArgs e)
        {
            // Attempt to parse the entered text as a double for max distance
            if (double.TryParse(MaxDistanceEntry.Text, out double maxDistanceKm))
            {
                var startingAirportCode = StartingAirportPicker.Text;

                // Check if the starting airport code is provided
                if (!string.IsNullOrWhiteSpace(startingAirportCode))
                {
                    // Check if the starting airport exists in the database
                    var startingAirport = _businessLogic.SelectAirportByCode(startingAirportCode);

                    if (startingAirport != null)
                    {
                        // Starting airport found; proceed with displaying nearby airports
                        DisplayNearbyAirports(startingAirport.Latitude, startingAirport.Longitude, maxDistanceKm);
                    }
                    else
                    {
                        // Starting airport not found in the database; show an error
                        DisplayAlert("Error", "Starting airport not found in the database.", "OK");
                    }
                }
                else
                {
                    DisplayAlert("Error", "Please enter a valid starting airport code.", "OK");
                }
            }
            else if (!string.IsNullOrEmpty(MaxDistanceEntry.Text))
            {
                DisplayAlert("Invalid Input", "Please enter a valid number for max distance.", "OK");
            }
        }

        public void DisplayNearbyAirports(double userLatitude, double userLongitude, double maxDistanceKm)
        {
            if (maxDistanceKm <= 0)
            {
                DisplayAlert("Invalid Distance", "Please enter a positive number for distance.", "OK");
                return;
            }

            // Get the list of airports within the specified distance
            var unsortedAirports = _businessLogic.GetWisconsinAirportsWithinDistance(userLatitude, userLongitude, maxDistanceKm);

            // Sort airports by distance
            WisconsinAirports = new ObservableCollection<Airport>(unsortedAirports.OrderBy(airport => airport.Distance));

            // Notify the UI of changes
            OnPropertyChanged(nameof(WisconsinAirports));
        }

        private string GenerateOpenStreetMapUrl(double latitude, double longitude, int zoom)
        {
            return $"https://www.openstreetmap.org/?mlat={latitude}&mlon={longitude}#map={zoom}/{latitude}/{longitude}";
        }
    }
}
