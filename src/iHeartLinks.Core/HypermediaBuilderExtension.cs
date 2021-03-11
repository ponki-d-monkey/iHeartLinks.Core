using System;

namespace iHeartLinks.Core
{
    public static class HypermediaBuilderExtension
    {
        public static IHypermediaBuilder<TDocument> AddLink<TDocument>(this IHypermediaBuilder<TDocument> builder, string rel, string href, Func<TDocument, bool> conditionHandler)
            where TDocument : IHypermediaDocument
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (string.IsNullOrWhiteSpace(rel))
            {
                throw new ArgumentException($"Parameter '{nameof(rel)}' must not be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(href))
            {
                throw new ArgumentException($"Parameter '{nameof(href)}' must not be null or empty.");
            }

            if (conditionHandler == null)
            {
                throw new ArgumentNullException(nameof(conditionHandler));
            }

            return DoAddLinksPerCondition(builder, conditionHandler, b => DoAddLink(b, rel, href));
        }

        public static IHypermediaBuilder<TDocument> AddLink<TDocument>(this IHypermediaBuilder<TDocument> builder, string rel, string href)
            where TDocument : IHypermediaDocument
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (string.IsNullOrWhiteSpace(rel))
            {
                throw new ArgumentException($"Parameter '{nameof(rel)}' must not be null or empty.");
            }

            return DoAddLink(builder, rel, href);
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

            return DoAddLinksPerCondition(builder, conditionHandler, builderHandler);
        }

        private static IHypermediaBuilder<TDocument> DoAddLink<TDocument>(IHypermediaBuilder<TDocument> builder, string rel, string href)
            where TDocument : IHypermediaDocument
        {
            var link = new Link(href);

            builder.AddLink(rel, link);

            return builder;
        }

        private static IHypermediaBuilder<TDocument> DoAddLinksPerCondition<TDocument>(IHypermediaBuilder<TDocument> builder, Func<TDocument, bool> conditionHandler, Action<IHypermediaBuilder<TDocument>> builderHandler) where TDocument : IHypermediaDocument
        {
            if (!conditionHandler.Invoke(builder.Document))
            {
                return builder;
            }

            builderHandler.Invoke(builder);

            return builder;
        }
    }
}
