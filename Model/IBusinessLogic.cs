using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Lab6_Starter.Model;
public interface IBusinessLogic
{
    ObservableCollection<VisitedAirport> VisitedAirports {get;}
    Task<AirportAdditionError> AddAirport(String id, String name, DateTime? dateVisited, int rating);
    Task<AirportDeletionError> DeleteAirport(String id);
    Task<AirportEditError> EditAirport(String id, String name, DateTime dateVisited, int rating);
    Task<VisitedAirport?> FindAirport(String id);
    String CalculateStatistics();
    Task<ObservableCollection<VisitedAirport>> GetVisitedAirports();
    ObservableCollection<WisconsinAirport> GetWisconsinAirports();
    Weather GetClosestAirportWeather();
    ObservableCollection<WisconsinAirport> CalculateNearbyAirports(WisconsinAirport sourceAirport, int maxMiles);
    ObservableCollection<WisconsinAirport> GetAllWisconsinAirports();
    ObservableCollection<WisconsinAirport> GetWisconsinAirportsWithinDistance(double userLatitude, double userLongitude, double maxDistanceKm);
    WisconsinAirport SelectAirportByCode(string airportCode);
    Route GetRoute(WisconsinAirport source, int maxMiles, bool unvisitedOnly = false);

// Authentication and registration methods
    public Task<Boolean> AuthenticateUser(string email, string password);

    public Task<Boolean> RegisterUser(string email, string password);
}
