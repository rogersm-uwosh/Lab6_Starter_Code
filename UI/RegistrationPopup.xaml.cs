namespace FWAPPA;

using CommunityToolkit.Maui.Views;


public partial class RegistrationPopup : Popup
{

	public RegistrationPopup()
	{
		InitializeComponent();
	}

/// <summary>
/// Handles registration, dispensing with obvious input errors bdefore 
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
	public void OnRegisterClicked(System.Object sender, System.EventArgs e)
	{
		
	}

	/// <summary>
	/// Closes the "Add New Airport" popup.
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="args">Arguments</param>
	public void OnCancelClicked(System.Object sender, System.EventArgs args)
	{
		
	}
}