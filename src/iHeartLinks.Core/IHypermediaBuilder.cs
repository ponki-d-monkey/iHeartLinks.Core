namespace iHeartLinks.Core
{
    public interface IHypermediaBuilder<out TDocument>
        where TDocument : IHypermediaDocument
    {
        TDocument Document { get; }

        IHypermediaService Service { get; }

        IHypermediaBuilder<TDocument> AddLink(string rel, Link link);
    }
}
