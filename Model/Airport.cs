using System;
using System.ComponentModel;

namespace Lab6_Starter.Model
{
    [Serializable()]
    public class Airport : INotifyPropertyChanged
    {
        private string id;
        private string city;
        private DateTime dateVisited;
        private int rating;
        private double longitude;
        private double latitude;
        private string name;
        private string url;
        private double distance;

        public string Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string City
        {
            get => city;
            set
            {
                city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        public DateTime DateVisited
        {
            get => dateVisited;
            set
            {
                dateVisited = value;
                OnPropertyChanged(nameof(DateVisited));
            }
        }

        public int Rating
        {
            get => rating;
            set
            {
                rating = value;
                OnPropertyChanged(nameof(Rating));
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

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
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

        public double Distance
        {
            get => distance;
            set
            {
                distance = value;
                OnPropertyChanged(nameof(Distance));
            }
        }

        // Constructor with parameters for ID, city, date visited, and rating
        public Airport(string id, string city, DateTime dateVisited, int rating)
        {
            Id = id;
            City = city;
            DateVisited = dateVisited;
            Rating = rating;
            Latitude = 0.0;
            Longitude = 0.0;
        }

    public Airport(String id, String city, double latitude, double longitude)
        {
            Id = id;
        City = city;
            DateVisited = DateTime.Now;
            Rating = 5;
            Latitude = latitude;
            Longitude = longitude;
        }

        // Constructor with parameters for ID, name, latitude, longitude, and URL
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
            return obj is Airport otherAirport && Id == otherAirport.Id;
        }
    }

    public class AirportEqualityComparer : IEqualityComparer<Airport> {
        public bool Equals(Airport x, Airport y) {
            return x.Equals(y);
        }

        public int GetHashCode(Airport obj) {
            return obj.Id.GetHashCode();
        }
    }

}