using System.Collections.ObjectModel;
using Supabase.Gotrue;

namespace FWAPPA.Model;
public partial interface IDatabaseSupa
{
    Task<ObservableCollection<VisitedAirport>> SelectAllVisitedAirports();
    Task<VisitedAirport?> SelectAirport(String id);
    Task<AirportAdditionError> InsertAirport(VisitedAirport airport);
    Task<AirportEditError> UpdateAirport(VisitedAirport replacementAirport);
    Task<AirportDeletionError> DeleteAirport(VisitedAirport airport);
    ObservableCollection<WisconsinAirport> GetAllWisconsinAirports();
    ObservableCollection<WisconsinAirport> GetWisconsinAirportsWithinDistance(double userLat, double userLon, double maxDistanceKm);
    WisconsinAirport SelectAirportByCode(string airportCode);

// Authentication and registration methods

    public Task<User?> AuthenticateUser(string email, string password);

    public Task<User?> RegisterUser(string email, string password);

}