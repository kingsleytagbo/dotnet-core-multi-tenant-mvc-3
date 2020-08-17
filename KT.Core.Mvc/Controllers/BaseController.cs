using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KT.Core.Mvc.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace KT.Core.Mvc.Controllers
{
    public class BaseController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<List<Tenant>> _tenants;
        private readonly ILogger<BaseController> _logger;

        public BaseController(
            ILogger<BaseController> logger,
            IConfiguration configuration,
            IOptions<List<Tenant>> tenants)
        {
            _logger = logger;
            _configuration = configuration;
            _tenants = tenants;
        }

        protected Tenant GetTenant()
        {
            return null;
        }


    }
}
