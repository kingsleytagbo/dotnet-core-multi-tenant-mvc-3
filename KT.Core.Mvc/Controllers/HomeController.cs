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
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<List<Tenant>> _tenants;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILogger<HomeController> logger,
            IConfiguration configuration,
            IOptions<List<Tenant>> tenants)
        {
            _logger = logger;
            _configuration = configuration;
            _tenants = tenants;
        }

        public IActionResult Index()
        {
            return View();
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

        public IActionResult Slug(string id)
        {
            @ViewData["Title"] = id;
            return View();
        }

    }
}
