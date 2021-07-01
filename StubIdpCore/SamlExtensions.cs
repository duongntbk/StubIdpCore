using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Sustainsys.Saml2.WebSso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace StubIdpCore
{
    public static class SamlExtensions
    {
        /// <summary>
        /// Converts a command result to an action result.
        /// </summary>
        /// <param name="commandResult">The source command result.</param>
        /// <returns>Action result</returns>
        /// <remarks>The reason to use a separate command result at all, instead
        /// of simply using ActionResult is that the core library should not
        /// be Mvc dependant.</remarks>
        public static ActionResult ToActionResult(this CommandResult commandResult)
        {
            if (commandResult == null)
            {
                throw new ArgumentNullException(nameof(commandResult));
            }

            switch (commandResult.HttpStatusCode)
            {
                case HttpStatusCode.SeeOther:
                    return new RedirectResult(commandResult.Location.OriginalString);
                case HttpStatusCode.OK:
                    var result = new ContentResult()
                    {
                        Content = commandResult.Content
                    };

                    if (!string.IsNullOrEmpty(commandResult.ContentType))
                    {
                        result.ContentType = commandResult.ContentType;
                    }

                    return result;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Extension method to convert a HttpRequest to a HttpRequestData.
        /// </summary>
        /// <param name="request">The request object used to populate the <c>HttpRequestData</c>.</param>
        /// <returns>The <c>HttpRequestData</c> object that has been populated by the request.</returns>
        public static HttpRequestData ToHttpRequestData(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new HttpRequestData(
                request.Method,
                new Uri(request.GetDisplayUrl()),
                request.GetRootUri().ToString(),
                Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>(),
                Enumerable.Empty<KeyValuePair<string, string>>(),
                v => v,
                ClaimsPrincipal.Current);
        }

        public static Uri GetRootUri(this HttpRequest request)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port.GetValueOrDefault(80)
            };

            return uriBuilder.Uri;
        }
    }
}
