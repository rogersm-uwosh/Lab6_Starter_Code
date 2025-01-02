﻿using System.Collections.ObjectModel;

using Npgsql; // To install this, add dotnet add package Npgsql 
using Supabase;
using Supabase.Gotrue;


using CsvHelper.Configuration;
using System.Runtime.Loader;
using System.Data;
using System.Text.Json;
using System.IO.Enumeration;

using Lab6_Starter;

namespace Lab6_Starter.Model;


public partial class DatabaseSupa : IDatabaseSupa
{
    private static System.Random rng = new();
    private static string url = "https://wtplxsypjoxsybqgygda.supabase.co";
    private static string key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Ind0cGx4c3lwam94c3licWd5Z2RhIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MjQxODc2MzYsImV4cCI6MjAzOTc2MzYzNn0.T4gIDo7X9C6T0eAjP594cIKNqnGwr6P17CZt8NWyy3M";

    public Supabase.Client? supabaseClient;

    ObservableCollection<VisitedAirport> visitedAirports = new();
    ObservableCollection<WisconsinAirport> wisconsinAirports;

    Dictionary<String, WisconsinAirport> wisconsinAirportsMap = new(); // for looking up airports by icao identifier

    public DatabaseSupa()
    {
        InitializeSupabaseClient();
        PopulateWisconsinAirports();
    }

    private async void InitializeSupabaseClient()
    {
        supabaseClient = new Supabase.Client(url, key);
        await supabaseClient.InitializeAsync();
    }

    public const String wiAirportsDictionaryFilename = "WIAirports.json";

    /// <summary>
    /// This reads in the map (dictionary) of all Wisconsin airports and converts it into an ObservableCollection<Airport>
    /// This should only need to be done once at launch time. Note that we really will want both a map and a observable collection
    /// Should we store both in the app bundle? That might be better -- an infintesimal amount of extra storage and
    /// (very slightly) faster bootup time
    /// </summary>
    /// <returns>an ObservableCollection<Airport>, representing all Wisconsin airports</returns>
    private async void PopulateWisconsinAirports()
    {
        wisconsinAirports = [];
        Dictionary<String, WisconsinAirport> wiAirportsDict = await ReadWisconsinAirportsMap(wiAirportsDictionaryFilename);

        foreach (KeyValuePair<String, WisconsinAirport> kvp in wiAirportsDict)
        {
            WisconsinAirport wiAirport = kvp.Value;
            wisconsinAirports.Add(new WisconsinAirport(wiAirport.Id, wiAirport.Name, wiAirport.Latitude, wiAirport.Longitude, wiAirport.Url));
        }
    }

    /// <summary>
    /// This reads in a map (Dictionary<String, WisconsinAirport>)stored in JSON in the app bundle
    /// </summary>
    /// <param name="filename"location of the WisconsinAirports map></param>
    /// <returns>A map of Wisconsin airports, indexed by id (icao identifier)</returns>
    private async Task<Dictionary<String, WisconsinAirport>> ReadWisconsinAirportsMap(String filename = wiAirportsDictionaryFilename)
    {
        try
        {
            using Stream stream = await FileSystem.OpenAppPackageFileAsync(filename);
            using StreamReader reader = new StreamReader(stream);
            string jsonAirports = await reader.ReadToEndAsync();

            return JsonSerializer.Deserialize<Dictionary<String, WisconsinAirport>>(jsonAirports)!;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading Wisconsin Airports: {ex.Message}");
            return null;
        }

    }

    /// Fun fact: for the following to work, with row level security, there must be policies in place for each of the
    /// SQL commands, otherwise things appear to silently fail. [There might be a log entry somewhere that shows the
    /// problem, but it's not obvious...]

    /// <summary>
    /// Fills our local Airports ObservableCollection with all the airports in the database
    /// We don't cache the airports in the database, so we have to go to the database to get them
    /// This is one of those "tradeoffs" we have to make when we use a database
    /// TODO: Implement cacheing?? Learn how to spell cacheing?
    /// </summary>
    /// <returns></returns>
    public async Task<ObservableCollection<VisitedAirport>> SelectAllVisitedAirports()
    {
        try
        {
            visitedAirports.Clear();

            var response = await supabaseClient!.From<VisitedAirport>().Get();

            foreach (var airport in response.Models)
            {
                visitedAirports.Add(airport);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return visitedAirports;
    }


    // Finds the visited airport with the given id, null if not found
    public async Task<VisitedAirport?> SelectAirport(String id)
    {
        VisitedAirport? airportToSelect = await supabaseClient!.From<VisitedAirport>().Where(x => x.Id == id).Single();

        return airportToSelect;
    }



    // This inserts a airport into the database, or prints out an error message and returns false if the airport already exists
    // Notice the try-catch block, how could INSERT possibly fail?
    public async Task<AirportAdditionError> InsertAirport(VisitedAirport airportToInsert)
    {
        try
        {
            airportToInsert.UserId = supabaseClient!.Auth.CurrentUser?.Id;
            await supabaseClient!.From<VisitedAirport>().Insert(airportToInsert);
            return AirportAdditionError.NoError;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return AirportAdditionError.DBAdditionError;
        }
    }


    // The UI >> BusinessLogic >> Database the currently selected airport (selected in the CollectionView) for upating 
    public async Task<AirportEditError> UpdateAirport(VisitedAirport airportToUpdate)
    {
        try
        {
            airportToUpdate.UserId = supabaseClient!.Auth.CurrentUser?.Id;
            await supabaseClient!.From<VisitedAirport>().Update(airportToUpdate);
            return AirportEditError.NoError;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return AirportEditError.DBEditError;
        }
    }

    public async Task<AirportDeletionError> DeleteAirport(VisitedAirport airportToDelete)
    {
        try
        {
            airportToDelete.UserId = supabaseClient!.Auth.CurrentUser?.Id;
            await supabaseClient!.From<VisitedAirport>().Where(x => x.Id == airportToDelete.Id && x.UserId == airportToDelete.UserId).Delete();
            return AirportDeletionError.NoError;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return AirportDeletionError.DBDeletionError;
        }
    }


    /// <summary>
    /// This returns the ObservableCollection of all Wisconsin Airports
    /// This method used to do a lot more, but since we now populate wisconsinAirports in the constructor, all it has to do is return it. 
    /// We retain this method since it is used elsewhere
    /// </summary>
    /// <returns>an ObservableCollection<Airport>, representing all Wisconsin airports</returns>
    public ObservableCollection<WisconsinAirport> GetAllWisconsinAirports()
    {
        return wisconsinAirports;
    }





    public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        var earthRadius = 3440.065; // Radius of the earth in nautical miles
        var dLat = (lat2 - lat1) * (Math.PI / 180);
        var dLon = (lon2 - lon1) * (Math.PI / 180);
        var a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return earthRadius * c; // Distance in nautical miles
    }

    public WisconsinAirport SelectAirportByCode(string airportCode)
    {
        return wisconsinAirportsMap[airportCode];
    }


    public ObservableCollection<WisconsinAirport> GetWisconsinAirportsWithinDistance(double userLat, double userLon, double maxDistanceKm)
    {
        ObservableCollection<WisconsinAirport> nearbyAirports = new();
        ObservableCollection<WisconsinAirport> allAirports = GetAllWisconsinAirports();

        foreach (WisconsinAirport airport in allAirports)
        {
            double distance = CalculateDistance(userLat, userLon, (double)airport.Latitude, (double)airport.Longitude);
            if (distance <= maxDistanceKm)
            {
                airport.Distance = distance; // Set the calculated distance
                nearbyAirports.Add(airport);
            }
        }
        return nearbyAirports;
    }





    // Fetches the password from the user secrets store (um, this works in VS, but not in the beta of VSC's C# extension)
    // This assumes the NuGet package is installed -- dotnet add package Microsoft.Extensions.Configuration.UserSecrets
    static String FetchPassword()
    {
        // IConfiguration config = new ConfigurationBuilder().AddUserSecrets<Database>().Build();
        // return config["CockroachDBPassword"] ?? "thQ0vyMPC96yfUpkjY4dBg"; // if it can't find the password, returns ... the password (this works in VS, not VSC) 
        return "thQ0vyMPC96yfUpkjY4dBg";
    }


    public async void ProcessUserRequest(string email, string password)
    {
        Console.Write("Do you wish to register or authenticate? (r/a): ");
        String? choice = Console.ReadLine();

        if (choice == "r" || choice == "R")
        {
            var user = await RegisterUser(email, password);
        }
        else if (choice == "a" || choice == "A")
        {
            var user = await AuthenticateUser(email, password);
            if (user == null)
            {
                Environment.Exit(-1); // bail here ()
            }
        }
        else
        {
            Console.WriteLine("Invalid choice.");
            Environment.Exit(-1);
        }
    }

    public async Task<User?> AuthenticateUser(string email, string password)
    {
        try
        {
            var session = await supabaseClient!.Auth.SignIn(email, password);
            return session?.User; // 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Authentication failed -- {ex.Message}");
            return null;
        }
    }

    public async Task<User?> RegisterUser(string email, string password)
    {
        try
        {
            var session = await supabaseClient!.Auth.SignUp(email, password);
            return session?.User;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Registration failed -- {ex.Message}");
            return null;
        }
    }
}
