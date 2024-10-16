using System.Globalization;

namespace Lab2_Solution.NearbyAirports;

/// <summary>
/// Alexander Johnston wrote this class
///
/// Converts boolean to a red circle or green circle image source.
/// </summary>
public class AirportVisitedConvertor : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isActive)
        {
            return isActive
                ? "green_circle.png"
                : "red_circle.png";
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
    
}