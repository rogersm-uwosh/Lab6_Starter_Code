using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Lab6_Starter;

namespace Lab6_Starter.Model;

public class BusinessLogic : IBusinessLogic
{
    private const int EARTH_RADUIS_IN_METERS = 6378000;
    private const double MILES_PER_METER = 0.00062137;
    
    const int BRONZE_LEVEL = 42;
    const int SILVER_LEVEL = 84;
    const int GOLD_LEVEL = 128;
    
    IDatabase db;
    private readonly int MAX_RATING = 5;

    public ObservableCollection<Airport> Airports
    {
        get { return GetAirports(); }

    }

    public ObservableCollection<Weather> Weathers
    {
        get { return GetWeathers(); }

    }
    public BusinessLogic(IDatabase? db)
    {
        this.db = db;
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

    public ObservableCollection<Weather> GetWeathers()
    {
        ObservableCollection<Weather> weathers = new ObservableCollection<Weather>();
        weathers.Add(new Weather("METAR KJFK 161853Z 21015G25KT 10SM -RA SCT020 BKN050", "TAF KJFK 161720Z 1618/1718 21015KT P6SM -RA BKN050"));
        return weathers;
    }

    /// <summary>
    /// Find all airports within maxMiles of sourceAirport
    /// </summary>
    /// <param name="sourceAirport">The airport whose location to use as reference.</param>
    /// <param name="maxMiles">How far the desired airports are to be from the sourceAirport.</param>
    /// <returns></returns>
    public ObservableCollection<Airport> CalculateNearbyAirports(Airport sourceAirport, int maxMiles)
    {
        ObservableCollection<Airport> nearbyAirports = [];
        ObservableCollection<Airport> allAirports = GetAirports();


        // Formula for when we are able to get coordinates
        nearbyAirports.Add(new Airport("KFLD", "Fond du Lac", DateTime.Now, 1));
        nearbyAirports.Add(new Airport("KMTW", "Manitowac", DateTime.Now, 1));
        nearbyAirports.Add(new Airport("79C", "Brenner", DateTime.Now, 5));
        nearbyAirports.Add(new Airport("KUNU", "Dodge County", DateTime.Now, 1));


        // foreach (Airport destinationAirport in allAirports)
        // {
        //     // Haversine formula to find distance between two points
        //     double sourceLatitudeRadians = sourceAirport.Latitude * (Math.PI / 180);
        //     double destinationLatitudeRadians = destinationAirport.Latitude * (Math.PI / 180);
        //     double latitudeDiffRadians = (destinationAirport.Latitude - sourceAirport.Latitude) * (Math.PI / 180);
        //     double longitudeDiffRadians = (destinationAirport.Longitude - sourceAirport.Longitude) * (Math.PI / 180);
        //     double flatDistance = Math.Pow(Math.Sin(latitudeDiffRadians / 2.0), 2.0) + 
        //                (Math.Cos(sourceLatitudeRadians) * 
        //                 Math.Cos(destinationLatitudeRadians) * 
        //                 Math.Pow(Math.Sin(longitudeDiffRadians / 2.0), 2.0));
        //     double angularDistance = 2 * Math.Atan2(Math.Sqrt(flatDistance), Math.Sqrt(1 - flatDistance));
        //     double distanceInMeters = EARTH_RADUIS_IN_METERS * angularDistance;
        //     double distanceInMiles = distanceInMeters * MILES_PER_METER;
        //     if (distanceInMiles < maxMiles)
        //     {
        //         nearbyAirports.Add(destinationAirport);
        //     }
        // }
        return nearbyAirports;
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

}

