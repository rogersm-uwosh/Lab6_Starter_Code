using System.Collections.ObjectModel;
//using CommunityToolkit.Maui.Core.Extensions;


namespace Lab6_Starter.Model;

public partial class BusinessLogic : IBusinessLogic
{

    const int BRONZE_LEVEL = 42;
    const int SILVER_LEVEL = 84;
    const int GOLD_LEVEL = 128;

    IDatabaseSupa db;
    private readonly int MAX_RATING = 5;

    ObservableCollection<VisitedAirport> visitedAirports = [];
    public ObservableCollection<VisitedAirport> VisitedAirports
    {
        get { return visitedAirports; }

    }

    public ObservableCollection<WisconsinAirport> WisconsinAirports
    {
        get { return GetAllWisconsinAirports(); }

    }

    public ObservableCollection<WisconsinAirport> GetAllWisconsinAirports()
    {
        return db.GetAllWisconsinAirports();
    }

    public ObservableCollection<WisconsinAirport> GetWisconsinAirportsWithinDistance(double userLatitude, double userLongitude, double maxDistanceKm)
    {
        return db.GetWisconsinAirportsWithinDistance(userLatitude, userLongitude, maxDistanceKm);
    }

    public WisconsinAirport SelectAirportByCode(string airportCode)
    {
        return db.SelectAirportByCode(airportCode);
    }

    public ObservableCollection<Weather> Weathers
    {
        get { return GetWeathers(); }

    }

    partial void LoadAirportCoordinates();

    public BusinessLogic(IDatabaseSupa db)
    {
        this.db = db;
        LoadAirportCoordinates();
    }


    public async Task<VisitedAirport?> FindAirport(String id)
    {
        return await db.SelectAirport(id);
    }

    private AirportAdditionError CheckAirportFields(String? id, String? name, DateTime? dateVisited, int rating)
    {

        if (id == null || id.Length < 3 || id.Length > 4)
        {
            return AirportAdditionError.InvalidIdLength;
        }
        if (name == null || name.Length < 3)
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


    public async Task<AirportAdditionError> AddAirport(String id, String name, DateTime? dateVisited, int rating)
    {

        var result = CheckAirportFields(id, name, dateVisited, rating);
        if (result != AirportAdditionError.NoError)
        {
            return result;
        }

        if (db.SelectAirport(id) != null)
        {
            return AirportAdditionError.DuplicateAirportId;
        }

        VisitedAirport airport = new VisitedAirport(id, name, (DateTime)dateVisited, rating); // this will never be null, we check in checkAirportFields
        await db.InsertAirport(airport);

        return AirportAdditionError.NoError;
    }



    public async Task<AirportDeletionError> DeleteAirport(String id)
    {

        var entry = await db.SelectAirport(id);

        if (entry != null)
        {
            AirportDeletionError success = await db.DeleteAirport(entry);
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
    public async Task<AirportEditError> EditAirport(String id, String name, DateTime dateVisited, int rating)
    {

        var fieldCheck = CheckAirportFields(id, name, dateVisited, rating);
        if (fieldCheck != AirportAdditionError.NoError)
        {
            return AirportEditError.InvalidFieldError;
        }

        VisitedAirport? airport = await db.SelectAirport(id);
        airport.Id = id;
        airport.Name = name;
        airport.DateVisited = dateVisited;
        airport.Rating = rating;

        AirportEditError success = await db.UpdateAirport(airport);
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

        int numAirportsVisited = VisitedAirports.Count;
        if (numAirportsVisited < BRONZE_LEVEL)
        {
            nextLevel = FlyWisconsinLevel.Bronze;
            numAirportsUntilNextLevel = BRONZE_LEVEL - numAirportsVisited;
        }
        else if (numAirportsVisited < SILVER_LEVEL)
        {
            nextLevel = FlyWisconsinLevel.Silver;
            numAirportsUntilNextLevel = SILVER_LEVEL - numAirportsVisited;
        }
        else if (numAirportsVisited < GOLD_LEVEL)
        {
            nextLevel = FlyWisconsinLevel.Gold;
            numAirportsUntilNextLevel = GOLD_LEVEL - numAirportsVisited;
        }
        else
        {
            nextLevel = FlyWisconsinLevel.None;
            numAirportsUntilNextLevel = 0;
        }

        return String.Format("{0} airport{1} visited; {2} airports remaining until achieving {3}",
              numAirportsVisited, numAirportsVisited != 1 ? "s" : "", numAirportsUntilNextLevel, nextLevel);
    }

    public async Task<ObservableCollection<VisitedAirport>> GetVisitedAirports()
    {
        return await db.SelectAllVisitedAirports();
    }

    public ObservableCollection<WisconsinAirport> GetWisconsinAirports()
    {
        return db.GetAllWisconsinAirports();
    }

    public ObservableCollection<Weather> GetWeathers()
    {
        ObservableCollection<Weather> weathers = new ObservableCollection<Weather>();
        weathers.Add(new Weather("METAR KJFK 161853Z 21015G25KT 10SM -RA SCT020 BKN050", "TAF KJFK 161720Z 1618/1718 21015KT P6SM -RA BKN050"));
        return weathers;
    }

    public Route GetRoute(WisconsinAirport source, int maxMiles, bool unvisitedOnly)
    {
        // // We need to force the start to be at the beginning, so we remove it
        // // and possibly already visited airports
        // IEnumerable<Airport> excluded;
        // if (unvisitedOnly)
        // {
        //     excluded = GetAirports().Append(source);
        // }
        // else
        // {
        //     excluded = [source];
        // }
        // // Convert the airports to RoutePoints
        // List<RoutePoint> routePoints = CalculateNearbyAirports(source, maxMiles)
        //     .Except(excluded, new AirportEqualityComparer())
        //     .Prepend(source)
        //     .Select(x => new RoutePoint(x))
        //     .ToList();

        // // Can't have a route with 0 or 1 airports
        // if (routePoints.Count < 2)
        // {
        //     return null;
        // }

        // return Route.GenerateTravelingSalesmanRoute(routePoints);

        return null;
    }

}

