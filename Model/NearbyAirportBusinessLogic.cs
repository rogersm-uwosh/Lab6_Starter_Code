using System.Collections.ObjectModel;
using Lab6_Starter.Model.NearbyAirports;

namespace Lab6_Starter.Model;

/// <summary>
/// Jason Wang
/// </summary>
public partial class BusinessLogic
{
    private const int EARTH_RADIUS_IN_METERS = 6378000;
    private const double MILES_PER_METER = 0.00062137;

    /// <summary>
    /// Find all airports within maxMiles of sourceAirport
    /// </summary>
    /// <param name="sourceAirport">The airport whose location to use as reference.</param>
    /// <param name="maxMiles">How far the desired airports are to be from the sourceAirport.</param>
    /// <returns></returns>
    public ObservableCollection<WisconsinAirport> CalculateNearbyAirports(WisconsinAirport sourceAirport, int maxMiles)
    {
        ObservableCollection<WisconsinAirport> nearbyAirports = [];
        Dictionary<string, int> idToMiles = new();

        ObservableCollection<WisconsinAirport> allAirports = GetWisconsinAirports();

        WisconsinAirport? wisconsinAirport =
            WisconsinAirports.FirstOrDefault(airport => airport?.Id == sourceAirport.Id, null);
        if (wisconsinAirport == null)
        {
            return [];
        }

        foreach (WisconsinAirport destinationAirport in allAirports)
        {
            WisconsinAirport? destinationAirportCoordinates =
                WisconsinAirports.FirstOrDefault(
                    airport => airport?.Id == destinationAirport.Id && airport.Id != sourceAirport.Id, null);

            if (destinationAirportCoordinates != null)
            {
                double distanceInMiles =
                    GetDistanceFromAirportCoordinates(wisconsinAirport, destinationAirportCoordinates);
                if (distanceInMiles < maxMiles)
                {
                    nearbyAirports.Add(destinationAirport);
                    idToMiles[destinationAirport.Id] = (int)Math.Round(distanceInMiles);
                }
            }
        }

        AirportToMilesConverter.ConvertAll(idToMiles); // calling convert all to ... populate _idToMiles with the (id, distance) entries
        return nearbyAirports;
    }

    public static double GetDistanceFromAirportCoordinates(
        WisconsinAirport sourceAirport,
        WisconsinAirport destinationAirport
    )
    {
        // Haversine formula to find distance between two points
        double sourceLatitudeRadians = sourceAirport.Latitude * (Math.PI / 180);
        double destinationLatitudeRadians = destinationAirport.Latitude * (Math.PI / 180);
        double latitudeDiffRadians =
            (destinationAirport.Latitude - sourceAirport.Latitude) * (Math.PI / 180);
        double longitudeDiffRadians =
            (destinationAirport.Longitude - sourceAirport.Longitude) * (Math.PI / 180);
        double flatDistance = Math.Pow(Math.Sin(latitudeDiffRadians / 2.0), 2.0) +
                              (Math.Cos(sourceLatitudeRadians) *
                               Math.Cos(destinationLatitudeRadians) *
                               Math.Pow(Math.Sin(longitudeDiffRadians / 2.0), 2.0));
        double angularDistance = 2 * Math.Atan2(Math.Sqrt(flatDistance), Math.Sqrt(1 - flatDistance));
        double distanceInMeters = EARTH_RADIUS_IN_METERS * angularDistance;
        double distanceInMiles = distanceInMeters * MILES_PER_METER;
        return distanceInMiles;
    }
}