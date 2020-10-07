using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;

namespace BlackGoldProperties_API.Controllers
{
    public class TokenValidationController : ApiController
    {
        //CHECK LOGGED IN//
        [HttpPost]
        [Route("api/tokenisloggedin")]
        public bool Post([FromUri] string token)
        {
            try
            {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return false; // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return false; // Returns as user is not logged in
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //GET ROLES//
        [HttpGet]
        [Route("api/tokenroles")]
        public bool Get([FromUri] string token)
        {
            try
            {
                var roles = TokenManager.GetRoles(token);
                if(TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
                {
                    return true;
                }
                else
                {
                    return false;
                }              
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
