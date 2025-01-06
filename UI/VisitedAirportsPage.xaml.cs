using CommunityToolkit.Maui.Views;
using Lab6_Starter.Model;
namespace Lab6_Starter;

public partial class VisitedAirportsPage : ContentPage
{
    public VisitedAirportsPage()
    {
        InitializeComponent();

        // We've set the BindingContext for the entire page to be the BusinessLogic layer
        // So any control on the page can bind to the BusinessLogic layer
        // There's really only one control that needs to talk to the BusinessLogic layer, and that's the CollectionView

        BindingContext = MauiProgram.BusinessLogic;
    }



    // Various event handlers for the buttons on the main page

    void AddAirport_Clicked(System.Object sender, System.EventArgs e)
    {
        //Changed to a popup insert [Popup Team]
        var popup = new EnterAirportDetailsPopup(null);

        this.ShowPopup(popup);
    }

    async void DeleteAirport_Clicked(System.Object sender, System.EventArgs e)
    {
        VisitedAirport currentAirport = CV.SelectedItem as VisitedAirport;

        // Check if an airport is selected
        if (currentAirport == null)
        {
            // Show an alert if no airport is selected
            await DisplayAlert("Selection Error", "Please select an airport to delete.", "OK");
            return; // Exit the method
        }

        // Proceed to delete the selected airport

        AirportDeletionError result = await MauiProgram.BusinessLogic.DeleteAirport(currentAirport.Id);
        if (result != AirportDeletionError.NoError)
        {
            await DisplayAlert("Ruhroh", result.ToString(), "OK");
        }
    }


    void EditAirport_Clicked(System.Object sender, System.EventArgs e)
    {
        //Changed to a popup insert [Popup Team]
        VisitedAirport? currentAirport = CV.SelectedItem as VisitedAirport;
        string? currentAirportId = currentAirport.Id;
        var popup = new EnterAirportDetailsPopup(currentAirport);
        this.ShowPopup(popup);
    }

    void CalculateStatistics_Clicked(System.Object sender, System.EventArgs e)
    {
        String result = MauiProgram.BusinessLogic.CalculateStatistics();
        DisplayAlert("Your Progress", result.ToString(), "Good to know");
    }

    void Logout_Clicked(System.Object sender, System.EventArgs e)
    {
        MauiProgram.BusinessLogic.VisitedAirports.Clear(); // otherwise, when logging in again, 
        Application.Current!.MainPage = new LoginPage();
    }
}


