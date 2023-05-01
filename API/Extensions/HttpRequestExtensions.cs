using Microsoft.AspNetCore.Http;
using System;

namespace API.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string BaseUrl(this HttpRequest req)
        {
            if (req == null) return null;
            var uriBuilder = new UriBuilder(req.Scheme, req.Host.Host, req.Host.Port ?? -1);
           
            return uriBuilder.Uri.AbsoluteUri;
        }
    }
}
