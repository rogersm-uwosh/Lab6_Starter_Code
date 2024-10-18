using CommunityToolkit.Maui.Views;
namespace Lab6_Starter;

public partial class SimplePopup : Popup
{
	public SimplePopup()
	{
		InitializeComponent();
	}

	void CancelAirportAdd_Clicked(object sender, EventArgs e) {
		Close();
	}

}
