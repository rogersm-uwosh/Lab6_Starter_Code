namespace FWAPPA.UI;

public partial class MainTabbedPage : TabbedPage
{

	Boolean IsMapTabVisible {get;set;}
	
		public MainTabbedPage()
	{
		BindingContext = this;
		InitializeComponent();
		NavigationPage.SetHasNavigationBar(this, false);
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

	}

// protected override void OnCurrentPageChanged()
// {
//     base.OnCurrentPageChanged();

//     var selectedTab = CurrentPage; // CurrentPage gives the currently selected tab
//     if (selectedTab != null)
//     {
//         Console.WriteLine($"Selected Tab: {selectedTab.Title}");

//         // Example: Perform actions based on the selected tab
//         if (selectedTab is NavigationPage navPage && navPage.CurrentPage is MapPageSimple mapPage)
//         {
//            Console.WriteLine("Got to here"); 
// 		   mapPage.InitializeMap();
//         }
//     }
// }
}
