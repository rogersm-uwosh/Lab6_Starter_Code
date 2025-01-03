using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Lab6_Starter.Model;
using CommunityToolkit.Maui;
using Syncfusion.Maui.Core.Hosting;
using BusinessLogic = Lab6_Starter.Model.BusinessLogic;

namespace Lab6_Starter;

public static class MauiProgram
{

	public static IBusinessLogic BusinessLogic = new BusinessLogic(new DatabaseSupa());

	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.ConfigureSyncfusionCore(); // for selecting date with calendar
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			//[[ Alex Robinson - Dependency of MapsUI
			.UseSkiaSharp(true)
			//]]
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			}).UseMauiMaps();


#if DEBUG
		builder.Logging.AddDebug();
#endif


		return builder.Build();
	}
}

