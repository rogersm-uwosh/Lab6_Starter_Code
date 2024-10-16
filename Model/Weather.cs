using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Lab6_Starter.Model;

[Serializable()]
public class Weather : INotifyPropertyChanged
{
    String metar;
    String taf;

    public String Metar 
    {
        get { return metar; }
        set
        {
            metar = value;
            OnPropertyChanged(nameof(Metar));
        }
    }

    public String Taf
    {
        get { return taf; }
        set
        {
            taf = value;
            OnPropertyChanged(nameof(Taf));
        }
    }

    public Weather(String metar, String taf)
    {
        Metar = metar;
        Taf = taf;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
