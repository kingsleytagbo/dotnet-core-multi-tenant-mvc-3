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
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using KT.Core.Mvc.Business;

namespace KT.Core.Mvc.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<List<Tenant>> _tenants;
        private readonly ILogger<AccountController> _logger;

        public ImagesController(
    ILogger<AccountController> logger,
    IConfiguration configuration,
    IOptions<List<Tenant>> tenants) : base
        (logger, configuration, tenants)
        {
            _logger = logger;
            _configuration = configuration;
            _tenants = tenants;
        }


        // GET: api/images
        [HttpGet]
        public IActionResult Get()
        {
            //var headers = Request.Headers;
            //var authorization = headers["Authorization"];
            IEnumerable<wp_image> result = null;
            var tenant = this.GetTenant();

            if (tenant != null)
            {
                result = Images.GetAll(tenant.ConnectionString, null, null);
                if (result != null)
                {
                    return Ok(result);
                }
            }

            return BadRequest(result);
        }



        // POST: api/AccountsContoller
        [HttpPost]
        [Authorize]
        public void Post([FromHeader] string url, [FromHeader] string name, [FromBody] string value)
        {
            var tenant = this.GetTenant();
            var result = 0;

            if (tenant != null)
            {
                var body = JsonSerializer.Deserialize<object>(value) as System.Text.Json.JsonElement?;
                var category = body?.GetProperty("category-create").ToString();

                var image = Images.GetImageBytes(url, new wp_image() { url = url, name = name, site_id = 1 });
                result = Images.Create(image, tenant.ConnectionString, null, null);
            }
        }

        // PUT: api/AccountsContoller/5
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, [FromBody] string value)
        {
            int? result = null;
            var tenant = this.GetTenant();
            wp_image body = System.Text.Json.JsonSerializer.Deserialize<wp_image>(value);
            if (tenant != null)
            {
                result = Images.Update(id, body.url, body.name, tenant.ConnectionString, null, null);
                if (result.HasValue)
                {
                    return Ok(result);
                }
            }
            return BadRequest(result);
        }

        // DELETE: api/images/5
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            int? result = null;
            var tenant = this.GetTenant();
            if (tenant != null)
            {
                result = Images.Delete(id, tenant.ConnectionString, null, null);
                if (result.HasValue)
                {
                    return Ok(result);
                }
            }
            return BadRequest(result);
        }


        /// <summary>
        /// Authenticates a User / Account
        /// </summary>
        /// <returns>Return a valid user account or null if authentication is unsuccessful</returns>
        [HttpPost("search")]
        public IActionResult Search(
            [FromHeader] string name)
        {
            int? result = null;
            var tenant = this.GetTenant();

            if (tenant != null)
            {
                result = null; // Users.register(first_name, last_name, user_email, user_pass, tenant.ConnectionString, null, null);
                if (result != null)
                {
                    return Ok(result);
                }
            }

            return BadRequest(result);
        }


    }
}
