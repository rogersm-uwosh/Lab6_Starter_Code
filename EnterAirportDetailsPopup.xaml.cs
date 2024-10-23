using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Lab6_Starter.Model;
using Syncfusion.Maui.Calendar;
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
    private DateTime? dateVisited;
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
        dateVisited = calendar.SelectedDate ;
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

    private void FillStars(int numStars)
    {
        var stars = new[] { starOne, starTwo, starThree, starFour, starFive };

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].Source = (i < numStars) ? yellowStarPath : greyStarPath;
        }
    }
    /// <summary>
    /// Based on the isEdit boolean, 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    async void AddAirport_Clicked(object sender, EventArgs e)
    {
        string errorMessage;
        if (dateVisited != null)
        {
            id = IdEntry.Text;
            city = CityEntry.Text;
            if (isEdit)
            {
                try
                {
                    AirportEditError error = MauiProgram.BusinessLogic.EditAirport(id, city, (DateTime) dateVisited,
                    rating);
                    switch (error.ToString())
                    {
                        case "AirportNotFound":
                            errorMessage = $"There is no Airport with the id:{id}";
                            break;
                        case "InvalidFieldError":
                            errorMessage = "One of the field is invalid";
                            break;
                        case "DBEditError":
                            errorMessage = "Database error";
                            break;
                        default:
                            errorMessage = $"Successfully Edited Airport{id}";
                            Close();
                            break;
                    }
                } catch (Exception ex)
                {
                    errorMessage = ex.StackTrace;
                }
                
            }
            else
            {
                AirportAdditionError error = MauiProgram.BusinessLogic.AddAirport(id, city, (DateTime) dateVisited, rating);

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
                        break;
                }
            }
        }
            
        else
        {
            errorMessage = "Date is not selected";
        }
        IToast errorMessageToast = Toast.Make(errorMessage);
        await errorMessageToast.Show();
    }
    void CancelAirportAdd_Clicked(object sender, EventArgs e)
    {
        Close();
    }


}
