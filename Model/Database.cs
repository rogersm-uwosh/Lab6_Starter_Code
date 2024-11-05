using System.Collections.ObjectModel;
using Microsoft.Extensions.Configuration;
using Npgsql; // To install this, add dotnet add package Npgsql 


namespace Lab6_Starter.Model;

public class Database : IDatabase
{
    private static System.Random rng = new();
    private String connString;

    ObservableCollection<Airport> airports = new();

    public Database()
    {
        connString = GetConnectionString();
    }



    // Fills our local Airports ObservableCollection with all the airports in the database
    // We don't cache the airports in the database, so we have to go to the database to get them
    // This is one of those "tradeoffs" we have to make when we use a database
    public ObservableCollection<Airport> SelectAllAirports()
    {
        airports.Clear();
        var conn = new NpgsqlConnection(connString);
        conn.Open();

        // using() ==> disposable types are properly disposed of, even if there is an exception thrown 
        using var cmd = new NpgsqlCommand("SELECT id, city, date_visited, rating FROM airports", conn);
        using var reader = cmd.ExecuteReader(); // used for SELECT statement, returns a forward-only traversable object

        while (reader.Read()) // each time through we get another row in the table (i.e., another Airport)
        {
            String id = reader.GetString(0);
            String city = reader.GetString(1);
            DateTime dateVisited = reader.GetDateTime(2);
            Int32 rating = reader.GetInt32(3);
            Airport airportToAdd = new(id, city, dateVisited, rating);
            airports.Add(airportToAdd);
            Console.WriteLine(airportToAdd);
        }

        return airports;
    }

    // Add this method to the Database class
    public ObservableCollection<Airport> GetAllWisconsinAirports()
    {
        ObservableCollection<Airport> wisconsinAirports = new ObservableCollection<Airport>();

        using var conn = new NpgsqlConnection(connString);
        conn.Open();

        using var cmd = new NpgsqlCommand("SELECT id, name, lat, long, url FROM wi_airports", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            string id = reader.GetString(0);
            string name = reader.GetString(1);
            double latitude = reader.GetDouble(2);
            double longitude = reader.GetDouble(3);
            string url = reader.GetString(4);

            wisconsinAirports.Add(new Airport(id, name, latitude, longitude, url));
        }

        return wisconsinAirports;
    }

    public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        var earthRadius = 6371; // Radius of the earth in km
        var dLat = (lat2 - lat1) * (Math.PI / 180);
        var dLon = (lon2 - lon1) * (Math.PI / 180);
        var a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return earthRadius * c; // Distance in km
    }

    public Airport SelectAirportByCode(string airportCode)
    {
        Airport selectedAirport = null;
        using var conn = new NpgsqlConnection(connString);
        conn.Open();

        using var cmd = new NpgsqlCommand("SELECT id, name, lat, long FROM wi_airports WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("id", airportCode);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            string id = reader.GetString(0);
            string name = reader.GetString(1);
            double latitude = reader.GetDouble(2);
            double longitude = reader.GetDouble(3);
            selectedAirport = new Airport(id, name, latitude, longitude, null);
        }

        return selectedAirport;
    }


    public ObservableCollection<Airport> GetWisconsinAirportsWithinDistance(double userLat, double userLon, double maxDistanceKm)
    {
        ObservableCollection<Airport> nearbyAirports = new ObservableCollection<Airport>();
        var allAirports = GetAllWisconsinAirports();

        foreach (var airport in allAirports)
        {
            double distance = CalculateDistance(userLat, userLon, airport.Latitude, airport.Longitude);
            if (distance <= maxDistanceKm)
            {
                airport.Distance = distance; // Set the calculated distance
                nearbyAirports.Add(airport);
            }
        }
        return nearbyAirports;
    }


    // Finds the airport with the given id, null if not found
    public Airport SelectAirport(String id)
    {
        Airport airportToAdd = null;
        var conn = new NpgsqlConnection(connString);
        conn.Open();

        using var cmd = new NpgsqlCommand("SELECT id, city, date_visited, rating FROM airports WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("id", id);

        using var reader = cmd.ExecuteReader(); // used for SELECT statement, returns a forward-only traversable object
        if (reader.Read())
        { // there should be only one row, so we don't need a while loop TODO: Sanity check

            id = reader.GetString(0);
            String city = reader.GetString(1);
            DateTime dateVisited = reader.GetDateTime(2);
            Int32 rating = reader.GetInt32(3);
            airportToAdd = new(id, city, dateVisited, rating);
        }
        return airportToAdd;
    }

    // This inserts a airport into the database, or prints out an error message and returns false if the airport already exists
    // Notice the try-catch block, how could INSERT possibly fail?
    public AirportAdditionError InsertAirport(Airport airport)
    {
        try
        {
            using var conn = new NpgsqlConnection(connString); // conn, short for connection, is a connection to the database

            conn.Open(); // open the connection ... now we are connected!
            var cmd = new NpgsqlCommand(); // create the sql commaned
            cmd.Connection = conn; // commands need a connection, an actual command to execute
            cmd.CommandText = "INSERT INTO airports (id, city, date_visited, rating) VALUES (@id, @city, @date_visited, @rating)";
            cmd.Parameters.AddWithValue("id", airport.Id);
            cmd.Parameters.AddWithValue("city", airport.City);
            cmd.Parameters.AddWithValue("date_visited", airport.DateVisited);
            cmd.Parameters.AddWithValue("rating", airport.Rating);
            cmd.ExecuteNonQuery(); // used for INSERT, UPDATE & DELETE statements - returns # of affected rows 

            SelectAllAirports();
        }
        catch (Npgsql.PostgresException pe)
        {
            Console.WriteLine("Insert failed, {0}", pe);
            return AirportAdditionError.DBAdditionError;
        }
        return AirportAdditionError.NoError;
    }



    // The UI >> BusinessLogic >> Database the currently selected airport (selected in the CollectionView) for upating 
    public AirportEditError UpdateAirport(Airport airportToUpdate)
    {
        try
        {
            using var conn = new NpgsqlConnection(connString); // conn, short for connection, is a connection to the database

            conn.Open(); // open the connection ... now we are connected!
            var cmd = new NpgsqlCommand(); // create the sql commaned
            cmd.Connection = conn; // commands need a connection, an actual command to execute
            cmd.CommandText = "UPDATE airports SET city = @city, date_visited = @date_visited, rating = @rating WHERE id = @id;";

            cmd.Parameters.AddWithValue("id", airportToUpdate.Id);
            cmd.Parameters.AddWithValue("city", airportToUpdate.City);
            cmd.Parameters.AddWithValue("date_visited", airportToUpdate.DateVisited);
            cmd.Parameters.AddWithValue("rating", airportToUpdate.Rating);
            var numAffected = cmd.ExecuteNonQuery();

            SelectAllAirports();
        }
        catch (Npgsql.PostgresException pe)
        {
            Console.WriteLine("Update failed, {0}", pe);
            return AirportEditError.DBEditError;
        }
        return AirportEditError.NoError;
    }


    public AirportDeletionError DeleteAirport(Airport airportToDelete)
    {
        var conn = new NpgsqlConnection(connString);
        conn.Open();

        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "DELETE FROM airports WHERE id = @id";
        cmd.Parameters.AddWithValue("id", airportToDelete.Id);
        int numDeleted = cmd.ExecuteNonQuery();

        if (numDeleted > 0)
        {
            SelectAllAirports(); // go and fetch the airports again, otherwise Airports will be out of sync with the database
            return AirportDeletionError.NoError;
        }
        else
        {
            return AirportDeletionError.AirportNotFound;
        }
    }


    // Builds a ConnectionString, which is used to connect to the database
    static String GetConnectionString()
    {
        var connStringBuilder = new NpgsqlConnectionStringBuilder();
        connStringBuilder.Host = "stormy-ocelot-12775.5xj.cockroachlabs.cloud";
        connStringBuilder.Port = 26257;
        connStringBuilder.SslMode = SslMode.VerifyFull;
        connStringBuilder.Username = "cs341"; // won't hardcode this in your app
        connStringBuilder.Password = FetchPassword();
        connStringBuilder.Database = "defaultdb";
        connStringBuilder.ApplicationName = "whatever";
        connStringBuilder.IncludeErrorDetail = true;

        return connStringBuilder.ConnectionString;
    }

    // Fetches the password from the user secrets store (um, this works in VS, but not in the beta of VSC's C# extension)
    // This assumes the NuGet package is installed -- dotnet add package Microsoft.Extensions.Configuration.UserSecrets
    static String FetchPassword()
    {
        IConfiguration config = new ConfigurationBuilder().AddUserSecrets<Database>().Build();
        return config["CockroachDBPassword"] ?? "thQ0vyMPC96yfUpkjY4dBg"; // if it can't find the password, returns ... the password (this works in VS, not VSC) 
    }


}

