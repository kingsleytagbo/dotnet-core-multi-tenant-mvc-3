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
        public void Post([FromBody] string value)
        {
            var tenant = this.GetTenant();
            Int64 result = 0;

            if (tenant != null)
            {
                Int64 ? parentPostId = null;
                var body = JsonSerializer.Deserialize<object>(value) as System.Text.Json.JsonElement?;
                var category = body?.GetProperty("category-create").ToString();
                var url = body?.GetProperty("url").ToString();
                var name = body?.GetProperty("name").ToString();
                if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(name))
                {
                    var post_parent = Posts.GetParentImage(category, tenant.ConnectionString, null, null);
                    if(post_parent == null)
                    {
                        post_parent = new wp_post()
                        {
                            post_name = category,
                            post_parent = 0,
                            post_content = category, 
                            comment_count = 0,
                            guid = Guid.NewGuid().ToString(),
                            post_status = "active", post_type = "image", post_date = DateTime.Now, post_title = category,
                            to_ping = "", post_date_gmt = DateTime.Now, post_modified = DateTime.Now, post_modified_gmt = DateTime.Now,
                             site_id = 1,
                            comment_status = "", post_mime_type = "", post_password = "" , 
                            pinged = "", ping_status = "", post_author=0, post_content_filtered = "", post_excerpt = "",
                        };

                        parentPostId = Posts.Create(post_parent, tenant.ConnectionString, null, null);
                    }
                    else
                    {
                        parentPostId = post_parent.ID;
                    }

                    if (parentPostId.HasValue)
                    {
                        var image = Images.GetImageBytes(url, new wp_image() { url = url, name = name, site_id = 1 });
                        using (var transaction = new System.Transactions.TransactionScope())
                        {
                            var newImageId = Images.Create(image, tenant.ConnectionString, null, null);

                            var postChild = new wp_post()
                            {
                                post_name = category,
                                post_parent = parentPostId.Value,
                                post_content = category,
                                comment_count = 0,
                                guid = Guid.NewGuid().ToString(),
                                post_status = "active",
                                post_type = "image",
                                post_date = DateTime.Now,
                                post_title = category,
                                to_ping = "",
                                post_date_gmt = DateTime.Now,
                                post_modified = DateTime.Now,
                                post_modified_gmt = DateTime.Now,
                                site_id = 1,
                                comment_status = "",
                                post_mime_type = "",
                                post_password = "",
                                pinged = "",
                                ping_status = "",
                                post_author = 0,
                                post_content_filtered = "",
                                post_excerpt = "",
                            };

                            var newChildPostId = Posts.Create(postChild, tenant.ConnectionString, null, null);
                            if(newImageId > 0 && newChildPostId > 0)
                            {
                                transaction.Complete();
                            }
                        }
                    }

                    return;
                }
            }
        }

        // PUT: api/AccountsContoller/5
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, [FromBody] string value)
        {
            Int64? result = null;
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
        [HttpGet("categories")]
        public IActionResult Categoriess()
        {
            List<string> result = null;
            var tenant = this.GetTenant();

            if (tenant != null)
            {
                result = Posts.GetImageCategories(tenant.ConnectionString, null, null);
                if (result != null)
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
