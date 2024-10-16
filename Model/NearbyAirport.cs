namespace Lab2_Solution.Model;

/// <summary>
/// Written by Hiba Seraj
/// Represents nearby airports.
/// </summary>
/// <param name="id"></param>
/// <param name="city"></param>
/// <param name="miles"></param>
/// <param name="isVisited"></param>
public class NearbyAirport(string id, string city, int miles, bool isVisited)
{
    public string Id { get;  } = id;
    public int Miles { get;  } = miles;
    public string City { get; } = city;
    public bool IsVisited { get; } = isVisited;
    
}