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

        private static readonly IStyle visitedPointStyle = new SymbolStyle {
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

            UpdateRoute(CurrentRoute);
        }

        private void GenerateRoute(object sender, EventArgs e) {
            string airportId = StartingAirportPicker.Text;
            Collection<Airport> available = _businessLogic.GetWisconsinAirports();
            Airport start = available.Where(x => x.Id.Equals(airportId)).FirstOrDefault();
            if (start == null) {
                UpdateRoute(null);
                return;
            }
            string distanceText = MaxDistanceEntry.Text;
            int distance = string.IsNullOrEmpty(distanceText) ? 0 : int.Parse(distanceText);
            bool unvisitedOnly = UnvisitedSwitch.IsToggled;

            UpdateRoute(_businessLogic.GetRoute(start, distance, unvisitedOnly));
        }

        private void UpdateRoute(Route route) {
            if (routeLayer != null) {
                RouteMap.Map.Layers.Remove(routeLayer);
            }

            CurrentRoute = route ?? new();
            if (route == null) {
                return;
            }
            List<Coordinate> points = route.Points.Select(point => {
                (double x, double y) = SphericalMercator.FromLonLat(point.X, point.Y);
                return new Coordinate(x, y);
            }).ToList();

            GeometryFeature polygonFeature = new() {
                Geometry = new LineString([.. points])
            };
            polygonFeature.Styles.Add(routeStyle);

            var pointFeatures = points
                .Take(points.Count - 1)
                .Select((point, i) => new PointFeature(point.X, point.Y) {
                    Styles = [i == 0 ? visitedPointStyle : pointStyle]
                });

            routeLayer = new MemoryLayer() {
                Features = [polygonFeature, .. pointFeatures],
                Style = null
            };

            RouteMap.Map.Layers.Add(routeLayer);
        }

        private static void ZoomToWisconsin(Navigator n)
        {
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
