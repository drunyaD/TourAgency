using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using TourAgency.WEB.Validation;

namespace TourAgency.WEB
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Конфигурация и службы веб-API
            config.EnableCors(new EnableCorsAttribute("http://localhost:8080", "*", "*", "Set-Cookie, Paging-Headers") { SupportsCredentials = true });
            // Маршруты веб-API
            config.MapHttpAttributeRoutes();
            config.Filters.Add(new ValidateModelAttribute());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
