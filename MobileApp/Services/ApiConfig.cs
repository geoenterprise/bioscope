namespace MobileApp.Services
{
    public static class ApiConfig
    {
#if DEBUG
#if ANDROID
        // Android emulator -> host machine “localhost”
        public const string BaseUrl = "http://10.0.2.2:5000/";
#else
        public const string BaseUrl = "https://localhost:5001/";
#endif
#else

        public const string BaseUrl = "https://bioscopeapi.onrender.com/";
#endif
    }
}
