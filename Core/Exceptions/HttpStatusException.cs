using System;

namespace Core.Exceptions
{
    public class HttpStatusException : Exception
    {
        public int StatusCode { get; set; }

        public HttpStatusException(int statusCode, string message = null) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
