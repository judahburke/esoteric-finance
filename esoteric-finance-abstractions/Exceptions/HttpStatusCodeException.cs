using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace Esoteric.Finance.Abstractions.Exceptions
{
    [Serializable]
    public class HttpStatusCodeException : Exception, ISerializable
    {
        public HttpStatusCodeException(HttpStatusCode statusCode, object? value = null)
            => (StatusCode, Value) = (statusCode, value);

        protected HttpStatusCodeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            StatusCode = (HttpStatusCode)info.GetInt32("StatusCode");
            Value = info.GetValue("Value", typeof(object));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(StatusCode), (int)StatusCode);
            info.AddValue(nameof(Value), Value);
        }

        public HttpStatusCode StatusCode { get; }

        public object? Value { get; }

    }
}
