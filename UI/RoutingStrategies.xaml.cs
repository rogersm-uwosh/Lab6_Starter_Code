using CommunityToolkit.Maui.Core.Platform;
using FWAPPA.Model;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling;
using NetTopologySuite.Geometries;
using Brush = Mapsui.Styles.Brush;
using Color = Mapsui.Styles.Color;

namespace FWAPPA.UI;

public partial class RoutingStrategies : ContentPage
{
    private static readonly IStyle PointStyle = new SymbolStyle
    {
        SymbolScale = 0.5d,
        Fill = new Brush(Color.Orange)
    };

    private static readonly IStyle StartPointStyle = new SymbolStyle
    {
        SymbolScale = 0.5d,
        Fill = new Brush(Color.Green)
    };

    private static readonly IStyle RouteStyle = new VectorStyle
    {
        Line = new Pen(Color.Black, 2)
    };

    private readonly IBusinessLogic businessLogic = MauiProgram.BusinessLogic;

    private ILayer? mapRouteLayer;

    public RoutingStrategies()
    {
        InitializeComponent();

        // Set the BindingContext for data binding
        BindingContext = businessLogic;

        // Initialize the map with OpenStreetMap's API and add it to MapPage's content
        Mapsui.Map map = new();
        map.Layers.Add(OpenStreetMap.CreateTileLayer());
        MRect wisconsinBox = new MRect(
            SphericalMercator.FromLonLat(-93.1, 41.7).x, 
            SphericalMercator.FromLonLat(-93.1, 41.7).y, 
            SphericalMercator.FromLonLat(-86, 47.3).x, 
            SphericalMercator.FromLonLat(-86, 47.3).y
        );
        map.Navigator.ZoomToBox(wisconsinBox);
        RouteMap.Map = map;

        // Update current route (shouldn't do anything on startup)
        UpdateRoute(null);
    }

    private async void GenerateRoute(object sender, EventArgs e)
    {
        StartingAirportEntry.HideKeyboardAsync(CancellationToken.None);
        MaxDistanceEntry.HideKeyboardAsync(CancellationToken.None);
        
        // Get the airport (not necessarily visited)
        string airportId = StartingAirportEntry.Text;
        if (airportId == null)
        {
            await DisplayAlert("", "Please enter a valid airport id", "OK");
            return;
        }
        
        WisconsinAirport start = businessLogic.SelectAirportByCode(airportId);
        if (start == null)
        {
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
        UpdateRoute(businessLogic.GetRoute(start, distance, unvisitedOnly));
    }

    private void UpdateRoute(Route? route)
    {
        // Remove current layer
        if (mapRouteLayer != null)
        {
            RouteMap.Map.Layers.Remove(mapRouteLayer);
        }

        // If the route is null, stop, otherwise set this to the current route.
        if (route == null)
        {
            RefreshMapRoute();
            return;
        }
        businessLogic.CurrentRoute = route;

        // Convert points to coordinates
        List<Coordinate> points = route.Points.Select(point =>
        {
            (double x, double y) = SphericalMercator.FromLonLat(point.X, point.Y);
            return new Coordinate(x, y);
        }).ToList();

        // Create path polygon
        GeometryFeature polygonFeature = new()
        {
            Geometry = new LineString([.. points])
        };
        polygonFeature.Styles.Add(RouteStyle);

        // Make the points on the map, with a different style if it is
        // the first point
        var pointFeatures = points
            .Take(points.Count - 1)
            .Select((point, i) => new PointFeature(point.X, point.Y)
            {
                Styles = [i == 0 ? StartPointStyle : PointStyle]
            });

        // Put the points on the map
        mapRouteLayer = new MemoryLayer
        {
            Features = [polygonFeature, .. pointFeatures],
            Style = null
        };
        RouteMap.Map.Layers.Add(mapRouteLayer);
        RefreshMapRoute();
    }

    private void RefreshMapRoute()
    {
        RoutesCollectionView.ItemsSource = null;
        RoutesCollectionView.ItemsSource = businessLogic.CurrentRoute.Edges;
    }

    private static void ZoomToWisconsin(Navigator n)
    {
        ZoomMapTo(n, 41.7, -93.1, 47.3, -86); // Wisconsin bounding box coordinates
    }

    private static void ZoomMapTo(Navigator n, double latitudeMin, double longitudeMin, double latitudeMax,
        double longitudeMax)
    {
        (double minX, double minY) = SphericalMercator.FromLonLat(longitudeMin, latitudeMin);
        (double maxX, double maxY) = SphericalMercator.FromLonLat(longitudeMax, latitudeMax);

        n.ZoomToBox(new MRect(minX, minY, maxX, maxY), MBoxFit.Fit);
    }
}