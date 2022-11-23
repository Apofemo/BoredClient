using MudBlazor;
using MudBlazor.Services;

namespace BoredClient;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMudServices(config =>
		{
			config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;

			config.SnackbarConfiguration.PreventDuplicates = true;
			config.SnackbarConfiguration.NewestOnTop = false;
			config.SnackbarConfiguration.ShowCloseIcon = false;
			config.SnackbarConfiguration.VisibleStateDuration = 1300;
			config.SnackbarConfiguration.HideTransitionDuration = 300;
			config.SnackbarConfiguration.ShowTransitionDuration = 300;
			config.SnackbarConfiguration.SnackbarVariant = Variant.Text;
			config.SnackbarConfiguration.MaximumOpacity = 100;
			config.SnackbarConfiguration.MaxDisplayedSnackbars = 2;
		});

		builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

		builder.Services.AddMudServices();

		return builder.Build();
	}
}
