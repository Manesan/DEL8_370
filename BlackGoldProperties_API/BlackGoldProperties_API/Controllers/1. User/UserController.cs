using System.Collections.Generic;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace BlackGoldProperties_API.Controllers._1._User
{
    public class UserController : ApiController
    {
        //LOGIN//
        [HttpPost]
        [Route("api/user")]
        public dynamic Login([FromUri] string email, [FromUri] string password)
        {
            //Null checks
            if (string.IsNullOrEmpty(email))
                return BadRequest();
            if (string.IsNullOrEmpty(password))
                return BadRequest();

            try
            {
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;
                if (email == null || password == null)
                    return null;
                if (!ModelState.IsValid)
                    return null;
                dynamic userToken = HomeController.Login(email.ToLower(), password);
                if (userToken == null)
                    return null;
                return userToken;
            }
            catch (System.Exception)
            {
                return NotFound();
            }
        }
    }
}
