using System.Globalization;

namespace FWAPPA.Model;

public class AirportConvertor : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is WisconsinAirport airport)
        {
                return $"{airport.Id} - {airport.Name}";
        }

        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}