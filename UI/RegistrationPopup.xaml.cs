namespace FWAPPA.UI;

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;


public partial class RegistrationPopup : Popup
{

	public RegistrationPopup()
	{
		InitializeComponent();
	}

	/// <summary>
	/// Handles registration. Note that we do not say exactly why registration failed, for security reasons
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public async void OnRegisterClicked(object? sender, EventArgs e)
	{
		Boolean success;
		if (PasswordENT.Text != ConfirmPasswordENT.Text) // passwords don't match => we didn't succeed
		{
			success = false;
		}
		else // passwords match, time to register the user
		{
			success = await MauiProgram.BusinessLogic.RegisterUser(EmailAddressENT.Text, PasswordENT.Text);
		}

		if (success)
		{
			await Toast.Make("Registration successful!").Show();
			await CloseAsync();
		}
		else
		{
			await Toast.Make("Registration failed! Please try again, making sure to use a unique email address").Show();
		}
	}

	public async void OnCancelClicked(object? sender, EventArgs e)
	{
		await CloseAsync();
	}


}