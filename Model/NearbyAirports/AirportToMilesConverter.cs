using System.Globalization;

namespace FWAPPA.Model.NearbyAirports;

/// <summary>
/// Alexander Johnston
/// </summary>

public class AirportToMilesConverter: IValueConverter
{

    private static readonly Dictionary<string, int> _idToMiles = new();
    
    public static void ConvertAll(Dictionary<string, int> idToMiles)
    {
        _idToMiles.Clear();
        foreach (var (id, miles) in idToMiles)
        {
            _idToMiles[id] = miles;
        }
    }
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Console.WriteLine("Greetings from Convert()");
        if (value is WisconsinAirport airport)
        {
            Console.WriteLine($"Processing {airport.Id}");
            return _idToMiles[airport.Id];
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}