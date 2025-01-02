using System.Globalization;

namespace Lab6_Starter.Model.NearbyAirports;

public interface IValueConverter
{
    object Convert(object value, Type targetType, object parameter, CultureInfo culture);
    object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
}