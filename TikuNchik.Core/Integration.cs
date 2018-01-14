using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TikuNchik.Core
{
    public class Integration<TBody> : Integration
    {
        public TBody Message
        {
            get;
            private set;
        }

        public Integration (TBody initialBody)
        {
            this.Body = initialBody;
        }

        /// <summary>
        /// This overload is only used internally to create integrations from existing integrations
        /// </summary>
        private Integration()
        {

        }

        public static Integration<TTarget> SetMessage<TTarget> (TTarget target)
        {
            return new Integration<TTarget>()
            {
                Message = target
            };
        }
    }


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
