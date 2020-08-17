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
using System.Net.Http;
using KT.Core.Mvc.Business;

namespace KT.Core.Mvc.Controllers
{
    public class BaseController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<List<Tenant>> _tenants;
        private readonly ILogger<BaseController> _logger;
        private readonly IHttpContextAccessor _httpContext;

        public BaseController(
            ILogger<BaseController> logger,
            IConfiguration configuration,
            IOptions<List<Tenant>> tenants,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _configuration = configuration;
            _tenants = tenants;
        }

        protected string BaseUrl
        {
            get
            {
                string base_url = "";
                return base_url;
            }
        }

        protected HttpContext _HttpContext
        {
            get
            {
                return _httpContext.HttpContext;
            }
        }
        protected string _DomainName
        {
            get
            {
                var value = _httpContext.HttpContext.Request.Host;
                return value.ToString();
            }
        }

        protected Tenant GetSetting()
        {
            var request = _HttpContext.Request;
            var host = request.Host.ToString();
            Tenant setting = null; // GetSetting(null, host);
            return setting;
        }

        protected Tenant GetTenant()
        {
            return null;
        }


        public Object GetHttpRequest(string path, bool isBasePath = false)
        {
            var response = new Object();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.BaseUrl);
                //client.DefaultRequestHeaders.Add("auth_origin", this.site_origin);
                //client.DefaultRequestHeaders.Add("auth_site", this.site_key);

                string url = !isBasePath ? (BaseUrl + path) : path;

                var responseTask = client.GetAsync(url);
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<object>();

                    readTask.Wait();

                    response = readTask.Result;
                }
            }

            return response;
        }

        public dynamic GetHttpRequestAsDynamic(string path, bool isBasePath = false)
        {
            var response = this.GetHttpRequest(path, isBasePath);
            var responseString = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(responseString);
            return result;
        }

        /// <summary>
        /// http://www.binaryintellect.net/articles/065c15ee-7da4-408c-aca8-4a86af6ede23.aspx
        /// </summary>
        /// <param name="path"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Object PostHttpRequest(string path, object value)
        {
            var response = new Object();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.BaseUrl);
                //client.DefaultRequestHeaders.Add("auth_origin", this.site_origin);
                //client.DefaultRequestHeaders.Add("auth_site", this.site_key);

                string url = BaseUrl + path;

                var responseTask = client.PostAsJsonAsync(url, value);

                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<object>();

                    readTask.Wait();

                    response = readTask.Result;
                }
            }

            return response;
        }



    }
}
