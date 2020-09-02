using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using KT.Core.Mvc.Models;
using KT.Core.Mvc.Business;

namespace KT.Core.Mvc.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<List<Tenant>> _tenants;
        private readonly ILogger<HomeController> _logger;
        private readonly Tenant _tenant = null;
        private string connectionString = "Data Source = SQL5052.site4now.net; Initial Catalog = DB_A38FF7_itcareercoachnet; User Id = DB_A38FF7_itcareercoachnet_admin; Password=Xarch2014#";

        public HomeController(
ILogger<HomeController> logger,
IConfiguration configuration,
IOptions<List<Tenant>> tenants, IHttpContextAccessor httpContextAccessor) : base
    (logger, configuration, tenants, httpContextAccessor)
    {
        _logger = logger;
        _configuration = configuration;
        _tenants = tenants;
            _tenant = this.GetTenant();
    }

        public IActionResult Index()
        {
            @ViewData["Title"] = "Login";
            @ViewData["Tenant"] = this._tenant;
            @ViewData["Layout"] = this._tenant.Template;
            // var login = new { auth_site= "d62c03a2-57b6-4e14-8153-d05d3aa9ab10", username="Kingsley", password= "..gmail.com", rememberme=false  };
            var headers = new Dictionary<string, string>(){
                {
                    "auth_site", "d62c03a2-57b6-4e14-8153-d05d3aa9ab10"
                },
                {
                    "username", "Kingsley"
                },
                {
                    "password", "..gmail.com"
                },
                {
                    "rememberme", "false"
                }
            };
            var result = this.PostHttpRequest("/api/account/login", headers, null);

            var post = new Post();
            var data = post.GetAllPosts(connectionString, null, null);

            return View(data);
        }

        public IActionResult AskAQuestion()
        {
            @ViewData["Title"] = "Ask A Question";
            @ViewData["Tenant"] = this._tenant;
            @ViewData["Layout"] = this._tenant.Template;
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
            @ViewData["Tenant"] = this._tenant;
            @ViewData["Layout"] = this._tenant.Template;
            return View();
        }

        public IActionResult Privacy()
        {
            @ViewData["Title"] = "Privacy";
            @ViewData["Tenant"] = this._tenant;
            @ViewData["Layout"] = this._tenant.Template;
            return View();
        }

        public IActionResult Slug(string id)
        {
            @ViewData["Title"] = id;
            @ViewData["Tenant"] = this._tenant;
            @ViewData["Layout"] = this._tenant.Template;
            var post = new Post();
            kt_wp_post data = null;
            try
            {
                data = post.GetPost(id, connectionString, null, null);
            }
            catch (Exception) { }
            @ViewData["Title"] = string.Concat( (((data!= null) && !string.IsNullOrEmpty(data.post_title))
                ? data.post_title : id), 
                " - ", this._tenant.Name ) ;
            return View("Details", data);
        }

    }
}
