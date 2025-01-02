using System.Collections.ObjectModel;

namespace Lab6_Starter.Model;
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

}