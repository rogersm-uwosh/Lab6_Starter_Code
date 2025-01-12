namespace FWAPPA.UI;

using CommunityToolkit.Maui.Views;
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
/// <summary>
/// Handles registration, dispensing with obvious input errors bdefore 
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
	public void Register_Clicked(System.Object sender, System.EventArgs e)
	{
	this.ShowPopup(new RegistrationPopup());
	}

	// /// <summary>
	// /// Closes the "Add New Airport" popup.
	// /// </summary>
	// /// <param name="sender">Sender</param>
	// /// <param name="args">Arguments</param>
	// public async void OnCancelClicked(System.Object sender, System.EventArgs args)
	// {
	// 	// await CloseAsync();
	// }

}