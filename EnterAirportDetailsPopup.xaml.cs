using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Lab6_Starter.Model;
using System.Formats.Tar;
using System.Runtime.InteropServices;

namespace Lab6_Starter;

public partial class EnterAirportDetailsPopup : Popup
{
    private bool isEdit;
    const string greyStarPath = "Resources/Images/ic_fluent_star_24_filled_grey.svg";
    const string yellowStarPath = "Resources/Images/ic_fluent_star_24_filled_yellow.svg";
    private string id = "";
    private string city = "";
    private DateTime? dateVisted;
    private int rating = 0;
    public EnterAirportDetailsPopup (Airport airport)
    {
        this.isEdit = isEdit;
        InitializeComponent();
        this.Calendar.MonthView.NumberOfVisibleWeeks = 6;
        Console.WriteLine("Popup Opened");
        if (airport != null)
        {
            IdEntry.Text = airport.Id;
            CityEntry.Text = airport.City;
            Calendar.SelectedDate = airport.DateVisited;
            FillStars(airport.Rating);
        }
    }
    

    void OnCalendarSelectionChanged(object sender, EventArgs e)
    {
        dateVisted = Calendar.SelectedDate ;
        Console.WriteLine(dateVisted.ToString()); //DELETE ME
    }
    
    //select rating
    private void Star_Clicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        {
            int starCount = Convert.ToInt32(button.CommandParameter);
            FillStars(starCount);
        }
    }
    
    private void FillStars (int numStars) {
        starOne.Source = starTwo.Source = starThree.Source = starFour.Source = starFive.Source = greyStarPath;
        var stars = new[] { starOne, starTwo, starThree, starFour, starFive };
        rating = numStars;
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].Source = (i < numStars) ? yellowStarPath : greyStarPath;
        }
    }

    async void Ok_Clicked(object sender, EventArgs e)
    {
        string errorMessage;
        if (dateVisted != null)
        {
            id = IdEntry.Text;
            city = CityEntry.Text;

            AirportAdditionError error = MauiProgram.BusinessLogic.AddAirport(id, city, (DateTime) dateVisted, rating);
            
            switch (error.ToString())
            {
                case "InvalidIdLength":
                    errorMessage = "Id length is not between 3 and 4";
                    break;
                case "InvalidCityLength":
                    errorMessage = "City length is not between 1 and 25";
                    break;
                case "InvalidRating":
                    errorMessage = "Rating is not selected";
                    break;
                case "InvalidDate":
                    errorMessage = "Date is invalid";
                    break;
                case "DuplicateAirportId":
                    errorMessage = "Airport id is already used";
                    break;
                case "DBAdditionError":
                    errorMessage = "Database error";
                    break;
                default:
                    errorMessage = $"Successfully Added Airport{id}";
                    Close();
                    return;
            }
        }
        else
        {
            errorMessage = "Date is not selected";
        }
        IToast errorMessageToast = Toast.Make(errorMessage);
        errorMessageToast.Show();
    }
    void Cancel_Clicked(object sender, EventArgs e)
    {
        Close();
    }


}
