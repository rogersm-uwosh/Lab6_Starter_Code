using System.ComponentModel;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

/*
This used to have a property called Distance, duplicating 
the Distance property in VisitedAirport
That's now exclusiveluy in the WisconsinAirport class
*/
namespace FWAPPA.Model;

    [Serializable]
    [Table("visited_airports")]
    public class VisitedAirport : BaseModel, INotifyPropertyChanged
    {
        private string id;
        private string name;
        private DateTime dateVisited;
        private int rating;
 
        private string userId;

        [Column("id")]
        [PrimaryKey("id")]
        public string Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        [Column("name")]
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        [Column("date_visited")]
        public DateTime DateVisited
        {
            get => dateVisited;
            set
            {
                dateVisited = value;
                OnPropertyChanged(nameof(DateVisited));
            }
        }

        [Column("rating")]
        public int Rating
        {
            get => rating;
            set
            {
                rating = value;
                OnPropertyChanged(nameof(Rating));
            }
        }

        [PrimaryKey("user_id")]
        [Column("user_id")]
        public string UserId {
            get => userId;
            set {
                userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }

        // Required by Supabase
        public VisitedAirport()
        {

        }

        // Constructor with parameters for ID, city, date visited, and rating
        public VisitedAirport(string id, string name, DateTime dateVisited, int rating)
        {
            Id = id;
            Name = name;
            DateVisited = dateVisited;
            Rating = rating;
        }

        public VisitedAirport(String id, String name)
        {
            Id = id;
            Name = name;
            DateVisited = DateTime.Now;
            Rating = 5;
        }


        override
        public String ToString()
        {
            return $"{Id}, {Name}, {DateVisited}, {Rating}";
        }

        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool Equals(object? obj)
        {
            return obj is VisitedAirport otherAirport && Id == otherAirport.Id;
        }

        public override int GetHashCode()
        {
            return Id!.GetHashCode();
        }
    }

    public class VisitedAirportEqualityComparer : IEqualityComparer<VisitedAirport>
    {
        public bool Equals(VisitedAirport? x, VisitedAirport? y)
        {
            if (x == null || y == null)
            {
                return false;
            }
            return x.Equals(y);
        }

        public int GetHashCode(VisitedAirport obj)
        {
            return obj.Id.GetHashCode();
        }
    }

