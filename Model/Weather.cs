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
    String airport;
    String metar;
    String taf;

    public String Airport
    {
        get { return airport; }
        set
        {
            airport = value;
            OnPropertyChanged(nameof(Airport));
        }
    }

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

    public Weather(String airport, String metar, String taf)
    {
        Airport = airport;
        Metar = metar;
        Taf = taf;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
