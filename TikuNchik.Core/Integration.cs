using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TikuNchik.Core
{
    public class Integration
    {
        public Guid Id
        {
            get;
            private set;
        } = Guid.NewGuid();

        private ConcurrentDictionary<string, object> InternalHeaders
        {
            get;
        } = new ConcurrentDictionary<string, object>();

        public IReadOnlyDictionary<string, object> Headers
        {
            get
            {
                return this.InternalHeaders;
            }
        }

        public void AddHeader (string header, object headerValue)
        {
            if (header == null)
            {
                throw new ArgumentNullException(nameof(header));
            }

            this.InternalHeaders[header] = headerValue;
        }

        public object Body
        {
            get;set;
        }
    }


}
