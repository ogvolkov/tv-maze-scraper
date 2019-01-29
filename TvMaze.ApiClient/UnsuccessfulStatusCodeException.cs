
using System;
using System.Net;

namespace TvMaze.ApiClient
{
    public class UnsuccessfulStatusCodeException: Exception
    {
        public HttpStatusCode StatusCode { get; }

        public UnsuccessfulStatusCodeException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
