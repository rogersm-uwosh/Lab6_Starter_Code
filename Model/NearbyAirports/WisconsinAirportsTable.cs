// using System.Collections.ObjectModel;
// using FWAPPA.NearbyAirports;
// using Npgsql;  

// namespace Lab6_Starter.Model;

// /// <summary>
// /// James Thao did this :) <3
// /// </summary>
// public partial class Database
// {

//     ObservableCollection<AirportCoordinates> wi_airports = new();
    
//     public ObservableCollection<AirportCoordinates> SelectAllAirportCoordinates()
//     {
//         wi_airports.Clear();
//         var conn = new NpgsqlConnection(connString);
//         conn.Open();

//         using var cmd = new NpgsqlCommand("SELECT id, name, lat, long, url FROM wi_airports", conn);
//         using var
//             reader = cmd.ExecuteReader();

//         while (reader.Read())
//         {
//             String id = reader.GetString(0);
//             String name = reader.GetString(1);
//             float lat = reader.GetFloat(2);
//             float lon = reader.GetFloat(3);
//             String url = reader.GetString(4);
//             AirportCoordinates airportToAdd = new(id, name, lat, lon, url);
//             wi_airports.Add(airportToAdd);
//             Console.WriteLine(airportToAdd);
//         }

//         return wi_airports;
//     }

//     public AirportCoordinates SelectAirportCoordinates(String id)
//     {
//         AirportCoordinates airportToAdd = null;
//         var conn = new NpgsqlConnection(connString);
//         conn.Open();

//         using var cmd = new NpgsqlCommand("SELECT id, name, lat, long, url FROM wi_airports WHERE id = @id",
//             conn);
//         cmd.Parameters.AddWithValue("id", id);

//         using var
//             reader = cmd.ExecuteReader(); 
//         if (reader.Read())
//         {
//             String name = reader.GetString(1);
//             float lat = reader.GetFloat(2);
//             float lon = reader.GetFloat(3);
//             String url = reader.GetString(4);
//             airportToAdd = new(id, name, lat, lon, url);
//         }

//         return airportToAdd;
//     }
    
//     public AirportAdditionError InsertAirport(AirportCoordinates airport)
//     {
//         try
//         {
//             using var
//                 conn = new NpgsqlConnection(
//                     connString); 

//             conn.Open();
//             var cmd = new NpgsqlCommand();
//             cmd.Connection = conn;
//             cmd.CommandText =
//                 "INSERT INTO wi_airports (id, name, lat, long, url) VALUES (@id, @name, @lat, @long, @url)";
//             cmd.Parameters.AddWithValue("id", airport.id);
//             cmd.Parameters.AddWithValue("name", airport.name);
//             cmd.Parameters.AddWithValue("lat", airport.lat);
//             cmd.Parameters.AddWithValue("long", airport.lon);
//             cmd.Parameters.AddWithValue("url", airport.url);
//             cmd.ExecuteNonQuery(); 

//             SelectAllAirports();
//         }
//         catch (Npgsql.PostgresException pe)
//         {
//             Console.WriteLine("Insert failed, {0}", pe);
//             return AirportAdditionError.DBAdditionError;
//         }

//         return AirportAdditionError.NoError;
//     }

//     public AirportEditError UpdateAirport(AirportCoordinates airportToUpdate)
//     {
//         try
//         {
//             using var
//                 conn = new NpgsqlConnection(
//                     connString);

//             conn.Open();
//             var cmd = new NpgsqlCommand(); 
//             cmd.Connection = conn;
//             cmd.CommandText =
//                 "UPDATE wi_airports SET name = @name, lat = @lat, long = @long, url = @url WHERE id = @id;";

//             cmd.Parameters.AddWithValue("id", airportToUpdate.id);
//             cmd.Parameters.AddWithValue("name", airportToUpdate.name);
//             cmd.Parameters.AddWithValue("lat", airportToUpdate.lat);
//             cmd.Parameters.AddWithValue("long", airportToUpdate.lon);
//             cmd.Parameters.AddWithValue("url", airportToUpdate.url);
//             var numAffected = cmd.ExecuteNonQuery();

//             SelectAllAirports();
//         }
//         catch (Npgsql.PostgresException pe)
//         {
//             Console.WriteLine("Update failed, {0}", pe);
//             return AirportEditError.DBEditError;
//         }

//         return AirportEditError.NoError;
//     }


//     public AirportDeletionError DeleteAirport(AirportCoordinates airportToDelete)
//     {
//         var conn = new NpgsqlConnection(connString);
//         conn.Open();

//         using var cmd = new NpgsqlCommand();
//         cmd.Connection = conn;
//         cmd.CommandText = "DELETE FROM wi_airports WHERE id = @id";
//         cmd.Parameters.AddWithValue("id", airportToDelete.id);
//         int numDeleted = cmd.ExecuteNonQuery();

//         if (numDeleted > 0)
//         {
//             SelectAllAirports(); 
//             return AirportDeletionError.NoError;
//         }
//         else
//         {
//             return AirportDeletionError.AirportNotFound;
//         }
//     }
// }