using System;

namespace iHeartLinks.Core
{
    internal class HypermediaBuilder<TDocument> : IHypermediaBuilder<TDocument>
        where TDocument : IHypermediaDocument
    {
        public HypermediaBuilder(
            IHypermediaService service,
            TDocument document)
        {
            Service = service ?? throw new ArgumentNullException(nameof(service));
            Document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public IHypermediaService Service { get; }

        public TDocument Document { get; }

        public IHypermediaBuilder<TDocument> AddLink(string rel, Link link)
        {
            if (string.IsNullOrWhiteSpace(rel))
            {
                throw new ArgumentException($"Parameter '{nameof(rel)}' must not be null or empty.");
            }

            if (link == null)
            {
                throw new ArgumentNullException(nameof(link));
            }

            Document.AddLink(rel, link);

            return this;
        }
    }
}
