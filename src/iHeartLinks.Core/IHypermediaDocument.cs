using System.Collections.Generic;

namespace iHeartLinks.Core
{
    public interface IHypermediaDocument
    {
        IDictionary<string, Link> Links { get; }

        void AddLink(string rel, Link link);
    }
}
