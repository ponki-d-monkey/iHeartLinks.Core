using System;

namespace iHeartLinks.Core
{
    public static class HypermediaServiceExtension
    {
        private const string SelfRel = "self";

        public static IHypermediaBuilder<TDocument> AddSelf<TDocument>(this IHypermediaService service, TDocument document)
            where TDocument : IHypermediaDocument
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var link = service.GetLink();

            document.AddLink(SelfRel, link);

            return new HypermediaBuilder<TDocument>(service, document);
        }

        public static IHypermediaBuilder<TDocument> Prepare<TDocument>(this IHypermediaService service, TDocument document)
            where TDocument : IHypermediaDocument
        {
            return new HypermediaBuilder<TDocument>(service, document);
        }
    }
}
