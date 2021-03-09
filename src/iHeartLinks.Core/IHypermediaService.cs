namespace iHeartLinks.Core
{
    public interface IHypermediaService
    {
        Link GetLink();

        Link GetLink(LinkRequest request);
    }
}
