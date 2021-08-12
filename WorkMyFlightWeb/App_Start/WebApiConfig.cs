using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Owin.Security.OAuth;
//using Newtonsoft.Json.Serialization;

namespace WorkMyFlightWeb
{
    public static class WebApiConfig
    {
   
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.

            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            var corsAttr = new EnableCorsAttribute("http://localhost:3000", "*", "*");
            config.EnableCors();
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(
             new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
            
            // Call Pre flight setting
            //config.MessageHandlers.Add(new PreFlightRequestsHandler()); // Defined above
        }
    }
}
