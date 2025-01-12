using System.Collections.ObjectModel;

namespace FWAPPA.Model;
public interface IBusinessLogic
{
    ObservableCollection<VisitedAirport> VisitedAirports {get;}
    Route CurrentRoute { get; set; }
    Task<AirportAdditionError> AddAirport(string id, string name, DateTime dateVisited, int rating);
    Task<AirportDeletionError> DeleteAirport(string id);
    Task<AirportEditError> EditAirport(string id, string name, DateTime dateVisited, int rating);
    Task<VisitedAirport?> FindAirport(string id);
    string CalculateStatistics();
    Task<ObservableCollection<VisitedAirport>> GetVisitedAirports();
    ObservableCollection<WisconsinAirport> GetWisconsinAirports();
    Weather GetClosestAirportWeather();
    ObservableCollection<WisconsinAirport> CalculateNearbyAirports(WisconsinAirport sourceAirport, int maxMiles);
    ObservableCollection<WisconsinAirport> GetAllWisconsinAirports();
    ObservableCollection<WisconsinAirport> GetWisconsinAirportsWithinDistance(double userLatitude, double userLongitude, double maxDistanceKm);
    WisconsinAirport SelectAirportByCode(string airportCode);
    Route? GetRoute(WisconsinAirport source, int maxMiles, bool unvisitedOnly = false);

// Authentication and registration methods
    public Task<Boolean> AuthenticateUser(string email, string password);

    public Task<Boolean> RegisterUser(string email, string password);
}
