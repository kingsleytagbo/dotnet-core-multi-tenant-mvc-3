using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KT.Core.Mvc.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

/* Installed Microsoft.AspNetCore.Authentication.JwtBearer version 3.17
 * https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer
 */
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace KT.Core.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            #region ORIGINAL

            Configuration = configuration;

            #endregion ORIGINAL
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region ORIGINAL

            services.AddControllersWithViews();

            #endregion ORIGINAL

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
            });

            // Add the whole configuration object here.
            services.AddSingleton<IConfiguration>(Configuration);
            services.Configure<List<Tenant>>(Configuration.GetSection("Tenants:Tenant"));

            IOptions<List<Tenant>> tenants = this.BuildTenantsFromServiceProvider(services);

            // Get access to the tenants defined in appsettings.json
            if (tenants != null)
            {
                Console.WriteLine(tenants.ToString());
            }

            #region JWT
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               //options.RequireHttpsMetadata = false;
               //options.SaveToken = true;

               options.TokenValidationParameters = new TokenValidationParameters
               {
                   //Use this for a Single Site / Non-tenant Site
                   //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                   //ValidAudience = Configuration["Jwt:Issuer"],

                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,

                   // Specify the valid issue from appsettings.json
                   ValidIssuer = Configuration["Jwt:Issuer"],

                   // Specify the tenant API keys as the valid audiences
                   ValidAudiences = tenants.Value.Select(t => t.Key).ToList(),

                   IssuerSigningKeyResolver = (string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters) =>
                   {
                       List<SecurityKey> keys = new List<SecurityKey>();
                       Tenant tenant = tenants.Value.Where(t => t.Key == kid).FirstOrDefault();
                       var privateKey = ((tenant != null) && !string.IsNullOrEmpty(tenant.PrivateKey)) ? tenant.PrivateKey : kid;

                       if (!string.IsNullOrEmpty(privateKey))
                       {
                           var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
                           keys.Add(signingKey);
                       }
                       return keys;
                   }
               };

               options.Events = new JwtBearerEvents
               {
                   OnAuthenticationFailed = ctx =>
                   {
                       // Console.WriteLine(ctx);
                       if (ctx.Exception.GetType() == typeof(SecurityTokenExpiredException))
                       {
                           ctx.Response.Headers.Add("Token-Expired", "true");
                       }
                       return Task.CompletedTask;
                   },

                   OnMessageReceived = ctx =>
                   {
                       // Console.WriteLine(ctx);
                       //ctx.Request.EnableBuffering();
                       return Task.CompletedTask;
                   },

                   OnTokenValidated = context =>
                   {
                       // Console.WriteLine(context.ToString());
                       return Task.CompletedTask;
                   }
                    
               };

           });
            #endregion JWT
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*  
             *  .NET Core 3.1, UseAuthentication goes before UseRouting, etc. 
             *  or a 401 Error is Thrown: https://stackoverflow.com/questions/61976960/asp-net-core-jwt-authentication-always-throwing-401-unauthorized
             */
            //JWT Token
            app.UseAuthentication();
            // Shows UseCors with named policy.
            app.UseCors("AllowAllHeaders");

            #region ORIGINAL

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            #endregion ORIGINAL

        }

        private IOptions<List<Tenant>> BuildTenantsFromServiceProvider(IServiceCollection services)
        {
            ServiceProvider sp = services.BuildServiceProvider();
            IOptions<List<Tenant>> tenants = sp.GetService<IOptions<List<Tenant>>>();
            return tenants;
        }
    }
}
