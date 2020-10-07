using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;

namespace BlackGoldProperties_API.Controllers._6._Property_Administration
{
    public class TermController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/term")]
        public IHttpActionResult Get(/*[FromUri] string token*/)
        {
            ////Check valid token, logged in, role
            //if (TokenManager.Validate(token) != true)
            //    return BadRequest(); // Returns as user is invalid
            //if (TokenManager.IsLoggedIn(token) != true)
            //    return BadRequest(); // Returns as user is not logged in
            //if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            //{
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all terms
                    var terms = db.TERMs.Select(x => new {
                        x.TERMID,
                        x.TERMDESCRIPTION
                    }).ToList();

                    if (terms == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(terms);
                    }
                }
                catch (Exception)
                {
                    return NotFound();
                }
            //}
            //return Unauthorized();
        }
    }
}
