using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Tiling;
using Mapsui.UI.Maui;
using Mapsui.UI.Maui.Extensions;
using Map = Mapsui.Map;

namespace Lab6_Starter;

/**
 * Lab 6 Team 5
 * Team members: Alex Robinson, Brady Blocksom, Matthew Steffens, and Zafeer Rahim
 * 
 * Description: A page with a map that can be accessed via a tab on the navigation bar
 * (This file's) Authors: Brady Blocksom and Alex Robinson
 * Date: 10/17/2024
 * 
 * Bugs: While technically fixed by a flag in MainTabbedPage.xaml, the tab bar was eating
 * most of the touch inputs, especially swiping, that the map needs to let you zoom and move
 * it around. That flag consequently disables the swipe interaction to change between tabs,
 * so with the current map we can either have a functional map or swiping to change tabs.
 * While not ideal, a workaround would likely be very confusing and/or messy when swiping
 * to change tabs likely wasn't a super important feature when you can simply click the buttons.
 * 
 * Reflection: This might not look like much, but there was a lot of trial and error with many
 * iterations that went into this lab. Adding a tab gave us some trouble since we weren't
 * immediately sure where the code for it needed to go, but it certainly ended up being valuable
 * practice for getting one to work since it didn't feel very intuitive at the start.
 * Adding a map was a struggle of its own since there are a ton of different options, most
 * requiring payment information even if they seem free or not making much sense in their usage,
 * eventually settling on making a placeholder image until a better alternative could be figured
 * out, which did happen with settling on MapsUI which is *actually* free. All in all, more time
 * went into figuring out how to actually do the work with the tools available to us than is
 * evident from the commits in this pull request.
 * 
 * Each team member helped out at various points during the lab, with Brady doing the initial
 * setup and working through the requirements and procedures for the lab, Alex building off that
 * and adding finishing touches later on, and Matthew and Zafeer providing ideas and effectively
 * being tech support during most of it. While not everyone submitted code, everyone contributed
 * to the progression of the lab by offering ideas, providing insight, and generally helping with
 * getting started with what needed doing during the time we met together for it.
 */
public partial class MapPage : ContentPage
{
	public MapPage()
	{
		InitializeComponent();
        
        MapControl mapControl = new();
        Map map = mapControl.Map;
        MyLocationLayer locationLayer = new(map) { IsCentered = false };

        // Initialize the map with OpenStreetMap's API and add it to MapPage's content
        map.Layers.Add(OpenStreetMap.CreateTileLayer());
        map.Layers.Add(locationLayer);
        
        MPoint mpoint = new(-88.4154, 44.2619);
        MPoint npoint = SphericalMercator.FromLonLat(mpoint.X, mpoint.Y).ToMPoint();
        map.Navigator.CenterOnAndZoomTo(npoint, map.Navigator.Resolutions[9]);

        MapGrid.Add(mapControl);
        locationLayer.UpdateMyLocation(npoint, true);
    }
}