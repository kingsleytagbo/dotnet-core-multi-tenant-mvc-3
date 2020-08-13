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
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<List<Tenant>> _tenants;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            ILogger<AccountController> logger,
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

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Authenticates a User / Account
        /// </summary>
        /// <returns>Return a valid user account or null if authentication is unsuccessful</returns>
        [HttpPost("login")]
        public IActionResult Authenticate([FromHeader] Login value)
        {
            IActionResult response = Unauthorized();
            kt_wp_user user = null;

            // Validate that this user is authentic and is authorized to access your system
            // TODO: Implement your own authetication logic
            if (value.UserName == "Kingsley")
            {
                user = new kt_wp_user { user_login = "Kingsley Tagbo", user_email = "test.test@gmail.com" };
                response = Ok(new { user = user });
            }

            return response;
        }

    }
}
