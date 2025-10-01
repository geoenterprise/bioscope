using MobileApp.Services;
using Microsoft.Extensions.DependencyInjection;


namespace MobileApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder()
            .UseMauiApp<App>();

        return builder.Build();
    }
}

