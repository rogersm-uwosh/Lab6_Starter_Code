using System.Collections.ObjectModel;
using FWAPPA.NearbyAirports;
using Lab6_Starter.Model;

namespace FWAPPA.Model;
/// <summary>
/// Jason Wang
/// </summary>
public partial class BusinessLogic
{
    private const int EARTH_RADIUS_IN_METERS = 6378000;
    private const double MILES_PER_METER = 0.00062137;
    private ObservableCollection<AirportCoordinates> airportCoordinates;

    partial void LoadAirportCoordinates()
    {
        airportCoordinates = db.SelectAllAirportCoordinates();
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
        Dictionary<string, int> idToMiles = new();

        ObservableCollection<Airport> allAirports = GetWisconsinAirports();

        AirportCoordinates? sourceAirportCoordinates = airportCoordinates.FirstOrDefault(coordinates => coordinates.id == sourceAirport.Id, null);
        if (sourceAirportCoordinates == null)
        {
            return [];
        }
        foreach (Airport destinationAirport in allAirports)
        {
            AirportCoordinates? destinationAirportCoordinates = airportCoordinates.FirstOrDefault(coordinates => coordinates.id == destinationAirport.Id && coordinates.id != sourceAirport.Id, null);

            if (destinationAirportCoordinates != null) {
                double distanceInMiles = GetDistanceFromAirportCoordinates(sourceAirportCoordinates, destinationAirportCoordinates);
                if (distanceInMiles < maxMiles) {
                    nearbyAirports.Add(destinationAirport);
                    idToMiles[destinationAirport.Id] = (int) Math.Round(distanceInMiles);
                }
            }
        }

        AirportToMilesConverter.ConvertAll(idToMiles);
        return nearbyAirports;
    }

    public static double GetDistanceFromAirportCoordinates(AirportCoordinates sourceAirportCoordinates, AirportCoordinates destinationAirportCoordinates) {
        // Haversine formula to find distance between two points
        double sourceLatitudeRadians = sourceAirportCoordinates.lat * (Math.PI / 180);
        double destinationLatitudeRadians = destinationAirportCoordinates.lat * (Math.PI / 180);
        double latitudeDiffRadians =
            (destinationAirportCoordinates.lat - sourceAirportCoordinates.lat) * (Math.PI / 180);
        double longitudeDiffRadians =
            (destinationAirportCoordinates.lon - sourceAirportCoordinates.lon) * (Math.PI / 180);
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