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

        WisconsinAirport currentCoordinates = GetCurrentCoordinates();

        foreach (WisconsinAirport destinationAirport in allAirports)
        {
            WisconsinAirport? destinationAirportCoordinates = WisconsinAirports.FirstOrDefault(wisconsinAirport => wisconsinAirport!.Id == destinationAirport.Id, null);

            if (destinationAirportCoordinates != null)
            {
                // Haversine formula to find distance between two points
                double sourceLatitudeRadians = currentCoordinates.Latitude * (Math.PI / 180);
                double destinationLatitudeRadians = destinationAirportCoordinates.Latitude * (Math.PI / 180);
                double latitudeDiffRadians =
                    (destinationAirportCoordinates.Latitude - currentCoordinates.Latitude) * (Math.PI / 180);
                double longitudeDiffRadians =
                    (destinationAirportCoordinates.Longitude - currentCoordinates.Longitude) * (Math.PI / 180);
                double flatDistance = Math.Pow(Math.Sin(latitudeDiffRadians / 2.0), 2.0) +
                                      (Math.Cos(sourceLatitudeRadians) *
                                       Math.Cos(destinationLatitudeRadians) *
                                       Math.Pow(Math.Sin(longitudeDiffRadians / 2.0), 2.0));
                double angularDistance = 2 * Math.Atan2(Math.Sqrt(flatDistance), Math.Sqrt(1 - flatDistance));
                double distanceInMeters = EARTH_RADIUS_IN_METERS * angularDistance;
                double distanceInMiles = distanceInMeters * MILES_PER_METER;
                if (distanceInMiles < closestDistance)
                {
                    closestAirport = destinationAirport.Id;
                    closestDistance = distanceInMiles;
                }
            }
        }
        return closestAirport;
    }

    private WisconsinAirport GetCurrentCoordinates()
    {
        var currLocation = Geolocation.GetLastKnownLocationAsync().Result;
        float lat = (float)currLocation.Latitude;
        float lon = (float)currLocation.Longitude;
        if (currLocation == null)
        {
            return new WisconsinAirport("", "", 0f, 0f, "");
        }
        else
        {
            return new WisconsinAirport("", "", lat, lon, "");
        }
    }

}