using KT.Core.Mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KT.Core.Mvc.Business
{
    public class AppSetupMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<List<Tenant>> _tenants;

        public AppSetupMiddleware(RequestDelegate next, IOptions<List<Tenant>> tenants)
        {
            _next = next;
            _tenants = tenants;
        }

        public async Task Invoke(HttpContext context, IConfiguration configuration)
        {
            if (!this.IsDatabaseInstalled(configuration, context.Request))
            {
                var url = "/Home/ConfigureSetup";
                //check if the current url contains the path of the installation url
                if (context.Request.Path.Value.IndexOf(url, StringComparison.CurrentCultureIgnoreCase) == -1)
                {
                    //redirect to another location if not ready
                    context.Response.Redirect(url);
                    return;
                }
            }
            //or call the next middleware in the request pipeline
            await _next(context);
        }

        public bool IsDatabaseInstalled(IConfiguration configuration, HttpRequest request)
        {
            // var key = configuration["AllowedHosts"];
            var success = false;
            if( (request != null && request.Host!= null) && (this._tenants != null) && (this._tenants.Value.Count > 0))
            {
                var host = request.Host.ToString().Trim().ToLower();
                foreach(var tenant in this._tenants.Value)
                {
                    var tenantHost = tenant.Host.ToLower().Trim();
                    if(host == tenantHost || tenantHost.Contains(host))
                    {
                        // test the databse connection:
                        var isReady = Website.IsDatabaseConfigured(tenant.ConnectionString, null, null);
                        success = isReady;
                        break;
                    }
                }
            }
            return success;
        }
    }
}
