using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Cors;
using Microsoft.Owin.Cors;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(acct.webapi.Startup))]

namespace acct.webapi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            CorsPolicy tokenCorsPolicy = new CorsPolicy
            {
                AllowAnyOrigin = true,
                AllowAnyHeader = true,
                AllowAnyMethod = true
            };

            CorsOptions corsOptions = new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = request => Task.FromResult(request.Path.ToString().StartsWith("/token") ? tokenCorsPolicy : null)
                }
            };

            app.UseCors(corsOptions);

            HttpConfiguration webApiConfig = new HttpConfiguration();
            app.UseWebApi(webApiConfig);

            ConfigureAuth(app);

        }
    }
}
