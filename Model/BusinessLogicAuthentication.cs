using Supabase.Gotrue;

//using CommunityToolkit.Maui.Core.Extensions;


namespace FWAPPA.Model;

public partial class BusinessLogic : IBusinessLogic
{
 public async Task<bool> AuthenticateUser(string email, string password)
    {
        // Implement your authentication logic here

       User? user = await db.AuthenticateUser(email, password);
       if(user != null){
        await GetVisitedAirports();
       }
     return user != null;
    }

    public async Task<bool> RegisterUser(string email, string password)
    {
        // Implement your registration logic here
        User? user = await db.RegisterUser(email, password);
        return user != null;
    }
}

