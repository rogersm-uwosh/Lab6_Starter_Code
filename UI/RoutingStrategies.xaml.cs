using Mapsui;
using Mapsui.Tiling;
using Lab6_Starter.Model;
using Mapsui.Projections;
using Mapsui.Layers;
using Mapsui.Nts;
using NetTopologySuite.Geometries;
using Mapsui.Styles;
using Color = Mapsui.Styles.Color;
using System.Collections.ObjectModel;

namespace Lab6_Starter {
    public partial class RoutingStrategies : ContentPage {
        private static readonly IStyle pointStyle = new SymbolStyle {
            SymbolScale = 0.5d,
            Fill = new(Color.Orange)
        };

        private static readonly IStyle startPointStyle = new SymbolStyle {
            SymbolScale = 0.5d,
            Fill = new(Color.Green)
        };

        private static readonly IStyle routeStyle = new VectorStyle() {
            Line = new(Color.Black, 2)
        };

        private IBusinessLogic _businessLogic = MauiProgram.BusinessLogic;

        private ILayer routeLayer;

        public Route CurrentRoute {
            get {
                return currentRoute;
            }
            set {
                if (currentRoute != value) {
                    currentRoute = value;
                    OnPropertyChanged();
                }
            }
        }
        private Route currentRoute;

        public RoutingStrategies() {
            InitializeComponent();

            // Get the route from the business logic
            CurrentRoute = null;

            // Set the BindingContext for data binding
            BindingContext = this;

            Mapsui.Map map = new();

            // Initialize the map with OpenStreetMap's API and add it to MapPage's content
            map.Layers.Add(OpenStreetMap.CreateTileLayer());
            map.Home = ZoomToWisconsin;

            RouteMap.Map = map;

            // Update current route (shouldn't do anything on startup)
            UpdateRoute(CurrentRoute);
        }

        private void GenerateRoute(object sender, EventArgs e) {
            // Get the airport (not necessarily visited)
            string airportId = StartingAirportPicker.Text;
            Collection<WisconsinAirport> available = _businessLogic.GetWisconsinAirports();
            WisconsinAirport start = available.Where(x => x.Id.Equals(airportId)).FirstOrDefault();
            if (start == null) {
                // Clears the map
                UpdateRoute(null);
                return;
            }

            // Get the distance
            string distanceText = MaxDistanceEntry.Text;
            int distance = string.IsNullOrEmpty(distanceText) ? 0 : int.Parse(distanceText);

            // Get if it should only find unvisited airports
            bool unvisitedOnly = UnvisitedSwitch.IsToggled;

            // Update the route to be the one generated based on this information
            UpdateRoute(_businessLogic.GetRoute(start, distance, unvisitedOnly));
        }

        private void UpdateRoute(Route route) {
            // Remove current layer
            if (routeLayer != null) {
                RouteMap.Map.Layers.Remove(routeLayer);
            }

            // If the route is null, stop, otherwise set this to the current route.
            // We must not let it be null, however, for the sake of the UI and null
            // such and such
            CurrentRoute = route ?? new();
            if (route == null) {
                return;
            }

            // Convert points to coordinates
            List<Coordinate> points = route.Points.Select(point => {
                (double x, double y) = SphericalMercator.FromLonLat(point.X, point.Y);
                return new Coordinate(x, y);
            }).ToList();

            // Create path polygon
            GeometryFeature polygonFeature = new() {
                Geometry = new LineString([.. points])
            };
            polygonFeature.Styles.Add(routeStyle);

            // Make the points on the map, with a different style if it is
            // the first point
            var pointFeatures = points
                .Take(points.Count - 1)
                .Select((point, i) => new PointFeature(point.X, point.Y) {
                    Styles = [i == 0 ? startPointStyle : pointStyle]
                });

            // Put the points on the map
            routeLayer = new MemoryLayer() {
                Features = [polygonFeature, .. pointFeatures],
                Style = null
            };
            RouteMap.Map.Layers.Add(routeLayer);
        }

        private static void ZoomToWisconsin(Navigator n)
        {
            ZoomMapTo(n, 41.7, -93.1, 47.3, -86); // Wisconsin bounding box coordinates
        }

        private static void ZoomMapTo(Navigator n, double latitudeMin, double longitudeMin, double latitudeMax, double longitudeMax)
        {
            (double minX, double minY) = SphericalMercator.FromLonLat(longitudeMin, latitudeMin);
            (double maxX, double maxY) = SphericalMercator.FromLonLat(longitudeMax, latitudeMax);

            n.ZoomToBox(new MRect(minX, minY, maxX, maxY), MBoxFit.Fit);
        }
    }
}
