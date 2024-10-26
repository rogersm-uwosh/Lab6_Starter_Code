using System.Collections.ObjectModel;

public class Route
{
    // Start and end nodes
    public string Start { get; set; }
    public string End { get; set; }

    // Collection of edges
    public ObservableCollection<RouteEdge> Edges { get; set; }

    // Constructor to initialize the Route with start, end, and edges
    public Route(string start, string end)
    {
        Start = start;
        End = end;
        Edges = new ObservableCollection<RouteEdge>();
    }

    // Method to add an edge to the route
    public void AddEdge(string from, string to, int distance)
    {
        Edges.Add(new RouteEdge(from, to, distance));
    }
}

public class RouteEdge
{
    public string From { get; set; }
    public string To { get; set; }
    public int Distance { get; set; }

    public RouteEdge(string from, string to, int distance)
    {
        From = from;
        To = to;
        Distance = distance;
    }
}
