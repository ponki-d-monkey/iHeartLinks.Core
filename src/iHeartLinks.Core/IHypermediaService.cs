namespace iHeartLinks.Core
{
    public interface IHypermediaService
    {
        string GetCurrentUrl();

        string GetUrl(string key);

        string GetUrl(string key, object args);

        string GetCurrentUrlTemplate();

        string GetUrlTemplate(string key);

        string GetCurrentMethod();

        string GetMethod(string key);
    }
}
