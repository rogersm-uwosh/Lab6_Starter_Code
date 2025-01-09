using System.Globalization;

namespace FWAPPA.Model.NearbyAirports;

/// <summary>
/// Alexander Johnston wrote this class
///
/// Converts boolean to a red circle or green circle image source.
/// </summary>
public class AirportVisitedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not WisconsinAirport airport) return null;
        bool isVisited = false;
        switch (airport.Id)
        {
            case "KFLD":
                isVisited = true;
                break;
            case "KMTW":
                isVisited = true;
                break;
            case "KUNU":
                isVisited = true;
                break;
        }
        return isVisited
            ? "green_circle.png"
            : "red_circle.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}