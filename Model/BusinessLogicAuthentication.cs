using Supabase.Gotrue;

//using CommunityToolkit.Maui.Core.Extensions;


namespace FWAPPA.Model;

public partial class BusinessLogic : IBusinessLogic
{
 public async Task<bool> AuthenticateUser(string username, string password)
    {
        // Implement your authentication logic here

       User? user = await db.AuthenticateUser(username, password);
        
     return user != null;
    }

    public async Task<bool> RegisterUser(string username, string password)
    {
        // Implement your registration logic here
        return await Task.FromResult(true);
    }
}

