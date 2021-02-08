using System;

namespace iHeartLinks.Core
{
    public class Link
    {
        public Link(string href)
            : this(href, null)
        {
        }

        public Link(string href, string method)
        {
            Href = !string.IsNullOrWhiteSpace(href) ? href : throw new ArgumentException($"Parameter '{nameof(href)}' must not be null or empty.");
            Method = method;
        }

        public string Href { get; }
        
        public string Method { get; }

        public bool? Templated { get; set; }
    }
}