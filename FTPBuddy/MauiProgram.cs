using FTPBuddy.Data;
using FTPBuddy.Pages;
using Microsoft.Extensions.Logging;

namespace FTPBuddy
{
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<AddNewServerPage>();
            builder.Services.AddTransient<ServersPage>();

            builder.Services.AddSingleton<FtpServerDatabase>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
