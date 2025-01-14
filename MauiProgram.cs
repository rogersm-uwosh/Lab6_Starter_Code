using CommunityToolkit.Maui;
using FWAPPA.Model;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Syncfusion.Maui.Core.Hosting;
using BusinessLogic = FWAPPA.Model.BusinessLogic;

namespace FWAPPA;

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
			.UseSkiaSharp()
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

