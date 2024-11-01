using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Lab6_Starter.Model
{
    public interface IDatabase
    {
        ObservableCollection<Airport> SelectAllAirports();
        ObservableCollection<Airport> SelectAllWisconsinAirports();
        Airport SelectAirport(String id);
        AirportAdditionError InsertAirport(Airport airport);
        AirportDeletionError DeleteAirport(Airport airport);
        AirportEditError UpdateAirport(Airport replacementAirport);
    }
}