namespace Lab6_Starter;

using Lab6_Starter.Model;

using CommunityToolkit.Maui.Views;
using Supabase.Gotrue;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// This is the code-behind for the LoginPage.xaml
/// </summary>
public partial class LoginPage : ContentPage
{

	public LoginPage()
	{
		InitializeComponent();
		UserIdENT.Text = "mprogers@mac.com";
		PasswordENT.Text = "password1234";
	}

	async void Login_Clicked(System.Object sender, System.EventArgs e)
	{
		if (String.IsNullOrEmpty(UserIdENT.Text) || String.IsNullOrEmpty(PasswordENT.Text))
		{
			await DisplayAlert("Credentials missing", "Please supply both an email address and password", "OK");
			return;
		}
		Boolean loggedIn = await MauiProgram.BusinessLogic.AuthenticateUser(UserIdENT.Text, PasswordENT.Text);
		if (loggedIn)
		{
			App.Current.MainPage = new NavigationPage(new MainTabbedPage());
			
		}
		else
		{
			await DisplayAlert("Error while logging in", "Unable to log in: please double-check your email and password", "OK");
		}

	}
	async void Register_Clicked(System.Object sender, System.EventArgs e)
	{



	}

}