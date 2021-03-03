namespace iHeartLinks.Core
{
    public interface IHypermediaService
    {
        Link GetLink();

        Link GetLink(string request, object args);
    }
}
