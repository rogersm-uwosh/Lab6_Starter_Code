using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using FWAPPA.NearbyAirports;
using Lab6_Starter.Model;
using Microsoft.Maui.Devices.Sensors;

namespace FWAPPA.Model;

public partial class BusinessLogic : IBusinessLogic
{
    
    const int BRONZE_LEVEL = 42;
    const int SILVER_LEVEL = 84;
    const int GOLD_LEVEL = 128;
    
    IDatabase db;
    private readonly int MAX_RATING = 5;

    public ObservableCollection<Airport> Airports
    {
        get { return GetAirports(); }

    }

    public Weather ClosestAirportWeather
    {
        get { return GetClosestAirportWeather(); }
    }
    
    partial void LoadAirportCoordinates();

    public BusinessLogic(IDatabase? db)
    {
        this.db = db;
        LoadAirportCoordinates();
    }
    

    public Airport FindAirport(String id)
    {
        return db.SelectAirport(id);
    }

    private AirportAdditionError CheckAirportFields(String? id, String? city, DateTime? dateVisited, int rating)
    {
        
        if (id == null || id.Length < 3 || id.Length > 4)
        {
            return AirportAdditionError.InvalidIdLength;
        }
        if (city == null || city.Length < 3)
        {
            return AirportAdditionError.InvalidCityLength;
        }
        if (rating < 1 || rating > MAX_RATING)
        {
            return AirportAdditionError.InvalidRating;
        }

        if (dateVisited == null)
        {
            return AirportAdditionError.InvalidDate;
        }

        return AirportAdditionError.NoError;
    }


    public AirportAdditionError AddAirport(String id, String city, DateTime? dateVisited, int rating)
    {

        var result = CheckAirportFields(id, city, dateVisited, rating);
        if (result != AirportAdditionError.NoError)
        {
            return result;
        }

        if (db.SelectAirport(id) != null)
        {
            return AirportAdditionError.DuplicateAirportId;
        }
        
        Airport airport = new Airport(id, city, (DateTime)dateVisited, rating); // this will never be null, we check in checkAirportFields
        db.InsertAirport(airport);

        return AirportAdditionError.NoError;
    }
    
    

    public AirportDeletionError DeleteAirport(String id)
    {

        var entry = db.SelectAirport(id);

        if (entry != null)
        {
            AirportDeletionError success = db.DeleteAirport(entry);
            if (success == AirportDeletionError.NoError)
            {
                return AirportDeletionError.NoError;

            }
            else
            {
                return AirportDeletionError.DBDeletionError;
            }
        }
        else
        {
            return AirportDeletionError.AirportNotFound;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clue"></param>
    /// <param name="answer"></param>
    /// <param name="difficulty"></param>
    /// <param name="date"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public AirportEditError EditAirport(String id, String city, DateTime dateVisited, int rating)
    {

        var fieldCheck = CheckAirportFields(id, city, dateVisited, rating);
        if (fieldCheck != AirportAdditionError.NoError)
        {
            return AirportEditError.InvalidFieldError;
        }

        var airport = db.SelectAirport(id);
        airport.Id = id;
        airport.City = city;
        airport.DateVisited = dateVisited;
        airport.Rating = rating;

        AirportEditError success = db.UpdateAirport(airport);
        if (success != AirportEditError.NoError)
        {
            return AirportEditError.DBEditError;
        }

        return AirportEditError.NoError;
    }


    public String CalculateStatistics()
    {
        FlyWisconsinLevel nextLevel;
        int numAirportsUntilNextLevel;

        int numAirportsVisited = db.SelectAllAirports().Count;
        if(numAirportsVisited < BRONZE_LEVEL)
        {
            nextLevel = FlyWisconsinLevel.Bronze;
            numAirportsUntilNextLevel = BRONZE_LEVEL - numAirportsVisited;
        } else if(numAirportsVisited < SILVER_LEVEL)
        {
            nextLevel = FlyWisconsinLevel.Silver;
            numAirportsUntilNextLevel = SILVER_LEVEL - numAirportsVisited;
        } else if(numAirportsVisited < GOLD_LEVEL)
        {
            nextLevel = FlyWisconsinLevel.Gold;
            numAirportsUntilNextLevel = GOLD_LEVEL - numAirportsVisited;
        } else
        {
            nextLevel = FlyWisconsinLevel.None;
            numAirportsUntilNextLevel = 0;
        }

        return String.Format("{0} airport{1} visited; {2} airports remaining until achieving {3}",
              numAirportsVisited, numAirportsVisited != 1 ? "s" : "", numAirportsUntilNextLevel, nextLevel);
    }

    public ObservableCollection<Airport> GetAirports()
    {
        return db.SelectAllAirports();
    }

    /// <summary>
    /// Get the weather of the closest airport
    /// </summary>
    /// <returns>The weather of the closest airport</returns>
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

        ObservableCollection<Airport> allAirports = GetAirports();

        AirportCoordinates currentCoordinates = GetCurrentCoordinates();

        foreach (Airport destinationAirport in allAirports)
        {
            AirportCoordinates? destinationAirportCoordinates = airportCoordinates.FirstOrDefault(coordinates => coordinates.id == destinationAirport.Id, null);

            if (destinationAirportCoordinates != null)
            {
                // Haversine formula to find distance between two points
                double sourceLatitudeRadians = currentCoordinates.lat * (Math.PI / 180);
                double destinationLatitudeRadians = destinationAirportCoordinates.lat * (Math.PI / 180);
                double latitudeDiffRadians =
                    (destinationAirportCoordinates.lat - currentCoordinates.lat) * (Math.PI / 180);
                double longitudeDiffRadians =
                    (destinationAirportCoordinates.lon - currentCoordinates.lon) * (Math.PI / 180);
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
 
    private AirportCoordinates GetCurrentCoordinates()
    {
        var currLocation = Geolocation.GetLastKnownLocationAsync().Result;
        float lat = (float)currLocation.Latitude;
        float lon = (float)currLocation.Longitude;
        if (currLocation == null) {
            return new AirportCoordinates("", "", 0f, 0f, "");
        } else {
            return new AirportCoordinates("", "", lat, lon, "");
        }
    }

    public Route GetRoute()
    {
        var route = new Route("KATW", "KUBB");

        // Add edges 
        route.AddEdge("KATW", "Appleton", 0);
        route.AddEdge("KFLD", "Fond du Lac", 23);
        route.AddEdge("KUNN", "Dodge County", 28);
        route.AddEdge("KUBB", "Burlington", 47);
        route.AddEdge("KATW", "Appleton", 95);

        return route;
    }

    public ObservableCollection<Weather> GetWeathers()
    {
        throw new NotImplementedException();
    }
}

