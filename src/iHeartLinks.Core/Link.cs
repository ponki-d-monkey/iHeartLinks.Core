using System;

namespace iHeartLinks.Core
{
    public class Link
    {
        public Link(string href)
        {
            Href = !string.IsNullOrWhiteSpace(href) ? href : throw new ArgumentException($"Parameter '{nameof(href)}' must not be null or empty.");
        }

        public string Href { get; }
    }
}