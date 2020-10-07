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
    public class MandateTypeController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/mandatetype")]
        public IHttpActionResult Get([FromUri] string token)
        {
            //Null checks
            if (string.IsNullOrEmpty(token))
                return BadRequest();

            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in
            if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all mandate types
                    var mandatetype = db.MANDATETYPEs.Select(x => new
                    {
                        x.MANDATETYPEID,
                        x.MANDATETYPEDESCRIPTION
                    }).ToList();

                    if (mandatetype == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(mandatetype);
                    }
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }


        //READ DATA OF SPECIFIC ID//
        [HttpGet]
        [Route("api/mandatetype")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        {
            //Null checks
            if (string.IsNullOrEmpty(token))
                return BadRequest();
            if (string.IsNullOrEmpty(id.ToString()))
                return BadRequest();

            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in
            if (TokenManager.GetRoles(token).Contains(6 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get specified mandate type
                    var mandatetype = db.MANDATETYPEs.Where(z => z.MANDATETYPEID == id).Select(x => new { x.MANDATETYPEID, x.MANDATETYPEDESCRIPTION }).FirstOrDefault();

                    if (mandatetype == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(mandatetype);
                    }
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }
    }
}
