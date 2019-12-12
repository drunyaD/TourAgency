using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject.Web.WebApi;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using TourAgency.BLL.Infrastructure;
using TourAgency.WEB.Infrastructure;
using TourAgency.WEB;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[assembly: OwinStartup(typeof(TourAgency.WEB.App_Start.Startup))]
namespace TourAgency.WEB.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            AutoMapperConfig.Initialize();
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });

            var config = new HttpConfiguration();
            config.DependencyResolver = new NinjectDependencyResolver(new Ninject.Web.Common.Bootstrapper().Kernel);

            var jsonFormatter = config.Formatters.JsonFormatter;
            jsonFormatter.UseDataContractJsonSerializer = false;
            var settings = jsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            WebApiConfig.Register(config);
            app.UseWebApi(config);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
           
        }
    }
}