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
using System.Text.Json;

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
            _httpContext = httpContextAccessor;
        }

        protected string BaseUrl
        {
            get
            {
                string base_url = "https://localhost:44373";
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

        protected Tenant GetTenant()
        {
            var request = _HttpContext.Request;
            var host = request.Host.ToString();
            Tenant tenant = GetTenant(null, host);
            return tenant;
        }

        protected Tenant GetTenant(string id, string name)
        {
            Tenant tenant = null;

            for (var s = 0; s < this._tenants.Value.Count; s++)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    if (this._tenants.Value[s].Key.Trim().ToLower() == id.Trim().ToLower())
                    {
                        tenant = this._tenants.Value[s];
                        break;
                    }
                }
                else if (!string.IsNullOrEmpty(name))
                {
                    var hosts = this._tenants.Value[s].Host.Trim().ToLower().Split(",", StringSplitOptions.None);
                    foreach (var item in hosts)
                    {
                        var hostConfig = item.Trim().ToLower();
                        var host = name.Trim().ToLower();

                        if ( (hostConfig == host) || (hostConfig.IndexOf(host) > -1))
                        {
                            tenant = this._tenants.Value[s];
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            return tenant;
        }


        protected Object GetHttpRequest(string path, IDictionary<string, string> headers, bool isBasePath = false)
        {
            var response = new Object();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.BaseUrl);
                if (headers != null && headers.Count > 0)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
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

        protected dynamic GetHttpRequestAsDynamic(string path, bool isBasePath = false)
        {
            var response = this.GetHttpRequest(path, null, isBasePath);
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
        protected Object PostHttpRequest(string path, IDictionary<string, string> headers, object body)
        {
            var response = new Object();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.BaseUrl);
                if (headers != null && headers.Count > 0)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                //client.DefaultRequestHeaders.Add("auth_origin", this.site_origin);
                //client.DefaultRequestHeaders.Add("auth_site", this.site_key);

                string url = BaseUrl + path;

                var responseTask = client.PostAsJsonAsync(url, body);

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


        /// <summary>
        /// http://www.binaryintellect.net/articles/065c15ee-7da4-408c-aca8-4a86af6ede23.aspx
        /// </summary>
        /// <param name="path"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected Object PutHttpRequest(string path, IDictionary<string, string> headers, object body)
        {
            var response = new Object();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.BaseUrl);
                if (headers != null && headers.Count > 0)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                //client.DefaultRequestHeaders.Add("auth_origin", this.site_origin);
                //client.DefaultRequestHeaders.Add("auth_site", this.site_key);

                string url = BaseUrl + path;

                var responseTask = client.PutAsJsonAsync(url, body);

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
