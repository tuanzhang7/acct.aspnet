using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;
using System.Configuration;

namespace acct.webapi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();
            string Cors_origins = ConfigurationManager.AppSettings["Cors_origins"];
            var cors = new EnableCorsAttribute(
            origins: Cors_origins,
            headers: "*",
            methods: "*",
            exposedHeaders:"*");

            cors.ExposedHeaders.Add("Content-Disposition");
            cors.ExposedHeaders.Add("X-Pagination");

            config.EnableCors(cors);


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            
        }

        
    }
}
