using CommunityToolkit.Maui.Views;
using Google.Android.Material.Slider;
using System.Formats.Tar;

namespace Lab6_Starter;

public partial class EnterAirportDetailsPopup : Popup
{

    const string greyStarPath = "Resources/Images/ic_fluent_star_24_filled_grey.svg";
    const string yellowStarPath = "Resources/Images/ic_fluent_star_24_filled_yellow.svg";
    public EnterAirportDetailsPopup ()
    {
        InitializeComponent();
        this.calendar.MonthView.NumberOfVisibleWeeks = 6;
    }

    void CancelAirportAdd_Clicked ( object sender , EventArgs e )
    {
        Close();
    }

    //Select Rating
    private void StarOne_Clicked ( object sender , EventArgs e )
    {
        starOne.Source = yellowStarPath;
        starTwo.Source = starThree.Source = starFour.Source = starFive.Source = greyStarPath;
    }
    private void StarTwo_Clicked ( object sender , EventArgs e )
    {
        starOne.Source = starTwo.Source = yellowStarPath;
        starThree.Source = starFour.Source = starFive.Source = greyStarPath;
    }
    private void StarThree_Clicked ( object sender , EventArgs e )
    {
        starOne.Source = starTwo.Source = starThree.Source = yellowStarPath;
        starFour.Source = starFive.Source = greyStarPath;
    }
    private void StarFour_Clicked ( object sender , EventArgs e )
    {
        starOne.Source = starTwo.Source = starThree.Source = starFour.Source = yellowStarPath;
        starFive.Source = greyStarPath;
    }
    private void StarFive_Clicked ( object sender , EventArgs e )
    {
        starOne.Source = yellowStarPath;
        starTwo.Source = starThree.Source = starFour.Source = starFive.Source = yellowStarPath;
    }


}
