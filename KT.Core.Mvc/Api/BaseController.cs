using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KT.Core.Mvc.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace KT.Core.Mvc.Api
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<List<Tenant>> _tenants;
        private readonly ILogger<AccountController> _logger;

        public BaseController(
    ILogger<AccountController> logger,
    IConfiguration configuration,
    IOptions<List<Tenant>> tenants)
        {
            _logger = logger;
            _configuration = configuration;
            _tenants = tenants;
        }

        protected string Request_DomainName
        {
            get
            {
                var value = string.Empty;

                if (Request != null && Request.Headers != null)
                {
                    var values = Request.Headers["auth_origin"];

                    if (values.Any() != false)
                    {
                        value = values.ToString();
                    }
                }

                return value;
            }
        }

        protected IHeaderDictionary GetRequestHeaders()
        {
                return Request.Headers;
        }

        protected string GetRequestHeaders(string key)
        {
            var value = string.Empty;

            if (Request != null && Request.Headers != null)
            {
                var values = Request.Headers[key];

                if (values.Any() != false)
                {
                    value = values.ToString();
                }
            }

            return value;
        }

        protected Tenant GetTenant()
        {
            var tenantId = GetRequestHeaders("auth_site");
            var tenant = (this._tenants != null) ? this._tenants.Value.Where(s => s.Key == tenantId).FirstOrDefault() : null;
            return tenant;
        }

        protected string CreateJWT(kt_wp_user userInfo, Tenant tenant, string tenantId, bool rememberMe)
        {
            var privateKey = ((tenant != null) && !string.IsNullOrEmpty(tenant.PrivateKey)) ? tenant.PrivateKey : tenantId;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            DateTime jwtExpires = DateTime.Now.AddMinutes(30);
            int jwtDuration = 15;

            if (rememberMe)
            {
                int.TryParse(_configuration["Jwt:Expires"], out jwtDuration);
            }
            jwtExpires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(jwtDuration));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                tenantId,
                new[]
                    {
                        new Claim(ClaimTypes.Name, userInfo.user_login)
                    },
              expires: jwtExpires,
              signingCredentials: credentials);

            token.Header.Add("kid", tenantId);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
