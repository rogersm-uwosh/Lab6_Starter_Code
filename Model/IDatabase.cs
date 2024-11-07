using System.Collections.ObjectModel;

namespace Lab6_Starter.Model
{
    public partial interface IDatabase
    {
        ObservableCollection<Airport> SelectAllAirports();
        ObservableCollection<Airport> SelectAllWisconsinAirports();
        Airport SelectAirport(String id);
        AirportAdditionError InsertAirport(Airport airport);
        AirportDeletionError DeleteAirport(Airport airport);
        AirportEditError UpdateAirport(Airport replacementAirport);
        ObservableCollection<Airport> GetAllWisconsinAirports();
        ObservableCollection<Airport> GetWisconsinAirportsWithinDistance(double userLat, double userLon, double maxDistanceKm);
        Airport SelectAirportByCode(string airportCode);
    }
}