using System.Collections.ObjectModel;
using Lab6_Starter.Model;

namespace FWAPPA.Model;

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
        //     double distanceInMeters = EARTH_RADIUS_IN_METERS * angularDistance;
        //     double distanceInMiles = distanceInMeters * MILES_PER_METER;
        //     if (distanceInMiles < maxMiles)
        //     {
        //         nearbyAirports.Add(destinationAirport);
        //     }
        // }
        return nearbyAirports;
    }
    
}