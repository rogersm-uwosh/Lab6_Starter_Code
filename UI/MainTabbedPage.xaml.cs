using CommunityToolkit.Maui;
using Syncfusion.Maui.Calendar;
namespace Lab6_Starter;

public partial class MainTabbedPage : TabbedPage
{
	public MainTabbedPage()
	{
		InitializeComponent();
		NavigationPage.SetHasNavigationBar(this, false);
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

	}
}
