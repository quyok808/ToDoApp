using System.Net;

namespace CoreLibs.Wrappers
{
    public class FetchResponse<T>
    {
        public HttpStatusCode StatusCode { get; private set; }

        public T Data { get; private set; }

        public static FetchResponse<T> Create(HttpStatusCode statusCode, T data)
        {
            return new FetchResponse<T>
            {
                StatusCode = statusCode,
                Data = data
            };
        }
    }
}
