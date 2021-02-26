using System;

namespace iHeartLinks.Core
{
    public static class HypermediaBuilderExtension
    {
        public static IHypermediaBuilder<TDocument> AddLink<TDocument>(this IHypermediaBuilder<TDocument> builder, string rel, string href)
            where TDocument : IHypermediaDocument
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var link = new Link(href);

            builder.AddLink(rel, link);

            return builder;
        }

        public static IHypermediaBuilder<TDocument> AddLinksToChild<TDocument>(this IHypermediaBuilder<TDocument> builder, Action<TDocument, IHypermediaService> childHandler)
            where TDocument : IHypermediaDocument
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (childHandler == null)
            {
                throw new ArgumentNullException(nameof(childHandler));
            }

            childHandler.Invoke(builder.Document, builder.Service);

            return builder;
        }

        public static IHypermediaBuilder<TDocument> AddLinksPerCondition<TDocument>(this IHypermediaBuilder<TDocument> builder, Func<TDocument, bool> conditionHandler, Action<IHypermediaBuilder<TDocument>> builderHandler)
            where TDocument : IHypermediaDocument
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (conditionHandler == null)
            {
                throw new ArgumentNullException(nameof(conditionHandler));
            }

            if (builderHandler == null)
            {
                throw new ArgumentNullException(nameof(builderHandler));
            }

            if (!conditionHandler.Invoke(builder.Document))
            {
                return builder;
            }

            builderHandler.Invoke(builder);

            return builder;
        }
    }
}
