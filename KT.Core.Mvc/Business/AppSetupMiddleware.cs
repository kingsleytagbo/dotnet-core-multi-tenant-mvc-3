using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KT.Core.Mvc.Business
{
    public class AppSetupMiddleware
    {
        public AppSetupMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        readonly RequestDelegate _next;

        public async Task Invoke(HttpContext context, IConfiguration configuration)
        {
            if (!IsDatabaseInstalled(configuration))
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

        public static bool IsDatabaseInstalled(IConfiguration configuration)
        {
            var key = configuration["SQLConnectionSettings:SqlServerIp"];
            return string.IsNullOrEmpty(key);
        }
    }
}
