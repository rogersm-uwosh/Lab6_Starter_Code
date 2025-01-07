namespace Lab6_Starter;

/*
 * This class is the background functionality/methods for the resources page.
 * This class references an object created in Link.cs in order to allow the hyperlinks to function.
 * Author: Krystal Schneider & Taylor Showalter
 * Date: October 30, 2024
 */

public partial class Resources : ContentPage
{
    public Resources()
    {
        InitializeComponent();
        LoadLinks(); // Load the list of links when the page is initialized
    }

    // Method to create and set the links as the ItemsSource for the CollectionView
    private void LoadLinks()
    {
        // Creating a list of Link objects to represent resources
        var links = new List<Link>
        {
            new Link("Department of Transportation Home", "https://wisconsindot.gov/Pages/Home.aspx"),
            new Link("Fly Wisconsin Pilot Information", "https://wisconsindot.gov/Pages/travel/air/pilot-info/flywi.aspx"),
            new Link("Wisconsin Airport Management Association", "https://wiama.org/"),
            new Link("Aviation Education Museums", "https://wisconsindot.gov/Pages/doing-bus/aeronautics/education/aved-museums.aspx"),
            new Link("Fly Wisconsin Registration Form", "https://forms.office.com/g/PEKhtVaTxe"),
            new Link("Fly Wisconsin Rules", "https://wisconsindot.gov/Pages/travel/air/pilot-info/flywi-rules.aspx")
        };

        LinksCollectionView.ItemsSource = links;
    }

    private async void OnLinkTapped(object sender, EventArgs e)
    {
        // Check if the sender is a Label and its BindingContext is of type Link
        if (sender is Label label && label.BindingContext is Link link)
        {
            // Use the Launcher to open the URL in the default browser
            await Launcher.Default.OpenAsync(link.Url);
        }
    }

    void Logout_Clicked(System.Object sender, System.EventArgs e)
    {
        MauiProgram.BusinessLogic.VisitedAirports.Clear(); // otherwise, when logging in again, Bad Things happen
        Application.Current!.MainPage = new LoginPage();
    }
}
