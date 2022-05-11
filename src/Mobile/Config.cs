namespace Microsoft.NetConf2021.Maui;

public static class Config
{
    public static bool ListenTogetherIsVisible => true;

    public static bool Desktop
    {
        get
        {
#if WINDOWS || MACCATALYST
            return true;
#else
            return false;
#endif
        }
    }

    public static string BaseWeb = $"https://dotnetconf2021-netpodcast.azurewebsites.net/listentogether";
    public static string APIUrl = $"https://podcastapica.gentlesea-ba4a72f3.canadacentral.azurecontainerapps.io/v1/";
    public static string ListenTogetherUrl = $"https://dotnetconf2021-netpodcast-hub.azurewebsites.net/listentogether";
}
