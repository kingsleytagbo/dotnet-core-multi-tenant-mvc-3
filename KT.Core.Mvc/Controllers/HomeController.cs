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
        private readonly string _connectionString;

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
            _connectionString = (_tenant != null && 
                !string.IsNullOrEmpty(_tenant.ConnectionString)) ? _tenant.ConnectionString : string.Empty;
    }

        public IActionResult Index(int page = 1, string search = null)
        {
            @ViewData["Title"] = "Home";
            @ViewData["Tenant"] = this._tenant;
            @ViewData["Layout"] = this._tenant.Template;

            var pageSize = 1;
            var total = Images.GetTotal(this._connectionString);
            List<wp_image> result = Images.GetAll(this._connectionString, page, pageSize);

            var pager = new Pager(total, page, 1, search);
            ViewData["Pager"] = pager;
            return View(result);

            /*
            var login = new { auth_site= "d62c03a2-57b6-4e14-8153-d05d3aa9ab10", username="Kingsley", password= "..gmail.com", rememberme=false  };
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
            /*
            var result = this.PostHttpRequest("/api/account/login", headers, null);
            var data = Posts.GetAllPosts(this._connectionString, null, null);
            */


        }

        public IActionResult AskAQuestion()
        {
            @ViewData["Title"] = "Ask A Question";
            @ViewData["Tenant"] = this._tenant;
            @ViewData["Layout"] = this._tenant.Template;

            return View();
        }

        public IActionResult ConfigureSetup()
        {
            @ViewData["Title"] = "Configuration & Setup";
            @ViewData["Tenant"] = this._tenant;
            @ViewData["Layout"] = this._tenant.Template;

            return View();
        }

        public IActionResult Contact()
        {
            @ViewData["Title"] = "Ask A Question";
            @ViewData["Tenant"] = this._tenant;
            @ViewData["Layout"] = this._tenant.Template;

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

        public IActionResult Register()
        {
            @ViewData["Title"] = "Register";
            @ViewData["Tenant"] = this._tenant;
            @ViewData["Layout"] = this._tenant.Template;

            return View();
        }

        public IActionResult Search(string search)
        {
            @ViewData["Title"] = search;
            @ViewData["Tenant"] = this._tenant;
            @ViewData["Layout"] = this._tenant.Template;

            List<wp_image> result = null;
            if (!string.IsNullOrEmpty(search))
            {
                result = Images.Search(search, this._connectionString, null, null);
                return View(result);
            }
            else
            {
                result = new List<wp_image>();
            }

            return View(result);
        }

        public IActionResult Slug(string id)
        {
            @ViewData["Title"] = id;
            @ViewData["Tenant"] = this._tenant;
            @ViewData["Layout"] = this._tenant.Template;

            wp_post data = null;
            try
            {
                data = Posts.GetPost(id, this._connectionString, null, null);
            }
            catch (Exception) { }
            @ViewData["Title"] = string.Concat( (((data!= null) && !string.IsNullOrEmpty(data.post_title))
                ? data.post_title : id), 
                " - ", this._tenant.Name ) ;
            return View("Details", data);
        }

    }
}
