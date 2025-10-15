using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using MobileApp.Services; 
using MobileApp.Pages; 


namespace MobileApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder()
            .UseMauiApp<App>();

#if DEBUG && ANDROID
        // Bypass dev cert issues for https during debug on Android only (optional)
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
        };
        builder.Services.AddSingleton(new HttpClient(handler) { BaseAddress = new Uri(ApiConfig.BaseUrl) });
#else
        builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(ApiConfig.BaseUrl) });
#endif
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<SignupPage>();
        

        return builder.Build();
    }
}

