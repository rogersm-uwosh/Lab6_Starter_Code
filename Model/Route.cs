using Lab6_Starter.Model;
using System.Collections.ObjectModel;

namespace Lab6_Starter;

public class Route
{
    // Start and end nodes
    public RoutePoint Start => Edges.FirstOrDefault()?.From;
    public RoutePoint End => Edges.LastOrDefault()?.To;
    public double Length => Edges.Select(edge => edge.Distance).Aggregate((a, b) => a + b);

    // Collection of edges
    public ObservableCollection<RouteEdge> Edges { get; set; }

    // Constructor to initialize the Route with start, end, and edges
    public Route(RoutePoint start, RoutePoint end)
    {
        Edges = [ new(start, end) ];
    }

    // Method to add a point on the end
    public void AddPointOnEnd(RoutePoint point)
    {
        Edges.Add(new RouteEdge(End, point));
    }

    /// <summary>
    /// Uses a solution to the traveling salesman problem to generate a good route.
    /// </summary>
    /// <param name="points">The points to get to on the route, starts and ends with the first in the list.</param>
    /// <returns>A decent route that visits each point exactly once.</returns>
    public static Route GenerateTravelingSalesmanRoute(List<RoutePoint> points) {
        return null;
    }
}

public class RouteEdge
{
    public RoutePoint From { get; set; }
    public RoutePoint To { get; set; }
    public double Distance => To.DistanceFrom(From);

    public RouteEdge(RoutePoint from, RoutePoint to)
    {
        From = from;
        To = to;
    }
}

public class RoutePoint
{
    private static double XScale = 1.0;
    private static double YScale = 1.0;
    public double X { get; set; }
    public double Y { get; set; }
    // Due to things like degrees not really converting to miles and some other painful mumbo jumbo,
    // we need to scale these to calculate distance. Basically this converts the meter-like value
    // into miles (kind of)
    public double XScaled => X * XScale;
    public double YScaled => Y * YScale;
    public Airport Airport { get; set; }

    public RoutePoint(double x, double y, Airport airport) {
        X = x;
        Y = y;
        Airport = airport;
    }

    public double DistanceFrom(RoutePoint other) {
        double diffX = other.XScaled - XScaled;
        double diffY = other.YScaled - YScaled;
        return Math.Sqrt(diffX * diffX + diffY * diffY);
    }
}
