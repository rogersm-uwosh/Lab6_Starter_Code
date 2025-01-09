namespace FWAPPA.UI;

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
