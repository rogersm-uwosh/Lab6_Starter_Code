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
    public EnterAirportDetailsPopup (bool isEdit)
    {
        this.isEdit = isEdit;
        InitializeComponent();
        this.calendar.MonthView.NumberOfVisibleWeeks = 6;
        Console.WriteLine("Popup Opened");
    }

    void OnCalendarSelectionChanged(object sender, EventArgs e)
    {
        dateVisted = calendar.SelectedDate ;
        Console.WriteLine(dateVisted.ToString()); //DELETE ME
    }
    
    //select rating
    private void Star_Clicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        {
            int starCount = Convert.ToInt32(button.CommandParameter);
            FillStars(starCount);
            rating = starCount;
        }
    }
    
    private void FillStars (int numStars) {
        starOne.Source = starTwo.Source = starThree.Source = starFour.Source = starFive.Source = greyStarPath;
        switch (numStars) 
        {
            case 5: starFive.Source = yellowStarPath;
            goto case 4;
            case 4: starFour.Source = yellowStarPath;
            goto case 3;
            case 3: starThree.Source = yellowStarPath;
            goto case 2;
            case 2: starTwo.Source = yellowStarPath;
            goto case 1;
            case 1: starOne.Source = yellowStarPath;
            break;
        }
    }

    async void AddAirport_Clicked(object sender, EventArgs e)
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
                    break;
            }
        }
        else
        {
            errorMessage = "Date is not selected";
        }
        IToast errorMessageToast = Toast.Make(errorMessage);
        errorMessageToast.Show();
    }
    void CancelAirportAdd_Clicked(object sender, EventArgs e)
    {
        Close();
    }


}
