using System.Collections.ObjectModel;

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

    public User? CurrentUser(){
        return supabaseClient!.Auth.CurrentUser;
    }
}
