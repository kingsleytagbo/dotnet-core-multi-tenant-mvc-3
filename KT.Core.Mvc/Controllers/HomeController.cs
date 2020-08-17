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
using Microsoft.AspNetCore.Http;

namespace KT.Core.Mvc.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IConfiguration _configuration;
    private readonly IOptions<List<Tenant>> _tenants;
    private readonly ILogger<HomeController> _logger;

    public HomeController(
ILogger<HomeController> logger,
IConfiguration configuration,
IOptions<List<Tenant>> tenants, IHttpContextAccessor httpContextAccessor) : base
    (logger, configuration, tenants, httpContextAccessor)
    {
        _logger = logger;
        _configuration = configuration;
        _tenants = tenants;
    }

        public IActionResult Index()
        {
            var login = new { auth_site= "d62c03a2-57b6-4e14-8153-d05d3aa9ab10", username="Kingsley", password= "..gmail.com", rememberme=false  };
            var result = this.PostHttpRequest("/api/account/login", login);

            return View(result);
        }

        public IActionResult AskAQuestion()
        {
            @ViewData["Title"] = "Ask A Question";
            return View();
        }

        public IActionResult Contact()
        {
            @ViewData["Title"] = "Contact";
            return View();
        }

        public IActionResult Login()
        {
            @ViewData["Title"] = "Login";
            return View();
        }

        public IActionResult Privacy()
        {
            @ViewData["Title"] = "Privacy";
            return View();
        }

        public IActionResult Slug(string id)
        {
            @ViewData["Title"] = id;
            return View();
        }

    }
}
