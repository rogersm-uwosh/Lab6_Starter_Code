using System.Collections.ObjectModel;
using FWAPPA.Model.NearbyAirports;

namespace FWAPPA.Model;

/// <summary>
/// Jason Wang
/// </summary>
public partial class BusinessLogic
{
    private const int EARTH_RADIUS_IN_METERS = 6378000;
    private const double MILES_PER_METER = 0.00062137;

    public ObservableCollection<WisconsinAirport> NearbyAirports { get; } = [];

    private bool isNearbyAirportsHeaderVisible;

    public bool IsNearbyAirportsHeaderVisible
    {
        get => isNearbyAirportsHeaderVisible;
        set
        {
            if (isNearbyAirportsHeaderVisible != value)
            {
                isNearbyAirportsHeaderVisible = value;
                OnPropertyChanged(nameof(IsNearbyAirportsHeaderVisible));
            }
        }
    }

    /// <summary>
    /// Find all airports within maxMiles of sourceAirport
    /// </summary>
    /// <param name="sourceAirport">The airport whose location to use as reference.</param>
    /// <param name="maxMiles">How far the desired airports are to be from the sourceAirport.</param>
    /// <returns></returns>
    public void CalculateNearbyAirports(WisconsinAirport sourceAirport, int maxMiles)
    {
        NearbyAirports.Clear();
        Dictionary<string, int> idToMiles = new();

        ObservableCollection<WisconsinAirport> allAirports = GetWisconsinAirports();

        foreach (WisconsinAirport destinationAirport in allAirports)
        {
            double distanceInMiles = GetDistanceBetweenCoordinates(
                sourceAirport.Coordinates,
                destinationAirport.Coordinates
            );
            if (distanceInMiles < maxMiles)
            {
                NearbyAirports.Add(destinationAirport);
                idToMiles[destinationAirport.Id] = (int)Math.Round(distanceInMiles);
            }
        }

        IsNearbyAirportsHeaderVisible = NearbyAirports.Count > 0;

        // ConvertAll is used to populate _idToMiles with the (id, distance) entries
        // This allows the AirportToMilesConverter to correctly display distance in XAML
        AirportToMilesConverter.ConvertAll(idToMiles); 
    }
}