using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System;
using System.Collections.Generic;
using Map = Microsoft.Maui.Controls.Maps.Map;


namespace Lab6_Starter
{
    public partial class RoutingStrategies : TabbedPage
    {
        public RoutingStrategies()
        {
            InitializeComponent();

            // Fill data for ListView (Hannah)
            RoutesListView.ItemsSource = new List<Route>
            {
                new Route { ICAO = "KATW", City = "Appleton", Distance = 0 },
                new Route { ICAO = "KFLD", City = "Fond du Lac", Distance = 29 },
                new Route { ICAO = "KUNU", City = "Dodge County", Distance = 23 },
                new Route { ICAO = "KBUU", City = "Burlington", Distance = 47 },
                new Route { ICAO = "KATW", City = "Appleton", Distance = 95 }
            };

            // Set initial map location (Pachia)
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(44.2619, 88.4154), Distance.FromMiles(10)));
            //Content = map;
        }
    }

    // Get airport data (Hannah)
    public class Route
    {
        public string ICAO { get; set; }
        public string City { get; set; }
        public int Distance { get; set; }
    }
}
