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
        public IActionResult Get(int page = 1, int pageSize = 1)
        {
            //var headers = Request.Headers;
            //var authorization = headers["Authorization"];
            IEnumerable<wp_image> result = null;
            var tenant = this.GetTenant();

            if (tenant != null)
            {
                result = Images.GetAll(tenant.ConnectionString, page, pageSize, null, null);
                var total = Images.GetTotal(tenant.ConnectionString);
                if (result != null)
                {
                    return Ok(
                        new
                        {
                            data = result,
                            total = total,
                            page = page,
                            pageSize = pageSize
                        }
                      );
                }
            }

            return BadRequest(result);
        }



        [HttpPost("Upload")]
        [Authorize]
        public IActionResult Upload([FromForm] wp_image_file file)
        {
            var files = Request.Form.Files;
            var tenant = this.GetTenant();
            if (tenant != null)
            {
                if ((files != null) && (files.Count > 0))
                {
                    using (var memory = new System.IO.MemoryStream())
                    {
                        var image = files[0];
                        image.CopyTo(memory);
                        var upload = memory.ToArray();
                        Images.SaveImageFromStream(upload, file.ID, file.category, file.url, file.name, tenant.ConnectionString);

                    }
                }
            }
            return Ok();
        }



        // POST: api/AccountsContoller
        [HttpPost]
        [Authorize]
        public void Post([FromBody] string value)
        {
            var tenant = this.GetTenant();
            if (tenant != null)
            {
                var body = JsonSerializer.Deserialize<object>(value) as System.Text.Json.JsonElement?;
                var category = body?.GetProperty("category").ToString();
                var url = body?.GetProperty("url").ToString();
                var name = body?.GetProperty("name").ToString();
                Images.SaveImageFromStream(null, 0, category, url, name, tenant.ConnectionString);

                /*
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
                        var image = Images.GetImageBytes(url, new wp_image() { category = category,  url = url, name = name, site_id = 1 });
                        using (var transaction = new System.Transactions.TransactionScope())
                        {
                            image.category = category;
                            var newImageId = Images.Create(image, tenant.ConnectionString, null, null);

                            var postChild = new wp_post()
                            {
                                post_parent = parentPostId.Value,
                                post_name = newImageId.ToString(),
                                post_content = category,
                                post_category = category,
                                post_status = "active",
                                post_type = "image",
                                post_date = DateTime.Now,
                                post_title = category,

                                comment_status = "",
                                post_mime_type = "",
                                guid = Guid.NewGuid().ToString(),
                                post_author = 0,
                                post_content_filtered = "",
                                post_excerpt = "",

                                to_ping = "",
                                post_password = "",
                                pinged = "",
                                ping_status = "",
                                post_date_gmt = DateTime.Now,
                                post_modified = DateTime.Now,
                                post_modified_gmt = DateTime.Now,
                                comment_count = 0,
                                site_id = 1,
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
                 */
            }
        }

        // PUT: api/AccountsContoller/5
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, [FromBody] string value)
        {
            Int64? result = null;
            var tenant = this.GetTenant();
            var user = Request.HttpContext.User;
            wp_image body = System.Text.Json.JsonSerializer.Deserialize<wp_image>(value);
            if (tenant != null)
            {
                result = Images.Update(id, body.category, body.url, body.name, tenant.ConnectionString, null, null);
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
