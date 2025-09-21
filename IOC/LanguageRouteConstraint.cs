using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Globalization;

namespace IoC
{
    public class LanguageRouteConstraint : IRouteConstraint
    {
        private readonly string[] _supportedLanguages = { "en", "ar" };

        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey("language"))
            {
                return false;
            }

            var lang = values["language"]?.ToString()?.ToLowerInvariant();
            if (string.IsNullOrEmpty(lang))
            {
                return false;
            }

            return _supportedLanguages.Contains(lang);
        }
    }
}