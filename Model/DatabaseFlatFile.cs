using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Collections.ObjectModel;

// https://www.dotnetperls.com/serialize-list
// https://www.daveoncsharp.com/2009/07/xml-serialization-of-collections/


namespace Lab6_Starter.Model;

public class DatabaseFlatFile : IDatabase
{
    const String filename = "airports.db";
    String airportsFile;
    ObservableCollection<Airport> airports;
    JsonSerializerOptions options;

    public DatabaseFlatFile()
    {
        SelectAllAirports();
        options = new JsonSerializerOptions { WriteIndented = true };
    }

    public ObservableCollection<Airport> GetAllWisconsinAirports()
    {
        // Here, SelectAllAirports is used to get all airports, adjust filtering if needed
        return SelectAllAirports();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ObservableCollection<Airport>? SelectAllAirports()
    {
        string jsonString;

        String mainDir = FileSystem.Current.AppDataDirectory;
        airportsFile = String.Format("{0}/{1}", mainDir, filename);
        if (!File.Exists(airportsFile))
        {
            File.CreateText(airportsFile);
            airports = new ObservableCollection<Airport>();
            jsonString = JsonSerializer.Serialize(airports, options);
            File.WriteAllText(airportsFile, jsonString);
            return airports;
        }

        jsonString = File.ReadAllText(airportsFile);
        if (jsonString.Length > 0)
        {
            if (airports == null) // just starting the app, so let Deserialize() instantiate the ObservableCollection
            {
                airports = JsonSerializer.Deserialize<ObservableCollection<Airport>>(jsonString);
            }
            else
            { // airports already exists, and we have bound to it, so we cannot recreate it
                airports.Clear();
                ObservableCollection<Airport> localAirports = JsonSerializer.Deserialize<ObservableCollection<Airport>>(jsonString);
                foreach (Airport airport in localAirports)
                {
                    airports.Add(airport);
                }
            }
        }
        return airports;
    }

    public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        var R = 6371; // Radius of the earth in km
        var dLat = (lat2 - lat1) * (Math.PI / 180);
        var dLon = (lon2 - lon1) * (Math.PI / 180);
        var a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c; // Distance in km
    }

    public ObservableCollection<Airport> GetWisconsinAirportsWithinDistance(double userLatitude, double userLongitude, double maxDistanceKm)
    {
        ObservableCollection<Airport> nearbyAirports = new ObservableCollection<Airport>();

        foreach (var airport in airports)
        {
            double distance = CalculateDistance(userLatitude, userLongitude, airport.Latitude, airport.Longitude);
            if (distance <= maxDistanceKm)
            {
                nearbyAirports.Add(airport);
            }
        }

        return nearbyAirports;
    }

    public Airport? SelectAirportByCode(string airportCode)
    {
        foreach (var airport in airports)
        {
            if (airport.Id == airportCode)
            {
                return airport;
            }
        }
        return null;
    }

    public Airport? SelectAirport(String id)
    {
        foreach (Airport airport in airports)
        {
            if (airport.Id == id)
            {
                return airport;
            }
        }
        return null;
    }

    public AirportAdditionError InsertAirport(Airport airport)
    {
        try
        {
            airports.Add(airport);

            string jsonString = JsonSerializer.Serialize(airports, options);
            File.WriteAllText(airportsFile, jsonString);
            return AirportAdditionError.NoError;

        }
        catch (IOException ioe)
        {
            Console.WriteLine("Error while adding airport: {0}", ioe);
            return AirportAdditionError.DBAdditionError;

        }
    }



    /// <summary>
    /// Deletes an airport 
    /// </summary>
    /// 
    /// <param name="airport">An airport, which is presumed to exist</param>
    public AirportDeletionError DeleteAirport(Airport airport)
    {
        try
        {
            var result = airports.Remove(airport);
            string jsonString = JsonSerializer.Serialize(airports, options);
            File.WriteAllText(airportsFile, jsonString);
            return AirportDeletionError.NoError;
        }
        catch (IOException ioe)
        {
            Console.WriteLine("Error while deleting airport: {0}", ioe);
            return AirportDeletionError.DBDeletionError;
        }
    }

    public AirportEditError UpdateAirport(Airport replacementAirport)
    {
        foreach (Airport airport in airports) // iterate through entries until we find the Entry in question
        {
            if (airport.Id == replacementAirport.Id) // found it
            {
                airport.City = replacementAirport.City;
                airport.DateVisited = replacementAirport.DateVisited;
                airport.Rating = replacementAirport.Rating;         // change it then write it out

                try
                {
                    string jsonString = JsonSerializer.Serialize(airports, options);
                    File.WriteAllText(airportsFile, jsonString);
                    return AirportEditError.NoError;
                }
                catch (IOException ioe)
                {
                    Console.WriteLine("Error while replacing airport: {0}", ioe);
                    return AirportEditError.DBEditError;
                }
            }
        }
        return AirportEditError.NoError;
    }


}
