public class Route
{
    // Properties for ICAO, City, and Distance
    public string ICAO { get; set; }
    public string City { get; set; }
    public int Distance { get; set; }

    // Constructor to initialize a Route object
    public Route(string icao, string city, int distance)
    {
        ICAO = icao;
        City = city;
        Distance = distance;
    }
}
