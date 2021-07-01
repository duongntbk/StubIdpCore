using StubIdpCore.Middleware;
using System;

namespace StubIdpCore
{
    public class UrlResolver
    {
        public static Uri AutoSsoServiceUri => new Uri(MyHttpContext.AppBaseUri, "auto/login");

        public static string AutoIdpEntityId => $"{MyHttpContext.AppBaseUrl}/auto";

        public static Uri InteractiveSsoServiceUri => new Uri(MyHttpContext.AppBaseUri, "interactive/login");

        public static string InteractiveIdpEntityId => $"{MyHttpContext.AppBaseUrl}/interactive";
    }
}
