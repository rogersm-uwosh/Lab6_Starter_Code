using CommunityToolkit.Maui.Views;
namespace Lab6_Starter;

public partial class SimplePopup : CommunityToolkit.Maui.Views.Popup
{
	public SimplePopup()
	{
		InitializeComponent();
	}

	void CancelAirportAdd_Clicked(System.Object sender, System.EventArgs e) {
		Close();
	}

}
