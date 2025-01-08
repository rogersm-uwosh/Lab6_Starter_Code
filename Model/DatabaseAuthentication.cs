using Supabase.Gotrue;

namespace FWAPPA.Model;

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
