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
    public class PointOfInterestController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/pointofinterest")]
        public IHttpActionResult Get([FromUri] string token)
        { 
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

                    //Get all points of interest
                    //var selection = db.POINTOFINTERESTs.Select(x => new { x.POINTOFINTERESTID, x.POINTOFINTERESTNAME, x.POINTOFINTERESTTYPE, x.SUBURBPOINTOFINTERESTs }).ToList();
                    var pointofinterest = db.SUBURBPOINTOFINTERESTs.Select(x => new {
                        x.POINTOFINTEREST.POINTOFINTERESTID,
                        x.POINTOFINTEREST.POINTOFINTERESTNAME,
                        x.POINTOFINTEREST.POINTOFINTERESTTYPEID,
                        x.POINTOFINTEREST.POINTOFINTERESTTYPE.POINTOFINTERESTTYPEDESCRIPTION,
                        x.SUBURB.SUBURBID,
                        x.SUBURB.SUBURBNAME
                    }).ToList();
                    if (pointofinterest == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(pointofinterest);
                    }
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }


        //READ POINTSOFINTERESTTYPES//
        [HttpPut]
        [Route("api/pointofinterest")]
        public IHttpActionResult Put([FromUri] string token)
        {
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in
            if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(6 /*Secretary*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all pointsofinterest
                    var pointofinteresttype = db.POINTOFINTERESTTYPEs.Select(x => new {
                        x.POINTOFINTERESTTYPEID,
                        x.POINTOFINTERESTTYPEDESCRIPTION,
                    }).ToList();

                    if (pointofinteresttype == null)
                        return BadRequest();
                    else
                        return Ok(pointofinteresttype);
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
        [Route("api/pointofinterest")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        { 
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

                    //Get specified point of interest
                    var pointofinterest = db.SUBURBPOINTOFINTERESTs.Where(z => z.POINTOFINTERESTID == id).Select(x => new {
                        x.POINTOFINTEREST.POINTOFINTERESTID,
                        x.POINTOFINTEREST.POINTOFINTERESTNAME,
                        x.POINTOFINTEREST.POINTOFINTERESTTYPEID,
                        x.POINTOFINTEREST.POINTOFINTERESTTYPE.POINTOFINTERESTTYPEDESCRIPTION,
                        x.SUBURB.SUBURBID,
                        x.SUBURB.SUBURBNAME
                    }).FirstOrDefault();

                    if (pointofinterest == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(pointofinterest);
                    }
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }


        //ADD//  
        [HttpPost]
        [Route("api/pointofinterest")]
        public IHttpActionResult Post([FromUri] string token, [FromUri] string name, [FromUri] int typeid, [FromUri] int suburbid)
        { 
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

                    //Null checks
                    if (string.IsNullOrEmpty(name))
                        return BadRequest();

                    //Add a point of interest
                    db.POINTOFINTERESTs.Add(new POINTOFINTEREST
                    {
                        POINTOFINTERESTNAME = name,
                        POINTOFINTERESTTYPEID = typeid

                    });

                    //Save DB changes
                    db.SaveChanges();

                    ////Find the point of interest that was just added
                    int lastid = db.POINTOFINTERESTs.OrderByDescending(item => item.POINTOFINTERESTID).FirstOrDefault().POINTOFINTERESTID;

                    ////Link the new point of interest to a suburb
                    db.SUBURBPOINTOFINTERESTs.Add(new SUBURBPOINTOFINTEREST
                    {
                        POINTOFINTERESTID = lastid,
                        SUBURBID = suburbid
                    });

                    //Save DB changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }


        //UPDATE//
        [HttpPatch]
        [Route("api/pointofinterest")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] string name, [FromUri] int typeid, [FromUri] int suburbid)
        { 
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
                    var pointofinterests = db.POINTOFINTERESTs.FirstOrDefault(x => x.POINTOFINTERESTID == id);
                    var suburbpointofinterests = db.SUBURBPOINTOFINTERESTs.FirstOrDefault(x => x.POINTOFINTERESTID == id);

                    //Null checks
                    if (string.IsNullOrEmpty(name))
                        return BadRequest();

                    //Update specified point of interest
                    pointofinterests.POINTOFINTERESTNAME = name;
                    pointofinterests.POINTOFINTERESTTYPEID = typeid;
                    suburbpointofinterests.SUBURBID = suburbid;

                    //Save DB changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
                }
                catch (System.Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }


        //DELETE//  
        [HttpDelete]
        [Route("api/pointofinterest")]
        public IHttpActionResult Delete([FromUri] string token, [FromUri] int id)
        { 
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

                    //Find point of interest
                    var pointofinterest = db.POINTOFINTERESTs.FirstOrDefault(x => x.POINTOFINTERESTID == id);
                    var suburbpointofinterests = db.SUBURBPOINTOFINTERESTs.FirstOrDefault(x => x.POINTOFINTERESTID == id);
                    if (pointofinterest == null)
                        return NotFound();

                    //Delete points of interest links to suburb in associative
                    db.SUBURBPOINTOFINTERESTs.Remove(suburbpointofinterests);

                    //Save DB Changes
                    db.SaveChanges();

                    //Delete specified point of interest
                    db.POINTOFINTERESTs.Remove(pointofinterest);

                    //Save DB Changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
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
