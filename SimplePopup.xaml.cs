using CommunityToolkit.Maui.Views;
using System.Formats.Tar;
namespace Lab6_Starter;

public partial class SimplePopup : Popup
{

    const string greyStarPath = "Resources/Images/ic_fluent_star_24_filled_grey.svg";
    const string yellowStarPath = "Resources/Images/ic_fluent_star_24_filled_yellow.svg";
    public SimplePopup()
	{
		InitializeComponent();
        this.calendar.MonthView.NumberOfVisibleWeeks = 6;
    }

	void CancelAirportAdd_Clicked(object sender, EventArgs e) {
		Close();
	}
    private void StarOne_Clicked(object sender, EventArgs e)
    {
        starOne.Source = yellowStarPath;
        starTwo.Source = greyStarPath;
        starThree.Source = greyStarPath;
        starFour.Source = greyStarPath;
        starFive.Source = greyStarPath;
    }
    private void StarTwo_Clicked(object sender, EventArgs e)
    {
        starOne.Source = yellowStarPath;
        starTwo.Source = yellowStarPath;
        starThree.Source = greyStarPath;
        starFour.Source = greyStarPath;
        starFive.Source = greyStarPath;
    }
    private void StarThree_Clicked(object sender, EventArgs e)
    {
        starOne.Source = yellowStarPath;
        starTwo.Source = yellowStarPath;
        starThree.Source = yellowStarPath;
        starFour.Source = greyStarPath;
        starFive.Source = greyStarPath;
    }
    private void StarFour_Clicked(object sender, EventArgs e)
    {
        starOne.Source = yellowStarPath;
        starTwo.Source = yellowStarPath;
        starThree.Source = yellowStarPath;
        starFour.Source = yellowStarPath;
        starFive.Source = greyStarPath;
    }
    private void StarFive_Clicked(object sender, EventArgs e)
    {
        starOne.Source = yellowStarPath;
        starTwo.Source = yellowStarPath;
        starThree.Source = yellowStarPath;
        starFour.Source = yellowStarPath;
        starFive.Source = yellowStarPath;
    }
    

}
