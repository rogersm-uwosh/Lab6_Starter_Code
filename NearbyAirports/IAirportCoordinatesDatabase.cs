using System.Collections.ObjectModel;
using FWAPPA.NearbyAirports;

namespace Lab6_Starter.Model;

public partial interface IDatabase
{
    ObservableCollection<AirportCoordinates> SelectAllAirportCoordinates();
    AirportCoordinates SelectAirportCoordinates(String id);
    AirportAdditionError InsertAirport(AirportCoordinates airport);
    AirportEditError UpdateAirport(AirportCoordinates airportToUpdate);
    AirportDeletionError DeleteAirport(AirportCoordinates airportToDelete); 
}