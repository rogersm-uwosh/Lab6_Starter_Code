using System.Collections.ObjectModel;

//using CommunityToolkit.Maui.Core.Extensions;

namespace FWAPPA.Model;

public partial class BusinessLogic : IBusinessLogic
{
    // private ObservableCollection<AirportCoordinates> airportCoordinates;

    // partial void LoadAirportCoordinates()
    // {
    //     airportCoordinates = db.SelectAllAirportCoordinates();
    // }


    public Weather ClosestAirportWeather
    {
        get { return GetClosestAirportWeather(); }
    }

    public Weather GetClosestAirportWeather()
    {
        string airport = "";
        airport = FindClosestAirport();
        HttpClient aviationWeatherCenter = new HttpClient();
        try
        {
            var metarUrl = "https://aviationweather.gov/api/data/metar?ids=" + airport + "&format=raw";
            var metar = aviationWeatherCenter.GetStringAsync(metarUrl).Result;
            var tafUrl = "https://aviationweather.gov/api/data/taf?ids=" + airport + "&format=raw";
            var taf = aviationWeatherCenter.GetStringAsync(tafUrl).Result;
            return new Weather(airport, metar, taf);
        }
        catch (Exception ex)
        {
            return new Weather("Catch", ex.Message, "Catch");
        }
    }

    /// <summary>
    /// Get the name of the closest airport (Shout out to the Nearby Airports Group since a lot of this logic uses code that they made)
    /// </summary>
    /// <returns>The name of the closest airport</returns>
    private string FindClosestAirport()
    {
        string closestAirport = "";
        double closestDistance = double.MaxValue;

        ObservableCollection<WisconsinAirport> allAirports = GetWisconsinAirports();
        Coordinates currentCoordinates = GetCurrentCoordinates();
        
        foreach (WisconsinAirport destinationAirport in allAirports)
        {
            double distanceInMiles = GetDistanceBetweenCoordinates(
                currentCoordinates,
                destinationAirport.Coordinates
            );
            if (distanceInMiles < closestDistance)
            {
                closestAirport = destinationAirport.Id;
                closestDistance = distanceInMiles;
            }
        }

        return closestAirport;
    }

    private static Coordinates GetCurrentCoordinates()
    {
        var currLocation = Geolocation.GetLastKnownLocationAsync().Result;
        if (currLocation == null)
        {
            return new Coordinates(0f, 0f);
        }

        float lat = (float)currLocation.Latitude;
        float lon = (float)currLocation.Longitude;
        return new Coordinates(lat, lon);
    }
}