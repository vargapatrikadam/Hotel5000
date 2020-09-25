namespace BlazorWebApp
{
    public class BaseUrlConfiguration
    {
        public const string CONFIG_NAME = "baseUrls";

        public string ApiBase { get; set; } = "https://localhost:5000/api/";
        public string WebBase { get; set; }
    }
}
