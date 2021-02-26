namespace iHeartLinks.Core
{
    public interface IHypermediaService
    {
        Link GetLink();

        Link GetLink(string key, object args);
    }
}
