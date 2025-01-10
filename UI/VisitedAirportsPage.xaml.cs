using CommunityToolkit.Maui.Views;
using FWAPPA.Model;

namespace FWAPPA.UI;

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
    private void AddAirport_Clicked(object sender, EventArgs e)
    {
        // Changed to a popup insert [Popup Team]
        var popup = new EnterAirportDetailsPopup(null);
        this.ShowPopup(popup);
    }

    private async void DeleteAirport_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button { BindingContext: VisitedAirport currentAirport })
        {
            return;
        }

        bool isConfirmed = await DisplayAlert(
            "Confirm Deletion",
            $"Are you sure you want to delete '{currentAirport.Name}'?",
            "Yes",
            "No"
        );
        if (!isConfirmed)
        {
            return;
        }
        
        // Proceed to delete the selected airport
        AirportDeletionError result = await MauiProgram.BusinessLogic.DeleteAirport(currentAirport.Id);
        if (result != AirportDeletionError.NoError)
        {
            await DisplayAlert("Ruhroh", result.ToString(), "OK");
        }
    }

    void EditAirport_Clicked(object sender, EventArgs e)
    {
        //Changed to a popup insert [Popup Team]
        if (sender is not Button { BindingContext: VisitedAirport currentAirport })
        {
            return;
        }
        var popup = new EnterAirportDetailsPopup(currentAirport);
        this.ShowPopup(popup);
    }

    void CalculateStatistics_Clicked(object sender, EventArgs e)
    {
        String result = MauiProgram.BusinessLogic.CalculateStatistics();
        DisplayAlert("Your Progress", result, "Good to know");
    }

    void Logout_Clicked(object sender, EventArgs e)
    {
        MauiProgram.BusinessLogic.VisitedAirports.Clear(); // otherwise, when logging in again, 
        Application.Current!.MainPage = new LoginPage();
    }
}