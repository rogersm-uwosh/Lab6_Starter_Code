using Mapsui;
using Mapsui.Tiling;
using Lab6_Starter.Model;
using Mapsui.Projections;

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

            Mapsui.Map map = new();

            // Initialize the map with OpenStreetMap's API and add it to MapPage's content
            map.Layers.Add(OpenStreetMap.CreateTileLayer());
            map.Home = ZoomToWisconsin;

            RouteMap.Map = map;
        }

        private void ZoomToWisconsin(Navigator n) {
            ZoomMapTo(n, 41.7, -93.1, 47.3, -86); // Wisconsin coordinates
        }

        private static void ZoomMapTo(Navigator n, double latitudeMin, double longitudeMin, double latitudeMax, double longitudeMax)
        {
            (double minX, double minY) = SphericalMercator.FromLonLat(longitudeMin, latitudeMin);
            (double maxX, double maxY) = SphericalMercator.FromLonLat(longitudeMax, latitudeMax);

            n.ZoomToBox(new MRect(minX, minY, maxX, maxY), MBoxFit.Fit);
        }
    }
}
