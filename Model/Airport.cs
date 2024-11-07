using System;
using System.ComponentModel;

namespace Lab6_Starter.Model;

[Serializable()]
public class Airport : INotifyPropertyChanged
{
    String id;
    String city;
    DateTime dateVisited;
    int rating;
    double longitude;
    double latitude;
    private string name;
    private string url;
    private double distance;

    public String Id
    {
        get { return id; }
        set
        {
            id = value;
            OnPropertyChanged(nameof(Id));
        }
    }

    public double Distance
    {
        get => distance;
        set
        {
            distance = value;
            OnPropertyChanged(nameof(Distance));
        }
    }

    public String City
    {
        get { return city; }
        set
        {
            city = value;
            OnPropertyChanged(nameof(City));
        }
    }

    public DateTime DateVisited
    {
        get { return dateVisited; }
        set
        {
            dateVisited = value;
            OnPropertyChanged(nameof(DateVisited));
        }
    }

    public int Rating
    {
        get { return rating; }
        set
        {
            rating = value;
            OnPropertyChanged(nameof(Rating));
        }
    }

    public double Latitude
    {
        get { return latitude; }
        set { latitude = value;
            OnPropertyChanged(nameof(Latitude));
        }
    }

    public double Longitude
    {
        get { return longitude; }
        set { longitude = value;
            OnPropertyChanged(nameof(Longitude));
        }
    }

    public Airport(String id, String city, DateTime dateVisited, int rating)
    {
        Id = id;
        City = city;
        DateVisited = dateVisited;
        Rating = rating;
        Latitude = 0.0;
        Longitude = 0.0;
    }

    public Airport(String id, double latitude, double longitude)
    {
        Id = id;
        City = "Appleton";
        DateVisited = DateTime.Now;
        Rating = 5;
        Latitude = latitude;
        Longitude = longitude;
    }


    public string Name
    {
        get => name;
        set
        {
            name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    public double Latitude
    {
        get => latitude;
        set
        {
            latitude = value;
            OnPropertyChanged(nameof(Latitude));
        }
    }

    public double Longitude
    {
        get => longitude;
        set
        {
            longitude = value;
            OnPropertyChanged(nameof(Longitude));
        }
    }

    public string Url
    {
        get => url;
        set
        {
            url = value;
            OnPropertyChanged(nameof(Url));
        }
    }

    // Constructor with five parameters
    public Airport(string id, string name, double latitude, double longitude, string url)
    {
        Id = id;
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        Url = url;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public override bool Equals(object obj)
    {
        var otherAirport = obj as Airport;
        return Id == otherAirport.Id;
    }

}
