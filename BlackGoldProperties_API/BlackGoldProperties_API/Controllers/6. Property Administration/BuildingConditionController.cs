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
    public class BuildingConditionController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/buildingcondition")]
        public IHttpActionResult Get([FromUri] string token)
        {
                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in
                if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all building conditions
                    var buildingconditions = db.BUILDINGCONDITIONs.Select(x => new
                    {
                        x.BUILDINGCONDITIONID,
                        x.BUILDINGCONDITIONDESCRIPTION
                    }).ToList();

                    if (buildingconditions == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(buildingconditions);
                    }
                }
                return Unauthorized();
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }


        //READ DATA OF SPECIFIC ID//
        [HttpGet]
        [Route("api/buildingcondition")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        {

                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in
                if (TokenManager.GetRoles(token).Contains(6 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
                {
                    //Null check
                    if (string.IsNullOrEmpty(id.ToString()))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get specified building condition
                    var buildingcondition = db.BUILDINGCONDITIONs.Where(z => z.BUILDINGCONDITIONID == id).Select(x => new { x.BUILDINGCONDITIONID, x.BUILDINGCONDITIONDESCRIPTION }).FirstOrDefault();

                    if (buildingcondition == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(buildingcondition);
                    }
                }
                return Unauthorized();
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }
    }
}
