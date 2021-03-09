using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace iHeartLinks.Core
{
    public class LinkRequest
    {
        private readonly IReadOnlyDictionary<string, object> requestParameters;

        public LinkRequest(IDictionary<string, object> requestParameters)
        {
            if (requestParameters == null || !requestParameters.Any())
            {
                throw new ArgumentException($"Parameter '{nameof(requestParameters)}' must not be null or empty.");
            }

            this.requestParameters = new ReadOnlyDictionary<string, object>(requestParameters);
        }

        public bool ContainsKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"Parameter '{nameof(key)}' must not be null or empty.");
            }

            return requestParameters.ContainsKey(key);
        }

        public T GetValueOrDefault<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"Parameter '{nameof(key)}' must not be null or empty.");
            }

            if (!requestParameters.TryGetValue(key, out object value))
            {
                return default;
            }

            return (T)value;
        }

        public object GetValueOrDefault(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"Parameter '{nameof(key)}' must not be null or empty.");
            }

            return requestParameters.GetValueOrDefault(key);
        }
    }
}
