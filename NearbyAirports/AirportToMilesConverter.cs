using System.Globalization;
using Lab6_Starter.Model;

namespace Lab2_Solution.NearbyAirports;

public class AirportToMilesConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Airport airport)
        {
            switch (airport.Id)
            {
                case "KFLD":
                    return "10 mi";
                case "KMTW":
                    return "15 mi";
                case "79C":
                    return "18 mi";
                case "KUNU":
                    return "64 mi";
            }
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}