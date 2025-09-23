using MobileApp.Services;

namespace MobileApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder()
            .UseMauiApp<App>();

        return builder.Build();


#if ANDROID
        var baseUri = new Uri("https://10.0.2.2:7071/");
#else
        var baseUri = new Uri("https://localhost:7071/");
#endif

        builder.Services.AddHttpClient<OrganismApiService>(c =>
        {
            c.BaseAddress = baseUri;
        });

        return builder.Build();
    }
}

//using Microsoft.Extensions.Logging;

//namespace MobileApp
//{
//    public static class MauiProgram
//    {
//        public static MauiApp CreateMauiApp()
//        {
//            var builder = MauiApp.CreateBuilder();
//            builder
//                .UseMauiApp<App>()
//                .ConfigureFonts(fonts =>
//                {
//                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
//                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
//                });

//#if DEBUG
//    		builder.Logging.AddDebug();
//#endif

//            return builder.Build();
//        }
//    }
//}
