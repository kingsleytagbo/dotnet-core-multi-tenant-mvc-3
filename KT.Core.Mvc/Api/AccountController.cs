using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KT.Core.Mvc.Models;

namespace KT.Core.Mvc.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // GET: api/AccountsContoller
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/AccountsContoller/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/AccountsContoller
        [HttpPost]
        public void Post([FromBody] string value)
        {
            Console.WriteLine(value);
        }

        // PUT: api/AccountsContoller/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        /// <summary>
        /// Authenticates a User / Account
        /// </summary>
        /// <returns>Return a valid user account or null if authentication is unsuccessful</returns>
        [HttpPost("login")]
        public kt_wp_user Login([FromHeader] Login value)
        {
            kt_wp_user user = null;

            // Validate that this user is authentic and is authorized to access your system
            // TODO: Implement your own authetication logic
            if (value.UserName == "Kingsley")
            {
                user = new kt_wp_user { user_login = "Kingsley Tagbo", user_email = "test.test@gmail.com" };
                //response = new JsonResult({ user = user });
            }

            return user;
        }

        // POST: api/Accounts/getusers
        [HttpPost("getusers")]
        public IEnumerable<string> GetUsers()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
