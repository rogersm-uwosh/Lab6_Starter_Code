using Mapsui.Tiling;
using Mapsui.UI.Maui;

namespace Lab6_Starter;

public partial class MapPage : ContentPage
{
	public MapPage()
	{
		InitializeComponent();

        MapControl mapControl = new();
        mapControl.Map?.Layers.Add(OpenStreetMap.CreateTileLayer());
        Content = mapControl;
    }
}