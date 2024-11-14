using Mapsui.UI.Maui;
using Mapsui.Layers;
using Mapsui.Tiling;
using Mapsui.Utilities;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Linq;
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
            WisconsinAirports = new ObservableCollection<Airport>(_businessLogic.GetAllWisconsinAirports());
            BindingContext = this;

            // Set up the map view with initial location (Appleton Airport coordinates)
            var mapUrl = GenerateOpenStreetMapUrl(44.2619, -88.4154, 10);
            MapView.Source = mapUrl;
        }

        private void OnMaxDistanceChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(MaxDistanceEntry.Text, out double maxDistanceKm))
            {
                var startingAirportCode = StartingAirportPicker.Text;
                if (!string.IsNullOrWhiteSpace(startingAirportCode))
                {
                    var startingAirport = _businessLogic.SelectAirportByCode(startingAirportCode);
                    if (startingAirport != null)
                    {
                        DisplayNearbyAirports(startingAirport.Latitude, startingAirport.Longitude, maxDistanceKm);
                    }
                    else
                    {
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

        private void OnUnvisitedSwitchToggled(object sender, ToggledEventArgs e)
        {
            // Get the starting airport code and max distance from the UI
            var startingAirportCode = StartingAirportPicker.Text;

            if (double.TryParse(MaxDistanceEntry.Text, out double maxDistanceKm) && !string.IsNullOrWhiteSpace(startingAirportCode))
            {
                // Get the starting airport details
                var startingAirport = _businessLogic.SelectAirportByCode(startingAirportCode);
                if (startingAirport == null)
                {
                    DisplayAlert("Error", "Starting airport not found in the database.", "OK");
                    return;
                }

                // Get the list of all airports and visited airports
                ObservableCollection<Airport> visitedAirports = _businessLogic.GetAirports();
                ObservableCollection<Airport> allAirports = _businessLogic.GetWisconsinAirportsWithinDistance(startingAirport.Latitude, startingAirport.Longitude, maxDistanceKm);

                // Filter airports based on whether they are visited and toggle status
                var filteredAirports = new ObservableCollection<Airport>();
                foreach (var airport in allAirports)
                {
                    bool isVisited = visitedAirports.Any(visited => visited.Id == airport.Id);

                    // Add to filtered list based on toggle status
                    if (UnvisitedSwitch.IsToggled && !isVisited)
                    {
                        filteredAirports.Add(airport); // Only unvisited airports
                    }
                    else if (!UnvisitedSwitch.IsToggled)
                    {
                        filteredAirports.Add(airport); // All nearby airports
                    }
                }

                // Update the WisconsinAirports collection to reflect the filtered results
                WisconsinAirports.Clear();
                foreach (var airport in filteredAirports.OrderBy(a => a.Distance))
                {
                    WisconsinAirports.Add(airport);
                }

                // Notify the UI of the change
                OnPropertyChanged(nameof(WisconsinAirports));
            }
            else
            {
                DisplayAlert("Invalid Input", "Please enter a valid starting airport code and distance.", "OK");
            }
        }

        public void DisplayNearbyAirports(double userLatitude, double userLongitude, double maxDistanceNm)
        {
            if (maxDistanceNm <= 0)
            {
                DisplayAlert("Invalid Distance", "Please enter a positive number for distance.", "OK");
                return;
            }

            // Fetch nearby airports within the specified distance
            var nearbyAirports = _businessLogic.GetWisconsinAirportsWithinDistance(userLatitude, userLongitude, maxDistanceNm);

            // Clear and update the WisconsinAirports collection
            WisconsinAirports.Clear();
            foreach (var airport in nearbyAirports.OrderBy(a => a.Distance))
            {
                WisconsinAirports.Add(airport);
            }

            // Notify the UI of the update
            OnPropertyChanged(nameof(WisconsinAirports));
        }

        private string GenerateOpenStreetMapUrl(double latitude, double longitude, int zoom)
        {
            return $"https://www.openstreetmap.org/?mlat={latitude}&mlon={longitude}#map={zoom}/{latitude}/{longitude}";
        }
    }
}
